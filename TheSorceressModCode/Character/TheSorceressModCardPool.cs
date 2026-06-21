using BaseLib.Abstracts;
using TheSorceressMod.TheSorceressModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using TheSorceressMod.TheSorceressModCode.Epochs;

namespace TheSorceressMod.TheSorceressModCode.Character;

public class TheSorceressModCardPool : CustomCardPoolModel
{
    public override string Title => TheSorceressMod.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/kalkara_big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/kalkara_text_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 0.675f; //Hue; changes the color.
    public override float S => 0.45f; //Saturation
    public override float V => 0.81f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load TheSorceressMod/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("37396d");

    public override bool IsColorless => false;
    
    protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
    {
        var list = cards.ToList();

        if (!unlockState.IsEpochRevealed<Sorceress3Epoch>())
        {
            list.RemoveAll(c => Sorceress3Epoch.Cards.Any(card => card.Id == c.Id));
        }

        if (!unlockState.IsEpochRevealed<Sorceress5Epoch>())
        {
            list.RemoveAll(c => Sorceress5Epoch.Cards.Any(card => card.Id == c.Id));
        }

        if (!unlockState.IsEpochRevealed<Sorceress7Epoch>())
        {
            list.RemoveAll(c => Sorceress7Epoch.Cards.Any(card => card.Id == c.Id));
        }

        return list;
    }
}