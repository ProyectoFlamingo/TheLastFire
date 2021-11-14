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
	public PlayerInputController[] playerControllers
	{
		get { return _playerControllers; }
		set { _playerControllers = value; }
	}

	/// <summary>Gets and Sets playerControllersMap property.</summary>
	public Dictionary<int, PlayerInputController> playerControllersMap
	{
		get { return _playerControllersMap; }
		private set { _playerControllersMap = value; }
	}

	/// <summary>PlayerInputsManager's instance initialization.</summary>
	private void Awake()
	{
		playerControllersMap = new Dictionary<int, PlayerInputController>();
		if(playerControllers != null) foreach(PlayerInputController controller in playerControllers)
		{
			playerControllersMap.Add(controller.playerInput.playerIndex, controller);
		}

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

		Debug.Log(builder.ToString());
	}

	/// <summary>Gets PlayerInputController with specified index.</summary>
	/// <param name="_index">Player's Index [0 by default].</param>
	public static PlayerInputController Get(int _index = 0)
	{
		_index = Mathf.Clamp(_index, 0, Instance.playerControllers.Length - 1);

		return Instance.playerControllersMap[_index];
	}
}
}