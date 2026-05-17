using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Ancient;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Starter;

public class EnsorcelledBlade() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy),ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10,ValueProp.Move),new EnergyVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.ForEnergy(this)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        EnsorcelledBlade source = this;
        await CommonActions.CardAttack(this, play, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<SorcerousMomentumPower>(choiceContext, source.Owner.Creature,
            source.DynamicVars.Energy.BaseValue, source.Owner.Creature, (CardModel)source);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<EldritchBlade>();
    }
}