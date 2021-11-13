using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public enum StareTarget
{
	Boss,
	Player
}

public enum MeditationType
{
	Normal,
	Sword,
	Fire,
	Jump
}

namespace Flamingo
{
[RequireComponent(typeof(RigidbodyMovementAbility))]
[RequireComponent(typeof(JumpAbility))]
[RequireComponent(typeof(DashAbility))]
[RequireComponent(typeof(RotationAbility))]
[RequireComponent(typeof(ShootChargedProjectile))]
[RequireComponent(typeof(TransformDeltaCalculator))]
[RequireComponent(typeof(SensorSystem2D))]
[RequireComponent(typeof(WallEvaluator))]
[RequireComponent(typeof(AnimationAttacksHandler))]
[RequireComponent(typeof(SlopeEvaluator))]
[RequireComponent(typeof(VCameraTarget))]
public class Mateo : Character
{
	public const int ID_INITIALPOSE_STARRINGATPLAYER = 0; 								/// <summary>Starring At Player's Initial Pose's ID.</summary>
	public const int ID_INITIALPOSE_STARRINGATBACKGROUND = 2; 							/// <summary>Starring At Background's Initial Pose's ID.</summary>
	public const int ID_INITIALPOSE_MEDITATING = 1; 									/// <summary>Meditation Initial Pose's ID.</summary>
	public const int ID_STATE_INITIALPOSE = 1 << 5; 									/// <summary>Initial Pose's State ID.</summary>
	public const int ID_STATE_MEDITATING = 1 << 5; 										/// <summary>Meditating's State ID.</summary>
	public const int ID_EVENT_INITIALPOSE_BEGINS = 0; 									/// <summary>Mateo Initial-Pose-Begins's Event ID.</summary>
	public const int ID_EVENT_INITIALPOSE_ENDED = 1; 									/// <summary>Mateo Initial-Pose-Finished's Event ID.</summary>
	public const int ID_EVENT_MEDITATION_BEGINS = 2; 									/// <summary>Meditation Begins' Event.</summary>
	public const int ID_EVENT_MEDITATION_ENDS = 3; 										/// <summary>Meditation Ends' Event.</summary>
	public const int ID_EVENT_HURT = 4; 												/// <summary>Mateo's Hurt Event.</summary>
	public const int ID_EVENT_DEAD = 5; 												/// <summary>Mateo's Dead Event.</summary>

	[SerializeField] private GameObject hurtBox; 										/// <summary>HurtBox's Container.</summary>
	[Header("Rotations:")]
	[SerializeField] private EulerRotation _stareAtBossRotation; 						/// <summary>Stare at Boss's Rotation.</summary>
	[SerializeField] private EulerRotation _stareAtPlayerRotation; 						/// <summary>SSStare At Player's Rotation.</summary>
	[Header("Hands:")]
	[SerializeField] private Transform _leftHand; 										/// <summary>Left Hand's reference [Fire caster].</summary>
	[SerializeField] private Transform _rightHand; 										/// <summary>Right Hand's reference [Sword holder].</summary>
	[Space(5f)]
	[SerializeField] private float _postMeditationDuration; 							/// <summary>Post-Meditation's Duration.</summary>
	[Space(5f)]
	[Header("Sword's Attributes:")]
	[SerializeField] private Sword _sword; 												/// <summary>Mateo's Sword.</summary>
	[SerializeField] private int _groundedNeutralComboIndex; 							/// <summary>Grounded Neutral Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField] private int _groundedDirectionalComboIndex; 						/// <summary>Grounded Directional Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField] private int _airNeutralComboIndex; 								/// <summary>Air Neutral Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField] private int _airDirectionalComboIndex; 							/// <summary>Air Directional Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _animationStateProgress; 							/// <summary>Minimum Animation State Progress to open the next attack's window.</summary>
	[SerializeField] private float _attackDurationWindow; 								/// <summary>Duration Window's for next sword attack.</summary>
	[Space(5f)]
	[SerializeField] private FloatRange _directionalThresholdX; 						/// <summary>Directional Threhold on the X's Axis to perform directional attacks.</summary>
	[SerializeField] private FloatRange _directionalThresholdY; 						/// <summary>Directional Threhold on the Y's Axis to perform directional attacks.</summary>
	[Space(5f)]
	[SerializeField] private float _gravityScale; 										/// <summary>Gravity Scale applied when attacking.</summary>
	[SerializeField] private int _scaleChangePriority; 									/// <summary>Gravity Scale's Change Priority.</summary>
	[Space(5f)]
	[SerializeField] private float _jumpingMovementScale; 								/// <summary>Movement's Scale when Mateo is Jumping.</summary>
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _dashXThreshold; 									/// <summary>Minimum left axis' X [absolute] value to be able to perform dash.</summary>
	[Space(5f)]
	[SerializeField] private Transform _animatorParent; 								/// <summary>Animator's Parent.</summary>
	[SerializeField] private Animator _animator; 										/// <summary>Animator's Component.</summary>
	[Space(5f)]
	[Header("AnimatorController's Parameters:")]
	[SerializeField] private AnimatorCredential _leftAxisXCredential; 					/// <summary>Left Axis X's Animator Credential.</summary>
	[SerializeField] private AnimatorCredential _leftAxisYCredential; 					/// <summary>Left Axis Y's Animator Credential.</summary>
	[SerializeField] private AnimatorCredential _rightAxisXCredential; 					/// <summary>Right Axis X's Animator Credential.</summary>
	[SerializeField] private AnimatorCredential _rightAxisYCredential; 					/// <summary>Right Axis Y's Animator Credential.</summary>
	[Space(5f)]
	[Header("Animation Layers:")]
	[SerializeField] private int _mainAnimationLayer; 									/// <summary>Main's Animation Layer.</summary>
	[SerializeField] private int _locomotionAnimationLayer; 							/// <summary>Locomotion's Animation Layer.</summary>
	[SerializeField] private int _fireConjuringAnimationLayer; 							/// <summary>Fire Conjuring's Animation Layer.</summary>
	[SerializeField] private int _jumpingAnimationLayer; 								/// <summary>Jumping's Animation Layer.</summary>
	[SerializeField] private int _attackAnimationLayer; 								/// <summary>Attack's Animation Layer.</summary>
	[Space(5f)]
	[Header("Animation Clips' Credentials:")]
	[SerializeField] private AnimatorCredential _idleCredential; 						/// <summary>Idle's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _crouchCredential; 						/// <summary>Crouch's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _normalMeditationCredential; 			/// <summary>Normal Meditaiton's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _brakeCredential; 						/// <summary>Brake's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _hitCredential; 						/// <summary>Hit's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _crashCredential; 						/// <summary>Crash's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _deadCredential; 						/// <summary>Dead's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _swordMeditationCredential; 			/// <summary>Sword Meditaiton's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _fireMeditationCredential; 				/// <summary>Fire Meditaiton's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _normalStandingCredential; 				/// <summary>Normal Standing's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _jumpStandingCredential; 				/// <summary>Jump Standing's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _normalJumpCredential; 					/// <summary>Normal Jump's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _additionalJumpCredential; 				/// <summary>Additional Jump's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _fallingCredential; 					/// <summary>Falling's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _softLandingCredential; 				/// <summary>Soft Landing's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _hardLandingCredential; 				/// <summary>Hard Landing's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _fireConjuringCredential; 				/// <summary>Fire Conjuring's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _fireMaxChargedCredential; 				/// <summary>Fire's Max Charged's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _fireThrowCredential; 					/// <summary>Fire Throw's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _swordObtentionCredential; 				/// <summary>Sword Obtention's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _fireObtentionCredential; 				/// <summary>Fire Obtention's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _groundSwordAttackCredential; 			/// <summary>Ground Sword Attack's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _normalJumpSwordAttackCredential; 		/// <summary>Normal Jump Sowrd Attack's AnimatorCredential.</summary>
	[SerializeField] private AnimatorCredential _additionalJumpSwordAttackCredential; 	/// <summary>Additional Jump Sowrd Attack's AnimatorCredential.</summary>
	[Space(5f)]
	[SerializeField] private TrailRenderer _extraJumpTrailRenderer; 					/// <summary>Extra-Jump's Trail Renderer.</summary>
	[Space(5f)]
	[SerializeField] private ParticleEffect _swordParticleEffect; 						/// <summary>Sword's Particle Effect when slashing.</summary>
	[Header("Meditation's Attributes:")]
	[SerializeField] private float _meditationWaitDuration; 							/// <summary>Meditation Wait's Duration.</summary>
	[SerializeField] private float _normalStandingAdditionalWait; 						/// <summary>Normal Meditation's Standing Additional Wait Duration.</summary>
	private float _meditationWaitTime; 													/// <summary>Current Meditation's Time.</summary>
	private RigidbodyMovementAbility _movementAbility; 									/// <summary>RigidbodyMovementAbility's Component.</summary>
	private RotationAbility _rotationAbility; 											/// <summary>RotationAbility's Component.</summary>
	private JumpAbility _jumpAbility; 													/// <summary>JumpAbility's Component.</summary>
	private ShootChargedProjectile _shootProjectile; 									/// <summary>ShootChargedProjectile's Component.</summary>
	private DashAbility _dashAbility; 													/// <summary>DashAbility's Component.</summary>
	private TransformDeltaCalculator _deltaCalculator; 									/// <summary>TransformDeltaCalculator's Component.</summary>
	private SensorSystem2D _sensorSystem; 												/// <summary>SensorSystem2D's Component.</summary>
	private WallEvaluator _wallEvaluator; 												/// <summary>WallEvaluator's Component.</summary>
	private AnimationAttacksHandler _attacksHandler; 									/// <summary>AnimationAttacksHandler's Component.</summary>
	private SlopeEvaluator _slopeEvaluator; 											/// <summary>SlopeEvaluator's Component.</summary>
	private VCameraTarget _cameraTarget; 												/// <summary>VCameraTarget's Component.</summary>
	private Vector3 _orientation; 														/// <summary>Mateo's Orientation.</summary>
	private Vector2 _leftAxes; 															/// <summary>Left Axes' Value.</summary>
	private Cooldown _postInitialPoseCooldown; 											/// <summary>Post-Meditation's Cooldown.</summary>
	private Coroutine meditationStanding; 												/// <summary>Meditation's Standing Coroutine's Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets stareAtBossRotation property.</summary>
	public EulerRotation stareAtBossRotation { get { return _stareAtBossRotation; } }

	/// <summary>Gets stareAtPlayerRotation property.</summary>
	public EulerRotation stareAtPlayerRotation { get { return _stareAtPlayerRotation; } }

	/// <summary>Gets leftHand property.</summary>
	public Transform leftHand { get { return _leftHand; } }

	/// <summary>Gets rightHand property.</summary>
	public Transform rightHand { get { return _rightHand; } }

	/// <summary>Gets animatorParent property.</summary>
	public Transform animatorParent { get { return _animatorParent; } }

	/// <summary>Gets sword property.</summary>
	public Sword sword { get { return _sword; } }

	/// <summary>Gets postMeditationDuration property.</summary>
	public float postMeditationDuration { get { return _postMeditationDuration; } }

	/// <summary>Gets animationStateProgress property.</summary>
	public float animationStateProgress { get { return _animationStateProgress; } }

	/// <summary>Gets attackDurationWindow property.</summary>
	public float attackDurationWindow { get { return _attackDurationWindow; } }

	/// <summary>Gets gravityScale property.</summary>
	public float gravityScale { get { return _gravityScale; } }

	/// <summary>Gets dashXThreshold property.</summary>
	public float dashXThreshold { get { return _dashXThreshold; } }

	/// <summary>Gets jumpingMovementScale property.</summary>
	public float jumpingMovementScale { get { return _jumpingMovementScale; } }

	/// <summary>Gets meditationWaitDuration property.</summary>
	public float meditationWaitDuration { get { return _meditationWaitDuration; } }

	/// <summary>Gets normalStandingAdditionalWait property.</summary>
	public float normalStandingAdditionalWait { get { return _normalStandingAdditionalWait; } }

	/// <summary>Gets and Sets meditationWaitTime property.</summary>
	public float meditationWaitTime
	{
		get { return _meditationWaitTime; }
		set { _meditationWaitTime = value; }
	}

	/// <summary>Gets directionalThresholdX property.</summary>
	public FloatRange directionalThresholdX { get { return _directionalThresholdX; } }

	/// <summary>Gets directionalThresholdY property.</summary>
	public FloatRange directionalThresholdY { get { return _directionalThresholdY; } }

	/// <summary>Gets groundedNeutralComboIndex property.</summary>
	public int groundedNeutralComboIndex { get { return _groundedNeutralComboIndex; } }

	/// <summary>Gets groundedDirectionalComboIndex property.</summary>
	public int groundedDirectionalComboIndex { get { return _groundedDirectionalComboIndex; } }

	/// <summary>Gets airNeutralComboIndex property.</summary>
	public int airNeutralComboIndex { get { return _airNeutralComboIndex; } }

	/// <summary>Gets airDirectionalComboIndex property.</summary>
	public int airDirectionalComboIndex { get { return _airDirectionalComboIndex; } }

	/// <summary>Gets scaleChangePriority property.</summary>
	public int scaleChangePriority { get { return _scaleChangePriority; } }

	/// <summary>Gets leftAxisXCredential property.</summary>
	public AnimatorCredential leftAxisXCredential { get { return _leftAxisXCredential; } }

	/// <summary>Gets leftAxisYCredential property.</summary>
	public AnimatorCredential leftAxisYCredential { get { return _leftAxisYCredential; } }

	/// <summary>Gets rightAxisXCredential property.</summary>
	public AnimatorCredential rightAxisXCredential { get { return _rightAxisXCredential; } }

	/// <summary>Gets rightAxisYCredential property.</summary>
	public AnimatorCredential rightAxisYCredential { get { return _rightAxisYCredential; } }

	/// <summary>Gets swordParticleEffect property.</summary>
	public ParticleEffect swordParticleEffect { get { return _swordParticleEffect; } }

	/// <summary>Gets extraJumpTrailRenderer property.</summary>
	public TrailRenderer extraJumpTrailRenderer { get { return _extraJumpTrailRenderer; } }

	/// <summary>Gets movementAbility Component.</summary>
	public RigidbodyMovementAbility movementAbility
	{ 
		get
		{
			if(_movementAbility == null) _movementAbility = GetComponent<RigidbodyMovementAbility>();
			return _movementAbility;
		}
	}

	/// <summary>Gets rotationAbility Component.</summary>
	public RotationAbility rotationAbility
	{ 
		get
		{
			if(_rotationAbility == null) _rotationAbility = GetComponent<RotationAbility>();
			return _rotationAbility;
		}
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

	/// <summary>Gets shootProjectile Component.</summary>
	public ShootChargedProjectile shootProjectile
	{ 
		get
		{
			if(_shootProjectile == null) _shootProjectile = GetComponent<ShootChargedProjectile>();
			return _shootProjectile;
		}
	}

	/// <summary>Gets animator Component.</summary>
	public Animator animator
	{ 
		get
		{
			if(_animator == null) _animator = GetComponent<Animator>();
			return _animator;
		}
	}

	/// <summary>Gets deltaCalculator Component.</summary>
	public TransformDeltaCalculator deltaCalculator
	{ 
		get
		{
			if(_deltaCalculator == null) _deltaCalculator = GetComponent<TransformDeltaCalculator>();
			return _deltaCalculator;
		}
	}

	/// <summary>Gets sensorSystem Component.</summary>
	public SensorSystem2D sensorSystem
	{ 
		get
		{
			if(_sensorSystem == null) _sensorSystem = GetComponent<SensorSystem2D>();
			return _sensorSystem;
		}
	}

	/// <summary>Gets wallEvaluator Component.</summary>
	public WallEvaluator wallEvaluator
	{ 
		get
		{
			if(_wallEvaluator == null) _wallEvaluator = GetComponent<WallEvaluator>();
			return _wallEvaluator;
		}
	}

	/// <summary>Gets attacksHandler Component.</summary>
	public AnimationAttacksHandler attacksHandler
	{ 
		get
		{
			if(_attacksHandler == null) _attacksHandler = GetComponent<AnimationAttacksHandler>();
			return _attacksHandler;
		}
	}

	/// <summary>Gets slopeEvaluator Component.</summary>
	public SlopeEvaluator slopeEvaluator
	{ 
		get
		{
			if(_slopeEvaluator == null) _slopeEvaluator = GetComponent<SlopeEvaluator>();
			return _slopeEvaluator;
		}
	}

	/// <summary>Gets cameraTarget Component.</summary>
	public VCameraTarget cameraTarget
	{ 
		get
		{
			if(_cameraTarget == null) _cameraTarget = GetComponent<VCameraTarget>();
			return _cameraTarget;
		}
	}

	/// <summary>Gets and Sets orientation property.</summary>
	public Vector3 orientation
	{
		get { return _orientation; }
		set { _orientation = value; }
	}

	/// <summary>Gets and Sets leftAxes property.</summary>
	public Vector2 leftAxes
	{
		get { return _leftAxes; }
		private set { _leftAxes = value; }
	}

	/// <summary>Gets and Sets postInitialPoseCooldown property.</summary>
	public Cooldown postInitialPoseCooldown
	{
		get { return _postInitialPoseCooldown; }
		private set { _postInitialPoseCooldown = value; }
	}

	/// <summary>Gets directionTowardsBackground property.</summary>
	public Vector3 directionTowardsBackground { get { return stareAtBossRotation * Vector3.forward; } }
#endregion

	/// <summary>Mateo's instance initialization when loaded [Before scene loads].</summary>
	protected override void Awake()
	{
		base.Awake();

		orientation = Vector3.right;
		sword.ActivateHitBoxes(false);
		sword.owner = gameObject;

		jumpAbility.onJumpStateChange += OnJumpStateChange;
		wallEvaluator.onWallEvaluatorEvent += OnWallEvaluatorEvent;
		dashAbility.onDashStateChange += OnDashStateChange;
		//attacksHandler.onAnimationAttackEvent += OnAnimationAttackEvent;

		postInitialPoseCooldown = new Cooldown(this, postMeditationDuration, OnPostMeditationEnds);

		animator.SetLayerWeight(_fireConjuringAnimationLayer, 0.0f);

		Meditate(true);
	}

	/// <summary>Updates Mateo's instance at each frame.</summary>
	private void Update()
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		Vector3 direction = new Vector3(
			leftAxes.x,
			0.0f,
			leftAxes.y
		);

		Debug.DrawRay(transform.position, direction * 10f, Color.magenta);

		if(leftAxes.x != 0.0f && !this.HasStates(ID_STATE_MEDITATING))
		rotationAbility.RotateTowardsDirection(animatorParent, direction);
		//else rotationAbility.RotateTowards(animatorParent, stareAtPlayerRotation);

		if(animator == null) return;

		if(movementAbility.braking)
		{
			animator.CrossFade(_brakeCredential, 0.3f);
		}

		MeditationEvaluation();
	}

	/// <summary>Callback for setting up animation IK (inverse kinematics).</summary>
	/// <param name="_layer">The index of the layer on which the IK solver is called.</param>
	private void OnAnimatorIK(int _layer)
	{
		if(animator == null) return;

		/*if(leftAxes.x > 0.0f) rotationAbility.RotateTowardsDirection(animator, Vector3.right * leftAxes.x);
		else rotationAbility.RotateTowards(animator, stareAtPlayerRotation);*/
	}

#region Callbacks:
	/// <summary>Callback invoked when the Left Axes changes.</summary>
	/// <param name="_axes">Left's Axes.</param>
	public void OnLeftAxesChange(Vector2 _axes)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		animator.SetFloat(leftAxisXCredential, _axes.x);
		animator.SetFloat(leftAxisYCredential, _axes.y);
		if(leftAxes.x == 0.0f && leftAxes == _axes) movementAbility.Stop();
		if(jumpAbility.HasAnyOfTheStates(JumpAbility.STATE_ID_FALLING)) jumpAbility.AddGravityScalar(_axes.y);
		leftAxes = _axes;
	}

	/// <summary>Callback invoked when the Right Axes changes.</summary>
	/// <param name="_axes">Right's Axes.</param>
	public void OnRightAxesChange(Vector2 _axes)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		animator.SetFloat(rightAxisXCredential, _axes.x);
		animator.SetFloat(rightAxisYCredential, _axes.y);
	}

	/// <summary>Callback invoked when JumpAbility's State Changes.</summary>
	/// <param name="_stateID">State's ID.</param>
	/// <param name="_jumpLevel">Jump's Level [index].</param>
	private void OnJumpStateChange(int _stateID, int _jumpLevel)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		switch(_stateID)
		{
			case JumpAbility.STATE_ID_GROUNDED:
			animator.SetLayerWeight(_jumpingAnimationLayer, 0.0f);
			if(!this.HasStates(ID_STATE_MEDITATING)) animator.SetLayerWeight(_locomotionAnimationLayer, 1.0f);
			CancelSwordAttack();
			if(extraJumpTrailRenderer != null) extraJumpTrailRenderer.enabled = false;
			break;

			case JumpAbility.STATE_ID_JUMPING:
			int animationHash = _normalJumpCredential;
			
			if(jumpAbility.GetJumpIndex() > 0 && extraJumpTrailRenderer != null)
			{
				animationHash = _additionalJumpCredential;
				extraJumpTrailRenderer.Clear();
				extraJumpTrailRenderer.enabled = true;
			}

			animator.SetLayerWeight(_jumpingAnimationLayer, 1.0f);
			animator.SetLayerWeight(_locomotionAnimationLayer, 0.0f);
			animator.CrossFade(animationHash, 0.3f, _jumpingAnimationLayer);
			break;

			case JumpAbility.STATE_ID_FALLING:
			//if(_jumpLevel <= 0) jumpAbility.AdvanceJumpIndex();
			animator.SetLayerWeight(_locomotionAnimationLayer, 0.0f);
			animator.CrossFade(_fallingCredential, 0.3f, _jumpingAnimationLayer);
			if(extraJumpTrailRenderer != null)
			{
				extraJumpTrailRenderer.enabled = false;
				extraJumpTrailRenderer.Clear();
			}
			break;

			case JumpAbility.STATE_ID_LANDING:
			animator.CrossFade(_softLandingCredential, 0.3f, _jumpingAnimationLayer);
			break;
	
			default:
			break;
		}

	}

	/// <summary>Callback invoked when a WallEvaluator's event occurs.</summary>
	/// <param name="_event">Event's argument.</param>
	private void OnWallEvaluatorEvent(WallEvaluationEvent _event)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		switch(_event)
		{
			case WallEvaluationEvent.OffWall:
			break;

			case WallEvaluationEvent.Walled:
			if(dashAbility.reachedMinMagnitude)
			{
				dashAbility.CancelDash();
				wallEvaluator.BounceOffWall(-orientation);
			}
			break;

			case WallEvaluationEvent.BounceEnds:
			break;
		}

		//Debug.Log("[Mateo] WallEvaluator Event invoked: " + _event.ToString());
	}

	/// <summary>Callback invoked whn a Dash State changes.</summary>
	/// <param name="_state">New Entered State.</param>
	private void OnDashStateChange(DashState _state)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

	}

	/// <summary>Callback invoked whan an Animation Attack event occurs.</summary>
	/// <param name="_state">Animation Attack's Event/State.</param>
	private void OnAnimationAttackEvent(AnimationCommandState _state)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		switch(_state)
		{
			case AnimationCommandState.None:
			sword.ActivateHitBoxes(false);
			break;

		    case AnimationCommandState.Startup:
		    sword.ActivateHitBoxes(false);
		    if(swordParticleEffect != null) swordParticleEffect.gameObject.SetActive(true);
		    //jumpAbility.gravityApplier.RequestScaleChange(GetInstanceID(), gravityScale, scaleChangePriority);
		    break;

		    case AnimationCommandState.Active:
		    sword.ActivateHitBoxes(true);
		    break;

		    case AnimationCommandState.Recovery:
		    break;

		    case AnimationCommandState.End:
		    CancelSwordAttack();
		    jumpAbility.gravityApplier.RejectScaleChange(GetInstanceID());
		    break;
		}

		/*Debug.Log("[Mateo] OnAnimationAttackEvent("
		+ _state.ToString()
		+ ")"
		+ ". With AttackHandler ID on "
		+ attacksHandler.attackID);*/
	}

	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		base.OnHealthEvent(_event, _amount);

		Debug.Log(
			"[Mateo] Received HealthEvent: "
			+ _event.ToString()
			+ " , from: "
			+ (_object != null ? _object.name : "NONE")
		);

		switch(_event)
		{
			case HealthEvent.Depleted:
			animator.CrossFade(_hitCredential, 0.3f);
			Game.SetTimeScale(Game.data.hurtTimeScale);
			break;

			case HealthEvent.Replenished:
			animator.CrossFade(_idleCredential, 0.3f);
			break;

			case HealthEvent.HitStunEnds:
			if(health.hp > 0.0f)
			animator.CrossFade(_idleCredential, 0.3f);
			Game.SetTimeScale(1.0f);
			break;

			case HealthEvent.InvincibilityEnds:
			if(health.hp > 0.0f)
			animator.CrossFade(_idleCredential, 0.3f);
			break;

			case HealthEvent.FullyDepleted:
			animator.SetAllLayersWeight(0.0f);
			animator.SetLayerWeight(1, 1.0f);
			animator.CrossFade(_deadCredential, 0.3f);
			CancelSwordAttack();
			CancelJump();
			dashAbility.CancelDash();
			DischargeFire();
			Move(Vector2.zero);
			leftAxes = Vector2.zero;
			this.ChangeState(ID_STATE_DEAD);
			this.StartCoroutine(animator.PlayAndWait(_deadCredential, _mainAnimationLayer, 0.0f, 0.0f,
			()=>
			{
				InvokeIDEvent(ID_EVENT_DEAD);
			}));
			break;
		}
	}
#endregion

#region Commands:
	/// <summary>Makes Mateo Meditate.</summary>
	/// <param name="_meditate">Meditate? true by default. If false, it ends the meditation.</param>
	public void Meditate(bool _meditate = true)
	{
		bool meditating = this.HasStates(ID_STATE_MEDITATING);

		switch(_meditate)
		{
			case true:
			if(meditating) return;

			this.AddStates(ID_STATE_MEDITATING);
			animator.SetLayerWeight(_mainAnimationLayer, 1.0f);
			animator.SetLayerWeight(_locomotionAnimationLayer, 0.0f);
			animator.CrossFade(_normalMeditationCredential, 0.5f, _mainAnimationLayer);
			leftAxes = Vector2.zero;
			CancelSwordAttack();
			InvokeIDEvent(ID_EVENT_MEDITATION_BEGINS);
			break;

			case false:
			meditationWaitTime = 0.0f;
			
			if(!meditating) return;

			animator.CrossFade(_normalStandingCredential, 0.3f, _mainAnimationLayer);
			this.StartCoroutine(animator.PlayAndWait(_normalStandingCredential, _mainAnimationLayer, 0.0f, normalStandingAdditionalWait,
			()=>
			{
				animator.SetLayerWeight(_locomotionAnimationLayer, 1.0f);
				animator.SetLayerWeight(_mainAnimationLayer, 0.0f);
				this.RemoveStates(ID_STATE_MEDITATING);
				InvokeIDEvent(ID_EVENT_MEDITATION_ENDS);
				this.DispatchCoroutine(ref meditationStanding);

			}), ref meditationStanding);

			/// Perform post-meditation cooldown:
			/*if(postInitialPoseCooldown != null && !postInitialPoseCooldown.onCooldown)
			postInitialPoseCooldown.Begin();*/
			break;
		}
	}

	/// <summary>Makes Mateo Perform its initial pose.</summary>
	/// <param name="_perform">Perform Initial Pose? True by default.</param>
	/// <param name="_ID">Initial Pose's ID [Staring at Player's Pose ID by default].</param>
	public void PerformPose(bool _perform = true, int _initialPoseID = ID_INITIALPOSE_STARRINGATPLAYER)
	{
		if(_perform)
		{
			this.AddStates(ID_STATE_MEDITATING);
			InvokeIDEvent(ID_EVENT_INITIALPOSE_BEGINS);
		}
		else
		{
			this.RemoveStates(ID_STATE_MEDITATING);
			InvokeIDEvent(ID_EVENT_INITIALPOSE_ENDED);
			
			if(postInitialPoseCooldown != null && !postInitialPoseCooldown.onCooldown)
			postInitialPoseCooldown.Begin();
		}
	}

	/// <summary>Moves Player towards given displacement axes.</summary>
	/// <param name="_axes">Displacement axes.</param>
	/// <param name="_scale">Displacement's Scale [1.0f by default].</param>
	public void Move(Vector2 _axes, float _scale = 1.0f)
	{
		Vector2 direction = wallEvaluator.GetWallHitInfo().point - (Vector2)transform.position;

		/* Move If:
			- Mateo is not hurt
			- Mateo is not landing.
			- Mateo is not attacking while grounded.
			- Mateo is not dashing.
			- Mateo is not bouncing.
			- Mateo is not walled and not trying to walk towards the wall.
			- Mateo is not on its initial pose.
		*/
		if(this.HasStates(ID_STATE_HURT)
		|| jumpAbility.HasStates(JumpAbility.STATE_ID_LANDING)
		|| (jumpAbility.grounded && /*attacksHandler.state != AttackState.None)*/ this.HasStates(ID_STATE_ATTACKING))
		|| dashAbility.state == DashState.Dashing
		|| wallEvaluator.state == WallEvaluationEvent.Bouncing
		|| (wallEvaluator.walled && Mathf.Sign(_axes.x) == Mathf.Sign(direction.x))
		|| /*postInitialPoseCooldown.onCooldown*/ meditationStanding != null) return;

		Meditate(false);

		float scale = (jumpAbility.HasStates(JumpAbility.STATE_ID_JUMPING) ? jumpingMovementScale : 1.0f) * _scale;

		//transform.rotation = Quaternion.Euler(0.0f, _axes.x < 0.0f ? 180.0f : 0.0f, 0.0f);
		if(!this.HasStates(ID_STATE_MEDITATING)) movementAbility.Move(slopeEvaluator.normalAdjuster.right.normalized * _axes.magnitude, scale, Space.World);
		slopeEvaluator.normalAdjuster.forward = _axes.x > 0.0f ? Vector3.forward : Vector3.back;
		orientation = _axes.x > 0.0f ? Vector3.right : Vector3.left;
	}

	/// <summary>Performs Jump.</summary>
	/// <param name="_axes">Additional Direction's Axes.</param>
	public void Jump(Vector2 _axes)
	{
		if(this.HasStates(ID_STATE_HURT)
		|| !this.HasStates(ID_STATE_ALIVE)
		|| (jumpAbility.grounded && /*attacksHandler.state != AttackState.None*/this.HasStates(ID_STATE_ATTACKING))) return;

		Meditate(false);

		jumpAbility.Jump(_axes);

		/*if(jumpAbility.GetJumpIndex() > 0 && extraJumpTrailRenderer != null)
		{
			extraJumpTrailRenderer.Clear();
			extraJumpTrailRenderer.enabled = true; 
		}*/
	}

	/// <summary>Cancels Jump.</summary>
	public void CancelJump()
	{
		jumpAbility.CancelJump();
	}

	/// <summary>Performs Dash.</summary>
	public void Dash()
	{
		if(this.HasStates(ID_STATE_HURT) || !this.HasStates(ID_STATE_ALIVE) || Mathf.Abs(leftAxes.x) < Mathf.Abs(dashXThreshold)) return;

		Meditate(false);
		dashAbility.Dash(orientation);
	}

	/// <summary>Performs Sword's Attack.</summary>
	/// <param name="_axes">Left Axes.</param>
	public void SwordAttack(Vector2 _axes)
	{
		/* Attack If Either:
			- Mateo is not hurt.
			- Mateo is not already attacking.
			- Mateo is not on waiting state.
			- Mateo is not bouncing from a wall.
			- Mateo is not landing.
		*/
		if(this.HasStates(ID_STATE_HURT)
		/*|| attacksHandler.state == AttackState.Attacking
		|| attacksHandler.state == AttackState.Waiting*/
		|| this.HasStates(ID_STATE_ATTACKING)
		|| wallEvaluator.state == WallEvaluationEvent.Bouncing
		|| jumpAbility.HasStates(JumpAbility.STATE_ID_LANDING)) return;
		
		//hurtBox.SetActive(false);
		health.BeginInvincibilityCooldown();
		Meditate(false);

		int index = 0;
		int animationHash = 0;
		bool applyDirectional = directionalThresholdX.ValueOutside(leftAxes.x) || directionalThresholdY.ValueOutside(leftAxes.y);
		bool grounded = jumpAbility.grounded;

		if(grounded) animationHash = _groundSwordAttackCredential;
		else animationHash = jumpAbility.GetJumpIndex() > 0 ? _additionalJumpSwordAttackCredential : _normalJumpSwordAttackCredential;

		if(grounded) index = applyDirectional ? groundedDirectionalComboIndex : groundedNeutralComboIndex;
		else index = applyDirectional ? airDirectionalComboIndex : airNeutralComboIndex;

		/*if(attacksHandler.BeginAttack(index))
		{
			sword.ActivateHitBoxes(true);
			animator.SetBool(attackCredential, true);
			animator.SetInteger(attackIDCredential, attacksHandler.attackID);

			//// OH BOY
		}*/

		this.AddStates(ID_STATE_ATTACKING);

		sword.ActivateHitBoxes(true);
		animator.SetLayerWeight(_attackAnimationLayer, 1.0f);
		this.StartCoroutine(animator.PlayAndWait(animationHash, _attackAnimationLayer, 0.0f, 0.0f,
		()=>
		{
			CancelSwordAttack();
		}));
	}

	/// <summary>Cancels Attacks.</summary>
	public void CancelSwordAttack()
	{
		//hurtBox.SetActive(true);
		health.OnInvincibilityCooldownEnds();
		if(swordParticleEffect != null) swordParticleEffect.gameObject.SetActive(false);
		//attacksHandler.CancelAttack();
		sword.ActivateHitBoxes(false);
		this.RemoveStates(ID_STATE_ATTACKING);
		animator.SetLayerWeight(_attackAnimationLayer, 0.0f);
		jumpAbility.gravityApplier.RejectScaleChange(GetInstanceID());
		/*animator.SetBool(attackCredential, false);
		animator.SetInteger(attackIDCredential, attacksHandler.attackID);*/
	}

	/// <summary>Charges Fire.</summary>
	/// <param name="_axes">Axes' Argument.</param>
	public void ChargeFire(Vector3 _axes)
	{
		if(this.HasStates(ID_STATE_HURT) || !this.HasStates(ID_STATE_ALIVE)) return;

		Meditate(false);
		animator.SetLayerWeight(_fireConjuringAnimationLayer, 1.0f);
		
		int chargeStateID = shootProjectile.OnCharge(_axes);
		int animationHash = 0;

		switch(chargeStateID)
		{
			case ShootChargedProjectile.STATE_ID_UNCHARGED:
			break;

			case ShootChargedProjectile.STATE_ID_CHARGING:
			animationHash = _fireConjuringCredential;
			break;

			case ShootChargedProjectile.STATE_ID_CHARGED:
			animationHash = _fireMaxChargedCredential;
			break;

			case ShootChargedProjectile.STATE_ID_RELEASED:
			break;
		}

		animator.CrossFade(animationHash, 0.3f, _fireConjuringAnimationLayer);
	}

	/// <summary>Discharges Fire.</summary>
	/// <param name="_shootResult">Was the shoot made? false by default.</param>
	public void DischargeFire(bool _shootResult = false)
	{
		if(shootProjectile.onCooldown || !this.HasStates(ID_STATE_ALIVE)) return;

		shootProjectile.OnDischarge();
		int stateID = _shootResult ? ShootChargedProjectile.STATE_ID_RELEASED : ShootChargedProjectile.STATE_ID_UNCHARGED;
		animator.CrossFade(_fireThrowCredential, 0.3f, _fireConjuringAnimationLayer);
	}

	/// <summary>Releases Fire.</summary>
	/// <param name="_axes">Axes' Argument.</param>
	public void ReleaseFire(Vector3 _axes)
	{
		if(this.HasStates(ID_STATE_HURT) || !this.HasStates(ID_STATE_ALIVE)) return;

		bool result = shootProjectile.Shoot(leftHand.position, _axes);
		//if(result) animator.SetInteger(shootingStateIDCredential, ShootChargedProjectile.STATE_ID_RELEASED);
		if(result)
		{
			animator.CrossFade(_fireThrowCredential, 0.3f, _fireConjuringAnimationLayer);
			this.StartCoroutine(animator.PlayAndWait(_fireThrowCredential, _fireConjuringAnimationLayer, 0.0f, 0.0f,
			()=>
			{
				animator.SetLayerWeight(_fireConjuringAnimationLayer, 0.0f);
			}));
		}
		DischargeFire(result);
	}

	/// <summary>Callback invoking when the Post-Meditation cooldown ends.</summary>
	private void OnPostMeditationEnds()
	{
		this.RemoveStates(ID_STATE_MEDITATING);
	}

	/// <summary>Changes rotation towards given target.</summary>
	/// <param name="_target">Target to stare at.</param>
	public void StareTowards(StareTarget _target = StareTarget.Boss)
	{
		if(animator != null) animator.transform.rotation = _target == StareTarget.Boss ? stareAtBossRotation : stareAtPlayerRotation;
	}
#endregion

	/// <summary>Evaluates for Meditation.</summary>
	private void MeditationEvaluation()
	{
		if(jumpAbility.HasStates(JumpAbility.STATE_ID_GROUNDED)
		&& deltaCalculator.velocity.sqrMagnitude == 0.0f
		&& /*!postInitialPoseCooldown.onCooldown*/ meditationStanding == null)
		{
			meditationWaitTime += Time.deltaTime;

			if(meditationWaitTime >= meditationWaitDuration && !this.HasStates(ID_STATE_MEDITATING))
			Meditate();
		}
		else meditationWaitTime = 0.0f;
	}

/// \TODO Eventually Remove:
#region TESTs:
	public void Hurt()
	{
		health.GiveDamage(0.5f);
		Debug.Log("[Mateo] Take Damage Astro Boy! Health's Information: " + health.ToString());
	}

	public void Kill()
	{
		Debug.Log("[Mateo] Die Astro Boy! Health's Information: " + health.ToString());
		health.GiveDamage(Mathf.Infinity);
	}

	public void Revive()
	{
		Debug.Log("[Mateo] I forgive you Astro Boy, come back! Health's Information: " + health.ToString());
		health.ReplenishHealth(Mathf.Infinity);
	}
#endregion
}
}