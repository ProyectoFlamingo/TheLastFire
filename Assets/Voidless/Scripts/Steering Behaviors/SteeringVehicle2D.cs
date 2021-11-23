using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Voidless
{
public class SteeringVehicle2D : MonoBehaviour
{
	[SerializeField] private float _maxSpeed; 		/// <summary>Vehicle's Maximum Speed.</summary>
	[SerializeField] private float _maxForce; 		/// <summary>Vehicle's Maximum Steering Force.</summary>
	[SerializeField] private float _mass; 			/// <summary>Vehicle's Mass.</summary>
	[Space(5f)]
	[Header("Wander's Attributes:")]
	[SerializeField] private float _offset; 		/// <summary>Wander's Offset [Circle Distance].</summary>
	[SerializeField] private float _radius; 		/// <summary>Wander's Radius.</summary>
	[SerializeField] private float _angleChange; 	/// <summary>Wander's Angle Change.</summary>
#if UNITY_EDITOR
	[SerializeField] private Color color; 			/// <summary>Gizmos' Color.</summary>
#endif
	private Vector2 velocity; 						/// <summary>Vehicle's Velocity.</summary>
	private float wanderAngle; 						/// <summary>Wander's Angle Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets maxSpeed property.</summary>
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set { _maxSpeed = value; }
	}

	/// <summary>Gets and Sets maxForce property.</summary>
	public float maxForce
	{
		get { return _maxForce; }
		set { _maxForce = value; }
	}

	/// <summary>Gets and Sets mass property.</summary>
	public float mass
	{
		get { return _mass; }
		set { _mass = value; }
	}

	/// <summary>Gets and Sets offset property.</summary>
	public float offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>Gets and Sets radius property.</summary>
	public float radius
	{
		get { return _radius; }
		set { _radius = value; }
	}

	/// <summary>Gets and Sets angleChange property.</summary>
	public float angleChange
	{
		get { return _angleChange; }
		set { _angleChange = value; }
	}
#endregion

	/// <summary>Resets SteeringVehicle2D's instance to its default values.</summary>
	private void Reset()
	{
		velocity = Vector2.zero;
		wanderAngle = 0.0f;
		mass = 1.0f;
#if UNITY_EDITOR
		color = Color.red;
#endif
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = color;

		if(radius != 0.0f)
		{
			Vector3 circleCenter = transform.position + (Vector3)(offset != 0.0f ? velocity.normalized * offset : Vector2.zero);

			Gizmos.DrawWireSphere(circleCenter, radius);
		}

		Gizmos.DrawRay(transform.position, velocity);
	}
#endif

	/// <summary>SteeringVehicle2D's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		velocity = Vector2.zero;
	}

	/// <returns>Vehicle's Velocity.</returns>
	public Vector2 GetVelocity() { return velocity; }

	/// <returns>Vehicle's Wander Angle.</returns>
	public float GetWanderAngle() { return wanderAngle; }

#region LocalFunctions:
	/// <summary>Gets Seek Steering Force.</summary>
	/// <param name="t">Target's position.</param>
	/// <returns>Seek Steering Force towards target.</returns>
	public Vector2 GetSeekForce(Vector2 t)
	{
		return GetSeekForce(transform.position, t, ref velocity, maxSpeed, maxForce, mass);
	}

	/// <summary>Gets Flee Steering Force.</summary>
	/// <param name="t">Target's position.</param>
	/// <returns>Flee Steering Force past the target.</returns>
	public Vector2 GetFleeForce(Vector2 t)
	{
		return GetFleeForce(transform.position, t, ref velocity, maxSpeed, maxForce, mass);
	}

	/// <returns>Wandering Steering Force.</returns>
	public Vector2 GetWanderForce()
	{
		return GetWanderForce(transform.position, ref velocity, maxSpeed, maxForce, offset, radius, ref wanderAngle, angleChange, mass);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="rMin">Minimum's Radius.</param>
	/// <param name="rMax">Maximum's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public float GetArrivalWeight(Vector2 t, float rMin, float rMax)
	{
		return GetArrivalWeight(transform.position, t, rMin, rMax);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="r">Arrival's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public float GetArrivalWeight(Vector2 t, float r)
	{
		return GetArrivalWeight(transform.position, t, r);
	}
#endregion

#region StaticFunctions:
	/// <summary>Gets Seek Steering Force.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's position.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <param name="m">Vehicle's Mass [1.0 by default].</param>
	/// <returns>Seek Steering Force towards target.</returns>
	public static Vector2 GetSeekForce(Vector2 p, Vector2 t, ref Vector2 v, float s, float f, float m = 1.0f)
	{
		Vector2 d = t - p;
		d = d.normalized * s;

		Vector2 steering = d - v;
		steering = Vector2.ClampMagnitude(steering, f);

		if(m != 1.0f) steering /= m;

		v = v + steering;
		v = Vector2.ClampMagnitude(v, s);

		return v;
	}

	/// <summary>Gets Flee Steering Force.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's position.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <param name="m">Vehicle's Mass [1.0 by default].</param>
	/// <returns>Flee Steering Force past the target.</returns>
	public static Vector2 GetFleeForce(Vector2 p, Vector2 t, ref Vector2 v, float s, float f, float m = 1.0f)
	{
		Vector2 d = p - t;
		d = d.normalized * s;

		Vector2 steering = d - v;
		steering = Vector2.ClampMagnitude(steering, f);

		if(m != 1.0f) steering /= m;

		v = v + steering;
		v = Vector2.ClampMagnitude(v, s);

		return v;
	}

	/// <summary>Gets Wandering Steering Force.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's position.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <param name="d">Circle's Distance from the position.</param>
	/// <param name="r">Circle's Radius.</param>
	/// <param name="a">Wander Angle's reference.</param>
	/// <param name="m">Vehicle's Mass [1.0 by default].</param>
	/// <returns>Wandering Steering Force.</returns>
	public static Vector2 GetWanderForce(Vector2 p, ref Vector2 v, float s, float f, float d, float r, ref float a, float c, float m = 1.0f)
	{
		Vector2 displacement = Vector2.right * r;
		Vector2 circleCenter = p + (v.normalized * d);
		Vector2 target = Vector2.zero;

		a += Random.Range(-c, c);

		/// No need to rotate the vector if there is no angle...
		if(a != 0.0f) displacement = displacement.Rotate(a);

		target = circleCenter + displacement;

#if UNITY_EDITOR
		Debug.DrawRay(target, Vector3.back * 5.0f, Color.cyan, 5.0f);
#endif

		/*displacement = Vector2.ClampMagnitude(displacement, f);

		if(m != 0.0f) displacement /= m;

		v = Vector2.ClampMagnitude(v + displacement, s);*/
		return GetSeekForce(p, target, ref v, s, f, m);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="rMin">Minimum's Radius.</param>
	/// <param name="rMax">Maximum's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public static float GetArrivalWeight(Vector2 p, Vector2 t, float rMin, float rMax)
	{
		if(rMin == rMax) rMin = 0.0f;
		
		float r = rMax - rMin;
		Vector2 d = t - p;

		r *= r;
		
		if(rMin > 0.0f) d = (t + (d.normalized * rMin)) - p;

		return (Mathf.Min(d.sqrMagnitude, r) / r);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="r">Arrival's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public static float GetArrivalWeight(Vector2 p, Vector2 t, float r)
	{
		return GetArrivalWeight(p, t, 0, r);
	}
#endregion

}
}