using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class FirestarterPower : TheSorceressModPower
{
    private bool _sleightPlayed = false;
    private bool _sorceryPlayed = false;
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sleight),HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery),HoverTipFactory.ForEnergy(this)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || (!cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight) && !cardPlay.Card.Keywords.Contains(SorceressKeywords.Sorcery)))
            return;
        if (cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight) && _sleightPlayed == false)
        {
            _sleightPlayed = true;
            this.Flash();
            await PowerCmd.Apply<SorcerousMomentumPower>(choiceContext, Owner, Amount, Owner, null);
        }
        if (cardPlay.Card.Keywords.Contains(SorceressKeywords.Sorcery) && _sorceryPlayed == false)
        {
            _sorceryPlayed = true;
            this.Flash();
            await PowerCmd.Apply<CunningMomentumPower>(choiceContext, Owner, Amount, Owner, null);
        }
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        _sleightPlayed = false;
        _sorceryPlayed = false;
        return Task.CompletedTask;
    }
}