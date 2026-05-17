using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace TheSorceressMod.TheSorceressModCode.helpers;

[GlobalClass]
public partial class SorceressParticles : NParticlesContainer
{
	public override void _Ready()
	{
		base._Ready();

		var field = typeof(NParticlesContainer).GetField("_particles",
			BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

		if (field?.GetValue(this) is Array<GpuParticles2D> { Count: > 0 })
			return;

		var list = new Array<GpuParticles2D>();
		foreach (var child in GetChildren())
		{
			if (child is GpuParticles2D particles)
				list.Add(particles);
		}
		field?.SetValue(this, list);
	}
}
  
