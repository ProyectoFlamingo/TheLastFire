using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum WallEvaluationEvent
{
	OffWall,
	Walled,
	Bouncing,
	BounceEnds
}

/// <summary>Event invoked when a WallEvaluator's event occurs.</summary>
/// <param name="_event">Event's argument.</param>
public delegate void OnWallEvaluatorEvent(WallEvaluationEvent _event);

[RequireComponent(typeof(SensorSystem2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DisplacementAccumulator2D))]
public class WallEvaluator : MonoBehaviour
{
	public event OnWallEvaluatorEvent onWallEvaluatorEvent; 	/// <summary>OmWallEvaluationEvent's event delegate.</summary>

	[SerializeField] private ForceInformation2D _forceInfo; 	/// <summary>Bounce's Force info.</summary>
	[SerializeField] private int _wallSensorID; 				/// <summary>Wall Sensor's ID.</summary>
	private TimeConstrainedForceApplier2D _forceApplier; 		/// <summary>Bounce's Force Applier.</summary>
	private bool _walled; 										/// <summary>Current Walled's State.</summary>
	private bool _previousWalled; 								/// <summary>Previous Walled's State.</summary>
	private RaycastHit2D wallInfo; 								/// <summary>Wall's Hit Information.</summary>
	private WallEvaluationEvent _state; 						/// <summary>Current's State.</summary>
	private SensorSystem2D _sensorSystem; 						/// <summary>SensorSystem2D's Component.</summary>
	private Rigidbody2D _rigidbody; 							/// <summary>Rigidbody2D's Component.</summary>
	private DisplacementAccumulator2D _accumulator; 			/// <summary>DisplacementAccumulator2D's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets forceInfo property.</summary>
	public ForceInformation2D forceInfo
	{
		get { return _forceInfo; }
		set { _forceInfo = value; }
	}

	/// <summary>Gets and Sets wallSensorID property.</summary>
	public int wallSensorID
	{
		get { return _wallSensorID; }
		set { _wallSensorID = value; }
	}

	/// <summary>Gets and Sets forceApplier property.</summary>
	public TimeConstrainedForceApplier2D forceApplier
	{
		get { return _forceApplier; }
		private set { _forceApplier = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public WallEvaluationEvent state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets walled property.</summary>
	public bool walled
	{
		get { return _walled; }
		private set { _walled = value; }
	}

	/// <summary>Gets and Sets previousWalled property.</summary>
	public bool previousWalled
	{
		get { return _previousWalled; }
		private set { _previousWalled = value; }
	}

	/// <summary>Gets sensorSystem Component.</summary>
	public SensorSystem2D sensorSystem
	{ 
		get
		{
			if(_sensorSystem == null) _sensorSystem = GetComponent<SensorSystem2D>();
			return _sensorSystem;
		}
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody2D rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
			return _rigidbody;
		}
	}

	/// <summary>Gets accumulator Component.</summary>
	public DisplacementAccumulator2D accumulator
	{ 
		get
		{
			if(_accumulator == null) _accumulator = GetComponent<DisplacementAccumulator2D>();
			return _accumulator;
		}
	}
#endregion

	/// <summary>WallEvaluator's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		forceApplier = new TimeConstrainedForceApplier2D(
			this,
			rigidbody,
			forceInfo.force,
			forceInfo.duration,
			forceInfo.forceMode,
			OnBounceEnds
		);
	}

	/// <summary>WallEvaluator's tick at each frame.</summary>
	private void Update ()
	{
		if(sensorSystem.GetSubsystemDetection(wallSensorID, out wallInfo))
		{
			SurfaceType surfaceType = Game.EvaluateSurfaceType(wallInfo.normal);
			walled = surfaceType == SurfaceType.Wall;

/*#if UNITY_EDITOR
			Debug.DrawRay(wallInfo.point, wallInfo.normal * 5.0f, Color.magenta, 3.0f);
#endif*/
		}
		else walled = false;

		if(walled != previousWalled && onWallEvaluatorEvent != null)
		onWallEvaluatorEvent(walled ? WallEvaluationEvent.Walled : WallEvaluationEvent.OffWall);

		if(state == WallEvaluationEvent.Bouncing) accumulator.AddDisplacement(forceApplier.velocity * forceApplier.timeScale);

		previousWalled = walled;
	}

	/// <returns>Wall's Hit information.</returns>
	public RaycastHit2D GetWallHitInfo() { return wallInfo; }

	/// <summary>Bounces GameObject off-the-wall.</summary>
	/// <param name="_direction">Bounce direction.</param>
	public void BounceOffWall(Vector3 _direction)
	{
		if(state != WallEvaluationEvent.Bouncing)
		{
			forceApplier.force = _direction * forceInfo.force.x;
			forceApplier.ApplyForce();
			state = WallEvaluationEvent.Bouncing;
		}
	}

	/// <summary>Cancels Wall-Bouncing.</summary>
	public void CancelBounce()
	{
		if(state == WallEvaluationEvent.Bouncing)
		{
			forceApplier.CancelForce();
			state = WallEvaluationEvent.OffWall;
			OnBounceEnds();
		}
	}

	/// <summary>Callback invoked when the bouncing's coroutine ends.</summary>
	private void OnBounceEnds()
	{
		state = WallEvaluationEvent.OffWall;
		if(onWallEvaluatorEvent != null) onWallEvaluatorEvent(WallEvaluationEvent.BounceEnds);
	}
}
}