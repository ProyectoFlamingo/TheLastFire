using Sirenix.OdinInspector;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

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
[RequireComponent(typeof(SlopeEvaluator))]
public class Mateo : Character
{
	[Space(5f)]
	[Header("AnimatorController's Parameters:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _leftAxisXCredential; 										/// <summary>Left Axis X's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _leftAxisYCredential; 										/// <summary>Left Axis Y's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _rightAxisXCredential; 										/// <summary>Right Axis X's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _rightAxisYCredential; 										/// <summary>Right Axis Y's Animator Credential.</summary>
	[Space(5f)]
	[Header("Animation Layers:")]
	[TabGroup("Animations")][SerializeField] private int _mainAnimationLayer; 														/// <summary>Main's Animation Layer.</summary>
	[TabGroup("Animations")][SerializeField] private int _fireConjuringAnimationLayer; 												/// <summary>Fire Conjuring's Animation Layer.</summary>
	[Space(5f)]
	[Header("Layer Weights' Change Durations:")]
	[TabGroup("Animations")][SerializeField] private float _fireConjuringAnimationWeightChangeDuration; 							/// <summary>Duration for the weight-changing or Fire-Conjuring's Animation Layer.</summary>
	[Space(5f)]
	[Header("Animation Clips' Credentials:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _emptyCredential; 											/// <summary>Empty State's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _noSwordLocomotionCredential; 								/// <summary>No-Sword's Locomotion's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _swordLocomotionCredential; 								/// <summary>Sword's Locomotion's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _crouchCredential; 											/// <summary>Crouch's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalMeditationCredential; 								/// <summary>Normal Meditaiton's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _brakeCredential; 											/// <summary>Brake's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _lightHitCredential; 										/// <summary>Light-Hit's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _strongHitCredential; 										/// <summary>Strong-Hit's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _crashCredential; 											/// <summary>Crash's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _deadCredential; 											/// <summary>Dead's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _swordMeditationCredential; 								/// <summary>Sword Meditaiton's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireMeditationCredential; 									/// <summary>Fire Meditaiton's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalStandingCredential; 									/// <summary>Normal Standing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _jumpStandingCredential; 									/// <summary>Jump Standing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalJumpCredential; 										/// <summary>Normal Jump's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _additionalJumpCredential; 									/// <summary>Additional Jump's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fallingCredential; 										/// <summary>Falling's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _softLandingCredential; 									/// <summary>Soft Landing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _hardLandingCredential; 									/// <summary>Hard Landing's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireConjuringCredential; 									/// <summary>Fire Conjuring's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireHoldingCredential; 									/// <summary>Fire Holding's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireMaxChargedCredential; 									/// <summary>Fire's Max Charged's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireThrowCredential; 										/// <summary>Fire Throw's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _swordObtentionCredential; 									/// <summary>Sword Obtention's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _fireObtentionCredential; 									/// <summary>Fire Obtention's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _groundSwordAttackCredential; 								/// <summary>Ground Sword Attack's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _normalJumpSwordAttackCredential; 							/// <summary>Normal Jump Sword Attack's AnimatorCredential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _additionalJumpSwordAttackCredential; 						/// <summary>Additional Jump Sword Attack's AnimatorCredential.</summary>
	[Space(5f)]
	[Header("Cross-Fades:")]
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _locomotionFadeDuration; 								/// <summary>Locmotion Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _crouchFadeDuration; 									/// <summary>Crouch Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _meditationFadeDuration; 								/// <summary>Meditation Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _brakeFadeDuration; 									/// <summary>Brake Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _hitFadeDuration; 									/// <summary>Hit Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _deadFadeDuration; 									/// <summary>Dead Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _meditationActionFadeDuration; 						/// <summary>Meditation Action Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _meditationStandingFadeDuration; 						/// <summary>Meditation Standing Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _jumpFadeDuration; 									/// <summary>Jump Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _fallingFadeDuration; 								/// <summary>Falling Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _landingFadeDuration; 								/// <summary>Landing Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _fireConjuringFadeDuration; 							/// <summary>Fire-Conjuring Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _obtentionFadeDuration; 								/// <summary>Obtention Animation's Fade Duration.</summary>
	[TabGroup("Animations")][SerializeField][Range(0.0f, 1.0f)] private float _attackFadeDuration; 									/// <summary>Attack Animation's Fade Duration.</summary>
	[Space(5f)]
	[Range(0.0f, 0.9f)]
	[TabGroup("Group A", "Movement")][SerializeField] private float _movementAxesThreshold; 										/// <summary>Movement Axes' Magnitude Threshold.</summary>
	[Space(5f)]
	[TabGroup("Group A", "Movement")][SerializeField] private float _crouchDuration; 												/// <summary>Crouch's Duration.</summary>
	[Space(5f)]
	[Header("Sword's Attributes:")]
	[TabGroup("Group A", "Sword Attacks")][SerializeField] private Sword _sword; 													/// <summary>Mateo's Sword.</summary>
	[Space(5f)]
	[TabGroup("Group A", "Sword Attacks")][SerializeField] private FloatRange _directionalThresholdX; 								/// <summary>Directional Threhold on the X's Axis to perform directional attacks.</summary>
	[TabGroup("Group A", "Sword Attacks")][SerializeField] private FloatRange _directionalThresholdY; 								/// <summary>Directional Threhold on the Y's Axis to perform directional attacks.</summary>
	[Space(5f)]
	[TabGroup("Group A", "Sword Attacks")][SerializeField] private float _attackingGravityScale; 									/// <summary>Gravity Scale applied while attacking.</summary>
	[TabGroup("Group A", "Sword Attacks")][SerializeField] private int _attackingScaleChangePriority; 								/// <summary>Scale Change's Priority while attacking.</summary>
	[Space(5f)]
	[Header("Jumping's Attributes:")]
	[TabGroup("Group A", "Jumping")][SerializeField] private TrailRenderer _extraJumpTrailRenderer; 								/// <summary>Extra-Jump's Trail Renderer.</summary>
	[TabGroup("Group A", "Jumping")][SerializeField] private float _gravityScale; 													/// <summary>Gravity Scale applied when attacking.</summary>
	[TabGroup("Group A", "Jumping")][SerializeField] private int _scaleChangePriority; 												/// <summary>Gravity Scale's Change Priority.</summary>
	[Space(5f)]
	[TabGroup("Group A", "Jumping")][SerializeField] private float _jumpingMovementScale; 											/// <summary>Movement's Scale when Mateo is Jumping.</summary>
	[Space(5f)]
	[Header("Meditation's Attributes:")]
	[TabGroup("Group A", "Meditation")][SerializeField] private float _meditationWaitDuration; 										/// <summary>Meditation Wait's Duration.</summary>
	[TabGroup("Group A", "Meditation")][SerializeField] private float _normalStandingAdditionalWait; 								/// <summary>Normal Meditation's Standing Additional Wait Duration.</summary>
	[Space(5f)]
	[SerializeField]
	[TabGroup("Group A", "Dash")][Range(0.0f, 1.0f)] private float _dashXThreshold; 												/// <summary>Minimum left axis' X [absolute] value to be able to perform dash.</summary>
	[Space(5f)]
	[Header("Particle Effects:")]
	[TabGroup("Group B", "Particle Effects")][SerializeField] private ParticleEffectEmissionData _initialJumpParticleEffect; 		/// <summary>Initial Jump's ParticleEffect's Emission Data.</summary>
	[TabGroup("Group B", "Particle Effects")][SerializeField] private ParticleEffectEmissionData _additionalJumpParticleEffect; 	/// <summary>Additional Jump's ParticleEffect's Emission Data.</summary>
	[TabGroup("Group B", "Particle Effects")][SerializeField] private ParticleEffectEmissionData _softLandingParticleEffect; 		/// <summary>Soft-Landing's ParticleEffect's Emission Data.</summary>
	[TabGroup("Group B", "Particle Effects")][SerializeField] private ParticleEffectEmissionData _hardLandingParticleEffect; 		/// <summary>Hard-Landing's ParticleEffect's Emission Data.</summary>
	private FloatWrapper _scalarWrapper;																							/// <summary>Gravity Change's Scalar Wrapper.</summary>
	private float _meditationWaitTime; 																								/// <summary>Current Meditation's Time.</summary>
	private RigidbodyMovementAbility _movementAbility; 																				/// <summary>RigidbodyMovementAbility's Component.</summary>
	private RotationAbility _rotationAbility; 																						/// <summary>RotationAbility's Component.</summary>
	private JumpAbility _jumpAbility; 																								/// <summary>JumpAbility's Component.</summary>
	private ShootChargedProjectile _shootProjectile; 																				/// <summary>ShootChargedProjectile's Component.</summary>
	private DashAbility _dashAbility; 																								/// <summary>DashAbility's Component.</summary>
	private TransformDeltaCalculator _deltaCalculator; 																				/// <summary>TransformDeltaCalculator's Component.</summary>
	private SensorSystem2D _sensorSystem; 																							/// <summary>SensorSystem2D's Component.</summary>
	private WallEvaluator _wallEvaluator; 																							/// <summary>WallEvaluator's Component.</summary>
	private SlopeEvaluator _slopeEvaluator; 																						/// <summary>SlopeEvaluator's Component.</summary>
	private Vector3 _orientation; 																									/// <summary>Mateo's Orientation.</summary>
	private Vector2 _leftAxes; 																										/// <summary>Left Axes' Value.</summary>
	private bool _attackedOnAir; 																									/// <summary>Has Mateo Attacked on Air?.</summary>

#region Getters/Setters:
	/// <summary>Gets sword property.</summary>
	public Sword sword { get { return _sword; } }

	/// <summary>Gets mainAnimationLayer property.</summary>
	public int mainAnimationLayer { get { return _mainAnimationLayer; } }

	/// <summary>Gets fireConjuringAnimationLayer property.</summary>
	public int fireConjuringAnimationLayer { get { return _fireConjuringAnimationLayer; } }

	/// <summary>Gets attackingScaleChangePriority property.</summary>
	public int attackingScaleChangePriority { get { return _attackingScaleChangePriority; } }

	/// <summary>Gets fireConjuringAnimationWeightChangeDuration property.</summary>
	public float fireConjuringAnimationWeightChangeDuration { get { return _fireConjuringAnimationWeightChangeDuration; } }

	/// <summary>Gets locomotionFadeDuration property.</summary>
	public float locomotionFadeDuration { get { return _locomotionFadeDuration; } }

	/// <summary>Gets crouchFadeDuration property.</summary>
	public float crouchFadeDuration { get { return _crouchFadeDuration; } }

	/// <summary>Gets meditationFadeDuration property.</summary>
	public float meditationFadeDuration { get { return _meditationFadeDuration; } }

	/// <summary>Gets brakeFadeDuration property.</summary>
	public float brakeFadeDuration { get { return _brakeFadeDuration; } }

	/// <summary>Gets hitFadeDuration property.</summary>
	public float hitFadeDuration { get { return _hitFadeDuration; } }

	/// <summary>Gets deadFadeDuration property.</summary>
	public float deadFadeDuration { get { return _deadFadeDuration; } }

	/// <summary>Gets meditationActionFadeDuration property.</summary>
	public float meditationActionFadeDuration { get { return _meditationActionFadeDuration; } }

	/// <summary>Gets meditationStandingFadeDuration property.</summary>
	public float meditationStandingFadeDuration { get { return _meditationStandingFadeDuration; } }

	/// <summary>Gets jumpFadeDuration property.</summary>
	public float jumpFadeDuration { get { return _jumpFadeDuration; } }

	/// <summary>Gets fallingFadeDuration property.</summary>
	public float fallingFadeDuration { get { return _fallingFadeDuration; } }

	/// <summary>Gets landingFadeDuration property.</summary>
	public float landingFadeDuration { get { return _landingFadeDuration; } }

	/// <summary>Gets fireConjuringFadeDuration property.</summary>
	public float fireConjuringFadeDuration { get { return _fireConjuringFadeDuration; } }

	/// <summary>Gets obtentionFadeDuration property.</summary>
	public float obtentionFadeDuration { get { return _obtentionFadeDuration; } }

	/// <summary>Gets attackFadeDuration property.</summary>
	public float attackFadeDuration { get { return _attackFadeDuration; } }

	/// <summary>Gets attackingGravityScale property.</summary>
	public float attackingGravityScale { get { return _attackingGravityScale; } }

	/// <summary>Gets movementAxesThreshold property.</summary>
	public float movementAxesThreshold { get { return _movementAxesThreshold; } }

	/// <summary>Gets crouchDuration property.</summary>
	public float crouchDuration { get { return _crouchDuration; } }

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

	/// <summary>Gets and Sets scalarWrapper property.</summary>
	public FloatWrapper scalarWrapper
	{
		get { return _scalarWrapper; }
		set { _scalarWrapper = value; }
	}

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

	/// <summary>Gets emptyCredential property.</summary>
	public AnimatorCredential emptyCredential { get { return _emptyCredential; } }

	/// <summary>Gets noSwordLocomotionCredential property.</summary>
	public AnimatorCredential noSwordLocomotionCredential { get { return _noSwordLocomotionCredential; } }

	/// <summary>Gets swordLocomotionCredential property.</summary>
	public AnimatorCredential swordLocomotionCredential { get { return _swordLocomotionCredential; } }

	/// <summary>Gets crouchCredential property.</summary>
	public AnimatorCredential crouchCredential { get { return _crouchCredential; } }

	/// <summary>Gets normalMeditationCredential property.</summary>
	public AnimatorCredential normalMeditationCredential { get { return _normalMeditationCredential; } }

	/// <summary>Gets brakeCredential property.</summary>
	public AnimatorCredential brakeCredential { get { return _brakeCredential; } }

	/// <summary>Gets lightHitCredential property.</summary>
	public AnimatorCredential lightHitCredential { get { return _lightHitCredential; } }

	/// <summary>Gets strongHitCredential property.</summary>
	public AnimatorCredential strongHitCredential { get { return _strongHitCredential; } }

	/// <summary>Gets crashCredential property.</summary>
	public AnimatorCredential crashCredential { get { return _crashCredential; } }

	/// <summary>Gets deadCredential property.</summary>
	public AnimatorCredential deadCredential { get { return _deadCredential; } }

	/// <summary>Gets swordMeditationCredential property.</summary>
	public AnimatorCredential swordMeditationCredential { get { return _swordMeditationCredential; } }

	/// <summary>Gets fireMeditationCredential property.</summary>
	public AnimatorCredential fireMeditationCredential { get { return _fireMeditationCredential; } }

	/// <summary>Gets normalStandingCredential property.</summary>
	public AnimatorCredential normalStandingCredential { get { return _normalStandingCredential; } }

	/// <summary>Gets jumpStandingCredential property.</summary>
	public AnimatorCredential jumpStandingCredential { get { return _jumpStandingCredential; } }

	/// <summary>Gets normalJumpCredential property.</summary>
	public AnimatorCredential normalJumpCredential { get { return _normalJumpCredential; } }

	/// <summary>Gets additionalJumpCredential property.</summary>
	public AnimatorCredential additionalJumpCredential { get { return _additionalJumpCredential; } }

	/// <summary>Gets fallingCredential property.</summary>
	public AnimatorCredential fallingCredential { get { return _fallingCredential; } }

	/// <summary>Gets softLandingCredential property.</summary>
	public AnimatorCredential softLandingCredential { get { return _softLandingCredential; } }

	/// <summary>Gets hardLandingCredential property.</summary>
	public AnimatorCredential hardLandingCredential { get { return _hardLandingCredential; } }

	/// <summary>Gets fireConjuringCredential property.</summary>
	public AnimatorCredential fireConjuringCredential { get { return _fireConjuringCredential; } }

	/// <summary>Gets fireHoldingCredential property.</summary>
	public AnimatorCredential fireHoldingCredential { get { return _fireHoldingCredential; } }

	/// <summary>Gets fireMaxChargedCredential property.</summary>
	public AnimatorCredential fireMaxChargedCredential { get { return _fireMaxChargedCredential; } }

	/// <summary>Gets fireThrowCredential property.</summary>
	public AnimatorCredential fireThrowCredential { get { return _fireThrowCredential; } }

	/// <summary>Gets swordObtentionCredential property.</summary>
	public AnimatorCredential swordObtentionCredential { get { return _swordObtentionCredential; } }

	/// <summary>Gets fireObtentionCredential property.</summary>
	public AnimatorCredential fireObtentionCredential { get { return _fireObtentionCredential; } }

	/// <summary>Gets groundSwordAttackCredential property.</summary>
	public AnimatorCredential groundSwordAttackCredential { get { return _groundSwordAttackCredential; } }

	/// <summary>Gets normalJumpSwordAttackCredential property.</summary>
	public AnimatorCredential normalJumpSwordAttackCredential { get { return _normalJumpSwordAttackCredential; } }

	/// <summary>Gets additionalJumpSwordAttackCredential property.</summary>
	public AnimatorCredential additionalJumpSwordAttackCredential { get { return _additionalJumpSwordAttackCredential; } }

	/// <summary>Gets initialJumpParticleEffect property.</summary>
	public ParticleEffectEmissionData initialJumpParticleEffect { get { return _initialJumpParticleEffect; } }

	/// <summary>Gets additionalJumpParticleEffect property.</summary>
	public ParticleEffectEmissionData additionalJumpParticleEffect { get { return _additionalJumpParticleEffect; } }

	/// <summary>Gets softLandingParticleEffect property.</summary>
	public ParticleEffectEmissionData softLandingParticleEffect { get { return _softLandingParticleEffect; } }

	/// <summary>Gets hardLandingParticleEffect property.</summary>
	public ParticleEffectEmissionData hardLandingParticleEffect { get { return _hardLandingParticleEffect; } }

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

	/// <summary>Gets slopeEvaluator Component.</summary>
	public SlopeEvaluator slopeEvaluator
	{ 
		get
		{
			if(_slopeEvaluator == null) _slopeEvaluator = GetComponent<SlopeEvaluator>();
			return _slopeEvaluator;
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

	/// <summary>Gets and Sets attackedOnAir property.</summary>
	public bool attackedOnAir
	{
		get { return _attackedOnAir; }
		private set { _attackedOnAir = value; }
	}
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
		if(softLandingParticleEffect != null) softLandingParticleEffect.DrawGizmos();
		if(hardLandingParticleEffect != null) hardLandingParticleEffect.DrawGizmos();
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

		animator.SetAllLayersWeight(0.0f);
		animator.SetLayerWeight(_mainAnimationLayer, 1.0f);
		scalarWrapper = new FloatWrapper(attackingGravityScale);

		//Meditate(true);
		MeditateInstantly();
		shootProjectile.muzzle = skeleton.leftHand;
		
		/// \TODO Load those from SaveData:
		EquipSword(true);
		state |= IDs.STATE_FIRECONJURINGCAPACITY;
	}

	/// <summary>Updates Mateo's instance at each frame.</summary>
	private void Update()
	{
		if(!this.HasStates(IDs.STATE_ALIVE)) return;

		RotateTowardsLeftAxes();

		if(animator == null) return;

		/*bool x = this.HasAnyOfTheStates(IDs.STATE_JUMPING | IDs.STATE_MEDITATING | IDs.STATE_STANDINGUP | IDs.STATE_HURT | IDs.STATE_DEAD | IDs.STATE_ATTACKING | IDs.STATE_BRAKING | IDs.STATE_CROUCHING);
		if(!this.HasStates(IDs.STATE_MEDITATING) && animator.GetLayerWeight(_mainAnimationLayer) > 0.0f && !x)
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
		if(!this.HasStates(IDs.STATE_ALIVE)
		|| this.HasStates(IDs.STATE_HURT)
		|| this.HasStates(IDs.STATE_CROUCHING)
		|| jumpAbility.HasStates(JumpAbility.STATE_ID_LANDING)
		|| (jumpAbility.grounded && this.HasStates(IDs.STATE_ATTACKING))
		|| dashAbility.state == DashState.Dashing
		|| wallEvaluator.state == WallEvaluationEvent.Bouncing
		|| (wallEvaluator.walled && Mathf.Sign(_axes.x) == Mathf.Sign(direction.x))) return;

		Meditate(false);

		if(this.HasStates(IDs.STATE_MEDITATING)) return;

		_scale = VMath.RemapValueToNormalizedRange(Mathf.Abs(_axes.x), 0.0f, movementAxesThreshold);
		float scale = (jumpAbility.HasStates(JumpAbility.STATE_ID_JUMPING) ? jumpingMovementScale : 1.0f) * _scale;

		//transform.rotation = Quaternion.Euler(0.0f, _axes.x < 0.0f ? 180.0f : 0.0f, 0.0f);
		if(!this.HasStates(IDs.STATE_MEDITATING)) movementAbility.Move(slopeEvaluator.normalAdjuster.right.normalized * _axes.magnitude, scale, Space.World);
		slopeEvaluator.normalAdjuster.forward = _axes.x > 0.0f ? Vector3.forward : Vector3.back;
		orientation = _axes.x > 0.0f ? Vector3.right : Vector3.left;

		//if(jumpAbility.grounded) GoToLocomotionAnimation();
	}

	/// \TODO DEPRECATE. Now it is made on a callback...
	/// <summary>Braking's Evaluation.</summary>
	private void BrakingEvaluation()
	{
		if(movementAbility.braking && !this.HasStates(IDs.STATE_BRAKING))
		{
			state |=  IDs.STATE_BRAKING;
			animatorController.CrossFadeAndWait(
				brakeCredential,
				brakeFadeDuration,
				mainAnimationLayer,
				0.0f,
				0.0f,
				()=>
				{	
					OnMainLayerAnimationFinished();
					state &=  ~IDs.STATE_BRAKING;
				}
			);
		}
	}

	/// <summary>Performs Dash.</summary>
	public void Dash()
	{
		if(this.HasStates(IDs.STATE_HURT) || !this.HasStates(IDs.STATE_ALIVE) || Mathf.Abs(leftAxes.x) < Mathf.Abs(dashXThreshold)) return;

		Meditate(false);
		dashAbility.Dash(orientation);
	}
#endregion

#region MeditationMethods:
	/// <summary>Meditates instantly [no Cross-Fading involved].</summary>
	public void MeditateInstantly()
	{
		CancelAllActions();
		state |= IDs.STATE_MEDITATING;
		animatorController.Play(normalMeditationCredential, mainAnimationLayer);
		InvokeIDEvent(IDs.EVENT_MEDITATION_BEGINS);
	}

	/// <summary>Makes Mateo Meditate.</summary>
	/// <param name="_meditate">Meditate? true by default. If false, it ends the meditation.</param>
	public void Meditate(bool _meditate = true, int _contextFlag =  0)
	{
		if(this.HasStates(IDs.STATE_STANDINGUP)) return;

		bool meditating = this.HasStates(IDs.STATE_MEDITATING);

		switch(_meditate)
		{
			case true:
			if(meditating) return;

			CancelAllActions();
			state |= IDs.STATE_MEDITATING;
			animatorController.CrossFade(normalMeditationCredential, meditationFadeDuration, mainAnimationLayer, 0.0f);
			InvokeIDEvent(IDs.EVENT_MEDITATION_BEGINS);
			break;

			case false:
			meditationWaitTime = 0.0f;
			
			if(!meditating) return;

			int standingHash = 0;

			if((_contextFlag | IDs.STATE_JUMPING) == _contextFlag) standingHash = jumpStandingCredential;
			else standingHash = normalStandingCredential;

			state |= IDs.STATE_STANDINGUP;
			animatorController.WaitForCrossFade(standingHash, meditationStandingFadeDuration, mainAnimationLayer, 0.0f,
			()=>
			{
				/// \TODO This is a forced patch to invoke the ceasing of meditation, but it should be done after GoToLocomotionAnimation(...);
				if(standingHash == jumpStandingCredential) InvokeIDEvent(IDs.EVENT_MEDITATION_ENDS);
				
				GoToLocomotionAnimation(()=>
				{
					state &= ~IDs.STATE_MEDITATING;
					state &= ~IDs.STATE_STANDINGUP;
					CancelJump();
					InvokeIDEvent(IDs.EVENT_MEDITATION_ENDS);
				});
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
		if(!this.HasStates(IDs.STATE_MEDITATING)
		|| this.HasStates(IDs.STATE_STANDINGUP)
		|| (_animationCredential == _fireMeditationCredential && !this.HasStates(IDs.STATE_FIRECONJURINGCAPACITY))
		|| (_animationCredential == _swordMeditationCredential && !this.HasStates(IDs.STATE_SWORDEQUIPPED))
		|| animatorController.IsActive(_animationCredential, mainAnimationLayer)) return;

		shootProjectile.OnDischarge();

		if(_animationCredential == fireMeditationCredential && this.HasStates(IDs.STATE_FIRECONJURINGCAPACITY))
		{
			shootProjectile.ID = shootProjectile.chargedProjectileID;
			shootProjectile.CreateProjectile();	
		}

		animatorController.CrossFadeAndWait(
			_animationCredential,
			meditationActionFadeDuration,
			mainAnimationLayer,
			0.0f,
			0.0f,
			()=>
			{
				shootProjectile.OnDischarge();
				animatorController.CrossFade(normalMeditationCredential, clipFadeDuration, mainAnimationLayer, 0.0f);
			}
		);
	}

	/// <summary>Evaluates for Meditation.</summary>
	private void MeditationEvaluation()
	{
		/* Evaluate Meditation if:
			- On Ground
			- Is Moving
			- Is Alive
			- Is not Attacking
			- Is not Hurt
			- Is not Crouching
			- Is not Charging Fire
		*/
		if(jumpAbility.HasStates(JumpAbility.STATE_ID_GROUNDED)
		&& deltaCalculator.deltaPosition.sqrMagnitude == 0.0f
		&& (this.HasStates(IDs.STATE_ALIVE) && !this.HasStates(IDs.STATE_ATTACKING) && !this.HasStates(IDs.STATE_CROUCHING) && !this.HasStates(IDs.STATE_CHARGINGFIRE) && !this.HasStates(IDs.STATE_STANDINGUP)))
		{
			meditationWaitTime += Time.deltaTime;

			if(meditationWaitTime >= meditationWaitDuration && !this.HasStates(IDs.STATE_MEDITATING))
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
			state |= IDs.STATE_SWORDEQUIPPED;
			break;

			case false:
			sword.transform.SetParent(null);
			state &= ~IDs.STATE_SWORDEQUIPPED;
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
			- Mateo has not attacked on air.
		*/
		if(!this.HasStates(IDs.STATE_ATTACKWINDOW)
		&& (!this.HasStates(IDs.STATE_ALIVE)
		|| this.HasStates(IDs.STATE_HURT)
		|| this.HasStates(IDs.STATE_ATTACKING)
		|| wallEvaluator.state == WallEvaluationEvent.Bouncing
		|| jumpAbility.HasStates(JumpAbility.STATE_ID_LANDING)
		|| attackedOnAir)) return;
		
		if(this.HasStates(IDs.STATE_MEDITATING))
		{
			ChangeMeditationPose(_swordMeditationCredential);
			return;
		}

		Meditate(false);
		health.BeginInvincibilityCooldown(); 	/// THIS IS GAY AND TEMPORAL

		int index = 0;
		int animationHash = 0;
		bool applyDirectional = directionalThresholdX.ValueOutside(leftAxes.x) || directionalThresholdY.ValueOutside(leftAxes.y);
		bool grounded = jumpAbility.grounded;

		if(grounded) animationHash = groundSwordAttackCredential;
		else
		{
			attackedOnAir = true;
			animationHash = jumpAbility.GetJumpIndex() > 0 ? additionalJumpSwordAttackCredential : normalJumpSwordAttackCredential;
		}

		state |= IDs.STATE_ATTACKING;
		state &= ~IDs.STATE_ATTACKWINDOW;

		animatorController.DeactivateLayer(mainAnimationLayer);
		animatorController.CrossFadeAndWait(
			animationHash,
			attackFadeDuration,
			mainAnimationLayer,
			0.0f,
			0.0f,
			()=>
			{
				CancelSwordAttack();
				GoToLocomotionAnimation();
			}
		);
		
	}

	/// <summary>Cancels Attacks.</summary>
	public void CancelSwordAttack()
	{
		//hurtBox.SetActive(true);
		health.OnInvincibilityCooldownEnds();
		sword.ActivateHitBoxes(false);
		state &= ~IDs.STATE_ATTACKING;
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
		if(!this.HasStates(IDs.STATE_FIRECONJURINGCAPACITY) || this.HasStates(IDs.STATE_HURT) || !this.HasStates(IDs.STATE_ALIVE)) return;

		if(this.HasStates(IDs.STATE_MEDITATING))
		{
			ChangeMeditationPose(fireMeditationCredential);
			return;
		}

		Meditate(false);

		if(!this.HasStates(IDs.STATE_CHARGINGFIRE))
		{
			state |= IDs.STATE_CHARGINGFIRE;
			animatorController.SetLayerWeight(fireConjuringAnimationLayer, 1.0f, fireConjuringAnimationWeightChangeDuration);
		}
		
		int chargeStateID = shootProjectile.OnCharge(_axes);
		int animationHash = 0;

		switch(chargeStateID)
		{
			case ShootChargedProjectile.STATE_ID_UNCHARGED:
			break;

			case ShootChargedProjectile.STATE_ID_CHARGING:
			animationHash = fireConjuringCredential;
			break;

			case ShootChargedProjectile.STATE_ID_CHARGED:
			animationHash = fireMaxChargedCredential;
			break;

			case ShootChargedProjectile.STATE_ID_RELEASED:
			break;
		}

		animatorController.CrossFade(animationHash, fireConjuringFadeDuration, fireConjuringAnimationLayer);
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
		if(!this.HasStates(IDs.STATE_FIRECONJURINGCAPACITY) || shootProjectile.onCooldown || this.HasStates(IDs.STATE_MEDITATING) || !this.HasStates(IDs.STATE_ALIVE)) return;

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
		if(!this.HasStates(IDs.STATE_FIRECONJURINGCAPACITY) || this.HasAnyOfTheStates(IDs.STATE_HURT | IDs.STATE_MEDITATING) || !this.HasStates(IDs.STATE_ALIVE)) return;

		bool result = shootProjectile.Shoot(skeleton.leftHand.position, _axes);

		if(result)
		{
			animatorController.CrossFadeAndWait(
				fireThrowCredential,
				fireConjuringFadeDuration,
				fireConjuringAnimationLayer,
				0.0f,
				0.0f,
				OnFireConjuringLayerAnimationFinished
			);
		}
		else DischargeFire(result);
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
		if(this.HasAnyOfTheStates(IDs.STATE_HURT | IDs.STATE_STANDINGUP)
		|| !this.HasStates(IDs.STATE_ALIVE)
		|| (jumpAbility.grounded && this.HasStates(IDs.STATE_ATTACKING))) return;

		Meditate(false, IDs.STATE_JUMPING);

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
		if(!this.HasStates(IDs.STATE_ALIVE)
		|| !jumpAbility.grounded
		|| this.HasAnyOfTheStates(IDs.STATE_CROUCHING | IDs.STATE_JUMPING | IDs.STATE_ATTACKING | IDs.STATE_MEDITATING | IDs.STATE_HURT | IDs.STATE_STANDINGUP)) return;

		state |= IDs.STATE_CROUCHING;
		animatorController.CrossFade(crouchCredential, crouchFadeDuration, mainAnimationLayer);
		/*animatorController.CrossFadeAndWait(
			crouchCredential,
			crouchFadeDuration,
			mainAnimationLayer,
			0.0f,
			crouchDuration,
			()=>
			{
				state &= ~IDs.STATE_CROUCHING;
				OnMainLayerAnimationFinished();
			}
		);*/
	}

	/// <summary>Cancels Crouch.</summary>
	public void CancelCrouch()
	{
		if(!this.HasStates(IDs.STATE_CROUCHING)) return;

		state &= ~IDs.STATE_CROUCHING;
		OnMainLayerAnimationFinished();
	}

	/// <summary>Rotates towards Left's Axes.</summary>
	public void RotateTowardsLeftAxes()
	{
		/* Rotate towards give directio if:
			- Not Meditating
			- Not Attacking
		*/
		if(!this.HasStates(IDs.STATE_ALIVE) || this.HasAnyOfTheStates(IDs.STATE_MEDITATING | IDs.STATE_HURT | IDs.STATE_STANDINGUP)) return;

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
	public void StareTowards(StareTarget _target = StareTarget.Background)
	{
		if(animator != null) animator.transform.rotation = _target == StareTarget.Background ? Game.data.stareAtBackgroundRotation : Game.data.stareAtPlayerRotation;
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
		CancelCrouch();
		dashAbility.CancelDash();
		DischargeFire();
		animator.SetAllLayersWeight(0.0f);
		animator.SetLayerWeight(_mainAnimationLayer, 1.0f);
		attackedOnAir = false;

		/// Remove all action flags:
		state &= ~(IDs.STATE_ATTACKING | IDs.STATE_JUMPING | IDs.STATE_CROUCHING | IDs.STATE_CHARGINGFIRE | IDs.STATE_MEDITATING | IDs.STATE_BRAKING | IDs.STATE_STANDINGUP | IDs.STATE_ATTACKWINDOW);

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
		if(!this.HasStates(IDs.STATE_ALIVE)) return;

		if(!this.HasStates(IDs.STATE_ATTACKING))
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
		if(!this.HasStates(IDs.STATE_ALIVE)) return;

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
		if(!this.HasStates(IDs.STATE_ALIVE)) return;

		switch(_stateID)
		{
			case JumpAbility.STATE_ID_GROUNDED:
				CancelSwordAttack();
				OnMainLayerAnimationFinished();
				attackedOnAir = false;
				if(extraJumpTrailRenderer != null) extraJumpTrailRenderer.enabled = false;
			break;

			case JumpAbility.STATE_ID_JUMPING:
				int animationHash = normalJumpCredential;
				
				if(jumpAbility.GetJumpIndex() > 0 && extraJumpTrailRenderer != null)
				{
					animationHash = additionalJumpCredential;
					extraJumpTrailRenderer.Clear();
					extraJumpTrailRenderer.enabled = true;
					additionalJumpParticleEffect.EmitParticleEffects();
				}
				else
				{
					initialJumpParticleEffect.EmitParticleEffects();
				}

				animatorController.CrossFade(animationHash, jumpFadeDuration, mainAnimationLayer, 0.0f);
			break;

			case JumpAbility.STATE_ID_FALLING:
				//if(_jumpLevel <= 0) jumpAbility.AdvanceJumpIndex();
				animatorController.CrossFade(fallingCredential, fallingFadeDuration, mainAnimationLayer);
				if(extraJumpTrailRenderer != null)
				{
					extraJumpTrailRenderer.enabled = false;
					extraJumpTrailRenderer.Clear();
				}
			break;

			case JumpAbility.STATE_ID_LANDING:
				if(!this.HasStates(IDs.STATE_MEDITATING) || this.HasStates(IDs.STATE_STANDINGUP))
				{
					if(animatorController.CancelCrossFading(mainAnimationLayer))
					{
						state &= ~(IDs.STATE_MEDITATING | IDs.STATE_STANDINGUP);
					}
					animatorController.CrossFade(softLandingCredential, landingFadeDuration, mainAnimationLayer);
					/*animatorController.CrossFadeAndWait(
						softLandingCredential,
						landingFadeDuration,
						mainAnimationLayer,
						0.0f,
						0.0f,
						OnMainLayerAnimationFinished
					);*/
					softLandingParticleEffect.EmitParticleEffects();
				}
			break;
		}

	}

	/// <summary>Callback invoked when a WallEvaluator's event occurs.</summary>
	/// <param name="_event">Event's argument.</param>
	private void OnWallEvaluatorEvent(WallEvaluationEvent _event)
	{
		if(!this.HasStates(IDs.STATE_ALIVE)) return;

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
		if(!this.HasStates(IDs.STATE_ALIVE)) return;
	}

	/// <summary>Callback invoked when the MovementAbility does a brake.</summary>
	/// <param name="_braking">Did it brake?.</param>
	private void OnMovementBraking(bool _braking)
	{
		if(!this.HasStates(IDs.STATE_ALIVE) || !_braking || this.HasStates(IDs.STATE_BRAKING)) return;
		
		/// \TODO Deprecate
		return;

		state |=  IDs.STATE_BRAKING;
		animatorController.CrossFadeAndWait(
			brakeCredential,
			brakeFadeDuration,
			mainAnimationLayer,
			0.0f,
			0.0f,
			()=>
			{	
				state &=  ~IDs.STATE_BRAKING;
				OnMainLayerAnimationFinished();
			}
		);
	}

	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		switch(_event)
		{
			case HealthEvent.Depleted:
				int animationHash = _amount > 1.0f ? strongHitCredential : lightHitCredential;
				state |= IDs.STATE_HURT;
				CancelAllActions();
				animatorController.CrossFade(animationHash, hitFadeDuration, mainAnimationLayer, 0.0f);
				Game.SetTimeScale(Game.data.hurtTimeScale);
			break;

			case HealthEvent.Replenished:
				ResetAxes();
			break;

			case HealthEvent.HitStunEnds:
				state &= ~IDs.STATE_HURT;
				
				if(health.hp > 0.0f)
				ResetAxes();
				
				Game.SetTimeScale(1.0f);
				OnMainLayerAnimationFinished();
			break;

			case HealthEvent.InvincibilityEnds:
				state &= ~IDs.STATE_HURT;
				if(health.hp > 0.0f)
				ResetAxes();
			break;

			case HealthEvent.FullyDepleted:
				if(!this.HasStates(IDs.STATE_ALIVE)) return;

				CancelAllActions();
				this.ChangeState(IDs.STATE_DEAD);
				animatorController.CrossFadeAndWait(
					deadCredential,
					deadFadeDuration,
					mainAnimationLayer,
					0.0f,
					0.0f,
					()=>
					{
						InvokeIDEvent(IDs.EVENT_DEAD);
					}
				);
			break;
		}
	}

	/// <summary>Callback invoked when an Animation Event is invoked.</summary>
	/// <param name="_ID">Int argument.</param>
	protected override void OnAnimationIntEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.ANIMATIONEVENT_DEACTIVATEHITBOXES:
				sword.ActivateHitBoxes(false);
				jumpAbility.gravityApplier.RejectScaleChange(GetInstanceID());
			break;

			case IDs.ANIMATIONEVENT_ACTIVATEHITBOXES:
				sword.ActivateHitBoxes(true);
				jumpAbility.gravityApplier.RequestScaleChange(GetInstanceID(), _scalarWrapper, attackingScaleChangePriority);
			break;

			case IDs.ANIMATIONEVENT_ATTACKEND:
				GoToLocomotionAnimation();
			break;

			case IDs.ANIMATIONEVENT_ATTACKWINDOW:
				Debug.Log("[Mateo] On Attack Window");
				state |= IDs.STATE_ATTACKWINDOW;
			break;
		}
	}
#endregion

#region AnimationMethods&Callbacks:
	/// <summary>Goes directly to Locomotion's State.</summary>
	private void GoToLocomotionAnimation()
	{
		if(!jumpAbility.grounded || this.HasAnyOfTheStates(IDs.STATE_HURT | IDs.STATE_MEDITATING)) return;
		//Debug.Log("[Mateo] Entering Locomotion..");
		state |= IDs.STATE_MOVING;
		
		AnimatorCredential animationHash = this.HasStates(IDs.STATE_SWORDEQUIPPED) ? swordLocomotionCredential : noSwordLocomotionCredential;
		animatorController.CrossFade(animationHash, locomotionFadeDuration, mainAnimationLayer, 0.0f);
	}

	/// <summary>Goes directly to Locomotion's State.</summary>
	/// <param name="onCrossFadeEnds">Optional callback invoked when it reached the Cross-Fade's end.</param>
	private void GoToLocomotionAnimation(Action onCrossFadeEnds = null)
	{
		state |= IDs.STATE_MOVING;
		
		AnimatorCredential animationHash = this.HasStates(IDs.STATE_SWORDEQUIPPED) ? swordLocomotionCredential : noSwordLocomotionCredential;
		animatorController.WaitForCrossFade(animationHash, locomotionFadeDuration, mainAnimationLayer, 0.0f, onCrossFadeEnds);	
	}

	/// <summary>Callback internally invoked after an animation from the Main Layer is finished.</summary>
	private void OnMainLayerAnimationFinished()
	{
		if(jumpAbility.grounded) GoToLocomotionAnimation();
	}

	/// <summary>Callback internally invoked after an animation from the Attack Layer is finished.</summary>
	private void OnAttackLayerAnimationFinished()
	{
		if(jumpAbility.grounded) GoToLocomotionAnimation();
		else OnJumpStateChange(jumpAbility.state, jumpAbility.GetJumpIndex());
	}

	/// <summary>Callback internally invoked after an animation from the Fire Conjuring's Layer is finished.</summary>
	private void OnFireConjuringLayerAnimationFinished()
	{
		animatorController.Play(emptyCredential, fireConjuringAnimationLayer);
		
		if(this.HasStates(IDs.STATE_CHARGINGFIRE))
		{
			animatorController.SetLayerWeight(fireConjuringAnimationLayer, 0.0f, fireConjuringAnimationWeightChangeDuration);
			state &= ~IDs.STATE_CHARGINGFIRE;
		}
	}
#endregion
}
}