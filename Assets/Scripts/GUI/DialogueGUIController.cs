using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;

namespace Flamingo
{
public class DialogueGUIController : Singleton<DialogueGUIController>
{
	[Space(5f)]
	[Header("Containers:")]
	[SerializeField] private GameObject _dialogueBoxContainer; 		/// <summary>Dialogue Box's Container.</summary>
	[SerializeField] private GameObject _choicesContainer; 			/// <summary>Choices' Container.</summary>
	[Space(5f)]
	[Header("Dialogue Box's Attributes:")]
	[SerializeField] private Text _speakerText; 					/// <summary>Speaker's Text.</summary>
	[SerializeField] private Text _dialogueText; 					/// <summary>Dialogue's Text.</summary>
	[Space(5f)]
	[Header("Buttons:")]
	[SerializeField] private WorldSpaceButton[] _choicesButtons; 	/// <summary>Choices' Buttons.</summary>
	[SerializeField] private WorldSpaceButton _skipButton; 	/// <summary>Skip's Button.</summary>
	[Space(5f)]
	[Header("Animations' Attributes:")]
	[SerializeField] private float _scaleDownDuration; 				/// <summary>Scale-Down's Duration.</summary>
	[SerializeField] private float _scaleUpDuration; 				/// <summary>Scale-Up's Duration.</summary>

	/// <summary>Gets dialogueBoxContainer property.</summary>
	public GameObject dialogueBoxContainer { get { return _dialogueBoxContainer; } }

	/// <summary>Gets choicesContainer property.</summary>
	public GameObject choicesContainer { get { return _choicesContainer; } }

	/// <summary>Gets speakerText property.</summary>
	public Text speakerText { get { return _speakerText; } }

	/// <summary>Gets dialogueText property.</summary>
	public Text dialogueText { get { return _dialogueText; } }

	/// <summary>Gets choicesButtons property.</summary>
	public WorldSpaceButton[] choicesButtons { get { return _choicesButtons; } }

	/// <summary>Gets skipButton property.</summary>
	public WorldSpaceButton skipButton { get { return _skipButton; } }

	/// <summary>Gets scaleDownDuration property.</summary>
	public float scaleDownDuration { get { return _scaleDownDuration; } }

	/// <summary>Gets scaleUpDuration property.</summary>
	public float scaleUpDuration { get { return _scaleUpDuration; } }

#region UnityMethods:
	/// <summary>DialogueGUICController's instance initialization.</summary>
	private void Awake()
	{
		
	}

	/// <summary>DialogueGUICController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		
	}
	
	/// <summary>DialogueGUICController's tick at each frame.</summary>
	private void Update ()
	{
		
	}
#endregion

	/// <summary>Activates all buttons.</summary>
	/// <param name="_activate">Activate? true by default.</param>
	public void ActivateButtons(bool _activate  = true)
	{
		foreach(WorldSpaceButton worldSpaceButton in choicesButtons)
		{
			/*Debug.Log("[DialogueGUIController] World Space Button? " + worldSpaceButton != null);
			Debug.Log("[DialogueGUIController] Button? " + worldSpaceButton.button != null);*/

			worldSpaceButton.button.onClick.RemoveAllListeners();
			worldSpaceButton.gameObject.SetActive(_activate);
		}
	}

}
}