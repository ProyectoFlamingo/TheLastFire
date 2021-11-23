using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using Voidless;

namespace Flamingo
{
public enum GUIState
{
	None,
	Pause
}

[RequireComponent(typeof(ScreenFaderGUI))]
[RequireComponent(typeof(Canvas))]
public class GameplayGUIController : MonoBehaviour
{
	public const int ID_STATE_PAUSED = 0; 								/// <summary>Paused's Event's ID.</summary>
	public const int ID_STATE_UNPAUSED = 1; 							/// <summary>Un-Paused's Event's ID.</summary>

	public event OnIDEvent onIDEvent; 									/// <summary>OnIDEvent's Delegate.</summary>

	[SerializeField] private InputSystemUIInputModule _inputModule; 	/// <summary>Input's Module.</summary>
	[Space(5f)]
	[Header("General Settings:")]
	[SerializeField] private float _scaleUpDuration; 					/// <summary>Scale-Up's Duration.</summary>
	[SerializeField] private float _scaleDownDuration; 					/// <summary>Scale-Down's Duration.</summary>
	[Space(5f)]
	[Header("Pause UI's Attributes:")]
	[SerializeField] private GameObject _pauseMenuGroup; 				/// <summary>Pause Menu's Group.</summary>
	[SerializeField] private Button _pauseSettingsButton; 				/// <summary>Pause Menu's Settings' Button.</summary>
	[SerializeField] private Button _pauseContinueButton; 				/// <summary>Pause Menu's Continue's Button.</summary>
	[SerializeField] private Button _pauseExitButton; 					/// <summary>Pause Menu's Exit's Button.</summary>
	private ScreenFaderGUI _screenFaderGUI; 							/// <summary>ScreenFaderGUI's Component.</summary>
	private Canvas _canvas; 											/// <summary>Canvas' Component.</summary>
	private Coroutine coroutine; 										/// <summary>Coroutine's Reference.</summary>
	private GUIState _state; 											/// <summary>Current State.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets inputModule property.</summary>
	public InputSystemUIInputModule inputModule
	{
		get { return _inputModule; }
		set { _inputModule = value; }
	}

	/// <summary>Gets scaleUpDuration property.</summary>
	public float scaleUpDuration { get { return _scaleUpDuration; } }

	/// <summary>Gets scaleDownDuration property.</summary>
	public float scaleDownDuration { get { return _scaleDownDuration; } }

	/// <summary>Gets pauseMenuGroup property.</summary>
	public GameObject pauseMenuGroup { get { return _pauseMenuGroup; } }

	/// <summary>Gets pauseSettingsButton property.</summary>
	public Button pauseSettingsButton { get { return _pauseSettingsButton; } }

	/// <summary>Gets pauseContinueButton property.</summary>
	public Button pauseContinueButton { get { return _pauseContinueButton; } }

	/// <summary>Gets pauseExitButton property.</summary>
	public Button pauseExitButton { get { return _pauseExitButton; } }

	/// <summary>Gets screenFaderGUI Component.</summary>
	public ScreenFaderGUI screenFaderGUI
	{ 
		get
		{
			if(_screenFaderGUI == null) _screenFaderGUI = GetComponent<ScreenFaderGUI>();
			return _screenFaderGUI;
		}
	}

	/// <summary>Gets canvas Component.</summary>
	public Canvas canvas
	{ 
		get
		{
			if(_canvas == null) _canvas = GetComponent<Canvas>();
			return _canvas;
		}
	}

	/// <summary>Gets and Sets state property.</summary>
	public GUIState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets onTransition property.</summary>
	public bool onTransition { get { return coroutine != null; } }
#endregion

	/// <summary>Callback invoked when GameplayGUIController's instance is disabled.</summary>
	private void OnDisable()
	{
		
	}

	/// <summary>GameplayGUIController's instance initialization.</summary>
	private void Awake()
	{
		state = GUIState.None;
		pauseMenuGroup.SetActive(false);
		AddListenersToButtons();
	}

	/// <summary>GameplayGUIController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		inputModule.ActivateModule();
	}

	/// <summary>Enables Pause's Menu.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public void EnablePauseMenu(bool _enable = true)
	{
		if(onTransition) return;

		state = _enable ? GUIState.Pause : GUIState.None;

		Transform[] buttons = new Transform[] { pauseSettingsButton.transform, pauseContinueButton.transform, pauseExitButton.transform };
		Selectable[] selectables = new Selectable[] { pauseSettingsButton, pauseContinueButton, pauseExitButton };

		EnableElements(false, selectables);
		pauseMenuGroup.SetActive(true);
		ScaleUIElementsInstatly(_enable, buttons);
		this.StartCoroutine(ScaleUIElements(_enable, OnPauseTransitionEnds, buttons), ref coroutine);
	}

	/// <summary>Invokes OnIDEvent.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void InvokeIDEvent(int _ID)
	{
		if(onIDEvent != null) onIDEvent(_ID);
	}

	/// <summary>Adds Listeners to all Buttons.</summary>
	private void AddListenersToButtons()
	{
		pauseSettingsButton.onClick.AddListener(OnPauseSettingsSelected);
		pauseContinueButton.onClick.AddListener(OnPauseContinueSelected);
		pauseExitButton.onClick.AddListener(OnPauseExitSelected);
	}

	/// <summary>Callback internally invoked when Settings' Option is selected on the Pause Menu.</summary>
	private void OnPauseSettingsSelected()
	{
		EnablePauseMenu(false);
	}

	/// <summary>Callback internally invoked when Continue's Option is selected on the Pause Menu.</summary>
	private void OnPauseContinueSelected()
	{
		EnablePauseMenu(false);		
	}

	/// <summary>Callback internally invoked when Exit's Option is selected on the Pause Menu.</summary>
	private void OnPauseExitSelected()
	{
		EnablePauseMenu(false);		
	}

	/// <summary>Callback internally invoked when the Pause Menu's transition is over.</summary>
	/// <param name="_scaleUp">Was the transition a Scale-Up?.</param>
	private void OnPauseTransitionEnds(bool _scaleUp)
	{
		switch(_scaleUp)
		{
			case true:
			EnableElements(true, pauseSettingsButton, pauseContinueButton, pauseExitButton);
			EventSystem.current.SetSelectedGameObject(pauseContinueButton.gameObject);
			break;

			case false:
			InvokeIDEvent(ID_STATE_UNPAUSED);
			pauseMenuGroup.SetActive(false);
			break;
		}

		this.DispatchCoroutine(ref coroutine);
	}

	/// <summary>Enables Selectable Elements.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	/// <param name="_elements">Elements to enable.</param>
	private void EnableElements(bool _enable = true, params Selectable[] _elements)
	{
		foreach(Selectable element in _elements)
		{
			element.enabled = _enable;
		}
	}

	/// <summary>Instantly scales UI Elements.</summary>
	/// <param name="_up">Scale up? Scales down otherwise.</param>
	/// <param name="_elements">Elements to scale.</param>
	private void ScaleUIElementsInstatly(bool _up = true, params Transform[] _elements)
	{
		Vector3 s = _up ? Vector3.one : Vector3.zero;

		foreach(Transform element in _elements)
		{
			element.localScale = s;
		}
	}

	/// <summary>Scale UI Elements' Coroutine.</summary>
	/// <param name="_up">Scale up? Scales down otherwise.</param>
	/// <param name="onScaleEnds">Callback invoked when the scaling ends.</param>
	/// <param name="_elements">Elements to scale.</param>
	private IEnumerator ScaleUIElements(bool _up = true, Action<bool> onScaleEnds = null, params Transform[] _elements)
	{
		Vector3 a = _up ? Vector3.zero : Vector3.one;
		Vector3 b = _up ? Vector3.one : Vector3.zero;
		float t = 0.0f;
		float inverseDuration = 1.0f / (_up ? scaleUpDuration : scaleDownDuration);

		while(t < 1.0f)
		{
			foreach(Transform element in _elements)
			{
				element.localScale = Vector3.Lerp(a, b, t);
			}

			t += (Time.unscaledDeltaTime * inverseDuration);
			yield return null;
		}

		foreach(Transform element in _elements)
		{
			element.localScale = b;
		}

		if(onScaleEnds != null) onScaleEnds(_up);
	}
}
}