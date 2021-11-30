using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class DeathBehavior : DestinoScriptableCoroutine
{
	[Space(5f)]
	[SerializeField] private ContactWeapon _scythe; 						/// <summary>Scythe Weapon's Reference.</summary>
	[Space(5f)]
	[SerializeField] private AnimatorCredential _scytheAttackIDCredential; 	/// <summary>Scythe's Attack ID's Credential.</summary>
	[Space(5f)]
	[SerializeField] private Vector3 _pivot; 								/// <summary>Scythe's Pivot.</summary>
	[SerializeField] private NormalizedVector3 _snathOrientationVector; 	/// <summary>Orientation Vector associated with the scythe's snath.</summary>
	[SerializeField] private NormalizedVector3 _scytheRotationAxis; 		/// <summary>Scythe's Rotation Axis.</summary>
	[SerializeField] private EulerRotation _initialScytheRotation; 			/// <summary>Initial Scythe's Rotation.</summary>
	[Space(5f)]
	[Header("Scythe's Rotation Data:")]
	[SerializeField] private float _leftAngle; 								/// <summary>Left orientation's degree.</summary>
	[SerializeField] private float _rightAngle; 							/// <summary>Right orientation's degree.</summary>
	[SerializeField] private float _rotationDuration; 						/// <summary>Rotation's Duration.</summary>
	[Space(5f)]
	[Header("Scythe's Steering Attributes:")]
	[SerializeField] private float _buildUpMaxSpeed; 						/// <summary>Scythe's Max Speed.</summary>
	[SerializeField] private float _buildUpMaxSteeringForce; 				/// <summary>Scythe's Max Steering Force.</summary>
	[SerializeField] private float _swingMaxSpeed; 							/// <summary>Scythe's Max Speed.</summary>
	[SerializeField] private float _swingMaxSteeringForce; 					/// <summary>Scythe's Max Steering Force.</summary>
	[SerializeField] private float _arrivalRadius; 							/// <summary>Steering Arrival's Radius.</summary>
	[Space(5f)]
	[Header("Swings Rotations' Data:")]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _dotProductTolerance; 				/// <summary>Dot Product's Tolerance to reach the desired angle.</summary>
	[SerializeField] private RotationDataSet[] _rotationsDataSet; 			/// <summary>Set of Rotations' Datas.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] public Color color; 									/// <summary>General Gizmos' Color.</summary>
	[SerializeField] public Color buildUpColor; 							/// <summary>Build-Up's Gizmos Color.</summary>
	[SerializeField] public Color swingColor; 								/// <summary>Slash' Gizmos Color.</summary>
	[SerializeField] public float gizmosRadius; 							/// <summary>Gizmos' Radius.</summary>
	[Space(5f)]
	[Header("Test's Settings:")]
	[SerializeField] private bool testDebugIndex; 							/// <summary>Test Debug's Index.</summary>
	[HideInInspector] public int debugIndex; 								/// <summary>RotationsDataSet's on given index to debug.</summary>
	[HideInInspector] public int debugSubIndex; 							/// <summary>RotationData on given sub-index to debug.</summary>
#endif
	private Coroutine scytheRotation; 										/// <summary>Scythe's Rotation Coroutine Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets scythe property.</summary>
	public ContactWeapon scythe { get { return _scythe; } }

	/// <summary>Gets scytheAttackIDCredential property.</summary>
	public AnimatorCredential scytheAttackIDCredential { get { return _scytheAttackIDCredential; } }

	/// <summary>Gets pivot property.</summary>
	public Vector3 pivot { get { return _pivot; } }

	/// <summary>Gets snathOrientationVector property.</summary>
	public NormalizedVector3 snathOrientationVector { get { return _snathOrientationVector; } }

	/// <summary>Gets and Sets scytheRotationAxis property.</summary>
	public NormalizedVector3 scytheRotationAxis
	{
		get { return _scytheRotationAxis; }
		set { _scytheRotationAxis = value; }
	}

	/// <summary>Gets leftAngle property.</summary>
	public float leftAngle { get { return _leftAngle; } }

	/// <summary>Gets rightAngle property.</summary>
	public float rightAngle { get { return _rightAngle; } }

	/// <summary>Gets rotationDuration property.</summary>
	public float rotationDuration { get { return _rotationDuration; } }

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

	/// <summary>Gets dotProductTolerance property.</summary>
	public float dotProductTolerance { get { return _dotProductTolerance; } }

	/// <summary>Gets initialScytheRotation property.</summary>
	public EulerRotation initialScytheRotation { get { return _initialScytheRotation; } }

	/// <summary>Gets rotationsDataSet property.</summary>
	public RotationDataSet[] rotationsDataSet { get { return _rotationsDataSet; } }
#endregion

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
#if UNITY_EDITOR
		base.DrawGizmos();
		
		Gizmos.color = color;
		Gizmos.DrawRay(pivot, snathOrientationVector.normalized * gizmosRadius);
		Gizmos.DrawWireSphere(pivot, gizmosRadius);

		RotationDataSet[] sets = rotationsDataSet;

		if(sets == null || sets.Length <= 0) return;
		
		RotationData data = sets[debugIndex].rotationDataSet[debugSubIndex];
		
		Gizmos.color = buildUpColor;
		Gizmos.DrawRay(pivot, data.buildUpDirection.normalized * gizmosRadius);
		Gizmos.color = swingColor;
		Gizmos.DrawRay(pivot, data.swingDirection.normalized * gizmosRadius);
#endif
	}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{

		bool scytheAttackEnded = false;
		Animator scytheAnimator = scythe.GetComponent<Animator>();
		AnimationAttacksHandler scytheAttacksHandler = scythe.GetComponent<AnimationAttacksHandler>();
		SteeringVehicle2D vehicle = scythe.GetComponent<SteeringVehicle2D>();
		Transform scytheMesh = scythe.transform.GetChild(0);
		IEnumerator rotate = null;
		int dataSetIndex = boss.currentStage;
#if UNITY_EDITOR
		if(testDebugIndex) dataSetIndex = debugIndex;
#endif
		RotationDataSet dataSet = rotationsDataSet[dataSetIndex];
		Action<RotationEvent, int> onRotationEvent = (rotationEvent, ID)=>
		{
			switch(rotationEvent)
			{
				case RotationEvent.BuildUpBegins:
				case RotationEvent.BuildUpEnds:
				case RotationEvent.SwingBegins:
				case RotationEvent.SwingEnds:
				//boss.OnScytheRotationEvent(rotationEvent, ID);
				return;
			}

			Vector3 position = scythe.transform.position;
			Vector3 mateoPosition = Game.mateo.transform.position.WithY(position.y);
			float arrivalWeight = vehicle.GetArrivalWeight(mateoPosition, arrivalRadius);

			switch(rotationEvent)
			{
				case RotationEvent.BuildingUp:
				vehicle.maxSpeed = buildUpMaxSpeed;
				vehicle.maxForce = buildUpMaxSteeringForce;
				break;

				case RotationEvent.Swinging:
				vehicle.maxSpeed = swingMaxSpeed;
				vehicle.maxForce = swingMaxSteeringForce;
				break;
			}

			position += (Vector3)(vehicle.GetSeekForce(mateoPosition) * arrivalWeight * Time.deltaTime);
			scythe.transform.position = position;
		};
		OnAnimationAttackEvent onAnimationAttackEvent = (_state)=>
		{
			switch(_state)
			{
				case AnimationCommandState.None:
				scythe.ActivateHitBoxes(false);
				break;

			    case AnimationCommandState.Startup:
			    scythe.ActivateHitBoxes(false);
			    AudioController.PlayOneShot(SourceType.SFX, 0, boss.buildUpSoundIndex);
			    break;

			    case AnimationCommandState.Active:
			    scythe.ActivateHitBoxes(true);
			    AudioController.PlayOneShot(SourceType.SFX, 0, boss.swingSoundIndex);
			    break;

			    case AnimationCommandState.Recovery:
			    break;

			    case AnimationCommandState.End:
			    scythe.gameObject.SetActive(false);
			    scytheAnimator.SetInteger(scytheAttackIDCredential, 0);
			    scytheAttackEnded = true;
			    break;
			}
		};

		scythe.gameObject.SetActive(true);

		scytheAttacksHandler.onAnimationAttackEvent -= onAnimationAttackEvent;
		scytheAttacksHandler.onAnimationAttackEvent += onAnimationAttackEvent;
		scytheAttacksHandler.BeginAttack(dataSetIndex);
		scytheAnimator.SetInteger(scytheAttackIDCredential, dataSetIndex);

		while(!scytheAttackEnded) yield return null;

		yield return null;
		InvokeCoroutineEnd();
	}

	private void EvaluateScytheRotation(ContactWeapon weapon, Transform scytheMesh, float s)
	{
		float angle = s < 0.0f ? leftAngle : rightAngle;

		weapon.StartCoroutine(scytheMesh.RotateOnAxis(Vector3.down, angle, rotationDuration), ref scytheRotation);
	}
}
}