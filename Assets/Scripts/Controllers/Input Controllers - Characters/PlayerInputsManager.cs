using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
		playerControllersMap = new Dictionary<int, PlayerInputController>();
		if(playerControllers != null) foreach(PlayerInputController controller in playerControllers)
		{
			playerControllersMap.Add(controller.playerInput.playerIndex, controller);
		}

#if UNITY_EDITOR
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("PlayerInputs' Map:\n");

		foreach(KeyValuePair<int, PlayerInputController> controllerPair in playerControllersMap)
		{
			builder.Append("Player Index ");
			builder.Append(controllerPair.Key);
			builder.Append(": ");
			builder.Append(controllerPair.Value.name);
			builder.AppendLine();
		}

		VDebug.Log(builder.ToString());
#endif

		EnableAll(false);
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

		playerControllersMap[_index].EnableAll(_enable);
	}

	/// <summary>Enables All PlayerInputControllers.</summary>
	/// <param name="_enable">Enable? True by default.</param>
	public static void EnableAll(bool _enable = true)
	{
		foreach(PlayerInputController controller in playerControllers)
		{
			controller.EnableAll(_enable);
		}
	}
}
}