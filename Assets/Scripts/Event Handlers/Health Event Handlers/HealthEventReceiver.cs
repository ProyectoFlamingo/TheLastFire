using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public abstract class HealthEventReceiver : MonoBehaviour
{
	[SerializeField] private Health _health; 			/// <summary>Health Component's Reference.</summary>
	[SerializeField] private HealthEvent _invokeAt; 	/// <summary>Event that triggers this component's routine.</summary>
	private Coroutine routine; 							/// <summary>Coroutine's Reference.</summary>

	/// <summary>Gets and Sets health Component.</summary>
	public Health health
	{ 
		get
		{
			if(_health == null) _health = GetComponent<Health>();
			return _health;
		}
		set { _health = value; }
	}

	/// <summary>Gets and Sets invokeAt property.</summary>
	public HealthEvent invokeAt
	{
		get { return _invokeAt; }
		set { _invokeAt = value; }
	}

	/// <summary>HealthEventReceiver's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		if(health != null) health.onHealthEvent += OnHealthEvent;
	}

	/// <summary>Callback invoked when HealthEventReceiver's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		if(health != null) health.onHealthEvent -= OnHealthEvent;
	}

	/// <summary>Event invoked when a Health's event has occured.</summary>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	private void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		if(_event == invokeAt) this.StartCoroutine(Routine(), ref routine);
	}

	/// <summary>Cancels Routine.</summary>
	public void CancelRoutine()
	{
		this.DispatchCoroutine(ref routine);
	}

	/// <summary>Routine.</summary>
	public abstract IEnumerator Routine();
}
}