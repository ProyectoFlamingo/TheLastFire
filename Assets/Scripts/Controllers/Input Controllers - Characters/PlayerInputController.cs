using System;
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
	[SerializeField] private MateoController _mateoController; 	/// <summary>Mateo's Controller.</summary>
	private PlayerInput _playerInput; 							/// <summary>PlayerInput's Component.</summary>

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

	/// <summary>TheLastFirePlayerInput's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		mateoController.playerInput = playerInput;
	}

	/*/// <returns>Current Character's Controller.</returns>
	public CharacterController<Character> GetCurrentController()
	{
		return mateoController;
	}*/

	/// <summary>Assigns Character to Mateo's Controller.</summary>
	/// <param name="_mateo">Mateo's Instance.</param>
	public void AssignCharacterToMateoController(Mateo _mateo)
	{
		if(_mateo == null) return;

		mateoController.character = _mateo;
		mateoController.enabled = true;
	}

	/// <summary>Changes Controller Map.</summary>
	/// <param name="_map">CharacterControllerMap's enum.</param>
	public void ChangeControllerMap(CharacterControllerMap _map)
	{
		switch(_map)
		{
			case CharacterControllerMap.Mateo:
			playerInput.SwitchCurrentActionMap(mateoController.actionMapName);
			mateoController.enabled = true;
			break;
		}
	}
}
}