using BaseLib.Abstracts;
using TheSorceressMod.TheSorceressModCode.Extensions;
using Godot;

namespace TheSorceressMod.TheSorceressModCode.Character;

public class TheSorceressModRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheSorceressMod.Color;

    public override string BigEnergyIconPath => "charui/kalkara_big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/kalkara_text_energy.png".ImagePath();
}