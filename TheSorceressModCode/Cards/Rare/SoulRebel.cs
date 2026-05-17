using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class SoulRebel() : TheSorceressModCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move),
    new CalculationBaseVar(0),
    new CalculationExtraVar(1),
    new CalculatedVar("ExhaustCount").WithMultiplier(Calc)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword( CardKeyword.Exhaust)];

    public override CardType Type => IsUpgraded ? CardType.Attack : CardType.Skill;

    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (!card.IsUpgraded)
        {
            return PileType.Discard.GetPile(card.Owner).Cards
                .Where<CardModel>((Func<CardModel, bool>)(c => c.Type == CardType.Attack)).Count();
        }
        else
        {
            return PileType.Discard.GetPile(card.Owner).Cards
                .Where<CardModel>((Func<CardModel, bool>)(c => c.Type == CardType.Skill)).Count();
        }
    }


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
        {
            return;
        }
        if (!IsUpgraded)
        {
            List<CardModel> list = PileType.Discard.GetPile(this.Owner).Cards
                .Where<CardModel>((Func<CardModel, bool>)(c => c.Type == CardType.Attack)).ToList<CardModel>();
            int cardCount = list.Count;
            foreach (CardModel card in list)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
            for (int hits = 0; hits < cardCount; hits++)
            {
                await CreatureCmd.Damage(choiceContext, play.Target, 7,
                    ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered, Owner.Creature, this);
            }
        }
        else
        {
            List<CardModel> list = PileType.Discard.GetPile(this.Owner).Cards
                .Where<CardModel>((Func<CardModel, bool>)(c => c.Type == CardType.Skill)).ToList<CardModel>();
            int cardCount = list.Count;
            foreach (CardModel card in list)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
            await CommonActions.CardAttack(this, play, cardCount,vfx:"vfx/vfx_fire_burst").Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(SorceressKeywords.Sorcery);
        
    }
}