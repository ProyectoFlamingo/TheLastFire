using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class BreakableTargetsContainer : MonoBehaviour
{
	[SerializeField] private BreakableTarget[] _targets; 	/// <summary>Breakable Targets.</summary>

	/// <summary>Gets and Sets targets property.</summary>
	public BreakableTarget[] targets
	{
		get { return _targets; }
		set { _targets = value; }
	}

	/// <summary>Activates Targets.</summary>
	/// <param name="_activate">Activate? true by default.</param>
	public void ActivateTargets(bool _activate = true)
	{
		foreach(BreakableTarget target in targets)
		{
			switch(_activate)
			{
				case true:
				target.OnObjectReset();
				break;

				case false:
				target.OnObjectDeactivation();
				break;
			}
		}
	}

	/// <summary>Subscribes to targets' events.</summary>
	/// <param name="_subscribe">Subscribe? true by default.</param>
	/// <param name="onDeactivated">Callback invoked by each target if they are deactivated.</param>
	public void SubscribeToTargetsDeactivations(bool _subscribe = true, OnDeactivated onDeactivated = null)
	{
		foreach(BreakableTarget target in targets)
		{
			switch(_subscribe)
			{
				case true:
				target.eventsHandler.onDeactivated -= onDeactivated;
				target.eventsHandler.onDeactivated += onDeactivated;
				break;

				case false:
				target.eventsHandler.onDeactivated -= onDeactivated;
				break;
			}
		}
	}

	[Button("Activate Targets")]
	/// <summary>Activates Targets.</summary>
	private void Activate()
	{
		ActivateTargets(true);
	}

	[Button("Deactivate Targets")]
	/// <summary>Deactivates Targets.</summary>
	private void Deactivate()
	{
		ActivateTargets(false);
	}
}
}