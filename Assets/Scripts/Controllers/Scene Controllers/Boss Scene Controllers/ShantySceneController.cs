﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Serializable] public struct Vector3Pair { public Vector3 a; public Vector3 b; }

public class ShantySceneController : Singleton<ShantySceneController>
{
	[Space(5f)]
	[SerializeField] private ShantyBoss _shanty; 								/// <summary>Captain Shanty's Reference.</summary>
	[SerializeField] private ShantyBossAIController _shantyController; 			/// <summary>Shanty's AI Controller.</summary>
	[Space(5f)]
	[Header("Ship's Attributes:")]
	[SerializeField] private ShantyShip _shantyShip; 							/// <summary>Shanty's Ship.</summary>
	[Space(5f)]
	[Header("Audio's Attributes:")]
	[SerializeField] private VAssetReference _loopReference; 					/// <summary>Loop's Reference.</summary>
	[Space(5f)]
	[Header("Camera Boundaries' Modifiers:")]
	[SerializeField] private Camera2DBoundariesModifier _stage1CameraSettings; 	/// <summary>Camera Settings for Stage 1.</summary>
	[SerializeField] private Camera2DBoundariesModifier _stage2CameraSettings; 	/// <summary>Camera Settings for Stage 2.</summary>
	[SerializeField] private Camera2DBoundariesModifier _stage3CameraSettings; 	/// <summary>Camera Settings for Stage 3.</summary>
	[Space(5f)]
	[Header("Boundaries:")]
	[SerializeField] private ScenarioBoundariesContainer _dockBoundaries; 		/// <summary>Dock's Boundaries.</summary>
	[SerializeField] private ScenarioBoundariesContainer _shipBoundaries; 		/// <summary>Ship's Boundaries.</summary>
	[Space(5f)]
	[Header("Spawn Positions:")]
	[SerializeField] private Vector3 _stage1MateoPosition; 						/// <summary>Spawn Position for Mateo at Stage 1.</summary>
	[SerializeField] private Vector3 _stage2MateoPosition; 						/// <summary>Spawn Position for Mateo at Stage 2.</summary>
	[SerializeField] private Vector3 _stage3MateoPosition; 						/// <summary>Spawn Position for Mateo at Stage 3.</summary>
	[SerializeField] private Vector3 _stage1ShantyPosition; 					/// <summary>Spawn Position for Shanty at Stage 1.</summary>
	[SerializeField] private Vector3 _stage2ShantyPosition; 					/// <summary>Spawn Position for Shanty at Stage 2.</summary>
	[SerializeField] private Vector3 _stage3ShantyPosition; 					/// <summary>Spawn Position for Shanty at Stage 3.</summary>
	[Space(5f)]
	[Header("Stage 2's Attributes:")]
	[SerializeField] private Line _mainDeckPath; 								/// <summary>Main Deck's Path.</summary>
	[SerializeField] private Line _leftStairPath; 								/// <summary>Path for the Left Staircase.</summary>
	[SerializeField] private Line _rightStairPath; 								/// <summary>Path for the Right Staircase.</summary>
	[SerializeField] private Vector3Pair _helmWaypointsPair; 					/// <summary>Helm's Waypoints' Pair.</summary>
	[SerializeField] private Vector3Pair _deckWaypointsPair; 					/// <summary>Deck's Waypoints' Pair.</summary>
	[SerializeField] private Vector3Pair _leftStairWaypointsPair; 				/// <summary>Left Stair's Waypoints' Pair.</summary>
	[SerializeField] private Vector3Pair _rightStairWaypointsPair; 				/// <summary>Right Stair's Waypoints' Pair.</summary>
	[SerializeField] private Vector3Pair _walkRange; 							/// <summary>Shanty's Walk Range on Stage 2.</summary> 
	[SerializeField] private Vector3 _shipScale; 								/// <summary>Ship's Scale on Stage 2.</summary>
	[SerializeField] private float _waitBeforeFade; 							/// <summary>Wait's duration before screen fade.</summary>
	[SerializeField] private float _waitAfterFade; 								/// <summary>Wait's duration after screen fade.</summary>
	[SerializeField] private float _fadeInDuration; 							/// <summary>Fade-In's Duration.</summary>
	[SerializeField] private float _fadeOutDuration; 							/// <summary>Fade-Out's Duration.</summary>
	[SerializeField] private Vector3Pair[] _whackAMoleWaypointsPairs; 			/// <summary>Waypoint pairs for the Whack-a-Mole phase.</summary>
	[Space(5f)]
	[Header("Particle Effects:")]
	[SerializeField] private Vector3[] _smokeSpawnPositions; 					/// <summary>Spawn Positions for the Some's ParticleEffect.</summary>
	[SerializeField] private VAssetReference _smokeEffectReference; 			/// <summary>Smoke ParticleEffect's Reference.</summary>
	[Space(5f)]
	[SerializeField] private TransformData _stage1ShipTransformData; 			/// <summary>Stage 1's Ship Transform Data.</summary>
	[SerializeField] private TransformData _stage2ShipTransformData; 			/// <summary>Stage 2's Ship Transform Data.</summary>
	[SerializeField] private TransformData _stage3ShipTransformData; 			/// <summary>Stage 3's Ship Transform Data.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor1; 								/// <summary>Gizmos' Color #1.</summary>
	[SerializeField] private Color gizmosColor2; 								/// <summary>Gizmos' Color #2.</summary>
	[SerializeField] private Color gizmosColor3; 								/// <summary>Gizmos' Color #3.</summary>
#endif
	private TransformData _initialShipTransformData; 							/// <summary>Initial Ship's TransformData.</summary>

#region Getters/Setters:
	/// <summary>Gets shanty property.</summary>
	public ShantyBoss shanty { get { return _shanty; } }

	/// <summary>Gets shantyController property.</summary>
	public ShantyBossAIController shantyController { get { return _shantyController; } }

	/// <summary>Gets shipScale property.</summary>
	public Vector3 shipScale { get { return _shipScale; } }

	/// <summary>Gets smokeSpawnPositions property.</summary>
	public Vector3[] smokeSpawnPositions { get { return _smokeSpawnPositions; } }

	/// <summary>Gets waitBeforeFade property.</summary>
	public float waitBeforeFade { get { return _waitBeforeFade; } }

	/// <summary>Gets waitAfterFade property.</summary>
	public float waitAfterFade { get { return _waitAfterFade; } }

	/// <summary>Gets fadeInDuration property.</summary>
	public float fadeInDuration { get { return _fadeInDuration; } }

	/// <summary>Gets fadeOutDuration property.</summary>
	public float fadeOutDuration { get { return _fadeOutDuration; } }

	/// <summary>Gets mainDeckPath property.</summary>
	public Line mainDeckPath { get { return _mainDeckPath; } }

	/// <summary>Gets leftStairPath property.</summary>
	public Line leftStairPath { get { return _leftStairPath; } }

	/// <summary>Gets rightStairPath property.</summary>
	public Line rightStairPath { get { return _rightStairPath; } }

	/// <summary>Gets whackAMoleWaypointsPairs property.</summary>
	public Vector3Pair[] whackAMoleWaypointsPairs { get { return _whackAMoleWaypointsPairs; } }

	/// <summary>Gets shantyShip property.</summary>
	public ShantyShip shantyShip { get { return _shantyShip; } }

	/// <summary>Gets loopReference property.</summary>
	public VAssetReference loopReference { get { return _loopReference; } }

	/// <summary>Gets smokeEffectReference property.</summary>
	public VAssetReference smokeEffectReference { get { return _smokeEffectReference; } }

	/// <summary>Gets stage1MateoPosition property.</summary>
	public Vector3 stage1MateoPosition { get { return _stage1MateoPosition; } }

	/// <summary>Gets stage2MateoPosition property.</summary>
	public Vector3 stage2MateoPosition { get { return shantyShip != null ?  shantyShip.transform.TransformPoint(_stage2MateoPosition) : _stage2MateoPosition; } }

	/// <summary>Gets stage3MateoPosition property.</summary>
	public Vector3 stage3MateoPosition { get { return _stage3MateoPosition; } }

	/// <summary>Gets stage1ShantyPosition property.</summary>
	public Vector3 stage1ShantyPosition { get { return _stage1ShantyPosition; } }

	/// <summary>Gets stage2ShantyPosition property.</summary>
	public Vector3 stage2ShantyPosition { get { return shantyShip != null ? shantyShip.transform.TransformPoint(_stage2ShantyPosition) : _stage2ShantyPosition; } }

	/// <summary>Gets stage3ShantyPosition property.</summary>
	public Vector3 stage3ShantyPosition { get { return _stage3ShantyPosition; } }

	/// <summary>Gets helmWaypointsPair property.</summary>
	public Vector3Pair helmWaypointsPair { get { return _helmWaypointsPair; } }

	/// <summary>Gets deckWaypointsPair property.</summary>
	public Vector3Pair deckWaypointsPair { get { return _deckWaypointsPair; } }

	/// <summary>Gets leftStairWaypointsPair property.</summary>
	public Vector3Pair leftStairWaypointsPair { get { return _leftStairWaypointsPair; } }

	/// <summary>Gets rightStairWaypointsPair property.</summary>
	public Vector3Pair rightStairWaypointsPair { get { return _rightStairWaypointsPair; } }

	/// <summary>Gets walkRange property.</summary>
	public Vector3Pair walkRange { get { return _walkRange; } }

	/// <summary>Gets dockBoundaries property.</summary>
	public ScenarioBoundariesContainer dockBoundaries { get { return _dockBoundaries; } }

	/// <summary>Gets shipBoundaries property.</summary>
	public ScenarioBoundariesContainer shipBoundaries { get { return _shipBoundaries; } }

	/// <summary>Gets stage1ShipTransformData property.</summary>
	public TransformData stage1ShipTransformData { get { return _stage1ShipTransformData; } }

	/// <summary>Gets stage2ShipTransformData property.</summary>
	public TransformData stage2ShipTransformData { get { return _stage2ShipTransformData; } }

	/// <summary>Gets stage3ShipTransformData property.</summary>
	public TransformData stage3ShipTransformData { get { return _stage3ShipTransformData; } }

	/// <summary>Gets initialShipTransformData property.</summary>
	public TransformData initialShipTransformData { get { return _initialShipTransformData; } }

	/// <summary>Gets stage1CameraSettings property.</summary>
	public Camera2DBoundariesModifier stage1CameraSettings { get { return _stage1CameraSettings; } }

	/// <summary>Gets stage2CameraSettings property.</summary>
	public Camera2DBoundariesModifier stage2CameraSettings { get { return _stage2CameraSettings; } }

	/// <summary>Gets stage3CameraSettings property.</summary>
	public Camera2DBoundariesModifier stage3CameraSettings { get { return _stage3CameraSettings; } }
#endregion
	
#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor1;

		if(smokeSpawnPositions != null) foreach(Vector3 position in smokeSpawnPositions)
		{
			Gizmos.DrawWireSphere(position, 0.5f);
		}

		if(whackAMoleWaypointsPairs != null && shantyShip != null) foreach(Vector3Pair pair in whackAMoleWaypointsPairs)
		{
			Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(pair.a), 0.5f);
			Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(pair.b), 0.5f);
		}

		Gizmos.color = gizmosColor2;

		Gizmos.DrawWireSphere(stage1MateoPosition, 0.5f);
		Gizmos.DrawWireSphere(stage2MateoPosition, 0.5f);
		Gizmos.DrawWireSphere(stage3MateoPosition, 0.5f);
		Gizmos.DrawWireSphere(stage1ShantyPosition, 0.5f);
		Gizmos.DrawWireSphere(stage2ShantyPosition, 0.5f);
		Gizmos.DrawWireSphere(stage3ShantyPosition, 0.5f);

		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(walkRange.a), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(walkRange.b), 0.5f);

		Gizmos.color = gizmosColor3;

		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(helmWaypointsPair.a), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(helmWaypointsPair.b), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(deckWaypointsPair.a), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(deckWaypointsPair.b), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(leftStairWaypointsPair.a), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(leftStairWaypointsPair.b), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(rightStairWaypointsPair.a), 0.5f);
		Gizmos.DrawWireSphere(shantyShip.transform.TransformPoint(rightStairWaypointsPair.b), 0.5f);

		VGizmos.DrawTransformData(stage1ShipTransformData);
		VGizmos.DrawTransformData(stage2ShipTransformData);
		VGizmos.DrawTransformData(stage3ShipTransformData);
	}
#endif

	/*IEnumerator TEST()
	{
		float duration = 1.0f / 3.2f;
		float t = 0.0f;

		while(t < 1.0f)
		{
			shanty.transform.position = _rightStairPath.Lerp(t);
			t += (Time.deltaTime * duration);

			yield return null;
		} 
	}*/

	/// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
   	protected override void OnAwake()
	{
		ResourcesManager.onResourcesLoaded += OnResourcesLoaded;
		AudioController.onAudioMappingLoaded += OnAudioMappingLoaded;
		
		if(shanty != null)
		{
			shanty.eventsHandler.onIDEvent += OnShantyIDEvent;
			if(shantyShip != null) shanty.ship = shantyShip;
		}
	}

	/// <summary>ShantySceneController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		//Introduction();
		//StartCoroutine(TEST());
		Game.EnablePlayerControl(false);
	}

	/// <summary>Callback invoked when ShantySceneController's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		ResourcesManager.onResourcesLoaded -= OnResourcesLoaded;
		AudioController.onAudioMappingLoaded -= OnAudioMappingLoaded;
	}

	/// <summary>Ties Shanty into rope and docks ship.</summary>
	private void Introduction()
	{
		if(shanty == null
		|| shantyController == null
		//|| shanty.animator == null
		|| shantyShip == null) return;

		shantyShip.ropeHitBox.onTriggerEvent2D -= OnRopeHit; 			/// Just in case...
		shantyShip.ropeHitBox.onTriggerEvent2D += OnRopeHit;

		Debug.Log("[ShantySceneController] Tying Argel...");

		shantyController.OnTie(shantyShip.transform, stage1ShantyPosition);
		shantyShip.GoToState(ShantyShip.ID_STATE_DOCKED);
	}

#region Callbacks:
	/// <summary>Callback invoked when all resources are loaded.</summary>
	private void OnResourcesLoaded()
	{
		Game.EnablePlayerControl(true);
	}

	/// <summary>Callback invoked when the Audio's Mapping has been loaded.</summary>
	private void OnAudioMappingLoaded()
	{
		AudioController.Play(SourceType.Loop, 0, loopReference);
	}

	/// <summary>Callback invoked when Shanty invokes an ID's Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnShantyIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_STAGECHANGED:
			int stageID = shanty.currentStage;

			/* Things modified per-stage:
				- Spawn Positions:
					- Mateo
					- Shanty
				- Camera Settings
				- Ship's Transform's Data
				- Floors enabled/disabled
				- Enable/Disable Shanty's Physics
			*/

			switch(stageID)
			{
				case Boss.STAGE_1:
					/*
						- Tie Shanty
					*/
					shantyShip.ActivateCannons(true);
					Game.AddTargetToCamera(shanty.cameraTarget);
					OnStageChanged(stageID);
					Introduction();
				break;

				case Boss.STAGE_2:
					ParticleEffect effect = null;
					shanty.ChangeState(IDs.STATE_ALIVE | IDs.STATE_IDLE);
					Game.EnablePlayerControl(false);
					Game.state = GameState.Transitioning;
					Game.RemoveTargetToCamera(shanty.cameraTarget);
					shantyController.StopAttackRoutine();
					shanty.GoToCryAnimation();

					foreach(Vector3 position in smokeSpawnPositions)
					{
						//effect = PoolManager.RequestParticleEffect(smokeEffectReference, position, Quaternion.identity);
					}

					this.StartCoroutine(this.WaitSeconds(waitBeforeFade,
					()=>
					{
						Game.gameplayGUIController.screenFaderGUI.FadeIn(Color.white, fadeInDuration,
						()=>
						{ /// Once the scenario is covered by the faded screen, do this:

							shantyShip.ActivateCannons(false);
							OnStageChanged(stageID);

							this.StartCoroutine(this.WaitSeconds(waitAfterFade,
							()=>
							{
								Game.gameplayGUIController.screenFaderGUI.FadeOut(Color.white, fadeOutDuration,
								()=>
								{
									Game.state = GameState.Playing;
									Game.EnablePlayerControl(true);
									shantyController.BeginAttackRoutine();
								});
							}));
						});
					}));
				break;

				case Boss.STAGE_3:
					Game.state = GameState.Transitioning;
					Game.AddTargetToCamera(shanty.cameraTarget);
					shanty.GoToCryAnimation();
					this.StartCoroutine(this.WaitSeconds(waitBeforeFade, 
					()=>
					{
						Game.gameplayGUIController.screenFaderGUI.FadeIn(Color.white, fadeInDuration,
						()=>
						{
							shantyShip.ActivateCannons(true);
							OnStageChanged(stageID);

							this.StartCoroutine(this.WaitSeconds(waitAfterFade,
							()=>
							{
								Game.gameplayGUIController.screenFaderGUI.FadeOut(Color.white, fadeOutDuration,
								()=>
								{
									Game.state = GameState.Playing;
									Game.EnablePlayerControl(true);
									shantyController.BeginAttackRoutine();
								});
							}));
						});
					}));
				break;
			}
			break;

			case IDs.EVENT_DEATHROUTINE_BEGINS:
				shanty.GoToCryAnimation();
				Game.EnablePlayerControl(false);
				Game.mateo.animatorController.CrossFadeAndWait(Game.mateo.bowCredential, Game.mateo.clipFadeDuration, Game.mateo.mainAnimationLayer, 0.0f, 0.0f,
				()=>
				{
					Game.LoadScene(Game.data.overworldSceneName);
				});
			break;

			case IDs.EVENT_DEATHROUTINE_ENDS:
			break;
		}

		//Debug.Log("[ShantySceneController] On Shanty Event with ID #" + _ID);
	}

	/// <summary>Event invoked when this Hit Collider2D intersects with another GameObject.</summary>
	/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_hitColliderID">Optional ID of the HitCollider2D.</param>
	private void OnRopeHit(Collider2D _collider, HitColliderEventTypes _eventType, int _ID = 0)
	{
		GameObject obj = _collider.gameObject;

		if(obj.CompareTag(Game.data.playerWeaponTag) || obj.CompareTag(Game.data.playerProjectileTag))
		{
			/// \TODO Cool disolving effect for the rope?
			shantyShip.ropeRenderer.gameObject.SetActive(false);
			shantyShip.ropeHitBox.onTriggerEvent2D -= OnRopeHit; 		/// Just in case...
			shantyShip.ropeHitBox.gameObject.SetActive(false);

			shantyController.OnUntie();
		}
	}

	/// <summary>Changes settings depending on the stage.</summary>
	/// <param name="_stageID">Stage's ID.</param>
	public void OnStageChanged(int _stageID)
	{
		shanty.transform.parent = null;

		switch(_stageID)
		{
			case Boss.STAGE_1:
			shantyShip.transform.Set(stage1ShipTransformData);
			dockBoundaries.Enable(true);
			shipBoundaries.Enable(false);
			Game.mateo.transform.position = stage1MateoPosition;
			shanty.transform.position = stage1ShantyPosition;
			Game.SetCameraBoundaries2DSettings(stage1CameraSettings);
			shanty.EnablePhysics(false);
			break;

			case Boss.STAGE_2:
			shantyShip.animation.Stop();
			shantyShip.animator.enabled = false;
			shantyShip.transform.Set(stage2ShipTransformData);
			dockBoundaries.Enable(false);
			shipBoundaries.Enable(true);
			Game.mateo.transform.position = stage2MateoPosition;
			shanty.transform.position = stage2ShantyPosition;
			Game.SetCameraBoundaries2DSettings(stage2CameraSettings);
			shanty.EnablePhysics(false);
			break;

			case Boss.STAGE_3:
			shantyShip.animation.Stop();
			shantyShip.animator.enabled = false;
			shantyShip.transform.Set(stage3ShipTransformData);
			dockBoundaries.Enable(true);
			shipBoundaries.Enable(false);
			Game.mateo.transform.position = stage3MateoPosition;
			shanty.transform.position = stage3ShantyPosition;
			Game.SetCameraBoundaries2DSettings(stage3CameraSettings);
			shanty.EnablePhysics(true);
			break;
		}
	}
#endregion

}
}