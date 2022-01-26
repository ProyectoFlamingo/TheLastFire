using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Voidless;
using Sirenix.OdinInspector;
#if UNITY_SWITCH
using UnityEngine.Switch;
#endif

namespace Flamingo
{
[CreateAssetMenu]
public class GameData : ScriptableObject
{
	public const string PATH_SCENE_TOLOAD = "SceneToLoad"; 										/// <summary>Scene to Load's Path on the Player Prefs [or default].</summary>
	public const string PATH_SCENE_DEFAULT = "Scene_LoadingScreen"; 							/// <summary>Default Scene to Load's Path.</summary>
	public const string PATH_SCENE_LOADING = "Scene_LoadingScreen"; 							/// <summary>Loading Scene's Path.</summary>

	[Header("Configurations:")]
	[SerializeField] [Range(0, 60)] private int _frameRate; 									/// <summary>Game's Frame rate.</summary>
	[SerializeField] [Range(0.0f, 1.0f)] private float _hurtTimeScale; 							/// <summary>Hurt's Time-Scale.</summary>
	[SerializeField] private float _timeScaleAcceleration; 										/// <summary>Time-Scale change's Acceleration.</summary>
	[SerializeField] private float _timeScaleDeceleration; 										/// <summary>Time-Scale change's Deceleration.</summary>
	[Space(5f)]
	[SerializeField] private string _firstSceneToLoad; 											/// <summary>First Scene to Load.</summary>
	[SerializeField] private string _loadingSceneName; 											/// <summary>Loading Scene's Name.</summary>
	[SerializeField] private string _overworldSceneName; 										/// <summary>Overworld Scene's Name.</summary>
	[SerializeField] private string _destinoSceneName; 											/// <summary>Destuino Scene's Name.</summary>
	[SerializeField] private string _moskarSceneName; 											/// <summary>Moskar Scene's Name.</summary>
	[SerializeField] private string _captainShantySceneName; 									/// <summary>Captain Shanty Scene's Name.</summary>
	[SerializeField] private string _oxfordTheFoxSceneName; 									/// <summary>Oxford the Fox Scene's Name.</summary>
	[SerializeField] private string _miauxiSceneName; 											/// <summary>Miauxi Scene's Name.</summary>
	[SerializeField] private string _rinocircusSceneName; 										/// <summary>Rinocircus Scene's Name.</summary>
	[SerializeField] private string _moctechzumaSceneName; 										/// <summary>Moctechzuma Scene's Name.</summary>
	[SerializeField] private string[] _trainingGroundScenesNames; 								/// <summary>Training Grounds' Scenes' Names.</summary>
	[Space(5f)]
	[Header("Rotations:")]
	[SerializeField] private EulerRotation _stareAtBackgroundRotation; 							/// <summary>Stare at Boss's Rotation.</summary>
	[SerializeField] private EulerRotation _stareAtPlayerRotation; 								/// <summary>Stare At Player's Rotation.</summary>
	[Space(5f)]
	[Header("Camera Configurations:")]
	[SerializeField] private float _deathZoom; 													/// <summary>Death's Zoom.</summary>
	[Header("Camera Shaking's Attributes:")]
	[SerializeField] private FloatRange _damageCameraShakeDuration; 							/// <summary>Camera Shake's Duration when Mateo receives damage.</summary>
	[SerializeField] private FloatRange _damageCameraShakeSpeed; 								/// <summary>Camera Shake's Speed when Mateo receives damage.</summary>
	[SerializeField] private FloatRange _damageCameraShakeMagnitude; 							/// <summary>Camera Shake's Magnitude when Mateo receives damage.</summary>
	[Space(5f)]
	[Header("Tags:")]
	[SerializeField] private GameObjectTag _playerTag; 											/// <summary>Player's Tag.</summary>
	[SerializeField] private GameObjectTag _enemyTag; 											/// <summary>Enemy's Tag.</summary>
	[SerializeField] private GameObjectTag _playerWeaponTag; 									/// <summary>Player Weapon's Tag.</summary>
	[SerializeField] private GameObjectTag _enemyWeaponTag; 									/// <summary>Enemy Weapon's Tag.</summary>
	[SerializeField] private GameObjectTag _playerProjectileTag; 								/// <summary>Player Projectile's Tag.</summary>
	[SerializeField] private GameObjectTag _enemyProjectileTag; 								/// <summary>Enemy Projectile's Tag.</summary>
	[SerializeField] private GameObjectTag _explodableTag; 										/// <summary>Explodable 's Tag.</summary>
	[SerializeField] private GameObjectTag _floorTag; 											/// <summary>Floor surface's Type Tag.</summary>
	[SerializeField] private GameObjectTag _wallTag; 											/// <summary>Wall surface's Type Tag.</summary>
	[SerializeField] private GameObjectTag _ceilingTag; 										/// <summary>Ceiling surface's Type Tag.</summary>
	[SerializeField] private GameObjectTag _outOfBoundsTag; 									/// <summary>Out-Of-Bounds' Tag.</summary>
	[Space(5f)]
	[Header("Layers:")]
	[SerializeField] private LayerValue _outOfBoundsLayer; 										/// <summary>Out of Bounds's Layer.</summary>
	[SerializeField] private LayerValue _surfaceLayer; 											/// <summary>Surface's Layer.</summary>
	[Space(5f)]
	[Header("Projectiles:")]
	[PreviewField]
	[TabGroup("Pool GameObjects")][SerializeField] private Projectile[] _projectiles; 			/// <summary>Game's Projectiles.</summary>
	[Space(5f)]
	[SerializeField] private PoolGameObject[] _poolObjects; 									/// <summary>Pool GameObjects.</summary>
	[Space(5f)]
	[Header("Audios:")]
	[TabGroup("Audio")][SerializeField] private SoundEffectLooper _looper; 						/// <summary>Sound-Effect's Looper Reference.</summary>
	[TabGroup("Audio")][SerializeField] private FiniteStateAudioClip[] _FSMLoops; 				/// <summary>Finite-State's Loop Effects.</summary>
	[TabGroup("Audio")][SerializeField] private AudioClip[] _loops; 							/// <summary>Loop Effects.</summary>
	[TabGroup("Audio")][SerializeField] private AudioClip[] _soundEffects; 						/// <summary>Sounds' Effects.</summary>
	[Space(5f)]
	[Header("Particle Effects:")]
	[TabGroup("Pool GameObjects")][SerializeField] private ParticleEffect[] _particleEffects; 	/// <summary>Particle Effects.</summary>
	[Space(5f)]
	[Header("Explodables:")]
	[TabGroup("Pool GameObjects")][SerializeField] private Explodable[] _explodables; 			/// <summary>Explodables.</summary>
	[HideInInspector] public FloatWrapper _ceilingDotProductThreshold; 							/// <summary>Dot-Product Threshold for the Ceiling.</summary>
	[HideInInspector] public FloatWrapper _floorDotProductThreshold; 							/// <summary>Dot-Product Threshold for the Floor.</summary>
	[HideInInspector] public FloatWrapper _ceilingAngleThreshold; 								/// <summary>Angle Threshold for the Ceiling.</summary>
	[HideInInspector] public FloatWrapper _floorAngleThreshold; 								/// <summary>Angle Threshold for the Floor.</summary>
	private GameObjectTag[] _allFactionsTags; 													/// <summary>All Factions' Tags.</summary>
	private GameObjectTag[] _allWeaponsTags; 													/// <summary>All Weapons' Tags.</summary>
	private GameObjectTag[] _allProjectilesTags; 												/// <summary>All Projectiles' Tags.</summary>
	private float _idealDeltaTime; 																/// <summary>Ideal delta time.</summary>
#if UNITY_EDITOR
	[HideInInspector] public bool showDotProducts; 												/// <summary>Enable settings for Dot Products' Thresholds? if false, it will show settings for the Angles' Thresholds.</summary>
#endif

#region Getters:
	/// <summary>Gets and Sets ceilingDotProductThreshold property.</summary>
	public FloatWrapper ceilingDotProductThreshold
	{
		get { return _ceilingDotProductThreshold; }
		set { _ceilingDotProductThreshold = value; }
	}

	/// <summary>Gets and Sets floorDotProductThreshold property.</summary>
	public FloatWrapper floorDotProductThreshold
	{
		get { return _floorDotProductThreshold; }
		set { _floorDotProductThreshold = value; }
	}

	/// <summary>Gets and Sets ceilingAngleThreshold property.</summary>
	public FloatWrapper ceilingAngleThreshold
	{
		get { return _ceilingAngleThreshold; }
		set { _ceilingAngleThreshold = value; }
	}

	/// <summary>Gets and Sets floorAngleThreshold property.</summary>
	public FloatWrapper floorAngleThreshold
	{
		get { return _floorAngleThreshold; }
		set { _floorAngleThreshold = value; }
	}

	/// <summary>Gets firstSceneToLoad property.</summary>
	public string firstSceneToLoad { get { return _firstSceneToLoad; } }

	/// <summary>Gets loadingSceneName property.</summary>
	public string loadingSceneName { get { return _loadingSceneName; } }

	/// <summary>Gets overworldSceneName property.</summary>
	public string overworldSceneName { get { return _overworldSceneName; } }

	/// <summary>Gets destinoSceneName property.</summary>
	public string destinoSceneName { get { return _destinoSceneName; } }

	/// <summary>Gets captainShantySceneName property.</summary>
	public string captainShantySceneName { get { return _captainShantySceneName; } }

	/// <summary>Gets moskarSceneName property.</summary>
	public string moskarSceneName { get { return _moskarSceneName; } }

	/// <summary>Gets oxfordTheFoxSceneName property.</summary>
	public string oxfordTheFoxSceneName { get { return _oxfordTheFoxSceneName; } }

	/// <summary>Gets miauxiSceneName property.</summary>
	public string miauxiSceneName { get { return _miauxiSceneName; } }

	/// <summary>Gets rinocircusSceneName property.</summary>
	public string rinocircusSceneName { get { return _rinocircusSceneName; } }

	/// <summary>Gets moctechzumaSceneName property.</summary>
	public string moctechzumaSceneName { get { return _moctechzumaSceneName; } }

	/// <summary>Gets trainingGroundScenesNames property.</summary>
	public string[] trainingGroundScenesNames { get { return _trainingGroundScenesNames; } }

	/// <summary>Gets stareAtBackgroundRotation property.</summary>
	public EulerRotation stareAtBackgroundRotation { get { return _stareAtBackgroundRotation; } }

	/// <summary>Gets stareAtPlayerRotation property.</summary>
	public EulerRotation stareAtPlayerRotation { get { return _stareAtPlayerRotation; } }

	/// <summary>Gets frameRate property.</summary>
	public int frameRate { get { return _frameRate; } }

	/// <summary>Gets hurtTimeScale property.</summary>
	public float hurtTimeScale { get { return _hurtTimeScale; } }

	/// <summary>Gets timeScaleAcceleration property.</summary>
	public float timeScaleAcceleration { get { return _timeScaleAcceleration; } }

	/// <summary>Gets timeScaleDeceleration property.</summary>
	public float timeScaleDeceleration { get { return _timeScaleDeceleration; } }

	/// <summary>Gets deathZoom property.</summary>
	public float deathZoom { get { return _deathZoom; } }

	/// <summary>Gets idealDeltaTime property.</summary>
	public float idealDeltaTime
	{
		get
		{
			if(_idealDeltaTime == 0.0f) _idealDeltaTime = 1.0f / (frameRate > 0 ? (float)frameRate : 60.0f);
			return _idealDeltaTime;
		}
	}

	/// <summary>Gets damageCameraShakeDuration property.</summary>
	public FloatRange damageCameraShakeDuration { get { return _damageCameraShakeDuration; } }

	/// <summary>Gets damageCameraShakeSpeed property.</summary>
	public FloatRange damageCameraShakeSpeed { get { return _damageCameraShakeSpeed; } }

	/// <summary>Gets damageCameraShakeMagnitude property.</summary>
	public FloatRange damageCameraShakeMagnitude { get { return _damageCameraShakeMagnitude; } }

	/// <summary>Gets playerTag property.</summary>
	public GameObjectTag playerTag { get { return _playerTag; } }

	/// <summary>Gets enemyTag property.</summary>
	public GameObjectTag enemyTag { get { return _enemyTag; } }

	/// <summary>Gets playerWeaponTag property.</summary>
	public GameObjectTag playerWeaponTag { get { return _playerWeaponTag; } }

	/// <summary>Gets enemyWeaponTag property.</summary>
	public GameObjectTag enemyWeaponTag { get { return _enemyWeaponTag; } }

	/// <summary>Gets playerProjectileTag property.</summary>
	public GameObjectTag playerProjectileTag { get { return _playerProjectileTag; } }

	/// <summary>Gets enemyProjectileTag property.</summary>
	public GameObjectTag enemyProjectileTag { get { return _enemyProjectileTag; } }

	/// <summary>Gets explodableTag property.</summary>
	public GameObjectTag explodableTag { get { return _explodableTag; } }

	/// <summary>Gets floorTag property.</summary>
	public GameObjectTag floorTag { get { return _floorTag; } }

	/// <summary>Gets wallTag property.</summary>
	public GameObjectTag wallTag { get { return _wallTag; } }

	/// <summary>Gets ceilingTag property.</summary>
	public GameObjectTag ceilingTag { get { return _ceilingTag; } }

	/// <summary>Gets outOfBoundsTag property.</summary>
	public GameObjectTag outOfBoundsTag { get { return _outOfBoundsTag; } }

	/// <summary>Gets outOfBoundsLayer property.</summary>
	public LayerValue outOfBoundsLayer { get { return _outOfBoundsLayer; } }

	/// <summary>Gets surfaceLayer property.</summary>
	public LayerValue surfaceLayer { get { return _surfaceLayer; } }

	/// <summary>Gets projectiles property.</summary>
	public Projectile[] projectiles { get { return _projectiles; } }

	/// <summary>Gets poolObjects property.</summary>
	public PoolGameObject[] poolObjects { get { return _poolObjects; } }

	/// <summary>Gets FSMLoops property.</summary>
	public FiniteStateAudioClip[] FSMLoops { get { return _FSMLoops; } }

	/// <summary>Gets looper property.</summary>
	public SoundEffectLooper looper { get { return _looper; } }

	/// <summary>Gets loops property.</summary>
	public AudioClip[] loops { get { return _loops; } }

	/// <summary>Gets soundEffects property.</summary>
	public AudioClip[] soundEffects { get { return _soundEffects; } }

	/// <summary>Gets particleEffects property.</summary>
	public ParticleEffect[] particleEffects { get { return _particleEffects; } }

	/// <summary>Gets explodables property.</summary>
	public Explodable[] explodables { get { return _explodables; } }

	/// <summary>Gets and Sets allFactionsTags property.</summary>
	public GameObjectTag[] allFactionsTags
	{
		get { return _allFactionsTags; }
		private set { _allFactionsTags = value; }
	}

	/// <summary>Gets and Sets allWeaponsTags property.</summary>
	public GameObjectTag[] allWeaponsTags
	{
		get { return _allWeaponsTags; }
		private set { _allWeaponsTags = value; }
	}

	/// <summary>Gets and Sets allProjectilesTags property.</summary>
	public GameObjectTag[] allProjectilesTags
	{
		get { return _allProjectilesTags; }
		private set { _allProjectilesTags = value; }
	}

	/// <summary>Gets directionTowardsBackground property.</summary>
	public Vector3 directionTowardsBackground { get { return stareAtBackgroundRotation * Vector3.forward; } }

	/// <summary>Gets directionTowardsPlayer property.</summary>
	public Vector3 directionTowardsPlayer { get { return stareAtPlayerRotation * Vector3.forward; } }
#endregion

	/// This finally fixed the camera issues: https://docs.unity3d.com/ScriptReference/Time-maximumDeltaTime.html
	/// <summary>Initializes Game's Data.</summary>
	public void Initialize()
	{
		Application.targetFrameRate = frameRate;
		Time.maximumDeltaTime = idealDeltaTime;
		Time.fixedDeltaTime = idealDeltaTime;

		allFactionsTags = new GameObjectTag[] { playerTag, enemyTag };
		allWeaponsTags = new GameObjectTag[] { playerWeaponTag, enemyWeaponTag };
		allProjectilesTags = new GameObjectTag[] { playerProjectileTag, enemyProjectileTag };

/*#if UNITY_EDITOR
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("Initializing Game's Data...");
		builder.Append("Frame Rate: ");
		builder.AppendLine(Application.targetFrameRate.ToString());
		builder.Append("Ideal Delta Time: ");
		builder.AppendLine(idealDeltaTime.ToString());
		builder.Append("Fixed Delta Time: ");
		builder.Append(Time.fixedDeltaTime);

		VDebug.Log(builder.ToString());
#endif*/

#if UNITY_SWITCH
		/// https://developer.nintendo.com/html/online-docs/g1kr9vj6-en/Packages/middleware/UnityForNintendoSwitch/Documents/contents-en/Pages/Page_220688182.html?highlighttext=light
		Performance.SetConfiguration(
    		Performance.PerformanceMode.Normal,
    		Performance.PerformanceConfiguration.Cpu1020MhzGpu384MhzEmc1331Mhz
    	);

    	QualitySettings.maxQueuedFrames = 2;
#endif
	}

	/// <summary>Resets FSM Loop's States.</summary>
	public void ResetFSMLoopStates()
	{
		if(FSMLoops == null) return;

		foreach(FiniteStateAudioClip FSMLoop in FSMLoops)
		{
			FSMLoop.ResetState();
		}
	}
}
}