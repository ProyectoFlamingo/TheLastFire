using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(BoxCollider2D))]
public class InvokeEventTriggerZone : TriggerZone<InvokeEventTriggerZone>
{
	[Space(5f)]
	[Header("(Additional) Events:")]
	[SerializeField] private UnityEvent onEnterEvent; 	/// <summary>Unity Event invoked when something Enters.</summary>
	[SerializeField] private UnityEvent onExitEvent; 	/// <summary>Unity Event invoked when something Exits.</summary>

	/// <summary>Gets and Sets OnEnterEvent property.</summary>
	public UnityEvent OnEnterEvent
	{
		get { return onEnterEvent; }
		set { onEnterEvent = value; }
	}

	/// <summary>Gets and Sets OnExitEvent property.</summary>
	public UnityEvent OnExitEvent
	{
		get { return onExitEvent; }
		set { onExitEvent = value; }
	}

	/// <summary>Callback internally invoked when a GameObject's Collider2D enters the TriggerZone.</summary>
	/// <param name="_collider">Collider2D that Enters.</param>
	protected override void OnEnter(Collider2D _collider)
	{
		if(OnEnterEvent != null) OnEnterEvent.Invoke();
	}

	/// <summary>Callback internally invoked when a GameObject's Collider2D exits the TriggerZone.</summary>
	/// <param name="_collider">Collider2D that Exits.</param>
	/// <param name="_trigger">Next Trigger that ought to be attended.</param>
	protected override void OnExit(Collider2D _collider, TriggerZone<InvokeEventTriggerZone> _trigger)
	{
		if(OnExitEvent != null) OnExitEvent.Invoke();
	}
}
}