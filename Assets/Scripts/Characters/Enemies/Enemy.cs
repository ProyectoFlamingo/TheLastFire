using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(VCameraTarget))]
public class Enemy : Character
{
	[Space(5f)]
	[SerializeField] private FXsScheduler _deadFXs; 	/// <summary>Dead's FXs emitted.</summary>

	/// <summary>Gets and Sets deadFXs property.</summary>
	public FXsScheduler deadFXs
	{
		get { return _deadFXs; }
		protected set { _deadFXs = value; }
	}

	/// <summary>Draws Gizmos [On Editor Mode].</summary>
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		deadFXs.DrawGizmos();
	}
}
}