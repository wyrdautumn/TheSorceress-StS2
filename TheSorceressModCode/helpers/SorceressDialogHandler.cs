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
            combatState.Encounter is not WaterfallGiantBoss && combatState.Encounter is not TheInsatiableBoss &&
            combatState.Encounter is not QueenBoss && combatState.Encounter is not TheKinBoss &&
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
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_CULTIST_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_CULTISTS_REPLY"), combatState.Enemies.First(),
                VfxColor.Blue,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is ByrdonisElite)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_BYRDONIS_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_BYRDONIS_REPLY"), combatState.Enemies.First(),
                VfxColor.Green,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is DevotedSculptorWeak)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_SCULPTOR_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is OwlMagistrateNormal)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_OWL_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is TheObscuraNormal)
        {
            Creature? parafright = combatState.Enemies.FirstOrDefault(creature => creature.Monster is Parafright);
            if (parafright != null && parafright.IsHittable)
            {
                TalkCmd.Play(new LocString("combat_messages", "SORCERESS_OBSCURA_BANTER"), sorceress.Creature,
                    VfxColor.Purple, SorceressKeywords.ExtraVeryLong);
                parafrightBantered = true;
            }
            else return;
        }

        if (combatState.Encounter is WaterfallGiantBoss)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_WATERFALL_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is TheInsatiableBoss)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_INSATIABLE_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is QueenBoss)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_QUEEN_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_QUEEN_REPLY"),combatState.Enemies.First(creature => creature.Monster is Queen),VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is TheKinBoss)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KIN_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);

        }

        if (combatState.Encounter is TestSubjectBoss)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_TEST_SUBJECT_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is AeonglassBoss)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_AEONGLASS_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is DenseVegetationEventEncounter)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_WRIGGLER_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is MysteriousKnightEventEncounter)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KEY_FIGHT_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KEY_FIGHT_REPLY"),combatState.Enemies.First(creature => creature.Monster is MysteriousKnight),VfxColor.Black,SorceressKeywords.ExtraVeryLong);
        }

        if (combatState.Encounter is PunchOffEventEncounter)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_PUNCHERS_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
            await Cmd.CustomScaledWait(0.4f, 1);
            foreach (Creature enemy in combatState.Enemies)
            {
                TalkCmd.Play(new LocString("combat_messages", "SORCERESS_PUNCHERS_REPLY"),
                    enemy, VfxColor.Cyan, SorceressKeywords.ExtraVeryLong);
            }
        }

        if (combatState.Encounter is FakeMerchantEventEncounter)
        {
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_FAKE_MERCHANT_BANTER"),sorceress.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong);
            await Cmd.CustomScaledWait(0.4f, 1);
            TalkCmd.Play(new LocString("combat_messages", "SORCERESS_FAKE_MERCHANT_REPLY"),combatState.Enemies.First(creature => creature.Monster is FakeMerchantMonster),VfxColor.Blue,SorceressKeywords.ExtraVeryLong);
        }
    }
}