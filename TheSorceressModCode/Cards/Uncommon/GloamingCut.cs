using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class GloamingCut() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move), new PowerVar<GloamingCutPower>(4)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, SorceressKeywords.Shadowdance];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        List<CardModel>? exhaust = CardPile.Get(PileType.Exhaust, Owner)
            ?.Cards
            .Where(c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Exhaust) && !c.Keywords.Contains(SorceressKeywords.Shadowdance)).ToList();
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_DANCE"), 1);
        if (exhaust == null)
            return;
        CardModel? card = (await CardSelectCmd.FromSimpleGrid(choiceContext, exhaust, Owner, prefs)).FirstOrDefault();
        if (card != null)
            CardCmd.ApplyKeyword(card, SorceressKeywords.Shadowdance);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3);
    }
}