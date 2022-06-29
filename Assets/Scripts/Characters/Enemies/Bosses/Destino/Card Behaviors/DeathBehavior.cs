using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

using Random = UnityEngine.Random;

namespace Flamingo
{
public class DeathBehavior : DestinoScriptableCoroutine
{
	[Space(5f)]
	[SerializeField] private Vector3Pair[] _destinoSpawnPointsPairs; 									/// <summary>Destino Spawn-Points' Pairs.</summary>
	[SerializeField] private Vector3Pair _slashZone; 													/// <summary>Slash's Zone [the only parameters that will matter are the X's coordinates of both vectors].</summary>
	[SerializeField] private float _entranceLerpDuration; 												/// <summary>Entrance Interpolation's Duration.</summary>
	[SerializeField] private float _exitLerpDuration; 													/// <summary>Exit Interpolation's Duration.</summary>
	[Space(5f)]
	[TabGroup("Scythe")][SerializeField] private AIContactWeapon _scythe; 								/// <summary>Scythe's AIContactWeapon's Component.</summary>
	[Space(5f)]
	[Header("Rotations:")]
	[TabGroup("Scythe")][SerializeField] private EulerRotation _leftRotation; 							/// <summary>Left's Rotation.</summary>
	[TabGroup("Scythe")][SerializeField] private EulerRotation _rightRotation; 							/// <summary>Right's Rotation.</summary>
	[TabGroup("Scythe")][SerializeField] private float _rotationSpeed; 									/// <summary>Scythe's Rotation Speed.</summary>
	[Space(5f)]
	[Header("Interpolations")]
	[TabGroup("Scythe")][SerializeField] private Vector3 _spawnPosition; 								/// <summary>Scythe's Spawn Position.</summary>
	[TabGroup("Scythe")][SerializeField] private Vector3 _entrancePosition; 							/// <summary>Scythe's Entrance Position.</summary>
	[TabGroup("Scythe")][SerializeField] private float _interpolationDuration; 							/// <summary>Interpolation's Duration.</summary>
	[Space(5f)]
	[Header("Scythe's Animations' Credentials:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _entranceCredential; 			/// <summary>Scythe's Entrance AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _stage1AttackCredential; 		/// <summary>Stage 1's Scythe's Attack AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _stage2AttackCredential; 		/// <summary>Stage 2's Scythe's Attack AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _stage3AttackCredential; 		/// <summary>Stage 3's Scythe's Attack AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _exitCredential; 				/// <summary>Scythe's Exit AnimatorCredential.</summary>
	[Space(5f)]
	[Header("Scythe's Steering Attributes:")]
	[TabGroup("Scythe")][SerializeField] private float _buildUpMaxSpeed; 								/// <summary>Scythe's Max Speed.</summary>
	[TabGroup("Scythe")][SerializeField] private float _buildUpMaxSteeringForce; 						/// <summary>Scythe's Max Steering Force.</summary>
	[TabGroup("Scythe")][SerializeField] private float _swingMaxSpeed; 									/// <summary>Scythe's Max Speed.</summary>
	[TabGroup("Scythe")][SerializeField] private float _swingMaxSteeringForce; 							/// <summary>Scythe's Max Steering Force.</summary>
	[TabGroup("Scythe")][SerializeField] private float _arrivalRadius; 									/// <summary>Steering Arrival's Radius.</summary>
	[TabGroup("Scythe")][SerializeField] private float _maxSpeedScalar; 								/// <summary>Maximum's Speed's Scalar [it goes up as the stage goes up].</summary>
	[TabGroup("Scythe")][SerializeField] private float _additionalYOffset; 								/// <summary>Additional Y-Offset [while on Build-Up].</summary>
	[TabGroup("Scythe")][SerializeField] private float _clampedHeight; 									/// <summary>Clamped Height for the Scythe.</summary>
	[Space(5f)]
	[Header("Sound Effects:")]
	[SerializeField] private SoundEffectEmissionData _buildUpSoundEffect; 								/// <summary>Build-Up's Sound-Effect's Data.</summary>
	[SerializeField] private SoundEffectEmissionData _swingSoundEffect; 								/// <summary>Swing's Sound-Effect's Data.</summary>
	private AnimationEventInvoker _animationsEventInvoker; 												/// <summary>AnimationsEventInvoker's Component.</summary>
	private Coroutine scytheRotation; 																	/// <summary>Scythe's Rotation Coroutine Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets destinoSpawnPointsPairs property.</summary>
	public Vector3Pair[] destinoSpawnPointsPairs { get { return _destinoSpawnPointsPairs; } }

	/// <summary>Gets slashZone property.</summary>
	public Vector3Pair slashZone { get { return _slashZone; } }

	/// <summary>Gets scythe property.</summary>
	public AIContactWeapon scythe { get { return _scythe; } }

	/// <summary>Gets leftRotation property.</summary>
	public EulerRotation leftRotation { get { return _leftRotation; } }

	/// <summary>Gets rightRotation property.</summary>
	public EulerRotation rightRotation { get { return _rightRotation; } }

	/// <summary>Gets spawnPosition property.</summary>
	public Vector3 spawnPosition { get { return _spawnPosition; } }

	/// <summary>Gets entrancePosition property.</summary>
	public Vector3 entrancePosition { get { return _entrancePosition; } }

	/// <summary>Gets entranceCredential property.</summary>
	public AnimatorCredential entranceCredential { get { return _entranceCredential; } }

	/// <summary>Gets stage1AttackCredential property.</summary>
	public AnimatorCredential stage1AttackCredential { get { return _stage1AttackCredential; } }

	/// <summary>Gets stage2AttackCredential property.</summary>
	public AnimatorCredential stage2AttackCredential { get { return _stage2AttackCredential; } }

	/// <summary>Gets stage3AttackCredential property.</summary>
	public AnimatorCredential stage3AttackCredential { get { return _stage3AttackCredential; } }

	/// <summary>Gets exitCredential property.</summary>
	public AnimatorCredential exitCredential { get { return _exitCredential; } }

	/// <summary>Gets entranceLerpDuration property.</summary>
	public float entranceLerpDuration { get { return _entranceLerpDuration; } }

	/// <summary>Gets exitLerpDuration property.</summary>
	public float exitLerpDuration { get { return _exitLerpDuration; } }

	/// <summary>Gets rotationSpeed property.</summary>
	public float rotationSpeed { get { return _rotationSpeed; } }

	/// <summary>Gets interpolationDuration property.</summary>
	public float interpolationDuration { get { return _interpolationDuration; } }

	/// <summary>Gets buildUpMaxSpeed property.</summary>
	public float buildUpMaxSpeed { get { return _buildUpMaxSpeed; } }

	/// <summary>Gets buildUpMaxSteeringForce property.</summary>
	public float buildUpMaxSteeringForce { get { return _buildUpMaxSteeringForce; } }

	/// <summary>Gets swingMaxSpeed property.</summary>
	public float swingMaxSpeed { get { return _swingMaxSpeed; } }

	/// <summary>Gets swingMaxSteeringForce property.</summary>
	public float swingMaxSteeringForce { get { return _swingMaxSteeringForce; } }

	/// <summary>Gets arrivalRadius property.</summary>
	public float arrivalRadius { get { return _arrivalRadius; } }

	/// <summary>Gets maxSpeedScalar property.</summary>
	public float maxSpeedScalar { get { return _maxSpeedScalar; } }

	/// <summary>Gets additionalYOffset property.</summary>
	public float additionalYOffset { get { return _additionalYOffset; } }

	/// <summary>Gets clampedHeight property.</summary>
	public float clampedHeight { get { return _clampedHeight; } }

	/// <summary>Gets buildUpSoundEffect property.</summary>
	public SoundEffectEmissionData buildUpSoundEffect { get { return _buildUpSoundEffect; } }

	/// <summary>Gets swingSoundEffect property.</summary>
	public SoundEffectEmissionData swingSoundEffect { get { return _swingSoundEffect; } }
#endregion

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
#if UNITY_EDITOR
		base.DrawGizmos();
		
		Gizmos.color = gizmosColor;

		Gizmos.DrawWireSphere(spawnPosition, gizmosRadius);
		Gizmos.DrawWireSphere(entrancePosition, gizmosRadius);

		if(destinoSpawnPointsPairs != null) foreach(Vector3Pair pair in destinoSpawnPointsPairs)
		{
			Gizmos.DrawWireSphere(pair.a, gizmosRadius);
			Gizmos.DrawWireSphere(pair.b, gizmosRadius);
		}

		Gizmos.DrawWireSphere(slashZone.a, gizmosRadius);
		Gizmos.DrawWireSphere(slashZone.b, gizmosRadius);
#endif
	}

	/// <summary>DeathBehavior's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		scythe.transform.position = spawnPosition;
		scythe.gameObject.SetActive(false);
		scythe.animationsEventInvoker.AddIntActionListener(OnAnimationIntEvent);
		scythe.state = AnimationCommandState.None;
	}

	/// <summary>Rotates scythe towards mateo.</summary>
	/// <param name="s">Stage's scalar.</param>
	private void RotateScythe(float s)
	{
		float dX = Game.mateo.transform.position.x - scythe.transform.position.x;
		scythe.transform.rotation = Quaternion.RotateTowards(scythe.transform.rotation, dX > 0.0f ? rightRotation : leftRotation, rotationSpeed * Time.deltaTime * s);
	}

	/// <summary>Callback invoked when an Animation Event is invoked.</summary>
	/// <param name="_ID">Int argument.</param>
	private void OnAnimationIntEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.ANIMATIONEVENT_DEACTIVATEHITBOXES:
			scythe.state = scythe.state == AnimationCommandState.Active ? AnimationCommandState.Recovery : AnimationCommandState.Startup;
			scythe.vehicle.maxSpeed = buildUpMaxSpeed;
			scythe.vehicle.maxForce = buildUpMaxSteeringForce;
			scythe.weapon.ActivateHitBoxes(scythe.state == AnimationCommandState.Recovery);
			break;
			
			case IDs.ANIMATIONEVENT_ACTIVATEHITBOXES:
			scythe.state = AnimationCommandState.Active;
			scythe.vehicle.maxSpeed = swingMaxSpeed;
			scythe.vehicle.maxForce = swingMaxSteeringForce;
			scythe.weapon.ActivateHitBoxes(true);
			swingSoundEffect.Play();
			break;

			case IDs.ANIMATIONEVENT_EMITSOUND_0:
			buildUpSoundEffect.Play();
			break;
		}
	}

#region Coroutines:
	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{
		int stage = boss.currentStage;
		IEnumerator coroutine = null;
		int animationHash = 0;
		bool right = Random.Range(0, 5) <= 2;

		switch(stage)
		{
			case Boss.STAGE_1:
			animationHash = stage1AttackCredential;
			break;

			case Boss.STAGE_2:
			animationHash = stage2AttackCredential;
			break;

			case Boss.STAGE_3:
			animationHash = stage3AttackCredential;
			break;

			default:
			animationHash = stage3AttackCredential;
			break;
		}

		coroutine = LaughingRoutine(boss);
		while(coroutine.MoveNext()) yield return null;

		coroutine = DestinoSceneController.TakeDestinoToInitialPoint();
		while(coroutine.MoveNext()) yield return null;

		//boss.animatorController.Play(boss.idleCredential);
		boss.Sing();
		scythe.state = AnimationCommandState.None;
		scythe.transform.position = spawnPosition;
		scythe.vehicle.ResetVelocity();
		scythe.gameObject.SetActive(true);
		scythe.animatorController.CrossFade(entranceCredential);
		scythe.weapon.ActivateHitBoxes(false);

		Game.AddTargetToCamera(scythe.weapon.cameraTarget);
		coroutine = GoTowards(entrancePosition);
		while(coroutine.MoveNext()) yield return null;

		coroutine = GoTowards(right ? slashZone.b : slashZone.a);
		while(coroutine.MoveNext()) yield return null;

		coroutine = ChasingRoutine(boss, animationHash, right);
		while(coroutine.MoveNext()) yield return null;

		scythe.animatorController.CrossFade(exitCredential);

		coroutine = GoTowards(entrancePosition);
		while(coroutine.MoveNext()) yield return null;

		Game.RemoveTargetToCamera(scythe.weapon.cameraTarget);
		coroutine = GoTowards(spawnPosition);
		while(coroutine.MoveNext()) yield return null;

		InvokeCoroutineEnd();
	}

	/// <summary>Interpolates towards given point.</summary>
	/// <param name="_point">Desired point.</param>
	private IEnumerator GoTowards(Vector3 _point)
	{
		Vector3 originalPosition = scythe.transform.position;
		float t = 0.0f;
		float iD = 1.0f / interpolationDuration;

		while(t < 1.0f)
		{
			scythe.transform.position = Vector3.Lerp(originalPosition, _point, t);
			t += (Time.deltaTime * iD);
			yield return null;
		}

		scythe.transform.position = _point;
	}

	/// <summary>Mateo Chase's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	/// <param name="animationHash">Attack Animation's Hash to use.</param>
	/// <param name="_right">Go to the right extreme? left if false.</param>
	private IEnumerator ChasingRoutine(DestinoBoss boss, int _animationHash, bool _right)
	{
		Vector3 a = _right ? slashZone.b : slashZone.a;
		Vector3 b = _right ? slashZone.a : slashZone.b;
		float t = boss.stageScale;
		float s = Mathf.Lerp(1.0f, maxSpeedScalar, t);
		float x = b.x;
		float y = s * Time.deltaTime;
		bool animationEnded = false;

		scythe.animatorController.CrossFade(_animationHash, 0.3f, 0, 0.0f, 0.0f);

		yield return null;

		AnimatorStateInfo info = scythe.animator.GetCurrentAnimatorStateInfo(0);
		AnimatorTransitionInfo transitionInfo = scythe.animator.GetAnimatorTransitionInfo(0);
		SecondsDelayWait wait = new SecondsDelayWait(transitionInfo.duration * info.length);

		while(wait.MoveNext()) yield return null;

		info = scythe.animator.GetCurrentAnimatorStateInfo(0);
		wait.ChangeDurationAndReset(info.length);

		while(!animationEnded || Mathf.Abs(scythe.transform.position.x - x) > 0.1f)
		{
			animationEnded = !wait.MoveNext();

			Vector3 target = Vector3.zero;

			switch(scythe.state)
			{
				case AnimationCommandState.None:
				case AnimationCommandState.Startup:
				target = a;
				break;

				case AnimationCommandState.Active:
				case AnimationCommandState.Recovery:
				target = b;
				break;
			}

			int anchorIndex = (scythe.state != AnimationCommandState.Active) ? 1 : 0;
			Vector3 anchoredPosition = scythe.weapon.anchorContainer.GetAnchoredPosition(target, anchorIndex);
			anchoredPosition = target;
			Vector3 scythePosition = scythe.transform.position;

			if(scythe.state != AnimationCommandState.Active && !animationEnded) RotateScythe(s);

			scythePosition += (Vector3)scythe.vehicle.GetSeekForce(anchoredPosition) * Time.deltaTime * y;
			scythePosition.y = Mathf.Max(scythePosition.y, clampedHeight);
			scythePosition.x = Mathf.Clamp(scythePosition.x, slashZone.a.x, slashZone.b.x);
			scythe.transform.position = scythePosition;
			
			y += (s * Time.deltaTime);

			yield return null;
		}

		scythe.state = AnimationCommandState.None;
		scythe.weapon.ActivateHitBoxes(false);
	}

	/// <summary>Mateo Chase's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	/// <param name="animationHash">Attack Animation's Hash to use.</param>
	private IEnumerator ChasingRoutine(DestinoBoss boss, int _animationHash)
	{
		float t = boss.stageScale;
		float s = Mathf.Lerp(1.0f, maxSpeedScalar, t);

		scythe.animatorController.CrossFade(_animationHash, 0.3f, 0, 0.0f, 0.0f);

		yield return null;

		AnimatorStateInfo info = scythe.animator.GetCurrentAnimatorStateInfo(0);
		AnimatorTransitionInfo transitionInfo = scythe.animator.GetAnimatorTransitionInfo(0);
		SecondsDelayWait wait = new SecondsDelayWait(transitionInfo.duration * info.length);

		while(wait.MoveNext()) yield return null;

		info = scythe.animator.GetCurrentAnimatorStateInfo(0);
		wait.ChangeDurationAndReset(info.length);


		while(wait.MoveNext())
		{
			int anchorIndex = (scythe.state != AnimationCommandState.Active) ? 1 : 0;
			Vector3 anchoredPosition = scythe.weapon.anchorContainer.GetAnchoredPosition(Game.mateo.transform.position, anchorIndex);
			Vector3 scythePosition = scythe.transform.position;

			if(scythe.state != AnimationCommandState.Active) anchoredPosition.y += additionalYOffset;

			scythePosition += (Vector3)scythe.vehicle.GetSeekForce(anchoredPosition) * Time.deltaTime * s;
			scythePosition.y = Mathf.Max(scythePosition.y, clampedHeight);
			scythe.transform.position = scythePosition;
			RotateScythe(s);
			yield return null;
		}
	}

	/// <summary>Laughing's Routine.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator LaughingRoutine(DestinoBoss boss)
	{
		if(destinoSpawnPointsPairs == null) yield break;

		Vector3Pair pair = destinoSpawnPointsPairs.Random();
		Vector3 up = (pair.b - pair.a).normalized;
		float inverseDuration = 1.0f / entranceLerpDuration;
		float t = 0.0f;
		bool playFinished = false;
		Action onPlayFinished = ()=> { playFinished = true; };

		boss.transform.rotation = Quaternion.LookRotation(Vector3.forward, up);

		while(t < 1.0f)
		{
			boss.transform.position = Vector3.Lerp(pair.a, pair.b, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		boss.transform.position = pair.b;
		boss.animatorController.PlayAndWait(boss.laughCredential, 0, Mathf.NegativeInfinity, 0.0f, onPlayFinished);
		boss.laughSoundEffect.Play();

		while(!playFinished) yield return null;

		inverseDuration = 1.0f / exitLerpDuration;
		t = 0.0f;

		while(t < 1.0f)
		{
			boss.transform.position = Vector3.Lerp(pair.b, pair.a, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		boss.transform.position = pair.a;
	}
#endregion
}
}