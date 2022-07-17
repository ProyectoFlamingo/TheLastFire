using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class CameraModifierTriggerZone : TriggerZone<CameraModifierTriggerZone>
{
	[Space(5f)]
	[Header("Distance Settings:")]
	[SerializeField] private bool _setDistance; 								/// <summary>Set Camera's Distance?.</summary>
	[SerializeField] private FloatRange _distanceRange; 						/// <summary>Camera's Distance Range.</summary>

	/// <summary>Gets and Sets setDistance property.</summary>
	public bool setDistance
	{
		get { return _setDistance; }
		set { _setDistance = value; }
	}

	/// <summary>Gets and Sets distanceRange property.</summary>
	public FloatRange distanceRange
	{
		get { return _distanceRange; }
		set { _distanceRange = value; }
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		if(!setDistance) return;

		Vector3 center = boundariesContainer.GetPosition();
		Vector3 minPoint = center + (Vector3.back * distanceRange.Min());
		Vector3 maxPoint = center + (Vector3.back * distanceRange.Max());

		Gizmos.DrawLine(center, minPoint);
		Gizmos.DrawLine(center, maxPoint);
		Gizmos.DrawWireSphere(minPoint, 0.05f);
		Gizmos.DrawWireSphere(maxPoint, 0.05f);
	}

	/// <summary>Callback internally invoked when a GameObject's Collider2D enters the TriggerZone.</summary>
	/// <param name="_collider">Collider2D that Enters.</param>
	protected override void OnEnter(Collider2D _collider)
	{
		if(triggerZones.Contains(this)) return;

		triggerZones.Add(this);

		InvokeEvent(HitColliderEventTypes.Enter);

		if(triggerZones.Count > 1) return;

		GameplayCameraController cameraController = Game.cameraController;

		if(cameraController == null) return;

		cameraController.constraints = CameraFollowingConstraints.BoundaryConstrained;
		cameraController.boundariesContainer.Set(boundariesContainer.ToBoundaries2D());
		//cameraController.boundariesContainer.InterpolateTowards(boundariesContainer.ToBoundaries2D(), interpolationDuration);

		if(setDistance) cameraController.distanceAdjuster.distanceRange = distanceRange;
	}

	/// <summary>Callback internally invoked when a GameObject's Collider2D exits the TriggerZone.</summary>
	/// <param name="_collider">Collider2D that Exits.</param>
	/// <param name="_nextTriggerZone">Next Trigger that ought to be attended.</param>
	protected override void OnExit(Collider2D _collider, TriggerZone<CameraModifierTriggerZone> _nextTriggerZone)
	{
		if(!triggerZones.Contains(this)) return;

		triggerZones.Remove(this);

		InvokeEvent(HitColliderEventTypes.Exit);

		GameplayCameraController cameraController = Game.cameraController;

		if(cameraController == null) return;

		boundariesContainer.OnInterpolationEnds();

		if(triggerZones.Count == 0)
		{
			cameraController.constraints = CameraFollowingConstraints.Unconstrained;
			//SetDefaultCameraBoundaries2D();
			Game.SetDefaultCameraDistanceRange();
		}
		else
		{
			CameraModifierTriggerZone modifier = triggerZones.First() as CameraModifierTriggerZone;

			cameraController.boundariesContainer.Set(modifier.boundariesContainer.ToBoundaries2D());
			//cameraController.boundariesContainer.InterpolateTowards(modifier.boundariesContainer.ToBoundaries2D(), modifier.interpolationDuration);
			
			if(modifier.setDistance) Game.cameraController.distanceAdjuster.distanceRange = modifier.distanceRange;
		}
	}
}
}