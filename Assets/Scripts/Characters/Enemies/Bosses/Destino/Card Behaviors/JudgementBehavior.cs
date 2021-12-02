using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

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
	[Header("Loops:")]
	[SerializeField] private CollectionIndex _fireShowPieceIndex; 					/// <summary>Fire Show's Piece's Index.</summary>
	[SerializeField] private CollectionIndex _swordShowPieceIndex; 					/// <summary>Sword Show's Piece's Index.</summary>
	[SerializeField] private CollectionIndex _danceShowPieceIndex; 					/// <summary>Dance Show's Piece's Index.</summary>
	[Space(5f)]
	[Header("Sound Effects:")]
	[SerializeField] private CollectionIndex _applauseSoundIndex; 					/// <summary>Applause's Sound Index.</summary>
	[SerializeField] private CollectionIndex _booingSoundIndex; 					/// <summary>Booing's Sound Index.</summary>
	[Space(5f)]
	[Header("Signs' Attributes:")]
	[SerializeField] private Vector3 _fireShowSignSpawnPoint; 						/// <summary>Fire Show Sign's Spawn Position.</summary>
	[SerializeField] private Vector3 _swordShowSignSpawnPoint; 						/// <summary>Sword Show Sign's Spawn Position.</summary>
	[SerializeField] private Vector3 _danceShowSignSpawnPoint; 						/// <summary>Dance Show Sign's Spawn Position.</summary>
	[SerializeField] private Vector3 _fireShowSignDestinyPoint; 					/// <summary>Fire Show Sign's Destiny Position.</summary>
	[SerializeField] private Vector3 _swordShowSignDestinyPoint; 					/// <summary>Sword Show Sign's Destiny Position.</summary>
	[SerializeField] private Vector3 _danceShowSignDestinyPoint; 					/// <summary>Dance Show Sign's Destiny Position.</summary>
	[SerializeField] private float _signEntranceDuration; 							/// <summary>Sign Entrance's Duration.</summary>
	[SerializeField] private float _signExitDuration; 								/// <summary>Sign Exit's Duration.</summary>
	[SerializeField] private float _signIdleDuration; 								/// <summary>Sign Idle's Duration.</summary>
	[Space(5f)]
	[Header("Fire Show's Attributes:")]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _fireShowSuccessPercentage; 					/// <summary>Success' Percentage for the Fire Show.</summary>
	[SerializeField] private CollectionIndex[] _fireTargetIndices; 					/// <summary>Fire Target's Indices on the Game's Data.</summary>
	[SerializeField] private BoundaryWaypointsContainer _fireShowWaypoints; 		/// <summary>Fire Show's Waypoints.</summary>
	[SerializeField] private IntRange _fireShowRounds; 								/// <summary>Fire Show Rounds' Range.</summary>
	[SerializeField] private IntRange _fireShowTargetsPerRound; 					/// <summary>Fire Show Targets per Round's Range.</summary>
	[SerializeField] private float _fireShowRoundDuration; 							/// <summary>Fire Show's Round Duration.</summary>
	[Space(5f)]
	[Header("Sword Show's Attributes:")]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _swordShowSuccessPercentage; 					/// <summary>Success' Percentage for the Sword Show.</summary>
	[SerializeField] private CollectionIndex[] _swordTargetIndices; 				/// <summary>Sword Target's Indices on the Game's Data.</summary>
	[SerializeField] private Vector3[] _swordTargetsSpawnWaypoints; 				/// <summary>Sword Targets Waypoints [As tuples since they define interpolation points].</summary>
	[SerializeField] private Vector3[] _swordTargetsDestinyWaypoints; 				/// <summary>Sword Targets Waypoints [As tuples since they define interpolation points].</summary>
	//[SerializeField] private BoundaryWaypointsContainer _swordShowWaypoints; 		/// <summary>Sword Show's Waypoints.</summary>
	[SerializeField] private IntRange _swordShowRounds; 							/// <summary>Sword Show Rounds' Range.</summary>
	[SerializeField] private IntRange _swordShowTargetsPerRound; 					/// <summary>Sword Show Targets per Round's Range.</summary>
	[SerializeField] private float _swordShowRoundDuration; 						/// <summary>Sword Show's Round Duration.</summary>
	[SerializeField] private float _swordTargetShakeDuration; 						/// <summary>Sword Target's Shake Duration.</summary>
	[SerializeField] private float _swordTargetShakeSpeed; 							/// <summary>Sword Target's Shake Speed.</summary>
	[SerializeField] private float _swordTargetShakeMagnitude; 						/// <summary>Sword Target's Shake Magnitude.</summary>
	[SerializeField] private float _swordTargetInterpolationDuration; 				/// <summary>Interpolation duration it takes the target to reach the spawn's destiny.</summary>
	[Space(5f)]
	[Header("Dance Show's Attributes:")]
	[SerializeField] private CollectionIndex[] _ringsIndices; 						/// <summary>Rings' Indices.</summary>
	[SerializeField] private FloatRange _ySpawnLimits; 								/// <summary>Spawn Limits on the Y's Axis.</summary>
	[SerializeField] private FloatRange _xOffset; 									/// <summary>Range of offset on the X's axis.</summary>
	[SerializeField] private IntRange _danceShowRounds; 							/// <summary>Dance Show's Rounds.</summary>
	[SerializeField] private IntRange _ringsPerRound; 								/// <summary>Rings per-round.</summary>
	[SerializeField] private float _danceShowDuration; 								/// <summary>Dance Show's Duration.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _danceShowSuccessPercentage; 					/// <summary>Success' Percentage for the Dance Show.</summary>
	[Space(5f)]
	[Header("Crowd's Boos Attributes:")]
	[SerializeField] private CollectionIndex[] _trashProjectilesIndices; 			/// <summary>Indices of all the trash (Parabola) Projectiles.</summary>
	[SerializeField] private CollectionIndex[] _applauseObjectsIndices; 			/// <summary>Indices of objects thrown at an applause.</summary>
	[SerializeField] private Vector3[] _trashProjectilesWaypoints; 					/// <summary>Trash Projectiles' Waypoints.</summary>
	[SerializeField] private IntRange _trashProjectilesPerRound; 					/// <summary>Range of Trash projectiles per round.</summary>
	[SerializeField] private FloatRange _trashProjectileCooldown; 					/// <summary>Cooldown duration's range per trash Projectile.</summary>
	[SerializeField] private FloatRange _mateoPositionProjection; 					/// <summary>Range of Mateo's Time Projection.</summary>
	[SerializeField] private float _trashProjectileTime; 							/// <summary>Parabola's (Trash) time it takes to potentially reach mateo.</summary>

#region Getters/Setters:
	/// <summary>Gets fireShowPieceIndex property.</summary>
	public CollectionIndex fireShowPieceIndex { get { return _fireShowPieceIndex; } }

	/// <summary>Gets swordShowPieceIndex property.</summary>
	public CollectionIndex swordShowPieceIndex { get { return _swordShowPieceIndex; } }

	/// <summary>Gets danceShowPieceIndex property.</summary>
	public CollectionIndex danceShowPieceIndex { get { return _danceShowPieceIndex; } }

	/// <summary>Gets applauseSoundIndex property.</summary>
	public CollectionIndex applauseSoundIndex { get { return _applauseSoundIndex; } }

	/// <summary>Gets booingSoundIndex property.</summary>
	public CollectionIndex booingSoundIndex { get { return _booingSoundIndex; } }

	/// <summary>Gets fireTargetIndices property.</summary>
	public CollectionIndex[] fireTargetIndices { get { return _fireTargetIndices; } }

	/// <summary>Gets swordTargetIndices property.</summary>
	public CollectionIndex[] swordTargetIndices { get { return _swordTargetIndices; } }

	/// <summary>Gets trashProjectilesIndices property.</summary>
	public CollectionIndex[] trashProjectilesIndices { get { return _trashProjectilesIndices; } }

	/// <summary>Gets applauseObjectsIndices property.</summary>
	public CollectionIndex[] applauseObjectsIndices { get { return _applauseObjectsIndices; } }

	/// <summary>Gets ringsIndices property.</summary>
	public CollectionIndex[] ringsIndices { get { return _ringsIndices; } }

	/// <summary>Gets trashProjectilesWaypoints property.</summary>
	public Vector3[] trashProjectilesWaypoints { get { return _trashProjectilesWaypoints; } }

	/// <summary>Gets fireShowSignSpawnPoint property.</summary>
	public Vector3 fireShowSignSpawnPoint { get { return _fireShowSignSpawnPoint; } }

	/// <summary>Gets swordShowSignSpawnPoint property.</summary>
	public Vector3 swordShowSignSpawnPoint { get { return _swordShowSignSpawnPoint; } }

	/// <summary>Gets danceShowSignSpawnPoint property.</summary>
	public Vector3 danceShowSignSpawnPoint { get { return _danceShowSignSpawnPoint; } }

	/// <summary>Gets fireShowSignDestinyPoint property.</summary>
	public Vector3 fireShowSignDestinyPoint { get { return _fireShowSignDestinyPoint; } }

	/// <summary>Gets swordShowSignDestinyPoint property.</summary>
	public Vector3 swordShowSignDestinyPoint { get { return _swordShowSignDestinyPoint; } }

	/// <summary>Gets danceShowSignDestinyPoint property.</summary>
	public Vector3 danceShowSignDestinyPoint { get { return _danceShowSignDestinyPoint; } }

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

	/// <summary>Gets fireShowWaypoints property.</summary>
	public BoundaryWaypointsContainer fireShowWaypoints { get { return _fireShowWaypoints; } }

	/*/// <summary>Gets swordShowWaypoints property.</summary>
	public BoundaryWaypointsContainer swordShowWaypoints { get { return _swordShowWaypoints; } }*/

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

		Gizmos.DrawWireSphere(fireShowSignSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(swordShowSignSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(danceShowSignSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(fireShowSignDestinyPoint, gizmosRadius);
		Gizmos.DrawWireSphere(swordShowSignDestinyPoint, gizmosRadius);
		Gizmos.DrawWireSphere(danceShowSignDestinyPoint, gizmosRadius);

		fireShowWaypoints.OnDrawGizmos();
		//swordShowWaypoints.OnDrawGizmos();

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

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{
		/// If you wanna test indefinitely just one:
		/*IEnumerator fireShowDisplacement = FireShowRoutine(boss);
		fireShowDisplacement = SwordShowRoutine(boss);
		fireShowDisplacement = DanceShowRoutine(boss);

		while(fireShowDisplacement.MoveNext()) yield return null;*/

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);

		/// If you wanna test all:
#region SignsShowcase:
		IEnumerator[] routines = VArray.RandomSet(FireShowRoutine(boss), SwordShowRoutine(boss), DanceShowRoutine(boss));
		//IEnumerator[] routines = new IEnumerator[3];
		//routines[0] = FireShowRoutine(boss);
		//routines[1] = SwordShowRoutine(boss);
		//routines[2] = DanceShowRoutine(boss);

		/*foreach(IEnumerator routine in routines)
		{
			while(routine.MoveNext()) yield return null;
		}*/

		IEnumerator fuckU = routines[0];
		while(fuckU.MoveNext()) yield return null;
#endregion

		yield return null;
		boss.Sing();
		InvokeCoroutineEnd();
	}

	/// <summary>Displays and Hides Sign.</summary>
	/// <param name="_sign">Sign's Transform.</param>
	/// <param name="_spawnPoint">Origin.</param>
	/// <param name="_destinyPoint">Destiny.</param>
	private IEnumerator DisplayAndHideSign(Transform _sign, Vector3 _spawnPoint, Vector3 _destinyPoint)
	{
		_sign.gameObject.SetActive(true);
		_sign.position = _spawnPoint;

		SecondsDelayWait wait = new SecondsDelayWait(signIdleDuration);
		IEnumerator displacement = _sign.DisplaceToPosition(_destinyPoint, signEntranceDuration);

		while(displacement.MoveNext()) yield return null;
		while(wait.MoveNext()) yield return null;

		displacement = _sign.DisplaceToPosition(_spawnPoint, signExitDuration);

		while(displacement.MoveNext()) yield return null;

		//_sign.gameObject.SetActive(false);
	}

	/// <summary>Fire Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator FireShowRoutine(DestinoBoss boss)
	{
		/// THANKS SALAZAR ELEFUCK
		/*return TargetShowRoutine(
			DestinoSceneController.Instance.fireShowSign,
			fireShowSignSpawnPoint,
			fireShowSignDestinyPoint,
			fireShowRoundDuration,
			fireShowRounds,
			fireShowTargetsPerRound,
			fireTargetIndices,
			fireShowSuccessPercentage
		);*/

		/// Still need to decide when to deprecate.
		/*AudioController.GetLoopSource(0).Pause();
		AudioController.GetLoopSource(1).Pause();*/
		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		IEnumerator signDisplacement = DisplayAndHideSign(DestinoSceneController.Instance.fireShowSign, fireShowSignSpawnPoint, fireShowSignDestinyPoint);
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(fireShowRoundDuration);
		int rounds = fireShowRounds.Random();
		int targetsPerRound = 0;
		int count = 0;
		float targetsDestroyed = 0.0f;
		//Gil Properties Test Area
		HashSet<int> projectileSet = new HashSet<int>();

		
		OnProjectileDeactivated onTargetDeactivation = (projectile, cause, info)=>
		{

			int ID = projectile.GetInstanceID();
			

			//Gil Modification Test
			if(projectileSet.Contains(ID))
			{
				return;
			}
			else{
				switch(cause)
				{
					case DeactivationCause.LeftBoundaries:
					Ray ray = fireShowWaypoints.GetTargetOriginAndDirection();
					projectile.OnObjectReset();
					//projectile.transform.position = ray.origin;
					//projectile.direction = direction;
					projectile.direction = -projectile.direction;
					projectile.activated = true;
					projectile.transform.position += (projectile.direction * 3.0f);
					break;

					default:
						targetsDestroyed++;
						count++;
					break;
				}
				projectileSet.Add(ID);
			}
		};

		while(signDisplacement.MoveNext()) yield return null;

		AudioClip clip = AudioController.Play(SourceType.Loop, 0, fireShowPieceIndex, false);
		wait.waitDuration = clip.length;
		wait.waitDuration = clip.length;
		targetsPerRound = fireShowTargetsPerRound.Random();
		float fTargetsPerRound = (float)targetsPerRound;
		//targetsDestroyed = 0.0f;
		Projectile[] targets = new Projectile[targetsPerRound];

		for(int j = 0; j < targetsPerRound; j++)
		{
			Ray ray = fireShowWaypoints.GetTargetOriginAndDirection();
			Projectile p = PoolManager.RequestProjectile(Faction.Enemy, fireTargetIndices.Random(), ray.origin, ray.direction);
			p.projectileEventsHandler.onProjectileDeactivated -= onTargetDeactivation;
			p.projectileEventsHandler.onProjectileDeactivated += onTargetDeactivation;
			p.lifespan = 0.0f; 	/// Make 'em last for ever...
			targets[j] = p;
		}

		while(wait.MoveNext() && count < targetsPerRound) yield return null;
		wait.Reset();

		AudioController.Stop(SourceType.Loop, 0);

		foreach(Projectile target in targets)
		{
			target.OnObjectDeactivation();
			target.projectileEventsHandler.onProjectileDeactivated -= onTargetDeactivation;
		}

		showJudgement = EvaluateShow(fTargetsPerRound, targetsDestroyed, fireShowSuccessPercentage);

		while(showJudgement.MoveNext()) yield return null;

		/// Still deciding when to deprecate
		/*AudioController.GetLoopSource(2).Stop();
		AudioController.GetLoopSource(0).UnPause();
		AudioController.GetLoopSource(1).UnPause();*/
		AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
		AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex);
		DestinoSceneController.Instance.fireShowSign.SetActive(false);
	}

	/// <summary>Sword Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator SwordShowRoutine(DestinoBoss boss)
	{
		/*return TargetShowRoutine(
			DestinoSceneController.Instance.swordShowSign,
			swordShowSignSpawnPoint,
			swordShowSignDestinyPoint,
			swordShowRoundDuration,
			swordShowRounds,
			swordShowTargetsPerRound,
			swordTargetIndices,
			swordShowSuccessPercentage
		);*/

		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		IEnumerator signDisplacement = DisplayAndHideSign(DestinoSceneController.Instance.swordShowSign, swordShowSignSpawnPoint, swordShowSignDestinyPoint);
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(swordShowRoundDuration);
		int rounds = swordShowRounds.Random();
		int targetsPerRound = 0;
		int count = 0;
		float targetsDestroyed = 0.0f;
		//Gil Properties
		HashSet<int> projectileSet = new HashSet<int>();


		OnProjectileDeactivated onTargetDeactivation = (projectile, cause, info)=>
		{
			int ID = projectile.GetInstanceID();
			
			
			if(projectileSet.Contains(ID))
			{
				return;
			}else{

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
			projectileSet.Add(ID);
			}
		};

		while(signDisplacement.MoveNext()) yield return null;

		/*for(int i = 0; i < rounds; i++)
		{*/
			AudioClip clip = AudioController.Play(SourceType.Loop, 0, swordShowPieceIndex, false);
			wait.waitDuration = clip.length;
			targetsPerRound = swordShowTargetsPerRound.Random();
			count = 0;
			float fTargetsPerRound = (float)targetsPerRound;
			targetsDestroyed = 0.0f;
			Projectile[] targets = new Projectile[targetsPerRound];

			for(int j = 0; j < targetsPerRound; j++)
			{
				int index = UnityEngine.Random.Range(0, swordTargetsSpawnWaypoints.Length);
				Vector3 origin = swordTargetsSpawnWaypoints[index];
				Vector3 destiny = swordTargetsDestinyWaypoints[index];
				Ray ray = new Ray(origin, destiny - origin);
				Projectile p = PoolManager.RequestProjectile(Faction.Enemy, swordTargetIndices.Random(), ray.origin, ray.direction);
				p.activated = false;
				p.projectileEventsHandler.onProjectileDeactivated -= onTargetDeactivation;
				p.projectileEventsHandler.onProjectileDeactivated += onTargetDeactivation;

				IEnumerator lerp = p.transform.DisplaceToPosition(destiny, swordTargetInterpolationDuration);

				while(lerp.MoveNext()) yield return null;

				targets[j] = p;

				IEnumerator targetShaking = p.transform.ShakePosition(swordTargetShakeDuration, swordTargetShakeSpeed, swordTargetShakeMagnitude);

				while(targetShaking.MoveNext()) yield return null;

				p.activated = true;
			}

			while(wait.MoveNext() && count < targetsPerRound) yield return null;
			wait.Reset();

			foreach(Projectile target in targets)
			{
				target.projectileEventsHandler.onProjectileDeactivated -= onTargetDeactivation;
			}

			showJudgement = EvaluateShow(fTargetsPerRound, targetsDestroyed, swordShowSuccessPercentage);

			while(showJudgement.MoveNext()) yield return null;
		//}

		AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
		AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex);
		DestinoSceneController.Instance.swordShowSign.SetActive(false);
	}

	/// <summary>Dance Show's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator DanceShowRoutine(DestinoBoss boss)
	{
		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		IEnumerator signDisplacement = DisplayAndHideSign(DestinoSceneController.Instance.danceShowSign, danceShowSignSpawnPoint, danceShowSignDestinyPoint);
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(danceShowDuration);
		int rounds = danceShowRounds.Random();
		float ringsPassed = 0.0f;
		float fRingsPerRound = 0.0f;
		float totalRingsPassed = 0.0f;
		Vector2 maxJumpForce = Game.GetMateoMaxJumpingHeight();
		OnRingPassed onRingPassed = (collider)=> { ringsPassed++; };

		while(signDisplacement.MoveNext()) yield return null;

		AudioClip clip = AudioController.Play(SourceType.Loop, 0, danceShowPieceIndex, false);
		wait.waitDuration = clip.length;

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
		}

		showJudgement = EvaluateShow(fRingsPerRound, totalRingsPassed, danceShowSuccessPercentage);

		while(showJudgement.MoveNext()) yield return null;

		AudioController.Stop(SourceType.Loop, 0, ()=>
		{
			AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
			AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex);
		});

		DestinoSceneController.Instance.danceShowSign.SetActive(false);
	}

	/// <summary>Evaluates show.</summary>
	/// <param name="_count">Current show's count to evaluate.</param>
	/// <param name="_achieved">Number of achieved instances.</param>
	/// <param name="_achievePercentageForSuccess">Required percentage for the show to be susccessful.</param>
	private IEnumerator EvaluateShow(float _count, float _achieved, float _achievePercentageForSuccess)
	{
		float ratio  = _achieved / _count;

		if(ratio >= _achievePercentageForSuccess)
		{
			/// Here yo play the cheer sounds:
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
}
}

/// DEPRECATED THANKS TO SUDDEN SALAZAR ELEFUCK'S CHANGES
	/*/// <summary>Target Show's (whether Fire or Sword) Routine.</summary>
	/// <param name="_sign">Sign to displace.</param>
	/// <param name="_spawn">Sign's spawn position.</param>
	/// <param name="_destiny">Sign's destiny position.</param>
	/// <param name="_duration">Show's duration.</param>
	/// <param name="_rounds">Range of rounds per show.</param>
	/// <param name="_targetsPerRound">Targets per-round.</param>
	/// <param name="_indices">Targets' Indices.</param>
	/// <param name="_successPercentage">Percentage to consider this show a success.</param>
	private IEnumerator TargetShowRoutine(Transform _sign, Vector3 _spawn, Vector3 _destiny, float _duration, IntRange _rounds, IntRange _targetsPerRound, CollectionIndex[] _indices, float _successPercentage)
	{
		IEnumerator signDisplacement = DisplayAndHideSign(_sign, _spawn, _destiny);
		IEnumerator showJudgement = null;
		SecondsDelayWait wait = new SecondsDelayWait(_duration);
		int rounds = _rounds.Random();
		int targetsPerRound = 0;
		float targetsDestroyed = 0.0f;
		OnProjectileDeactivated onTargetDeactivation = (cause, info)=>
		{
			switch(cause)
			{
				case DeactivationCause.Destroyed:
				targetsDestroyed++;
				break;
			}
		};

		while(signDisplacement.MoveNext()) yield return null;

		for(int i = 0; i < rounds; i++)
		{
			targetsPerRound = _targetsPerRound.Random();
			float fTargetsPerRound = (float)targetsPerRound;
			targetsDestroyed = 0.0f;
			Projectile[] targets = new Projectile[targetsPerRound];

			for(int j = 0; j < targetsPerRound; j++)
			{
				Ray ray = fireShowWaypoints.GetTargetOriginAndDirection();
				Projectile p = PoolManager.RequestProjectile(Faction.Enemy, _indices.Random(), ray.origin, ray.direction);
				p.projectileEventsHandler.onProjectileDeactivated -= onTargetDeactivation;
				p.projectileEventsHandler.onProjectileDeactivated += onTargetDeactivation;

				targets[j] = p;
			}

			while(wait.MoveNext()) yield return null;
			wait.Reset();

			foreach(Projectile target in targets)
			{
				target.projectileEventsHandler.onProjectileDeactivated -= onTargetDeactivation;
			}

			showJudgement = EvaluateShow(fTargetsPerRound, targetsDestroyed, _successPercentage);

			while(showJudgement.MoveNext()) yield return null;
		}
	}*/