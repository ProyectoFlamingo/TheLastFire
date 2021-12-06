using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{

public enum MotionType
{
	Default,
	Radial,
	Sinusoidal
}

public class SelfMotionPerformer : MonoBehaviour
{
	[SerializeField] private ChildSelfMotionData[] _childrenSelfMotionData; 	/// <summary>Children's Self-Motion Data.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private Color gizmosColor; 								/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 								/// <summary>Gizmos' Radius.</summary>
#endif

	/// <summary>Gets and Sets childrenSelfMotionData property.</summary>
	public ChildSelfMotionData[] childrenSelfMotionData
	{
		get { return _childrenSelfMotionData; }
		set { _childrenSelfMotionData = value; }
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected virtual void OnDrawGizmos()
	{
		if(childrenSelfMotionData == null) return;
		
		Gizmos.color = gizmosColor;

		foreach(ChildSelfMotionData data in childrenSelfMotionData)
		{
			if(data.child == null) continue;

			float speed = data.selfMotionData.speed;
			float t = data.t;

			switch(data.selfMotionData.type)
			{
				case MotionType.Radial:
				Gizmos.DrawWireSphere(transform.position, data.selfMotionData.radius);
				break;

				case MotionType.Sinusoidal:
				Vector3 a = data.selfMotionData.axes.normalized * data.selfMotionData.radius;

				Gizmos.DrawRay(transform.position, a);
				Gizmos.DrawRay(transform.position, -a);	
				break;
			}

			Vector3 projection = Lerp(transform.position, data.selfMotionData.axes, data.selfMotionData.radius, data.selfMotionData.type, data.t);
			Gizmos.DrawSphere(projection, gizmosRadius);
		}
	}
#endif

	/// <summary>Resets MotionProjectile's instance to its default values.</summary>
	public void Reset()
	{
		foreach(ChildSelfMotionData motionData in childrenSelfMotionData)
		{
			motionData.Initialize();
		}
	}

	/// <summary>SelfMotionPerformer's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		PositionChildren();
	}

	/// <summary>SelfMotionPerformer's tick at each frame.</summary>
	private void Update ()
	{
		if(childrenSelfMotionData == null) return;

		Vector3 position = transform.position;
		Vector3 newPosition = Vector3.zero;
		Vector3 axis = Vector3.zero;
		float time = 0.0f;
		float radius = 0.0f;

		foreach(ChildSelfMotionData data in childrenSelfMotionData)
		{
			if(data.child == null) continue;

			data.AdvanceTime();
			axis = data.selfMotionData.axes.normalized;
			time = data.time;
			radius = data.selfMotionData.radius;

			switch(data.selfMotionData.type)
			{
				case MotionType.Radial:
				newPosition = (Quaternion.Euler(axis * time) * (Vector3.right * radius));
				break;

				case MotionType.Sinusoidal:
				newPosition = (axis * Mathf.Sin(time * Mathf.Deg2Rad) * radius);
				break;
			}

			data.child.position = (position + newPosition);
		}
	}

	/// <summary>Positions Children relative to their motion and their respective Normalized Time parameters.</summary>
	public void PositionChildren()
	{
		if(childrenSelfMotionData == null) return;

		foreach(ChildSelfMotionData data in childrenSelfMotionData)
		{
			if(data.child == null) continue;

			data.Initialize();

			data.child.transform.position = Lerp(
				transform.position,
				data.selfMotionData.axes,
				data.selfMotionData.radius,
				data.selfMotionData.type,
				data.t
			);
		}
	}

	/// <summary>Lerps given the motion type.</summary>
	/// <param name="p">Origin's Position.</param>
	/// <param name="a">Motion's Axis.</param>
	/// <param name="r">Radius.</param>
	/// <param name="type">Motion's Type.</param>
	/// <param name="t">Time's Parameter [internally clamped].</param>
	/// <returns>Interpolation givent the motion type.</returns>
	private Vector3 Lerp(Vector3 p, Vector3 a, float r, MotionType type, float t)
	{
		t = Mathf.Clamp(t, 0.0f, 1.0f);

		if(type == MotionType.Sinusoidal) t = 1.0f - t;

		float x = 360.0f * t;
		Vector3 o = Vector3.zero;

		a.Normalize();

		switch(type)
		{
			case MotionType.Radial:
			o = Quaternion.Euler(a * x) * (Vector3.right * r);
			break;

			case MotionType.Sinusoidal:
			o = a * Mathf.Sin(x * Mathf.Deg2Rad) * r;
			break;
		}

		return p + o;
	}
}
}