using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class DestinoSceneController : Singleton<DestinoSceneController>
{
	public const float WEIGHT_BLENDSHAPE_CURTAIN_CLOSED = 100.0f; 												/// <summary>Closed Curtain's Blend Shape's Weight.</summary>
	public const float WEIGHT_BLENDSHAPE_CURTAIN_OPEN = 0.0f; 													/// <summary>Open Curtain's Blend Shape's Weight.</summary>

	[SerializeField] private DestinoBoss _destino; 																/// <summary>Destino's Reference.</summary>
	[SerializeField] private DestinoBossAIController _destinoController; 										/// <summary>Destino's AI Controller.</summary>
	[SerializeField] private Vector3Pair _destinoInitialSpawnPointPair; 										/// <summary>Spawn Point pair that refers for Destino's initial position.</summary>
	[SerializeField] private float _initialPointLerpDuration; 													/// <summary>Interpolation duration from pair point B to pair point A.</summary>
	[Space(5f)]
	[SerializeField] private float _cooldownBeforeReleasingPlayerControl; 										/// <summary>Cooldown Duration before releasing Player's Control.</summary>
	[Space(5f)]
	[Header("Curtain's Settings:")]
	[Space(5f)]
	[SerializeField]
	[TabGroup("Scenery Group", "Curtains")][Range(0.0f, 1.0f)] private float _openingClipPercentage; 			/// <summary>Opening Clip's duration percentage that takes for the curtain to open [at the beginning of the scene].</summary>
	[SerializeField]
	[TabGroup("Scenery Group", "Curtains")][Range(0.0f, 100.0f)] private float _stage1CurtainClosure; 			/// <summary>Curtain's Closure's Percentage on Stage 1.</summary>
	[SerializeField]
	[TabGroup("Scenery Group", "Curtains")][Range(0.0f, 100.0f)] private float _stage2CurtainClosure; 			/// <summary>Curtain's Closure's Percentage on Stage 2.</summary>
	[SerializeField]
	[TabGroup("Scenery Group", "Curtains")][Range(0.0f, 100.0f)] private float _stage3CurtainClosure; 			/// <summary>Curtain's Closure's Percentage on Stage 3.</summary>
	[TabGroup("Scenery Group", "Curtains")][SerializeField] private float _curtainsClosureDuration; 			/// <summary>Curtains' Closure Duration.</summary>
	[TabGroup("Scenery Group", "Curtains")][SerializeField] private float _curtainsApertureDuration; 			/// <summary>Curtains' Aperture Duration.</summary>
	[TabGroup("Scenery Group", "Curtains")][SerializeField] private float _cooldownBeforeAperture; 				/// <summary>Cooldown's duration before curtains' aperture.</summary>
	[Space(5f)]
	[Header("Stage's Sceneries:")]
	[TabGroup("Scenery Groups")][SerializeField] private GameObject _stage1SceneryGroup; 						/// <summary>Stage 1's Scenery Group.</summary>
	[TabGroup("Scenery Groups")][SerializeField] private GameObject _stage2SceneryGroup; 						/// <summary>Stage 2's Scenery Group.</summary>
	[TabGroup("Scenery Groups")][SerializeField] private GameObject _stage3SceneryGroup; 						/// <summary>Stage 3's Scenery Group.</summary>
	[Space(5f)]
	[Header("Lights:")]
	[TabGroup("Lights")][SerializeField] private Light _moonLight; 												/// <summary>Moon's Light.</summary>
	[TabGroup("Lights")][SerializeField] private Light _thunderLight; 											/// <summary>Thunder's Light.</summary>
	[TabGroup("Lights")][SerializeField] private Light _mateoSpotLight; 										/// <summary>Mateo's Spot Light.</summary>
	[TabGroup("Lights")][SerializeField] private Light _destinoSpotLight; 										/// <summary>Destino's Spot Light.</summary>
	[Space(5f)]
	[TabGroup("Lights")][SerializeField] private float _spotLightMaxSpeed; 										/// <summary>Spot Light's Maximum Speed.</summary>
	[TabGroup("Lights")][SerializeField] private float _spotLightMaxSteeringForce; 								/// <summary>Spot Light's Maximum Steering Force.</summary>
	[TabGroup("Lights")][SerializeField] private float _spotLightArrivalRadius; 								/// <summary>Spot Light's Arrival Radius.</summary>
	[Space(5f)]
	[TabGroup("Scenery Group", "Curtains")][SerializeField] private SkinnedMeshRenderer _leftCurtainRenderer; 	/// <summary>Left Curtain's SkinnedMeshRenderer.</summary>
	[TabGroup("Scenery Group", "Curtains")][SerializeField] private SkinnedMeshRenderer _rightCurtainRenderer; 	/// <summary>Right Curtain's SkinnedMeshRenderer.</summary>
	[Space(5f)]
	[Header("Devil's Scenery:")]
	[TabGroup("Scenery Group", "Devil's Scenery")][SerializeField] private Health _devilCeiling; 				/// <summary>Devil's Ceiling.</summary>
	[TabGroup("Scenery Group", "Devil's Scenery")][SerializeField] private Health _leftDevilTower; 				/// <summary>Left Tower [appears on the Devil's behavior].</summary>
	[TabGroup("Scenery Group", "Devil's Scenery")][SerializeField] private Health _rightDevilTower; 			/// <summary>Right Tower [appears on the Devil's behavior].</summary>
	[Space(10f)]
	[Header("Signs:")]
	[TabGroup("Scenery Group", "Judgement's Scenery")][SerializeField] private Transform _fireShowSign; 		/// <summary>Fire Show's Sign.</summary>
	[TabGroup("Scenery Group", "Judgement's Scenery")][SerializeField] private Transform _swordShowSign; 		/// <summary>Sword Show's Sign.</summary>
	[TabGroup("Scenery Group", "Judgement's Scenery")][SerializeField] private Transform _danceShowSign; 		/// <summary>Dance Show's Sign.</summary>
	[Space(5f)]
	[Header("Loops' Indices:")]
	[TabGroup("Audio")][SerializeField] private int _mainLoopIndex; 											/// <summary>Main Loop's Index.</summary>
	[TabGroup("Audio")][SerializeField] private int _mainLoopVoiceIndex; 										/// <summary>Main Loop's Voice Index.</summary>
	[TabGroup("Audio")][SerializeField] private AudioLoopData _mainLoopLoopData; 								/// <summary>Main Loop's  Data.</summary>
	[TabGroup("Audio")][SerializeField] private AudioLoopData _mainLoopVoiceLoopData; 							/// <summary>Main Loop's Voice Loop Data.</summary>
	[Space(5f)]
	[Header("Sound Effects' Indices:")]
	[TabGroup("Audio")][SerializeField] private int _orchestraTunningSoundIndex; 								/// <summary>Orchestra Tunning Sound FX's Index.</summary>
	[TabGroup("Audio")][SerializeField] private int _curtainOpeningSoundIndex; 									/// <summary>Curtain's Opening Sound FX's Index.</summary>
	[Space(5f)]
	[TabGroup("Dead Cutscene")]
	[Header("Dead's Cutscene:")]
	[SerializeField] private float _destinoDeadDistance; 														/// <summary>Camera's distance for Destino.</summary>
//#if UNITY_EDITOR
	[Space(5f)]
	[Header("Testing:")]
	[TabGroup("Testing Group", "Testing")][SerializeField] private bool test; 									/// <summary>Test?.</summary>
	[TabGroup("Testing Group", "Testing")][SerializeField] private int testLoopState; 							/// <summary>Loop's State testing index.</summary>
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[TabGroup("Testing Group", "Gizmos")][SerializeField] private Color color; 									/// <summary>Gizmos' Color.</summary>
	[TabGroup("Testing Group", "Gizmos")][SerializeField] private float radius; 								/// <summary>Waypoints' Radius.</summary>
	[TabGroup("Testing Group", "Gizmos")][SerializeField] private float rayLength; 								/// <summary>Ray's Length.</summary>
//#endif
	private Vector2 mateoSpotLightVelocity; 																	/// <summary>Mateo Spot Light's Velocity reference.</summary>
	private Vector2 destinoSpotLightVelocity; 																	/// <summary>Destino Spot Light's Velocity reference.</summary>
	private bool _deckPresented; 																				/// <summary>Was the deck already presented tomm the Player?.</summary>
	private bool _curtainOpened; 																				/// <summary>Has the curtain been opened?.</summary>

#region Getters/Setters:
	/// <summary>Gets destino property.</summary>
	public DestinoBoss destino { get { return _destino; } }

	/// <summary>Gets destinoController property.</summary>
	public DestinoBossAIController destinoController { get { return _destinoController; } }

	/// <summary>Gets destinoInitialSpawnPointPair property.</summary>
	public Vector3Pair destinoInitialSpawnPointPair { get { return _destinoInitialSpawnPointPair; } }

	/// <summary>Gets stage1SceneryGroup property.</summary>
	public GameObject stage1SceneryGroup { get { return _stage1SceneryGroup; } }

	/// <summary>Gets stage2SceneryGroup property.</summary>
	public GameObject stage2SceneryGroup { get { return _stage2SceneryGroup; } }

	/// <summary>Gets stage3SceneryGroup property.</summary>
	public GameObject stage3SceneryGroup { get { return _stage3SceneryGroup; } }

	/// <summary>Gets moonLight property.</summary>
	public Light moonLight { get { return _moonLight; } }

	/// <summary>Gets thunderLight property.</summary>
	public Light thunderLight { get { return _thunderLight; } }

	/// <summary>Gets mateoSpotLight property.</summary>
	public Light mateoSpotLight { get { return _mateoSpotLight; } }

	/// <summary>Gets destinoSpotLight property.</summary>
	public Light destinoSpotLight { get { return _destinoSpotLight; } }

	/// <summary>Gets initialPointLerpDuration property.</summary>
	public float initialPointLerpDuration { get { return _initialPointLerpDuration; } }

	/// <summary>Gets openingClipPercentage property.</summary>
	public float openingClipPercentage { get { return _openingClipPercentage; } }

	/// <summary>Gets cooldownBeforeReleasingPlayerControl property.</summary>
	public float cooldownBeforeReleasingPlayerControl { get { return _cooldownBeforeReleasingPlayerControl; } }

	/// <summary>Gets stage1CurtainClosure property.</summary>
	public float stage1CurtainClosure { get { return _stage1CurtainClosure; } }

	/// <summary>Gets stage2CurtainClosure property.</summary>
	public float stage2CurtainClosure { get { return _stage2CurtainClosure; } }

	/// <summary>Gets stage3CurtainClosure property.</summary>
	public float stage3CurtainClosure { get { return _stage3CurtainClosure; } }

	/// <summary>Gets curtainsClosureDuration property.</summary>
	public float curtainsClosureDuration { get { return _curtainsClosureDuration; } }

	/// <summary>Gets curtainsApertureDuration property.</summary>
	public float curtainsApertureDuration { get { return _curtainsApertureDuration; } }

	/// <summary>Gets cooldownBeforeAperture property.</summary>
	public float cooldownBeforeAperture { get { return _cooldownBeforeAperture; } }

	/// <summary>Gets spotLightMaxSpeed property.</summary>
	public float spotLightMaxSpeed { get { return _spotLightMaxSpeed; } }

	/// <summary>Gets spotLightMaxSteeringForce property.</summary>
	public float spotLightMaxSteeringForce { get { return _spotLightMaxSteeringForce; } }

	/// <summary>Gets spotLightArrivalRadius property.</summary>
	public float spotLightArrivalRadius { get { return _spotLightArrivalRadius; } }

	/// <summary>Gets destinoDeadDistance property.</summary>
	public float destinoDeadDistance { get { return _destinoDeadDistance; } }

	/// <summary>Gets leftCurtainRenderer property.</summary>
	public SkinnedMeshRenderer leftCurtainRenderer { get { return _leftCurtainRenderer; } }

	/// <summary>Gets rightCurtainRenderer property.</summary>
	public SkinnedMeshRenderer rightCurtainRenderer { get { return _rightCurtainRenderer; } }

	/// <summary>Gets devilCeiling property.</summary>
	public Health devilCeiling { get { return _devilCeiling; } }

	/// <summary>Gets leftDevilTower property.</summary>
	public Health leftDevilTower { get { return _leftDevilTower; } }

	/// <summary>Gets rightDevilTower property.</summary>
	public Health rightDevilTower { get { return _rightDevilTower; } }

	/// <summary>Gets fireShowSign property.</summary>
	public Transform fireShowSign { get { return _fireShowSign; } }

	/// <summary>Gets swordShowSign property.</summary>
	public Transform swordShowSign { get { return _swordShowSign; } }

	/// <summary>Gets danceShowSign property.</summary>
	public Transform danceShowSign { get { return _danceShowSign; } }

	/// <summary>Gets mainLoopIndex property.</summary>
	public int mainLoopIndex { get { return _mainLoopIndex; } }

	/// <summary>Gets mainLoopVoiceIndex property.</summary>
	public int mainLoopVoiceIndex { get { return _mainLoopVoiceIndex; } }

	/// <summary>Gets orchestraTunningSoundIndex property.</summary>
	public int orchestraTunningSoundIndex { get { return _orchestraTunningSoundIndex; } }

	/// <summary>Gets curtainOpeningSoundIndex property.</summary>
	public int curtainOpeningSoundIndex { get { return _curtainOpeningSoundIndex; } }

	/// <summary>Gets mainLoopLoopData property.</summary>
	public AudioLoopData mainLoopLoopData { get { return _mainLoopLoopData; } }

	/// <summary>Gets mainLoopVoiceLoopData property.</summary>
	public AudioLoopData mainLoopVoiceLoopData { get { return _mainLoopVoiceLoopData; } }

	/// <summary>Gets and Sets deckPresented property.</summary>
	public bool deckPresented
	{
		get { return _deckPresented; }
		set { _deckPresented = value; }
	}

	/// <summary>Gets and Sets curtainOpened property.</summary>
	public bool curtainOpened
	{
		get { return _curtainOpened; }
		set { _curtainOpened = value; }
	}
#endregion
	
//#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawWireSphere(destinoInitialSpawnPointPair.a, 0.5f);
		Gizmos.DrawWireSphere(destinoInitialSpawnPointPair.b, 0.5f);
	}
//#endif

	/// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
   	protected override void OnAwake()
	{
		deckPresented = false;
		curtainOpened = false;

		/// Deactivate Scenery Objects:
		devilCeiling.gameObject.SetActive(false);
		leftDevilTower.gameObject.SetActive(false);
		rightDevilTower.gameObject.SetActive(false);
		fireShowSign.gameObject.SetActive(false);
		swordShowSign.gameObject.SetActive(false);
		danceShowSign.gameObject.SetActive(false);

		/// Subscribe to Mateo & Destino's Events:
		destino.eventsHandler.onIDEvent += OnDestinoIDEvent;
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		destino.transform.position = destinoInitialSpawnPointPair.b;

		Game.mateo.Meditate();
		Game.mateo.eventsHandler.onIDEvent += OnMateoIDEvent;
		Game.ResetFSMLoopStates();

		AudioClip clip = AudioController.Play(SourceType.Scenario, 0, orchestraTunningSoundIndex);
		SetCurtainsWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, 0.0f, null);

		/// Turn-off Player's Control:
		if(cooldownBeforeReleasingPlayerControl > 0.0f)
		{
			Game.EnablePlayerControl(false);
			StartCoroutine(
				this.WaitSeconds(
					cooldownBeforeReleasingPlayerControl,
					()=> { Game.mateoController.enabled = true; }
				)
			);
		}
	}

	/// <summary>Updates DestinoSceneController's instance at each frame.</summary>
	private void Update()
	{
		SetSpotlightAbove(mateoSpotLight, Game.mateo.transform.position, ref mateoSpotLightVelocity);
		SetSpotlightAbove(destinoSpotLight, destino.transform.position, ref destinoSpotLightVelocity);
	}

#region Callbacks:
	/// <summary>Callback invoked when Destino invokes an ID's Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnDestinoIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_STAGECHANGED:
			{
				int stageID = destino.currentStage;

				switch(stageID)
				{
					case Boss.STAGE_1:
					/*
					- Close the curtain
					- Enable the scenery group #1
					*/
					SetCurtainsWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, 0.0f);
					ActivateSceneryGroup(stageID);
					break;

					case Boss.STAGE_2:
					/*
					- Close the curtain
					- Enable the scenery group #2
					- Open the Curtain
					*/
					SetCurtainsWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, curtainsClosureDuration, ()=>
					{
						ActivateSceneryGroup(stageID);
						this.StartCoroutine(this.WaitSeconds(cooldownBeforeAperture, ()=>
						{
							SetCurtainsWeight(stage2CurtainClosure, curtainsApertureDuration, null);
						}));
					});
					break;

					case Boss.STAGE_3:
					/*
					- Close the curtain
					- Enable the scenery group #3
					- Open the Curtain
					*/
					SetCurtainsWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, curtainsClosureDuration, ()=>
					{
						ActivateSceneryGroup(stageID);
						this.StartCoroutine(this.WaitSeconds(cooldownBeforeAperture, ()=>
						{
							SetCurtainsWeight(stage3CurtainClosure, curtainsApertureDuration, null);
						}));
					});
					break;
				}
			}
			break;

			case IDs.EVENT_DEATHROUTINE_BEGINS:
			SetCurtainsWeight(WEIGHT_BLENDSHAPE_CURTAIN_OPEN, 0.0f);
			Game.EnablePlayerControl(false);
			Game.mateo.CancelAllActions();
			Game.cameraController.distanceAdjuster.distanceRange = destinoDeadDistance;
			Game.AddTargetToCamera(destino.cameraTarget);
			break;

			case IDs.EVENT_DEATHROUTINE_ENDS:
			AudioController.Stop(SourceType.Loop, 0);
			AudioController.Stop(SourceType.Loop, 1);
			SetCurtainsWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, curtainsClosureDuration,
			()=>
			{
				Mateo mateo = Game.mateo;
				mateo.rotationAbility.DoRotateTowardsRotationRoutine(mateo.animatorParent, Game.data.stareAtPlayerRotation, 0.1f,
				()=>
				{
					mateo.animatorController.CrossFadeAndWait(mateo.bowCredential, mateo.clipFadeDuration, mateo.mainAnimationLayer, 0.0f, 0.0f,
					()=>
					{
						//Debug.Log("[DestinoSceneController] Now What?");
					});
				});
			});
			break;
		}
	}

	/// <summary>Callback invoked when Mateo invokes an ID's Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnMateoIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_MEDITATION_ENDS:
			{
				/*
					- Play Applause's Sound Effect.
					- Stop Orchestra Tunning's Loop.
					- Open the curtain.
					- Play the Finite-State AudioClips for Destino's Loops (Intrumental and voice).
					- Make Destino Sing and request a Card
				*/
				if(curtainOpened) return;

				curtainOpened = true;
				
				AudioClip openingClip = AudioController.PlayOneShot(SourceType.Scenario, 1, curtainOpeningSoundIndex);

				AudioController.Stop(SourceType.Scenario, 0);
				SetCurtainsWeight(stage1CurtainClosure, openingClip.length * openingClipPercentage, ()=>
				{
//#if UNITY_EDITOR
					/*if(test)
					{
						Game.data.FSMLoops[mainLoopIndex].ChangeState(testLoopState);
						Game.data.FSMLoops[mainLoopVoiceIndex].ChangeState(testLoopState);
						//destino.Sing();
					}*/
//#endif
					/*AudioController.PlayFSMLoop(0, mainLoopIndex, true);
					AudioController.PlayFSMLoop(1, mainLoopVoiceIndex, true, destino.Sing);*/
					AudioController.PlayFSMLoops(destino.Sing, mainLoopLoopData, mainLoopVoiceLoopData);
					
					destinoController.RequestCard();

					// Thunder Test
					//this.StartCoroutine(thunderLight.StormLightningEffectRoutine(0.25f, 15, 1.5f, 1.0f, 0.7f, 8.5f, 2.0f, 1.3f));
				});
			}
			break;
		}
	}
#endregion

	public static void ContinueLoop()
	{
		DestinoSceneController s = Instance;
		AudioController.PlayFSMLoops(s.destino.Sing, s.mainLoopLoopData, s.mainLoopVoiceLoopData);
	}

#region PrivateMethods:
	/// <summary>Activates Scenery Group related to the given stage.</summary>
	/// <param name="_stageID">Stage's ID.</param>
	private void ActivateSceneryGroup(int _stageID)
	{
		stage1SceneryGroup.SetActive(false);
		stage2SceneryGroup.SetActive(false);
		stage3SceneryGroup.SetActive(false);

		switch(_stageID)
		{
			case Boss.STAGE_1:
			stage1SceneryGroup.SetActive(true);
			break;

			case Boss.STAGE_2:
			stage2SceneryGroup.SetActive(true);
			break;

			case Boss.STAGE_3:
			stage3SceneryGroup.SetActive(true);
			break;
		}
	}

	/// <summary>Sets given Spot-Light above given point.</summary>
	/// <param name="_spotLight">Spot-Light to position.</param>
	/// <param name="_position">Desired point of interest.</param>
	/// <param name="velocity">Velocity's Reference.</param>
	private void SetSpotlightAbove(Light _spotLight, Vector3 _position, ref Vector2 velocity)
	{
		if(_spotLight == null) return;

		Vector3 lightPosition = _spotLight.transform.position;
		_position.y = lightPosition.y;
		
		Vector3 seekForce = (Vector3)SteeringVehicle2D.GetSeekForce(lightPosition, _position, ref velocity, spotLightMaxSpeed, spotLightMaxSteeringForce);
		float weight = SteeringVehicle2D.GetArrivalWeight(lightPosition, _position, spotLightArrivalRadius);

		lightPosition += seekForce * Time.deltaTime * weight;
		lightPosition.z = _position.z;

		_spotLight.transform.position = lightPosition;
	}

	/// <summary>Changes the state of the Curtain.</summary>
	/// <param name="_open">Should the curtain be opened?.</param>
	/// <param name="_duration">Duration of the state change.</param>
	/// <param name="onStateChangingEnds">Optional callback invoked when the change of state of the curtain ends.</param>
	private void SetCurtainsWeight(float _closurePercentage, float _duration, Action onStateChangingEnds = null)
	{
		this.StartCoroutine(ChangeCurtainsState(_closurePercentage, _duration, onStateChangingEnds));
	}
#endregion

#region Coroutines:
	/// <summary>Takes Destino into the initial point.</summary>
	/// <param name="onInterpolationEnds">Optional callback invoked when the interpolation ends [null by default].</param>
	public static IEnumerator TakeDestinoToInitialPoint(Action onInterpolationEnds = null)
	{
		Transform destinoTransform = Instance.destino.transform;
		Vector3Pair pair = Instance.destinoInitialSpawnPointPair;

		destinoTransform.rotation = Quaternion.identity;
		destinoTransform.position = pair.a;
		
		IEnumerator routine = destinoTransform.DisplaceToPosition(pair.b, Instance.initialPointLerpDuration, onInterpolationEnds, VMath.EaseInQuad);

		while(routine.MoveNext()) yield return null;
	}

	/// <summary>Changes the state of the Curtain.</summary>
	/// <param name="_open">Should the curtain be opened?.</param>
	/// <param name="_duration">Duration of the state change.</param>
	/// <param name="onStateChangingEnds">Optional callback invoked when the change of state of the curtain ends.</param>
	private static IEnumerator ChangeCurtainsState(float _closurePercentage, float _duration, Action onStateChangingEnds = null)
	{
		float weight = _closurePercentage;

		if(_duration > 0.0)
		{
			bool keepRunning = true;
			IEnumerator leftCurtainStateChange = Instance.leftCurtainRenderer.SetBlendShapeWeight(0, weight, _duration, null);
			IEnumerator rightCurtainStateChange = Instance.rightCurtainRenderer.SetBlendShapeWeight(0, weight, _duration, null);
			
			do
			{
				leftCurtainStateChange.MoveNext();
				keepRunning = rightCurtainStateChange.MoveNext();
				
				yield return null;

			}while(keepRunning);
		}
		else
		{
			Instance.leftCurtainRenderer.SetBlendShapeWeight(0, weight);
			Instance.rightCurtainRenderer.SetBlendShapeWeight(0, weight);

			yield return null;
		}

		if(onStateChangingEnds != null) onStateChangingEnds();
	}
#endregion
}
}