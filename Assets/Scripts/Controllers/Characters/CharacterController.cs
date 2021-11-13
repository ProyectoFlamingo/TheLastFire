using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(PlayerInput))]
public class CharacterController<T> : MonoBehaviour
{
	[SerializeField] private T _character; 					/// <summary>Character controlled by this CharacterController.</summary>
	[SerializeField] private string _actionMapName; 		/// <summary>Assigned InputActionMap's name.</summary>
	[Space(5f)]
	[Header("Axes' Input Actions:")]
	[SerializeField] private string _leftAxesID; 			/// <summary>Left-Axes' Input's ID.</summary>
	[SerializeField] private string _rightAxesID; 			/// <summary>Right-Axes' Input's ID.</summary>
	[Space(5f)]
	[Header("Axes' Settings:")]
	[Range(0.0f, 0.9f)]
	[SerializeField] private float _leftDeadZoneRadius; 	/// <summary>Left-Axes' Dead-Zone's Radius.</summary>
	[Range(0.0f, 0.9f)]
	[SerializeField] private float _rightDeadZoneRadius; 	/// <summary>Right-Axes' Dead-Zone's Radius.</summary>
	private InputActionMap _actionMap; 						/// <summary>Input's ActionMap.</summary>
	private InputAction _leftAxesAction; 					/// <summary>Left-Axes' Input's Action.</summary>
	private InputAction _rightAxesAction; 					/// <summary>Right-Axes' Input's Action.</summary>
	private Vector2 _leftAxes; 								/// <summary>Left-Axes.</summary>
	private Vector2 _rightAxes; 							/// <summary>Right-Axes.</summary>
	private Vector2 _previousLeftAxes; 						/// <summary>Previous' Left-Axes.</summary>
	private Vector2 _previousRightAxes; 					/// <summary>Previous' Right-Axes.</summary>
	private int _inputFlags; 								/// <summary>Input's Flags.</summary>
	private float _rightAxesMagnitude; 						/// <summary>Right-Axes' Magnitude.</summary>
	private PlayerInput _playerInput; 						/// <summary>PlayerInput's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets character property.</summary>
	public T character
	{
		get { return _character; }
		set { _character = value; }
	}

	/// <summary>Gets actionMapName property.</summary>
	public string actionMapName { get { return _actionMapName; } }

	/// <summary>Gets leftAxesID property.</summary>
	public string leftAxesID { get { return _leftAxesID; } }

	/// <summary>Gets rightAxesID property.</summary>
	public string rightAxesID { get { return _rightAxesID; } }

	/// <summary>Gets leftDeadZoneRadius property.</summary>
	public float leftDeadZoneRadius { get { return _leftDeadZoneRadius; } }

	/// <summary>Gets rightDeadZoneRadius property.</summary>
	public float rightDeadZoneRadius { get { return _rightDeadZoneRadius; } }

	/// <summary>Gets and Sets rightAxesMagnitude property.</summary>
	public float rightAxesMagnitude
	{
		get { return _rightAxesMagnitude; }
		protected set { _rightAxesMagnitude = value; }
	}

	/// <summary>Gets and Sets actionMap property.</summary>
	public InputActionMap actionMap
	{
		get { return _actionMap; }
		protected set { _actionMap = value; }
	}

	/// <summary>Gets and Sets leftAxesAction property.</summary>
	public InputAction leftAxesAction
	{
		get { return _leftAxesAction; }
		protected set { _leftAxesAction = value; }
	}

	/// <summary>Gets and Sets rightAxesAction property.</summary>
	public InputAction rightAxesAction
	{
		get { return _rightAxesAction; }
		protected set { _rightAxesAction = value; }
	}

	/// <summary>Gets and Sets leftAxes property.</summary>
	public Vector2 leftAxes
	{
		get { return _leftAxes; }
		protected set { _leftAxes = value; }
	}

	/// <summary>Gets and Sets rightAxes property.</summary>
	public Vector2 rightAxes
	{
		get { return _rightAxes; }
		protected set { _rightAxes = value; }
	}

	/// <summary>Gets and Sets previousLeftAxes property.</summary>
	public Vector2 previousLeftAxes
	{
		get { return _previousLeftAxes; }
		protected set { _previousLeftAxes = value; }
	}

	/// <summary>Gets and Sets previousRightAxes property.</summary>
	public Vector2 previousRightAxes
	{
		get { return _previousRightAxes; }
		protected set { _previousRightAxes = value; }
	}

	/// <summary>Gets and Sets inputFlags property.</summary>
	public int inputFlags
	{
		get { return _inputFlags; }
		protected set { _inputFlags = value; }
	}

	/// <summary>Gets playerInput Component.</summary>
	public PlayerInput playerInput
	{ 
		get
		{
			if(_playerInput == null) _playerInput = GetComponent<PlayerInput>();
			return _playerInput;
		}
	}
#endregion

	/// <summary>Callback invoked when CharacterController's instance is enabled.</summary>
	private void OnEnable()
	{
		actionMap.Enable();
	}

	/// <summary>Callback invoked when CharacterController's instance is disabled.</summary>
	private void OnDisable()
	{
		actionMap.Disable();
	}

	/// <summary>Resets CharacterController's instance to its default values.</summary>
	public virtual void Reset()
	{
		leftAxes = Vector2.zero;
		rightAxes = Vector2.zero;
		previousLeftAxes = Vector2.zero;
		previousRightAxes = Vector2.zero;
		rightAxesMagnitude = 0.0f;
		inputFlags = 0;
	}

	/// <summary>CharacterController's instance initialization.</summary>
	protected virtual void Awake()
	{
		SetInputActionMap();
		SetInputActions();
	}
	
	/// <summary>CharacterController's tick at each frame.</summary>
	protected virtual void Update ()
	{
		if(leftAxesAction != null) leftAxes = leftAxesAction.ReadValue<Vector2>();
		if(rightAxesAction != null)
		{
			rightAxes = rightAxesAction.ReadValue<Vector2>();
			rightAxesMagnitude = rightAxes.magnitude;
		}

		if(leftAxes.sqrMagnitude < (leftDeadZoneRadius * leftDeadZoneRadius)) leftAxes = Vector2.zero;
		if(rightAxes.sqrMagnitude < (rightDeadZoneRadius * rightDeadZoneRadius)) rightAxes = Vector2.zero;

		OnAxesUpdated();

		previousLeftAxes = leftAxes;
		previousRightAxes = rightAxes;
	}

	/// <summary>Updates CharacterController's instance at each Physics Thread's frame.</summary>
	protected virtual void FixedUpdate()
	{
		
	}

	/// <summary>Callback internally invoked when the Axes are updated, but before the previous axes' values get updated.</summary>
	protected virtual void OnAxesUpdated() { /*...*/ }

	/// <summary>Sets Input's Action Map.</summary>
	private void SetInputActionMap()
	{
		playerInput.SwitchCurrentActionMap(actionMapName);
		playerInput.defaultActionMap = actionMapName;
		actionMap = playerInput.currentActionMap;
	}

	/// <summary>Sets Input's Actions.</summary>
	protected virtual void SetInputActions()
	{
		leftAxesAction = actionMap.FindAction(leftAxesID, true);
		rightAxesAction = actionMap.FindAction(rightAxesID, true);
	}
}
}