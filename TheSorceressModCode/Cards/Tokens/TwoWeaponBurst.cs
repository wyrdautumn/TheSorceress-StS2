using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class TwoWeaponBurst() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Token,
    TargetType.AllEnemies)
{
    private bool _returnToHand = false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move), new DynamicVar("hits",1)];
    public override int MaxUpgradeLevel => 999;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery,CardKeyword.Ethereal,CardKeyword.Exhaust];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }

    public override Task BeforeCombatStart()
    {
        _returnToHand = false;
        return Task.CompletedTask;
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (_returnToHand)
        {
            await CardPileCmd.Add(this, PileType.Hand.GetPile(Owner));
            CardCmd.Upgrade(this);
            _returnToHand = false;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        _returnToHand = true;
        await CommonActions.CardAttack(this, play, DynamicVars["hits"].IntValue, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["hits"].UpgradeValueBy(1);
    }
}