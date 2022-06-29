using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class EmitSoundEffectEventsListener : EventsListener
{
	[SerializeField] private IntIntPair[] _IDEvents; 							/// <summary>ID Events to subscribe.</summary>
	[SerializeField] private TagIntPair[] _impactEvents; 						/// <summary>Impact Events to subscribe.</summary>
	[SerializeField] private DeactivationCauseIntPair[] _deactivationEvents; 	/// <summary>Deactivation Events to Listen.</summary>
	private Dictionary<int, int> _IDEventsDic; 									/// <summary>Dictionary of ID Events Subscribed.</summary>
	private Dictionary<GameObjectTag, int> _impactEventsDic; 					/// <summary>Dictionary of Impact Events Subscribed.</summary>
	private Dictionary<DeactivationCause, int> _deactivationEventsDic; 			/// <summary>Dictionary of Deactivation Events Subscribed.</summary>

	/// <summary>Gets IDEvents property.</summary>
	public IntIntPair[] IDEvents { get { return _IDEvents; } }

	/// <summary>Gets impactEvents property.</summary>
	public TagIntPair[] impactEvents { get { return _impactEvents; } }

	/// <summary>Gets deactivationEvents property.</summary>
	public DeactivationCauseIntPair[] deactivationEvents { get { return _deactivationEvents; } }

	/// <summary>Gets and Sets IDEventsDic property.</summary>
	public Dictionary<int, int> IDEventsDic
	{
		get { return _IDEventsDic; }
		protected set { _IDEventsDic = value; }
	}

	/// <summary>Gets and Sets impactEventsDic property.</summary>
	public Dictionary<GameObjectTag, int> impactEventsDic
	{
		get { return _impactEventsDic; }
		protected set { _impactEventsDic = value; }
	}

	/// <summary>Gets and Sets deactivationEventsDic property.</summary>
	public Dictionary<DeactivationCause, int> deactivationEventsDic
	{
		get { return _deactivationEventsDic; }
		protected set { _deactivationEventsDic = value; }
	}

	/// <summary>EmitSoundEffectEventsListener's instance initialization when loaded [Before scene loads].</summary>
	protected override void Awake()
	{
		base.Awake();

		if(IDEvents != null)
		{
			IDEventsDic = new Dictionary<int, int>();

			foreach(IntIntPair pair in IDEvents)
			{
				IDEventsDic.Add(pair.ID, pair.index);
			}
		}
		if(impactEvents != null)
		{
			impactEventsDic = new Dictionary<GameObjectTag, int>();

			foreach(TagIntPair pair in impactEvents)
			{
				impactEventsDic.Add(pair.tag, pair.index);
			}
		}
		if(deactivationEvents != null)
		{
			deactivationEventsDic = new Dictionary<DeactivationCause, int>();

			foreach(DeactivationCauseIntPair pair in deactivationEvents)
			{
				deactivationEventsDic.Add(pair.cause, pair.index);
			}
		}
	}

	/// <summary>Callback invoked when an ID event occurs.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected override void OnIDEvent(int _ID)
	{
		if(!IDEventsDic.ContainsKey(_ID)) return;

		//AudioController.PlayOneShot(SourceType.SFX, 0, IDEventsDic[_ID]);
	}

	/// <summary>Callback invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	protected override void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		string tag = _info.collider.tag;

		if(!impactEventsDic.ContainsKey(tag)) return;

		//AudioController.PlayOneShot(SourceType.SFX, 0, impactEventsDic[tag]);
	}

	/// <summary>Callback invoked when the GameObject is deactivated.</summary>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	protected override void OnDeactivated(DeactivationCause _cause, Trigger2DInformation _info)
	{
		if(!deactivationEventsDic.ContainsKey(_cause)) return;

		//AudioController.PlayOneShot(SourceType.SFX, 0, deactivationEventsDic[_cause]);
	}
}
}