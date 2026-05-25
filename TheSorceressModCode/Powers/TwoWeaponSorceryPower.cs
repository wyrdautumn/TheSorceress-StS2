using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class TwoWeaponSorceryPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<TwoWeaponBurst>(false)];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (Owner.Player == null)
            return;
        CardModel burst = CombatState.CreateCard<TwoWeaponBurst>(Owner.Player);
        await CardPileCmd.AddGeneratedCardToCombat(burst, PileType.Hand, Owner.Player);
        await PowerCmd.Decrement(this);
    }
}