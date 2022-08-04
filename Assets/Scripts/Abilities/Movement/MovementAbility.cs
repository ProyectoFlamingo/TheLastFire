using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Event invoked when braking (or not) happens.</summary>
/// <param name="_braking">Did it brake?.</param>
public delegate void OnBraking(bool _braking);

public class MovementAbility : MonoBehaviour
{
	public event OnBraking onBraking; 						/// <summary>OnBraking's delegate.</summary>

	[SerializeField] private float _speed; 					/// <summary>Movement's Speed.</summary>
	[SerializeField] private float _airScalar; 				/// <summary>Movement's scalar applied when the movement is made on air.</summary>
	[Space(5f)]
	[Header("Braking's Attributes:")]
	[SerializeField] private float _minSpeedForBraking; 	/// <summary>Min Speed's magnitude required for braking when changing to a contrary direction.</summary>
	[SerializeField] private float _maxBrakingSpeed; 		/// <summary>Maximum's Displacement speed to provoke brakinf if moving on an opposite direction.</summary>
	[SerializeField] private float _brakingDuration; 		/// <summary>Braking's duration.</summary>
	[SerializeField]
	[Range(-1.0f, 0.0f)] private float _dotTolerance; 		/// <summary>Dot product tolerance between the accumulated displacement and the current displacement to consider whether to perform brake.</summary>
	private Vector2 _accumulatedDisplacement; 				/// <summary>Accumulated's Movement displacement.</summary>
	private bool _braking; 									/// <summary>Is the movement on Braking State?.</summary>
	protected Coroutine brakingRoutine; 					/// <summary>Braking's Coroutine reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets speed property.</summary>
	public float speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	/// <summary>Gets and Sets airScalar property.</summary>
	public float airScalar
	{
		get { return _airScalar; }
		set { _airScalar = value; }
	}

	/// <summary>Gets and Sets minSpeedForBraking property.</summary>
	public float minSpeedForBraking
	{
		get { return _minSpeedForBraking; }
		set { _minSpeedForBraking = value; }
	}

	/// <summary>Gets and Sets maxBrakingSpeed property.</summary>
	public float maxBrakingSpeed
	{
		get { return _maxBrakingSpeed; }
		set { _maxBrakingSpeed = value; }
	}

	/// <summary>Gets and Sets brakingDuration property.</summary>
	public float brakingDuration
	{
		get { return _brakingDuration; }
		set { _brakingDuration = value; }
	}

	/// <summary>Gets and Sets dotTolerance property.</summary>
	public float dotTolerance
	{
		get { return _dotTolerance; }
		set { _dotTolerance = value; }
	}

	/// <summary>Gets and Sets accumulatedDisplacement property.</summary>
	public Vector2 accumulatedDisplacement
	{
		get { return _accumulatedDisplacement; }
		protected set { _accumulatedDisplacement = value; }
	}

	/// <summary>Gets and Sets braking property.</summary>
	public bool braking
	{
		get { return _braking; }
		protected set { _braking = value; }
	}
#endregion

	/// <summary>Displaces towards given direction.</summary>
	/// <param name="direction">Movement's Direction.</param>
	/// <param name="scale">Additional scalar [1.0f by default].</param>
	/// <param name="space">Space relativity [Space.Self by default].</param>
	public virtual void Move(Vector2 direction, float scale = 1.0f, Space space = Space.Self)
	{
		transform.Translate(CalculateDisplacement(direction, Time.deltaTime, scale) * Time.deltaTime, space);
	}

	/// I MAY HAVE CREATED A DYNAMIC HERE. WHAT AN IDIOT...
	/// <summary>Stops [resets the accumulated velocity].</summary>
	public void Stop()
	{
		accumulatedDisplacement = Vector2.zero;
	}

	/// <summary>Cancels Braaking's Routine.</summary>
	public void CancelBraking()
	{
		this.DispatchCoroutine(ref brakingRoutine);
		braking = false;
		Stop();
		if(onBraking != null) onBraking(false);
	}

	/// <summary>Calculates ideal displacement.</summary>
	/// <param name="direction">Movement's Direction.</param>
	/// <param name="deltaTime">Delta Time's Value.</param>
	/// <param name="scale">Additional scalar [1.0f by default].</param>
	/// <returns>Displacement Vector.</returns>
	protected virtual Vector2 CalculateDisplacement(Vector2 direction, float deltaTime, float scale = 1.0f)
	{
		Vector2 displacement = direction * speed * scale;
		
		if(!braking)
		{
			float dot = Vector2.Dot(displacement.normalized, accumulatedDisplacement.normalized);
			float minMagnitude = minSpeedForBraking * minSpeedForBraking;

			if(accumulatedDisplacement.sqrMagnitude >= minMagnitude && dot <= dotTolerance)
			{
				braking = true;
				if(onBraking != null) onBraking(true);
				this.StartCoroutine(DiminishAccumulatedDisplacement(), ref brakingRoutine);
			}

			accumulatedDisplacement += displacement * deltaTime;
			if(accumulatedDisplacement.sqrMagnitude > maxBrakingSpeed) accumulatedDisplacement = Vector2.ClampMagnitude(accumulatedDisplacement, maxBrakingSpeed);
		}

		if(braking) displacement += Vector2.ClampMagnitude(accumulatedDisplacement, maxBrakingSpeed);

		return displacement;
	}

	/// <summary>Diminishes the accumulated displacement for a braking duration.</summary>
	private IEnumerator DiminishAccumulatedDisplacement()
	{
		Vector2 displacement = accumulatedDisplacement;
		float t = 0.0f;
		float inverseDuration = 1.0f / brakingDuration;

		while(t < 1.0f)
		{
			accumulatedDisplacement = Vector2.Lerp(displacement, Vector2.zero, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		accumulatedDisplacement = Vector2.zero;
		CancelBraking();
	}
}
}