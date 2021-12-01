using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Voidless;

namespace Flamingo
{
public class PlayerInputsManager : Singleton<PlayerInputsManager>
{
	[SerializeField] private PlayerInputController[] _playerControllers; 	/// <summary>PlayerInputControllers.</summary>
	private Dictionary<int, PlayerInputController> _playerControllersMap; 	/// <summary>PlayerInputControllers' Dictionary.</summary>

	/// <summary>Gets and Sets playerControllers property.</summary>
	public static PlayerInputController[] playerControllers
	{
		get { return Instance._playerControllers; }
		set { Instance._playerControllers = value; }
	}

	/// <summary>Gets and Sets playerControllersMap property.</summary>
	public static Dictionary<int, PlayerInputController> playerControllersMap
	{
		get { return Instance._playerControllersMap; }
		private set { Instance._playerControllersMap = value; }
	}

	/// <summary>PlayerInputsManager's instance initialization.</summary>
	private void Awake()
	{
		InputSystem.onDeviceChange += OnDeviceChange;

		playerControllersMap = new Dictionary<int, PlayerInputController>();
		if(playerControllers != null) foreach(PlayerInputController controller in playerControllers)
		{
			playerControllersMap.Add(controller.playerInput.playerIndex, controller);
		}

		//EnableAll(false);
		Enable(0, true);
	}

	/// <summary>Gets PlayerInputController with specified index.</summary>
	/// <param name="_index">Player's Index [0 by default].</param>
	public static PlayerInputController Get(int _index = 0)
	{
		_index = Mathf.Clamp(_index, 0, playerControllers.Length - 1);

		return playerControllersMap[_index];
	}

	/// <summary>Enables Specific PlayerInputController.</summary>
	/// <param name="_index">Player's Index [0 by default].</param>
	/// <param name="_enable">Enable? True by default.</param>
	public static void Enable(int _index = 0, bool _enable = true)
	{
		_index = Mathf.Clamp(_index, 0, playerControllers.Length - 1);

		PlayerInputController controller = playerControllersMap[_index];
		controller.Enable(_enable);
	}

	/// <summary>Enables All PlayerInputControllers.</summary>
	/// <param name="_enable">Enable? True by default.</param>
	public static void EnableAll(bool _enable = true)
	{
		foreach(PlayerInputController controller in playerControllers)
		{
			controller.Enable(_enable);
		}
	}

	/// <summary>Callback invoked when Device Changes.</summary>
	/// <param name="_device">Device that invoked the event.</param>
	/// <param name="_changeEvent">Change's Argument.</param>
	private void OnDeviceChange(InputDevice _device, InputDeviceChange _changeEvent)
	{
		switch(_changeEvent)
		{
			case InputDeviceChange.Added:
                // New Device.
            break;

            case InputDeviceChange.Disconnected:
                // Device got unplugged.
            break;

            case InputDeviceChange.Reconnected:
                // Plugged back in.
            break;

            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
            break;

            default:
                // See InputDeviceChange reference for other event types.
            break;
		}
	}

	/// <returns>String representing this Input Manager.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();
		PlayerInputController controller = null;
		
		builder.AppendLine("PlayerInputs' Map:\n");

		for(int i = 0; i < playerControllersMap.Count; i++)
		{
			controller = playerControllersMap[i];

			builder.Append("Player Index ");
			builder.Append(i);
			builder.Append(": ");
			builder.Append(controller.name);
			builder.AppendLine();
			builder.AppendLine("Controller Status: ");
			builder.AppendLine(controller.ToString());
		}

		return builder.ToString();
	}
}
}