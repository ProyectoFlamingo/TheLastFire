using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

using Random = UnityEngine.Random;

namespace Flamingo
{
[Flags]
public enum DisplacementType
{
	Horizontal = 1,
	Vertical = 2,
	Diagonal = Horizontal | Vertical
}

public class JudgementBehavior : DestinoScriptableCoroutine
{
	[Header("Fire Show's Attributes:")]
	[TabGroup("Show Group", "Fire Show")][SerializeField][Range(0.0f, 1.0f)] private float _fireShowPieceDurationPercentage; 	/// <summary>Duration's Percentage of the Fire's Show [determines how much the Show will last].</summary>
	[TabGroup("Show Group", "Fire Show")][SerializeField][Range(0.0f, 1.0f)] private float _fireShowSuccessPercentage; 			/// <summary>Success' Percentage for the Fire Show.</summary>
	[Space(5f)]
	[Header("Break the Targets' Mini-Games:")]
	[TabGroup("Show Group", "Fire Show")][SerializeField] private BreakTheTargetsMiniGame[] _stage1BreakTheTargetsMiniGames; 	/// <summary>Available Break-the-Targets' Mini-Games for Stage 1.</summary>
	[TabGroup("Show Group", "Fire Show")][SerializeField] private BreakTheTargetsMiniGame[] _stage2BreakTheTargetsMiniGames; 	/// <summary>Available Break-the-Targets' Mini-Games for Stage 2.</summary>
	[TabGroup("Show Group", "Fire Show")][SerializeField] private BreakTheTargetsMiniGame[] _stage3BreakTheTargetsMiniGames; 	/// <summary>Available Break-the-Targets' Mini-Games for Stage 3.</summary>
	[Space(5f)]
	[Header("Sword Show's Attributes:")]
	[TabGroup("Show Group", "Sword Show")][SerializeField][Range(0.0f, 1.0f)] private float _swordShowPieceDurationPercentage; 	/// <summary>Duration's Percentage of the Sword's Show [determines how much the Show will last].</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField][Range(0.0f, 1.0f)] private float _swordShowSuccessPercentage; 		/// <summary>Success' Percentage for the Sword Show.</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField] private VAssetReference[] _swordTargetReferences; 					/// <summary>Sword Target's References on the Game's Data.</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField] private Vector3Pair[] _swordTargetsWaypoints; 						/// <summary>Sword Targets' Waypoints.</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField] private FloatRange _waitBetweenSwordTarget; 							/// <summary>Wait between each Sword Target.</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField] private float _swordTargetShakeDuration; 							/// <summary>Sword Target's Shake Duration.</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField] private float _swordTargetShakeSpeed; 								/// <summary>Sword Target's Shake Speed.</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField] private float _swordTargetShakeMagnitude; 							/// <summary>Sword Target's Shake Magnitude.</summary>
	[TabGroup("Show Group", "Sword Show")][SerializeField] private float _swordTargetInterpolationDuration; 					/// <summary>Interpolation duration it takes the target to reach the spawn's destiny.</summary>
	[Space(5f)]
	[Header("Dance Show's Attributes:")]
	[TabGroup("Show Group", "Dance Show")][SerializeField][Range(0.0f, 1.0f)] private float _danceShowPieceDurationPercentage; 	/// <summary>Duration's Percentage of the Dance's Show [determines how much the Show will last].</summary>
	[TabGroup("Show Group", "Dance Show")][SerializeField][Range(0.0f, 1.0f)] private float _danceShowSuccessPercentage; 		/// <summary>Success' Percentage for the Dance Show.</summary>
	[Space(5f)]
	[Header("Ring-Madness Mini-Games:")]
	[TabGroup("Show Group", "Dance Show")][SerializeField] private RingMadnessMiniGame[] _stage1RingMadnessMiniGames; 			/// <summary>Available Ring-Madness' Mini-Games for Stage 1.</summary>
	[TabGroup("Show Group", "Dance Show")][SerializeField] private RingMadnessMiniGame[] _stage2RingMadnessMiniGames; 			/// <summary>Available Ring-Madness' Mini-Games for Stage 2.</summary>
	[TabGroup("Show Group", "Dance Show")][SerializeField] private RingMadnessMiniGame[] _stage3RingMadnessMiniGames; 			/// <summary>Available Ring-Madness' Mini-Games for Stage 3.</summary>
	[Space(5f)]
	[Header("Loops:")]
	[TabGroup("Audio")][SerializeField] private SoundEffectEmissionData _fireShowPieceSoundEffect; 								/// <summary>Fire Show's Piece's Soun-Effect's Data.</summary>
	[TabGroup("Audio")][SerializeField] private SoundEffectEmissionData _swordShowPieceSoundEffect; 							/// <summary>Sword Show's Piece's Soun-Effect's Data.</summary>
	[TabGroup("Audio")][SerializeField] private SoundEffectEmissionData _danceShowPieceSoundEffect; 							/// <summary>Dance Show's Piece's Soun-Effect's Data.</summary>
	[Space(5f)]
	[Header("Sound Effects:")]
	[TabGroup("Audio")][SerializeField] private SoundEffectEmissionData _applauseSoundEffect; 									/// <summary>Applause's Sound-Effect's Data.</summary>
	[TabGroup("Audio")][SerializeField] private SoundEffectEmissionData _booingSoundEffect; 									/// <summary>Booing's Sound-Effect's Data.</summary>
	[Space(5f)]
	[Header("Signs' Attributes:")]
	[TabGroup("Signs")][SerializeField] private Vector3 _showSignSpawnPoint; 													/// <summary>Show Sign's Spawn Point.</summary>
	[TabGroup("Signs")][SerializeField] private Vector3 _showSignPresentationPoint; 											/// <summary>Show Sign's Presentation Point.</summary>
	[TabGroup("Signs")][SerializeField] private Vector3 _showSignDestinyPoint; 													/// <summary>Show Sign's Destiny Point.</summary>
	[TabGroup("Signs")][SerializeField] private float _signEntranceDuration; 													/// <summary>Sign Entrance's Duration.</summary>
	[TabGroup("Signs")][SerializeField] private float _signExitDuration; 														/// <summary>Sign Exit's Duration.</summary>
	[TabGroup("Signs")][SerializeField] private float _signIdleDuration; 														/// <summary>Sign Idle's Duration.</summary>
	[Space(5f)]
	[Header("Crowd's Attributes:")]
	[TabGroup("Crowd")][SerializeField] private VAssetReference[] _trashProjectilesReferences; 									/// <summary>References of all the trash (Parabola) Projectiles.</summary>
	[TabGroup("Crowd")][SerializeField] private VAssetReference[] _applauseObjectsReferences; 									/// <summary>References of objects thrown at an applause.</summary>
	[TabGroup("Crowd")][SerializeField] private Vector3[] _trashProjectilesWaypoints; 											/// <summary>Trash Projectiles' Waypoints.</summary>
	[TabGroup("Crowd")][SerializeField] private IntRange _trashProjectilesPerRound; 											/// <summary>Range of Trash projectiles per round.</summary>
	[TabGroup("Crowd")][SerializeField] private FloatRange _trashProjectileCooldown; 											/// <summary>Cooldown duration's range per trash Projectile.</summary>
	[TabGroup("Crowd")][SerializeField] private FloatRange _mateoPositionProjection; 											/// <summary>Range of Mateo's Time Projection.</summary>
	[TabGroup("Crowd")][SerializeField] private float _trashProjectileTime; 													/// <summary>Parabola's (Trash) time it takes to potentially reach mateo.</summary>

#region Getters/Setters:
	/// <summary>Gets stage1BreakTheTargetsMiniGames property.</summary>
	public BreakTheTargetsMiniGame[] stage1BreakTheTargetsMiniGames { get { return _stage1BreakTheTargetsMiniGames; } }

	/// <summary>Gets stage2BreakTheTargetsMiniGames property.</summary>
	public BreakTheTargetsMiniGame[] stage2BreakTheTargetsMiniGames { get { return _stage2BreakTheTargetsMiniGames; } }

	/// <summary>Gets stage3BreakTheTargetsMiniGames property.</summary>
	public BreakTheTargetsMiniGame[] stage3BreakTheTargetsMiniGames { get { return _stage3BreakTheTargetsMiniGames; } }

	/// <summary>Gets stage1RingMadnessMiniGames property.</summary>
	public RingMadnessMiniGame[] stage1RingMadnessMiniGames { get { return _stage1RingMadnessMiniGames; } }

	/// <summary>Gets stage2RingMadnessMiniGames property.</summary>
	public RingMadnessMiniGame[] stage2RingMadnessMiniGames { get { return _stage2RingMadnessMiniGames; } }

	/// <summary>Gets stage3RingMadnessMiniGames property.</summary>
	public RingMadnessMiniGame[] stage3RingMadnessMiniGames { get { return _stage3RingMadnessMiniGames; } }

	/// <summary>Gets fireShowPieceSoundEffect property.</summary>
	public SoundEffectEmissionData fireShowPieceSoundEffect { get { return _fireShowPieceSoundEffect; } }

	/// <summary>Gets swordShowPieceSoundEffect property.</summary>
	public SoundEffectEmissionData swordShowPieceSoundEffect { get { return _swordShowPieceSoundEffect; } }

	/// <summary>Gets danceShowPieceSoundEffect property.</summary>
	public SoundEffectEmissionData danceShowPieceSoundEffect { get { return _danceShowPieceSoundEffect; } }

	/// <summary>Gets applauseSoundEffect property.</summary>
	public SoundEffectEmissionData applauseSoundEffect { get { return _applauseSoundEffect; } }

	/// <summary>Gets booingSoundEffect property.</summary>
	public SoundEffectEmissionData booingSoundEffect { get { return _booingSoundEffect; } }

	/// <summary>Gets swordTargetReferences property.</summary>
	public VAssetReference[] swordTargetReferences { get { return _swordTargetReferences; } }

	/// <summary>Gets trashProjectilesReferences property.</summary>
	public VAssetReference[] trashProjectilesReferences { get { return _trashProjectilesReferences; } }

	/// <summary>Gets applauseObjectsReferences property.</summary>
	public VAssetReference[] applauseObjectsReferences { get { return _applauseObjectsReferences; } }

	/// <summary>Gets trashProjectilesWaypoints property.</summary>
	public Vector3[] trashProjectilesWaypoints { get { return _trashProjectilesWaypoints; } }

	/// <summary>Gets showSignSpawnPoint property.</summary>
	public Vector3 showSignSpawnPoint { get { return _showSignSpawnPoint; } }

	/// <summary>Gets showSignPresentationPoint property.</summary>
	public Vector3 showSignPresentationPoint { get { return _showSignPresentationPoint; } }

	/// <summary>Gets showSignDestinyPoint property.</summary>
	public Vector3 showSignDestinyPoint { get { return _showSignDestinyPoint; } }

	/// <summary>Gets waitBetweenSwordTarget property.</summary>
	public FloatRange waitBetweenSwordTarget { get { return _waitBetweenSwordTarget; } }

	/// <summary>Gets fireShowPieceDurationPercentage property.</summary>
	public float fireShowPieceDurationPercentage { get { return _fireShowPieceDurationPercentage; } }

	/// <summary>Gets swordShowPieceDurationPercentage property.</summary>
	public float swordShowPieceDurationPercentage { get { return _swordShowPieceDurationPercentage; } }

	/// <summary>Gets danceShowPieceDurationPercentage property.</summary>
	public float danceShowPieceDurationPercentage { get { return _danceShowPieceDurationPercentage; } }

	/// <summary>Gets signEntranceDuration property.</summary>
	public float signEntranceDuration { get { return _signEntranceDuration; } }

	/// <summary>Gets signExitDuration property.</summary>
	public float signExitDuration { get { return _signExitDuration; } }

	/// <summary>Gets signIdleDuration property.</summary>
	public float signIdleDuration { get { return _signIdleDuration; } }

	/// <summary>Gets swordTargetShakeDuration property.</summary>
	public float swordTargetShakeDuration { get { return _swordTargetShakeDuration; } }

	/// <summary>Gets swordTargetShakeSpeed property.</summary>
	public float swordTargetShakeSpeed { get { return _swordTargetShakeSpeed; } }

	/// <summary>Gets swordTargetShakeMagnitude property.</summary>
	public float swordTargetShakeMagnitude { get { return _swordTargetShakeMagnitude; } }

	/// <summary>Gets trashProjectileTime property.</summary>
	public float trashProjectileTime { get { return _trashProjectileTime; } }

	/// <summary>Gets fireShowSuccessPercentage property.</summary>
	public float fireShowSuccessPercentage { get { return _fireShowSuccessPercentage; } }

	/// <summary>Gets swordShowSuccessPercentage property.</summary>
	public float swordShowSuccessPercentage { get { return _swordShowSuccessPercentage; } }

	/// <summary>Gets swordTargetInterpolationDuration property.</summary>
	public float swordTargetInterpolationDuration { get { return _swordTargetInterpolationDuration; } }

	/// <summary>Gets danceShowSuccessPercentage property.</summary>
	public float danceShowSuccessPercentage { get { return _danceShowSuccessPercentage; } }

	/// <summary>Gets swordTargetsWaypoints property.</summary>
	public Vector3Pair[] swordTargetsWaypoints { get { return _swordTargetsWaypoints; } }

	/// <summary>Gets trashProjectilesPerRound property.</summary>
	public IntRange trashProjectilesPerRound { get { return _trashProjectilesPerRound; } }

	/// <summary>Gets trashProjectileCooldown property.</summary>
	public FloatRange trashProjectileCooldown { get { return _trashProjectileCooldown; } }

	/// <summary>Gets mateoPositionProjection property.</summary>
	public FloatRange mateoPositionProjection { get { return _mateoPositionProjection; } }
#endregion

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
#if UNITY_EDITOR
		base.DrawGizmos();

		Gizmos.color = gizmosColor;

		Gizmos.DrawWireSphere(showSignSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(showSignPresentationPoint, gizmosRadius);
		Gizmos.DrawWireSphere(showSignDestinyPoint, gizmosRadius);

		if(trashProjectilesWaypoints != null) foreach(Vector3 waypoint in trashProjectilesWaypoints)
		{
			Gizmos.DrawWireSphere(waypoint, gizmosRadius);
		}
		if(swordTargetsWaypoints != null) foreach(Vector3Pair pair in swordTargetsWaypoints)
		{
			Gizmos.DrawWireSphere(pair.a, gizmosRadius);
			Gizmos.DrawWireSphere(pair.b, gizmosRadius);
		}
#endif
	}

#region Callbacks:
	/// <summary>Callback invoked when the Break-The-Targets Mini-Game invokes an Event.</summary>
    /// <param name="_miniGame">Mini-Game's Instance that invoked the Event.</param>
    /// <param name="_ID">Event's ID.</param>
	private void OnBreakTheTargetsMiniGameEvent(MiniGame _miniGame, int _ID)
	{
		switch(_ID)
		{
			case MiniGame.ID_EVENT_MINIGAME_ENDED:
			break;

			case MiniGame.ID_EVENT_MINIGAME_SUCCESS:
			break;

			case MiniGame.ID_EVENT_MINIGAME_FAILURE:
			break;
		}

		Debug.Log("[JudgementBehavior] Invoked Event: " + _ID);
	}

	/// <summary>Callback invoked when the Break-The-Targets Mini-Game invokes an Event.</summary>
    /// <param name="_miniGame">Mini-Game's Instance that invoked the Event.</param>
    /// <param name="_ID">Event's ID.</param>
	private void OnRingMadnessMiniGameEvent(MiniGame _miniGame, int _ID)
	{
		switch(_ID)
		{
			case MiniGame.ID_EVENT_MINIGAME_ENDED:
			break;

			case MiniGame.ID_EVENT_MINIGAME_SUCCESS:
			break;

			case MiniGame.ID_EVENT_MINIGAME_FAILURE:
			break;
		}
		
		Debug.Log("[JudgementBehavior] Invoked Event: " + _ID);
	}
#endregion

#region Coroutines:
	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{
		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);

		IEnumerator[] routines = VArray.RandomSet(DanceShowRoutine(boss), FireShowRoutine(boss), SwordShowRoutine(boss));
		IEnumerator routine = routines.Random();

		/*foreach(IEnumerator routine in routines)
		{
			while(routine.MoveNext()) yield return null;
		}*/

		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		while(routine.MoveNext()) yield return null;

		/*AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
		AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex, true, boss.Sing);*/

		AudioController.PlayFSMLoops(boss.Sing, DestinoSceneController.Instance.mainLoopLoopData, DestinoSceneController.Instance.mainLoopVoiceLoopData);
		InvokeCoroutineEnd();
	}

	/// <summary>Displays and Hides Sign.</summary>
	/// <param name="_sign">Sign's Transform.</param>
	/// <param name="_spawnPoint">Origin.</param>
	/// <param name="_destinyPoint">Destiny.</param>
	private IEnumerator DisplayShowSign(Transform _sign)
	{
		_sign.gameObject.SetActive(true);
		_sign.position = showSignSpawnPoint;

		SecondsDelayWait wait = new SecondsDelayWait(signIdleDuration);
		IEnumerator displacement = _sign.DisplaceToPosition(showSignDestinyPoint, signEntranceDuration);

		while(displacement.MoveNext()) yield return null;
		while(wait.MoveNext()) yield return null;

		displacement = _sign.DisplaceToPosition(showSignDestinyPoint, signExitDuration);
		while(displacement.MoveNext()) yield return null;
	}

	/// <summary>Hides Show's Sign.</summary>
	/// <param name="_sign">Sign's Transform to Hide.</param>
	private IEnumerator HidehowSign(Transform _sign)
	{
		IEnumerator displacement = _sign.DisplaceToPosition(showSignSpawnPoint, signExitDuration);
		while(displacement.MoveNext()) yield return null;

		_sign.gameObject.SetActive(false);
	}

	/// <summary>Fire Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator FireShowRoutine(DestinoBoss boss)
	{
		/*
			- Show the Sign, wait till the exposure is over
			- Play the Fire Show's Loop
			- Play the Minigame
			- Make a judgement after the game has finished
		*/

		Transform fireShowSign = DestinoSceneController.Instance.fireShowSign;
		AudioClip clip = null;
		IEnumerator signDisplacement = DisplayShowSign(fireShowSign);
		BreakTheTargetsMiniGame miniGame = null;
		IEnumerator showJudgement = null;

		while(signDisplacement.MoveNext()) yield return null;

		switch(boss.currentStage)
		{
			case Boss.STAGE_1:
			miniGame = stage1BreakTheTargetsMiniGames.Random();
			break;

			case Boss.STAGE_2:
			miniGame = stage2BreakTheTargetsMiniGames.Random();
			break;

			case Boss.STAGE_3:
			miniGame = stage3BreakTheTargetsMiniGames.Random();
			break;
		}

		clip = fireShowPieceSoundEffect.Play();
		miniGame.timeLimit = clip.length * fireShowPieceDurationPercentage;
		miniGame.Initialize(this, OnBreakTheTargetsMiniGameEvent);

		while(miniGame.state == MiniGameState.Running) yield return null;

		showJudgement = EvaluateShow(miniGame.scorePercentage, fireShowSuccessPercentage);
		while(showJudgement.MoveNext()) yield return null;

		signDisplacement = HidehowSign(fireShowSign);
		while(signDisplacement.MoveNext()) yield return null;

		miniGame.Terminate();

		DestinoSceneController.Instance.fireShowSign.SetActive(false);
	}

	/// <summary>Sword Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator SwordShowRoutine(DestinoBoss boss)
	{
		/*
			- Show the Sign, wait till the exposure is over
			- Play the Sword Show's Loop
			- Play the Minigame
			- Make a judgement after the game has finished
		*/
		

		List<Fragmentable> targets = new List<Fragmentable>();
		Transform swordShowSign = DestinoSceneController.Instance.swordShowSign;
		AudioClip clip = null;
		IEnumerator signDisplacement = DisplayShowSign(DestinoSceneController.Instance.swordShowSign);
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		float totalTargets = 0.0f;
		float targetsDestroyed = 0.0f;
		OnPoolGameObjectDeactivated onTargetDeactivation = (poolObject, cause, info)=>
		{
			Fragmentable fragmentable = poolObject as Fragmentable;

			if(fragmentable == null) return;

			switch(cause)
			{
				case DeactivationCause.LeftBoundaries:
				case DeactivationCause.LifespanOver:

				break;

				default:
				targetsDestroyed++;
				break;
			}

			targets.Remove(fragmentable);
			Game.RemoveTargetToCamera(fragmentable.cameraTarget);
		};

		while(signDisplacement.MoveNext()) yield return null;

		clip = swordShowPieceSoundEffect.Play();
		wait.ChangeDurationAndReset(clip.length * swordShowPieceDurationPercentage);

		/// While the Sword-Show's Piece Keeps Playing Keep Throwing Targets...
		while((wait.MoveNext()) && (wait.remainingTime > swordTargetInterpolationDuration))
		{
			//Debug.Log("[JudgementBehavior] Wait Remaining Time: " + wait.remainingTime);

			Vector3Pair pair = swordTargetsWaypoints.Random();
			Vector3 origin = pair.a;
			Vector3 destiny = pair.b;
			Ray ray = new Ray(origin, destiny - origin);
			Fragmentable fragmentableTarget = PoolManager.RequestPoolGameObject(swordTargetReferences.Random(), origin, Quaternion.identity) as Fragmentable;
			SecondsDelayWait waitBetweenTarget = new SecondsDelayWait(waitBetweenSwordTarget.Random());

			if(fragmentableTarget == null) yield break;

			totalTargets++;
			targets.Add(fragmentableTarget);
			fragmentableTarget.rigidbody.gravityScale = 0.0f;
			fragmentableTarget.eventsHandler.onPoolGameObjectDeactivated -= onTargetDeactivation;
			fragmentableTarget.eventsHandler.onPoolGameObjectDeactivated += onTargetDeactivation;
			Game.AddTargetToCamera(fragmentableTarget.cameraTarget);

			IEnumerator lerp = fragmentableTarget.transform.DisplaceToPosition(destiny, swordTargetInterpolationDuration);

			while(lerp.MoveNext())
			{
				wait.MoveNext();
				yield return null;
			}

			IEnumerator targetShaking = fragmentableTarget.transform.ShakePosition(swordTargetShakeDuration, swordTargetShakeSpeed, swordTargetShakeMagnitude);

			while(targetShaking.MoveNext())
			{
				wait.MoveNext();
				yield return null;
			}

			fragmentableTarget.rigidbody.gravityScale = 1.0f;

			while(waitBetweenTarget.MoveNext())
			{
				wait.MoveNext();
				yield return null;
			}
		}

		foreach(Fragmentable target in targets)
		{
			target.eventsHandler.onPoolGameObjectDeactivated -= onTargetDeactivation;
			target.OnObjectDeactivation();
			Game.RemoveTargetToCamera(target.cameraTarget);
		}

		showJudgement = EvaluateShow(targetsDestroyed / totalTargets, swordShowSuccessPercentage);

		while(showJudgement.MoveNext()) yield return null;

		DestinoSceneController.Instance.swordShowSign.SetActive(false);
	}

	/// <summary>Dance Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator DanceShowRoutine(DestinoBoss boss)
	{
		/*
			- Show the Sign, wait till the exposure is over
			- Play the Dance Show's Loop
			- Play the Ring-Madness' Minigame
			- Make a judgement after the game has finished
		*/

		Transform danceShowSign = DestinoSceneController.Instance.danceShowSign;
		AudioClip clip = null;
		RingMadnessMiniGame miniGame = null;
		IEnumerator signDisplacement = DisplayShowSign(DestinoSceneController.Instance.danceShowSign);
		IEnumerator showJudgement = null;

		while(signDisplacement.MoveNext()) yield return null;

		switch(boss.currentStage)
		{
			case Boss.STAGE_1:
			miniGame = stage1RingMadnessMiniGames.Random();
			break;

			case Boss.STAGE_2:
			miniGame = stage2RingMadnessMiniGames.Random();
			break;

			case Boss.STAGE_3:
			miniGame = stage3RingMadnessMiniGames.Random();
			break;
		}

		clip = danceShowPieceSoundEffect.Play();
		miniGame.timeLimit = clip.length * danceShowPieceDurationPercentage;
		miniGame.Initialize(this, OnRingMadnessMiniGameEvent);

		while(miniGame.state == MiniGameState.Running) yield return null;

		showJudgement = EvaluateShow(miniGame.scorePercentage, danceShowSuccessPercentage);
		while(showJudgement.MoveNext()) yield return null;

		signDisplacement = HidehowSign(danceShowSign);
		while(signDisplacement.MoveNext()) yield return null;

		miniGame.Terminate();

		DestinoSceneController.Instance.danceShowSign.SetActive(false);
	}

	/// <summary>Evaluates show.</summary>
	/// <param name="_ratio">Mini-Game's Score Percentage.</param>
	/// <param name="_achievePercentageForSuccess">Required percentage for the show to be susccessful.</param>
	private IEnumerator EvaluateShow(float _ratio, float _achievePercentageForSuccess)
	{
		int rounds = 0;
		Faction faction = Faction.None;
		int[] projectilesIndices = null;
		VAssetReference[] projectilesReferences = null;
		SoundEffectEmissionData soundEffect = default(SoundEffectEmissionData);

		switch(_ratio >= _achievePercentageForSuccess)
		{
			case true:
			soundEffect = applauseSoundEffect;
			rounds = trashProjectilesPerRound.Random();
			faction = Faction.Ally;
			projectilesReferences = applauseObjectsReferences;
			break;

			case false:
			soundEffect = booingSoundEffect;
			rounds = trashProjectilesPerRound.Random();
			faction = Faction.Enemy;
			projectilesReferences = trashProjectilesReferences;
			break;
		}

		soundEffect.Play();

		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		//int rounds = trashProjectilesPerRound.Random();

		for(int i = 0; i < rounds; i++)
		{
			wait.ChangeDurationAndReset(trashProjectileCooldown.Random());
			PoolManager.RequestParabolaProjectile(
				faction,
				projectilesReferences.Random(),
				trashProjectilesWaypoints.Random(),
				Game.ProjectMateoPosition(mateoPositionProjection.Random()),
				trashProjectileTime
			);

			while(wait.MoveNext()) yield return null;
		}
	}
#endregion
}
}


/* 	This was a cool algorithm:

	for(int i = 0; i < rounds; i++)
		{
			int ringsPR =  ringsPerRound.Random();
			float currentRingsPR = (float)ringsPR;
			fRingsPerRound += currentRingsPR;
			Ring[] rings = new Ring[ringsPR];
			float[] spaces = new float[rings.Length - 1];
			ringsPassed = 0.0f;
			float width = 0.0f;
			float halfWidth = 0.0f;
			float x = 0.0f;
			float y = 0.0f;
			Ring ring = null;

			/// Create and store some values:
			for(int j = 0; j < ringsPR; j++)
			{
				ring = PoolManager.RequestPoolGameObject(ringsIndices.Random(), Vector3.zero, Random.Range(0, 2) == 0 ? Quaternion.identity : Quaternion.Euler(Vector3.up * 180.0f)).GetComponent<Ring>();
				width += ring.renderer.bounds.size.x;

				if(j < ringsPR - 1)
				{
					spaces[j] = xOffset.Random();
					width += spaces[j];
				}

				rings[j] = ring;
			}

			halfWidth = width * -0.25f;
			x = halfWidth; 	/// Starting Spawn's Position...

			for(int j = 0; j < ringsPR; j++)
			{
				ring = rings[j];

				Vector2 extents = ring.renderer.bounds.extents;
				y = Random.Range((ySpawnLimits.Min() + extents.y),  (maxJumpForce.y - extents.y));
				x += extents.x;
				Vector3 spawnPosition = new Vector3(x, y, 0.0f);

				x += extents.x + xOffset.Random();
				if(j < ringsPR - 1) x += spaces[j];

				ring.transform.position = spawnPosition;
				ring.onRingPassed += onRingPassed;
				
			}

			x = width * 0.5f;

			foreach(Ring currentRing in rings)
			{
				Vector3 ringPosition = currentRing.transform.position;
				ringPosition.x -= x;
				currentRing.transform.position = ringPosition;
			}

			while(wait.MoveNext() && ringsPassed < currentRingsPR) yield return null;
			if(wait.progress == 1.0f) break; /// Direct to the evaluation
			wait.Reset();

			totalRingsPassed += ringsPassed;

			foreach(Ring currentRing in rings)
			{
				currentRing.onRingPassed -= onRingPassed;
				currentRing.OnObjectDeactivation();
			}
		}*/