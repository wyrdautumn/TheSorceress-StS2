using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.CrossMod;
using TheSorceressMod.TheSorceressModCode.Character;

namespace TheSorceressMod.TheSorceressModCode.Cards.Event;

[Pool(typeof(EventCardPool))]
public class Deceive : TheSorceressModHeroExpansionCard
{
    public Deceive() : base(0,
        CardType.Skill, CardRarity.Event,
        TargetType.Self)
    {
        var mysteriousFlashlightType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension");
        if (mysteriousFlashlightType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(mysteriousFlashlightType, "AddFlashlightCardForCustomCharacter");
            addMethod.Invoke(null, [this, ModelDb.Character<Character.TheSorceressMod>()]);
        }
    }
    
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheSorceressModCardPool>();
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.Cunning };
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sleight),HoverTipFactory.Static(SorceressKeywords.HeroExpansion)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_TRICK"), 1);
        CardModel? card2 = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (c => !c.Keywords.Contains(SorceressKeywords.Sleight)), this)).FirstOrDefault<CardModel>();
        if (card2 != null)
        {
            card2.EnergyCost.SetThisCombat(0);
            CardCmd.ApplyKeyword(card2, SorceressKeywords.Sleight);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(SorceressKeywords.Shadowdance);
    }
}