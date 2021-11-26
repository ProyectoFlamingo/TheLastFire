using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class DestinoSceneController : Singleton<DestinoSceneController>
{
	public const int INDEX_AUDIOSOURCE_LOOP_ORCHESTRA = 0;
	public const int INDEX_AUDIOSOURCE_LOOP_VOICE = 1;
	public const int INDEX_AUDIOSOURCE_LOOP_PIECES = 1;
	public const float WEIGHT_BLENDSHAPE_CURTAIN_CLOSED = 100.0f; 			/// <summary>Closed Curtain's Blend Shape's Weight.</summary>
	public const float WEIGHT_BLENDSHAPE_CURTAIN_OPEN = 0.0f; 				/// <summary>Open Curtain's Blend Shape's Weight.</summary>

	[SerializeField] private DestinoBoss _destino; 							/// <summary>Destino's Reference.</summary>
	[Space(5f)]
	[SerializeField] private float _cooldownBeforeReleasingPlayerControl; 	/// <summary>Cooldown Duration before releasing Player's Control.</summary>
	[Space(5f)]
	[Header("Scenario's Attributes:")]
	[Space(5f)]
	[Header("Curtain's Settings:")]
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _openingClipPercentage; 				/// <summary>Opening Clip's duration percentage that takes for the curtain to open [at the beginning of the scene].</summary>
	[SerializeField]
	[Range(0.0f, 100.0f)] private float _stage1CurtainClosure; 				/// <summary>Curtain's Closure's Percentage on Stage 1.</summary>
	[SerializeField]
	[Range(0.0f, 100.0f)] private float _stage2CurtainClosure; 				/// <summary>Curtain's Closure's Percentage on Stage 2.</summary>
	[SerializeField]
	[Range(0.0f, 100.0f)] private float _stage3CurtainClosure; 				/// <summary>Curtain's Closure's Percentage on Stage 3.</summary>
	[SerializeField] private float _curtainsClosureDuration; 				/// <summary>Curtains' Closure Duration.</summary>
	[SerializeField] private float _curtainsApertureDuration; 				/// <summary>Curtains' Aperture Duration.</summary>
	[SerializeField] private float _cooldownBeforeAperture; 				/// <summary>Cooldown's duration before curtains' aperture.</summary>
	[Space(5f)]
	[Header("Stage's Sceneries:")]
	[SerializeField] private GameObject _stage1SceneryGroup; 				/// <summary>Stage 1's Scenery Group.</summary>
	[SerializeField] private GameObject _stage2SceneryGroup; 				/// <summary>Stage 2's Scenery Group.</summary>
	[SerializeField] private GameObject _stage3SceneryGroup; 				/// <summary>Stage 3's Scenery Group.</summary>
	[Space(5f)]
	[Header("Lights:")]
	[SerializeField] private Light _moonLight; 								/// <summary>Moon's Light.</summary>
	[SerializeField] private Light _mateoSpotLight; 						/// <summary>Mateo's Spot Light.</summary>
	[SerializeField] private Light _destinoSpotLight; 						/// <summary>Destino's Spot Light.</summary>
	[Space(5f)]
	[SerializeField] private float _mateoSpotLightMaxSpeed; 				/// <summary>Mateo's Spot Light's Maximum Speed.</summary>
	[SerializeField] private float _mateoSpotLightMaxSteeringForce; 		/// <summary>Mateo's Spot Light's Maximum Steering Force.</summary>
	[Space(5f)]
	[SerializeField] private SkinnedMeshRenderer _leftCurtainRenderer; 		/// <summary>Left Curtain's SkinnedMeshRenderer.</summary>
	[SerializeField] private SkinnedMeshRenderer _rightCurtainRenderer; 	/// <summary>Right Curtain's SkinnedMeshRenderer.</summary>
	[Space(5f)]
	[Header("Devil's Scenery:")]
	[SerializeField] private Health _devilCeiling; 							/// <summary>Devil's Ceiling.</summary>
	[SerializeField] private Health _leftDevilTower; 						/// <summary>Left Tower [appears on the Devil's behavior].</summary>
	[SerializeField] private Health _rightDevilTower; 						/// <summary>Right Tower [appears on the Devil's behavior].</summary>
	[Space(10f)]
	[Header("Signs:")]
	[SerializeField] private Transform _fireShowSign; 						/// <summary>Fire Show's Sign.</summary>
	[SerializeField] private Transform _swordShowSign; 						/// <summary>Sword Show's Sign.</summary>
	[SerializeField] private Transform _danceShowSign; 						/// <summary>Dance Show's Sign.</summary>
	[Space(5f)]
	[Header("Loops' Indices:")]
	[SerializeField] private CollectionIndex _mainLoopIndex; 				/// <summary>Main Loop's Index.</summary>
	[SerializeField] private CollectionIndex _mainLoopVoiceIndex; 			/// <summary>Main Loop's Voice Index.</summary>
	[Space(5f)]
	[Header("Sound Effects' Indices:")]
	[SerializeField] private CollectionIndex _orchestraTunningSoundIndex; 	/// <summary>Orchestra Tunning Sound FX's Index.</summary>
	[SerializeField] private CollectionIndex _curtainOpeningSoundIndex; 	/// <summary>Curtain's Opening Sound FX's Index.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private Color color; 									/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float radius; 									/// <summary>Waypoints' Radius.</summary>
	[SerializeField] private float rayLength; 								/// <summary>Ray's Length.</summary>
#endif
	private Vector2 mateoSpotLightVelocity; 								/// <summary>Mateo Spot Light's Velocity reference.</summary>
	private bool _deckPresented; 											/// <summary>Was the deck already presented tomm the Player?.</summary>
	private bool _curtainOpened; 											/// <summary>Has the curtain been opened?.</summary>

#region Getters/Setters:
	/// <summary>Gets destino property.</summary>
	public DestinoBoss destino { get { return _destino; } }

	/// <summary>Gets stage1SceneryGroup property.</summary>
	public GameObject stage1SceneryGroup { get { return _stage1SceneryGroup; } }

	/// <summary>Gets stage2SceneryGroup property.</summary>
	public GameObject stage2SceneryGroup { get { return _stage2SceneryGroup; } }

	/// <summary>Gets stage3SceneryGroup property.</summary>
	public GameObject stage3SceneryGroup { get { return _stage3SceneryGroup; } }

	/// <summary>Gets moonLight property.</summary>
	public Light moonLight { get { return _moonLight; } }

	/// <summary>Gets mateoSpotLight property.</summary>
	public Light mateoSpotLight { get { return _mateoSpotLight; } }

	/// <summary>Gets destinoSpotLight property.</summary>
	public Light destinoSpotLight { get { return _destinoSpotLight; } }

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

	/// <summary>Gets mateoSpotLightMaxSpeed property.</summary>
	public float mateoSpotLightMaxSpeed { get { return _mateoSpotLightMaxSpeed; } }

	/// <summary>Gets mateoSpotLightMaxSteeringForce property.</summary>
	public float mateoSpotLightMaxSteeringForce { get { return _mateoSpotLightMaxSteeringForce; } }

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
	public CollectionIndex mainLoopIndex { get { return _mainLoopIndex; } }

	/// <summary>Gets mainLoopVoiceIndex property.</summary>
	public CollectionIndex mainLoopVoiceIndex { get { return _mainLoopVoiceIndex; } }

	/// <summary>Gets orchestraTunningSoundIndex property.</summary>
	public CollectionIndex orchestraTunningSoundIndex { get { return _orchestraTunningSoundIndex; } }

	/// <summary>Gets curtainOpeningSoundIndex property.</summary>
	public CollectionIndex curtainOpeningSoundIndex { get { return _curtainOpeningSoundIndex; } }

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
	
#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		/// ... ?
	}
#endif

	public bool test;
	public int testLoopState;

	/// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
   	protected override void OnAwake()
	{
		deckPresented = false;
		curtainOpened = false;

		/// Deactivate Scenery Objects:
		devilCeiling.gameObject.SetActive(false);
		leftDevilTower.gameObject.SetActive(false);
		rightDevilTower.gameObject.SetActive(false);



		/// Subscribe to Mateo & Destino's Events:
		destino.eventsHandler.onIDEvent += OnDestinoIDEvent;
		Game.mateo.eventsHandler.onIDEvent += OnMateoIDEvent;

		Game.ResetFSMLoopStates();

		AudioClip clip = AudioController.PlayOneShot(SourceType.Scenario, 0, orchestraTunningSoundIndex);
		CloseCurtainsWithWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, 0.0f, null);
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		Game.mateo.Meditate();

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
		SetSpotlightAboveMateo();
	}

#region Callbacks:
	/// <summary>Callback invoked when Destino invokes an ID's Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnDestinoIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_STAGECHANGED:
			int stageID = destino.currentStage;

			switch(stageID)
			{
				case Boss.STAGE_1:
				ActivateSceneryGroup(stageID);
				break;

				case Boss.STAGE_2:
				CloseCurtainsWithWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, curtainsClosureDuration, ()=>
				{
					ActivateSceneryGroup(stageID);
					this.StartCoroutine(this.WaitSeconds(cooldownBeforeAperture, ()=>
					{
						CloseCurtainsWithWeight(stage2CurtainClosure, curtainsApertureDuration, null);
					}));
				});
				break;

				case Boss.STAGE_3:
				CloseCurtainsWithWeight(WEIGHT_BLENDSHAPE_CURTAIN_CLOSED, curtainsClosureDuration, ()=>
				{
					ActivateSceneryGroup(stageID);
					this.StartCoroutine(this.WaitSeconds(cooldownBeforeAperture, ()=>
					{
						CloseCurtainsWithWeight(stage3CurtainClosure, curtainsApertureDuration, null);
					}));
				});
				break;
			}
			break;

			case IDs.EVENT_DEATHROUTINE_BEGINS:
			break;

			case IDs.EVENT_DEATHROUTINE_ENDS:
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
			if(curtainOpened) return;

			curtainOpened = true;
			
			AudioClip openingClip = AudioController.PlayOneShot(SourceType.Scenario, 0, curtainOpeningSoundIndex);
			CloseCurtainsWithWeight(stage1CurtainClosure, openingClip.length * openingClipPercentage, ()=>
			{
#region TEST_DESTINO_SINGING
				if(test)
				{
					Game.data.FSMLoops[mainLoopIndex].ChangeState(testLoopState);
					Game.data.FSMLoops[mainLoopVoiceIndex].ChangeState(testLoopState);
					//destino.Sing();
				}
#endregion

				AudioController.PlayFSMLoop(0, mainLoopIndex, true);
				AudioController.PlayFSMLoop(1, mainLoopVoiceIndex, true);
				destino.Sing();
				destino.RequestCard();
			});
			break;
		}
	}
#endregion

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

	/// \TODO Make the light follow Mateo with Steering Behavior.
	/// <summary>Sets Spotlight above Mateo.</summary>
	private void SetSpotlightAboveMateo()
	{
		if(Game.mateo == null || mateoSpotLight == null) return;

		Vector3 mateoPosition = Game.mateo.transform.position;
		mateoPosition.y = mateoSpotLight.transform.position.y;
		mateoSpotLight.transform.position = mateoPosition;
		
		/*Vector3 seekForce = (Vector3)SteeringVehicle2D.GetSeekForce(mateoSpotLight.transform.position, mateoPosition, ref mateoSpotLightVelocity, mateoSpotLightMaxSpeed, mateoSpotLightMaxSteeringForce);
		
		mateoSpotLight.transform.position += seekForce * Time.deltaTime;*/
	}

	/// <summary>Changes the state of the Curtain.</summary>
	/// <param name="_open">Should the curtain be opened?.</param>
	/// <param name="_duration">Duration of the state change.</param>
	/// <param name="onStateChangingEnds">Optional callback invoked when the change of state of the curtain ends.</param>
	public void CloseCurtainsWithWeight(float _closurePercentage, float _duration, Action onStateChangingEnds = null)
	{
		this.StartCoroutine(ChangeCurtainsState(_closurePercentage, _duration, onStateChangingEnds));
	}

	/// <summary>Changes the state of the Curtain.</summary>
	/// <param name="_open">Should the curtain be opened?.</param>
	/// <param name="_duration">Duration of the state change.</param>
	/// <param name="onStateChangingEnds">Optional callback invoked when the change of state of the curtain ends.</param>
	public static IEnumerator ChangeCurtainsState(float _closurePercentage, float _duration, Action onStateChangingEnds = null)
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
}
}