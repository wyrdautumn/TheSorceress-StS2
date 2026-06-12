using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class TwoWeaponFlurryPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => (object) new TwoWeaponFlurryPower.Data();
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != this.Owner || cardPlay.Card.Type != CardType.Attack)
            return Task.CompletedTask;
        this.GetInternalData<TwoWeaponFlurryPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount;
        if (cardPlay.Card.Owner.Creature != Owner || !GetInternalData<TwoWeaponFlurryPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, out amount) || amount <= 0)
            return;
        this.Flash();
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, amount, ValueProp.Unpowered, Owner, null);
    }
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
    }
}