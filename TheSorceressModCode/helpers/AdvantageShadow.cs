using Godot;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public partial class AdvantageShadow : Node2D
{
	private Sprite2D? _sprite;
	public override void _Ready()
	{
		Visible = false;
		_sprite = GetNodeOrNull<Sprite2D>("ShadowSprite");
	}

	public void CheckShadow(bool hasAdvantage)
	{
		if (Visible != hasAdvantage)
			Visible = hasAdvantage;
	}
}
