using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Event invoked when a Boolean's state changes.</summary>
/// <param name="_grounded">New Boolean's State.</param>
public delegate void OnBoolStateChange(bool _grounded);

/*
	Scale Change Requests takes Priorities that are registered in a Dictionary<int, VTuple<FloatWrapper, int>>

	Where:
	 - Key - int:  Instance ID of the requester
	 - Value:
	 	- FloatWrapper: Wrapper that contains the scalar  (it is a wrapper so the requester can change the scalar without making another request)
	 	- int: Priority of the Scalar change, since many other requesters may ask for a scalar change at the same time.
*/

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DisplacementAccumulator2D))]
[RequireComponent(typeof(SensorSystem2D))]
public class GravityApplier : MonoBehaviour
{
	public event OnBoolStateChange onGroundedStateChange; 						/// <summary>OnBoolStateChange's event delegate.</summary>

	[SerializeField] private Vector2 _gravity; 									/// <summary>Gravity's Vector.</summary>
	[SerializeField] private int _groundSensorID; 								/// <summary>Ground Sensor's ID.</summary>
	[SerializeField] private float _maxMagnitude; 								/// <summary>Max's Magnitude.</summary>
	[SerializeField] private float _scale; 										/// <summary>Additional Gravity's Scale.</summary>
	[SerializeField] private int _scaleChangePriority; 							/// <summary>Scale Change's Priority.</summary>
	[SerializeField] private bool _useGravity; 									/// <summary>Use Gravity? true by default.</summary>
	private Rigidbody2D _rigidbody; 											/// <summary>Rigidbody2D's Component.</summary>
	private DisplacementAccumulator2D _accumulator; 							/// <summary>displacementAccumulator's Component.</summary>
	private SensorSystem2D _sensorSystem; 										/// <summary>SensorSystem's Component.</summary>
	private Vector2 _velocity; 													/// <summary>Gravity's Velocity.</summary>
	private float _bestScale; 													/// <summary>Best Gravity's Scalar.</summary>
	private bool _grounded; 													/// <summary>Current Grounded's State.</summary>
	private bool _previousGrounded; 											/// <summary>Previous' Grounded State.</summary>
	private Dictionary<int, VTuple<FloatWrapper, int>> _scaleChangeRequests; 	/// <summary>HashSet that registers all scale change requests.</summary>
#if UNITY_EDITOR
	private SurfaceType groundSensorSurfaceType; 								/// <summary>Surface Type detected on the Ground Sensor.</summary>		
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets gravity property.</summary>
	public Vector2 gravity
	{
		get { return _gravity; }
		set { _gravity = value; }
	}

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector2 velocity
	{
		get { return _velocity; }
		set { _velocity = value; }
	}

	/// <summary>Gets and Sets groundSensorID property.</summary>
	public int groundSensorID
	{
		get { return _groundSensorID; }
		set { _groundSensorID = value; }
	}

	/// <summary>Gets and Sets maxMagnitude property.</summary>
	public float maxMagnitude
	{
		get { return _maxMagnitude; }
		set { _maxMagnitude = value; }
	}

	/// <summary>Gets and Sets scale property.</summary>
	public float scale
	{
		get { return _scale; }
		set { _scale = value; }
	}

	/// <summary>Gets and Sets bestScale property.</summary>
	public float bestScale
	{
		get { return _bestScale; }
		protected set { _bestScale = value; }
	}

	/// <summary>Gets and Sets useGravity property.</summary>
	public bool useGravity
	{
		get { return _useGravity; }
		set { _useGravity = value; }
	}

	/// <summary>Gets and Sets grounded property.</summary>
	public bool grounded
	{
		get { return _grounded; }
		private set { _grounded = value; }
	}

	/// <summary>Gets and Sets previousGrounded property.</summary>
	public bool previousGrounded
	{
		get { return _previousGrounded; }
		protected set { _previousGrounded = value; }
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

	/// <summary>Gets sensorSystem Component.</summary>
	public SensorSystem2D sensorSystem
	{ 
		get
		{
			if(_sensorSystem == null) _sensorSystem = GetComponent<SensorSystem2D>();
			return _sensorSystem;
		}
	}

	/// <summary>Gets and Sets scaleChangeRequests property.</summary>
	public Dictionary<int, VTuple<FloatWrapper, int>> scaleChangeRequests
	{
		get { return _scaleChangeRequests; }
		protected set { _scaleChangeRequests = value; }
	}
#endregion

	/// <summary>Resets GravityApplier's instance to its default values.</summary>
	private void Reset()
	{
		gravity = Physics2D.gravity;
		scale = 1.0f;
		useGravity = true;
		velocity = Vector2.zero;
	}

	/// <summary>GravityApplier's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		scaleChangeRequests = new Dictionary<int, VTuple<FloatWrapper, int>>();
		UpdateBestScale();
	}

	/// <summary>Updates GravityApplier's instance at each frame.</summary>
	private void Update()
	{
		RaycastHit2D hitInfo = default(RaycastHit2D);
		Vector2 sensorOrigin = Vector2.zero;

		if(sensorSystem.GetSubsystemDetection(groundSensorID, out hitInfo, out sensorOrigin))
		{
			if(hitInfo.collider.PointInside(sensorOrigin) && !previousGrounded)
			{
				grounded = false;
			}
			else
			{
				SurfaceType surfaceType = Game.EvaluateSurfaceType(hitInfo.normal);
				grounded = surfaceType == SurfaceType.Floor;
			}
		}
		else
		{
			grounded = false;
#if UNITY_EDITOR
			groundSensorSurfaceType = SurfaceType.Undefined;
#endif
		}

		if(grounded != previousGrounded && onGroundedStateChange != null)
		onGroundedStateChange(grounded);

		if(grounded) ResetVelocity();
		previousGrounded = grounded;
	}

	/// <summary>Updates GravityApplier's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		if(useGravity)
		{
			velocity += bestScale != 0.0f ? (gravity * bestScale * Time.fixedDeltaTime) : Vector2.zero;
			if(maxMagnitude > 0.0f) velocity = Vector2.ClampMagnitude(velocity, maxMagnitude);
			accumulator.AddDisplacement(velocity);
		}
		else ResetVelocity();
	}

	/// <summary>Resets Velocity.</summary>
	public void ResetVelocity()
	{
		velocity *= 0.0f;
	}

	/// <summary>Requests Scale changes.</summary>
	/// <param name="_ID">Object's ID.</param>
	/// <param name="_scaleChangeInfo">VTuple FloatWrapperining the scale change's information.</param>
	public void RequestScaleChange(int _ID, VTuple<FloatWrapper, int> _scaleChangeInfo)
	{
		if(scaleChangeRequests == null) return;

		if(!scaleChangeRequests.ContainsKey(_ID)) scaleChangeRequests.Add(_ID, _scaleChangeInfo);
		else scaleChangeRequests[_ID] = _scaleChangeInfo;

		UpdateBestScale();
	}

	/// <summary>Requests Scale changes.</summary>
	/// <param name="_ID">Object's ID.</param>
	/// <param name="_scale">Requested Gravity's Scale.</param>
	/// <param name="_priority">Priority's Value.</param>
	public void RequestScaleChange(int _ID, FloatWrapper _scaleWrapper, int _priority)
	{
		RequestScaleChange(_ID, new VTuple<FloatWrapper, int>(_scaleWrapper, _priority));
	}

	/// <summary>Rejects Scale Cahnge.</summary>
	/// <param name="_ID">Object's ID.</param>
	public void RejectScaleChange(int _ID)
	{
		if(scaleChangeRequests == null) return;

		if(scaleChangeRequests.ContainsKey(_ID))
		{
			scaleChangeRequests.Remove(_ID);
			UpdateBestScale();
		}
	}

	/// <summary>Updates best scale.</summary>
	public void UpdateBestScale()
	{
		if(scaleChangeRequests == null || scaleChangeRequests.Count <= 0)
		{
			bestScale = scale;
			return;
		}

		int highestPriority = int.MinValue;

		foreach(VTuple<FloatWrapper, int> tuple in scaleChangeRequests.Values)
		{
			if(tuple.Item2 > highestPriority)
			{
				highestPriority = tuple.Item2;
				bestScale = tuple.Item1.value;
			}
		}
	}

	/// <returns>String representing this GravityApplier's Instance.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("GravityApplier" );
		builder.AppendLine("{ ");
		builder.Append("\tGrounded: ");
		builder.AppendLine(grounded.ToString());
		builder.Append("\tPrevious Grounded: ");
		builder.AppendLine(previousGrounded.ToString());
		builder.Append("\tVelocity: ");
		builder.AppendLine(velocity.ToString());
		builder.AppendLine("\tScale-Change Requests: ");
		builder.Append("\t");
		builder.AppendLine(scaleChangeRequests.DictionaryToString());
#if UNITY_EDITOR
		builder.Append("\tLast Surface-Type: ");
		builder.AppendLine(groundSensorSurfaceType.ToString());
#endif
		builder.Append(" }");

		return builder.ToString();
	}
}
}