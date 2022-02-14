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
	private const float DEFAULT_OFFSET_Z = 1.0f; 							/// <summary>Default  Offset for the Z-Axis.</summary>

	[SerializeField] private bool _keepEvaluatingFarColliders; 				/// <summary>Keep Evaluating for far objects that entered trigger?.</summary>
	[SerializeField] private HitCollider2D[] _hitBoxes; 					/// <summary>HitBoxes' Array.</summary>
	[SerializeField] private List<HitCollider2D> _externalHitBoxes; 		/// <summary>Additional Hit-Boxes, external to the EventsHandler [e.g., Weapon].</summary>
	[SerializeField] private float _zOffsetTolerance; 						/// <summary>Z-Offset's Tolerance.</summary>
	private EventsHandler _eventsHandler; 									/// <summary>EventsHandler's Component.</summary>
	private Dictionary<int, VTuple<Collider2D, Collider2D>> _tuples; 		/// <summary>Collider2Ds' Tuples.</summary>
	private Coroutine zEvaluation; 											/// <summary>Z-Axis' Evaluation Coroutine's reference.</summary>

	/// <summary>Gets and Sets keepEvaluatingFarColliders property.</summary>
	public bool keepEvaluatingFarColliders
	{
		get { return _keepEvaluatingFarColliders; }
		set { _keepEvaluatingFarColliders = value; }
	}

	/// <summary>Gets and Sets hitBoxes property.</summary>
	public HitCollider2D[] hitBoxes
	{
		get { return _hitBoxes; }
		set { _hitBoxes = value; }
	}

	/// <summary>Gets and Sets externalHitBoxes property.</summary>
	public List<HitCollider2D> externalHitBoxes
	{
		get { return _externalHitBoxes; }
		set { _externalHitBoxes = value; }
	}

	/// <summary>Gets and Sets zOffsetTolerance property.</summary>
	public float zOffsetTolerance
	{
		get { return _zOffsetTolerance; }
		set { _zOffsetTolerance = value; }
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

	/// <summary>Gets and Sets tuples property.</summary>
	public Dictionary<int, VTuple<Collider2D, Collider2D>> tuples
	{
		get { return _tuples; }
		set { _tuples = value; }
	}

	/// <summary>ImpactEventHandler's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		SubscribeToHitCollidersEvents(true);
		tuples = new Dictionary<int, VTuple<Collider2D, Collider2D>>();
		if(zOffsetTolerance <= 0.0f) zOffsetTolerance = DEFAULT_OFFSET_Z;
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

		if(externalHitBoxes != null)
		foreach(HitCollider2D hitBox in externalHitBoxes)
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

		if(externalHitBoxes != null)
		foreach(HitCollider2D hitBox in externalHitBoxes)
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

		if(externalHitBoxes == null) return;

		i = 0;

		foreach(HitCollider2D hitBox in externalHitBoxes)
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
		float deltaZ = Mathf.Abs(obj.transform.position.z - collider.transform.position.z);

		switch(_eventType)
		{
			case HitColliderEventTypes.Enter:
			if(deltaZ > (zOffsetTolerance * zOffsetTolerance))
			{
				/// Store or stack into a registry that will deal with these objects while the trigger stays
				int instanceID = _collider.GetInstanceID();
				VTuple<Collider2D, Collider2D> tuple = new VTuple<Collider2D, Collider2D>(_collider, collider);
				if(!tuples.ContainsKey(instanceID)) tuples.Add(instanceID, tuple);

				if(zEvaluation == null && keepEvaluatingFarColliders) this.StartCoroutine(ZEvaluationRoutine(), ref zEvaluation);

				return;
			}
			break;

			case HitColliderEventTypes.Exit:

			break;
		}

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

	/// <summary>Evaluates Objects on Z-Axis.</summary>
	private IEnumerator ZEvaluationRoutine()
	{
		List<int> indices = new List<int>();

		while(keepEvaluatingFarColliders)
		{

			if(tuples != null && tuples.Count > 0) foreach(KeyValuePair<int, VTuple<Collider2D, Collider2D>> pair in tuples)
			{
				VTuple<Collider2D, Collider2D> colliderTuple = pair.Value;
				Collider2D a = colliderTuple.Item1;
				Collider2D b = colliderTuple.Item2;
				float deltaZ = Mathf.Abs(a.transform.position.z - b.transform.position.z);
				float tolerance = zOffsetTolerance * zOffsetTolerance;

				if(deltaZ <= tolerance)
				{
					Trigger2DInformation info = new Trigger2DInformation(b, a);
					eventsHandler.InvokeTriggerEvent(info, HitColliderEventTypes.Enter, 0);

					indices.Add(pair.Key);
				}
			}

			if(indices.Count > 0)
			{
				foreach(int index in indices)
				{
					tuples.Remove(index);
				}

				indices.Clear();
			}

			yield return null;
		}

		this.DispatchCoroutine(ref zEvaluation);
	}
}
}