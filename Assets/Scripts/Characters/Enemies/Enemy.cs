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

	/// <returns>States to string</returns>
	public override string StatesToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine(base.StatesToString());

		builder.AppendLine("State Mask:");
		builder.Append("Alive: ");
		builder.AppendLine(this.HasState(IDs.STATE_ALIVE).ToString());
		builder.Append("Idle: ");
		builder.AppendLine(this.HasState(IDs.STATE_IDLE).ToString());
		builder.Append("PlayerOnSight: ");
		builder.AppendLine(this.HasState(IDs.STATE_TARGETONSIGHT).ToString());
		builder.Append("FollowPlayer: ");
		builder.AppendLine(this.HasState(IDs.STATE_FOLLOWTARGET).ToString());
		builder.Append("Attack: ");
		builder.AppendLine(this.HasState(IDs.STATE_ATTACKING).ToString());
		builder.Append("Vulnerable: ");
		builder.Append(this.HasState(IDs.STATE_VULNERABLE).ToString());
		builder.Append("Hurt: ");
		builder.Append(this.HasState(IDs.STATE_HURT).ToString());

		return builder.ToString();
	}
}
}