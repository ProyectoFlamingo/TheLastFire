using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
public static class IDs
{
	public const int STATE_DEAD = 0; 				/// <summary>Dead's State ID.</summary>
	public const int STATE_ALIVE = 1 << 0; 			/// <summary>Alive's State ID.</summary>
	public const int STATE_IDLE = 1 << 1; 			/// <summary>Idle's State ID.</summary>
	public const int STATE_HURT = 1 << 2; 			/// <summary>Hurt's State ID.</summary>
	public const int STATE_COLLIDED = 1 << 3; 		/// <summary>Collided's State ID.</summary>
	public const int STATE_ATTACKING = 1 << 4; 		/// <summary>Attacking's State ID.</summary>
	public const int STATE_VULNERABLE = 1 << 5; 	/// <summary>Vulnerable's State ID.</summary>
}
}