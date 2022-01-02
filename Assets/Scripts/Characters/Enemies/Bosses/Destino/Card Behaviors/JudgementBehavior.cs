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
	[Space(5f)]
	[Header("Break the Targets' Mini-Games:")]
	[TabGroup("Mini-Game Group", "Break-the-Targets")][SerializeField] private BreakTheTargetsMiniGame[] _stage1BreakTheTargetsMiniGames; 	/// <summary>Available Break-the-Targets' Mini-Games for Stage 1.</summary>
	[TabGroup("Mini-Game Group", "Break-the-Targets")][SerializeField] private BreakTheTargetsMiniGame[] _stage2BreakTheTargetsMiniGames; 	/// <summary>Available Break-the-Targets' Mini-Games for Stage 2.</summary>
	[TabGroup("Mini-Game Group", "Break-the-Targets")][SerializeField] private BreakTheTargetsMiniGame[] _stage3BreakTheTargetsMiniGames; 	/// <summary>Available Break-the-Targets' Mini-Games for Stage 3.</summary>
	[Space(5f)]
	[Header("Ring-Madness Mini-Games:")]
	[TabGroup("Mini-Game Group", "Ring-Madness")][SerializeField] private RingMadnessMiniGame[] _stage1RingMadnessMiniGames; 				/// <summary>Available Ring-Madness' Mini-Games for Stage 1.</summary>
	[TabGroup("Mini-Game Group", "Ring-Madness")][SerializeField] private RingMadnessMiniGame[] _stage2RingMadnessMiniGames; 				/// <summary>Available Ring-Madness' Mini-Games for Stage 2.</summary>
	[TabGroup("Mini-Game Group", "Ring-Madness")][SerializeField] private RingMadnessMiniGame[] _stage3RingMadnessMiniGames; 				/// <summary>Available Ring-Madness' Mini-Games for Stage 3.</summary>
	[Header("Loops:")]
	[TabGroup("Audio")][SerializeField] private int _fireShowPieceIndex; 																	/// <summary>Fire Show's Piece's Index.</summary>
	[TabGroup("Audio")][SerializeField] private int _swordShowPieceIndex; 																	/// <summary>Sword Show's Piece's Index.</summary>
	[TabGroup("Audio")][SerializeField] private int _danceShowPieceIndex; 																	/// <summary>Dance Show's Piece's Index.</summary>
	[Space(5f)]
	[Header("Sound Effects:")]
	[TabGroup("Audio")][SerializeField] private int _applauseSoundIndex; 																	/// <summary>Applause's Sound Index.</summary>
	[TabGroup("Audio")][SerializeField] private int _booingSoundIndex; 																		/// <summary>Booing's Sound Index.</summary>
	[Space(5f)]
	[Header("Signs' Attributes:")]
	[SerializeField] private Vector3 _showSignSpawnPoint; 						/// <summary>Show Sign's Spawn Point.</summary>
	[SerializeField] private Vector3 _showSignPresentationPoint; 				/// <summary>Show Sign's Presentation Point.</summary>
	[SerializeField] private Vector3 _showSignDestinyPoint; 					/// <summary>Show Sign's Destiny Point.</summary>
	[SerializeField] private float _signEntranceDuration; 						/// <summary>Sign Entrance's Duration.</summary>
	[SerializeField] private float _signExitDuration; 							/// <summary>Sign Exit's Duration.</summary>
	[SerializeField] private float _signIdleDuration; 							/// <summary>Sign Idle's Duration.</summary>
	[Space(5f)]
	[Header("Fire Show's Attributes:")]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _fireShowSuccessPercentage; 				/// <summary>Success' Percentage for the Fire Show.</summary>
	[SerializeField] private int[] _fireTargetIndices; 							/// <summary>Fire Target's Indices on the Game's Data.</summary>
	[SerializeField] private IntRange _fireShowRounds; 							/// <summary>Fire Show Rounds' Range.</summary>
	[SerializeField] private IntRange _fireShowTargetsPerRound; 				/// <summary>Fire Show Targets per Round's Range.</summary>
	[SerializeField] private float _fireShowRoundDuration; 						/// <summary>Fire Show's Round Duration.</summary>
	[Space(5f)]
	[Header("Sword Show's Attributes:")]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _swordShowSuccessPercentage; 				/// <summary>Success' Percentage for the Sword Show.</summary>
	[SerializeField] private int[] _swordTargetIndices; 						/// <summary>Sword Target's Indices on the Game's Data.</summary>
	[SerializeField] private Vector3[] _swordTargetsSpawnWaypoints; 			/// <summary>Sword Targets Waypoints [As tuples since they define interpolation points].</summary>
	[SerializeField] private Vector3[] _swordTargetsDestinyWaypoints; 			/// <summary>Sword Targets Waypoints [As tuples since they define interpolation points].</summary>
	[SerializeField] private IntRange _swordShowRounds; 						/// <summary>Sword Show Rounds' Range.</summary>
	[SerializeField] private IntRange _swordShowTargetsPerRound; 				/// <summary>Sword Show Targets per Round's Range.</summary>
	[SerializeField] private float _swordShowRoundDuration; 					/// <summary>Sword Show's Round Duration.</summary>
	[SerializeField] private float _swordTargetShakeDuration; 					/// <summary>Sword Target's Shake Duration.</summary>
	[SerializeField] private float _swordTargetShakeSpeed; 						/// <summary>Sword Target's Shake Speed.</summary>
	[SerializeField] private float _swordTargetShakeMagnitude; 					/// <summary>Sword Target's Shake Magnitude.</summary>
	[SerializeField] private float _swordTargetInterpolationDuration; 			/// <summary>Interpolation duration it takes the target to reach the spawn's destiny.</summary>
	[Space(5f)]
	[Header("Dance Show's Attributes:")]
	[SerializeField] private int[] _ringsIndices; 								/// <summary>Rings' Indices.</summary>
	[SerializeField] private FloatRange _ySpawnLimits; 							/// <summary>Spawn Limits on the Y's Axis.</summary>
	[SerializeField] private FloatRange _xOffset; 								/// <summary>Range of offset on the X's axis.</summary>
	[SerializeField] private IntRange _danceShowRounds; 						/// <summary>Dance Show's Rounds.</summary>
	[SerializeField] private IntRange _ringsPerRound; 							/// <summary>Rings per-round.</summary>
	[SerializeField] private float _danceShowDuration; 							/// <summary>Dance Show's Duration.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _danceShowSuccessPercentage; 				/// <summary>Success' Percentage for the Dance Show.</summary>
	[Space(5f)]
	[Header("Crowd's Boos Attributes:")]
	[SerializeField] private int[] _trashProjectilesIndices; 					/// <summary>Indices of all the trash (Parabola) Projectiles.</summary>
	[SerializeField] private int[] _applauseObjectsIndices; 					/// <summary>Indices of objects thrown at an applause.</summary>
	[SerializeField] private Vector3[] _trashProjectilesWaypoints; 				/// <summary>Trash Projectiles' Waypoints.</summary>
	[SerializeField] private IntRange _trashProjectilesPerRound; 				/// <summary>Range of Trash projectiles per round.</summary>
	[SerializeField] private FloatRange _trashProjectileCooldown; 				/// <summary>Cooldown duration's range per trash Projectile.</summary>
	[SerializeField] private FloatRange _mateoPositionProjection; 				/// <summary>Range of Mateo's Time Projection.</summary>
	[SerializeField] private float _trashProjectileTime; 						/// <summary>Parabola's (Trash) time it takes to potentially reach mateo.</summary>

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

	/// <summary>Gets fireShowPieceIndex property.</summary>
	public int fireShowPieceIndex { get { return _fireShowPieceIndex; } }

	/// <summary>Gets swordShowPieceIndex property.</summary>
	public int swordShowPieceIndex { get { return _swordShowPieceIndex; } }

	/// <summary>Gets danceShowPieceIndex property.</summary>
	public int danceShowPieceIndex { get { return _danceShowPieceIndex; } }

	/// <summary>Gets applauseSoundIndex property.</summary>
	public int applauseSoundIndex { get { return _applauseSoundIndex; } }

	/// <summary>Gets booingSoundIndex property.</summary>
	public int booingSoundIndex { get { return _booingSoundIndex; } }

	/// <summary>Gets fireTargetIndices property.</summary>
	public int[] fireTargetIndices { get { return _fireTargetIndices; } }

	/// <summary>Gets swordTargetIndices property.</summary>
	public int[] swordTargetIndices { get { return _swordTargetIndices; } }

	/// <summary>Gets trashProjectilesIndices property.</summary>
	public int[] trashProjectilesIndices { get { return _trashProjectilesIndices; } }

	/// <summary>Gets applauseObjectsIndices property.</summary>
	public int[] applauseObjectsIndices { get { return _applauseObjectsIndices; } }

	/// <summary>Gets ringsIndices property.</summary>
	public int[] ringsIndices { get { return _ringsIndices; } }

	/// <summary>Gets trashProjectilesWaypoints property.</summary>
	public Vector3[] trashProjectilesWaypoints { get { return _trashProjectilesWaypoints; } }

	/// <summary>Gets showSignSpawnPoint property.</summary>
	public Vector3 showSignSpawnPoint { get { return _showSignSpawnPoint; } }

	/// <summary>Gets showSignPresentationPoint property.</summary>
	public Vector3 showSignPresentationPoint { get { return _showSignPresentationPoint; } }

	/// <summary>Gets showSignDestinyPoint property.</summary>
	public Vector3 showSignDestinyPoint { get { return _showSignDestinyPoint; } }

	/// <summary>Gets signEntranceDuration property.</summary>
	public float signEntranceDuration { get { return _signEntranceDuration; } }

	/// <summary>Gets signExitDuration property.</summary>
	public float signExitDuration { get { return _signExitDuration; } }

	/// <summary>Gets signIdleDuration property.</summary>
	public float signIdleDuration { get { return _signIdleDuration; } }

	/// <summary>Gets fireShowRoundDuration property.</summary>
	public float fireShowRoundDuration { get { return _fireShowRoundDuration; } }

	/// <summary>Gets swordShowRoundDuration property.</summary>
	public float swordShowRoundDuration { get { return _swordShowRoundDuration; } }

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

	/// <summary>Gets danceShowDuration property.</summary>
	public float danceShowDuration { get { return _danceShowDuration; } }

	/// <summary>Gets danceShowSuccessPercentage property.</summary>
	public float danceShowSuccessPercentage { get { return _danceShowSuccessPercentage; } }

	/// <summary>Gets swordTargetsSpawnWaypoints property.</summary>
	public Vector3[] swordTargetsSpawnWaypoints { get { return _swordTargetsSpawnWaypoints; } }

	/// <summary>Gets swordTargetsDestinyWaypoints property.</summary>
	public Vector3[] swordTargetsDestinyWaypoints { get { return _swordTargetsDestinyWaypoints; } }

	/// <summary>Gets fireShowRounds property.</summary>
	public IntRange fireShowRounds { get { return _fireShowRounds; } }

	/// <summary>Gets fireShowTargetsPerRound property.</summary>
	public IntRange fireShowTargetsPerRound { get { return _fireShowTargetsPerRound; } }

	/// <summary>Gets swordShowRounds property.</summary>
	public IntRange swordShowRounds { get { return _swordShowRounds; } }

	/// <summary>Gets swordShowTargetsPerRound property.</summary>
	public IntRange swordShowTargetsPerRound { get { return _swordShowTargetsPerRound; } }

	/// <summary>Gets trashProjectilesPerRound property.</summary>
	public IntRange trashProjectilesPerRound { get { return _trashProjectilesPerRound; } }

	/// <summary>Gets danceShowRounds property.</summary>
	public IntRange danceShowRounds { get { return _danceShowRounds; } }

	/// <summary>Gets ringsPerRound property.</summary>
	public IntRange ringsPerRound { get { return _ringsPerRound; } }

	/// <summary>Gets trashProjectileCooldown property.</summary>
	public FloatRange trashProjectileCooldown { get { return _trashProjectileCooldown; } }

	/// <summary>Gets mateoPositionProjection property.</summary>
	public FloatRange mateoPositionProjection { get { return _mateoPositionProjection; } }

	/// <summary>Gets ySpawnLimits property.</summary>
	public FloatRange ySpawnLimits { get { return _ySpawnLimits; } }

	/// <summary>Gets xOffset property.</summary>
	public FloatRange xOffset { get { return _xOffset; } }
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

		Gizmos.color = gizmosColor;

		if(trashProjectilesWaypoints != null) foreach(Vector3 waypoint in trashProjectilesWaypoints)
		{
			Gizmos.DrawWireSphere(waypoint, gizmosRadius);
		}

		if(swordTargetsSpawnWaypoints != null) foreach(Vector3 waypoint in swordTargetsSpawnWaypoints)
		{
			Gizmos.DrawWireSphere(waypoint, gizmosRadius);
		}
		if(swordTargetsDestinyWaypoints != null) foreach(Vector3 waypoint in swordTargetsDestinyWaypoints)
		{
			Gizmos.DrawWireSphere(waypoint, gizmosRadius);
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

		IEnumerator[] routines = VArray.RandomSet(SwordShowRoutine(boss), SwordShowRoutine(boss), SwordShowRoutine(boss));

		foreach(IEnumerator routine in routines)
		{
			while(routine.MoveNext()) yield return null;
		}

		boss.Sing();
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
			- Stop the instrumental and voice loops
			- Show the Sign, wait till the exposure is over
			- Play the Fire Show's Loop
			- Play the Minigame
			- Make a judgement after the game has finished
		*/
		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		Transform fireShowSign = DestinoSceneController.Instance.fireShowSign;
		AudioClip clip = null;
		IEnumerator signDisplacement = DisplayShowSign(fireShowSign);
		BreakTheTargetsMiniGame miniGame = null;
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(clip.length);

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

		clip = AudioController.Play(SourceType.Loop, 0, fireShowPieceIndex, false);
		miniGame.timeLimit = clip.length;
		miniGame.Initialize(this, OnBreakTheTargetsMiniGameEvent);

		while(miniGame.state == MiniGameState.Running) yield return null;

		showJudgement = EvaluateShow(miniGame.scorePercentage, fireShowSuccessPercentage);
		while(showJudgement.MoveNext()) yield return null;

		signDisplacement = HidehowSign(fireShowSign);
		while(signDisplacement.MoveNext()) yield return null;

		AudioController.Stop(SourceType.Loop, 0, ()=>
		{
			AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
			AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex);
		});
	}

	/// <summary>Sword Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator SwordShowRoutine(DestinoBoss boss)
	{
		/*
			- Stop the instrumental and voice loops
			- Show the Sign, wait till the exposure is over
			- Play the Sword Show's Loop
			- Play the Minigame
			- Make a judgement after the game has finished
		*/
		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		Transform swordShowSign = DestinoSceneController.Instance.swordShowSign;
		AudioClip clip = null;
		IEnumerator signDisplacement = DisplayShowSign(DestinoSceneController.Instance.swordShowSign);
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(swordShowRoundDuration);
		int rounds = swordShowRounds.Random();
		int targetsPerRound = 0;
		int count = 0;
		float targetsDestroyed = 0.0f;
		OnDeactivated onTargetDeactivation = (cause, info)=>
		{
			switch(cause)
			{

				case DeactivationCause.LeftBoundaries:
				case DeactivationCause.LifespanOver:
				count++;
				break;

				default:
				targetsDestroyed++;
				count++;
				break;
			}
		};

		while(signDisplacement.MoveNext()) yield return null;

		clip = AudioController.Play(SourceType.Loop, 0, swordShowPieceIndex, false);
		wait.waitDuration = clip.length;

		for(int i = 0; i < rounds; i++)
		{
			wait.waitDuration = clip.length;
			targetsPerRound = swordShowTargetsPerRound.Random();
			count = 0;
			float fTargetsPerRound = (float)targetsPerRound;
			targetsDestroyed = 0.0f;
			Fragmentable[] targets = new Fragmentable[targetsPerRound];

			for(int j = 0; j < targetsPerRound; j++)
			{
				int index = UnityEngine.Random.Range(0, swordTargetsSpawnWaypoints.Length);
				Vector3 origin = swordTargetsSpawnWaypoints[index];
				Vector3 destiny = swordTargetsDestinyWaypoints[index];
				Ray ray = new Ray(origin, destiny - origin);
				Fragmentable fragmentableTarget = PoolManager.RequestPoolGameObject(swordTargetIndices.Random(), origin, Quaternion.identity) as Fragmentable;
				
				if(fragmentableTarget == null) yield break;

				fragmentableTarget.rigidbody.gravityScale = 0.0f;
				fragmentableTarget.eventsHandler.onDeactivated -= onTargetDeactivation;
				fragmentableTarget.eventsHandler.onDeactivated += onTargetDeactivation;

				IEnumerator lerp = fragmentableTarget.transform.DisplaceToPosition(destiny, swordTargetInterpolationDuration);

				while(lerp.MoveNext()) yield return null;

				targets[j] = fragmentableTarget;

				IEnumerator targetShaking = fragmentableTarget.transform.ShakePosition(swordTargetShakeDuration, swordTargetShakeSpeed, swordTargetShakeMagnitude);

				while(targetShaking.MoveNext()) yield return null;

				fragmentableTarget.rigidbody.gravityScale = 1.0f;
			}

			while(wait.MoveNext() && count < targetsPerRound) yield return null;
			wait.Reset();

			foreach(Fragmentable target in targets)
			{
				target.eventsHandler.onDeactivated -= onTargetDeactivation;

			}

			showJudgement = EvaluateShow((float)targetsDestroyed / targetsDestroyed, swordShowSuccessPercentage);

			while(showJudgement.MoveNext()) yield return null;
		}

		AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
		AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex);
		DestinoSceneController.Instance.swordShowSign.SetActive(false);
	}

	/// <summary>Dance Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator DanceShowRoutine(DestinoBoss boss)
	{
		/*
			- Stop the instrumental and voice loops
			- Show the Sign, wait till the exposure is over
			- Play the Dance Show's Loop
			- Play the Ring-Madness' Minigame
			- Make a judgement after the game has finished
		*/
		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		Transform danceShowSign = DestinoSceneController.Instance.danceShowSign;
		AudioClip clip = null;
		RingMadnessMiniGame miniGame = null;
		IEnumerator signDisplacement = DisplayShowSign(DestinoSceneController.Instance.danceShowSign);
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(danceShowDuration);

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

		clip = AudioController.Play(SourceType.Loop, 0, danceShowPieceIndex, false);
		miniGame.timeLimit = clip.length;
		miniGame.Initialize(this, OnRingMadnessMiniGameEvent);

		while(miniGame.state == MiniGameState.Running) yield return null;

		showJudgement = EvaluateShow(miniGame.scorePercentage, danceShowSuccessPercentage);
		while(showJudgement.MoveNext()) yield return null;

		signDisplacement = HidehowSign(danceShowSign);
		while(signDisplacement.MoveNext()) yield return null;

		AudioController.Stop(SourceType.Loop, 0, ()=>
		{
			AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
			AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex);
		});
	}

	/// <summary>Evaluates show.</summary>
	/// <param name="_ratio">Mini-Game's Score Percentage.</param>
	/// <param name="_achievePercentageForSuccess">Required percentage for the show to be susccessful.</param>
	private IEnumerator EvaluateShow(float _ratio, float _achievePercentageForSuccess)
	{
		if(_ratio >= _achievePercentageForSuccess)
		{
			AudioController.PlayOneShot(SourceType.Scenario, 0, applauseSoundIndex);

			SecondsDelayWait wait = new SecondsDelayWait(0.0f);
			int rounds = trashProjectilesPerRound.Random();

			for(int i = 0; i < rounds; i++)
			{
				wait.ChangeDurationAndReset(trashProjectileCooldown.Random());
				PoolManager.RequestParabolaProjectile(
					Faction.Enemy,
					applauseObjectsIndices.Random(),
					trashProjectilesWaypoints.Random(),
					Game.ProjectMateoPosition(mateoPositionProjection.Random()),
					trashProjectileTime
				);

				while(wait.MoveNext()) yield return null;
			}
		}
		else
		{
			AudioController.PlayOneShot(SourceType.Scenario, 0, booingSoundIndex);

			SecondsDelayWait wait = new SecondsDelayWait(0.0f);
			int rounds = trashProjectilesPerRound.Random();

			for(int i = 0; i < rounds; i++)
			{
				wait.ChangeDurationAndReset(trashProjectileCooldown.Random());
				PoolManager.RequestParabolaProjectile(
					Faction.Enemy,
					trashProjectilesIndices.Random(),
					trashProjectilesWaypoints.Random(),
					Game.ProjectMateoPosition(mateoPositionProjection.Random()),
					trashProjectileTime
				);

				while(wait.MoveNext()) yield return null;
			}
		}
	
		yield return null;
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