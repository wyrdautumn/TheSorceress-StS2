using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.ReshuffledSpire;

public class BlankSlate() : TheSorceressModReshuffledSpireCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddReshuffledSpire(),HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromPower<VulnerablePower>(),HoverTipFactory.FromPower<FrailPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        PowerModel? weak = Owner.Creature.GetPower<WeakPower>();
        PowerModel? vulnerable = Owner.Creature.GetPower<VulnerablePower>();
        PowerModel? frail = Owner.Creature.GetPower<FrailPower>();
        if (weak != null)
        {
            await PowerCmd.Remove(weak);
            if (CombatState != null && CombatState.Encounter is QueenBoss && Owner.Character is Character.TheSorceressMod)
            {
                TalkCmd.Play(new LocString("combat_messages", "SORCERESS_QUEEN_BOSS_BLANK_SLATE_BANTER"),Owner.Creature,VfxColor.Purple,SorceressKeywords.ExtraVeryLong); 
            }
        }
        if (vulnerable != null)
            await PowerCmd.Remove(vulnerable);
        if (frail != null)
            await PowerCmd.Remove(frail);
        if (Owner.PlayerCombatState == null)
            return;
        foreach (CardModel card in Owner.PlayerCombatState.AllCards.Where(c => c.Type == CardType.Status && c.Pile?.Type != PileType.Exhaust).ToList())
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}