using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class RingsContainer : MonoBehaviour
{
	[SerializeField] private Ring[] _rings; 	/// <summary>Rings.</summary>

	/// <summary>Gets and Sets rings property.</summary>
	public Ring[] rings
	{
		get { return _rings; }
		set { _rings = value; }
	}

	/// <summary>Activates Rings.</summary>
	/// <param name="_activate">Activate? true by default.</param>
	public void ActivateRings(bool _activate = true)
	{
		foreach(Ring ring in rings)
		{
			switch(_activate)
			{
				case true:
				ring.OnObjectReset();
				break;

				case false:
				ring.OnObjectDeactivation();
				break;
			}
		}
	}

	/// <summary>Subscribes to rings' events.</summary>
	/// <param name="_subscribe">Subscribe? true by default.</param>
	/// <param name="onRingPassed">Callback invoked by each ring if they are passed by an object.</param>
	public void SubscribeToRingsDeactivations(bool _subscribe = true, OnRingPassed onRingPassed = null)
	{
		foreach(Ring ring in rings)
		{
			switch(_subscribe)
			{
				case true:
				ring.onRingPassed -= onRingPassed;
				ring.onRingPassed += onRingPassed;
				break;

				case false:
				ring.onRingPassed -= onRingPassed;
				break;
			}
		}
	}

	[Button("Activate Rings")]
	/// <summary>Activates Rings.</summary>
	private void Activate()
	{
		ActivateRings(true);
	}

	[Button("Deactivate Rings")]
	/// <summary>Deactivates Rings.</summary>
	private void Deactivate()
	{
		ActivateRings(false);
	}
}
}