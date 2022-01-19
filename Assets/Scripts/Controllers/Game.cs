using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.SceneManagement;

namespace Flamingo
{
public enum StareTarget
{
	Background,
	Player
}
	
public enum Faction
{
	None,
	Ally,
	Enemy,
	Player1 = Ally,
	Player2 = Enemy
}

public enum GameMode
{
	SinglePlayer,
	MultiPlayer
}

public enum GameState
{
	None,
	Playing,
	Cutscene,
	Paused,
	Transitioning
}

public enum GameContext
{
	Gameplay,
	MiniGame,
	NonSerious
}

[Flags]
public enum SurfaceType
{
	Undefined = 0,
	Floor  = 1,
	Wall = 2,
	Ceiling = 4,

	FloorAndWall = Floor | Wall,
	FloorAndCeiling = Floor | Ceiling,
	WallAndCeiling = Wall | Ceiling,
	All = Floor | Wall | Ceiling
}

/// \TODO Update the Camera on Editor Mode
//[ExecuteInEditMode]
public class Game : Singleton<Game>
{
	public const float DAMAGE_MIN = 1.0f; 									/// <summary>Minimum Damage Applyable.</summary>
	public const float DAMAGE_MAX = Mathf.Infinity; 						/// <summary>Maximum Damage Applyable.</summary>

	[Space(5f)]
	[Header("Game's Data:")]
	[SerializeField] private GameData _data; 								/// <summary>Game's Data.</summary>
	[Space(5f)]
	[SerializeField] private MateoController _mateoController; 				/// <summary>Mateo's Controller.</summary>
	[SerializeField] private Mateo _mateo; 									/// <summary>Mateo's Reference.</summary>
	[SerializeField] private GameplayCameraController _cameraController; 	/// <summary>Gameplay's Camera Controller.</summary>
	[SerializeField] private Camera _UICamera; 								/// <summary>UI's Camera.</summary>
	[SerializeField] private GameplayGUIController _gameplayGUIController; 	/// <summary>Gameplay's GUI Controller.</summary>
	private Boundaries2D _defaultCameraBoundaries; 							/// <summary>Default Camera's Boundaries2D.</summary>
	private FloatRange _defaultDistanceRange; 								/// <summary>Default Camera's Distance Range.</summary>
	private GameState _state; 												/// <summary>Game's State.</summary>
	private GameContext _context; 											/// <summary>Current Game's Context.</summary>
	private GameMode _gameMode; 											/// <summary>Current's Game Mode.</summary>
	private HashSet<Camera2DBoundariesModifier> _boundariesModifiers; 		/// <summary>Boundaries2DContainers for the Gameplay Camera.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets data property.</summary>
	public static GameData data
	{
		get { return Instance._data; }
		set { Instance._data = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public static GameState state
	{
		get { return Instance._state; }
		set { Instance._state = value; }
	}

	/// <summary>Gets and Sets context property.</summary>
	public static GameContext context
	{
		get { return Instance._context; }
		set { Instance._context = value; }
	}

	/// <summary>Gets and Sets gameMode property.</summary>
	public static GameMode gameMode
	{
		get { return Instance._gameMode; }
		set { Instance._gameMode = value; }
	}

	/// <summary>Gets and Sets mateoController property.</summary>
	public static MateoController mateoController
	{
		get { return Instance._mateoController; }
		set { Instance._mateoController = value; }
	}

	/// <summary>Gets and Sets mateo property.</summary>
	public static Mateo mateo
	{
		get { return Instance._mateo; }
		set { Instance._mateo = value; }
	}

	/// <summary>Gets and Sets cameraController property.</summary>
	public static GameplayCameraController cameraController
	{
		get { return Instance._cameraController; }
		set { Instance._cameraController = value; }
	}

	/// <summary>Gets and Sets UICamera property.</summary>
	public static Camera UICamera
	{
		get { return Instance._UICamera; }
		set { Instance._UICamera = value; }
	}

	/// <summary>Gets and Sets gameplayGUIController property.</summary>
	public static GameplayGUIController gameplayGUIController
	{
		get { return Instance._gameplayGUIController; }
		set { Instance._gameplayGUIController = value; }
	}

	/// <summary>Gets and Sets defaultCameraBoundaries property.</summary>
	public static Boundaries2D defaultCameraBoundaries
	{
		get { return Instance._defaultCameraBoundaries; }
		set { Instance._defaultCameraBoundaries = value; }
	}

	/// <summary>Gets and Sets defaultDistanceRange property.</summary>
	public static FloatRange defaultDistanceRange
	{
		get { return Instance._defaultDistanceRange; }
		set { Instance._defaultDistanceRange = value; }
	}

	/// <summary>Gets and Sets boundariesModifiers property.</summary>
	public static HashSet<Camera2DBoundariesModifier> boundariesModifiers
	{
		get { return Instance._boundariesModifiers; }
		set { Instance._boundariesModifiers = value; }
	}
#endregion

	/// <summary>Callback internally called immediately after Awake.</summary>
	protected override void OnAwake()
	{
		data.Initialize();

		defaultCameraBoundaries = cameraController.boundariesContainer.ToBoundaries2D();
		defaultDistanceRange = cameraController.distanceAdjuster.distanceRange;
		boundariesModifiers = new HashSet<Camera2DBoundariesModifier>();

		if(mateo != null)
		{
			mateo.eventsHandler.onIDEvent += OnMateoIDEvent;
			mateo.health.onHealthEvent += OnMateoHealthEvent;
		}

		if(gameplayGUIController != null)
		{
			gameplayGUIController.canvas.worldCamera = UICamera;
			gameplayGUIController.onIDEvent += OnGUIIDEvent;
		}

		state = GameState.Playing;
	}

	private void Start()
	{
		if(mateo != null) AddTargetToCamera(mateo.cameraTarget);
		
		SetForSinglePlayer();
	}

#region TEMPORAL
	/// <summary>Updates Game's instance at each frame.</summary>
	private void LateUpdate()
	{
		/*if(InputController.InputBegin(6))
		{
			enabled = false;
			ResetScene();
		}*/

#if UNITY_EDITOR
		if(!Application.isPlaying && cameraController != null && mateo != null)
		{
			AddTargetToCamera(mateo.cameraTarget);
			cameraController.TEST_CAMERAUPDATE();
		}
#endif
	}
#endregion

	/// <summary>Changes Game Mode to Single Player.</summary>
	public static void SetForSinglePlayer()
	{
		gameMode = GameMode.SinglePlayer;
		PlayerInputController controller = PlayerInputsManager.Get();

		//PlayerInputsManager.EnableAll(false);
		PlayerInputsManager.Enable(); 			/// By default, enables the 1st player

		if(controller != null)
		{
			controller.ChangeControllerMap(CharacterControllerMap.Mateo);
			mateoController = controller.mateoController;
			mateoController.ChangeControllerScheme(ControllerSchemeType.Character);
			controller.AssignCharacterToMateoController(mateo);
		}
		else Debug.LogError("[Game] Controller not detected");
	}

	/// <summary>Resets FSM Loop's States.</summary>
	public static void ResetFSMLoopStates()
	{
		if(data != null) data.ResetFSMLoopStates();
	}

	/// <returns>Mateo's Position.</returns>
	public static Vector2 GetMateoPosition()
	{
		return mateo != null ? mateo.transform.position : Vector3.zero;
	}

	/// <summary>Calculates Mateo's projected position [heuristics] on given time.</summary>
	/// <param name="t">Time's projection.</param>
	/// <returns>Mateo's Projected Position.</returns>
	public static Vector2 ProjectMateoPosition(float t)
	{
		return mateo != null ? mateo.transform.position + (mateo.deltaCalculator.deltaPosition * t) : Vector3.zero;
	}

	/// <summary>Enables player control.</summary>
	/// <param name="_enable">Enable? True by default.</param>
	public static void EnablePlayerControl(bool _enable = true)
	{
		mateoController.enabled = _enable;
	}

	/// <summary>Gets Mateo's Maximum Jump Force.</summary>
	/// <param name="_allJumps">Predict the force of all jumps? If false, it will only project the force of the first jump.</param>
	/// <returns>Force projection of Mateo's jumps.</returns>
	public static Vector2 GetMateoMaxJumpingHeight(bool _allJumps = true)
	{
		return _allJumps ? mateo.jumpAbility.PredictForces() : mateo.jumpAbility.PredictForce(0);
	}

	/// <summary>Sets default Boundaries2D's settings for the camera.</summary>
	public static void SetDefaultCameraBoundaries2D()
	{
		cameraController.boundariesContainer.Set(defaultCameraBoundaries);
	}

	/// <summary>Sets default distance range settigns for the camera.</summary>
	public static void SetDefaultCameraDistanceRange()
	{
		cameraController.distanceAdjuster.distanceRange = defaultDistanceRange;
	}

	/// <summary>Sets Camera2DBoundariesModifier's Settings into the Gameplay Camera.</summary>
	/// <param name="_modifier">Camera2DBdounratiesModifier that contains the new settings.</param>
	public static void SetCameraBoundaries2DSettings(Camera2DBoundariesModifier _modifier)
	{
		cameraController.boundariesContainer.Set(_modifier.boundariesContainer.ToBoundaries2D());
		if(_modifier.setDistance) cameraController.distanceAdjuster.distanceRange = _modifier.distanceRange;
	}

	/// <summary>Adds Target's VCameraTarget into the Camera.</summary>
	/// <param name="_target">VCameraTarget to Add.</param>
	public static void AddTargetToCamera(VCameraTarget _target)
	{
		if(cameraController != null) cameraController.middlePointTargetRetriever.AddTarget(_target);
	}

	/// <summary>Removes Target's VCameraTarget into the Camera.</summary>
	/// <param name="_target">VCameraTarget to Remove.</param>
	public static void RemoveTargetToCamera(VCameraTarget _target)
	{
		if(cameraController != null) cameraController.middlePointTargetRetriever.RemoveTarget(_target);
	}

	/// <summary>Loads Scene.</summary>
	/// <param name="_scene">Scene's Name.</param>
	public static void LoadScene(string _scene, bool _fadeIn = true)
	{
		PlayerPrefs.SetString(GameData.PATH_SCENE_TOLOAD, _scene);
		
		switch(_fadeIn)
		{
			case true:
			FadeInScreen(Color.black, gameplayGUIController.screenFaderGUI.inDuration,
			()=>
			{
				SceneManager.LoadScene(GameData.PATH_SCENE_LOADING);
			});
			break;

			case false:
			SceneManager.LoadScene(GameData.PATH_SCENE_LOADING);
			break;
		}
	}

	/// <summary>Resets current Scene.</summary>
	public static void ResetScene()
	{
		Scene scene = SceneManager.GetActiveScene();
		LoadScene(scene.name);
	}

	/// <summary>Fades-In Screen and invokes callback afterwards.</summary>
	/// <param name="_color">Fade Color.</param>
	/// <param name="_duration">Fade-In's Duration.</param>
	/// <param name="onFadeEnds">Optional callback invoked when the Fade-In ends.</param>
	public static void FadeInScreen(Color _color, float _duration, Action onFadeEnds)
	{
		gameplayGUIController.screenFaderGUI.FadeIn(_color, _duration, onFadeEnds);
	}

	/// <summary>Fades-Out Screen and invokes callback afterwards.</summary>
	/// <param name="_color">Fade Color.</param>
	/// <param name="_duration">Fade-Out's Duration.</param>
	/// <param name="onFadeEnds">Optional callback invoked when the Fade-Out ends.</param>
	public static void FadeOutScreen(Color _color, float _duration, Action onFadeEnds)
	{
		gameplayGUIController.screenFaderGUI.FadeOut(_color, _duration, onFadeEnds);
	}

	/// <summary>Callback invoked when a Camera2DBoundariesModifier is entered.</summary>
	/// <param name="_modifier">Camera2DBoundariesModifier that invoked the event.</param>
	public static void OnCamera2DBoundariesModifierEnter(Camera2DBoundariesModifier _modifier)
	{
		if(boundariesModifiers.Contains(_modifier)) return;
		
		boundariesModifiers.Add(_modifier);

		if(boundariesModifiers.Count > 1) return;

		cameraController.constraints = CameraFollowingConstraints.BoundaryConstrained;
		cameraController.boundariesContainer.Set(_modifier.boundariesContainer.ToBoundaries2D());
		//cameraController.boundariesContainer.InterpolateTowards(_modifier.boundariesContainer.ToBoundaries2D(), _modifier.interpolationDuration);

		if(_modifier.setDistance) cameraController.distanceAdjuster.distanceRange = _modifier.distanceRange;

	}

	/// <summary>Callback invoked when a Camera2DBoundariesModifier is left.</summary>
	/// <param name="_modifier">Camera2DBoundariesModifier that invoked the event.</param>
	public static void OnCamera2DBoundariesModifierExit(Camera2DBoundariesModifier _modifier)
	{
		if(!boundariesModifiers.Contains(_modifier)) return;

		boundariesModifiers.Remove(_modifier);

		_modifier.boundariesContainer.OnInterpolationEnds();

		if(boundariesModifiers.Count == 0)
		{
			cameraController.constraints = CameraFollowingConstraints.Unconstrained;
			//SetDefaultCameraBoundaries2D();
			SetDefaultCameraDistanceRange();
		}
		else
		{
			Camera2DBoundariesModifier modifier = boundariesModifiers.First();

			cameraController.boundariesContainer.Set(modifier.boundariesContainer.ToBoundaries2D());
			//cameraController.boundariesContainer.InterpolateTowards(modifier.boundariesContainer.ToBoundaries2D(), modifier.interpolationDuration);
			
			if(modifier.setDistance) Game.cameraController.distanceAdjuster.distanceRange = modifier.distanceRange;
		}
	}

	/// <summary>Activates Gameplay's Camera.</summary>
	/// <param name="_activate">Activate? true by default.</param>
	public static void ActivateGameplayCamera(bool _activate = true)
	{
		cameraController.gameObject.SetActive(_activate);
	}

	/// <summary>Activates UI's Camera.</summary>
	/// <param name="_activate">Activate? true by default.</param>
	public static void ActivateUICamera(bool _activate = true)
	{
		UICamera.gameObject.SetActive(_activate);
	}

	/// <summary>Activates Mateo.</summary>
	/// <param name="_activate">Activate? true by default.</param>
	public static void ActivateMateo(bool _activate = true)
	{
		EnablePlayerControl(_activate);
		mateo.gameObject.SetActive(_activate);
	}

	/// <summary>Callback invoked when a pause is requested.</summary>
	public static void OnPause(int _playerID = 0)
	{
		state = state != GameState.Paused ? GameState.Paused : GameState.Playing;
		
		bool pause = state == GameState.Paused;
		PlayerInputController controller = PlayerInputsManager.Get();

		controller.mateoController.ChangeControllerScheme(pause ? ControllerSchemeType.UI : ControllerSchemeType.Character);
		UICamera.gameObject.SetActive(pause);
		gameplayGUIController.EnablePauseMenu(pause);

		Time.timeScale = pause ? 0.0f : 1.0f;

		if(!pause) controller.ChangeControllerMap(CharacterControllerMap.Mateo);
	}

	/// <summary>Evaluates Surface Type.</summary>
	/// <param name="u">Up's Normal.</param>
	/// <param name="n">Face's Normal.</param>
	public static SurfaceType EvaluateSurfaceType(Vector2 n)
	{
		if(n.sqrMagnitude != 1.0f) n.Normalize();

		Vector2 g = -Physics2D.gravity.normalized;
		float d = Vector2.Dot(n, g);
		float c = data.ceilingDotProductThreshold;
		float f = data.floorDotProductThreshold;

		if(d >= -1.0f && d < c) return SurfaceType.Ceiling;
		if(d >= c && d < f) return SurfaceType.Wall;
		if(d  >= f && d <= 1.0f) return SurfaceType.Floor; 

		return SurfaceType.Undefined;
	}

	/// <summary>Callback invoked when Mateo invokes an Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private static void OnMateoIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_DEAD:
			ResetScene();
			break;
		}
	}

	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected static void OnMateoHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		switch(_event)
		{
			case HealthEvent.Depleted:
			float t = (1.0f - mateo.health.hpRatio);
			float duration = data.damageCameraShakeDuration.Lerp(t); 
			float speed = data.damageCameraShakeSpeed.Lerp(t); 	
			float magnitude = data.damageCameraShakeMagnitude.Lerp(t);

			if(cameraController == null) return;

			Instance.StartCoroutine(cameraController.transform.ShakePosition(duration, speed, magnitude));
			Instance.StartCoroutine(cameraController.transform.ShakeRotation(duration, speed, magnitude));
			break;

			case HealthEvent.FullyDepleted:
			cameraController.middlePointTargetRetriever.ClearTargets();
			AddTargetToCamera(mateo.cameraTarget);
			cameraController.distanceAdjuster.distanceRange = data.deathZoom;
			break;
		}
	}

	/// <summary>Callback invoked when the GUI invokes an Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private static void OnGUIIDEvent(int _ID)
	{
		switch(_ID)
		{
			case GameplayGUIController.ID_STATE_UNPAUSED:
			Time.timeScale = 1.0f;
			state = GameState.Playing;
			break;
		}
	}

	/// <summary>Sets Time-Scale.</summary>
	/// <param name="_timeScale">Time-Scale.</param>
	/// <param name="_changeAudioPitch">Change Audio-Pitch also? true by default.</param>
	/// <param name="_accelerate">Accelerate towards new time-scale or instantly change? true by default.</param>
	public static void SetTimeScale(float _timeScale, bool _changeAudioPitch = true, bool _accelerate = true)
	{
		switch(_accelerate)
		{
			case true:
			Instance.StartCoroutine(Instance.ChangeTimeScaleRoutine(_timeScale, _changeAudioPitch));
			break;

			case false:
			Time.timeScale = _timeScale;
			if(_changeAudioPitch) AudioController.SetPitch(Time.timeScale);
			break;
		}
	}

	/// <returns>String representing Game.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Expected Frame-Rate: ");
		builder.Append(data.frameRate.ToString());
		builder.AppendLine(" FPSs");
		builder.Append("Current Frame-Rate: ");
		builder.Append(VExtensionMethods.GetFrameRate().ToString());
		builder.AppendLine(" FPSs");
		builder.Append("Game State: ");
		builder.AppendLine(state.ToString());
		builder.Append("Game Context: ");
		builder.AppendLine(context.ToString());
		builder.Append("Game Mode: ");
		builder.AppendLine(gameMode.ToString());

		return builder.ToString();
	}

	/// <summary>Sets Time-Scale's Routine.</summary>
	/// <param name="_timeScale">Time-Scale.</param>
	/// <param name="_changeAudioPitch">Change Audio-Pitch also? true by default.</param>
	protected IEnumerator ChangeTimeScaleRoutine(float _timeScale, bool _changeAudioPitch = true)
	{
		float timeScale = Time.timeScale;

		if(timeScale == _timeScale) yield break;

		float t = 0.0f;
		float s = Mathf.Sign(_timeScale - timeScale);
		float a = s >= 0.0 ? data.timeScaleAcceleration : data.timeScaleDeceleration;
		
		while((s == 1.0f) ? (timeScale < _timeScale) : (timeScale > _timeScale))
		{
			timeScale += (t * s);
			t += (a * Time.unscaledDeltaTime);
			t = Mathf.Clamp(t, 0.0f, 1.0f);

			SetTimeScale(timeScale, _changeAudioPitch, false);

			yield return null;
		}

		SetTimeScale(_timeScale, _changeAudioPitch, false);
	}
}
}