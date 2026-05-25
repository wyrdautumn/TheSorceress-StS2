using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class TwoWeaponCombination() : TheSorceressModCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move),
    new CalculationBaseVar(1),
    new CalculationExtraVar(1),
    new CalculatedVar("hits").WithMultiplier(Calc)];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal,CardKeyword.Exhaust];

    private static decimal Calc(CardModel card, Creature? arg2)
        => CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>(e =>
            e.CardPlay.Card is TwoWeaponCombination && e.CardPlay.Card.Owner == card.Owner);
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, (int) ((CalculatedVar) DynamicVars["hits"]).Calculate(Owner.Creature), vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}