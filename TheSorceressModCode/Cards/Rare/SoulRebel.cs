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

public class SoulRebel() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    private Decimal _extraDamageFromExhaust;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move), new DynamicVar("Extra", 4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword( CardKeyword.Exhaust)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
        {
            return;
        }
        List<CardModel> list = PileType.Discard.GetPile(this.Owner).Cards
            .Where<CardModel>((Func<CardModel, bool>)(c => c.Type == CardType.Attack)).ToList<CardModel>();
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_starry_impact", "blunt_attack.mp3")
            .Execute(choiceContext);
        foreach (CardModel card in list)
        {
            await CardCmd.Exhaust(choiceContext, card);
            BuffFromExhaust(DynamicVars["Extra"].BaseValue);
        }
    }
    
    private Decimal ExtraDamageFromExhaust
    {
        get => this._extraDamageFromExhaust;
        set
        {
            this.AssertMutable();
            this._extraDamageFromExhaust = value;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Extra"].UpgradeValueBy(2);
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DynamicVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.ExtraDamageFromExhaust;
    }

    public void BuffFromExhaust(Decimal extraDamage)
    {
        DynamicVar damage = this.DynamicVars.CalculationBase;
        damage.BaseValue = damage.BaseValue + extraDamage;
        this.ExtraDamageFromExhaust += extraDamage;
    }
}