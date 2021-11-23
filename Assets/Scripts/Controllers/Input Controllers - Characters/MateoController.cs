using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Voidless;

namespace Flamingo
{
public class MateoController : CharacterController<Mateo>
{
	public const int FLAG_INPUT_JUMP = 1 << 0; 						/// <summary>Input flag for the jumping.</summary>	
	public const int FLAG_INPUT_DASH = 1 << 1; 						/// <summary>Input flag for the dashing.</summary>
	public const int FLAG_INPUT_ATTACK_SWORD = 1 << 2; 				/// <summary>Input flag for the sword attacking.</summary>
	public const int FLAG_INPUT_CHARGING_FIRE_FRONTAL = 1 << 3; 	/// <summary>Input flag for the frontal fire charging.</summary>
	public const int FLAG_INPUT_CHARGING_FIRE = 1 << 4; 			/// <summary>Input flag for the fire charging.</summary>
	public const int FLAG_INPUT_CROUCH = 1 << 5; 					/// <summary>Input flag for the crouching.</summary>

	[Space(5f)]
	[Header("Input's Actions:")]
	[SerializeField] private string _jumpID; 						/// <summary>Jump's Input Action's ID.</summary>
	[SerializeField] private string _swordAttackID; 				/// <summary>Sword Attack's Input Action's ID.</summary>
	[SerializeField] private string _frontalFireConjuringID; 		/// <summary>Frontal Fire's Conjuring's Input Action's ID.</summary>
	[SerializeField] private string _crouchActionID; 				/// <summary>Crouch's Input Action's ID.</summary>
	[Space(5f)]
	[Header("Axes' Thresholds:")]
	[Range(0.0f, 0.9f)]
	[SerializeField] private float _movementAxesThreshold; 			/// <summary>Movement Axes' Magnitude Threshold.</summary>
	[Range(0.0f, 0.9f)]
	[SerializeField] private float _fireChargeAxesThreshold; 		/// <summary>Fire's Charge Axes' Magnitude Threshold.</summary>
	[Space(5f)]
	[Range(0.0f, 1.0f)]
	[SerializeField] private float _lowSpeedScalar; 				/// <summary>Movement's Low Speed Scalar.</summary>
	private InputAction _jumpAction; 								/// <summary>Jump's Input Action.</summary>
	private InputAction _swordAttackAction; 						/// <summary>Sword Attack's Input Action.</summary>
	private InputAction _frontalFireConjuringAction; 				/// <summary>Frontal Fire's Conjuring's Input Action.</summary>
	private InputAction _crouchAction; 								/// <summary>Crouch's Input Action.</summary>
	private Vector2 _fireDirection; 								/// <summary>Fire's Direction.</summary>

#region Getters/Setters:
	/// <summary>Gets jumpID property.</summary>
	public string jumpID { get { return _jumpID; } }

	/// <summary>Gets swordAttackID property.</summary>
	public string swordAttackID { get { return _swordAttackID; } }

	/// <summary>Gets frontalFireConjuringID property.</summary>
	public string frontalFireConjuringID { get { return _frontalFireConjuringID; } }

	/// <summary>Gets crouchActionID property.</summary>
	public string crouchActionID { get { return _crouchActionID; } }

	/// <summary>Gets movementAxesThreshold property.</summary>
	public float movementAxesThreshold { get { return _movementAxesThreshold; } }

	/// <summary>Gets fireChargeAxesThreshold property.</summary>
	public float fireChargeAxesThreshold { get { return _fireChargeAxesThreshold; } }

	/// <summary>Gets lowSpeedScalar property.</summary>
	public float lowSpeedScalar { get { return _lowSpeedScalar; } }

	/// <summary>Gets and Sets jumpAction property.</summary>
	public InputAction jumpAction
	{
		get { return _jumpAction; }
		protected set { _jumpAction = value; }
	}

	/// <summary>Gets and Sets swordAttackAction property.</summary>
	public InputAction swordAttackAction
	{
		get { return _swordAttackAction; }
		protected set { _swordAttackAction = value; }
	}

	/// <summary>Gets and Sets frontalFireConjuringAction property.</summary>
	public InputAction frontalFireConjuringAction
	{
		get { return _frontalFireConjuringAction; }
		protected set { _frontalFireConjuringAction = value; }
	}

	/// <summary>Gets and Sets crouchAction property.</summary>
	public InputAction crouchAction
	{
		get { return _crouchAction; }
		protected set { _crouchAction = value; }
	}

	/// <summary>Gets and Sets fireDirection property.</summary>
	public Vector2 fireDirection
	{
		get { return _fireDirection; }
		protected set { _fireDirection = value; }
	}
#endregion

	/// <summary>Sets Input's Actions.</summary>
	protected override void SetInputActions()
	{
		base.SetInputActions();

		jumpAction = actionMap.FindAction(jumpID, true);
		swordAttackAction = actionMap.FindAction(swordAttackID, true);
		frontalFireConjuringAction = actionMap.FindAction(frontalFireConjuringID, true);
		crouchAction = actionMap.FindAction(crouchActionID, true);

		jumpAction.performed += OnJumpActionPerformed;
		jumpAction.canceled += OnJumpActionCanceled;
		swordAttackAction.performed += OnSwordAttackActionPerformed;
		frontalFireConjuringAction.performed += OnFrontalFireConjuringActionPerformed;
		frontalFireConjuringAction.canceled += OnFrontalFireConjuringActionCanceled;
		crouchAction.performed += OnCrouchActionPerformed;
		crouchAction.canceled += OnCrouchActionCanceled;
	}

	/// <summary>Callback internally invoked when the Axes are updated, but before the previous axes' values get updated.</summary>
	protected override void OnAxesUpdated()
	{
		if(character == null || Game.state != GameState.Playing) return;

		if((inputFlags | FLAG_INPUT_JUMP) == inputFlags)
		{
			character.Jump(leftAxes);
			inputFlags &= ~FLAG_INPUT_JUMP;
		}

		if(rightAxesMagnitude >= fireChargeAxesThreshold)
		{
			inputFlags |= FLAG_INPUT_CHARGING_FIRE;
			character.ChargeFire(rightAxes);

			fireDirection = rightAxes;
		}
		else
		{
			if((inputFlags | FLAG_INPUT_CHARGING_FIRE_FRONTAL) == inputFlags)
			{
				character.ChargeFire(character.directionTowardsBackground);
			
			} else if((inputFlags | FLAG_INPUT_CHARGING_FIRE) == inputFlags)
			{
				inputFlags &= ~FLAG_INPUT_CHARGING_FIRE;
				character.ReleaseFire(fireDirection.normalized);
			
			} else if((inputFlags | FLAG_INPUT_CHARGING_FIRE) != inputFlags)
			{
				inputFlags &= ~FLAG_INPUT_CHARGING_FIRE;
				character.DischargeFire();
			
			}
		}

		character.OnLeftAxesChange(leftAxes);
		character.OnRightAxesChange(rightAxes);
	}

	/// <summary>Updates CharacterController's instance at each Physics Thread's frame.</summary>
	protected virtual void FixedUpdate()
	{
		if(character == null || Game.state != GameState.Playing) return;

		Vector2 movement = leftAxes.WithY(0.0f);
		
		if(movement.x != 0.0f)
		character.Move(movement, Mathf.Abs(movement.x) > movementAxesThreshold ? 1.0f : lowSpeedScalar);
	}

#region Callbacks:
	/// <summary>Callback invoked when the Jump's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnJumpActionPerformed(InputAction.CallbackContext _context)
	{
		if(Game.state != GameState.Playing) return;
		inputFlags |= FLAG_INPUT_JUMP;
	}

	/// <summary>Callback invoked when the Jump's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnJumpActionCanceled(InputAction.CallbackContext _context)
	{
		//if(Game.state != GameState.Playing) return;

		if(character != null)
		character.CancelJump();
		
		inputFlags &= ~FLAG_INPUT_JUMP;
	}

	/// <summary>Callback invoked when the Sword Attack's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnSwordAttackActionPerformed(InputAction.CallbackContext _context)
	{
		if(Game.state != GameState.Playing) return;

		if(character != null) character.SwordAttack(leftAxes);
	}

	/// <summary>Callback invoked when the Frontal-Fire Conjuring's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnFrontalFireConjuringActionPerformed(InputAction.CallbackContext _context)
	{
		if(Game.state != GameState.Playing) return;

		if((inputFlags | FLAG_INPUT_CHARGING_FIRE_FRONTAL) == inputFlags
		&& rightAxesMagnitude < rightDeadZoneRadius
		&& character != null)
		character.ChargeFire(character.directionTowardsBackground);
		
		inputFlags |= FLAG_INPUT_CHARGING_FIRE_FRONTAL;

	}

	/// <summary>Callback invoked when the Frontal-Fire Conjuring's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnFrontalFireConjuringActionCanceled(InputAction.CallbackContext _context)
	{
		//if(Game.state != GameState.Playing) return;

		if((inputFlags | FLAG_INPUT_CHARGING_FIRE_FRONTAL) == inputFlags && character != null)
		character.ReleaseFire(character.directionTowardsBackground);
		
		inputFlags &= ~FLAG_INPUT_CHARGING_FIRE_FRONTAL;
	}

	/// <summary>Callback invoked when the Crouch's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnCrouchActionPerformed(InputAction.CallbackContext _context)
	{
		if(Game.state != GameState.Playing) return;

		character.Crouch();
		inputFlags |= FLAG_INPUT_CROUCH;
	}

	/// <summary>Callback invoked when the Crouch's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnCrouchActionCanceled(InputAction.CallbackContext _context)
	{
		//if(Game.state != GameState.Playing) return;

		inputFlags &= ~FLAG_INPUT_CROUCH;
	}
#endregion
}
}