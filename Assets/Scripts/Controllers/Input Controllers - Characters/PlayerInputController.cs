using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Voidless;

namespace Flamingo
{
public enum CharacterControllerMap
{
	None,
	Mateo
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
	[Space(5f)]
	[Header("Character Controllers:")]
	[SerializeField] private MateoController _mateoController; 				/// <summary>Mateo's Controller.</summary>
	private PlayerInput _playerInput; 										/// <summary>PlayerInput's Component.</summary>
	private CharacterControllerMap _currentCharacterControl; 				/// <summary>Current Character's Control.</summary>

	/// <summary>Gets mateoController property.</summary>
	public MateoController mateoController { get { return _mateoController; } }

	/// <summary>Gets playerInput Component.</summary>
	public PlayerInput playerInput
	{ 
		get
		{
			if(_playerInput == null) _playerInput = GetComponent<PlayerInput>();
			return _playerInput;
		}
	}

	/// <summary>Gets and Sets currentCharacterControl property.</summary>
	public CharacterControllerMap currentCharacterControl
	{
		get { return _currentCharacterControl; }
		protected set { _currentCharacterControl = value; }
	}

	/// <summary>Callback invoked when PlayerInputController's instance is enabled.</summary>
	private void OnEnable()
	{
		Enable(true);
	}

	/// <summary>Callback invoked when PlayerInputController's instance is disabled.</summary>
	private void OnDisable()
	{
		Enable(false);

		StringBuilder builder = new StringBuilder();

		builder.Append("Player ");
		builder.Append(playerInput.playerIndex.ToString());
		builder.Append(" Disabled.");

		Debug.Log(builder.ToString());
	}

	/// <summary>TheLastFirePlayerInput's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		mateoController.playerInput = playerInput;
	}

	/// <summary>Assigns Character to Mateo's Controller.</summary>
	/// <param name="_mateo">Mateo's Instance.</param>
	public void AssignCharacterToMateoController(Mateo _mateo, bool _enable = true)
	{
		if(_mateo == null) return;

		mateoController.character = _mateo;
		if(_enable) mateoController.enabled = true;
	}

	/// <summary>Changes Controller Map.</summary>
	/// <param name="_map">CharacterControllerMap's enum.</param>
	public void ChangeControllerMap(CharacterControllerMap _map)
	{
		/// \TODO Disable the rest of possible Controller Maps...

		switch(_map)
		{
			case CharacterControllerMap.Mateo:
			playerInput.SwitchCurrentActionMap(mateoController.actionMapName);
			mateoController.enabled = true;
			break;
		}

		currentCharacterControl = _map;
	}

	/// <summary>Enables all controllers.</summary>
	/// <param name="_enable">Enable? True by default.</param>
	public void Enable(bool _enable = true)
	{
		if(_enable && !playerInput.enabled) playerInput.ActivateInput();
		mateoController.enabled = _enable;
	}

	/// <returns>String representing PlayerInputController.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();
		
		builder.Append("PlayerInput: ");
		builder.AppendLine(playerInput.ToString());
		builder.Append("Current Character Control: ");
		builder.AppendLine(currentCharacterControl.ToString());
		builder.Append("Mateo Controller: ");
		builder.AppendLine(mateoController.ToString());

		return builder.ToString();
	}
}
}