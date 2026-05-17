using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public partial class NKalkara : NCreatureVisuals
{
	private Node2D? _visuals;
	private Sprite2D? _shadow;
	private SorceressParticles? _loseEffect;
	private SorceressParticles? _gainEffect;
	private SorceressParticles? _loseEffectSmall;
	private SorceressParticles? _gainEffectSmall;

	public override void _Ready()
	{
		base._Ready();
		_visuals = GetNodeOrNull<Node2D>("Visuals");
		_shadow = GetNodeOrNull<Sprite2D>("ShadowSprite");
		_loseEffect = GetNodeOrNull<SorceressParticles>("%LoseAdvantage");
		_gainEffect = GetNodeOrNull<SorceressParticles>("%GainAdvantage");
		_loseEffectSmall = GetNodeOrNull<SorceressParticles>("%LoseAdvantageSmall");
		_gainEffectSmall = GetNodeOrNull<SorceressParticles>("%GainAdvantageSmall");
	}
	
	public override void _Process(double delta)
	{
		if (_shadow != null)
		{
			var parentNCreature = GetParentOrNull<NCreature>();
			if (parentNCreature != null && parentNCreature.Entity != null)
			{
				Creature creature = parentNCreature.Entity;
				bool hasAdvantage = creature.HasPower<CombatAdvantagePower>();
				bool hasSmall = creature.HasPower<ShrinkPower>();
				if (hasAdvantage && !_shadow.Visible)
				{
					_shadow.Visible = true;
					if (!hasSmall)
					{
						if (_gainEffect != null) _gainEffect.Restart();
					}
					else
					{
						if (_gainEffectSmall != null) _gainEffectSmall.Restart();
					}
				}

				if (!hasAdvantage && _shadow.Visible)
				{
					_shadow.Visible = false;
					if (!hasSmall)
					{
						if (_loseEffect != null) _loseEffect.Restart();
					}
					else
					{
						if (_loseEffectSmall != null) _loseEffectSmall.Restart();
					}
				}
			}
		}
	}
}
