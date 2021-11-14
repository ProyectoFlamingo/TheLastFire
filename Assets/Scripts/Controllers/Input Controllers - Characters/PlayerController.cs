using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class PlayerController : Singleton<PlayerController>
{
	public const int FLAG_INPUT_JUMP = 1 << 0; 						/// <summary>Input flag for the jumping.</summary>	
	public const int FLAG_INPUT_DASH = 1 << 1; 						/// <summary>Input flag for the dashing.</summary>
	public const int FLAG_INPUT_ATTACK_SWORD = 1 << 2; 				/// <summary>Input flag for the sword attacking.</summary>
	public const int FLAG_INPUT_CHARGING_FIRE_FRONTAL = 1 << 3; 	/// <summary>Input flag for the frontal fire charging.</summary>
	public const int FLAG_INPUT_CHARGING_FIRE = 1 << 4; 			/// <summary>Input flag for the fire charging.</summary>

	[SerializeField] private Mateo _mateo; 							/// <summary>Mateo's Reference.</summary>
	[Space(5f)]
	[Header("Axes Threshold's Settings:")]
	[SerializeField]
	[Range(0.0f, 0.9f)] private float _movementAxesThreshold; 		/// <summary>Movement axes' threshold that must be passed in order for the player to displace at full speed.</summary>
	[SerializeField]
	[Range(0.0f, 0.9f)] private float _chargeFireAxesThreshold; 	/// <summary>Charge Fire axes' threshold that must be passed in order for the player to charge the flame.</summary>
	[SerializeField]
	[Range(0.0f, 0.9f)] private float _rightDeadZoneThreshold; 		/// <summary>Dead Zone's Threshold.</summary>
	[Space(5f)]
	[Header("Input Settings:")]
	[SerializeField] private int _jumpID; 							/// <summary>Jump's Input ID.</summary>
	[SerializeField] private int _dashID; 							/// <summary>Dash's Input ID.</summary>
	[SerializeField] private int _swordAttackID; 					/// <summary>Sword Attack's Input ID.</summary>
	[SerializeField] private int _frontalFireConjuringID0; 			/// <summary>Frontal Fire Conjuring's Input ID 0.</summary>
	[SerializeField] private int _frontalFireConjuringID1; 			/// <summary>Frontal Fire Conjuring's Input ID 1.</summary>
	[SerializeField] private int _pauseID; 							/// <summary>Pause's Input ID.</summary>
	[SerializeField] private int _resetID; 							/// <summary>Reset's Input ID.</summary>
	[Space(5f)]
	[SerializeField] private float _lowSpeedScalar; 				/// <summary>Low Speed's Scalar.</summary>
	private int _inputFlags; 										/// <summary>Input Flags.</summary>
	private Vector2 _leftAxes; 										/// <summary>Left Axes.</summary>
	private Vector2 _rightAxes; 									/// <summary>Right Axes.</summary>
	private Vector2 previousRightAxes;

#region Getters/Setters:
	/// <summary>Gets and Sets mateo property.</summary>
	public Mateo mateo
	{
		get { return _mateo; }
		set { _mateo = value; }
	}

	/// <summary>Gets and Sets movementAxesThreshold property.</summary>
	public float movementAxesThreshold
	{
		get { return _movementAxesThreshold; }
		set { _movementAxesThreshold = value; }
	}

	/// <summary>Gets and Sets chargeFireAxesThreshold property.</summary>
	public float chargeFireAxesThreshold
	{
		get { return _chargeFireAxesThreshold; }
		set { _chargeFireAxesThreshold = value; }
	}

	/// <summary>Gets and Sets rightDeadZoneThreshold property.</summary>
	public float rightDeadZoneThreshold
	{
		get { return _rightDeadZoneThreshold; }
		set { _rightDeadZoneThreshold = value; }
	}

	/// <summary>Gets and Sets lowSpeedScalar property.</summary>
	public float lowSpeedScalar
	{
		get { return _lowSpeedScalar; }
		set { _lowSpeedScalar = value; }
	}

	/// <summary>Gets and Sets jumpID property.</summary>
	public int jumpID
	{
		get { return _jumpID; }
		set { _jumpID = value; }
	}

	/// <summary>Gets and Sets dashID property.</summary>
	public int dashID
	{
		get { return _dashID; }
		set { _dashID = value; }
	}

	/// <summary>Gets and Sets swordAttackID property.</summary>
	public int swordAttackID
	{
		get { return _swordAttackID; }
		set { _swordAttackID = value; }
	}

	/// <summary>Gets and Sets frontalFireConjuringID0 property.</summary>
	public int frontalFireConjuringID0
	{
		get { return _frontalFireConjuringID0; }
		set { _frontalFireConjuringID0 = value; }
	}

	/// <summary>Gets and Sets frontalFireConjuringID1 property.</summary>
	public int frontalFireConjuringID1
	{
		get { return _frontalFireConjuringID1; }
		set { _frontalFireConjuringID1 = value; }
	}

	/// <summary>Gets and Sets pauseID property.</summary>
	public int pauseID
	{
		get { return _pauseID; }
		set { _pauseID = value; }
	}

	/// <summary>Gets and Sets resetID property.</summary>
	public int resetID
	{
		get { return _resetID; }
		set { _resetID = value; }
	}

	/// <summary>Gets and Sets inputFlags property.</summary>
	public int inputFlags
	{
		get { return _inputFlags; }
		set { _inputFlags = value; }
	}

	/// <summary>Gets and Sets leftAxes property.</summary>
	public Vector2 leftAxes
	{
		get { return _leftAxes; }
		set { _leftAxes = value; }
	}

	/// <summary>Gets and Sets rightAxes property.</summary>
	public Vector2 rightAxes
	{
		get { return _rightAxes; }
		set { _rightAxes = value; }
	}
#endregion
	
	/// <summary>PlayerController's tick at each frame.</summary>
	private void Update()
	{
		/// Pause:
		if(InputController.InputEnds(pauseID)) Game.OnPause();
		
		if(mateo == null || !mateo.HasStates(Character.ID_STATE_ALIVE) || Game.state != GameState.Playing) return;

		leftAxes = InputController.Instance.leftAxes;
		rightAxes = InputController.Instance.rightAxes;

		float rightAxesMagnitude = InputController.GetAxesMagnitude(rightAxes);

		/// Jump Evaluation:
		if(InputController.InputBegin(jumpID)) inputFlags |= FLAG_INPUT_JUMP;
		else inputFlags &= ~FLAG_INPUT_JUMP;

		if(InputController.InputEnds(jumpID)) mateo.CancelJump();

		/// Dash Evaluation:
		if(InputController.InputBegin(dashID)) inputFlags |= FLAG_INPUT_DASH;
		else inputFlags &= ~FLAG_INPUT_DASH;

		/// Sword Attack Evaluation:
		if(InputController.InputBegin(swordAttackID)) inputFlags |= FLAG_INPUT_ATTACK_SWORD;
		else inputFlags &= ~FLAG_INPUT_ATTACK_SWORD;

		/// Frontal-Fire Evaluation:
		if((inputFlags | FLAG_INPUT_CHARGING_FIRE_FRONTAL) != inputFlags
		&& (InputController.InputBegin(frontalFireConjuringID0) || InputController.InputBegin(frontalFireConjuringID1)))
		{
			inputFlags |= FLAG_INPUT_CHARGING_FIRE_FRONTAL;
		
		} else if((inputFlags | FLAG_INPUT_CHARGING_FIRE_FRONTAL) == inputFlags
		&& (InputController.InputEnds(frontalFireConjuringID0) || InputController.InputEnds(frontalFireConjuringID1)))
		{
			if(((inputFlags | FLAG_INPUT_CHARGING_FIRE_FRONTAL) == inputFlags) && (rightAxesMagnitude <= rightDeadZoneThreshold))
			mateo.ReleaseFire(mateo.directionTowardsBackground);

			inputFlags &= ~FLAG_INPUT_CHARGING_FIRE_FRONTAL;
		}

		/// Jump Action:
		if((inputFlags | FLAG_INPUT_JUMP) == inputFlags)
		{
			mateo.Jump(leftAxes);
			inputFlags &= ~FLAG_INPUT_JUMP;
		}

/*#region DashIsDead:
		/// Dash Action:
		if((inputFlags | FLAG_INPUT_DASH) == inputFlags)
		{
			mateo.Dash();
			inputFlags &= ~FLAG_INPUT_DASH;
		}
#endregion*/

		/// Sword Attack Action:
		if((inputFlags | FLAG_INPUT_ATTACK_SWORD) == inputFlags)
		{
			mateo.SwordAttack(leftAxes);
			inputFlags &= ~FLAG_INPUT_ATTACK_SWORD;
		}

		/// Fire Charge Evaluation:
		if(rightAxesMagnitude >= chargeFireAxesThreshold)
		{
			inputFlags |= FLAG_INPUT_CHARGING_FIRE;
			mateo.ChargeFire(rightAxes);
			
			previousRightAxes = rightAxes;
		}
		else
		{
			if((inputFlags | FLAG_INPUT_CHARGING_FIRE_FRONTAL) == inputFlags)
			{
				mateo.ChargeFire(mateo.directionTowardsBackground);
			
			} else if((inputFlags | FLAG_INPUT_CHARGING_FIRE) == inputFlags)
			{
				inputFlags &= ~FLAG_INPUT_CHARGING_FIRE;
				mateo.ReleaseFire(previousRightAxes.normalized);
			
			} else if((inputFlags | FLAG_INPUT_CHARGING_FIRE) != inputFlags)
			{
				inputFlags &= ~FLAG_INPUT_CHARGING_FIRE;
				mateo.DischargeFire();
			
			}
		}

		/// Reset:
		if(InputController.InputEnds(resetID)) Game.ResetScene();

		mateo.OnLeftAxesChange(leftAxes);
		mateo.OnRightAxesChange(rightAxes);

/*#region PreviousFireEvaluation:
		/// Fire Release Evaluation:
		if((inputFlags | FLAG_INPUT_CHARGING_FIRE) == inputFlags)
		{
			float dot = Vector2.Dot(rightAxes.normalized, previousRightAxes);
		
			if(dot != 1.0f)
			{
				inputFlags &= ~FLAG_INPUT_CHARGING_FIRE;
				mateo.DischargeFire();
			
			} else if(dot == 1.0f && rightAxesMagnitude <= rightDeadZoneThreshold)
			{
				inputFlags &= ~FLAG_INPUT_CHARGING_FIRE;
				mateo.ReleaseFire(rightAxes);
			}

			previousRightAxes = rightAxes;
		}
#endregion*/

/*#region TESTs
		if(InputController.InputBegin(3)) mateo.Hurt();
		if(InputController.InputBegin(4)) mateo.Kill();
		if(InputController.InputBegin(5)) mateo.Revive();
#endregion*/
	}

	/// <summary>Updates PlayerController's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		if(mateo == null || !mateo.HasStates(Character.ID_STATE_ALIVE) || Game.state != GameState.Playing) return;
		
		if(leftAxes.x != 0.0f) mateo.Move(leftAxes.WithY(0.0f), Mathf.Abs(leftAxes.x) > movementAxesThreshold ? 1.0f : lowSpeedScalar);
	}
}
}