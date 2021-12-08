using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// \TODO Practically Deprecated...
public enum JumpState
{
	Grounded = 1,
	Jumping = 2,
	Falling = 4,
	Landing = 8
}

/// <summary>Event invoked when JumpAbility's State Changes.</summary>
/// <param name="_stateID">State's ID.</param>
/// <param name="_jumpLevel">Jump's Level [index].</param>
public delegate void OnJumpStateChange(int _stateID, int _jumpLevel);

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DisplacementAccumulator2D))]
[RequireComponent(typeof(GravityApplier))]
public class JumpAbility : MonoBehaviour, IStateMachine
{
	public event OnJumpStateChange onJumpStateChange; 			/// <summary>OnJumpStateChange's event delegate.</summary>

	public const int STATE_FLAG_GROUNDED = 0; 					/// <summary>Grounded State's Flag.</summary>
	public const int STATE_FLAG_JUMPING = 1; 					/// <summary>Jumping State's Flag.</summary>
	public const int STATE_FLAG_FALLING = 2; 					/// <summary>Falling State's Flag.</summary>
	public const int STATE_FLAG_LANDING = 3; 					/// <summary>Landing State's Flag.</summary>
	public const int STATE_ID_GROUNDED = 1 << 0; 				/// <summary>Grounded State's ID.</summary>
	public const int STATE_ID_JUMPING = 1 << 1; 				/// <summary>Jumping State's ID.</summary>
	public const int STATE_ID_FALLING = 1 << 2; 				/// <summary>Falling State's ID.</summary>
	public const int STATE_ID_LANDING = 1 << 3; 				/// <summary>Landing State's ID.</summary>

	[SerializeField] private int _applyDirectionFromIndex; 	/// <summary>Index from which the jumps can apply additional direction.</summary>
	[Space(5f)]
	[Header("Gravity Scales' Settings:")]
	[SerializeField] private float _groundedScale; 				/// <summary>Gravity's Scale when Grounded.</summary>
	[SerializeField] private float _jumpingScale; 				/// <summary>Gravity's Scale when Jumping.</summary>
	[SerializeField] private float _fallingScale; 				/// <summary>Gravity's Scale when Falling.</summary>
	[SerializeField] private float _downwardsScalarAddition; 	/// <summary>Downwards Gravity-Scalar's Addition.</summary>
	[SerializeField] private float _upwardsScalarAddition; 		/// <summary>Upwards Gravity-Scalar's Addition.</summary>
	[SerializeField] private int _scaleChangePriority; 			/// <summary>Gravity Scale's Change Priority.</summary>
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _progressForExtraJump; 	/// <summary>Minimum Progress required for extra jump.</summary>
	[SerializeField] private ForceInformation2D[] _forcesInfo; 	/// <summary>Force's Information.</summary>
	[SerializeField] private int[] _SFXsIndices; 				/// <summary>Sound Effects' Indices.</summary>
	[Space(5f)]
	[SerializeField] private float _landingDuration; 			/// <summary>Landing's Duration.</summary>
	private TimeConstrainedForceApplier2D[] _forcesAppliers; 	/// <summary>Forces' Appliers.</summary>
	private FloatWrapper _scalarWrapper; 						/// <summary>Gravity's Scalar Wrapper.</summary>
	private int _currentJumpIndex; 								/// <summary>Current Jump's Index.</summary>
	private int _state; 										/// <summary>Current State.</summary>
	private int _previousState; 								/// <summary>Previous State.</summary>
	private int _ignoreResetMask; 								/// <summary>Mask that selectively contains state to ignore resetting if they were added again [with AddState's method]. As it is 0 by default, it won't ignore resetting any state [~0 = 11111111]</summary>
	private Rigidbody2D _rigidbody; 							/// <summary>Rigidbody's Component.</summary>
	private DisplacementAccumulator2D _accumulator; 			/// <summary>displacementAccumulator's Component.</summary>
	private GravityApplier _gravityApplier; 					/// <summary>GravityApplier's Component.</summary>
	private Cooldown _landingCooldown; 							/// <summary>Landing Cooldown's reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets groundedScale property.</summary>
	public float groundedScale
	{
		get { return _groundedScale; }
		set { _groundedScale = value; }
	}

	/// <summary>Gets and Sets jumpingScale property.</summary>
	public float jumpingScale
	{
		get { return _jumpingScale; }
		set { _jumpingScale = value; }
	}

	/// <summary>Gets and Sets fallingScale property.</summary>
	public float fallingScale
	{
		get { return _fallingScale; }
		set { _fallingScale = value; }
	}

	/// <summary>Gets and Sets landingDuration property.</summary>
	public float landingDuration
	{
		get { return _landingDuration; }
		set { _landingDuration = value; }
	}

	/// <summary>Gets and Sets progressForExtraJump property.</summary>
	public float progressForExtraJump
	{
		get { return _progressForExtraJump; }
		set { _progressForExtraJump = value; }
	}

	/// <summary>Gets and Sets downwardsScalarAddition property.</summary>
	public float downwardsScalarAddition
	{
		get { return _downwardsScalarAddition; }
		set { _downwardsScalarAddition = value; }
	}

	/// <summary>Gets and Sets upwardsScalarAddition property.</summary>
	public float upwardsScalarAddition
	{
		get { return _upwardsScalarAddition; }
		set { _upwardsScalarAddition = value; }
	}

	/// <summary>Gets and Sets applyDirectionFromIndex property.</summary>
	public int applyDirectionFromIndex
	{
		get { return _applyDirectionFromIndex; }
		set { _applyDirectionFromIndex = value; }
	}

	/// <summary>Gets and Sets scaleChangePriority property.</summary>
	public int scaleChangePriority
	{
		get { return _scaleChangePriority; }
		set { _scaleChangePriority = value; }
	}

	/// <summary>Gets and Sets forcesInfo property.</summary>
	public ForceInformation2D[] forcesInfo
	{
		get { return _forcesInfo; }
		private set { _forcesInfo = value; }
	}

	/// <summary>Gets and Sets currentJumpIndex property.</summary>
	public int currentJumpIndex
	{
		get { return _currentJumpIndex; }
		private set { _currentJumpIndex = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public int state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public int previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}

	/// <summary>Gets and Sets ignoreResetMask property.</summary>
	public int ignoreResetMask
	{
		get { return _ignoreResetMask; }
		set { _ignoreResetMask = value; }
	}

	/// <summary>Gets and Sets forcesAppliers property.</summary>
	public TimeConstrainedForceApplier2D[] forcesAppliers
	{
		get { return _forcesAppliers; }
		set { _forcesAppliers = value; }
	}

	/// <summary>Gets and Sets SFXsIndices property.</summary>
	public int[] SFXsIndices
	{
		get { return _SFXsIndices; }
		set { _SFXsIndices = value; }
	}

	/// <summary>Gets and Sets scalarWrapper property.</summary>
	public FloatWrapper scalarWrapper
	{
		get { return _scalarWrapper; }
		set { _scalarWrapper = value; }
	}

	/// <summary>Gets grounded property.</summary>
	public bool grounded { get { return this.HasState(STATE_ID_GROUNDED); } }

	/// <summary>Gets and Sets landingCooldown property.</summary>
	public Cooldown landingCooldown
	{
		get { return _landingCooldown; }
		private set { _landingCooldown = value; }
	}

	/// <summary>Gets rigidbody Component.</summary>
	public new Rigidbody2D rigidbody
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

	/// <summary>Gets gravityApplier Component.</summary>
	public GravityApplier gravityApplier
	{ 
		get
		{
			if(_gravityApplier == null) _gravityApplier = GetComponent<GravityApplier>();
			return _gravityApplier;
		}
	}
#endregion

	/// <summary>JumpAbility's instance initialization.</summary>
	private void Awake()
	{
		scalarWrapper = new FloatWrapper(0.0f);
		this.ChangeState(STATE_ID_GROUNDED);
		UpdateForcesAppliers();
		landingCooldown = new Cooldown(this, landingDuration, OnLandingCooldownEnds);
		gravityApplier.onGroundedStateChange += OnGroundedStateChange;
		currentJumpIndex = -1;
	}

	/// <summary>Updates JumpAbility's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate(){ /*...*/ }

#region StateMachineCallbacks:
	/// <summary>Enters int State.</summary>
	/// <param name="_state">int State that will be entered.</param>
	public void OnEnterState(int _state)
	{
		switch(_state)
		{
			case STATE_ID_GROUNDED:
			scalarWrapper.value = groundedScale;
			gravityApplier.RequestScaleChange(GetInstanceID(), _scalarWrapper, scaleChangePriority);
			CancelForce(currentJumpIndex);
			currentJumpIndex = -1;
			this.RemoveStates(STATE_ID_JUMPING);
			break;

			case STATE_ID_JUMPING:
			scalarWrapper.value = jumpingScale;
			gravityApplier.RequestScaleChange(GetInstanceID(), _scalarWrapper, scaleChangePriority);
			break;

			case STATE_ID_FALLING:
			scalarWrapper.value = fallingScale;
			gravityApplier.RequestScaleChange(GetInstanceID(), _scalarWrapper, scaleChangePriority);
			this.RemoveStates(STATE_ID_JUMPING);
			break;

			case STATE_ID_LANDING:
			landingCooldown.Begin();
			this.RemoveStates(STATE_ID_JUMPING);
			break;

			default:
			break;
		}
	}
	
	/// <summary>Leaves int State.</summary>
	/// <param name="_state">int State that will be left.</param>
	public void OnExitState(int _state)
	{ /*...*/ }

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public void OnStatesAdded(int _state)
	{
		InvokeOnGroundedStateChange(_state, GetJumpIndex());
	}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public void OnStatesRemoved(int _state)
	{ /*...*/ }
#endregion

#region Callbacks:
	/// <summary>Callback invoked when the jump ends.</summary>
	private void OnJumpEnds()
	{
		this.ChangeState(STATE_ID_FALLING);
	}

	/// <summary>Callback invokes when the Landing's cooldown ends.</summary>
	private void OnLandingCooldownEnds()
	{
		this.ChangeState(STATE_ID_GROUNDED);
	}

	/// <summary>Callback invoked when the GravityApplier's grounded state changes.</summary>
	/// <param name="_grounded">New Grounded State.</param>
	private void OnGroundedStateChange(bool _grounded)
	{
		switch(_grounded)
		{
			case true:
			this.RemoveStates(STATE_ID_JUMPING);
			this.ChangeState(STATE_ID_LANDING);
			break;

			case false:
			if(!this.HasStates(STATE_ID_JUMPING))
			{
				this.ChangeState(STATE_ID_FALLING);
				if(currentJumpIndex == 0 && forcesInfo.Length > 1) forcesAppliers[currentJumpIndex].CancelForce();
			}
			break;
		}
	}

	/// <summary>Invokes JumpAbility's State Change.</summary>
	/// <param name="_stateID">State's ID.</param>
	/// <param name="_jumpLevel">Jump's Level [index].</param>
	private void InvokeOnGroundedStateChange(int _stateID, int _jumpLevel)
	{
		if(onJumpStateChange != null) onJumpStateChange(_stateID, _jumpLevel);
	}
#endregion

	/// <returns>Clamped Jump Index.</returns>
	public int GetJumpIndex() { return Mathf.Clamp(currentJumpIndex, 0, forcesInfo.Length - 1); }

	/// <summary>Adds Gravity Scalar given  an _ax.</summary>
	public void AddGravityScalar(float _axis)
	{
		if(!this.HasStates(STATE_ID_FALLING)) return;

		float scalar = 0.0f;

		if(_axis > 0.0f)
		{
			scalar = fallingScale - (Mathf.Abs(_axis) * upwardsScalarAddition);
		
		} else if(_axis < 0.0f)
		{
			scalar = fallingScale + (Mathf.Abs(_axis) * downwardsScalarAddition);
		
		} else if(_axis == 0.0f)
		{
			scalar = fallingScale;
		}

		_scalarWrapper.value = scalar;
		gravityApplier.UpdateBestScale();
	}

	/// <summary>Updates Forces' Appliers.</summary>
	public void UpdateForcesAppliers()
	{
		forcesAppliers = new TimeConstrainedForceApplier2D[forcesInfo.Length];

		for(int i = 0; i < forcesAppliers.Length; i++)
		{
			ForceInformation2D forceInfo = forcesInfo[i];
			forcesAppliers[i] = new TimeConstrainedForceApplier2D(
				accumulator,
				forceInfo.force,
				forceInfo.duration,
				forceInfo.forceMode,
				OnJumpEnds
			);
		}
	}

	/// <summary>Performs Jump's Ability.</summary>
	/// <param name="_axes">Additional Displacement Axes.</param>
	public void Jump(Vector2 _axes)
	{
		if(this.HasStates(STATE_ID_LANDING)) return;

		int limit = forcesAppliers.Length - 1;
		TimeConstrainedForceApplier2D forceApplier = null;

		if(this.HasStates(STATE_ID_GROUNDED) && currentJumpIndex <= 0)
		{
			currentJumpIndex++; /// Equals 0 on first jump
			forceApplier = forcesAppliers[currentJumpIndex];

		} else if(this.HasAnyOfTheStates(STATE_ID_JUMPING | STATE_ID_FALLING) && currentJumpIndex < limit)
		{
			if(currentJumpIndex > 0)
			{ /// If above the 1st jump, cancel the prior jump force.
				forceApplier = forcesAppliers[currentJumpIndex - 1];
				forceApplier.CancelForce();
			}
			currentJumpIndex++;
			forceApplier = forcesAppliers[currentJumpIndex];
		}

		if(forceApplier == null) return;

		gravityApplier.ResetVelocity();

		if(currentJumpIndex >= applyDirectionFromIndex && _axes.sqrMagnitude > 0.0f)
		{
			forceApplier.force = _axes.normalized * forcesInfo[currentJumpIndex].force.magnitude;
		}
		else forceApplier.force = forcesInfo[currentJumpIndex].force;

		forceApplier.ApplyForce();

		if(SFXsIndices != null && SFXsIndices.Length > 0)
		{
			int index = Mathf.Clamp(currentJumpIndex, 0, SFXsIndices.Length);
			AudioController.PlayOneShot(SourceType.SFX, 0, SFXsIndices[index]);
		}

		this.ChangeState(STATE_ID_JUMPING);
	}

	/// <summary>Cancels Jump.</summary>
	public void CancelJump()
	{
		if(!this.HasStates(STATE_ID_JUMPING) || currentJumpIndex >= applyDirectionFromIndex) return;

		TimeConstrainedForceApplier2D forceApplier = forcesAppliers[currentJumpIndex];
		if(forceApplier != null) forceApplier.EndForce();
	}

	/// <summary>Advances Jump's Index.</summary>
	public void AdvanceJumpIndex()
	{
		currentJumpIndex++;
	}

	/// <summary>Predicts Force at its duration [considering its ForceMode].</summary>
	/// <param name="index">Force's Index.</param>
	/// <returns>Force's prediction.</returns>
	public Vector2 PredictForce(int index)
	{
		index = Mathf.Clamp(index, 0, forcesInfo.Length - 1);

		ForceInformation2D forceInfo = forcesInfo[index];
		float t = forceInfo.duration;
		float iT = (t * t * 0.5f); 					/// Time's Integral.
		Vector2 f = forceInfo.force;
		Vector2 g = gravityApplier.gravity * iT * jumpingScale;

		switch(forceInfo.forceMode)
		{
			case ForceMode.Force: 			f =  (f * iT) / rigidbody.mass; 	break;
			case ForceMode.Acceleration: 	f =  f * iT; 						break;
			case ForceMode.Impulse: 		f =  (f * t) / rigidbody.mass; 		break;
			case ForceMode.VelocityChange: 	f =  f * t; 						break;
			default:  						f =  f * t; 						break;
		}

		return f + g;
	}

	/// <returns>Accumulation of the Predictions of all Jump forces.</returns>
	public Vector2 PredictForces()
	{
		Vector2 f = Vector2.zero;

		for(int i = 0; i < forcesInfo.Length; i++)
		{
			f += PredictForce(i);
		}

		return f;
	}

	/// <summary>Cancels Force at given index.</summary>
	/// <param name="_index">Force's Index.</param>
	private void CancelForce(int _index)
	{
		if(forcesAppliers == null) return;

		forcesAppliers[Mathf.Clamp(_index, 0, forcesAppliers.Length - 1)].CancelForce();
	}

	/// <summary>Cancels all forces [all indices].</summary>
	private void CancelForces()
	{
		if(forcesAppliers == null) return;

		for(int i = 0; i < forcesAppliers.Length; i++)
		{
			CancelForce(i);
		}
	}
}
}