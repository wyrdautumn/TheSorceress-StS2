using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class SorcerousPulse() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new PowerVar<SorcerousMomentumPower>(1),
    new DynamicVar("Exhaust",1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust),HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel sorcerousPulse = this;
        if (IsUpgraded)
        {
            await CommonActions.Draw(this, choiceContext);
        }
        foreach (CardModel card in PileType.Discard.GetPile(sorcerousPulse.Owner).Cards.ToList<CardModel>()
                     .StableShuffle<CardModel>(sorcerousPulse.Owner.RunState.Rng.Shuffle).Take<CardModel>(1))
        {
            await CardCmd.Exhaust(choiceContext, card);
        }

        await CommonActions.ApplySelf<SorcerousMomentumPower>(choiceContext, sorcerousPulse);
    }

    protected override void OnUpgrade()
    {

    }
}