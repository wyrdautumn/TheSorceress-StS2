using Godot;
using MegaCrit.Sts2.Core.Nodes.RestSite;

namespace TheSorceressMod.TheSorceressModCode.helpers;

[GlobalClass]
public partial class NSorceressRestSite : NRestSiteCharacter
{
	public override void _Ready()
	{
		base._Ready();
		string str;
		switch (this.Player.RunState.CurrentActIndex)
		{
			case 0:
				str = "overgrowth_loop";
				break;
			case 1:
				str = "hive_loop";
				break;
			case 2:
				str = "glory_loop";
				break;
			default:
				throw new InvalidOperationException("Unexpected act");
		}
		string animationName = str;
		AnimatedSprite2D sprite = GetNodeOrNull<AnimatedSprite2D>("KalkaraSprite");
		if (sprite != null)
		{
			sprite.SetAnimation(str);
		}
	}
}
