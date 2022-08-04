using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public delegate void OnTriggerZoneEvent<T>(TriggerZone<T> _triggerZone, HitColliderEventTypes _eventType);

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Boundaries2DContainer))]
public abstract class TriggerZone<T> : MonoBehaviour/* where T : TriggerZone<T>*/
{
	public static event OnTriggerZoneEvent<T> onTriggerZoneEvent; 	/// <summary>OnTriggerZoneEvent's Delegate.</summary>

	protected static HashSet<TriggerZone<T>> triggerZones; 			/// <summary>TriggerZone's static registry.</summary>

	[SerializeField] private GameObjectTag[] _tags; 				/// <summary>GameObject Tags that invoke the trigger.</summary>
	[Space(5f)]
	[SerializeField] protected Color gizmosColor; 					/// <summary>Gizmos' Color.</summary>
	private Boundaries2DContainer _boundariesContainer; 			/// <summary>Boundaries2DContainer's Component.</summary>
	private BoxCollider2D _boxCollider; 							/// <summary>BoxCollider2D's Component.</summary>
	private bool _entered; 											/// <summary>Has an Object entered inside this TriggerZone?.</summary>

	/// <summary>Gets tags property.</summary>
	public GameObjectTag[] tags { get { return _tags; } }

	/// <summary>Gets and Sets entered property.</summary>
	public bool entered
	{
		get { return _entered; }
		protected set { _entered = value; }
	}

	/// <summary>Gets boundariesContainer Component.</summary>
	public Boundaries2DContainer boundariesContainer
	{ 
		get
		{
			if(_boundariesContainer == null) _boundariesContainer = GetComponent<Boundaries2DContainer>();
			return _boundariesContainer;
		}
	}

	/// <summary>Gets boxCollider Component.</summary>
	public BoxCollider2D boxCollider
	{ 
		get
		{
			if(_boxCollider == null) _boxCollider = GetComponent<BoxCollider2D>();
			return _boxCollider;
		}
	}

	/// <summary>Static TriggerZone<T>'s Constructor.</summary>
	static TriggerZone()
	{
		triggerZones = new HashSet<TriggerZone<T>>();
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected virtual void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Vector3 center = boundariesContainer.GetPosition();
		Vector3 size = boundariesContainer.size;

		size.z = Mathf.Min(size.z, 0.1f);

		Gizmos.color = gizmosColor.WithAlpha(0.25f);

		Gizmos.DrawCube(center, size);
#endif
	}

	/// <summary>TriggerZone's instance initialization.</summary>
	protected virtual void Awake()
	{
		entered = false;
	}

	[OnInspectorGUI]
	/// <summary>Updates BoxCollider2D.</summary>
	protected virtual void UpdateBoxCollider()
	{
		boxCollider.size = boundariesContainer.size;
		//boxCollider.offset = boundariesContainer.center;
	}

	/// <summary>Invokes event.</summary>
	/// <param name="_eventType">Interaction Event Type.</param>
	protected void InvokeEvent(HitColliderEventTypes _eventType)
	{
		if(onTriggerZoneEvent != null) onTriggerZoneEvent(this, _eventType);
	}

	/// <summary>Clears Trigger-Zones' Mapping.</summary>
	public static void ClearTriggerZonesMapping()
	{
		triggerZones.Clear();
	}

	/// <summary>Event triggered when this Collider2D enters another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(entered || tags == null) return;

		GameObject obj = col.gameObject;
		
		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				//triggerZones.Add(this);
				entered = true;
				OnEnter(col);
				return;
			}
		}
	}

	/// <summary>Event triggered when this Collider2D exits another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerExit2D(Collider2D col)
	{
		if(!entered || tags == null) return;

		GameObject obj = col.gameObject;
		
		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				//triggerZones.Remove(this);
				entered = false;
				OnExit(col, triggerZones.Count > 0 ? triggerZones.First() : null);
				return;
			}
		}
	}

	/// <summary>Callback internally invoked when a GameObject's Collider2D enters the TriggerZone.</summary>
	/// <param name="_collider">Collider2D that Enters.</param>
	protected virtual void OnEnter(Collider2D _collider)
	{
		if(triggerZones.Contains(this)) return;

		triggerZones.Add(this);

		InvokeEvent(HitColliderEventTypes.Enter);
	}

	/// <summary>Callback internally invoked when a GameObject's Collider2D exits the TriggerZone.</summary>
	/// <param name="_collider">Collider2D that Exits.</param>
	/// <param name="_nextTriggerZone">Next Trigger that ought to be attended.</param>
	protected virtual void OnExit(Collider2D _collider, TriggerZone<T> _nextTriggerZone)
	{
		if(!triggerZones.Contains(this)) return;

		triggerZones.Remove(this);

		InvokeEvent(HitColliderEventTypes.Exit);
	}

	/// <summary>Adds TriggerZone to HashSet.</summary>
	public static void AddTriggerZone(TriggerZone<T> _triggerZone)
	{
		triggerZones.Add(_triggerZone);
	}

	/// <summary>Removes TriggerZone to HashSet.</summary>
	public static void RemoveTriggerZone(TriggerZone<T> _triggerZone)
	{
		triggerZones.Remove(_triggerZone);
	}

	/// <returns>HashSet's Count.</returns>
	public static int GetTriggersCount()
	{
		return triggerZones.Count;
	}
}
}