using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.ReshuffledSpire;

public class Ensorcell() : TheSorceressModReshuffledSpireCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<EnsorcellPower>(3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddReshuffledSpire(), HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery), HoverTipFactory.FromPower<CharismaPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_ENSORCELL"), 1);
        CardModel? card2 = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (c => !c.Keywords.Contains(SorceressKeywords.Sorcery)), this)).FirstOrDefault<CardModel>();
        if (card2 != null)
        {
            CardCmd.ApplyKeyword(card2, SorceressKeywords.Sorcery);
        }
        await CommonActions.ApplySelf<EnsorcellPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["EnsorcellPower"].UpgradeValueBy(2);
    }
}