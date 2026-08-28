using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class TwoWeaponFlurryUpgradePower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => new Data();
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != this.Owner || !cardPlay.Card.Tags.Contains(SorceressKeywords.TwoWeapon))
            return Task.CompletedTask;
        GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount;
        if (cardPlay.Card.Owner.Creature != Owner || !GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out amount) || amount <= 0)
            return;
        Flash();
        foreach (Creature enemy in CombatState.HittableEnemies)
            VfxCmd.PlayOnCreatureCenter(enemy, "vfx/vfx_attack_slash");
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, amount, ValueProp.Unpowered, Owner, null, null);
    }
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
    }
}