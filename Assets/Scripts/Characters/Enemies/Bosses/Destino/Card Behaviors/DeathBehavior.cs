using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class DeathBehavior : DestinoScriptableCoroutine
{
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
	[Space(5f)]
	[Header("Sound Effects::")]
	[SerializeField] private int _buildUpSoundIndex; 													/// <summary>Build-Up's Sound's Index.</summary>
	[SerializeField] private int _swingSoundIndex; 														/// <summary>Swing's Sound's Index.</summary>
	[SerializeField] private int _sourceIndex; 															/// <summary>Sound Effects' Source Index.</summary>
	private AnimationEventInvoker _animationsEventInvoker; 												/// <summary>AnimationsEventInvoker's Component.</summary>
	private Coroutine scytheRotation; 																	/// <summary>Scythe's Rotation Coroutine Reference.</summary>

#region Getters/Setters:
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

	/// <summary>Gets buildUpSoundIndex property.</summary>
	public int buildUpSoundIndex { get { return _buildUpSoundIndex; } }

	/// <summary>Gets swingSoundIndex property.</summary>
	public int swingSoundIndex { get { return _swingSoundIndex; } }

	/// <summary>Gets sourceIndex property.</summary>
	public int sourceIndex { get { return _sourceIndex; } }
#endregion

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
#if UNITY_EDITOR
		base.DrawGizmos();
		
		Gizmos.color = gizmosColor;

		Gizmos.DrawWireSphere(spawnPosition, gizmosRadius);
		Gizmos.DrawWireSphere(entrancePosition, gizmosRadius);
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
			scythe.weapon.ActivateHitBoxes(false);
			break;
			
			case IDs.ANIMATIONEVENT_ACTIVATEHITBOXES:
			scythe.state = AnimationCommandState.Active;
			scythe.vehicle.maxSpeed = swingMaxSpeed;
			scythe.vehicle.maxForce = swingMaxSteeringForce;
			scythe.weapon.ActivateHitBoxes(true);
			AudioController.PlayOneShot(SourceType.Scenario, sourceIndex, swingSoundIndex);
			break;

			case IDs.ANIMATIONEVENT_EMITSOUND_0:
			AudioController.PlayOneShot(SourceType.Scenario, sourceIndex, buildUpSoundIndex);
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

		scythe.state = AnimationCommandState.None;
		scythe.transform.position = spawnPosition;
		scythe.vehicle.ResetVelocity();
		scythe.gameObject.SetActive(true);
		scythe.animatorController.CrossFade(entranceCredential);

		Game.AddTargetToCamera(scythe.weapon.cameraTarget);
		coroutine = GoTowards(entrancePosition);
		while(coroutine.MoveNext()) yield return null;

		coroutine = ChasingRoutine(boss, animationHash);
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
			Vector3 anchoredPosition = scythe.weapon.anchorContainer.GetAnchoredPosition(Game.mateo.transform.position, 1);

			if(scythe.state != AnimationCommandState.Active) anchoredPosition.y += additionalYOffset;

			scythe.transform.position += (Vector3)scythe.vehicle.GetSeekForce(anchoredPosition) * Time.deltaTime * s;
			RotateScythe(s);
			yield return null;
		}
	}
#endregion
}
}