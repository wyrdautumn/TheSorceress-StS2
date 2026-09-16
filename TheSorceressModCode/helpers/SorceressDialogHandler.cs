using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public class SorceressDialogHandler() : CustomSingletonModel(HookType.Combat)
{
    private bool parafrightBantered = false;
    private bool attackBantered = false;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (combatState.RoundNumber == 1 && side == CombatSide.Player)
            attackBantered = false;
        
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

    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer == null || dealer.Player == null || dealer.Player.Character is not Character.TheSorceressMod || attackBantered)
            return Task.CompletedTask;
        EncounterModel? encounter = dealer.CombatState?.Encounter;
        if (dealer.CombatState == null || encounter == null || encounter.RoomType != RoomType.Boss)
            return Task.CompletedTask;
        if (encounter is CeremonialBeastBoss)
        {
            if (result.TotalDamage >= 40)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_BEAST_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_BEAST_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is TheKinBoss)
        {
            if (result.TotalDamage >= 40)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KIN_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_KIN_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is VantomBoss)
        {
            if (result.TotalDamage >= 40)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_VANTOM_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_VANTOM_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is WaterfallGiantBoss)
        {
            if (result.TotalDamage >= 40)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25 || (double)target.CurrentHp / target.MaxHp > .5)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_WATERFALL_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_WATERFALL_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is SoulFyshBoss)
        {
            if (result.TotalDamage >= 40)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_FISH_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_FISH_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is LagavulinMatriarchBoss)
        {
            if (result.TotalDamage >= 40)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_MATRIARCH_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_MATRIARCH_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is KnowledgeDemonBoss)
        {
            if (result.TotalDamage >= 55)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_DEMON_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_DEMON_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is KaiserCrabBoss)
        {
            if (result.TotalDamage >= 55)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_CRAB_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_CRAB_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is TheInsatiableBoss)
        {
            if (result.TotalDamage >= 55)
            {
                SandpitPower? sandpitPower = target.Powers.OfType<SandpitPower>().FirstOrDefault(s => s.Target == dealer);
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25 && sandpitPower == null || (sandpitPower != null && sandpitPower.Amount > 1))
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_INSATIABLE_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_INSATIABLE_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is QueenBoss)
        {
            if (result.TotalDamage >= 70)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_QUEEN_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_QUEEN_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is TestSubjectBoss)
        {
            if (result.TotalDamage >= 70)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_SUBJECT_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_SUBJECT_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is AeonglassBoss)
        {
            if (result.TotalDamage >= 70)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_GLASS_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "SORCERESS_GLASS_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,VfxDuration.VeryLong);
                attackBantered = true;
            }
        }
        return Task.CompletedTask;
    }
}