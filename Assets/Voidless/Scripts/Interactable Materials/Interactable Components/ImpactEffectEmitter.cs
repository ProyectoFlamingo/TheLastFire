using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(InteractableMaterial))]
public abstract class ImpactEffectEmitter : MonoBehaviour
{
	[SerializeField] private FloatRange _sqrMagnitudeRange; 									/// <summary>Square-Magnitude range zone for  the normalized volume.</summary>
	private EventsHandler _eventsHandler; 														/// <summary>EventsHandler's Component.</summary>
	private InteractableMaterial _interactableMaterial; 										/// <summary>InteractableMaterial's Component.</summary>

	/// <summary>Gets and Sets sqrMagnitudeRange property.</summary>
	public FloatRange sqrMagnitudeRange
	{
		get { return _sqrMagnitudeRange; }
		set { _sqrMagnitudeRange = value; }
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

	/// <summary>Gets interactableMaterial Component.</summary>
	public InteractableMaterial interactableMaterial
	{ 
		get
		{
			if(_interactableMaterial == null) _interactableMaterial = GetComponent<InteractableMaterial>();
			return _interactableMaterial;
		}
	}

	/// <summary>'s instance initialization when loaded [Before scene loads].</summary>
	protected virtual void Awake()
	{
		eventsHandler.onTriggerEvent += OnTriggerEvent;
	}

	/// <summary>Callback invoked when 's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	protected virtual void OnDestroy()
	{
		eventsHandler.onTriggerEvent -= OnTriggerEvent;
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	private void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		if(_eventType != HitColliderEventTypes.Enter) return;

		InteractableMaterial material = _info.collider.GetComponent<InteractableMaterial>();

		if(material == null) return;

		MaterialID materialID = material.ID;
		float v = _info.velocity.sqrMagnitude;
		//float t = sqrMagnitudeRange.RemapToNormalizedRange(v);
		float sMin = sqrMagnitudeRange.Min();
		float sMax = sqrMagnitudeRange.Max();
		sMin *= sMin;
		sMax *= sMax;
		float t = (Mathf.Clamp(v, sMin, sMax) - sMin) / (sMax - sMin);
		
		Emit(materialID, t);
	}

	/// <summary>Emits Effect.</summary>
	/// <param name="ID">Material ID of the GameObject that this GameObject impacted with.</param>
	/// <param name="t">Normalized t value [calculated from the displacement velocity and the range].</param>
	protected abstract void Emit(MaterialID ID, float t);
}
}