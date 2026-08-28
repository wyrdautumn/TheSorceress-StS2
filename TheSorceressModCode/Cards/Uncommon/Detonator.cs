using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class Detonator() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PrimedPower>(4), new PowerVar<DetonatorPower>(6)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel? card = PileType.Draw.GetPile(Owner).Cards.FirstOrDefault();
        if (card != null)
            await CardCmd.Exhaust(choiceContext, card);
        if (play.Target == null)
            return;
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        Node? child = NShivThrowVfx.Create(Owner.Creature, play.Target, new Color("b18aff"));
        NCombatRoom? instance = NCombatRoom.Instance;
        if (instance != null && child != null)
            instance.CombatVfxContainer.AddChildSafely(child);
        NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(play.Target);
        if (creatureNode != null && instance != null)
            instance.CombatVfxContainer.AddChildSafely(NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Color("6c43c7")));
        await CommonActions.Apply<PrimedPower>(choiceContext, play.Target, this);
        await CommonActions.Apply<DetonatorPower>(choiceContext, play.Target, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PrimedPower"].UpgradeValueBy(2);
        DynamicVars["DetonatorPower"].UpgradeValueBy(2);
    }
}