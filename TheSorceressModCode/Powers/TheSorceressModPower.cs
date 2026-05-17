using BaseLib.Abstracts;
using BaseLib.Extensions;
using TheSorceressMod.TheSorceressModCode.Extensions;
using Godot;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public abstract class TheSorceressModPower : CustomPowerModel
{
    //Loads from TheSorceressMod/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}