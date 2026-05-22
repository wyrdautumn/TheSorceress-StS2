using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
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

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class SorcerousPulse() : TheSorceressModCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharismaPower>(3), new PowerVar<SorcerousMomentumPower>(2),
    new DynamicVar("Exhaust",1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust),HoverTipFactory.FromPower<CharismaPower>(),HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery),HoverTipFactory.ForEnergy(this)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<CharismaPower>(choiceContext, this);
        if (!IsUpgraded)
        {
            foreach (CardModel card in PileType.Discard.GetPile(this.Owner).Cards.ToList<CardModel>()
                         .StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).Take<CardModel>(1))
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
        }
        else
        {
            var prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
            var card = (await CardSelectCmd.FromSimpleGrid(
                choiceContext,
                PileType.Discard.GetPile(Owner).Cards,
                Owner,
                prefs)).FirstOrDefault();
            if (card != null)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
        }
        await CommonActions.ApplySelf<SorcerousMomentumPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        
    }
}