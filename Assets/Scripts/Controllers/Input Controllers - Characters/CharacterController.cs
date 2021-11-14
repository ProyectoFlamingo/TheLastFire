using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Voidless;

namespace Flamingo
{
public enum ControllerSchemeType
{
	Character,
	UI
}

//[RequireComponent(typeof(PlayerInput))]
public class CharacterController<T> : MonoBehaviour where T : Character
{
	[SerializeField] private PlayerInput _playerInput; 		/// <summary>PlayerInput's Component.</summary>
	[Space(5f)]
	[Header("UI Input's Attributes")]
	[SerializeField] private string _UIActionMapName; 		/// <summary>UI's ActionMap's Name.</summary>
	[SerializeField] private string _UIMoveID; 				/// <summary>UI Move's Input's ID.</summary>
	[SerializeField] private string _UISubmitID; 			/// <summary>UI Submit's Input's ID.</summary>
	[SerializeField] private string _UICancelID; 			/// <summary>UI Cancel's Input's ID.</summary>
	[Space(5f)]
	[SerializeField] private T _character; 					/// <summary>Character controlled by this CharacterController.</summary>
	[SerializeField] private string _actionMapName; 		/// <summary>Assigned InputActionMap's name.</summary>
	[Space(5f)]
	[SerializeField] private string _pauseID; 				/// <summary>Pause's ID.</summary>
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
	private InputActionMap _UIActionMap; 					/// <summary>UI's ActionMap.</summary>
	private InputAction _UIMoveAction; 						/// <summary>UI's Move Input's Action.</summary>
	private InputAction _UISubmitAction; 					/// <summary>UI's Submit Input's Action.</summary>
	private InputAction _UICancelAction; 					/// <summary>UI's Cancel Input's Action.</summary>
	private InputAction _leftAxesAction; 					/// <summary>Left-Axes' Input's Action.</summary>
	private InputAction _rightAxesAction; 					/// <summary>Right-Axes' Input's Action.</summary>
	private InputAction _pauseAction; 						/// <summary>Pause's Input Action.</summary>
	private Vector2 _leftAxes; 								/// <summary>Left-Axes.</summary>
	private Vector2 _rightAxes; 							/// <summary>Right-Axes.</summary>
	private Vector2 _previousLeftAxes; 						/// <summary>Previous' Left-Axes.</summary>
	private Vector2 _previousRightAxes; 					/// <summary>Previous' Right-Axes.</summary>
	private int _inputFlags; 								/// <summary>Input's Flags.</summary>
	private float _rightAxesMagnitude; 						/// <summary>Right-Axes' Magnitude.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets character property.</summary>
	public T character
	{
		get { return _character; }
		set { _character = value; }
	}

	/// <summary>Gets actionMapName property.</summary>
	public string actionMapName { get { return _actionMapName; } }

	/// <summary>Gets UIActionMapName property.</summary>
	public string UIActionMapName { get { return _UIActionMapName; } }

	/// <summary>Gets UIMoveID property.</summary>
	public string UIMoveID { get { return _UIMoveID; } }

	/// <summary>Gets UISubmitID property.</summary>
	public string UISubmitID { get { return _UISubmitID; } }

	/// <summary>Gets UICancelID property.</summary>
	public string UICancelID { get { return _UICancelID; } }

	/// <summary>Gets pauseID property.</summary>
	public string pauseID { get { return _pauseID; } }

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

	/// <summary>Gets and Sets UIActionMap property.</summary>
	public InputActionMap UIActionMap
	{
		get { return _UIActionMap; }
		protected set { _UIActionMap = value; }
	}

	/// <summary>Gets and Sets UIMoveAction property.</summary>
	public InputAction UIMoveAction
	{
		get { return _UIMoveAction; }
		protected set { _UIMoveAction = value; }
	}

	/// <summary>Gets and Sets UISubmitAction property.</summary>
	public InputAction UISubmitAction
	{
		get { return _UISubmitAction; }
		protected set { _UISubmitAction = value; }
	}

	/// <summary>Gets and Sets UICancelAction property.</summary>
	public InputAction UICancelAction
	{
		get { return _UICancelAction; }
		protected set { _UICancelAction = value; }
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

	/// <summary>Gets and Sets pauseAction property.</summary>
	public InputAction pauseAction
	{
		get { return _pauseAction; }
		protected set { _pauseAction = value; }
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

	/// <summary>Gets and Sets playerInput Component.</summary>
	public PlayerInput playerInput
	{ 
		get
		{
			if(_playerInput == null) _playerInput = GetComponent<PlayerInput>();
			return _playerInput;
		}
		set { _playerInput = value; }
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
		actionMap = playerInput.actions.FindActionMap(actionMapName, true);;
		UIActionMap = playerInput.actions.FindActionMap(UIActionMapName, true);
	}

	/// <summary>Changes Controller Scheme.</summary>
	/// <param name="_type">Type of Controller Scheme.</param>
	public void ChangeControllerScheme(ControllerSchemeType _type)
	{
		string actionMapID = string.Empty;

		switch(_type)
		{
			case ControllerSchemeType.Character:
			actionMapID = actionMapName;
			break;

			case ControllerSchemeType.UI:
			actionMapID = UIActionMapName;
			break;
		}

		playerInput.SwitchCurrentActionMap(actionMapID);
	}

	/// <summary>Sets Input's Actions.</summary>
	protected virtual void SetInputActions()
	{
		UIMoveAction = UIActionMap.FindAction(UIMoveID, true);
		UISubmitAction = UIActionMap.FindAction(UISubmitID, true);
		UICancelAction = UIActionMap.FindAction(UICancelID, true);
		leftAxesAction = actionMap.FindAction(leftAxesID, true);
		rightAxesAction = actionMap.FindAction(rightAxesID, true);
		pauseAction = actionMap.FindAction(pauseID, true);

		pauseAction.performed += OnPauseActionPerformed;
	}

	/// <summary>Callback invoked when the Jump's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	protected virtual void OnPauseActionPerformed(InputAction.CallbackContext _context)
	{
		Game.OnPause();
	}
}
}