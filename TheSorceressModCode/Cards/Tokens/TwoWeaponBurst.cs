using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class TwoWeaponBurst() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Token,
    TargetType.AllEnemies)
{
    private decimal _extraHitsFromPlays;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move), new DynamicVar("hits",1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery,CardKeyword.Ethereal,CardKeyword.Exhaust];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(SorceressKeywords.Rekindle)];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player == Owner && CombatManager.Instance.History.CardPlaysFinished.Any(e =>
                e.HappenedLastPlayerTurn(Owner) && e.CardPlay.Card == this) && this.Pile?.Type == PileType.Exhaust)
        {
            await CardPileCmd.Add(this, PileType.Hand.GetPile(Owner));
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
        {
            return;
        }
        await CommonActions.CardAttack(this, play, DynamicVars["hits"].IntValue, vfx: "vfx/vfx_attack_slash").BeforeDamage(() =>
            {
                foreach (Creature target in CombatState.HittableEnemies)
                {
                    NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
                    if (creatureNode != null)
                    {
                        NFireBurstVfx? child =
                            NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("b18aff"));
                        if (child == null)
                            return Task.CompletedTask;
                        SfxCmd.Play("event:/sfx/characters/attack_fire");
                        NCombatRoom? instance = NCombatRoom.Instance;
                        if (instance != null)
                            instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                    }
                }
                return Task.CompletedTask;
            })
            .WithAttackerAnim("Cast",0.2f).Execute(choiceContext);
        DynamicVars["hits"].BaseValue += 1;
        ExtraHitsFromPlays += 1;
    }
    
    private Decimal ExtraHitsFromPlays
    {
        get => _extraHitsFromPlays;
        set
        {
            AssertMutable();
            _extraHitsFromPlays = value;
        }
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DynamicVars["hits"].BaseValue += ExtraHitsFromPlays;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}