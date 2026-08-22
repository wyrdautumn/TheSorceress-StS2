using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.CrossMod;
using TheSorceressMod.TheSorceressModCode.Character;

namespace TheSorceressMod.TheSorceressModCode.Cards.Event;

[Pool(typeof(EventCardPool))]
public class TheObsidianBlade : TheSorceressModHeroExpansionCard
{
    public TheObsidianBlade() : base(3,
    CardType.Attack, CardRarity.Event,
    TargetType.AnyEnemy)
    {
        var brokenBladeType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomBrokenBladeExtension");
        if (brokenBladeType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(brokenBladeType, "AddBladeCardForCustomCharacter");
            addMethod.Invoke(null, [this, ModelDb.Character<Character.TheSorceressMod>()]);
        }
    }
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(24, ValueProp.Move)];
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheSorceressModCardPool>();
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Subtle, SorceressKeywords.Shadowdance];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery),..AddHeroExpansion()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_dramatic_stab").Execute(choiceContext);
        if (RunState == null || CombatState == null)
            return;
        var card = PileType.Deck.GetPile(Owner).Cards.Where(c => c.Keywords.Contains(SorceressKeywords.Sorcery)).TakeRandom(1, RunState.Rng.CombatCardSelection).FirstOrDefault();
        if (card == null)
        {
            return;
        }
        var clone = CombatState?.CloneCard(card);
        if (clone != null)
        {
            clone.DeckVersion = card;
            await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, Owner);
            await CardCmd.AutoPlay(choiceContext, clone, null);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8);
    }
}