using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class PrimeTheFire() : TheSorceressModCard(4,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(26, ValueProp.Move), new PowerVar<PrimedPower>(6)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.Fire };
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
        {
            return;
        }
        await CommonActions.CardAttack(this, play).BeforeDamage(() =>
            {
                foreach (Creature target in CombatState.HittableEnemies)
                {
                    NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
                    if (creatureNode != null)
                    {
                        NFireBurningVfx? child = NFireBurningVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, true, new Color("b18aff"));
                        if (child == null)
                            return Task.CompletedTask;
                        SfxCmd.Play("event:/sfx/characters/attack_fire");
                        NCombatRoom? instance = NCombatRoom.Instance;
                        if (instance != null)
                            instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                    }
                }
                return Task.CompletedTask;
            }
            ).WithAttackerAnim("Cast",0.2f).Execute(choiceContext);
        await PowerCmd.Apply<PrimedPower>(choiceContext, CombatState.HittableEnemies,
            DynamicVars["PrimedPower"].BaseValue, Owner.Creature, this);
        PrimeTheFirePower? power = await PowerCmd.Apply<PrimeTheFirePower>(choiceContext, Owner.Creature, DynamicVars.Damage.PreviewValue, Owner.Creature, this);
        if (power != null)
        {
            power.DynamicVars["PrimedPower"].BaseValue += DynamicVars["PrimedPower"].BaseValue;
            power.InvokeSecondAmountChanged();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars["PrimedPower"].UpgradeValueBy(4);
    }
}