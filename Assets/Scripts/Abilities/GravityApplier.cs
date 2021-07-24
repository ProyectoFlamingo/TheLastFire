﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Event invoked when a Boolean's state changes.</summary>
/// <param name="_grounded">New Boolean's State.</param>
public delegate void OnBoolStateChange(bool _grounded);

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DisplacementAccumulator2D))]
[RequireComponent(typeof(SensorSystem2D))]
public class GravityApplier : MonoBehaviour
{
	public event OnBoolStateChange onGroundedStateChange; 				/// <summary>OnBoolStateChange's event delegate.</summary>

	[SerializeField] private Vector2 _gravity; 								/// <summary>Gravity's Vector.</summary>
	[SerializeField] private int _groundSensorID; 							/// <summary>Ground Sensor's ID.</summary>
	[SerializeField] private float _scale; 									/// <summary>Additional Gravity's Scale.</summary>
	[SerializeField] private int _scaleChangePriority; 						/// <summary>Scale Change's Priority.</summary>
	[SerializeField] private bool _useGravity; 								/// <summary>Use Gravity? true by default.</summary>
	private Rigidbody2D _rigidbody; 										/// <summary>Rigidbody2D's Component.</summary>
	private DisplacementAccumulator2D _accumulator; 						/// <summary>displacementAccumulator's Component.</summary>
	private SensorSystem2D _sensorSystem; 									/// <summary>SensorSystem's Component.</summary>
	private Vector2 _velocity; 												/// <summary>Gravity's Velocity.</summary>
	private float _bestScale; 												/// <summary>Best Gravity's Scalar.</summary>
	private bool _grounded; 												/// <summary>Current Grounded's State.</summary>
	private bool _previousGrounded; 										/// <summary>Previous' Grounded State.</summary>
	private Dictionary<int, ValueVTuple<float, int>> _scaleChangeRequests; 	/// <summary>HashSet that registers all scale change requests.</summary>

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
	public Dictionary<int, ValueVTuple<float, int>> scaleChangeRequests
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
		scaleChangeRequests = new Dictionary<int, ValueVTuple<float, int>>();
		UpdateBestScale();
	}

	/// <summary>Updates GravityApplier's instance at each frame.</summary>
	private void Update()
	{
		grounded = sensorSystem.GetSubsystemDetection(groundSensorID);

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
	/// <param name="_scaleChangeInfo">ValueVTuple containing the scale change's information.</param>
	public void RequestScaleChange(int _ID, ValueVTuple<float, int> _scaleChangeInfo)
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
	public void RequestScaleChange(int _ID, float _scale, int _priority)
	{
		RequestScaleChange(_ID, new ValueVTuple<float, int>(_scale, _priority));
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
	private void UpdateBestScale()
	{
		if(scaleChangeRequests == null || scaleChangeRequests.Count <= 0)
		{
			bestScale = scale;
			return;
		}

		int highestPriority = int.MinValue;

		foreach(ValueVTuple<float, int> tuple in scaleChangeRequests.Values)
		{
			if(tuple.Item2 > highestPriority)
			{
				highestPriority = tuple.Item2;
				bestScale = tuple.Item1;
			}
		}
	}
}
}