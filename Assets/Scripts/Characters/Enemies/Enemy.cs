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