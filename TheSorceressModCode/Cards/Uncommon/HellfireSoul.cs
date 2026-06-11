using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class HellfireSoul() : TheSorceressModCard(5,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20, ValueProp.Move), new DynamicVar("Exhaust", 3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.Fire };
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card != this || originalCost <= 0)
            return false;
        int discard = PileType.Discard.GetPile(Owner).Cards.Count;
        int exhaust = DynamicVars["Exhaust"].IntValue;
        int reduceCost;
        if (discard > exhaust)
        {
            reduceCost = exhaust;
        }
        else
        {
            reduceCost = discard;
        }
        modifiedCost = originalCost - reduceCost;
        if (modifiedCost < 0M)
            modifiedCost = 0M;
        return true;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
        {
            return;
        }
        float scale = 0.8f;
        await CommonActions.CardAttack(this, play).BeforeDamage(() =>
        {
            foreach (Creature target in CombatState.HittableEnemies)
            {
                NGroundFireVfx? child = NGroundFireVfx.Create(target, VfxColor.Purple);
                if (child == null)
                    return Task.CompletedTask;
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                child.Scale = Vector2.One * scale;
                NCombatRoom? instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                scale += 0.1f;
            }
            return Task.CompletedTask;
        }).WithAttackerAnim("Cast",0.2f).Execute(choiceContext);
        HellfireSoul hellfireSoul = this;
        foreach (CardModel card in PileType.Discard.GetPile(hellfireSoul.Owner).Cards.ToList<CardModel>()
                     .StableShuffle<CardModel>(hellfireSoul.Owner.RunState.Rng.Shuffle).Take<CardModel>(hellfireSoul
                         .DynamicVars
                             ["Exhaust"].IntValue))
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Exhaust"].UpgradeValueBy(2);
    }
    }