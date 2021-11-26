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
	public const int ID_STATE_DEAD = 0; 						/// <summary>Dead's State Flag.</summary>
	public const int ID_STATE_ALIVE = 1 << 0; 					/// <summary>Alive's State Flag.</summary>
	public const int ID_STATE_IDLE = 1 << 1; 					/// <summary>Idle's State Flag.</summary>
	public const int ID_STATE_PLAYERONSIGHT = 1 << 2; 			/// <summary>Player On Sight's State Flag.</summary>
	public const int ID_STATE_FOLLOWPLAYER = 1 << 3; 			/// <summary>Follow Player's State Flag.</summary>
	public const int ID_STATE_ATTACK = 1 << 4; 					/// <summary>Attack's State Flag.</summary>
	public const int ID_STATE_VULNERABLE = 1 << 5; 				/// <summary>Vulnerable's State Flag (it means the enemy is available to be attacked).</summary>
	public const int ID_STATE_HURT = 1 << 6; 					/// <summary>Hurt's State Flag.</summary>

	/// <returns>States to string</returns>
	public override string StatesToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine(base.StatesToString());

		builder.AppendLine("State Mask:");
		builder.Append("Alive: ");
		builder.AppendLine(this.HasState(ID_STATE_ALIVE).ToString());
		builder.Append("Idle: ");
		builder.AppendLine(this.HasState(ID_STATE_IDLE).ToString());
		builder.Append("PlayerOnSight: ");
		builder.AppendLine(this.HasState(ID_STATE_PLAYERONSIGHT).ToString());
		builder.Append("FollowPlayer: ");
		builder.AppendLine(this.HasState(ID_STATE_FOLLOWPLAYER).ToString());
		builder.Append("Attack: ");
		builder.AppendLine(this.HasState(ID_STATE_ATTACK).ToString());
		builder.Append("Vulnerable: ");
		builder.Append(this.HasState(ID_STATE_VULNERABLE).ToString());
		builder.Append("Hurt: ");
		builder.Append(this.HasState(ID_STATE_HURT).ToString());

		return builder.ToString();
	}
}
}