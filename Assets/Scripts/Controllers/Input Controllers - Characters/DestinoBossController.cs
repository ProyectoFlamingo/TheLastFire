using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.InputSystem;

namespace Flamingo
{
public class DestinoBossController : MonoBehaviour
{
	public const int FLAG_INPUT_JUMP = 1 << 0; 			/// <summary>Input Flag for Jumping Action.</summary>

	[SerializeField] private InputMaster _inputMaster; 	/// <summary>InputMaster's reference.</summary>
	[SerializeField] private DestinoBoss _destino; 		/// <summary>Destino's reference.</summary>
	[Space(5f)]
	[Header("Inputs' IDs:")]
	[SerializeField] private int _jumpIndex; 			/// <summary>Jump's Input Index.</summary>
	private Vector2 _leftAxes; 							/// <summary>Left Axes.</summary>
	private int _actionFlags; 							/// <summary>Actions' Flags.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets inputMaster property.</summary>
	public InputMaster inputMaster
	{
		get
		{
			if(_inputMaster == null) _inputMaster = new InputMaster();
			return _inputMaster;
		}
		set { _inputMaster = value; }
	}

	/// <summary>Gets and Sets destino property.</summary>
	public DestinoBoss destino
	{
		get { return _destino; }
		set { _destino = value; }
	}

	/// <summary>Gets and Sets jumpIndex property.</summary>
	public int jumpIndex
	{
		get { return _jumpIndex; }
		set { _jumpIndex = value; }
	}

	/// <summary>Gets and Sets leftAxes property.</summary>
	public Vector2 leftAxes
	{
		get { return _leftAxes; }
		set { _leftAxes = value; }
	}

	/// <summary>Gets and Sets actionFlags property.</summary>
	public int actionFlags
	{
		get { return _actionFlags; }
		set { _actionFlags = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>Callback invoked when DestinoBossController's instance is enabled.</summary>
	private void OnEnable()
	{
		inputMaster.Enable();
	}

	/// <summary>Callback invoked when DestinoBossController's instance is disabled.</summary>
	private void OnDisable()
	{
		inputMaster.Disable();
	}

	/// <summary>DestinoBossController's instance initialization.</summary>
	private void Awake()
	{
		if(destino != null) destino.EnablePhysics(true);

		inputMaster.Character_Destino.Jump.performed += OnJumpInputPerformed;
		inputMaster.Character_Destino.Jump.canceled += OnJumpInputCanceled;
		inputMaster.Character_Destino.LeftAxes.performed += OnLeftAxes;
		inputMaster.Character_Destino.LeftAxes.canceled += OnLeftAxesCanceled;
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		if(destino != null) destino.EnablePhysics(true);
	}
	
	/// <summary>DestinoBossController's tick at each frame.</summary>
	private void Update ()
	{
		if(destino == null) return;

		destino.OnLeftAxesChange(leftAxes);
	}

	/// <summary>Updates DestinoBossController's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		if(destino == null) return;
		
		//if((actionFlags | FLAG_INPUT_JUMP) == actionFlags) destino.Jump(leftAxes);
		/*if(leftAxes.x != 0.0f)*/ destino.Move(leftAxes/*.WithY(0.0f)*/);
	}

	private void OnJumpInputPerformed(InputAction.CallbackContext _context)
	{
		actionFlags |= FLAG_INPUT_JUMP;
	}

	private void OnJumpInputCanceled(InputAction.CallbackContext _context)
	{
		actionFlags &= ~FLAG_INPUT_JUMP;
		destino.CancelJump();
	}

	private void OnLeftAxes(InputAction.CallbackContext _context)
	{
		leftAxes = _context.ReadValue<Vector2>();
	}

	private void OnLeftAxesCanceled(InputAction.CallbackContext _context)
	{
		leftAxes = Vector2.zero;
	}
#endregion
}
}