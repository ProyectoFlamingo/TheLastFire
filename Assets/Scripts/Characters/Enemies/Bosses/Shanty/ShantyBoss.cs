using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

using Random = UnityEngine.Random;

namespace Flamingo
{
[RequireComponent(typeof(JumpAbility))]
[RequireComponent(typeof(DashAbility))]
[RequireComponent(typeof(RigidbodyMovementAbility))]
public class ShantyBoss : Boss
{
	public const int ID_WAYPOINTSPAIR_HELM = 0; 																					/// <summary>Helm's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_DECK = 1; 																					/// <summary>Deck's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_STAIR_LEFT = 2; 																				/// <summary>Left Stair's Waypoints' Pair ID.</summary>
	public const int ID_WAYPOINTSPAIR_STAIR_RIGHT = 3; 																				/// <summary>Right Stair's Waypoints' Pair ID.</summary>

	[Space(10f)]
	[Header("Shanty's Attributes:")]
	[Space(5f)]
	[SerializeField] private ShantyShip _ship; 																						/// <summary>Shanty's Ship.</summary>
	[Space(5f)]
	[Header("Weapons' Atrributes:")]
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private ContactWeapon _sword; 												/// <summary>Shanty's Sword.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private Transform _falseSword; 											/// <summary>False Sword's Reference (the one stuck to the rigging).</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private VAssetReference _TNTReference; 									/// <summary>TNT's Reference.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private VAssetReference _bombReference; 									/// <summary>Bomb's Reference.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private VAssetReference _bouncingBombReference; 							/// <summary>Bouncing Bomb's Reference.</summary>
	[Space(5f)]
	[Header("Tennis' Attributes:")]
	[TabGroup("BattleGroup", "Tennis' Phase")][SerializeField][Range(0.0f, 1.0f)] private float _windowPercentage; 					/// <summary>Time-window before swinging sword (to hit bomb).</summary>
	[Space(5f)]
	[Header("Stage 1 Bomb's Attributes:")]
	[TabGroup("ExplosiveGroup", "Bombs' Attributes")][SerializeField] private float _bombProjectionTime; 							/// <summary>Bomb's Projection Time.</summary>
	[TabGroup("ExplosiveGroup", "Bombs' Attributes")][SerializeField][Range(0.0f, 1.0f)] private float _bombProjectionPercentage; 	/// <summary>Bomb Projection Time's Percentage.</summary>
	[Space(5f)]
	[Header("Stage 2 Bomb's Attributes:")]
	[TabGroup("ExplosiveGroup", "Bombs' Attributes")][SerializeField] private float _bouncingBombProjectionTime; 					/// <summary>Bouncing Bomb's Projection Time.</summary>
	[Space(5f)]
	[Header("Stage 1 TNT's Attributes:")]
	[TabGroup("ExplosiveGroup", "TNT's Attributes")][SerializeField] private VAssetReference _stage1ExplodableReference; 			/// <summary>Explodable's Reference for TNT on Stage 1.</summary>
	[TabGroup("ExplosiveGroup", "TNT's Attributes")][SerializeField] private float _stage1TNTFuseDuration; 							/// <summary>Fuse Duration for TNT on Stage 1.</summary>
	[TabGroup("ExplosiveGroup", "TNT's Attributes")][SerializeField] private float _TNTProjectionTime; 								/// <summary>TNT's Projection Time.</summary>
	[TabGroup("ExplosiveGroup", "TNT's Attributes")][SerializeField][Range(0.0f, 1.0f)] private float _TNTProjectionPercentage; 	/// <summary>TNT Projection Time's Percentage.</summary>
	[TabGroup("ExplosiveGroup", "TNT's Attributes")][SerializeField][Range(0.0f, 1.0f)] private float _TNTTimeScaleChangeProgress; 	/// <summary>TNT parabolas' progress necessary to slow down time.</summary>
	[TabGroup("ExplosiveGroup", "TNT's Attributes")][SerializeField][Range(0.0f, 1.0f)] private float _TNTThrowTimeScale; 			/// <summary>Time Scale when throwing TNT.</summary>
	[Space(5f)]
	[TabGroup("Health")][SerializeField][Range(0.0f, 1.0f)] private float _stage1HealthPercentageLimit; 							/// <summary>Health Limit's Percentage for TNT.</summary>
	[TabGroup("Health")][SerializeField][Range(0.0f, 1.0f)] private float _stage2HealthPercentageLimit; 							/// <summary>Health Limit's Percentage for TNT.</summary>
	[Space(5f)]
	[Header("Stage 2's TNT's Attributes:")]
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private VAssetReference _stage2ExplodableReference; 						/// <summary>Explodable's Reference for TNT on Stage 2.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private float _stage2TNTFuseDuration; 										/// <summary>Fuse Duration for TNT on Stage 2.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private float _stairParabolaTime; 											/// <summary>Duration from throw to beginning of stair.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private float _stairSlideDuration; 										/// <summary>Stair Slide's Duration.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private float _sidewaysMovementSpeed; 										/// <summary>Sideways' Movement Speed.</summary>
	[TabGroup("WeaponGroup", "Weapons")][SerializeField] private float _TNTRotationSpeed; 											/// <summary>TNT's Rotation Angular Speed.</summary>
	[Space(5f)]
	[Header("Whack-A-Mole's Attributes:")]
	[TabGroup("BattleGroup", "Whack-A-Mole")][SerializeField] private float _vectorPairInterpolationDuration; 						/// <summary>Interpolation duration for Whack-A-Mole's Waypoints.</summary>
	[TabGroup("BattleGroup", "Whack-A-Mole")][SerializeField] private float _waitBeforeWaypointReturn; 								/// <summary>Wait before Waypoint's Return.</summary>
	[TabGroup("BattleGroup", "Whack-A-Mole")][SerializeField][Range(0.0f, 1.0f)] private float _progressToToggleHurtBoxes; 			/// <summary>Process percentage on the interpolation to toggle the Hurt-Boxes.</summary>
	[Space(5f)]
	[Header("Duel's Attributes:")]
	[TabGroup("BattleGroup", "Duel")][SerializeField] private FloatRange _attackRadiusRange; 										/// <summary>Attacks' Radius Range.</summary>
	[TabGroup("BattleGroup", "Duel")][SerializeField] private FloatRange _strongAttackWaitInterval; 								/// <summary>Strong Attack's Wait Interval.</summary>
	[TabGroup("BattleGroup", "Duel")][SerializeField] private float _movementSpeed; 												/// <summary>Movement's Speed.</summary>
	[TabGroup("BattleGroup", "Duel")][SerializeField] private float _rotationSpeed; 												/// <summary>Rotation's Speed.</summary>
	[TabGroup("BattleGroup", "Duel")][SerializeField] private float _regressionDuration; 											/// <summary>Regression's Duration.</summary>
	[TabGroup("BattleGroup", "Duel")][SerializeField] private float _attackDistance; 												/// <summary>Attack's Distance.</summary>
	[TabGroup("BattleGroup", "Duel")][SerializeField] private float _normalAttackCooldownDuration; 									/// <summary>Normal Attack Cooldown's Duration.</summary>
	[TabGroup("BattleGroup", "Duel")][SerializeField] private float _strongAttackCooldownDuration; 									/// <summary>Strong Attack Cooldown's Duration.</summary>
	[Space(5f)]
	[Header("Inmunities:")]
	[TabGroup("Health")][SerializeField] private GameObjectTag[] _stage1Inmunities; 												/// <summary>Inmunities on Stage 1.</summary>
	[TabGroup("Health")][SerializeField] private GameObjectTag[] _stage2Inmunities; 												/// <summary>Inmunities on Stage 2.</summary>
	[TabGroup("Health")][SerializeField] private GameObjectTag[] _defaultBombImpactTags; 											/// <summary>Default Impact Tags for Bombs.</summary>
	[TabGroup("Health")][SerializeField] private GameObjectTag[] _wickOnBombImpactTags; 											/// <summary>Wick-On Impact Tags for Bombs.</summary>
	[Space(5f)]
	[Header("Shanty's Animations:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential[] _tiedCredentials; 										/// <summary>Tied Animations.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _untiedCredential; 											/// <summary>Untied's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _idleCredential; 											/// <summary>Idle's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _laughCredential; 											/// <summary>Laugh's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _tauntCredential; 											/// <summary>Taun's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _tiredCredential; 											/// <summary>Tired's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _shootCredential; 											/// <summary>Shoot's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _shootToAirCredential; 										/// <summary>Shoot To Air's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _throwBarrelCredential; 									/// <summary>Throw Barrel's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _throwBombCredential; 										/// <summary>Throw Bomb's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _tennisHitCredential; 										/// <summary>tennis Hit's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _hitBombCredential; 										/// <summary>Hit Bomb's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _hitBarrelCredential; 										/// <summary>Hit Barrel's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _hitSwordCredential; 										/// <summary>Hit Sword's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _cryCredential; 											/// <summary>Cry's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalAttackCredential; 									/// <summary>Normal Attack's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _strongAttackCredential; 									/// <summary>Strong Attack's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _walkingCredential; 										/// <summary>Walking AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _backStepCredential; 										/// <summary>Back-Setp AnimatorCredential.</summary>
	private Coroutine coroutine; 																									/// <summary>Coroutine's Reference.</summary>
	private Coroutine TNTRotationCoroutine; 																						/// <summary>TNT's Rotation Coroutine's Reference.</summary>
	private Behavior attackBehavior; 																								/// <summary>Attack's Behavior [it is behavior so it can be paused].</summary>
	private Projectile _bomb; 																										/// <summary>Bomb's Reference.</summary>
	private Projectile _TNT; 																										/// <summary>TNT's Reference.</summary>
	private JumpAbility _jumpAbility; 																								/// <summary>JumpAbility's Component.</summary>
	private DashAbility _dashAbility; 																								/// <summary>DashAbility's Component.</summary>
	private RigidbodyMovementAbility _movementAbility; 																				/// <summary>MovementAbility's Component.</summary>
	private Cooldown _normalAttackCooldown; 																						/// <summary>Normal Attack's Cooldown.</summary>
	private Cooldown _strongAttackCooldown; 																						/// <summary>Strong Attack's Cooldown.</summary>
	private Line _line; 																											/// <summary>Current Stair's Line.</summary>
	private bool _tntActive; 																										/// <summary>Is the TNT Coroutine's Running?.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ship property.</summary>
	public ShantyShip ship
	{
		get { return _ship; }
		set { _ship = value; }
	}

	/// <summary>Gets bombReference property.</summary>
	public VAssetReference bombReference { get { return _bombReference; } }

	/// <summary>Gets bouncingBombReference property.</summary>
	public VAssetReference bouncingBombReference { get { return _bouncingBombReference; } }

	/// <summary>Gets TNTReference property.</summary>
	public VAssetReference TNTReference { get { return _TNTReference; } }

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

	/// <summary>Gets sword property.</summary>
	public ContactWeapon sword { get { return _sword; } }

	/// <summary>Gets falseSword property.</summary>
	public Transform falseSword { get { return _falseSword; } }

	/// <summary>Gets stage1Inmunities property.</summary>
	public GameObjectTag[] stage1Inmunities { get { return _stage1Inmunities; } }

	/// <summary>Gets stage2Inmunities property.</summary>
	public GameObjectTag[] stage2Inmunities { get { return _stage2Inmunities; } }

	/// <summary>Gets defaultBombImpactTags property.</summary>
	public GameObjectTag[] defaultBombImpactTags { get { return _defaultBombImpactTags; } }

	/// <summary>Gets wickOnBombImpactTags property.</summary>
	public GameObjectTag[] wickOnBombImpactTags { get { return _wickOnBombImpactTags; } }

	/// <summary>Gets tiedCredentials property.</summary>
	public AnimatorCredential[] tiedCredentials { get { return _tiedCredentials; } }

	/// <summary>Gets untiedCredential property.</summary>
	public AnimatorCredential untiedCredential { get { return _untiedCredential; } }

	/// <summary>Gets idleCredential property.</summary>
	public AnimatorCredential idleCredential { get { return _idleCredential; } }

	/// <summary>Gets laughCredential property.</summary>
	public AnimatorCredential laughCredential { get { return _laughCredential; } }

	/// <summary>Gets tauntCredential property.</summary>
	public AnimatorCredential tauntCredential { get { return _tauntCredential; } }

	/// <summary>Gets tiredCredential property.</summary>
	public AnimatorCredential tiredCredential { get { return _tiredCredential; } }

	/// <summary>Gets shootToAirCredential property.</summary>
	public AnimatorCredential shootToAirCredential { get { return _shootToAirCredential; } }

	/// <summary>Gets shootCredential property.</summary>
	public AnimatorCredential shootCredential { get { return _shootCredential; } }

	/// <summary>Gets throwBarrelCredential property.</summary>
	public AnimatorCredential throwBarrelCredential { get { return _throwBarrelCredential; } }

	/// <summary>Gets throwBombCredential property.</summary>
	public AnimatorCredential throwBombCredential { get { return _throwBombCredential; } }

	/// <summary>Gets tennisHitCredential property.</summary>
	public AnimatorCredential tennisHitCredential { get { return _tennisHitCredential; } }

	/// <summary>Gets hitBombCredential property.</summary>
	public AnimatorCredential hitBombCredential { get { return _hitBombCredential; } }

	/// <summary>Gets hitBarrelCredential property.</summary>
	public AnimatorCredential hitBarrelCredential { get { return _hitBarrelCredential; } }

	/// <summary>Gets hitSwordCredential property.</summary>
	public AnimatorCredential hitSwordCredential { get { return _hitSwordCredential; } }

	/// <summary>Gets cryCredential property.</summary>
	public AnimatorCredential cryCredential { get { return _cryCredential; } }

	/// <summary>Gets normalAttackCredential property.</summary>
	public AnimatorCredential normalAttackCredential { get { return _normalAttackCredential; } }

	/// <summary>Gets strongAttackCredential property.</summary>
	public AnimatorCredential strongAttackCredential { get { return _strongAttackCredential; } }

	/// <summary>Gets backStepCredential property.</summary>
	public AnimatorCredential backStepCredential { get { return _backStepCredential; } }

	/// <summary>Gets walkingCredential property.</summary>
	public AnimatorCredential walkingCredential { get { return _walkingCredential; } }

	/// <summary>Gets and Sets bomb property.</summary>
	public Projectile bomb
	{
		get { return _bomb; }
		set { _bomb = value; }
	}

	/// <summary>Gets and Sets TNT property.</summary>
	public Projectile TNT
	{
		get { return _TNT; }
		set { _TNT = value; }
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
		normalAttackCooldown = new Cooldown(this, normalAttackCooldownDuration);
		strongAttackCooldown = new Cooldown(this, strongAttackCooldownDuration);
		animatorController.animator.SetLayerWeight(locomotionAnimationLayer, 0.0f);

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
	public void Move(Vector3 direction, float scale = 1.0f)
	{
		Debug.Log("[ShantyBoss] Dude, move (" + direction + ")...");

		bool move = direction.sqrMagnitude > 0.0f && Mathf.Abs(scale) > 0.0f;
		
		animatorController.animator.SetLayerWeight(locomotionAnimationLayer, move ? 1.0f : 0.0f);
		animator.SetFloat(leftAxisXCredential, 1.0f);
		movementAbility.Move(direction, scale, Space.World);
	}

	/// <summary>Activates/Deactivates Sword and False Sword.</summary>
	/// <param name="_activate">Activate Sword? true by default.</param>
	public void ActivateSword(bool _activate = true)
	{
		sword.owner = gameObject;
		sword.ActivateHitBoxes(_activate);
		
		if(_activate)
		{
			sword.transform.position = skeleton.rightHand.position;
			sword.transform.rotation = skeleton.rightHand.rotation;
			sword.transform.parent = skeleton.rightHand;
		}
		else
		{
			sword.transform.parent = meshParent;
			sword.transform.localPosition = Vector3.zero;
			sword.transform.rotation = Quaternion.identity;
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
#endregion

#region AnimationCallbacks:
	/// <summary>CrossFades To Given Animation.</summary>
	/// <param name="onAnimationEnds">Optional Callback invoked when Animation Ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool CrossFadeToAnimation(AnimatorCredential hash, Action onAnimationEnds = null)
	{
		//Debug.Log("[ShantyBoss] Cross-Fading To: " + hash.tag);
		animatorController.CancelCrossFading(0);
		return animatorController.CrossFadeAndWait(hash, clipFadeDuration, 0, 0.0f, 0.0f, onAnimationEnds);
	}

	/// <summary>Goes to Idle Animation.</summary>
	/// <param name="onIdleEnds">Optional Callback invoked when Idle's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToIdleAnimation(Action onIdleEnds = null)
	{
		return CrossFadeToAnimation(idleCredential, onIdleEnds);
	}

	/// <summary>Goes to Untie Animation.</summary>
	/// <param name="onUntieEnds">Optional Callback invoked when Untie's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToUntieAnimation(Action onUntieEnds = null)
	{
		return CrossFadeToAnimation(untiedCredential, onUntieEnds);
	}

	/// <summary>Goes to TennisHit Animation.</summary>
	/// <param name="onTennisHitEnds">Optional Callback invoked when TennisHit's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToTennisHitAnimation(Action onTennisHitEnds = null)
	{
		return CrossFadeToAnimation(tennisHitCredential, onTennisHitEnds);
	}

	/// <summary>Goes to ThrowBomb Animation.</summary>
	/// <param name="onThrowBombEnds">Optional Callback invoked when ThrowBomb's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToThrowBombAnimation(Action onThrowBombEnds = null)
	{
		return CrossFadeToAnimation(throwBombCredential, onThrowBombEnds);
	}

	/// <summary>Goes to ThrowBarrel Animation.</summary>
	/// <param name="onThrowBarrelEnds">Optional Callback invoked when ThrowBarrel's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToThrowBarrelAnimation(Action onThrowBarrelEnds = null)
	{
		return CrossFadeToAnimation(throwBarrelCredential, onThrowBarrelEnds);
	}

	/// <summary>Goes to TennisHit Animation.</summary>
	/// <param name="obj">Cause of Damage's GameObject, null by  default.</param>
	/// <param name="onTennisHitEnds">Optional Callback invoked when TennisHit's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToDamageAnimation(GameObject obj = null, Action onTennisHitEnds = null)
	{
		AnimatorCredential hash = default(AnimatorCredential);

		if(obj  == null) hash = hitSwordCredential;
		else
		{
			if(obj.CompareTag(Game.data.playerWeaponTag))
			{
				hash = hitSwordCredential;

			} else if(obj.CompareTag(Game.data.playerProjectileTag))
			{
				hash = hitBombCredential;

			} else if(obj.CompareTag(Game.data.explodableTag))
			{
				hash = hitBarrelCredential;
			}
		}

		return CrossFadeToAnimation(hash, onTennisHitEnds);
	}

	/// <summary>Goes to Cry Animation.</summary>
	/// <param name="onCryEnds">Optional Callback invoked when Cry's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToCryAnimation(Action onCryEnds = null)
	{
		return CrossFadeToAnimation(cryCredential, onCryEnds);
	}

	/// <summary>Goes to Normal Attack Animation.</summary>
	/// <param name="onNormalAttackEnds">Optional Callback invoked when Normal Attack's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToNormalAttackAnimation(Action onNormalAttackEnds = null)
	{
		return CrossFadeToAnimation(normalAttackCredential, onNormalAttackEnds);
	}

	/// <summary>Goes to Strong Attack Animation.</summary>
	/// <param name="onStrongAttackEnds">Optional Callback invoked when Strong Attack's animation ends.</param>
	/// <returns>Whether the Cross-Fade could be made.</returns>
	public bool GoToStrongAttackAnimation(Action onStrongAttackEnds = null)
	{
		return CrossFadeToAnimation(strongAttackCredential, onStrongAttackEnds);
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

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public override void OnStatesAdded(int _state)
	{
		if((_state | IDs.STATE_IDLE) == _state)
		{
			//GoToIdleAnimation();

		} else if((_state | IDs.STATE_ATTACKING_0) == _state)
		{
			//BeginAttackRoutine();

		} else if((_state | IDs.STATE_HURT) == _state)
		{
			//this.RemoveStates(IDs.STATE_ATTACKING_0);
		}

		base.OnStatesAdded(_state);
	}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public override void OnStatesRemoved(int _state)
	{
		base.OnStatesRemoved(_state);
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
						if(_object == null) return;
						GoToDamageAnimation(_object);
						this.AddStates(IDs.STATE_HURT);
					break;
				}
			break;

			case HealthEvent.FullyDepleted:
				if(currentStage >= stages)
				{
					//eventsHandler.InvokeIDEvent(ID_EVENT_BOSS_DEATHROUTINE_BEGINS);
					this.DispatchCoroutine(ref behaviorCoroutine);
					this.DispatchCoroutine(ref coroutine);
					GoToCryAnimation();
					this.ChangeState(IDs.STATE_DEAD);
					BeginDeathRoutine();
				}
			break;

			case HealthEvent.HitStunEnds:
				OnHitStunEnds();
			break;
		}
	}

	/// <summary>Callback invoked when Hit-stun ends.</summary>
	private void OnHitStunEnds()
	{
		this.RemoveStates(IDs.STATE_HURT | IDs.STATE_ATTACKING_0);
	}
#endregion

	[Button("TEST Unsheath Sword")]
	/// <summary>Tests Sword.</summary>
	private void TESTUnsheathSword()
	{
		ActivateSword();
	}

	[Button("TEST Sheath Sword")]
	/// <summary>Tests Sword.</summary>
	private void TESTSheathSword()
	{
		ActivateSword(false);
	}

	/// <summary>Waits for Cross-Fade to occur.</summary>
	/// <param name="hash">Animation's Hash.</param>
	/// <param name="onCrossFadeEnds">Optional Callback invoked when te cross-fade ends [null by default].</param>
	public IEnumerator WaitForCrossFade(int hash, Action onCrossFadeEnds = null)
	{
		bool crossFadeEnds = false;
		Action CrossFadeEnds = ()=>
		{
			if(onCrossFadeEnds != null) onCrossFadeEnds();
		};

		if(!animatorController.CrossFadeAndWait(hash, clipFadeDuration, 0, Mathf.NegativeInfinity, 0.0f, CrossFadeEnds)) yield break;

		while(!crossFadeEnds) yield return null;
	}

	/// <summary>Waits for Normal-Attack's Cross-Fade to occur.</summary>
	/// <param name="hash">Animation's Hash.</param>
	/// <param name="onCrossFadeEnds">Optional Callback invoked when te cross-fade ends [null by default].</param>
	public IEnumerator WaitForNormalAttackCrossFade(Action onCrossFadeEnds = null)
	{
		IEnumerator wait = WaitForCrossFade(normalAttackCredential, onCrossFadeEnds);
		while(wait.MoveNext()) yield return null;
	}

	/// <summary>Waits for Strong-Attack's Cross-Fade to occur.</summary>
	/// <param name="hash">Animation's Hash.</param>
	/// <param name="onCrossFadeEnds">Optional Callback invoked when te cross-fade ends [null by default].</param>
	public IEnumerator WaitForStrongAttackCrossFade(Action onCrossFadeEnds = null)
	{
		IEnumerator wait = WaitForCrossFade(strongAttackCredential, onCrossFadeEnds);
		while(wait.MoveNext()) yield return null;
	}

	/// <summary>Death's Routine.</summary>
	/// <param name="onDeathRoutineEnds">Callback invoked when the routine ends.</param>
	protected override IEnumerator DeathRoutine(Action onDeathRoutineEnds)
	{
		GoToCryAnimation();
		yield return null;
		if(onDeathRoutineEnds != null) onDeathRoutineEnds();
	}
}
}