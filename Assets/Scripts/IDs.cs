using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public static class IDs
{
	public static readonly string[] statesNames; 				/// <summary>States' Names.</summary>

#region EventsIDs:
	public const int EVENT_NONE = 0; 							/// <summary>Non-Assigned Event's ID.</summary>
	public const int EVENT_STAGECHANGED = 1; 					/// <summary>Stage Changed's Event's ID.</summary>
	public const int EVENT_DEATHROUTINE_BEGINS = 2; 			/// <summary>Death-Routine Begins' Event's ID.</summary>
	public const int EVENT_DEATHROUTINE_ENDS = 3; 				/// <summary>Death-Routine Ends' Event's ID.</summary>
	public const int EVENT_MEDITATION_BEGINS = 4; 				/// <summary>Meditation Begins' Event's ID.</summary>
	public const int EVENT_MEDITATION_ENDS = 5; 				/// <summary>Meditation Ends' Event's ID.</summary>
	public const int EVENT_HURT = 6; 							/// <summary>Mateo's Hurt Event's ID.</summary>
	public const int EVENT_DEAD = 7; 							/// <summary>Mateo's Dead Event's ID.</summary>
	public const int EVENT_REPELLED = 8; 						/// <summary>Repelled's Event's ID.</summary>
#endregion

#region AnimationEventsIDS:
	public const int ANIMATIONEVENT_DEACTIVATEHITBOXES = 0; 	/// <summary>Deactivate Hit-Boxes' Event's ID.</summary>
	public const int ANIMATIONEVENT_ACTIVATEHITBOXES = 1; 		/// <summary>Activate Hit-Boxes' Event's ID.</summary>
	public const int ANIMATIONEVENT_WEAPON_UNSHEATH = 2; 		/// <summary>Weapon Un-Sheath's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_WEAPON_SHEATH = 3; 			/// <summary>Weapon Sheath's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_EMITSOUND_0 = 4; 			/// <summary>Emit Sound with Sub-index argument 0.</summary>
	public const int ANIMATIONEVENT_EMITSOUND_1 = 5; 			/// <summary>Emit Sound with Sub-index argument 1.</summary>
	public const int ANIMATIONEVENT_EMITSOUND_2 = 6; 			/// <summary>Emit Sound with Sub-index argument 2.</summary>
	public const int ANIMATIONEVENT_EMITPARTICLEEFFECT_0 = 7; 	/// <summary>Emit Particle-Effect with index argument 0.</summary>
	public const int ANIMATIONEVENT_EMITPARTICLEEFFECT_1 = 8; 	/// <summary>Emit Particle-Effect with index argument 1.</summary>
	public const int ANIMATIONEVENT_EMITPARTICLEEFFECT_2 = 9; 	/// <summary>Emit Particle-Effect with index argument 2.</summary>
	public const int ANIMATIONEVENT_GOIDLE = 10; 				/// <summary>Go Idle's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_JUMP = 11; 					/// <summary>Jump's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_PICKBOMB = 12; 				/// <summary>Pick Bomb's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_THROWBOMB = 13; 			/// <summary>Throw Bomb's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_PICKTNT = 14; 				/// <summary>Pick TNT's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_THROWTNT = 15; 				/// <summary>Throw TNT's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_REPELBOMB = 16; 			/// <summary>Repel Bomb's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_ATTACKEND = 17; 			/// <summary>Attack End's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_ATTACKWINDOW = 18; 			/// <summary>Weapon Un-Sheath's Animation Event's ID.</summary>
	public const int ANIMATIONEVENT_WEAPON_UNSHEATH_0 = 19; 	/// <summary>Weapon Un-Sheath's Animation Event's ID with Sub-index argument 0.</summary>
	public const int ANIMATIONEVENT_WEAPON_UNSHEATH_1 = 20; 	/// <summary>Weapon Un-Sheath's Animation Event's ID with Sub-index argument 1.</summary>
	public const int ANIMATIONEVENT_WEAPON_UNSHEATH_2 = 21; 	/// <summary>Weapon Un-Sheath's Animation Event's ID with Sub-index argument 2.</summary>
	public const int ANIMATIONEVENT_WEAPON_SHEATH_0 = 22; 		/// <summary>Weapon Sheath's Animation Event's ID with Sub-index argument 0.</summary>
	public const int ANIMATIONEVENT_WEAPON_SHEATH_1 = 23; 		/// <summary>Weapon Sheath's Animation Event's ID with Sub-index argument 1.</summary>
	public const int ANIMATIONEVENT_WEAPON_SHEATH_2 = 24; 		/// <summary>Weapon Sheath's Animation Event's ID with Sub-index argument 2.</summary>
#endregion

#region StateFlagsIDs:
	public const int STATE_DEAD = 0; 							/// <summary>Dead's State ID.</summary>
	public const int STATE_ALIVE = 1 << 0; 						/// <summary>Alive's State ID.</summary>
	public const int STATE_IDLE = 1 << 1; 						/// <summary>Idle's State ID.</summary>
	public const int STATE_MOVING = 1 << 2; 					/// <summary>Moving's State ID.</summary>
	public const int STATE_HURT = 1 << 3; 						/// <summary>Hurt's State ID.</summary>
	public const int STATE_COLLIDED = 1 << 4; 					/// <summary>Collided's State ID.</summary>
	public const int STATE_ATTACKING_0 = 1 << 5; 				/// <summary>Attacking Type 0's State ID.</summary>
	public const int STATE_ATTACKING_1 = 1 << 6; 				/// <summary>Attacking Type 1's State ID.</summary>
	public const int STATE_ATTACKING_2 = 1 << 7; 				/// <summary>Attacking Type 2's State ID.</summary>
	public const int STATE_ATTACKING_3 = 1 << 8; 				/// <summary>Attacking Type 3's State ID.</summary>
	public const int STATE_ATTACKING_4 = 1 << 9; 				/// <summary>Attacking Type 4's State ID.</summary>
	public const int STATE_VULNERABLE = 1 << 10; 				/// <summary>Vulnerable's State ID.</summary>
	public const int STATE_MEDITATING = 1 << 12; 				/// <summary>Meditating's State ID.</summary>
	public const int STATE_SWORDEQUIPPED = 1 << 14; 			/// <summary>Sword Equipped's State ID.</summary>
	public const int STATE_FIRECONJURINGCAPACITY = 1 << 15; 	/// <summary>Mire Conjuring's Capacity State's ID.</summary>
	public const int STATE_JUMPING = 1 << 16; 					/// <summary>Jumping's State ID.</summary>
	public const int STATE_CHARGINGFIRE = 1 << 17; 				/// <summary>Fire Conjuring's State ID.</summary>
	public const int STATE_CROUCHING = 1 << 18; 				/// <summary>Crouching's State ID.</summary>
	public const int STATE_BRAKING = 1 << 19; 					/// <summary>Braking's State ID.</summary>
	public const int STATE_STANDINGUP = 1 << 20; 				/// <summary>Standing Up's State ID.</summary>
	public const int STATE_TARGETONSIGHT = 1 << 21; 			/// <summary>Target On-Sight's State ID.</summary>
	public const int STATE_FOLLOWTARGET = 1 << 22; 				/// <summary>Following Target's State ID.</summary>
	public const int STATE_ATTACKWINDOW = 1 << 23; 				/// <summary>Attack-Window's State ID.</summary>
	public const int STATE_EVADE = 1 << 24; 					/// <summary>Evade's State ID.</summary>
#endregion

#region CoroutinesIDs:
	public const int COROUTINE_DEFAULT = 0; 					/// <summary>Default Coroutine's ID.</summary>
	public const int COROUTINE_ATTACK = 1; 						/// <summary>Attack Coroutine's ID.</summary>
	public const int COROUTINE_ROTATION = 2; 					/// <summary>Rotation Coroutine's ID.</summary>
#endregion

	/// <summary>Static Constructor.</summary>
	static IDs()
	{
		statesNames = new string[]
		{
			"ALIVE",
			"IDLE",
			"MOVING",
			"HURT",
			"COLLIDED",
			"ATTACKING_0",
			"ATTACKING_1",
			"ATTACKING_2",
			"ATTACKING_3",
			"ATTACKING_4",
			"VULNERABLE",
			"MEDITATING",
			"SWORDEQUIPPED",
			"FIRECONJURINGCAPACITY",
			"JUMPING",
			"CHARGINGFIRE",
			"CROUCHING",
			"BRAKING",
			"STANDINGUP",
			"TARGETONSIGHT",
			"FOLLOWTARGET",
			"ATTACKWINDOW",
			"EVADE"
		};
	}

	/// <returns>States contained on integer.</returns>
	public static string GetStates(int _state)
	{
		if(_state == 0) return "DEAD";

		return VString.GetNamedBitChain(_state, statesNames);
	}
}
}