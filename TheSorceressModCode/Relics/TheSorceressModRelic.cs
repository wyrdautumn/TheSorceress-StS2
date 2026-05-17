using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheSorceressMod.TheSorceressModCode.Character;
using TheSorceressMod.TheSorceressModCode.Extensions;
using Godot;

namespace TheSorceressMod.TheSorceressModCode.Relics;

[Pool(typeof(TheSorceressModRelicPool))]
public abstract class TheSorceressModRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}