using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class HellfireSoul() : TheSorceressModCard(5,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20, ValueProp.Move), new DynamicVar("Exhaust", 3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? source)
    {
        if (card.Owner.PlayerCombatState != null)
        {
            SetCost();
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.PlayerCombatState != null)
        {
            SetCost();
        }
        return Task.CompletedTask;
    }

    public override Task AfterPotionUsed(PotionModel potion, Creature? target)
    {
        if (potion.Owner.PlayerCombatState != null)
        {
            SetCost();
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner.PlayerCombatState != null)
        {
            SetCost();
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.PlayerCombatState != null)
        {
            SetCost();
        }
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.PlayerCombatState != null)
        {
            SetCost();
        }
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        EnergyCost.SetThisCombat(this.EnergyCost.Canonical);
        return Task.CompletedTask;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        HellfireSoul hellfireSoul = this;
        foreach (CardModel card in PileType.Discard.GetPile(hellfireSoul.Owner).Cards.ToList<CardModel>()
                     .StableShuffle<CardModel>(hellfireSoul.Owner.RunState.Rng.Shuffle).Take<CardModel>(hellfireSoul
                         .DynamicVars
                             ["Exhaust"].IntValue))
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Exhaust"].UpgradeValueBy(2);
    }

    private void SetCost()
    {
        int discard = PileType.Discard.GetPile(Owner).Cards.Count;
        int exhaust = DynamicVars["Exhaust"].IntValue;
        int reduceCost;
        if (discard > exhaust)
        {
            reduceCost = exhaust;
        }
        else
        {
            reduceCost = discard;
        }

        this.EnergyCost.SetThisCombat(this.EnergyCost.Canonical - reduceCost);
    }
}