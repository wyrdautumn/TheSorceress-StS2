using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public partial class NShadowAttackVfx : Node2D
{
	SorceressParticles? _particles;


	public static NShadowAttackVfx? Create(Creature target)
	{
		NShadowAttackVfx shadow = PreloadManager.Cache.GetScene("res://TheSorceressMod/scenes/shadow_vfx_effect.tscn")
			.Instantiate<NShadowAttackVfx>();
		return shadow;
	}
}
