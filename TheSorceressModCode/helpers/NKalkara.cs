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
	private SorceressParticles? _loseEffectFlipped;
	private SorceressParticles? _gainEffectFlipped;

	public override void _Ready()
	{
		base._Ready();
		_visuals = GetNodeOrNull<Node2D>("%Visuals");
		_shadow = GetNodeOrNull<Sprite2D>("%Visuals/ShadowSprite");
		_loseEffect = GetNodeOrNull<SorceressParticles>("%Visuals/%LoseAdvantage");
		_gainEffect = GetNodeOrNull<SorceressParticles>("%Visuals/%GainAdvantage");
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
				if (hasAdvantage && !_shadow.Visible)
				{
					_shadow.Visible = true;
					if (_gainEffect != null) _gainEffect.Restart();
				}
				if (!hasAdvantage && _shadow.Visible)
				{
					_shadow.Visible = false;
					if (_loseEffect != null) _loseEffect.Restart();
				}
			}
		}
	}
}
