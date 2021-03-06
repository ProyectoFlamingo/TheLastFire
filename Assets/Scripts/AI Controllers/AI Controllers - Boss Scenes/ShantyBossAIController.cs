using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

using Random = UnityEngine.Random;

namespace Flamingo
{
public class ShantyBossAIController : CharacterAIController<ShantyBoss>
{
	public const int ID_WAYPOINTSPAIR_HELM = 0; 							/// <summary>Helm's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_DECK = 1; 							/// <summary>Deck's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_STAIR_LEFT = 2; 						/// <summary>Left Stair's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_STAIR_RIGHT = 3; 						/// <summary>Right Stair's Waypoints' Pair ID.</summary>

	[Space(5f)]
	[Header("Shanty's Attributes:")]
	[Space(5f)]
	[SerializeField] private ShantyShip _ship; 								/// <summary>Shanty's Ship.</summary>
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _windowPercentage; 					/// <summary>Time-window before swinging sword (to hit bomb).</summary>
	[Space(5f)]
	[Header("Stage 1 Bomb's Attributes:")]
	[SerializeField] private float _bombProjectionTime; 					/// <summary>Bomb's Projection Time.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _bombProjectionPercentage; 			/// <summary>Bomb Projection Time's Percentage.</summary>
	[Space(5f)]
	[Header("Stage 2 Bomb's Attributes:")]
	[SerializeField] private float _bouncingBombProjectionTime; 			/// <summary>Bouncing Bomb's Projection Time.</summary>
	[Space(5f)]
	[Header("Stage 1 TNT's Attributes:")]
	[SerializeField] private VAssetReference _stage1ExplodableReference; 	/// <summary>Explodable's Reference for TNT on Stage 1.</summary>
	[SerializeField] private float _stage1TNTFuseDuration; 					/// <summary>Fuse Duration for TNT on Stage 1.</summary>
	[SerializeField] private float _TNTProjectionTime; 						/// <summary>TNT's Projection Time.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _TNTProjectionPercentage; 			/// <summary>TNT Projection Time's Percentage.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _TNTTimeScaleChangeProgress; 			/// <summary>TNT parabolas' progress necessary to slow down time.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _TNTThrowTimeScale; 					/// <summary>Time Scale when throwing TNT.</summary>
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _stage1HealthPercentageLimit; 		/// <summary>Health Limit's Percentage for TNT.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _stage2HealthPercentageLimit; 		/// <summary>Health Limit's Percentage for TNT.</summary>
	[Space(5f)]
	[Header("Stage 2's TNT's Attributes:")]
	[SerializeField] private VAssetReference _stage2ExplodableReference; 	/// <summary>Explodable's Reference for TNT on Stage 2.</summary>
	[SerializeField] private float _stage2TNTFuseDuration; 					/// <summary>Fuse Duration for TNT on Stage 2.</summary>
	[SerializeField] private float _stairParabolaTime; 						/// <summary>Duration from throw to beginning of stair.</summary>
	[SerializeField] private float _stairSlideDuration; 					/// <summary>Stair Slide's Duration.</summary>
	[SerializeField] private float _sidewaysMovementSpeed; 					/// <summary>Sideways' Movement Speed.</summary>
	[SerializeField] private float _TNTRotationSpeed; 						/// <summary>TNT's Rotation Angular Speed.</summary>
	[Space(5f)]
	[Header("Whack-A-Mole's Attributes:")]
	[SerializeField] private float _vectorPairInterpolationDuration; 		/// <summary>Interpolation duration for Whack-A-Mole's Waypoints.</summary>
	[SerializeField] private float _waitBeforeWaypointReturn; 				/// <summary>Wait before Waypoint's Return.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _progressToToggleHurtBoxes; 			/// <summary>Process percentage on the interpolation to toggle the Hurt-Boxes.</summary>
	[Space(5f)]
	[Header("Duel's Attributes:")]
	[SerializeField] private FloatRange _attackRadiusRange; 				/// <summary>Attacks' Radius Range.</summary>
	[SerializeField] private FloatRange _strongAttackWaitInterval; 			/// <summary>Strong Attack's Wait Interval.</summary>
	[SerializeField] private float _movementSpeed; 							/// <summary>Movement's Speed.</summary>
	[SerializeField] private float _rotationSpeed; 							/// <summary>Rotation's Speed.</summary>
	[SerializeField] private float _regressionDuration; 					/// <summary>Regression's Duration.</summary>
	[SerializeField] private float _attackDistance; 						/// <summary>Attack's Distance.</summary>
	[SerializeField] private float _normalAttackCooldownDuration; 			/// <summary>Normal Attack Cooldown's Duration.</summary>
	[SerializeField] private float _strongAttackCooldownDuration; 			/// <summary>Strong Attack Cooldown's Duration.</summary>
	[Space(5f)]
	[Header("Inmunities:")]
	[SerializeField] private GameObjectTag[] _stage1Inmunities; 			/// <summary>Inmunities on Stage 1.</summary>
	[SerializeField] private GameObjectTag[] _stage2Inmunities; 			/// <summary>Inmunities on Stage 2.</summary>
	private Coroutine coroutine; 											/// <summary>Coroutine's Reference.</summary>
	private Coroutine TNTRotationCoroutine; 								/// <summary>TNT's Rotation Coroutine's Reference.</summary>
	private Behavior attackBehavior; 										/// <summary>Attack's Behavior [it is behavior so it can be paused].</summary>
	private Cooldown _normalAttackCooldown; 								/// <summary>Normal Attack's Cooldown.</summary>
	private Cooldown _strongAttackCooldown; 								/// <summary>Strong Attack's Cooldown.</summary>
	private Line _line; 													/// <summary>Current Stair's Line.</summary>
	private bool _tntActive; 												/// <summary>Is the TNT Coroutine's Running?.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ship property.</summary>
	public ShantyShip ship
	{
		get { return _ship; }
		set { _ship = value; }
	}

	/// <summary>Gets stage1ExplodableReference property.</summary>
	public VAssetReference stage1ExplodableReference { get { return _stage1ExplodableReference; } }

	/// <summary>Gets stage2ExplodableReference property.</summary>
	public VAssetReference stage2ExplodableReference { get { return _stage2ExplodableReference; } }

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

	/// <summary>Gets stage1Inmunities property.</summary>
	public GameObjectTag[] stage1Inmunities { get { return _stage1Inmunities; } }

	/// <summary>Gets stage2Inmunities property.</summary>
	public GameObjectTag[] stage2Inmunities { get { return _stage2Inmunities; } }

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
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();

		if(character == null) return;

		Gizmos.DrawWireSphere(character.transform.position, attackDistance);
		Gizmos.DrawWireSphere(character.transform.position, attackRadiusRange.Min());
		Gizmos.DrawWireSphere(character.transform.position, attackRadiusRange.Max());
	}
#endif

	/// <summary>ShantyBoss's instance initialization.</summary>
	protected override void Awake()
	{
		character.ActivateSword(false);
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
	/// <summary>Performs Jumps.</summary>
	private void JumpAttack()
	{
		this.StartCoroutine(JumpAttackRoutine());
	}

	/// <summary>Begins Attack's Routine.</summary>
	public void BeginAttackRoutine()
	{
		if(Game.state == GameState.Transitioning) return;

		this.RemoveStates(IDs.STATE_IDLE);
		this.AddStates(IDs.STATE_ATTACKING_0);
		//animator.SetInteger(stateIDCredential, ID_ANIMATIONSTATE_ATTACK);

		switch(character.currentStage)
		{
			case Boss.STAGE_1:
			AnimationState state = null;

			if(character.health.hpRatio <=  stage1HealthPercentageLimit)
			{
				state = character.animation.GetAnimationState(character.throwBarrelAnimation);
				//if(!state.enabled)
				{
					BeginTNTThrowingRoutine();
				}
			}
			else
			{
				state = character.animation.GetAnimationState(character.throwBombAnimation);
				//if(!state.enabled)
				{
					BeginBombThrowingRoutine();
				}
			}
			break;

			case Boss.STAGE_2:
			this.StartCoroutine(WhackAMoleRoutine(), ref behaviorCoroutine);
			break;

			case Boss.STAGE_3:
			character.ActivateSword(true);
			character.sword.ActivateHitBoxes(false);
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
		character.ActivateSword(false);
		character.animation.CrossFade(character.throwBombAnimation);
		character.animation.PlayQueued(character.idleAnimation);
	}

	/// <summary>Picks Bomb.</summary>
	public void PickBomb()
	{
		VAssetReference reference = null;
		float time = 0.0f;

		switch(character.currentStage)
		{
			case Boss.STAGE_1:
			reference = character.bombReference;
			time = bombProjectionTime;
			break;

			case Boss.STAGE_2:
			reference = character.bouncingBombReference;
			time = bouncingBombProjectionTime;
			break;
		}

		character.bomb = PoolManager.RequestParabolaProjectile(Faction.Enemy, reference, character.skeleton.rightHand.position, Game.mateo.transform.position, time, gameObject);
		character.bomb.activated = false;
		character.bomb.ActivateHitBoxes(false);
		character.bomb.transform.parent = character.skeleton.rightHand;
	}

	/// <summary>Throws Bomb [called after an specific frame of the Bom-Throwing Animation].</summary>
	public void ThrowBomb()
	{
		if(character.bomb == null) return;

		VAssetReference reference = null;
		float time = 0.0f;

		switch(character.currentStage)
		{
			case Boss.STAGE_1:
			reference = character.bombReference;
			time = bombProjectionTime;
			break;

			case Boss.STAGE_2:
			reference = character.bouncingBombReference;
			time = bouncingBombProjectionTime;

			this.StartCoroutine(this.WaitSeconds(time,
			()=>
			{
				if(character.bomb != null)
				{
					Vector3 position = character.bomb.transform.position;
					position.z = Game.mateo.transform.position.z;

					character.bomb.activated = false;
					character.bomb.transform.position = position;
					character.bomb.rigidbody.bodyType = RigidbodyType2D.Dynamic;
					character.bomb.rigidbody.isKinematic = false;
					character.bomb.rigidbody.gravityScale = 4.0f;
					character.bomb.direction = Vector3.zero;
					character.bomb.speed = 0.0f;
				}
			}));
			break;
		}

		character.bomb.transform.parent = null;
		character.bomb.OnObjectDeactivation();
		character.bomb = PoolManager.RequestParabolaProjectile(Faction.Enemy, reference, character.skeleton.rightHand.position, Game.mateo.transform.position, time, gameObject);
		character.bomb.rigidbody.bodyType = RigidbodyType2D.Kinematic;
		character.bomb.rigidbody.isKinematic = true;
		character.bomb.rigidbody.gravityScale = 0.0f;

		switch(character.currentStage)
		{
			case Boss.STAGE_1:
				character.bomb.projectileEventsHandler.onProjectileEvent -= OnBombEvent;
				character.bomb.projectileEventsHandler.onProjectileEvent += OnBombEvent;
				character.bomb.projectileEventsHandler.onProjectileDeactivated -= OnBombDeactivated;
				character.bomb.projectileEventsHandler.onProjectileDeactivated += OnBombDeactivated;
			break;

			case Boss.STAGE_2:
				BombParabolaProjectile parabolaBomb = character.bomb as BombParabolaProjectile;
				parabolaBomb.ChangeState(BombState.WickOn);
			break;
		}

		character.bomb.activated = true;
		character.bomb.ActivateHitBoxes(true);
	}
#endregion

#region TNTThrowingRoutines:
	/// <summary>Begins TNT ThrowingRoutine.</summary>
	public void BeginTNTThrowingRoutine()
	{
		character.ActivateSword(false);
		character.animation.Rewind(character.throwBarrelAnimation);
		character.animation.CrossFade(character.throwBarrelAnimation);
		character.animation.PlayQueued(character.idleAnimation);
	}

	/// <summary>Picks TNT.</summary>
	public void PickTNT()
	{
		Vector3 anchoredPosition = Vector3.zero;

		character.TNT = PoolManager.RequestParabolaProjectile(Faction.Enemy, character.TNTReference, character.skeleton.rightHand.position, Game.mateo.transform.position, TNTProjectionTime, gameObject);
		anchoredPosition = character.TNT.anchorContainer.GetAnchoredPosition(character.skeleton.rightHand.position, 0);
		character.TNT.transform.position = anchoredPosition;
		character.TNT.activated = false;
		character.TNT.ActivateHitBoxes(false);
		character.TNT.transform.parent = character.skeleton.rightHand;

		BombParabolaProjectile TNTBomb = character.TNT as BombParabolaProjectile;
		//TNTBomb.ChangeState(BombState.WickOn);
	}

	/// <summary>Throws TNT.</summary>
	public void ThrowTNT()
	{
		if(character.TNT == null) return;

		Vector3 anchoredPosition = Vector3.zero;
		Vector3 p = Vector3.zero;
		GameObjectTag[] impactTags = null;
		GameObjectTag[] flamableTags = null;
		float fuseDuration = 0.0f;
		float damage = 0.0f;
		float t = 0.0f;
		VAssetReference reference = null;

		switch(character.currentStage)
		{
			case Boss.STAGE_1:
				p = Game.mateo.transform.position;
				t = TNTProjectionTime;
				reference = stage1ExplodableReference;
				impactTags = new GameObjectTag[] { Game.data.floorTag, Game.data.playerTag, Game.data.playerProjectileTag };
				flamableTags = new GameObjectTag[] { Game.data.playerProjectileTag };
				fuseDuration = stage1TNTFuseDuration;
				damage = Game.DAMAGE_MAX;
			break;

			case Boss.STAGE_2:
				p = line.a;
				t = stairParabolaTime;
				reference = stage2ExplodableReference;
				impactTags = new GameObjectTag[] { Game.data.playerTag };
				flamableTags = new GameObjectTag[] { Game.data.playerProjectileTag };
				fuseDuration = stage2TNTFuseDuration;
				damage = Game.DAMAGE_MIN;
			break;
		}

		character.TNT.transform.parent = null;
		character.TNT.OnObjectDeactivation();
		character.TNT = PoolManager.RequestParabolaProjectile(Faction.Enemy, character.TNTReference, character.skeleton.rightHand.position, p, t, gameObject);
		anchoredPosition = character.TNT.anchorContainer.GetAnchoredPosition(character.skeleton.rightHand.position, 0);
		character.TNT.transform.position = anchoredPosition;
		character.TNT.ActivateHitBoxes(true);
		character.TNT.impactTags = impactTags;
		character.TNT.damage = damage;

		character.TNT.projectileEventsHandler.onProjectileEvent -= OnBombEvent;
		character.TNT.projectileEventsHandler.onProjectileEvent += OnBombEvent;
		character.TNT.projectileEventsHandler.onProjectileDeactivated -= OnBombDeactivated;
		character.TNT.projectileEventsHandler.onProjectileDeactivated += OnBombDeactivated;

		BombParabolaProjectile TNTBomb = character.TNT as BombParabolaProjectile;
		TNTBomb.fuseDuration = fuseDuration;
		TNTBomb.ChangeState(BombState.WickOn);
		TNTBomb.explodableReference = reference;

		IEnumerator routine = null;

		switch(character.currentStage)
		{
			case Boss.STAGE_1:
			routine = Stage1TNTRoutine();
			break;

			case Boss.STAGE_2:
			routine = Stage2TNTRoutine();
			tntActive = true;
			break;
		}

		this.StartCoroutine(routine, ref coroutine);
	}
#endregion

	/// <summary>Subscribes to Character's Events [Deactivation, ID and State events].</summary>
	/// <param name="_character">Target Character.</param>
	/// <param name="_subscribe">Subscribe? true by default.</param>
	protected override void SubscribeToCharacterEvents(ShantyBoss _character, bool _subscribe = true)
	{
		base.SubscribeToCharacterEvents(_character, _subscribe);

		AnimationEventInvoker animationEventInvoker = _character.animationEventInvoker;

		if(animationEventInvoker == null) return;

		switch(_subscribe)
		{
			case true:
				animationEventInvoker.AddIntActionListener(OnAnimationIntEvent);
			break;

			case false:
				animationEventInvoker.RemoveIntActionListener(OnAnimationIntEvent);
			break;
		}
	}

#region Callbacks:
	/// <summary>Callback invoked when a Character's state is changed.</summary>
	/// <param name="_character">Character that invokes the event.</param>
	/// <param name="_flags">State Flags.</param>
	/// <param name="_stateChange">Type of State Change.</param>
	protected override void OnCharacterStateChanged(Character _character, int _state, StateChange _stateChange)
	{
		base.OnCharacterStateChanged(_character, _state, _stateChange);

		switch(_stateChange)
		{
			case StateChange.Entered:
			break;

			case StateChange.Left:
			break;

			case StateChange.Added:
				if((_state | IDs.STATE_IDLE) == _state)
				{
					character.animation.CrossFade(character.idleAnimation);

				} else if((_state | IDs.STATE_ATTACKING_0) == _state)
				{
					BeginAttackRoutine();

				} else if((_state | IDs.STATE_HURT) == _state)
				{
					this.RemoveStates(IDs.STATE_ATTACKING_0);
				}
			break;

			case StateChange.Removed:
				if((_state | IDs.STATE_HURT) == _state) BeginAttackRoutine();
			break;
		}
	}

	/// <summary>Callback invoked when the character invokes an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected override void OnCharacterIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_STAGECHANGED:
				if(character.bomb != null)
				{
					character.bomb.OnObjectDeactivation();
					character.bomb = null;
				}
				if(character.TNT != null)
				{
					character.TNT.OnObjectDeactivation();
					character.TNT = null;
				}

				switch(character.currentStage)
				{
					case Boss.STAGE_1:
					character.EnablePhysics(false);
					character.health.inmunities = stage1Inmunities;
					break;

					case Boss.STAGE_2:
					character.EnablePhysics(false);
					character.health.inmunities = stage2Inmunities;
					break;

					case Boss.STAGE_3:
					character.EnablePhysics(true);
					break;
				}
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
		this.StartCoroutine(TieRoutine(), ref behaviorCoroutine);

	}

	/// <summary>Callback invoked when Shaanty ought to be untied.</summary>
	public void OnUntie()
	{
		this.DispatchCoroutine(ref behaviorCoroutine);

		this.StartCoroutine(character.animation.CrossFadeAndWait(character.untiedAnimation, 0.3f, PlayMode.StopSameLayer, 0.0f,
		()=>
		{
			this.AddStates(IDs.STATE_ATTACKING_0);
		}));
	}

	/// <summary>Callback invoked when an Animation Event is invoked.</summary>
	/// <param name="_ID">Int argument.</param>
	private void OnAnimationIntEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.ANIMATIONEVENT_PICKBOMB:
				character.ActivateSword(false);
				PickBomb();
			break;

			case IDs.ANIMATIONEVENT_THROWBOMB:
				ThrowBomb();
			break;

			case IDs.ANIMATIONEVENT_WEAPON_UNSHEATH:
				character.ActivateSword(true);
			break;

			case IDs.ANIMATIONEVENT_WEAPON_SHEATH:
				character.ActivateSword(false);
			break;

			case IDs.ANIMATIONEVENT_GOIDLE:
				character.animation.CrossFade(character.idleAnimation);
			break;

			case IDs.ANIMATIONEVENT_PICKTNT:
				PickTNT();
			break;

			case IDs.ANIMATIONEVENT_THROWTNT:
				ThrowTNT();
			break;

			case IDs.ANIMATIONEVENT_REPELBOMB:
				if(character.bomb != null) character.bomb.RequestRepel(gameObject);
			break;

			case IDs.ANIMATIONEVENT_JUMP:
				JumpAttack();
			break;

			default:
				Debug.Log("[ShantyBossAIController] Default Case #" + _ID);
			break;
		}
	}

	/// <summary>Callback invoked when a Bomb Event occurs.</summary>
	/// <param name="_projecile">Bomb's reference.</param>
	/// <param name="_eventID">Bomb's Event ID.</param>
	/// <param name="_info">Additional Trigger2DInformation.</param>
	private void OnBombEvent(Projectile _projectile, int _eventID, Trigger2DInformation _info)
	{
		switch(character.currentStage)
		{
			case Boss.STAGE_1:
				switch(_eventID)
				{
					case IDs.EVENT_REPELLED:
					if(_projectile.owner == gameObject) return;

					character.ActivateSword(false);
					character.animation.Stop();
					this.StartCoroutine(character.animation.PlayAndSincronizeAnimationWithTime(character.tennisHitAnimation, 14, _projectile.parabolaTime * bombProjectionPercentage), ref behaviorCoroutine);
					break;
				}
			break;

			case Boss.STAGE_2:
			break;
		}
	}

	/// <summary>Event invoked when the projectile is deactivated.</summary>
	/// <param name="_projectile">Projectile that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	private void OnBombDeactivated(Projectile _projectile, DeactivationCause _cause, Trigger2DInformation _info)
	{
		switch(character.currentStage)
		{
			case Boss.STAGE_1:
			switch(_cause)
			{
				default:
				BeginAttackRoutine();
				break;
			}
			break;

			case Boss.STAGE_2:
				if(_projectile == character.TNT)
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
			case IDs.EVENT_REPELLED:
				if(_projectile.owner == null || _projectile.owner == gameObject) return;

				float durationBeforeSwordSwing = _projectile.parabolaTime * windowPercentage;

				this.StartCoroutine(this.WaitSeconds(durationBeforeSwordSwing, 
				()=>
				{
					//...
				}));
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
			foreach(AnimationClip clip in character.tiedAnimations)
			{
				character.animation.CrossFade(clip); /// Update with VAnimation's function (waiting for Zucun to upload his version)
				animationState = character.animation.GetAnimationState(clip);
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

		character.TNT.activated = true;
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

		character.TNT.ActivateHitBoxes(false);

		while(wait.MoveNext()) yield return null;

		character.TNT.activated = false;
		character.TNT.StartCoroutine(character.TNT.meshContainer.transform.RotateOnAxis(Vector3.right, TNTRotationSpeed), ref TNTRotationCoroutine);

		while(t < 1.0f)
		{
			a = character.TNT.transform.position;
			b = line.Lerp(t);
			d = (b - a).normalized;
			character.TNT.transform.position = b;
			character.TNT.transform.rotation = Quaternion.LookRotation(d);

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		character.TNT.ActivateHitBoxes(true);
		b = mainDeckPath.Lerp(0.5f);
		d = b - character.TNT.transform.position;
		float sqrDistance = (0.1f * 0.1f);

		while(d.sqrMagnitude < sqrDistance)
		{
			character.TNT.transform.position += (d * sidewaysMovementSpeed * Time.deltaTime);
			character.TNT.transform.rotation = Quaternion.LookRotation(d);
			d = b - character.TNT.transform.position;
			yield return null;
		}

		b = line == ShantySceneController.Instance.leftStairPath ? mainDeckPath.b : mainDeckPath.a;
		d = b - character.TNT.transform.position;

		while(tntActive)
		{
			character.TNT.transform.position += (d.normalized * sidewaysMovementSpeed * Time.deltaTime);
			character.TNT.transform.rotation = Quaternion.LookRotation(d);
			d = b - character.TNT.transform.position;

			if(d.sqrMagnitude <= sqrDistance)
			b = b == mainDeckPath.a ? mainDeckPath.b : mainDeckPath.a;

			yield return null;
		}

		character.TNT.DispatchCoroutine(ref TNTRotationCoroutine);
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

		character.EnableHurtBoxes(false);

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
					clipA = character.throwBombAnimation;
				break;

				case ID_WAYPOINTSPAIR_DECK:
					pair = ShantySceneController.Instance.deckWaypointsPair;
					clipA = character.throwBombAnimation;
				break;

				case ID_WAYPOINTSPAIR_STAIR_LEFT:
					pair = ShantySceneController.Instance.leftStairWaypointsPair;
					clipA = character.throwBarrelAnimation;
					line = ShantySceneController.Instance.leftStairPath;
				break;

				case ID_WAYPOINTSPAIR_STAIR_RIGHT:
					pair = ShantySceneController.Instance.rightStairWaypointsPair;
					clipA = character.throwBarrelAnimation;
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
					character.EnableHurtBoxes(true);
				}

				t += (Time.deltaTime * inverseDuration);

				yield return null;
			}

			t = 0.0f;
			toggled = false;

			character.animation.CrossFade(clipA);
			wait.ChangeDurationAndReset(clipA.length);

			while(wait.MoveNext()) yield return null;

			character.animation.CrossFade(character.idleAnimation);
			wait.ChangeDurationAndReset(waitBeforeWaypointReturn);

			while(wait.MoveNext()) yield return null;

			while(t < 1.0f)
			{
				transform.position = Vector3.Lerp(b, a, t);

				if(!toggled && t >= progressToToggleHurtBoxes)
				{
					toggled = true;
					character.EnableHurtBoxes(false);
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

						character.animation.CrossFade(character.idleAnimation);
						wait.ChangeDurationAndReset(strongAttackCooldownDuration);

						while(wait.MoveNext()) yield return null;

						enteredAttackRadius = false;
					}
					else
					{
						direction = direction.x > 0.0f ? Vector3.right : Vector3.left;
						character.Move(direction);
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
				character.Move(direction);
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


		character.animation.CrossFade(character.normalAttackAnimation);
		animationState = character.animation.GetAnimationState(character.normalAttackAnimation);
		character.sword.ActivateHitBoxes(true);
		wait.ChangeDurationAndReset(animationState.length);
		character.dashAbility.Dash(direction);

		while(wait.MoveNext()) yield return null;

		character.sword.ActivateHitBoxes(false);

		wait.ChangeDurationAndReset(regressionDuration);

		while(character.dashAbility.state != DashState.Unactive) yield return null;

		while(wait.MoveNext())
		{
			character.Move(-direction);
			yield return null;
		}

		normalAttackCooldown.Begin();
	}

	/// <summary>Strong Attack's Routine.</summary>
	private IEnumerator StrongAttackRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		AnimationState animationState = null;

		character.animation.CrossFade(character.strongAttackAnimation);
		animationState = character.animation.GetAnimationState(character.strongAttackAnimation);
		wait.ChangeDurationAndReset(character.strongAttackAnimation.length);

		while(wait.MoveNext()) yield return null;

		strongAttackCooldown.Begin();
	}

	/// <summary>Jump's Routine.</summary>
	private IEnumerator JumpAttackRoutine()
	{
		Vector3 direction = Game.mateo.transform.position - transform.position;
		direction.y = 0.0f;
		direction.Normalize();
		character.sword.ActivateHitBoxes(true);

		while(!character.jumpAbility.HasStates(JumpAbility.STATE_ID_FALLING))
		{
			character.jumpAbility.Jump(Vector3.up);
			character.Move(direction, 2.0f);
			yield return null;
		}

		while(!character.jumpAbility.HasStates(JumpAbility.STATE_ID_GROUNDED))
		{
			character.Move(direction, 2.0f);
			yield return null;
		}

		character.sword.ActivateHitBoxes(false);
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