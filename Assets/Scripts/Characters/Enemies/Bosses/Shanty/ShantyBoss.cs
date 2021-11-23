using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

using Random = UnityEngine.Random;

namespace Flamingo
{
[RequireComponent(typeof(Skeleton))]
[RequireComponent(typeof(JumpAbility))]
[RequireComponent(typeof(DashAbility))]
[RequireComponent(typeof(RigidbodyMovementAbility))]
public class ShantyBoss : Boss
{
	public const int ID_WAYPOINTSPAIR_HELM = 0; 						/// <summary>Helm's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_DECK = 1; 						/// <summary>Deck's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_STAIR_LEFT = 2; 					/// <summary>Left Stair's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_STAIR_RIGHT = 3; 					/// <summary>Right Stair's Waypoints' Pair ID.</summary>
	public const int ID_ANIMATIONEVENT_PICKBOMB = 0; 					/// <summary>Pick Bomb's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_THROWBOMB = 1; 					/// <summary>Throw Bomb's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_SWORD_UNSHEATH = 2; 				/// <summary>Sword Un-Sheath's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_SWORD_SHEATH = 3; 				/// <summary>Sword Sheath's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_GOIDLE = 4; 						/// <summary>Go Idle's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_PICKTNT = 5; 					/// <summary>Pick TNT's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_THROWTNT = 6; 					/// <summary>Throw TNT's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_REPELBOMB = 7; 					/// <summary>Repel Bomb's Animation Event's ID.</summary>
	public const int ID_ANIMATIONEVENT_JUMP = 8; 						/// <summary>Jump's Animation Event's ID.</summary>
	public const int ID_ANIMATIONSTATE_INTRO = 0; 						/// <summary>Intro's State ID [for AnimatorController].</summary>
	public const int ID_ANIMATIONSTATE_TIED = 1; 						/// <summary>Intro's State ID [for AnimatorController].</summary>
	public const int ID_ANIMATIONSTATE_IDLE = 2; 						/// <summary>Idle's State ID [for AnimatorController].</summary>
	public const int ID_ANIMATIONSTATE_ATTACK = 3; 						/// <summary>Attack's State ID [for AnimatorController].</summary>
	public const int ID_ANIMATIONSTATE_DAMAGE = 4; 						/// <summary>Damage's State ID [for AnimatorController].</summary>
	public const int ID_ATTACK_SHOOT_AIR = 0; 							/// <summary>Shoot's State ID [for AnimatorController].</summary>
	public const int ID_ATTACL_HIT_TENNIS = 1; 							/// <summary>Tennis Hit's State ID [for AnimatorController].</summary>
	public const int ID_ATTACK_BOMB_THROW = 2; 							/// <summary>Bomb Throw's State ID [for AnimatorController].</summary>
	public const int ID_ATTACK_BARREL_THROW = 3; 						/// <summary>Barrel Throw's State ID [for AnimatorController].</summary>
	public const int ID_DAMAGE_BOMB = 0; 								/// <summary>Bomb Damage's State ID [for AnimatorController].</summary>
	public const int ID_DAMAGE_SWORD = 1; 								/// <summary>Sword Damage's State ID [for AnimatorController].</summary>
	public const int ID_DAMAGE_CRY = 2; 								/// <summary>Cry's State ID [for AnimatorController].</summary>
	public const int ID_DAMAGE_BARREL = 3; 								/// <summary>Barrel Damage's State ID [for AnimatorController].</summary>
	public const int ID_IDLE = 0; 										/// <summary>Intro's State ID [for AnimatorController].</summary>
	public const int ID_LAUGH = 1; 										/// <summary>Intro's State ID [for AnimatorController].</summary>
	public const int ID_TAUNT = 2; 										/// <summary>Intro's State ID [for AnimatorController].</summary>

	[Space(10f)]
	[Header("Shanty's Attributes:")]
	[Space(5f)]
	[SerializeField] private ShantyShip _ship; 							/// <summary>Shanty's Ship.</summary>
	[Space(5f)]
	[Header("Weapons' Atrributes:")]
	[SerializeField] private ContactWeapon _sword; 						/// <summary>Shanty's Sword.</summary>
	[SerializeField] private Transform _falseSword; 					/// <summary>False Sword's Reference (the one stuck to the rigging).</summary>
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _windowPercentage; 				/// <summary>Time-window before swinging sword (to hit bomb).</summary>
	[Space(5f)]
	[Header("Stage 1 Bomb's Attributes:")]
	[SerializeField] private CollectionIndex _bombIndex; 				/// <summary>Bomb's Index.</summary>
	[SerializeField] private float _bombProjectionTime; 				/// <summary>Bomb's Projection Time.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _bombProjectionPercentage; 		/// <summary>Bomb Projection Time's Percentage.</summary>
	[Space(5f)]
	[Header("Stage 2 Bomb's Attributes:")]
	[SerializeField] private CollectionIndex _bouncingBombIndex; 		/// <summary>Bouncing Bomb's Index.</summary>
	[SerializeField] private float _bouncingBombProjectionTime; 		/// <summary>Bouncing Bomb's Projection Time.</summary>
	[Space(5f)]
	[Header("Stage 1 TNT's Attributes:")]
	[SerializeField] private CollectionIndex _TNTIndex; 				/// <summary>TNT's Index.</summary>
	[SerializeField] private CollectionIndex _stage1ExplodableIndex; 	/// <summary>Explodable;s Index for TNT on Stage 1.</summary>
	[SerializeField] private float _stage1TNTFuseDuration; 				/// <summary>Fuse Duration for TNT on Stage 1.</summary>
	[SerializeField] private float _TNTProjectionTime; 					/// <summary>TNT's Projection Time.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _TNTProjectionPercentage; 		/// <summary>TNT Projection Time's Percentage.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _TNTTimeScaleChangeProgress; 		/// <summary>TNT parabolas' progress necessary to slow down time.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _TNTThrowTimeScale; 				/// <summary>Time Scale when throwing TNT.</summary>
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _stage1HealthPercentageLimit; 	/// <summary>Health Limit's Percentage for TNT.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _stage2HealthPercentageLimit; 	/// <summary>Health Limit's Percentage for TNT.</summary>
	[Space(5f)]
	[Header("Stage 2's TNT's Attributes:")]
	[SerializeField] private CollectionIndex _stage2ExplodableIndex; 	/// <summary>Explodable;s Index for TNT on Stage 2.</summary>
	[SerializeField] private float _stage2TNTFuseDuration; 				/// <summary>Fuse Duration for TNT on Stage 2.</summary>
	[SerializeField] private float _stairParabolaTime; 					/// <summary>Duration from throw to beginning of stair.</summary>
	[SerializeField] private float _stairSlideDuration; 				/// <summary>Stair Slide's Duration.</summary>
	[SerializeField] private float _sidewaysMovementSpeed; 				/// <summary>Sideways' Movement Speed.</summary>
	[SerializeField] private float _TNTRotationSpeed; 					/// <summary>TNT's Rotation Angular Speed.</summary>
	[Space(5f)]
	[Header("Whack-A-Mole's Attributes:")]
	[SerializeField] private float _vectorPairInterpolationDuration; 	/// <summary>Interpolation duration for Whack-A-Mole's Waypoints.</summary>
	[SerializeField] private float _waitBeforeWaypointReturn; 			/// <summary>Wait before Waypoint's Return.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _progressToToggleHurtBoxes; 		/// <summary>Process percentage on the interpolation to toggle the Hurt-Boxes.</summary>
	[Space(5f)]
	[Header("Duel's Attributes:")]
	[SerializeField] private FloatRange _attackRadiusRange; 			/// <summary>Attacks' Radius Range.</summary>
	[SerializeField] private FloatRange _strongAttackWaitInterval; 		/// <summary>Strong Attack's Wait Interval.</summary>
	[SerializeField] private float _movementSpeed; 						/// <summary>Movement's Speed.</summary>
	[SerializeField] private float _rotationSpeed; 						/// <summary>Rotation's Speed.</summary>
	[SerializeField] private float _regressionDuration; 				/// <summary>Regression's Duration.</summary>
	[SerializeField] private float _attackDistance; 					/// <summary>Attack's Distance.</summary>
	[SerializeField] private float _normalAttackCooldownDuration; 		/// <summary>Normal Attack Cooldown's Duration.</summary>
	[SerializeField] private float _strongAttackCooldownDuration; 		/// <summary>Strong Attack Cooldown's Duration.</summary>
	[Space(5f)]
	[Header("Inmunities:")]
	[SerializeField] private GameObjectTag[] _stage1Inmunities; 		/// <summary>Inmunities on Stage 1.</summary>
	[SerializeField] private GameObjectTag[] _stage2Inmunities; 		/// <summary>Inmunities on Stage 2.</summary>
	[Space(5f)]
	[Header("Animator's Attributes:")]
	[SerializeField] private AnimatorCredential _stateIDCredential; 	/// <summary>State ID's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _attackIDCredential; 	/// <summary>Attack ID's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _idleIDCredential; 		/// <summary>Idle ID's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _vitalityIDCredential; 	/// <summary>Vitality ID's AnimatorCredential.</summary>
	[Space(5f)]
	[Header("Animations:")]
	[SerializeField] private AnimationClip[] tiedAnimations; 			/// <summary>Tied Animations.</summary>
	[SerializeField] private AnimationClip untiedAnnimation; 			/// <summary>Untied's Animation.</summary>
	[SerializeField] private AnimationClip idleAnimation; 				/// <summary>Idle's Animation.</summary>
	[SerializeField] private AnimationClip laughAnimation; 				/// <summary>Laugh's Animation.</summary>
	[SerializeField] private AnimationClip tauntAnimation; 				/// <summary>Taun's Animation.</summary>
	[SerializeField] private AnimationClip tiredAnimation; 				/// <summary>Tired's Animation.</summary>
	[SerializeField] private AnimationClip shootAnimation; 				/// <summary>Shoot's Animation.</summary>
	[SerializeField] private AnimationClip throwBarrelAnimation; 		/// <summary>Throw Barrel's Animation.</summary>
	[SerializeField] private AnimationClip throwBombAnimation; 			/// <summary>Throw Bomb's Animation.</summary>
	[SerializeField] private AnimationClip tenninsHitAnimation; 		/// <summary>tennis Hit's Animation.</summary>
	[SerializeField] private AnimationClip hitBombAnimation; 			/// <summary>Hit Bomb's Animation.</summary>
	[SerializeField] private AnimationClip hitBarrelAnimation; 			/// <summary>Hit Barrel's Animation.</summary>
	[SerializeField] private AnimationClip hitSwordAnimation; 			/// <summary>Hit Sword's Animation.</summary>
	[SerializeField] private AnimationClip cryAnimation; 				/// <summary>Cry's Animation.</summary>
	[SerializeField] private AnimationClip normalAttackAnimation; 		/// <summary>Normal Attack's Animation.</summary>
	[SerializeField] private AnimationClip strongAttackAnimation; 		/// <summary>Strong Attack's Animation.</summary>
	[SerializeField] private AnimationClip backStepAnimation; 			/// <summary>Back-Setp Animation.</summary>
	private Skeleton _skeleton; 										/// <summary>Skeleton's Component.</summary>
	private Coroutine coroutine; 										/// <summary>Coroutine's Reference.</summary>
	private Coroutine TNTRotationCoroutine; 							/// <summary>TNT's Rotation Coroutine's Reference.</summary>
	private Behavior attackBehavior; 									/// <summary>Attack's Behavior [it is behavior so it can be paused].</summary>
	private Projectile _bomb; 											/// <summary>Bomb's Reference.</summary>
	private Projectile _TNT; 											/// <summary>TNT's Reference.</summary>
	private JumpAbility _jumpAbility; 									/// <summary>JumpAbility's Component.</summary>
	private DashAbility _dashAbility; 									/// <summary>DashAbility's Component.</summary>
	private RigidbodyMovementAbility _movementAbility; 					/// <summary>MovementAbility's Component.</summary>
	private Cooldown _normalAttackCooldown; 							/// <summary>Normal Attack's Cooldown.</summary>
	private Cooldown _strongAttackCooldown; 							/// <summary>Strong Attack's Cooldown.</summary>
	private Line _line; 												/// <summary>Current Stair's Line.</summary>
	private bool _tntActive; 											/// <summary>Is the TNT Coroutine's Running?.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ship property.</summary>
	public ShantyShip ship
	{
		get { return _ship; }
		set { _ship = value; }
	}

	/// <summary>Gets bombIndex property.</summary>
	public CollectionIndex bombIndex { get { return _bombIndex; } }

	/// <summary>Gets bouncingBombIndex property.</summary>
	public CollectionIndex bouncingBombIndex { get { return _bouncingBombIndex; } }

	/// <summary>Gets TNTIndex property.</summary>
	public CollectionIndex TNTIndex { get { return _TNTIndex; } }

	/// <summary>Gets stage1ExplodableIndex property.</summary>
	public CollectionIndex stage1ExplodableIndex { get { return _stage1ExplodableIndex; } }

	/// <summary>Gets stage2ExplodableIndex property.</summary>
	public CollectionIndex stage2ExplodableIndex { get { return _stage2ExplodableIndex; } }

	/// <summary>Gets bombProjectionTime property.</summary>
	public float bombProjectionTime { get { return _bombProjectionTime; } }

	/// <summary>Gets bombProjectionPercentage property.</summary>
	public float bombProjectionPercentage { get { return _bombProjectionPercentage; } }

	/// <summary>Gets bouncingBombProjectionTime property.</summary>
	public float bouncingBombProjectionTime { get { return _bouncingBombProjectionTime; } }

	/// <summary>Gets stage1TNTFuseDuration property.</summary>
	public float stage1TNTFuseDuration { get { return _stage1TNTFuseDuration; } }

	/// <summary>Gets TNTProjectionTime property.</summary>
	public float TNTProjectionTime { get { return _TNTProjectionTime; } }

	/// <summary>Gets TNTProjectionPercentage property.</summary>
	public float TNTProjectionPercentage { get { return _TNTProjectionPercentage; } }

	/// <summary>Gets TNTTimeScaleChangeProgress property.</summary>
	public float TNTTimeScaleChangeProgress { get { return _TNTTimeScaleChangeProgress; } }

	/// <summary>Gets TNTThrowTimeScale property.</summary>
	public float TNTThrowTimeScale { get { return _TNTThrowTimeScale; } }

	/// <summary>Gets windowPercentage property.</summary>
	public float windowPercentage { get { return _windowPercentage; } }

	/// <summary>Gets stage1HealthPercentageLimit property.</summary>
	public float stage1HealthPercentageLimit { get { return _stage1HealthPercentageLimit; } }

	/// <summary>Gets stage2HealthPercentageLimit property.</summary>
	public float stage2HealthPercentageLimit { get { return _stage2HealthPercentageLimit; } }

	/// <summary>Gets stage2TNTFuseDuration property.</summary>
	public float stage2TNTFuseDuration { get { return _stage2TNTFuseDuration; } }

	/// <summary>Gets stairParabolaTime property.</summary>
	public float stairParabolaTime { get { return _stairParabolaTime; } }

	/// <summary>Gets stairSlideDuration property.</summary>
	public float stairSlideDuration { get { return _stairSlideDuration; } }

	/// <summary>Gets sidewaysMovementSpeed property.</summary>
	public float sidewaysMovementSpeed { get { return _sidewaysMovementSpeed; } }

	/// <summary>Gets TNTRotationSpeed property.</summary>
	public float TNTRotationSpeed { get { return _TNTRotationSpeed; } }

	/// <summary>Gets vectorPairInterpolationDuration property.</summary>
	public float vectorPairInterpolationDuration { get { return _vectorPairInterpolationDuration; } }

	/// <summary>Gets waitBeforeWaypointReturn property.</summary>
	public float waitBeforeWaypointReturn { get { return _waitBeforeWaypointReturn; } }

	/// <summary>Gets progressToToggleHurtBoxes property.</summary>
	public float progressToToggleHurtBoxes { get { return _progressToToggleHurtBoxes; } }

	/// <summary>Gets movementSpeed property.</summary>
	public float movementSpeed { get { return _movementSpeed; } }

	/// <summary>Gets regressionDuration property.</summary>
	public float regressionDuration { get { return _regressionDuration; } }

	/// <summary>Gets rotationSpeed property.</summary>
	public float rotationSpeed { get { return _rotationSpeed; } }

	/// <summary>Gets attackDistance property.</summary>
	public float attackDistance { get { return _attackDistance; } }

	/// <summary>Gets normalAttackCooldownDuration property.</summary>
	public float normalAttackCooldownDuration { get { return _normalAttackCooldownDuration; } }

	/// <summary>Gets strongAttackCooldownDuration property.</summary>
	public float strongAttackCooldownDuration { get { return _strongAttackCooldownDuration; } }

	/// <summary>Gets attackRadiusRange property.</summary>
	public FloatRange attackRadiusRange { get { return _attackRadiusRange; } }

	/// <summary>Gets strongAttackWaitInterval property.</summary>
	public FloatRange strongAttackWaitInterval { get { return _strongAttackWaitInterval; } }

	/// <summary>Gets sword property.</summary>
	public ContactWeapon sword { get { return _sword; } }

	/// <summary>Gets falseSword property.</summary>
	public Transform falseSword { get { return _falseSword; } }

	/// <summary>Gets stage1Inmunities property.</summary>
	public GameObjectTag[] stage1Inmunities { get { return _stage1Inmunities; } }

	/// <summary>Gets stage2Inmunities property.</summary>
	public GameObjectTag[] stage2Inmunities { get { return _stage2Inmunities; } }

	/// <summary>Gets stateIDCredential property.</summary>
	public AnimatorCredential stateIDCredential { get { return _stateIDCredential; } }

	/// <summary>Gets attackIDCredential property.</summary>
	public AnimatorCredential attackIDCredential { get { return _attackIDCredential; } }

	/// <summary>Gets idleIDCredential property.</summary>
	public AnimatorCredential idleIDCredential { get { return _idleIDCredential; } }

	/// <summary>Gets vitalityIDCredential property.</summary>
	public AnimatorCredential vitalityIDCredential { get { return _vitalityIDCredential; } }

	/// <summary>Gets skeleton Component.</summary>
	public Skeleton skeleton
	{ 
		get
		{
			if(_skeleton == null) _skeleton = GetComponent<Skeleton>();
			return _skeleton;
		}
	}

	/// <summary>Gets and Sets bomb property.</summary>
	public Projectile bomb
	{
		get { return _bomb; }
		private set { _bomb = value; }
	}

	/// <summary>Gets and Sets TNT property.</summary>
	public Projectile TNT
	{
		get { return _TNT; }
		private set { _TNT = value; }
	}

	/// <summary>Gets jumpAbility Component.</summary>
	public JumpAbility jumpAbility
	{ 
		get
		{
			if(_jumpAbility == null) _jumpAbility = GetComponent<JumpAbility>();
			return _jumpAbility;
		}
	}

	/// <summary>Gets dashAbility Component.</summary>
	public DashAbility dashAbility
	{ 
		get
		{
			if(_dashAbility == null) _dashAbility = GetComponent<DashAbility>();
			return _dashAbility;
		}
	}

	/// <summary>Gets movementAbility Component.</summary>
	public RigidbodyMovementAbility movementAbility
	{ 
		get
		{
			if(_movementAbility == null) _movementAbility = GetComponent<RigidbodyMovementAbility>();
			return _movementAbility;
		}
	}

	/// <summary>Gets and Sets normalAttackCooldown property.</summary>
	public Cooldown normalAttackCooldown
	{
		get { return _normalAttackCooldown; }
		private set { _normalAttackCooldown = value; }
	}

	/// <summary>Gets and Sets strongAttackCooldown property.</summary>
	public Cooldown strongAttackCooldown
	{
		get { return _strongAttackCooldown; }
		private set { _strongAttackCooldown = value; }
	}

	/// <summary>Gets and Sets line property.</summary>
	public Line line
	{
		get { return _line; }
		private set { _line = value; }
	}

	/// <summary>Gets and Sets tntActive property.</summary>
	public bool tntActive
	{
		get { return _tntActive; }
		private set { _tntActive = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos [On Editor Mode].</summary>
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		Gizmos.DrawWireSphere(transform.position, attackDistance);
		Gizmos.DrawWireSphere(transform.position, attackRadiusRange.Min());
		Gizmos.DrawWireSphere(transform.position, attackRadiusRange.Max());
	}
#endif

	/// <summary>ShantyBoss's instance initialization.</summary>
	protected override void Awake()
	{
		ActivateSword(false);

		if(tiedAnimations != null) foreach(AnimationClip clip in tiedAnimations)
		{
			animation.AddClip(clip);
		}

		animation.AddClips(
			untiedAnnimation,
			idleAnimation,
			laughAnimation,
			tauntAnimation,
			tiredAnimation,
			shootAnimation,
			throwBarrelAnimation,
			throwBombAnimation,
			tenninsHitAnimation,
			hitBombAnimation,
			hitBarrelAnimation,
			hitSwordAnimation,
			cryAnimation,
			normalAttackAnimation,
			strongAttackAnimation,
			backStepAnimation
		);

		normalAttackCooldown = new Cooldown(this, normalAttackCooldownDuration);
		strongAttackCooldown = new Cooldown(this, strongAttackCooldownDuration);

		base.Awake();
	}

	/// <summary>ShantyBoss's starting actions before 1st Update frame.</summary>
	protected override void Start ()
	{
		base.Start();
	}

#region Methods:
	/// <summary>Moves towards direction.</summary>
	/// <param name="direction">Direction.</param>
	/// <param name="scale">Optional movement scalar [1.0f by default].</param>
	private void Move(Vector3 direction, float scale = 1.0f)
	{
		animation.CrossFade(backStepAnimation);
		movementAbility.Move(direction, scale);
	}

	/// <summary>Performs Jumps.</summary>
	private void Jump()
	{
		this.StartCoroutine(JumpRoutine());
	}

	/// <summary>Activates/Deactivates Sword and False Sword.</summary>
	/// <param name="_activate">Activate Sword? true by default.</param>
	private void ActivateSword(bool _activate = true)
	{
		sword.owner = gameObject;
		sword.ActivateHitBoxes(_activate);
		
		if(_activate)
		{
			sword.transform.position = skeleton.rightHand.position;
			sword.transform.rotation = skeleton.rightHand.rotation;
			sword.transform.parent = skeleton.rightHand;
		}

		sword.gameObject.SetActive(_activate);
		falseSword.gameObject.SetActive(!_activate);
	}

	/// <summary>Enables Physics.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public override void EnablePhysics(bool _enable = true)
	{
		base.EnablePhysics(_enable);
		jumpAbility.gravityApplier.useGravity = _enable;
	}

	/// <summary>Begins Attack's Routine.</summary>
	public void BeginAttackRoutine()
	{
		if(Game.onTransition) return;

		this.RemoveStates(ID_STATE_IDLE);
		this.AddStates(ID_STATE_ATTACK);
		//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_ATTACK);

		switch(currentStage)
		{
			case STAGE_1:
			AnimationState state = null;

			if(health.hpRatio <=  stage1HealthPercentageLimit)
			{
				state = animation.GetAnimationState(throwBarrelAnimation);
				//if(!state.enabled)
				{
					BeginTNTThrowingRoutine();
				}
			}
			else
			{
				state = animation.GetAnimationState(throwBombAnimation);
				//if(!state.enabled)
				{
					BeginBombThrowingRoutine();
				}
			}
			break;

			case STAGE_2:
			this.StartCoroutine(WhackAMoleRoutine(), ref behaviorCoroutine);
			break;

			case STAGE_3:
			ActivateSword(true);
			sword.ActivateHitBoxes(false);
			this.StartCoroutine(DuelRoutine(), ref behaviorCoroutine);
			this.StartCoroutine(RotateTowardsMateo(), ref coroutine);
			break;
		}	
	}
#endregion

#region BombThrowingRoutines:
	/// <summary>Begins the Bomb Throwing Animations.</summary>
	public void BeginBombThrowingRoutine()
	{
		ActivateSword(false);
		animation.CrossFade(throwBombAnimation);
		animation.PlayQueued(idleAnimation);
		/// During the throw animation, a callback will be invoked that will then invoke ThrowBomb()
		//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_ATTACK);
		//animator.SetInteger(attackIDCredential, ID_ATTACK_BOMB_THROW);
	}

	/// <summary>Picks Bomb.</summary>
	public void PickBomb()
	{
		int index = 0;
		float time = 0.0f;

		switch(currentStage)
		{
			case STAGE_1:
			index = bombIndex;
			time = bombProjectionTime;
			break;

			case STAGE_2:
			index = bouncingBombIndex;
			time = bouncingBombProjectionTime;
			break;
		}

		bomb = PoolManager.RequestParabolaProjectile(Faction.Enemy, index, skeleton.rightHand.position, Game.mateo.transform.position, time, gameObject);
		bomb.activated = false;
		bomb.ActivateHitBoxes(false);
		bomb.transform.parent = skeleton.rightHand;
	}

	/// <summary>Throws Bomb [called after an specific frame of the Bom-Throwing Animation].</summary>
	public void ThrowBomb()
	{
		if(bomb == null) return;

		int index = 0;
		float time = 0.0f;

		switch(currentStage)
		{
			case STAGE_1:
			index = bombIndex;
			time = bombProjectionTime;
			break;

			case STAGE_2:
			index = bouncingBombIndex;
			time = bouncingBombProjectionTime;

			this.StartCoroutine(this.WaitSeconds(time,
			()=>
			{
				if(bomb != null)
				{
					Vector3 position = bomb.transform.position;
					position.z = Game.mateo.transform.position.z;

					bomb.activated = false;
					bomb.transform.position = position;
					bomb.rigidbody.bodyType = RigidbodyType2D.Dynamic;
					bomb.rigidbody.isKinematic = false;
					bomb.rigidbody.gravityScale = 4.0f;
					/*bomb.direction = Vector3.one;
					bomb.speed = 9.81f;*/
					bomb.direction = Vector3.zero;
					bomb.speed = 0.0f;
				}
			}));
			break;
		}

		bomb.transform.parent = null;
		bomb.OnObjectDeactivation();
		bomb = PoolManager.RequestParabolaProjectile(Faction.Enemy, index, skeleton.rightHand.position, Game.mateo.transform.position, time, gameObject);
		bomb.rigidbody.bodyType = RigidbodyType2D.Kinematic;
		bomb.rigidbody.isKinematic = true;
		bomb.rigidbody.gravityScale = 0.0f;

		switch(currentStage)
		{
			case STAGE_1:
			bomb.projectileEventsHandler.onProjectileEvent -= OnBombEvent;
			bomb.projectileEventsHandler.onProjectileEvent += OnBombEvent;
			bomb.projectileEventsHandler.onProjectileDeactivated -= OnBombDeactivated;
			bomb.projectileEventsHandler.onProjectileDeactivated += OnBombDeactivated;
			break;

			case STAGE_2:
			BombParabolaProjectile parabolaBomb = bomb as BombParabolaProjectile;
			parabolaBomb.ChangeState(BombState.WickOn);
			break;
		}

		bomb.activated = true;
		bomb.ActivateHitBoxes(true);

		//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_IDLE);
	}
#endregion

#region TNTThrowingRoutines:
	/// <summary>Begins TNT ThrowingRoutine.</summary>
	public void BeginTNTThrowingRoutine()
	{
		/// During the throw animation, a callback will be invoked that will then invoke ThrowTNT()
		//animator.SetInteger(stateIDCredential, ID_ATTACK_BARREL_THROW);
		//animator.SetInteger(attackIDCredential, ID_ANIMATIONSTATE_ATTACK);
		ActivateSword(false);
		//animation.Stop();
		animation.Rewind(throwBarrelAnimation);
		animation.CrossFade(throwBarrelAnimation);
		animation.PlayQueued(idleAnimation);
	}

	/// <summary>Picks TNT.</summary>
	public void PickTNT()
	{
		Vector3 anchoredPosition = Vector3.zero;

		TNT = PoolManager.RequestParabolaProjectile(Faction.Enemy, TNTIndex, skeleton.rightHand.position, Game.mateo.transform.position, TNTProjectionTime, gameObject);
		anchoredPosition = TNT.anchorContainer.GetAnchoredPosition(skeleton.rightHand.position, 0);
		TNT.transform.position = anchoredPosition;
		TNT.activated = false;
		TNT.ActivateHitBoxes(false);
		TNT.transform.parent = skeleton.rightHand;

		BombParabolaProjectile TNTBomb = TNT as BombParabolaProjectile;
		//TNTBomb.ChangeState(BombState.WickOn);
	}

	/// <summary>Throws TNT.</summary>
	public void ThrowTNT()
	{
		if(TNT == null) return;

		Vector3 anchoredPosition = Vector3.zero;
		Vector3 p = Vector3.zero;
		GameObjectTag[] impactTags = null;
		GameObjectTag[] flamableTags = null;
		float fuseDuration = 0.0f;
		float damage = 0.0f;
		float t = 0.0f;
		int explodableIndex = 0;

		switch(currentStage)
		{
			case STAGE_1:
			p = Game.mateo.transform.position;
			t = TNTProjectionTime;
			explodableIndex = stage1ExplodableIndex;
			impactTags = new GameObjectTag[] { Game.data.floorTag, Game.data.playerTag, Game.data.playerProjectileTag };
			flamableTags = new GameObjectTag[] { Game.data.playerProjectileTag };
			fuseDuration = stage1TNTFuseDuration;
			damage = Game.DAMAGE_MAX;
			break;

			case STAGE_2:
			p = line.a;
			t = stairParabolaTime;
			explodableIndex = stage2ExplodableIndex;
			impactTags = new GameObjectTag[] { Game.data.playerTag };
			flamableTags = new GameObjectTag[] { Game.data.playerProjectileTag };
			fuseDuration = stage2TNTFuseDuration;
			damage = Game.DAMAGE_MIN;
			break;
		}

		TNT.transform.parent = null;
		TNT.OnObjectDeactivation();
		TNT = PoolManager.RequestParabolaProjectile(Faction.Enemy, TNTIndex, skeleton.rightHand.position, p, t, gameObject);
		anchoredPosition = TNT.anchorContainer.GetAnchoredPosition(skeleton.rightHand.position, 0);
		TNT.transform.position = anchoredPosition;
		TNT.ActivateHitBoxes(true);
		TNT.impactTags = impactTags;
		TNT.damage = damage;
		//TNT.flamableTags = flamableTags;

		TNT.projectileEventsHandler.onProjectileEvent -= OnBombEvent;
		TNT.projectileEventsHandler.onProjectileEvent += OnBombEvent;
		TNT.projectileEventsHandler.onProjectileDeactivated -= OnBombDeactivated;
		TNT.projectileEventsHandler.onProjectileDeactivated += OnBombDeactivated;
		//TNT.activated = true;

		BombParabolaProjectile TNTBomb = TNT as BombParabolaProjectile;
		TNTBomb.fuseDuration = fuseDuration;
		TNTBomb.ChangeState(BombState.WickOn);
		TNTBomb.explodableIndex = explodableIndex;

		IEnumerator routine = null;

		switch(currentStage)
		{
			case STAGE_1:
			routine = Stage1TNTRoutine();
			break;

			case STAGE_2:
			routine = Stage2TNTRoutine();
			tntActive = true;
			break;
		}

		this.StartCoroutine(routine, ref coroutine);
		//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_IDLE);
	}
#endregion

#region Callbacks:
	/// <summary>Callback internally called when the Boss advances stage.</summary>
	protected override void OnStageChanged()
	{
		base.OnStageChanged();

		if(bomb != null)
		{
			bomb.OnObjectDeactivation();
			bomb = null;
		}
		if(TNT != null)
		{
			TNT.OnObjectDeactivation();
			TNT = null;
		}

		switch(currentStage)
		{
			case STAGE_1:
			EnablePhysics(false);
			health.inmunities = stage1Inmunities;
			break;

			case STAGE_2:
			EnablePhysics(false);
			health.inmunities = stage2Inmunities;
			break;

			case STAGE_3:
			EnablePhysics(true);
			break;
		}
	}

	/// <summary>Callback invoked when Shanty ought to be tied.</summary>
	/// <param name="_ship">Ship that will contain Shanty.</param>
	/// <param name="_tiePosition">Tie Position.</param>
	public void OnTie(Transform _ship, Vector3 _tiePosition)
	{
		transform.position = _tiePosition;
		transform.parent = _ship;
		//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_TIED);
		this.StartCoroutine(TieRoutine(), ref behaviorCoroutine);

	}

	/// <summary>Callback invoked when Shaanty ought to be untied.</summary>
	public void OnUntie()
	{
		this.DispatchCoroutine(ref behaviorCoroutine);

		this.StartCoroutine(animation.CrossFadeAndWait(untiedAnnimation, 0.3f, PlayMode.StopSameLayer, 0.0f,
		()=>
		{
			this.AddStates(ID_STATE_ATTACK);
		}));
	}

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public override void OnStatesAdded(int _state)
	{
		if((_state | ID_STATE_IDLE) == _state)
		{
			//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_IDLE);
			animation.CrossFade(idleAnimation);

		} else if((_state | ID_STATE_ATTACK) == _state)
		{
			//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_ATTACK);
			BeginAttackRoutine();

		} else if((_state | ID_STATE_HURT) == _state)
		{
			//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_DAMAGE);
			this.RemoveStates(ID_STATE_ATTACK);
			/*this.StartCoroutine(animator.WaitForAnimatorState(0, 0.0f,
			()=>
			{
				this.AddStates(ID_STATE_ATTACK);
			}));*/
		}
	}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public override void OnStatesRemoved(int _state)
	{
		if((_state | ID_STATE_HURT) == _state) BeginAttackRoutine();
	}

	/// <summary>Callback invoked when an Animation Event is invoked.</summary>
	/// <param name="_ID">Int argument.</param>
	protected override void OnAnimationIntEvent(int _ID)
	{
		switch(_ID)
		{
			case ID_ANIMATIONEVENT_PICKBOMB:
			ActivateSword(false);
			PickBomb();
			break;

			case ID_ANIMATIONEVENT_THROWBOMB:
			ThrowBomb();
			break;

			case ID_ANIMATIONEVENT_SWORD_UNSHEATH:
			ActivateSword(true);
			//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_IDLE);
			break;

			case ID_ANIMATIONEVENT_SWORD_SHEATH:
			ActivateSword(false);
			break;

			case ID_ANIMATIONEVENT_GOIDLE:
			//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_IDLE);
			animation.CrossFade(idleAnimation);
			break;

			case ID_ANIMATIONEVENT_PICKTNT:
			PickTNT();
			break;

			case ID_ANIMATIONEVENT_THROWTNT:
			ThrowTNT();
			break;

			case ID_ANIMATIONEVENT_REPELBOMB:
			if(bomb != null) bomb.RequestRepel(gameObject);
			break;

			case ID_ANIMATIONEVENT_JUMP:
			Jump();
			break;

			case 10:
			break;
		}
	}

	/// <summary>Callback invoked when a Bomb Event occurs.</summary>
	/// <param name="_projecile">Bomb's reference.</param>
	/// <param name="_eventID">Bomb's Event ID.</param>
	/// <param name="_info">Additional Trigger2DInformation.</param>
	private void OnBombEvent(Projectile _projectile, int _eventID, Trigger2DInformation _info)
	{
		switch(currentStage)
		{
			case STAGE_1:
			switch(_eventID)
			{
				case Projectile.ID_EVENT_REPELLED:
				if(_projectile.owner == gameObject) return;

				ActivateSword(false);
				animation.Stop();
				this.StartCoroutine(animation.PlayAndSincronizeAnimationWithTime(tenninsHitAnimation, 14, _projectile.parabolaTime * bombProjectionPercentage), ref behaviorCoroutine);
				break;
			}
			break;

			case STAGE_2:
			break;
		}
	}

	/// <summary>Event invoked when the projectile is deactivated.</summary>
	/// <param name="_projectile">Projectile that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	private void OnBombDeactivated(Projectile _projectile, DeactivationCause _cause, Trigger2DInformation _info)
	{
		switch(currentStage)
		{
			case STAGE_1:
			switch(_cause)
			{
				default:
				BeginAttackRoutine();
				break;
			}
			break;

			case STAGE_2:
			if(_projectile == TNT)
			{
				tntActive = false;
				this.DispatchCoroutine(ref TNTRotationCoroutine);
			}
			break;
		}
	}

	/// <summary>Callback invoked when a TNT Event occurs.</summary>
	/// <param name="_projecile">TNT's reference.</param>
	/// <param name="_eventID">TNT's Event ID.</param>
	/// <param name="_info">Additional Trigger2DInformation.</param>
	private void OnTNTEvent(Projectile _projectile, int _eventID, Trigger2DInformation _info)
	{
		switch(_eventID)
		{
			case Projectile.ID_EVENT_REPELLED:
			/*Projectile projectile = _info.collider.GetComponentInParent<Projectile>();
			
			if(projectile == null) return;*/

			if(_projectile.owner == null || _projectile.owner == gameObject) return;

			float durationBeforeSwordSwing = _projectile.parabolaTime * windowPercentage;

			this.StartCoroutine(this.WaitSeconds(durationBeforeSwordSwing, 
			()=>
			{
				//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_ATTACK);
				//animator.SetInteger(attackIDCredential, ID_ATTACL_HIT_TENNIS);
				//animation.CrossFade(tenninsHitAnimation);
			}));
			break;
		}
	}

	/// <summary>Callback invoked when a Health's event has occured.</summary>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		base.OnHealthEvent(_event, _amount, _object);

		switch(_event)
		{
			case HealthEvent.Depleted:
			switch(currentStage)
			{
				case STAGE_1:
				//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_DAMAGE);

				if(_object == null) return;

				int damageID = 0;
				AnimationClip damageClip = null;

				if(_object.CompareTag(Game.data.playerWeaponTag))
				{
					damageID = ID_DAMAGE_SWORD;
					damageClip = hitSwordAnimation;

				} else if(_object.CompareTag(Game.data.playerProjectileTag))
				{
					damageID = ID_DAMAGE_BOMB;
					damageClip = hitBombAnimation;

				} else if(_object.CompareTag(Game.data.explodableTag))
				{
					damageID = ID_DAMAGE_BARREL;
					damageClip = hitBarrelAnimation;
				}

				//animator.SetInteger(vitalityIDCredential, damageID);
				animation.CrossFade(damageClip);
				this.RemoveStates(ID_STATE_ATTACK);
				this.AddStates(ID_STATE_HURT);
				break;
			}
			break;

			case HealthEvent.FullyDepleted:
			//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_DAMAGE);
			//animator.SetInteger(vitalityIDCredential, ID_DAMAGE_CRY);

			if(currentStage >= stages)
			{
				//eventsHandler.InvokeIDEvent(ID_EVENT_BOSS_DEATHROUTINE_BEGINS);
				this.DispatchCoroutine(ref behaviorCoroutine);
				this.DispatchCoroutine(ref coroutine);
				animation.CrossFade(cryAnimation);
				this.ChangeState(ID_STATE_DEAD);
				BeginDeathRoutine();
			}
			break;

			case HealthEvent.HitStunEnds:
			this.RemoveStates(ID_STATE_HURT);
			this.ReturnToPreviousState();
			break;
		}
	}
#endregion

#region Coroutines:
	/// <summary>Tie's Coroutine.</summary>
	private IEnumerator TieRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		AnimationState animationState = null;

		while(true)
		{
			foreach(AnimationClip clip in tiedAnimations)
			{
				animation.CrossFade(clip); /// Update with VAnimation's function (waiting for Zucun to upload his version)
				animationState = animation.GetAnimationState(clip);
				wait.ChangeDurationAndReset(animationState.clip.length);

				while(wait.MoveNext()) yield return null;
			}
		}
	}

	/// <summary>TNT's Routine when it is thrown [for Stage 1].</summary>
	private IEnumerator Stage1TNTRoutine()
	{
		float waitDuration = TNTProjectionTime * TNTTimeScaleChangeProgress;
		float slowDownDuration = TNTProjectionTime - waitDuration;

		TNT.activated = true;
		SecondsDelayWait wait = new SecondsDelayWait(waitDuration);

		while(wait.MoveNext()) yield return null;

		Game.SetTimeScale(TNTThrowTimeScale);
		wait.ChangeDurationAndReset(slowDownDuration);

		while(wait.MoveNext()) yield return null;

		Game.SetTimeScale(1.0f);
	}

	/// <summary>TNT's Routine when it is thrown [for Stage 2].</summary>
	private IEnumerator Stage2TNTRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(stairParabolaTime);
		Vector3 a = line.a;
		Vector3 b = line.b;
		Vector3 d = Vector3.zero;
		float t = 0.0f;
		float inverseDuration = (1.0f / stairSlideDuration);
		Line mainDeckPath = ShantySceneController.Instance.mainDeckPath;
		/*bool run = true;
		OnProjectileDeactivated onProjectileDeactivated = (_projectile, _cause, _info)=>
		{
			run = false;
		};

		TNT.projectileEventsHandler.onProjectileDeactivated -= onProjectileDeactivated;
		TNT.projectileEventsHandler.onProjectileDeactivated += onProjectileDeactivated;*/
		TNT.ActivateHitBoxes(false);

		while(wait.MoveNext()) yield return null;

		TNT.activated = false;
		TNT.StartCoroutine(TNT.meshContainer.transform.RotateOnAxis(Vector3.right, TNTRotationSpeed), ref TNTRotationCoroutine);

		while(t < 1.0f)
		{
			a = TNT.transform.position;
			b = line.Lerp(t);
			d = (b - a).normalized;
			TNT.transform.position = b;
			TNT.transform.rotation = Quaternion.LookRotation(d);

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		TNT.ActivateHitBoxes(true);
		b = mainDeckPath.Lerp(0.5f);
		d = b - TNT.transform.position;
		float sqrDistance = (0.1f * 0.1f);

		while(d.sqrMagnitude < sqrDistance)
		{
			TNT.transform.position += (d * sidewaysMovementSpeed * Time.deltaTime);
			TNT.transform.rotation = Quaternion.LookRotation(d);
			d = b - TNT.transform.position;
			yield return null;
		}

		b = line == ShantySceneController.Instance.leftStairPath ? mainDeckPath.b : mainDeckPath.a;
		d = b - TNT.transform.position;

		while(tntActive)
		{
			TNT.transform.position += (d.normalized * sidewaysMovementSpeed * Time.deltaTime);
			TNT.transform.rotation = Quaternion.LookRotation(d);
			d = b - TNT.transform.position;

			if(d.sqrMagnitude <= sqrDistance)
			b = b == mainDeckPath.a ? mainDeckPath.b : mainDeckPath.a;

			yield return null;
		}

		TNT.DispatchCoroutine(ref TNTRotationCoroutine);
	}

	/// <summary>Whack-A-Mole's Routine.</summary>
	private IEnumerator WhackAMoleRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		AnimationClip clipA = null;
		AnimationClip clipB = null;
		Vector3Pair pair = default(Vector3Pair);
		Vector3 a = Vector3.zero;
		Vector3 b = Vector3.zero;
		int pairID = 0;
		float t = 0.0f;
		float inverseDuration = 1.0f / vectorPairInterpolationDuration;
		bool toggled = false;

		EnableHurtBoxes(false);

		while(true)
		{
			int max = tntActive ? 2 : 4;
			//max = 2;
			pairID = Random.Range(0, max);
			t = 0.0f;
			toggled = false;

			switch(pairID)
			{
				case ID_WAYPOINTSPAIR_HELM:
				pair = ShantySceneController.Instance.helmWaypointsPair;
				clipA = throwBombAnimation;
				break;

				case ID_WAYPOINTSPAIR_DECK:
				pair = ShantySceneController.Instance.deckWaypointsPair;
				clipA = throwBombAnimation;
				break;

				case ID_WAYPOINTSPAIR_STAIR_LEFT:
				pair = ShantySceneController.Instance.leftStairWaypointsPair;
				clipA = throwBarrelAnimation;
				line = ShantySceneController.Instance.leftStairPath;
				break;

				case ID_WAYPOINTSPAIR_STAIR_RIGHT:
				pair = ShantySceneController.Instance.rightStairWaypointsPair;
				clipA = throwBarrelAnimation;
				line = ShantySceneController.Instance.rightStairPath;
				break;
			}

			a = ship.transform.TransformPoint(pair.a);
			b = ship.transform.TransformPoint(pair.b);

			while(t < 1.0f)
			{
				transform.position = Vector3.Lerp(a, b, t);

				if(!toggled && t >= progressToToggleHurtBoxes)
				{
					toggled = true;
					EnableHurtBoxes(true);
				}

				t += (Time.deltaTime * inverseDuration);

				yield return null;
			}

			t = 0.0f;
			toggled = false;

			animation.CrossFade(clipA);
			wait.ChangeDurationAndReset(clipA.length);

			while(wait.MoveNext()) yield return null;

			animation.CrossFade(idleAnimation);
			wait.ChangeDurationAndReset(waitBeforeWaypointReturn);

			while(wait.MoveNext()) yield return null;

			while(t < 1.0f)
			{
				transform.position = Vector3.Lerp(b, a, t);

				if(!toggled && t >= progressToToggleHurtBoxes)
				{
					toggled = true;
					EnableHurtBoxes(false);
				}

				t += (Time.deltaTime * inverseDuration);

				yield return null;
			}

			yield return null;
		}
	}

	/// <summary>Duel's Routine.</summary>
	private IEnumerator DuelRoutine()
	{
		IEnumerator attackRoutine = null;
		AnimationClip clip = null;
		AnimationState animationState = null;
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		Vector3 direction = Vector3.zero;
		float min = attackRadiusRange.Min();
		float max = attackRadiusRange.Max();
		float minSqrDistance = min * min;
		float maxSqrDistance = max * max;
		float sqrDistance = 0.0f;
		bool enteredAttackRadius = false;

		while(true)
		{
			direction = Game.mateo.transform.position - transform.position;
			sqrDistance = direction.sqrMagnitude;

			if(sqrDistance <= maxSqrDistance)
			{
				if(sqrDistance > minSqrDistance)
				{
					if(!enteredAttackRadius)
					{
						wait.ChangeDurationAndReset(strongAttackWaitInterval.Random());
						enteredAttackRadius = true;
					}

					if(!wait.MoveNext())
					{
						if(!strongAttackCooldown.onCooldown)
						attackRoutine = StrongAttackRoutine();
						while(attackRoutine.MoveNext()) yield return null;

						animation.CrossFade(idleAnimation);
						wait.ChangeDurationAndReset(strongAttackCooldownDuration);

						while(wait.MoveNext()) yield return null;

						enteredAttackRadius = false;
					}
					else
					{
						direction = direction.x > 0.0f ? Vector3.right : Vector3.left;
						Move(direction);
					}

				} else if(sqrDistance <= minSqrDistance && !normalAttackCooldown.onCooldown)
				{
					enteredAttackRadius = false;
					direction = direction.x > 0.0f ? Vector3.right : Vector3.left;
					attackRoutine = FrontAttackRoutine(direction);
					while(attackRoutine.MoveNext()) yield return null;
				}
			}
			else
			{
				enteredAttackRadius = false;
				direction = direction.x > 0.0f ? Vector3.right : Vector3.left;
				Move(direction);
			}

			yield return null;
		}
	}

	/// <summary>Rotate Towards Mateo's Routine [used on the Duel].</summary>
	private IEnumerator RotateTowardsMateo()
	{
		float x = 0.0f;
		Vector3 direction = Vector3.zero;

		while(true)
		{
			x = Game.mateo.transform.position.x - transform.position.x;
			direction = x > 0.0f ? Vector3.right : Vector3.left;

			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

			yield return null;
		}
	}

	/// <summary>Front Attack's routine.</summary>
	private IEnumerator FrontAttackRoutine(Vector3 direction)
	{
		AnimationState animationState = null;
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);


		animation.CrossFade(normalAttackAnimation);
		animationState = animation.GetAnimationState(normalAttackAnimation);
		sword.ActivateHitBoxes(true);
		wait.ChangeDurationAndReset(animationState.length);
		dashAbility.Dash(direction);

		while(wait.MoveNext()) yield return null;

		sword.ActivateHitBoxes(false);

		wait.ChangeDurationAndReset(regressionDuration);

		while(dashAbility.state != DashState.Unactive) yield return null;

		while(wait.MoveNext())
		{
			Move(-direction);
			yield return null;
		}

		normalAttackCooldown.Begin();
	}

	/// <summary>Strong Attack's Routine.</summary>
	private IEnumerator StrongAttackRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		AnimationState animationState = null;

		animation.CrossFade(strongAttackAnimation);
		animationState = animation.GetAnimationState(strongAttackAnimation);
		wait.ChangeDurationAndReset(strongAttackAnimation.length);

		while(wait.MoveNext()) yield return null;

		strongAttackCooldown.Begin();
	}

	/// <summary>Jump's Routine.</summary>
	private IEnumerator JumpRoutine()
	{
		Vector3 direction = Game.mateo.transform.position - transform.position;
		direction.y = 0.0f;
		direction.Normalize();
		sword.ActivateHitBoxes(true);

		while(!jumpAbility.HasStates(JumpAbility.STATE_ID_FALLING))
		{
			jumpAbility.Jump(Vector3.up);
			movementAbility.Move(direction, 2.0f);
			yield return null;
		}

		while(!jumpAbility.HasStates(JumpAbility.STATE_ID_GROUNDED))
		{
			movementAbility.Move(direction, 2.0f);
			yield return null;
		}

		sword.ActivateHitBoxes(false);
	}
#endregion

	/*/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.CompareTag(Game.data.playerTag))
		rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
	}

	/// <summary>Event triggered when this Collider/Rigidbody began having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.CompareTag(Game.data.playerTag))
		rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
	}*/
}
}