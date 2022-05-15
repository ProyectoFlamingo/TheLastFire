using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

namespace Flamingo
{
[RequireComponent(typeof(SteeringVehicle2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoskarBoss : Boss
{
	private const int ID_TAUNT_1 = 1; 														/// <summary>Taunt 1's ID.</summary>
	private const int ID_TAUNT_2 = 2; 														/// <summary>Taunt 1's ID.</summary>
	private const int ID_LOCOMOTION_IDLE = 1; 												/// <summary>Idle Locomotion's ID.</summary>
	private const int ID_LOCOMOTION_WALK = 2; 												/// <summary>Walk Locomotion's ID.</summary>
	private const int ID_LOCOMOTION_FLY = 3; 												/// <summary>Fly Locomotion's ID.</summary>
	private const int ID_LOCOMOTION_LAND = 4; 												/// <summary>Land Locomotion's ID.</summary>
	
	[Space(5f)]
	[SerializeField] private int _phases; 													/// <summary>Moskar's Phases.</summary>
	[Space(5f)]
	[Header("Moskar's Components:")]
	[SerializeField] private FOVSight2D _sightSensor; 										/// <summary>FOVSight2D's Component.</summary>
	[SerializeField] private Transform _tail; 												/// <summary>Moskar's Tail's Transform.</summary>
	[Space(5f)]
	[Header("Attack's Attributes:")]
	[SerializeField] private VAssetReference _projectileReference; 							/// <summary>Projectikle's Asset Reference.</summary>
	[Space(5f)]
	[Header("Rotations' Attributes:")]
	[SerializeField] private EulerRotation _walkingRotation; 								/// <summary>Moskar's Walking Rotation.</summary>
	[SerializeField] private EulerRotation _flyingRotation; 								/// <summary>Moskar's Flying Rotation.</summary>
	[SerializeField] private EulerRotation _fallingRotation; 								/// <summary>Moskar's Rotation when Falling.</summary>
	[SerializeField] private float _rotationSpeed; 											/// <summary>Moskar's rotation speed.</summary>
	[SerializeField] private float _rotationDuration; 										/// <summary>Falling Rotation's Duration.</summary>
	[Space(5f)]
	[Header("Sounds FXs:")]
	[SerializeField] private SoundEffectEmissionData _hurtSoundEffect; 						/// <summary>Hurt Sound-Effect's Emission Data.</summary>
	[SerializeField] private SoundEffectEmissionData _fallenSoundEffect; 					/// <summary>Fallen Sound-Effect's Emission Data.</summary>
	[Space(5f)]
	[Header("Particle Effects' Attributes:")]
	[SerializeField] private ParticleEffectEmissionData _duplicateParticleEffect; 			/// <summary>Duplication Particle-Effect's Emission Data.</summary>
	[Space(5f)]
	[Header("Animator's Attributes:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _introCredential; 	/// <summary>Intro's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _deadCredential; 	/// <summary>Dead's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _idleCredential; 	/// <summary>Idle's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _walkCredential; 	/// <summary>Walk's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _flyCredential; 	/// <summary>Fly's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _landCredential; 	/// <summary>Land's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _poopCredential; 	/// <summary>Poop's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _taunt1Credential; 	/// <summary>Taunt 1's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _taunt2Credential; 	/// <summary>Taunt 2's Animator Credential.</summary>
	[Space(5f)]
	[TabGroup("Animations")][SerializeField] private int _introAnimationLayer; 				/// <summary>Intro's Animation Layer.</summary>
	[Space(5f)]
	[SerializeField] private TrailRenderer _tackleTrailRenderer; 							/// <summary>Tackle's Trail Renderer.</summary>
	private SteeringVehicle2D _vehicle; 													/// <summary>SteeringVehicle2D's Component.</summary>
	public Coroutine attackCoroutine; 														/// <summary>Attack's Coroutine.</summary>
	public Coroutine rotationCoroutine; 													/// <summary>Rotation's Coroutine.</summary>
	private int _currentPhase; 																/// <summary>Moskar's Current Phase. It is base 0, meaning it goes from 0 to [phases - 1]</summary>
	private float _phaseProgress; 															/// <summary>Phase's Normalized Progress.</summary>

#region Getters/Setters:
	/// <summary>Gets phases property.</summary>
	public int phases { get { return _phases; } }

	/// <summary>Gets and Sets currentPhase property.</summary>
	public int currentPhase
	{
		get { return _currentPhase; }
		set { _currentPhase = value; }
	}

	/// <summary>Gets introAnimationLayer property.</summary>
	public int introAnimationLayer { get { return _introAnimationLayer; } }

	/// <summary>Gets and Sets rotationSpeed property.</summary>
	public float rotationSpeed
	{
		get { return _rotationSpeed; }
		set { _rotationSpeed = value; }
	}

	/// <summary>Gets and Sets rotationDuration property.</summary>
	public float rotationDuration
	{
		get { return _rotationDuration; }
		set { _rotationDuration = value; }
	}

	/// <summary>Gets and Sets phaseProgress property.</summary>
	public float phaseProgress
	{
		get { return _phaseProgress; }
		set { _phaseProgress = value; }
	}

	/// <summary>Gets projectileReference property.</summary>
	public VAssetReference projectileReference { get { return _projectileReference; } }

	/// <summary>Gets walkingRotation property.</summary>
	public EulerRotation walkingRotation { get { return _walkingRotation; } }

	/// <summary>Gets flyingRotation property.</summary>
	public EulerRotation flyingRotation { get { return _flyingRotation; } }

	/// <summary>Gets fallingRotation property.</summary>
	public EulerRotation fallingRotation { get { return _fallingRotation; } }

	/// <summary>Gets hurtSoundEffect property.</summary>
	public SoundEffectEmissionData hurtSoundEffect { get { return _hurtSoundEffect; } }

	/// <summary>Gets fallenSoundEffect property.</summary>
	public SoundEffectEmissionData fallenSoundEffect { get { return _fallenSoundEffect; } }

	/// <summary>Gets duplicateParticleEffect property.</summary>
	public ParticleEffectEmissionData duplicateParticleEffect { get { return _duplicateParticleEffect; } }

	/// <summary>Gets introCredential property.</summary>
	public AnimatorCredential introCredential { get { return _introCredential; } }

	/// <summary>Gets deadCredential property.</summary>
	public AnimatorCredential deadCredential { get { return _deadCredential; } }

	/// <summary>Gets idleCredential property.</summary>
	public AnimatorCredential idleCredential { get { return _idleCredential; } }

	/// <summary>Gets walkCredential property.</summary>
	public AnimatorCredential walkCredential { get { return _walkCredential; } }

	/// <summary>Gets flyCredential property.</summary>
	public AnimatorCredential flyCredential { get { return _flyCredential; } }

	/// <summary>Gets landCredential property.</summary>
	public AnimatorCredential landCredential { get { return _landCredential; } }

	/// <summary>Gets poopCredential property.</summary>
	public AnimatorCredential poopCredential { get { return _poopCredential; } }

	/// <summary>Gets taunt1Credential property.</summary>
	public AnimatorCredential taunt1Credential { get { return _taunt1Credential; } }

	/// <summary>Gets taunt2Credential property.</summary>
	public AnimatorCredential taunt2Credential { get { return _taunt2Credential; } }

	/// <summary>Gets sightSensor property.</summary>
	public FOVSight2D sightSensor { get { return _sightSensor; } }

	/// <summary>Gets tail property.</summary>
	public Transform tail { get { return _tail; } }

	/// <summary>Gets tackleTrailRenderer property.</summary>
	public TrailRenderer tackleTrailRenderer { get { return _tackleTrailRenderer; } }

	/// <summary>Gets vehicle Component.</summary>
	public SteeringVehicle2D vehicle
	{ 
		get
		{
			if(_vehicle == null) _vehicle = GetComponent<SteeringVehicle2D>();
			return _vehicle;
		}
	}
#endregion

	/// <summary>MoskarBoss's instance initialization.</summary>
	protected override void Awake()
	{
		base.Awake();

		this.ChangeState(IDs.STATE_ALIVE);

		animator.SetAllLayersWeight(0.0f);

		sightSensor.onSightEvent += OnSightEvent;
		eventsHandler.onTriggerEvent += OnTriggerEvent;

		sightSensor.enabled = false;

		coroutinesMap.Add(IDs.COROUTINE_ATTACK, null);
		coroutinesMap.Add(IDs.COROUTINE_ROTATION, null);

		tackleTrailRenderer.enabled = false;
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	protected override void Start()
	{
		base.Start();
	}

#region Methods:
	/// <summary>Simulates Rigidbody and resets its Velocity.</summary>
	public void SimulateInteractionsAndResetVelocity()
	{
		rigidbody.simulated = true;
		rigidbody.Sleep();
	}

	/// <summary>Shoots poop towards direction.</summary>
	/// <param name="_direction">Shooting Direction.</param>
	/// <returns>Poop that was shot.</returns>
	public Projectile ShootPoop(Vector3 _direction)
	{
		this.AddStates(IDs.STATE_ATTACKING_0);
		animator.SetLayerWeight(attackAnimationLayer, 1.0f);
		animatorController.CancelCrossFading(attackAnimationLayer);
		animatorController.CrossFadeAndWait(poopCredential, clipFadeDuration, attackAnimationLayer, Mathf.NegativeInfinity, 0.0f,
		()=>
		{
			animatorController.Play(VAnimator.CREDENTIAL_EMPTY, attackAnimationLayer);
			animator.SetLayerWeight(attackAnimationLayer, 0.0f);
			this.RemoveStates(IDs.STATE_ATTACKING_0);
		});

		return PoolManager.RequestProjectile(Faction.Enemy, projectileReference, tail.transform.position, _direction);
	}
#endregion

#region Callbacks:
	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		switch(_event)
		{
			case HealthEvent.Depleted:
			AudioController.PlayOneShot(SourceType.SFX, hurtSoundEffect.sourceIndex, ResourcesManager.GetAudioClip(hurtSoundEffect.soundReference, SourceType.SFX));
			break;

			case HealthEvent.FullyDepleted:
			AudioController.PlayOneShot(SourceType.SFX, hurtSoundEffect.sourceIndex, ResourcesManager.GetAudioClip(hurtSoundEffect.soundReference, SourceType.SFX));
			BeginDeathRoutine();
			base.OnDeathRoutineEnds();
			this.RemoveStates(IDs.STATE_ALIVE);
			this.ChangeState(0);
			this.DispatchCoroutine(ref behaviorCoroutine);
			this.DispatchCoroutine(ref attackCoroutine);
			this.DispatchCoroutine(ref rotationCoroutine);
			break;
		}
	}

	/// <summary>Callback invoked after the Death's routine ends.</summary>
	protected override void OnDeathRoutineEnds()
	{
		OnObjectDeactivation();
		eventsHandler.InvokeIDEvent(IDs.EVENT_DEATHROUTINE_ENDS);
	}

	/// <summary>Callback invoked when this FOV Sight leaves another collider.</summary>
	/// <param name="_collider">Collider sighted.</param>
	/// <param name="_eventType">Type of interaction.</param>
	protected virtual void OnSightEvent(Collider2D _collider, HitColliderEventTypes _eventType)
	{
		GameObject obj = _collider.gameObject;

		if(obj.CompareTag(Game.data.playerTag))
		{
			switch(_eventType)
			{
				case HitColliderEventTypes.Enter:
					this.AddStates(IDs.STATE_TARGETONSIGHT);
				break;

				case HitColliderEventTypes.Exit:
					this.RemoveStates(IDs.STATE_TARGETONSIGHT);
				break;
			}
		}
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		this.ChangeState(IDs.STATE_ALIVE);
		EnableHurtBoxes(true);
		Game.AddTargetToCamera(cameraTarget);
		rigidbody.gravityScale = 0.0f;
		rigidbody.bodyType = RigidbodyType2D.Kinematic;
		animator.SetAllLayersWeight(0.0f);
		currentPhase = 0;
	}

	/// <summary>Callback invoked when the object is deactivated.</summary>
	public override void OnObjectDeactivation()
	{
		base.OnObjectDeactivation();
		Game.RemoveTargetToCamera(cameraTarget);
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		if(_eventType != HitColliderEventTypes.Enter) return;

		GameObject obj = _info.collider.gameObject;

		switch(this.HasStates(IDs.STATE_ALIVE))
		{
			case true:
				if(health.invincibilityCooldown.onCooldown) return;

				if(obj.CompareTag(Game.data.playerTag))
				{
					Health health = obj.GetComponentInParent<Health>();

					if(health == null)
					{
						HealthLinker linker = obj.GetComponent<HealthLinker>();
						if(linker != null) health = linker.component;
					}

					if(health != null)
					{
						health.GiveDamage(1.0f);
					}
				}
			break;

			case false:
				Debug.Log("[MoskarBoss] Dead, intersecting with " + obj.tag);
				if(obj.CompareTag(Game.data.floorTag))
				{
					Debug.DrawRay(transform.position, Vector3.up * 10.0f, Color.magenta, 10.0f);
					Debug.DrawRay(transform.position, Vector3.back * 10.0f, Color.magenta, 10.0f);
					OnDeathRoutineEnds();
				}
			break;
		}
	}
#endregion

	/// <summary>Death's Routine.</summary>
	/// <param name="onDeathRoutineEnds">Callback invoked when the routine ends.</param>
	protected override IEnumerator DeathRoutine(Action onDeathRoutineEnds)
	{
		EnableHurtBoxes(false);

		this.StartCoroutine(meshParent.PivotToRotation(walkingRotation, rotationDuration, TransformRelativeness.Local), ref rotationCoroutine);

		animator.SetAllLayersWeight(0.0f);
		animatorController.CrossFade(deadCredential, clipFadeDuration);

		if(currentPhase < (phases - 1) && onDeathRoutineEnds != null)
		{
			onDeathRoutineEnds();
			yield break;
		}

		float t = 0.0f;
		float inverseDuration = 1.0f / rotationDuration;
		Quaternion originalRotation = transform.rotation;
		Quaternion rotation = fallingRotation;
		rigidbody.bodyType = RigidbodyType2D.Dynamic;
		rigidbody.gravityScale = 1.0f;

		AudioController.PlayOneShot(SourceType.SFX, fallenSoundEffect.sourceIndex, ResourcesManager.GetAudioClip(fallenSoundEffect.soundReference, SourceType.SFX));

		while(t < 1.0f)
		{
			transform.rotation = Quaternion.Lerp(originalRotation, rotation, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		transform.rotation = rotation;
	}

}
}