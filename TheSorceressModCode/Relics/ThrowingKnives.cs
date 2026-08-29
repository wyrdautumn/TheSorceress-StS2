using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Powers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Relics;

public class ThrowingKnives() : TheSorceressModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Unpowered)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sleight)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight))
        {
            if (Owner.Creature.CombatState == null)
                return;
            Creature? target =
                Owner.RunState.Rng.CombatTargets.NextItem<Creature>(Owner.Creature.CombatState.HittableEnemies);
            if (target == null)
                return;
            NDebugAudioManager.Instance?.Play("dagger_throw.mp3");
            Node? child = NShivThrowVfx.Create(Owner.Creature, target, new Color("b18aff"));
            NCombatRoom? instance = NCombatRoom.Instance;
            if (instance != null && child != null)
                instance.CombatVfxContainer.AddChildSafely(child);
            await CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage, Owner.Creature);
        }
    }
}