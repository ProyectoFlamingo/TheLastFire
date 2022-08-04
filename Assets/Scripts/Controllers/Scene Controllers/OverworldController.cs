using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;

namespace Flamingo
{
public class OverworldController : Singleton<OverworldController>
{
	public const int FLAG_INSIDEMEDITATIONPLATFORM = 1 << 0; 							/// <summary>Inside Meditation's Platform Flag.</summary>
	public const int FLAG_INSIDETEMPLE = 1 << 1; 										/// <summary>Inside Temple's Flag.</summary>

	[Space(5f)]

	[Space(5f)]
	[Header("Intro Sequence:")]
	[SerializeField] private ImageSettings _logoImageSettings; 							/// <summary>Logo Imaage's Settings.</summary>
	[SerializeField] private float _logoStayDuration; 									/// <summary>Logo Stay's Duration.</summary>
	[SerializeField] private float _logoFadeDuration; 									/// <summary>Logo Fade's Duration.</summary>
	[SerializeField] private float _logoStayCameraDistance; 							/// <summary>Logo-Stay Camera's Distance.</summary>
	[SerializeField] private float _initialCameraDistance; 								/// <summary>Initial Camera's Distance.</summary>
	[Space(5f)]
	[Header("Audio:")]
	[SerializeField] private AudioLoopData _mainLoopReference; 							/// <summary>Main Loop's Reference.</summary>
	[Space(5f)]
	[Header("Invoke-Event Trigger Zones:")]
	[SerializeField] private InvokeEventTriggerZone _destinoWellTriggerZone; 			/// <summary>Destino's Well Trigger Zone.</summary>
	[SerializeField] private InvokeEventTriggerZone _meditationPlatformTriggerZone; 	/// <summary>Meditation Platform's Trigger Zone.</summary>
	[SerializeField] private InvokeEventTriggerZone _templeTriggerZone; 				/// <summary>Temple's Trigger Zone.</summary>
	private int _flags; 																/// <summary>Scene's Flags.</summary>

	/// <summary>Gets logoImageSettings property.</summary>
	public ImageSettings logoImageSettings { get { return _logoImageSettings; } }

	/// <summary>Gets logoStayDuration property.</summary>
	public float logoStayDuration { get { return _logoStayDuration; } }

	/// <summary>Gets logoFadeDuration property.</summary>
	public float logoFadeDuration { get { return _logoFadeDuration; } }

	/// <summary>Gets logoStayCameraDistance property.</summary>
	public float logoStayCameraDistance { get { return _logoStayCameraDistance; } }

	/// <summary>Gets initialCameraDistance property.</summary>
	public float initialCameraDistance { get { return _initialCameraDistance; } }

	/// <summary>Gets mainLoopReference property.</summary>
	public AudioLoopData mainLoopReference { get { return _mainLoopReference; } }

	/// <summary>Gets destinoWellTriggerZone property.</summary>
	public InvokeEventTriggerZone destinoWellTriggerZone { get { return _destinoWellTriggerZone; } }

	/// <summary>Gets meditationPlatformTriggerZone property.</summary>
	public InvokeEventTriggerZone meditationPlatformTriggerZone { get { return _meditationPlatformTriggerZone; } }

	/// <summary>Gets templeTriggerZone property.</summary>
	public InvokeEventTriggerZone templeTriggerZone { get { return _templeTriggerZone; } }

	/// <summary>Gets and Sets flags property.</summary>
	public int flags
	{
		get { return _flags; }
		private set { _flags = value; }
	}

#region UnityMethods:
	/// <summary>OverworldController's instance initialization.</summary>
	protected override void OnAwake()
	{
		ResourcesManager.onResourcesLoaded += OnResourcesLoaded;
		AudioController.onAudioMappingLoaded += OnAudioMappingLoaded;
		Game.mateo.eventsHandler.onIDEvent += OnMateoIDEvent;
		destinoWellTriggerZone.OnEnterEvent.AddListener(OnDestinoWellEnter);
		meditationPlatformTriggerZone.OnEnterEvent.AddListener(OnMeditationPlatformEnter);
		meditationPlatformTriggerZone.OnExitEvent.AddListener(OnMeditationPlatformExit);
		templeTriggerZone.OnEnterEvent.AddListener(OnTempleEnter);
		templeTriggerZone.OnExitEvent.AddListener(OnTempleExit);
	}

	/// <summary>OverworldController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		
	}
	
	/// <summary>OverworldController's tick at each frame.</summary>
	private void Update ()
	{
		
	}

	/// <summary>Callback invoked when OverworldController's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		ResourcesManager.onResourcesLoaded -= OnResourcesLoaded;
		AudioController.onAudioMappingLoaded -= OnAudioMappingLoaded;
		if(Game.mateo != null) Game.mateo.eventsHandler.onIDEvent -= OnMateoIDEvent;
	}
#endregion

	/// <summary>Logo Image's Sequence.</summary>
	private void LogoImageSequence()
	{
		GameplayGUIController.SetMainImage(logoImageSettings);
		GameplayGUIController.EnableImageView();
		Game.cameraController.distanceAdjuster.distanceRange = logoStayCameraDistance;
		Game.EnablePlayerControl(false);
		this.StartCoroutine(LogoImageSequenceCoroutine());
	}

#region Callbacks:
	/// <summary>Callback invoked when resources haven been loaded by the ResourcesLoader.</summary>
	protected void OnResourcesLoaded()
	{
		LogoImageSequence();
	}

	/// <summary>Callback invokedn when the AudioController's Audio-Mapping has been loaded.</summary>
	protected void OnAudioMappingLoaded()
	{
		Debug.Log("[OverworldController] AudioController's Audio-Mapping has been loaded...");
		mainLoopReference.Play();
		//AudioController.Play(mainLoopReference.GetSourceType(), mainLoopReference.sourceIndex, mainLoopReference.soundReference, mainLoopReference.loop);
	}

	/// <summary>Callback invoked when Mateo invokes an Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnMateoIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_MEDITATION_BEGINS:
				if((flags | FLAG_INSIDEMEDITATIONPLATFORM) == flags)
				{
					Debug.Log("[OverworldController] Cool, to Moskar...");
					Game.LoadScene(Game.data.moskarSceneName);

				} else if((flags | FLAG_INSIDETEMPLE) == flags)
				{
					Debug.Log("[OverworldController] Cool, to Tails the Fox...");
					Game.LoadScene(Game.data.oxfordTheFoxSceneName);
				}
			break;
		}
	}

	/// <summary>Callback invoked when Mateo enters Destino's Well.</summary>
	private void OnDestinoWellEnter()
	{
		Debug.Log("[OverworldController] Cool, to Destino...");
		Game.LoadScene(Game.data.destinoSceneName);
	}

	/// <summary>Callback invoked when Mateo Enters the Meditation Platform.</summary>
	private void OnMeditationPlatformEnter()
	{
		flags |= FLAG_INSIDEMEDITATIONPLATFORM;
	}

	/// <summary>Callback invoked when Mateo Exits the Meditation Platform.</summary>
	private void OnMeditationPlatformExit()
	{
		flags &= ~FLAG_INSIDEMEDITATIONPLATFORM;
	}

	/// <summary>Callback invoked when Mateo Enters the Temple.</summary>
	private void OnTempleEnter()
	{
		flags |= FLAG_INSIDETEMPLE;
	}

	/// <summary>Callback invoked when Mateo Exits the Temple.</summary>
	private void OnTempleExit()
	{
		flags &= ~FLAG_INSIDETEMPLE;
	}
#endregion

	/// <summary>Logo's Sequence Coroutine.</summary>
	private IEnumerator LogoImageSequenceCoroutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(logoStayDuration);
		Image mainImage = GameplayGUIController.Instance.mainImage;

		while(wait.MoveNext()) yield return null;

		wait.ChangeDurationAndReset(logoStayDuration);
		mainImage.CrossFadeAlpha(0.0f, logoFadeDuration, false);

		while(wait.MoveNext()) yield return null;

		GameplayGUIController.EnableImageView(false);
		Game.EnablePlayerControl(true);
		Game.SetDefaultCameraDistanceRange();
	}
}
}