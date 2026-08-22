using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

public class PrescientPlanner() : TheSorceressModHeroExpansionCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sleight),..AddHeroExpansion()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.Cunning };
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CommonActions.Draw(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
}