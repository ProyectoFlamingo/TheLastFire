using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;

namespace Flamingo
{
public class FleetingOverworldController : Singleton<FleetingOverworldController>
{
	[SerializeField] private WorldSpaceButton destinoWorldSpaceButton; 	/// <summary>World Space Button for Destino.</summary>
	[SerializeField] private WorldSpaceButton moskarWorldSpaceButton; 	/// <summary>World Space Button for Moskar.</summary>

	/// <summary>'s instance initialization when loaded [Before scene loads].</summary>
	protected override void OnAwake()
	{
		destinoWorldSpaceButton.button.onClick.AddListener(ToDestino);
		moskarWorldSpaceButton.button.onClick.AddListener(ToMoskar);
		Game.AddTargetToCamera(destinoWorldSpaceButton.GetComponent<VCameraTarget>());
		Game.AddTargetToCamera(moskarWorldSpaceButton.GetComponent<VCameraTarget>());

		Debug.Log("[FleetingOverworldController] Events added to the World-Space Buttons");
	}

	/// <summary>Goes to Destino's Scene.</summary>
	private void ToDestino()
	{
		Game.LoadScene(Game.data.destinoSceneName);
	}

	/// <summary>Goes to Moskar's Scene.</summary>
	private void ToMoskar()
	{
		Game.LoadScene(Game.data.moskarSceneName);
	}
}
}