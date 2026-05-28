using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class FlameInTheDark() : TheSorceressModCard(3,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    private const string _increaseKey = "Increase";
    private const int _baseCharisma = 2;
    private int _currentCharisma = 2;
    private int _increasedCharisma;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20, ValueProp.Move), new PowerVar<CharismaPower>(this.CurrentCharisma),new IntVar("Increase", 2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery,CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CharismaPower>(),HoverTipFactory.Static(StaticHoverTip.Fatal)];
    
    [SavedProperty]
    public int CurrentCharisma
    {
        get => this._currentCharisma;
        set
        {
            this.AssertMutable();
            this._currentCharisma = value;
            this.DynamicVars["CharismaPower"].BaseValue = (Decimal) this._currentCharisma;
        }
    }

    [SavedProperty]
    public int IncreasedCharisma
    {
        get => this._increasedCharisma;
        set
        {
            this.AssertMutable();
            this._increasedCharisma = value;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null || NCombatRoom.Instance == null || NCombatRoom.Instance.GetCreatureNode(play.Target) == null)
        {
            return;
        }
        bool shouldTriggerFatal = play.Target.Powers.All<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldOwnerDeathTriggerFatal()));
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).Targeting(play.Target).BeforeDamage(() =>
        {
            NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(play.Target);
            if (creatureNode != null)
            {
                NFireBurstVfx? child = NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("b18aff"));
                if (child == null)
                    return Task.CompletedTask;
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                NCombatRoom? instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
            }
            return Task.CompletedTask;
        }).Execute(choiceContext);
        await PowerCmd.Apply<CharismaPower>(choiceContext, Owner.Creature, DynamicVars["CharismaPower"].BaseValue, Owner.Creature, this);
        if (!shouldTriggerFatal || !attackCommand.Results
                .SelectMany<List<DamageResult>, DamageResult>(
                    (Func<List<DamageResult>, IEnumerable<DamageResult>>)(r => (IEnumerable<DamageResult>)r))
                .Any<DamageResult>((Func<DamageResult, bool>)(r => r.WasTargetKilled)))
        {
            return;
        }
        this.BuffFromKill();
        if (!(this.DeckVersion is FlameInTheDark deckVersion))
            return;
        deckVersion.BuffFromKill();
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(10);
    }
    
    protected override void AfterDowngraded() => this.UpdateCharisma();

    private void BuffFromKill()
    {
        this.IncreasedCharisma += this.DynamicVars["Increase"].IntValue;
        UpdateCharisma();
    }

    private void UpdateCharisma() => this.CurrentCharisma = 2 + this.IncreasedCharisma;
}