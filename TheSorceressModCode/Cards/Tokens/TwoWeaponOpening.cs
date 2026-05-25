using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class TwoWeaponOpening() : TheSorceressModCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move), new PowerVar<VulnerablePower>(1)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal,CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<TwoWeaponLink>(IsUpgraded),HoverTipFactory.FromPower<VulnerablePower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.Apply<VulnerablePower>(choiceContext, this, play);
        if (CombatState == null)
            return;
        CardModel link = CombatState.CreateCard<TwoWeaponLink>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(link, PileType.Hand, Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(link);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
        AddKeyword(CardKeyword.Retain);
    }
}