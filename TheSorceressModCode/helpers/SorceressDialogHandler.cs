using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public class SorceressDialogHandler() : CustomSingletonModel(HookType.Combat)
{
    private bool parafrightBantered = false;
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != CombatSide.Player)
            return;

        Player? sorceress = combatState.Players.FirstOrDefault(p => p.Character is Character.TheSorceressMod);
        if (sorceress == null)
            return;

        if (combatState.Encounter is not CultistsNormal && combatState.Encounter is not ByrdonisElite &&
            combatState.Encounter is not DevotedSculptorWeak &&
            combatState.Encounter is not OwlMagistrateNormal && combatState.Encounter is not TheObscuraNormal &&
            combatState.Encounter is not TheKinBoss &&
            combatState.Encounter is not WaterfallGiantBoss && combatState.Encounter is not TheInsatiableBoss &&
            combatState.Encounter is not QueenBoss &&
            combatState.Encounter is not TestSubjectBoss && combatState.Encounter is not AeonglassBoss &&
            combatState.Encounter is not DenseVegetationEventEncounter
            && combatState.Encounter is not MysteriousKnightEventEncounter &&
            combatState.Encounter is not PunchOffEventEncounter && combatState.Encounter is not FakeMerchantEventEncounter)
            return;

        if (combatState.Encounter is not TheObscuraNormal && combatState.RoundNumber > 1)
            return;

        if (combatState.Encounter is TheObscuraNormal && combatState.RoundNumber == 1)
        {
            parafrightBantered = false;
            return;
        }
        
        if (combatState.Encounter is TheObscuraNormal && parafrightBantered)
            return;

        if (combatState.Encounter is CultistsNormal)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_CULTIST_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_CULTISTS_REPLY"), combatState.Enemies.First(),
                VfxColor.Blue,VfxDuration.Standard);
        }

        if (combatState.Encounter is ByrdonisElite)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_BYRDONIS_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_BYRDONIS_REPLY"), combatState.Enemies.First(),
                VfxColor.Green,VfxDuration.Standard);
        }

        if (combatState.Encounter is DevotedSculptorWeak)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_SCULPTOR_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is OwlMagistrateNormal)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_OWL_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is TheObscuraNormal)
        {
            Creature? parafright = combatState.Enemies.FirstOrDefault(creature => creature.Monster is Parafright);
            if (parafright != null && parafright.IsHittable)
            {
                await Cmd.CustomScaledWait(1.1f, 0.4f);
                TalkCmd.Play(new LocString("combat_messages", "SORCERESS_OBSCURA_BANTER"), sorceress.Creature,
                    VfxColor.Purple, VfxDuration.Standard);
                parafrightBantered = true;
            }
            else return;
        }

        if (combatState.Encounter is TheKinBoss)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KIN_BANTER1"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
            await Cmd.CustomScaledWait(1f, 1.5f);
            foreach (Creature enemy in combatState.Enemies.Where(creature => creature.Monster is KinFollower))
            {
                TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KIN_REPLY"),
                    enemy, VfxColor.Blue, VfxDuration.Standard);
            }
            await Cmd.CustomScaledWait(1f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KIN_BANTER2"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is WaterfallGiantBoss)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_WATERFALL_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is TheInsatiableBoss)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_INSATIABLE_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is QueenBoss)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_QUEEN_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_QUEEN_REPLY"),combatState.Enemies.First(creature => creature.Monster is Queen),VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is TestSubjectBoss)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_TEST_SUBJECT_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is AeonglassBoss)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_AEONGLASS_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is DenseVegetationEventEncounter)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_WRIGGLER_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
        }

        if (combatState.Encounter is MysteriousKnightEventEncounter)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KEY_FIGHT_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KEY_FIGHT_REPLY"),combatState.Enemies.First(creature => creature.Monster is MysteriousKnight),VfxColor.Black,VfxDuration.Standard);
        }

        if (combatState.Encounter is PunchOffEventEncounter)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_PUNCHERS_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
            foreach (Creature enemy in combatState.Enemies)
            {
                TalkCmd.Play(new LocString("combat_messages", "SORCERESS_PUNCHERS_REPLY"),
                    enemy, VfxColor.Cyan, VfxDuration.Standard);
            }
        }

        if (combatState.Encounter is FakeMerchantEventEncounter)
        {
            await Cmd.CustomScaledWait(3f, 1.5f);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_FAKE_MERCHANT_BANTER"),sorceress.Creature,VfxColor.Purple,VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_FAKE_MERCHANT_REPLY"),combatState.Enemies.First(creature => creature.Monster is FakeMerchantMonster),VfxColor.Blue,VfxDuration.Standard);
        }
    }
}