using Sirenix.OdinInspector;
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
	public const int ID_INITIALPOSE_STARRINGATPLAYER = 0; 																/// <summary>Starring At Player's Initial Pose's ID.</summary>
	public const int ID_INITIALPOSE_STARRINGATBACKGROUND = 2; 															/// <summary>Starring At Background's Initial Pose's ID.</summary>
	public const int ID_INITIALPOSE_MEDITATING = 1; 																	/// <summary>Meditation Initial Pose's ID.</summary>
	public const int ID_STATE_INITIALPOSE = 1 << 5; 																	/// <summary>Initial Pose's State ID.</summary>
	public const int ID_STATE_MEDITATING = 1 << 5; 																		/// <summary>Meditating's State ID.</summary>
	public const int ID_STATE_SWORDEQUIPPED = 1 << 6; 																	/// <summary>Sword's Equipped's State ID.</summary>
	public const int ID_STATE_FIRECONJURINGCAPACITY = 1 << 7; 															/// <summary>Mire Conjuring's Capacity State's ID.</summary>
	public const int ID_STATE_JUMPING = 1 << 8; 																		/// <summary>Jumping's State ID.</summary>
	public const int ID_STATE_CHARGINGFIRE = 1 << 8; 																	/// <summary>Fire Conjuring's State ID.</summary>
	public const int ID_STATE_CROUCHING = 1 << 10; 																		/// <summary>Crouching's State ID.</summary>
	public const int ID_STATE_MOVING = 1 << 11; 																		/// <summary>Moving's State ID.</summary>
	public const int ID_STATE_BRAKING = 1 << 12; 																		/// <summary>Braking's State ID.</summary>
	public const int ID_STATE_STANDINGUP = 1 << 13; 																	/// <summary>Standing Up's State ID.</summary>
	public const int ID_EVENT_INITIALPOSE_BEGINS = 0; 																	/// <summary>Mateo Initial-Pose-Begins's Event ID.</summary>
	public const int ID_EVENT_INITIALPOSE_ENDED = 1; 																	/// <summary>Mateo Initial-Pose-Finished's Event ID.</summary>
	public const int ID_EVENT_MEDITATION_BEGINS = 2; 																	/// <summary>Meditation Begins' Event.</summary>
	public const int ID_EVENT_MEDITATION_ENDS = 3; 																		/// <summary>Meditation Ends' Event.</summary>
	public const int ID_EVENT_HURT = 4; 																				/// <summary>Mateo's Hurt Event.</summary>
	public const int ID_EVENT_DEAD = 5; 																				/// <summary>Mateo's Dead Event.</summary>

	[Space(5f)]
	[SerializeField] private GameObject hurtBox; 																		/// <summary>HurtBox's Container.</summary>
	[Header("Rotations:")]
	[SerializeField] private EulerRotation _stareAtBossRotation; 														/// <summary>Stare at Boss's Rotation.</summary>
	[SerializeField] private EulerRotation _stareAtPlayerRotation; 														/// <summary>Stare At Player's Rotation.</summary>
	[Space(5f)]
	[SerializeField] private float _crouchDuration; 																	/// <summary>Crouch's Duration.</summary>
	[Space(5f)]
	[Header("Sword's Attributes:")]
	[TabGroup("Sword Attacks")][SerializeField] private Sword _sword; 													/// <summary>Mateo's Sword.</summary>
	[SerializeField] private int _groundedNeutralComboIndex; 															/// <summary>Grounded Neutral Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField] private int _groundedDirectionalComboIndex; 														/// <summary>Grounded Directional Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField] private int _airNeutralComboIndex; 																/// <summary>Air Neutral Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField] private int _airDirectionalComboIndex; 															/// <summary>Air Directional Combo's index on the AnimationAttacksHandler.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _animationStateProgress; 															/// <summary>Minimum Animation State Progress to open the next attack's window.</summary>
	[SerializeField] private float _attackDurationWindow; 																/// <summary>Duration Window's for next sword attack.</summary>
	[Space(5f)]
	[TabGroup("Sword Attacks")][SerializeField] private FloatRange _directionalThresholdX; 								/// <summary>Directional Threhold on the X's Axis to perform directional attacks.</summary>
	[TabGroup("Sword Attacks")][SerializeField] private FloatRange _directionalThresholdY; 								/// <summary>Directional Threhold on the Y's Axis to perform directional attacks.</summary>
	[Space(5f)]
	[SerializeField] private float _gravityScale; 																		/// <summary>Gravity Scale applied when attacking.</summary>
	[SerializeField] private int _scaleChangePriority; 																	/// <summary>Gravity Scale's Change Priority.</summary>
	[Space(5f)]
	[SerializeField] private float _jumpingMovementScale; 																/// <summary>Movement's Scale when Mateo is Jumping.</summary>
	[Space(5f)]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _dashXThreshold; 																	/// <summary>Minimum left axis' X [absolute] value to be able to perform dash.</summary>
	[Space(5f)]
	[Header("AnimatorController's Parameters:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _leftAxisXCredential; 							/// <summary>Left Axis X's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _leftAxisYCredential; 							/// <summary>Left Axis Y's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _rightAxisXCredential; 							/// <summary>Right Axis X's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _rightAxisYCredential; 							/// <summary>Right Axis Y's Animator Credential.</summary>
	[Space(5f)]
	[Header("Animation Layers:")]
	[TabGroup("Animations")][SerializeField] private int _mainAnimationLayer; 											/// <summary>Main's Animation Layer.</summary>
	[TabGroup("Animations")][SerializeField] private int _fireConjuringAnimationLayer; 									/// <summary>Fire Conjuring's Animation Layer.</summary>
	[TabGroup("Animations")][SerializeField] private int _jumpingAnimationLayer; 										/// <summary>Jumping's Animation Layer.</summary>
	[TabGroup("Animations")][SerializeField] private int _attackAnimationLayer; 										/// <summary>Attack's Animation Layer.</summary>
	[Space(5f)]
	[Header("Animation Clips' Credentials:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _emptyCredential; 								/// <summary>Empty State's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _noSwordLocomotionCredential; 					/// <summary>No-Sword's Locomotion's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _swordLocomotionCredential; 					/// <summary>Sword's Locomotion's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _crouchCredential; 								/// <summary>Crouch's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalMeditationCredential; 					/// <summary>Normal Meditaiton's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _brakeCredential; 								/// <summary>Brake's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _hitCredential; 								/// <summary>Hit's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _crashCredential; 								/// <summary>Crash's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _deadCredential; 								/// <summary>Dead's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _swordMeditationCredential; 					/// <summary>Sword Meditaiton's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireMeditationCredential; 						/// <summary>Fire Meditaiton's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalStandingCredential; 						/// <summary>Normal Standing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _jumpStandingCredential; 						/// <summary>Jump Standing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalJumpCredential; 							/// <summary>Normal Jump's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _additionalJumpCredential; 						/// <summary>Additional Jump's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fallingCredential; 							/// <summary>Falling's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _softLandingCredential; 						/// <summary>Soft Landing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _hardLandingCredential; 						/// <summary>Hard Landing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireConjuringCredential; 						/// <summary>Fire Conjuring's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireHoldingCredential; 						/// <summary>Fire Holding's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireMaxChargedCredential; 						/// <summary>Fire's Max Charged's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireThrowCredential; 							/// <summary>Fire Throw's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _swordObtentionCredential; 						/// <summary>Sword Obtention's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireObtentionCredential; 						/// <summary>Fire Obtention's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _groundSwordAttackCredential; 					/// <summary>Ground Sword Attack's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalJumpSwordAttackCredential; 				/// <summary>Normal Jump Sword Attack's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _additionalJumpSwordAttackCredential; 			/// <summary>Additional Jump Sword Attack's AnimatorCredential.</summary>
	[Space(5f)]
	[Header("Particle Effects:")]
	[TabGroup("Particle Effects")][SerializeField] private ParticleEffectEmissionData _initialJumpParticleEffect; 		/// <summary>Initial Jump's ParticleEffect's Emission Data.</summary>
	[TabGroup("Particle Effects")][SerializeField] private ParticleEffectEmissionData _additionalJumpParticleEffect; 	/// <summary>Additional Jump's ParticleEffect's Emission Data.</summary>
	[TabGroup("Particle Effects")][SerializeField] private ParticleEffectEmissionData _softLandingParticleEffect; 		/// <summary>Soft-Landing's ParticleEffect's Emission Data.</summary>
	[TabGroup("Particle Effects")][SerializeField] private ParticleEffectEmissionData _hardLandingParticleEffect; 		/// <summary>Hard-Landing's ParticleEffect's Emission Data.</summary>
	[Space(5f)]
	[SerializeField] private TrailRenderer _extraJumpTrailRenderer; 													/// <summary>Extra-Jump's Trail Renderer.</summary>
	[Space(5f)]
	[SerializeField] private ParticleEffect _swordParticleEffect; 														/// <summary>Sword's Particle Effect when slashing.</summary>
	[Space(5f)]
	[Header("Sound Effect's:")]
	[TabGroup("Sound Effects")][SerializeField] private CollectionIndex _initialJumpSoundEffectIndex; 					/// <summary>Initial Jump Sound Effect's Index.</summary>
	[TabGroup("Sound Effects")][SerializeField] private CollectionIndex _additionalJumpSoundEffectIndex; 				/// <summary>Additional Jump Sound Effect's Index.</summary>
	[Header("Meditation's Attributes:")]
	[SerializeField] private float _meditationWaitDuration; 															/// <summary>Meditation Wait's Duration.</summary>
	[SerializeField] private float _normalStandingAdditionalWait; 														/// <summary>Normal Meditation's Standing Additional Wait Duration.</summary>
	private float _meditationWaitTime; 																					/// <summary>Current Meditation's Time.</summary>
	private RigidbodyMovementAbility _movementAbility; 																	/// <summary>RigidbodyMovementAbility's Component.</summary>
	private RotationAbility _rotationAbility; 																			/// <summary>RotationAbility's Component.</summary>
	private JumpAbility _jumpAbility; 																					/// <summary>JumpAbility's Component.</summary>
	private ShootChargedProjectile _shootProjectile; 																	/// <summary>ShootChargedProjectile's Component.</summary>
	private DashAbility _dashAbility; 																					/// <summary>DashAbility's Component.</summary>
	private TransformDeltaCalculator _deltaCalculator; 																	/// <summary>TransformDeltaCalculator's Component.</summary>
	private SensorSystem2D _sensorSystem; 																				/// <summary>SensorSystem2D's Component.</summary>
	private WallEvaluator _wallEvaluator; 																				/// <summary>WallEvaluator's Component.</summary>
	private AnimationAttacksHandler _attacksHandler; 																	/// <summary>AnimationAttacksHandler's Component.</summary>
	private SlopeEvaluator _slopeEvaluator; 																			/// <summary>SlopeEvaluator's Component.</summary>
	private VCameraTarget _cameraTarget; 																				/// <summary>VCameraTarget's Component.</summary>
	private Vector3 _orientation; 																						/// <summary>Mateo's Orientation.</summary>
	private Vector2 _leftAxes; 																							/// <summary>Left Axes' Value.</summary>

#region Getters/Setters:
	/// <summary>Gets stareAtBossRotation property.</summary>
	public EulerRotation stareAtBossRotation { get { return _stareAtBossRotation; } }

	/// <summary>Gets stareAtPlayerRotation property.</summary>
	public EulerRotation stareAtPlayerRotation { get { return _stareAtPlayerRotation; } }

	/// <summary>Gets sword property.</summary>
	public Sword sword { get { return _sword; } }

	/// <summary>Gets crouchDuration property.</summary>
	public float crouchDuration { get { return _crouchDuration; } }

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

	/// <summary>Gets initialJumpParticleEffect property.</summary>
	public ParticleEffectEmissionData initialJumpParticleEffect { get { return _initialJumpParticleEffect; } }

	/// <summary>Gets additionalJumpParticleEffect property.</summary>
	public ParticleEffectEmissionData additionalJumpParticleEffect { get { return _additionalJumpParticleEffect; } }

	/// <summary>Gets softLandingParticleEffect property.</summary>
	public ParticleEffectEmissionData softLandingParticleEffect { get { return _softLandingParticleEffect; } }

	/// <summary>Gets hardLandingParticleEffect property.</summary>
	public ParticleEffectEmissionData hardLandingParticleEffect { get { return _hardLandingParticleEffect; } }

	/// <summary>Gets swordParticleEffect property.</summary>
	public ParticleEffect swordParticleEffect { get { return _swordParticleEffect; } }

	/// <summary>Gets initialJumpSoundEffectIndex property.</summary>
	public CollectionIndex initialJumpSoundEffectIndex { get { return _initialJumpSoundEffectIndex; } }

	/// <summary>Gets additionalJumpSoundEffectIndex property.</summary>
	public CollectionIndex additionalJumpSoundEffectIndex { get { return _additionalJumpSoundEffectIndex; } }

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

	/// <summary>Gets directionTowardsBackground property.</summary>
	public Vector3 directionTowardsBackground { get { return stareAtBossRotation * Vector3.forward; } }
#endregion

//---------------------------------------
//	 		UNITY-CALLBACKS: 			|
//---------------------------------------
#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when Mateo's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(initialJumpParticleEffect != null) initialJumpParticleEffect.DrawGizmos();
		if(additionalJumpParticleEffect != null) additionalJumpParticleEffect.DrawGizmos();
	}
#endif

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
		movementAbility.onBraking += OnMovementBraking;
		//attacksHandler.onAnimationAttackEvent += OnAnimationAttackEvent;

		animator.SetAllLayersWeight(0.0f); 						/// Just in case...
		animator.SetLayerWeight(_mainAnimationLayer, 1.0f);

		Meditate(true);
		EquipSword(true);
		state |= ID_STATE_FIRECONJURINGCAPACITY;
		shootProjectile.muzzle = skeleton.leftHand;
	}

	/// <summary>Updates Mateo's instance at each frame.</summary>
	private void Update()
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		RotateTowardsLeftAxes();

		if(animator == null) return;

		/*bool x = this.HasAnyOfTheStates(ID_STATE_JUMPING | ID_STATE_MEDITATING | ID_STATE_STANDINGUP | ID_STATE_HURT | ID_STATE_DEAD | ID_STATE_ATTACKING | ID_STATE_BRAKING | ID_STATE_CROUCHING);
		if(!this.HasStates(ID_STATE_MEDITATING) && animator.GetLayerWeight(_mainAnimationLayer) > 0.0f && !x)
		GoToLocomotionAnimation();*/

		//BrakingEvaluation();
		MeditationEvaluation();
	}

//---------------------------------------
//	 		METHODS: 					|
//---------------------------------------
#region MovementMethods:
	/// <summary>Moves Player towards given displacement axes.</summary>
	/// <param name="_axes">Displacement axes.</param>
	/// <param name="_scale">Displacement's Scale [1.0f by default].</param>
	public void Move(Vector2 _axes, float _scale = 1.0f)
	{
		Vector2 direction = wallEvaluator.GetWallHitInfo().point - (Vector2)transform.position;

		/* Move If:
			- Mateo is not hurt
			- Mateo is not crouching.
			- Mateo is not landing.
			- Mateo is not attacking while grounded.
			- Mateo is not dashing.
			- Mateo is not bouncing.
			- Mateo is not walled and not trying to walk towards the wall.
			- Mateo is not on its initial pose.
		*/
		if(this.HasStates(ID_STATE_HURT)
		|| this.HasStates(ID_STATE_CROUCHING)
		|| jumpAbility.HasStates(JumpAbility.STATE_ID_LANDING)
		|| (jumpAbility.grounded && /*attacksHandler.state != AttackState.None)*/ this.HasStates(ID_STATE_ATTACKING))
		|| dashAbility.state == DashState.Dashing
		|| wallEvaluator.state == WallEvaluationEvent.Bouncing
		|| (wallEvaluator.walled && Mathf.Sign(_axes.x) == Mathf.Sign(direction.x))) return;

		Meditate(false);

		if(this.HasStates(ID_STATE_MEDITATING)) return;

		float scale = (jumpAbility.HasStates(JumpAbility.STATE_ID_JUMPING) ? jumpingMovementScale : 1.0f) * _scale;

		//transform.rotation = Quaternion.Euler(0.0f, _axes.x < 0.0f ? 180.0f : 0.0f, 0.0f);
		if(!this.HasStates(ID_STATE_MEDITATING)) movementAbility.Move(slopeEvaluator.normalAdjuster.right.normalized * _axes.magnitude, scale, Space.World);
		slopeEvaluator.normalAdjuster.forward = _axes.x > 0.0f ? Vector3.forward : Vector3.back;
		orientation = _axes.x > 0.0f ? Vector3.right : Vector3.left;

		//if(jumpAbility.grounded) GoToLocomotionAnimation();
	}

	/// \TODO DEPRECATE. Now it is made on a callback...
	/// <summary>Braking's Evaluation.</summary>
	private void BrakingEvaluation()
	{
		if(movementAbility.braking && !this.HasStates(ID_STATE_BRAKING))
		{
			state |=  ID_STATE_BRAKING;
			animatorController.CrossFadeAndWait(
				_brakeCredential,
				clipFadeDuration,
				_mainAnimationLayer,
				0.0f,
				0.0f,
				()=>
				{	
					OnMainLayerAnimationFinished();
					state &=  ~ID_STATE_BRAKING;
				}
			);
		}
	}

	/// <summary>Performs Dash.</summary>
	public void Dash()
	{
		if(this.HasStates(ID_STATE_HURT) || !this.HasStates(ID_STATE_ALIVE) || Mathf.Abs(leftAxes.x) < Mathf.Abs(dashXThreshold)) return;

		Meditate(false);
		dashAbility.Dash(orientation);
	}
#endregion

#region MeditationMethods:
	/// <summary>Makes Mateo Meditate.</summary>
	/// <param name="_meditate">Meditate? true by default. If false, it ends the meditation.</param>
	public void Meditate(bool _meditate = true, int _contextFlag =  0)
	{
		if(this.HasStates(ID_STATE_STANDINGUP)) return;

		bool meditating = this.HasStates(ID_STATE_MEDITATING);

		switch(_meditate)
		{
			case true:
			if(meditating) return;

			CancelAllActions();
			state |= ID_STATE_MEDITATING;
			animator.SetLayerWeight(_mainAnimationLayer, 1.0f);
			animatorController.CrossFade(_normalMeditationCredential, clipFadeDuration, _mainAnimationLayer, 0.0f);
			InvokeIDEvent(ID_EVENT_MEDITATION_BEGINS);
			break;

			case false:
			meditationWaitTime = 0.0f;
			
			if(!meditating) return;

			int standingHash = 0;

			if((_contextFlag | ID_STATE_JUMPING) == _contextFlag) standingHash = _jumpStandingCredential;
			else standingHash = _normalStandingCredential;

			state |= ID_STATE_STANDINGUP;
			animatorController.CrossFadeAndWait(standingHash, clipFadeDuration, _mainAnimationLayer, 0.0f, normalStandingAdditionalWait,
			()=>
			{
				state &= ~ID_STATE_MEDITATING;
				state &= ~ID_STATE_STANDINGUP;
				InvokeIDEvent(ID_EVENT_MEDITATION_ENDS);
				OnMainLayerAnimationFinished();

			});
			break;
		}
	}

	/// <summary>Changes Meditation's Pose.</summary>
	/// <param name="_animatonHas">Meditation Pose's Animation Hash.</param>
	public void ChangeMeditationPose(AnimatorCredential _animationCredential)
	{
		/* Perform Meditation Pose if:
			- Is meditating and not already playing another meditation pose
			- Have Fire Connjuring's capacity if the meditation pose is that of the fire
			- Have the Sword equipped if the meditation pose is that of the sword
		*/
		if(!this.HasStates(ID_STATE_MEDITATING)
		|| (_animationCredential == _fireMeditationCredential && !this.HasStates(ID_STATE_FIRECONJURINGCAPACITY))
		|| (_animationCredential == _swordMeditationCredential && !this.HasStates(ID_STATE_SWORDEQUIPPED))
		|| animatorController.GetActive(_animationCredential, _mainAnimationLayer)) return;

		shootProjectile.OnDischarge();

		if(_animationCredential == _fireMeditationCredential && this.HasStates(ID_STATE_FIRECONJURINGCAPACITY))
		{
			shootProjectile.ID = shootProjectile.chargedProjectileID;
			shootProjectile.CreateProjectile();	
		}

		animatorController.CrossFadeAndWait(
			_animationCredential,
			clipFadeDuration,
			_mainAnimationLayer,
			0.0f,
			0.0f,
			()=>
			{
				shootProjectile.OnDischarge();
				animatorController.CrossFade(_normalMeditationCredential, clipFadeDuration, _mainAnimationLayer, 0.0f);
			}
		);
	}

	/// <summary>Evaluates for Meditation.</summary>
	private void MeditationEvaluation()
	{
		if(jumpAbility.HasStates(JumpAbility.STATE_ID_GROUNDED)
		&& deltaCalculator.deltaPosition.sqrMagnitude == 0.0f)
		{
			meditationWaitTime += Time.deltaTime;

			if(meditationWaitTime >= meditationWaitDuration && !this.HasStates(ID_STATE_MEDITATING))
			Meditate(true);
		}
		else meditationWaitTime = 0.0f;
	}
#endregion

#region SwordMethods:
	/// <summary>Equips Sword to Mateo.</summary>
	/// <param name="_equip">Equip? True by default.</param>
	public void EquipSword(bool _equip = true)
	{
		sword.gameObject.SetActive(_equip);

		switch(_equip)
		{
			case true:
			sword.transform.SetParent(skeleton.rightHand);
			state |= ID_STATE_SWORDEQUIPPED;
			break;

			case false:
			sword.transform.SetParent(null);
			state &= ~ID_STATE_SWORDEQUIPPED;
			break;
		}

		//GoToLocomotionAnimation();
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
		
		if(this.HasStates(ID_STATE_MEDITATING))
		{
			ChangeMeditationPose(_swordMeditationCredential);
			return;
		}

		//hurtBox.SetActive(false);
		Meditate(false);
		health.BeginInvincibilityCooldown(); 	/// THIS IS GAY AND TEMPORAL

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

		state |= ID_STATE_ATTACKING;

		sword.ActivateHitBoxes(true);
		animator.SetLayerWeight(_mainAnimationLayer, 0.0f);
		animator.SetLayerWeight(_attackAnimationLayer, 1.0f);
		animatorController.CrossFadeAndWait(animationHash, clipFadeDuration, _attackAnimationLayer, 0.0f, 0.0f, CancelSwordAttack);
	}

	/// <summary>Cancels Attacks.</summary>
	public void CancelSwordAttack()
	{
		//hurtBox.SetActive(true);
		health.OnInvincibilityCooldownEnds();
		if(swordParticleEffect != null) swordParticleEffect.gameObject.SetActive(false);
		//attacksHandler.CancelAttack();
		sword.ActivateHitBoxes(false);
		state &= ~ID_STATE_ATTACKING;
		animator.SetLayerWeight(_attackAnimationLayer, 0.0f);
		animator.SetLayerWeight(_mainAnimationLayer, 1.0f);
		jumpAbility.gravityApplier.RejectScaleChange(GetInstanceID());
	}
#endregion

#region FireConjuringMethods:
	/// <summary>Charges Fire.</summary>
	/// <param name="_axes">Axes' Argument.</param>
	public void ChargeFire(Vector3 _axes)
	{
		/* Charge Fire if:
			- Has Fire Conjuring's Capacity.
			- Is not hurt.
			- It is alive.
		*/
		if(!this.HasStates(ID_STATE_FIRECONJURINGCAPACITY) || this.HasStates(ID_STATE_HURT) || !this.HasStates(ID_STATE_ALIVE)) return;

		if(this.HasStates(ID_STATE_MEDITATING))
		{
			ChangeMeditationPose(_fireMeditationCredential);
			return;
		}

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

		animatorController.CrossFade(animationHash, clipFadeDuration, _fireConjuringAnimationLayer);
	}

	/// <summary>Discharges Fire.</summary>
	/// <param name="_shootResult">Was the shoot made? false by default.</param>
	public void DischargeFire(bool _shootResult = false)
	{
		/* Discharge Fire if:
			- Has Fire Conjuring's Capacity.
			- Shooting is not on cooldown.
			- It is alive.
		*/
		if(!this.HasStates(ID_STATE_FIRECONJURINGCAPACITY) || shootProjectile.onCooldown || this.HasStates(ID_STATE_MEDITATING) || !this.HasStates(ID_STATE_ALIVE)) return;

		shootProjectile.OnDischarge();
		OnFireConjuringLayerAnimationFinished();
	}

	/// <summary>Releases Fire.</summary>
	/// <param name="_axes">Axes' Argument.</param>
	public void ReleaseFire(Vector3 _axes)
	{
		/* Release Fire if:
			- Has Fire Conjuring's Capacity.
			- Is not hurt or meditating.
			- It is alive.
		*/
		if(!this.HasStates(ID_STATE_FIRECONJURINGCAPACITY) || this.HasAnyOfTheStates(ID_STATE_HURT | ID_STATE_MEDITATING) || !this.HasStates(ID_STATE_ALIVE)) return;

		bool result = shootProjectile.Shoot(skeleton.leftHand.position, _axes);

		if(result)
		{
			animatorController.CrossFadeAndWait(
				_fireThrowCredential,
				clipFadeDuration,
				_fireConjuringAnimationLayer,
				0.0f,
				0.0f,
				OnFireConjuringLayerAnimationFinished
			);
		}
		DischargeFire(result);
	}
#endregion

#region JumpingMethods:
	/// <summary>Performs Jump.</summary>
	/// <param name="_axes">Additional Direction's Axes.</param>
	public void Jump(Vector2 _axes)
	{
		/* Jump if:
			- Not meditating or hurt.
			- Alive.
			- Grounded, but not attacking.
		*/
		if(this.HasStates(ID_STATE_HURT | ID_STATE_MEDITATING)
		|| !this.HasStates(ID_STATE_ALIVE)
		|| (jumpAbility.grounded && /*attacksHandler.state != AttackState.None*/this.HasStates(ID_STATE_ATTACKING))) return;

		Meditate(false, ID_STATE_JUMPING);

		jumpAbility.Jump(_axes);
	}
	
	/// <summary>Cancels Jump.</summary>
	public void CancelJump()
	{
		jumpAbility.CancelJump();
	}
#endregion

#region LocomotionMethods:
	/// <summary>Makes Mateo Crouch.</summary>
	public void Crouch()
	{
		if(!this.HasStates(ID_STATE_ALIVE)
		|| (this.HasStates(ID_STATE_CROUCHING) || this.HasAnyOfTheStates(ID_STATE_JUMPING | ID_STATE_ATTACKING | ID_STATE_MEDITATING | ID_STATE_HURT))) return;

		state |= ID_STATE_CROUCHING;
		animatorController.CrossFadeAndWait(
			_crouchCredential,
			clipFadeDuration,
			_mainAnimationLayer,
			0.0f,
			crouchDuration,
			()=>
			{
				state &= ~ID_STATE_CROUCHING;
				OnMainLayerAnimationFinished();
			}
		);
	}

	/// <summary>Rotates towards Left's Axes.</summary>
	public void RotateTowardsLeftAxes()
	{
		/* Rotate towards give directio if:
			- Not Meditating
			- Not Attacking
		*/
		if(this.HasAnyOfTheStates(ID_STATE_MEDITATING | ID_STATE_HURT)) return;

		Vector3 direction = new Vector3(
			leftAxes.x,
			0.0f,
			leftAxes.y
		);

		if(leftAxes.x != 0.0f)
		rotationAbility.RotateTowardsDirection(animatorParent, direction);
	}

	/// <summary>Changes rotation towards given target.</summary>
	/// <param name="_target">Target to stare at.</param>
	public void StareTowards(StareTarget _target = StareTarget.Boss)
	{
		if(animator != null) animator.transform.rotation = _target == StareTarget.Boss ? stareAtBossRotation : stareAtPlayerRotation;
	}
#endregion

#region OtherMethods:
	/// <summary>Resets Axes.</summary>
	private void ResetAxes()
	{
		leftAxes = Vector2.zero;
	}

	/// <summary>Cancels All Actions.</summary>
	public void CancelAllActions()
	{
		ResetAxes();
		CancelSwordAttack();
		CancelJump();
		dashAbility.CancelDash();
		DischargeFire();
		animator.SetAllLayersWeight(0.0f);
		animator.SetLayerWeight(_mainAnimationLayer, 1.0f);

		/// Remove all action flags:
		state &= ~(ID_STATE_ATTACKING | ID_STATE_JUMPING | ID_STATE_CROUCHING | ID_STATE_CHARGINGFIRE | ID_STATE_MEDITATING | ID_STATE_BRAKING | ID_STATE_STANDINGUP);

		/// Re-evaluate Callbacks:
		OnJumpStateChange(jumpAbility.state, jumpAbility.GetJumpIndex());
	}
#endregion

//---------------------------------------
//	 		CALLBACKS: 					|
//---------------------------------------
#region OtherCallbacks:
	/// <summary>Callback invoked when the Left Axes changes.</summary>
	/// <param name="_axes">Left's Axes.</param>
	public void OnLeftAxesChange(Vector2 _axes)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		if(!this.HasStates(ID_STATE_ATTACKING))
		{
			animator.SetFloat(leftAxisXCredential, _axes.x);
			animator.SetFloat(leftAxisYCredential, _axes.y);
		}
			
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
#endregion

#region ComponentsCallbacks:
	/// <summary>Callback invoked when JumpAbility's State Changes.</summary>
	/// <param name="_stateID">State's ID.</param>
	/// <param name="_jumpLevel">Jump's Level [index].</param>
	private void OnJumpStateChange(int _stateID, int _jumpLevel)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;

		switch(_stateID)
		{
			case JumpAbility.STATE_ID_GROUNDED:
				CancelSwordAttack();
				
				if(!this.HasAnyOfTheStates(ID_STATE_MEDITATING | ID_STATE_HURT))

				if(extraJumpTrailRenderer != null)
				extraJumpTrailRenderer.enabled = false;
			break;

			case JumpAbility.STATE_ID_JUMPING:
				int animationHash = _normalJumpCredential;
				
				if(jumpAbility.GetJumpIndex() > 0 && extraJumpTrailRenderer != null)
				{
					animationHash = _additionalJumpCredential;
					extraJumpTrailRenderer.Clear();
					extraJumpTrailRenderer.enabled = true;
					additionalJumpParticleEffect.EmitParticleEffects();
					AudioController.PlayOneShot(SourceType.SFX, 0, additionalJumpSoundEffectIndex);
				}
				else
				{
					initialJumpParticleEffect.EmitParticleEffects();
					AudioController.PlayOneShot(SourceType.SFX, 0, initialJumpSoundEffectIndex);
				}

				animatorController.CrossFade(animationHash, clipFadeDuration, _mainAnimationLayer, 0.0f);
			break;

			case JumpAbility.STATE_ID_FALLING:
				//if(_jumpLevel <= 0) jumpAbility.AdvanceJumpIndex();
				animatorController.CrossFade(_fallingCredential, clipFadeDuration, _mainAnimationLayer);
				if(extraJumpTrailRenderer != null)
				{
					extraJumpTrailRenderer.enabled = false;
					extraJumpTrailRenderer.Clear();
				}
			break;

			case JumpAbility.STATE_ID_LANDING:
				if(!this.HasStates(ID_STATE_MEDITATING))
				{
					animatorController.CrossFadeAndWait(
						_softLandingCredential,
						clipFadeDuration,
						_mainAnimationLayer,
						0.0f,
						0.0f,
						OnMainLayerAnimationFinished
					);
					softLandingParticleEffect.EmitParticleEffects();
				}
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
	}

	/// <summary>Callback invoked whn a Dash State changes.</summary>
	/// <param name="_state">New Entered State.</param>
	private void OnDashStateChange(DashState _state)
	{
		if(!this.HasStates(ID_STATE_ALIVE)) return;
	}

	/// <summary>Callback invoked when the MovementAbility does a brake.</summary>
	/// <param name="_braking">Did it brake?.</param>
	private void OnMovementBraking(bool _braking)
	{
		if(!_braking || this.HasStates(ID_STATE_BRAKING)) return;
		
		state |=  ID_STATE_BRAKING;
		animatorController.CrossFadeAndWait(
			_brakeCredential,
			clipFadeDuration,
			_mainAnimationLayer,
			0.0f,
			0.0f,
			()=>
			{	
				OnMainLayerAnimationFinished();
				state &=  ~ID_STATE_BRAKING;
			}
		);
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
	}

	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		base.OnHealthEvent(_event, _amount);

		switch(_event)
		{
			case HealthEvent.Depleted:
				animatorController.CrossFade(_hitCredential, clipFadeDuration, _mainAnimationLayer, 0.0f);
				Game.SetTimeScale(Game.data.hurtTimeScale);
			break;

			case HealthEvent.Replenished:
				ResetAxes();
			break;

			case HealthEvent.HitStunEnds:
				state &= ~ID_STATE_HURT;
				
				if(health.hp > 0.0f)
				ResetAxes();
				
				Game.SetTimeScale(1.0f);
				OnMainLayerAnimationFinished();
			break;

			case HealthEvent.InvincibilityEnds:
				if(health.hp > 0.0f)
				ResetAxes();
			break;

			case HealthEvent.FullyDepleted:
				CancelAllActions();
				this.ChangeState(ID_STATE_DEAD);
				animatorController.CrossFadeAndWait(
					_deadCredential,
					clipFadeDuration,
					_mainAnimationLayer,
					0.0f,
					0.0f,
					()=>
					{
						InvokeIDEvent(ID_EVENT_DEAD);
					}
				);
			break;
		}
	}
#endregion

#region AnimationMethods&Callbacks:
	/// <summary>Goes directly to Locomotion's State.</summary>
	private void GoToLocomotionAnimation()
	{
		//if(this.HasStates(ID_STATE_MOVING)) return;

		state |= ID_STATE_MOVING;
		
		AnimatorCredential animationHash = this.HasStates(ID_STATE_SWORDEQUIPPED) ? _swordLocomotionCredential : _noSwordLocomotionCredential;
		animatorController.CrossFade(animationHash, clipFadeDuration, _mainAnimationLayer, 0.0f);
	}

	/// <summary>Callback internally invoked after an animation from the Main Layer is finished.</summary>
	private void OnMainLayerAnimationFinished()
	{
		animatorController.Play(_emptyCredential, _mainAnimationLayer);
		animator.SetLayerWeight(_mainAnimationLayer, 1.0f);
		if(jumpAbility.grounded && !this.HasStates(ID_STATE_MEDITATING)) GoToLocomotionAnimation();
	}

	/// <summary>Callback internally invoked after an animation from the Attack Layer is finished.</summary>
	private void OnAttackLayerAnimationFinished()
	{
		animator.SetLayerWeight(_attackAnimationLayer, 0.0f);
		animator.SetLayerWeight(_mainAnimationLayer, 1.0f);
		GoToLocomotionAnimation();
	}

	/// <summary>Callback internally invoked after an animation from the Fire Conjuring's Layer is finished.</summary>
	private void OnFireConjuringLayerAnimationFinished()
	{
		animatorController.Play(_emptyCredential, _fireConjuringAnimationLayer);
		animator.SetLayerWeight(_fireConjuringAnimationLayer, 0.0f);
		state &= ~ID_STATE_MOVING;
	}
#endregion
}
}