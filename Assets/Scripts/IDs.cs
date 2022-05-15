using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
public static class IDs
{
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
	public const int ANIMATIONEVENT_ATTACKWINDOW = 18; 			/// <summary>Attack Window's Animation Event's ID.</summary>
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

	/// <returns>States contained on integer.</returns>
	public static string GetStates(int _state)
	{
		if(_state == 0) return "DEAD";

		StringBuilder builder = new StringBuilder();

		builder.Append("{ ");

		if((_state | STATE_ALIVE) == _state) builder.Append("ALIVE, ");
		else builder.Append("DEAD [ALIVE is not turned on], ");
		if((_state | STATE_IDLE) == _state) builder.Append("IDLE, ");
		if((_state | STATE_MOVING) == _state) builder.Append("MOVING, ");
		if((_state | STATE_HURT) == _state) builder.Append("HURT, ");
		if((_state | STATE_COLLIDED) == _state) builder.Append("COLLIDED, ");
		if((_state | STATE_ATTACKING_0) == _state) builder.Append("ATTACKING_0, ");
		if((_state | STATE_ATTACKING_1) == _state) builder.Append("ATTACKING_1, ");
		if((_state | STATE_ATTACKING_2) == _state) builder.Append("ATTACKING_2, ");
		if((_state | STATE_ATTACKING_3) == _state) builder.Append("ATTACKING_3, ");
		if((_state | STATE_ATTACKING_4) == _state) builder.Append("ATTACKING_4, ");
		if((_state | STATE_VULNERABLE) == _state) builder.Append("VULNERABLE, ");
		if((_state | STATE_MEDITATING) == _state) builder.Append("MEDITATING, ");
		if((_state | STATE_SWORDEQUIPPED) == _state) builder.Append("SWORDEQUIPPED, ");
		if((_state | STATE_FIRECONJURINGCAPACITY) == _state) builder.Append("FIRECONJURINGCAPACITY, ");
		if((_state | STATE_JUMPING) == _state) builder.Append("JUMPING, ");
		if((_state | STATE_CHARGINGFIRE) == _state) builder.Append("CHARGINGFIRE, ");
		if((_state | STATE_CROUCHING) == _state) builder.Append("CROUCHING, ");
		if((_state | STATE_BRAKING) == _state) builder.Append("BRAKING, ");
		if((_state | STATE_STANDINGUP) == _state) builder.Append("STANDINGUP, ");
		if((_state | STATE_TARGETONSIGHT) == _state) builder.Append("TARGETONSIGHT, ");
		if((_state | STATE_FOLLOWTARGET) == _state) builder.Append("FOLLOWTARGET, ");
		if((_state | STATE_ATTACKWINDOW) == _state) builder.Append("ATTACKWINDOW, ");
		if((_state | STATE_EVADE) == _state) builder.Append("EVADE, ");

		builder.Remove(builder.Length - 2, 2); 	/// Remove ", "
		builder.Append(" }");

		return builder.ToString();
	}
}
}