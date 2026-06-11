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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class ChaosBolt() : TheSorceressModCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9,ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered),
    new PowerVar<PrimedPower>(3), new DynamicVar("Debuff",3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>(),HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromPower<VulnerablePower>()];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
        {
            return;
        }
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        int num1 = DynamicVars["Debuff"].IntValue;
        NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(play.Target);
        if (creatureNode != null && NCombatRoom.Instance != null)
        {
            NLargeMagicMissileVfx? child = NLargeMagicMissileVfx.Create(creatureNode.GetBottomOfHitbox(),
                new Color("b18aff"));
            if (child != null)
            {
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(child);
                await Cmd.Wait(child.WaitTime);
            }
        }
        VfxCmd.PlayOnCreature(play.Target, "vfx/vfx_attack_blunt");
        SfxCmd.Play("blunt_attack.mp3");
        await CreatureCmd.Damage(choiceContext, play.Target, this.DynamicVars.Damage, (CardModel) this);
        await CommonActions.Apply<PrimedPower>(choiceContext, play.Target, this);
        for (int loops = num1; loops > 0;)
        {
            --loops;
            if (play.Card.Owner.RunState.Rng.Niche.NextBool())
            {
                await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, 1, this.Owner.Creature, this);
            }
            else
            {
                await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, 1, this.Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PrimedPower"].UpgradeValueBy(2);
        DynamicVars["Debuff"].UpgradeValueBy(2);
    }
}