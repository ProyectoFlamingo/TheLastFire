using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(EventsHandler))]
public class ImpactEventHandler : MonoBehaviour
{
	[SerializeField] private HitCollider2D[] _hitBoxes; 	/// <summary>HitBoxes' Array.</summary>
	private EventsHandler _eventsHandler; 					/// <summary>EventsHandler's Component.</summary>

	/// <summary>Gets and Sets hitBoxes property.</summary>
	public HitCollider2D[] hitBoxes
	{
		get { return _hitBoxes; }
		set { _hitBoxes = value; }
	}

	/// <summary>Gets eventsHandler Component.</summary>
	public EventsHandler eventsHandler
	{ 
		get
		{
			if(_eventsHandler == null) _eventsHandler = GetComponent<EventsHandler>();
			return _eventsHandler;
		}
	}

	/// <summary>ImpactEventHandler's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		SubscribeToHitCollidersEvents(true);
	}

	/// <summary>Callback invoked when ImpactEventHandler's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		SubscribeToHitCollidersEvents(false);
	}

	/// <summary>Activates HitBoxes contained within ContactWeapon.</summary>
	/// <param name="_activate">Activate? [true by default].</param>
	public virtual void ActivateHitBoxes(bool _activate = true)
	{
		if(hitBoxes != null)
		foreach(HitCollider2D hitBox in hitBoxes)
		{
			hitBox.SetTrigger(true);
			hitBox.Activate(_activate);
		}
	}

	/// <summary>Sets all HitBoxes as Trigger.</summary>
	/// <param name="_set">Set as Trigger? True by default.</param>
	public void SetHitBoxesAsTrigger(bool _set = true)
	{
		if(hitBoxes != null)
		foreach(HitCollider2D hitBox in hitBoxes)
		{
			hitBox.SetTrigger(_set);
		}
	}

	/// <summary>Subscribes to HitColliders' Events.</summary>
	/// <param name="_subscribe">Subscribe? true by default.</param>
	private void SubscribeToHitCollidersEvents(bool _subscribe = true)
	{
		if(hitBoxes == null) return;

		int i = 0;

		foreach(HitCollider2D hitBox in hitBoxes)
		{
			hitBox.detectableHitEvents = HitColliderEventTypes.EnterAndExit;

			switch(_subscribe)
			{
				case true:
				hitBox.onTriggerEvent2D += OnHitColliderTriggerEvent2D;
				hitBox.ID = i;
				break;

				case false:
				hitBox.onTriggerEvent2D -= OnHitColliderTriggerEvent2D;
				break;
			}

			i++;
		}
	}

	/// <summary>Event invoked when this Hit Collider2D hits a GameObject.</summary>
	/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public void OnHitColliderTriggerEvent2D(Collider2D _collider, HitColliderEventTypes _eventType, int _ID = 0)
	{
		GameObject obj = _collider.gameObject;
		Collider2D collider = hitBoxes[Mathf.Clamp(_ID, 0, hitBoxes.Length - 1)].collider;

/*#if UNITY_EDITOR
		VDebug.Log(
			"[ImpactEventHandler]",
			gameObject.name, 
			" Had Interaction with ",
			obj.name,
			", with tag: ",
			obj.tag,
			", Event's ID: ",
			_ID,
			"."
		);
#endif*/

		Trigger2DInformation info = new Trigger2DInformation(collider, _collider);
		eventsHandler.InvokeTriggerEvent(info, _eventType, _ID);
		return;
	}
}
}