using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

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

	public void ActivateTargets(bool _activate)
	{
		/*foreach(BreakableTarget target in targets)
		{
			switch(_activate)
		}*/
	}

	public void SubscribeToTargetsDeactivations(Action<DeactivationCause, Trigger2DInformation> onDeactivated)
	{
		/*foreach(BreakableTarget target in targets)
		{
			target.eventsHandler.onDeactivated += onDeactivated;
		}*/
	}
}
}