using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Serializable]
public class ChildSelfMotionData
{
	[SerializeField] private Transform _child; 						/// <summary>Children Transform.</summary>
	[SerializeField] [Range(0.0f, 1.0f)] private float _t; 			/// <summary>Normalized [initial] Time.</summary>
	[SerializeField] private SelfMotionData _selfMotionData; 		/// <summary>Self-Motions' Data.</summary>
	private float _time; 											/// <summary>Current Time.</summary>
	private float _initialTime; 									/// <summary>Initial Time.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets child property.</summary>
	public Transform child
	{
		get { return _child; }
		set { _child = value; }
	}

	/// <summary>Gets and Sets t property.</summary>
	public float t
	{
		get { return _t; }
		set { _t = value; }
	}

	/// <summary>Gets and Sets time property.</summary>
	public float time
	{
		get { return _time; }
		set { _time = value; }
	}

	/// <summary>Gets and Sets selfMotionData property.</summary>
	public SelfMotionData selfMotionData
	{
		get { return _selfMotionData; }
		set { _selfMotionData = value; }
	}
#endregion

	/// <summary>ChildSelfMotion's Constructor.</summary>
	/// <param name="_child">Children Transform.</param>
	/// <param name="_selfMotionData">Self-Motions' Data.</param>
	/// <param name="t">Normalized Time [1.0f by default].</param>
	public ChildSelfMotionData(Transform _child, SelfMotionData _selfMotionData, float _t = 1.0f)
	{
		child = _child;
		t = _t;
		selfMotionData = _selfMotionData;
		Initialize();
	}

	/// <summary>Initializes Time.</summary>
	public void Initialize()
	{
		time = t * 360.0f * selfMotionData.speed;
	}

	/// <summary>Advances Time.</summary>
	public void AdvanceTime()
	{
		float speed = selfMotionData.speed;

		time = time < (360.0f * speed) ? time + (Time.deltaTime * speed) : 0.0f;
	}
}
}