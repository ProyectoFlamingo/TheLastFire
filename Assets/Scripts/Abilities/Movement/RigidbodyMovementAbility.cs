using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DisplacementAccumulator2D))]
[RequireComponent(typeof(GravityApplier))]
[RequireComponent(typeof(SlopeEvaluator))]
public class RigidbodyMovementAbility : MovementAbility
{
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _onSlopeScalar; 	/// <summary>Gravity Scalar Applied on Slope.</summary>
	private DisplacementAccumulator2D _accumulator; 	/// <summary>DisplacementAccumulator2D's Component.</summary>
	private Rigidbody2D _rigidbody; 					/// <summary>Rigidbpdy2D's Component.</summary>
	private GravityApplier _gravityApplier; 			/// <summary>GravityApplier's Component.</summary>
	private SlopeEvaluator _slopeEvaluator; 			/// <summary>SlopeEvaluator's Component.</summary>

	/// <summary>Gets and Sets onSlopeScalar property.</summary>
	public float onSlopeScalar
	{
		get { return _onSlopeScalar; }
		set { _onSlopeScalar = value; }
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

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody2D rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
			return _rigidbody;
		}
	}

	/// <summary>Gets gravityApplier Component.</summary>
	public GravityApplier gravityApplier
	{ 
		get
		{
			if(_gravityApplier == null) _gravityApplier = GetComponent<GravityApplier>();
			return _gravityApplier;
		}
	}

	/// <summary>Gets slopeEvaluator Component.</summary>
	public SlopeEvaluator slopeEvaluator
	{ 
		get
		{
			if(_slopeEvaluator == null) _slopeEvaluator = GetComponent<SlopeEvaluator>();
			return _slopeEvaluator;
		}
	}

	/// <summary>Displaces towards given direction.</summary>
	/// <param name="direction">Movement's Direction.</param>
	/// <param name="scale">Additional scalar [1.0f by default].</param>
	/// <param name="space">Space relativity [Space.Self by default].</param>
	public override void Move(Vector2 direction, float scale = 1.0f, Space space = Space.Self)
	{
		float scalar = gravityApplier.grounded ? 1.0f : airScalar;

		if(space == Space.Self) direction = rigidbody.rotation * direction;
		Vector2 displacement = CalculateDisplacement(direction, Time.fixedDeltaTime, scale * scalar);

		if(slopeEvaluator.onSlope) displacement += (gravityApplier.gravity * Time.fixedDeltaTime * onSlopeScalar);

		accumulator.AddDisplacement(displacement);
	}
}
}