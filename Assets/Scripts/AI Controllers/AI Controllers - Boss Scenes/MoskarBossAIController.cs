using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.AddressableAssets;

/*
 - Calculate the total of Moskars to destroy:
 
	2 ^ 0 = 1
	2 ^ 1 = 2
	2 ^ 2 = 4
	2 ^ 3 = 8
	2 ^ 4 = 16
	Total = 31
*/

namespace Flamingo
{
public class MoskarBossAIController : CharacterAIController<MoskarBoss>
{
	public event OnCharacterDeactivated onMoskarDeactivated; 		/// <summary>OnMoskarDeactivated's event. Triggered when any moskar invokes the event of the same type.</summary>

	[Space(5f)]
	[SerializeField] private VAssetReference _moskarReference; 		/// <summary>Moskar's Reference.</summary>
	[Space(5f)]
	[Header("Reproductions' Attributes:")]
	[SerializeField] private FloatRange _scaleRange; 				/// <summary>Scale's Range.</summary>
	[SerializeField] private float _reproductionDuration; 			/// <summary>Reproduction Duration. Determines how long it lasts the reproduction's displacement and scaling.</summary>
	[SerializeField] private float _reproductionPushForce; 			/// <summary>Reproduction's Push Force.</summary>
	[Space(5f)]
	[Header("Warning's Attributes:")]
	[SerializeField] private float _warningSpeed; 					/// <summary>Warning's Steering Speed.</summary>
	[SerializeField] private float _dangerRadius; 					/// <summary>Danger's Radius.</summary>
	[SerializeField] private float _fleeDistance; 					/// <summary>Flee distance between Moskar and Mateo.</summary>
	[Space(5f)]
	[Header("Evasion Attributes: ")]
	[SerializeField] private FloatRange _evasionSpeed; 				/// <summary>Evasion's Speed's Range.</summary>
	[Space(5f)]
	[Header("Introduction's Attributes:")]
	[SerializeField] private float _waitBeforeTaunt; 				/// <summary>Seconds to wait before Taunting.</summary>
	[SerializeField] private float _waitBeforeEndingTaunt; 			/// <summary>Seconds to wait before ceasing taunt.</summary>
	[SerializeField] private float _waitBeforeIntro; 				/// <summary>Seconds to wait before beginning the Introduction.</summary>
	[SerializeField] private IntRange _waypointsGeneration; 		/// <summary>Waypoints Generation's Range.</summary>
	[Space(5f)]
	[SerializeField] private IntRange _shootInterval; 				/// <summary>Shooting's Interval.</summary>
	[SerializeField] private IntRange _fireBursts; 					/// <summary>Fire Burst per Shooting.</summary>
	[Space(5f)]
	[Header("Steering's Attributes:")]
	[Space(5f)]
	[Header("Wander Attributes: ")]
	[SerializeField] private FloatRange _wanderSpeed; 				/// <summary>Wander's Max Speed's Range.</summary>
	[SerializeField] private FloatRange _wanderInterval; 			/// <summary>Wander interval between each angle change [as a range].</summary>
	private Dictionary<int, MoskarBoss> _reproductions; 			/// <summary>Moskar's Reproductions.</summary>
	private float _phaseProgress; 									/// <summary>Phase's Normalized Progress.</summary>
	private float _speedScale; 										/// <summary>Additional Speed's Scale.</summary>
	private float _totalMoskars; 									/// <summary>Total Moskars that will be reproduced.</summary>
	private float _moskarsDestroyed; 								/// <summary>Moskar Reproductions destroyed.</summary>
	private Boundaries2DContainer _boundaries; 						/// <summary>Boundaries' Container.</summary>
	private Vector3[] _waypoints; 									/// <summary>Waypoints.</summary>
	private Coroutine attackCoroutine; 								/// <summary>AttackBehavior's Coroutine reference.</summary>
	private Coroutine rotationCoroutine; 							/// <summary>Rotation Coroutine's Reference.</summary>
	private Dictionary<int, Coroutine> attackCoroutines; 			/// <summary>Attack Coroutines for each Moskar reproduction.</summary>
	private Dictionary<int, Coroutine> rotationCoroutines; 			/// <summary>Rotation Coroutines for each Moskar reproduction.</summary>

#region Getters/Setters:
	/// <summary>Gets moskarReference property.</summary>
	public VAssetReference moskarReference { get { return _moskarReference; } }

	/// <summary>Gets waypointsGeneration property.</summary>
	public IntRange waypointsGeneration { get { return _waypointsGeneration; } }

	/// <summary>Gets shootInterval property.</summary>
	public IntRange shootInterval { get { return _shootInterval; } }

	/// <summary>Gets fireBursts property.</summary>
	public IntRange fireBursts { get { return _fireBursts; } }

	/// <summary>Gets scaleRange property.</summary>
	public FloatRange scaleRange { get { return _scaleRange; } }

	/// <summary>Gets and Sets wanderSpeed property.</summary>
	public FloatRange wanderSpeed { get { return _wanderSpeed; } }

	/// <summary>Gets and Sets evasionSpeed property.</summary>
	public FloatRange evasionSpeed { get { return _evasionSpeed; } }

	/// <summary>Gets and Sets wanderInterval property.</summary>
	public FloatRange wanderInterval { get { return _wanderInterval; } }

	/// <summary>Gets waitBeforeTaunt property.</summary>
	public float waitBeforeTaunt { get { return _waitBeforeTaunt; } }

	/// <summary>Gets waitBeforeEndingTaunt property.</summary>
	public float waitBeforeEndingTaunt { get { return _waitBeforeEndingTaunt; } }

	/// <summary>Gets waitBeforeIntro property.</summary>
	public float waitBeforeIntro { get { return _waitBeforeIntro; } }

	/// <summary>Gets and Sets warningSpeed property.</summary>
	public float warningSpeed
	{
		get { return _warningSpeed; }
		set { _warningSpeed = value; }
	}

	/// <summary>Gets and Sets dangerRadius property.</summary>
	public float dangerRadius
	{
		get { return _dangerRadius; }
		set { _dangerRadius = value; }
	}

	/// <summary>Gets and Sets fleeDistance property.</summary>
	public float fleeDistance
	{
		get { return _fleeDistance; }
		set { _fleeDistance = value; }
	}

	/// <summary>Gets and Sets speedScale property.</summary>
	public float speedScale
	{
		get { return _speedScale; }
		set { _speedScale = value; }
	}

	/// <summary>Gets reproductionDuration property.</summary>
	public float reproductionDuration { get { return _reproductionDuration; } }

	/// <summary>Gets reproductionPushForce property.</summary>
	public float reproductionPushForce { get { return _reproductionPushForce; } }

	/// <summary>Gets and Sets totalMoskars property.</summary>
	public float totalMoskars
	{
		get { return _totalMoskars; }
		private set { _totalMoskars = value; }
	}
	
	/// <summary>Gets and Sets phaseProgress property.</summary>
	public float phaseProgress
	{
		get { return _phaseProgress; }
		set { _phaseProgress = value; }
	}

	/// <summary>Gets and Sets moskarsDestroyed property.</summary>
	public float moskarsDestroyed
	{
		get { return _moskarsDestroyed; }
		private set { _moskarsDestroyed = value; }
	}

	/// <summary>Gets and Sets reproductions property.</summary>
	public Dictionary<int, MoskarBoss> reproductions
	{
		get { return _reproductions; }
		private set { _reproductions = value; }
	}

	/// <summary>Gets and Sets boundaries property.</summary>
	public Boundaries2DContainer boundaries
	{
		get { return _boundaries; }
		set { _boundaries = value; }
	}

	/// <summary>Gets and Sets waypoints property.</summary>
	public Vector3[] waypoints
	{
		get { return _waypoints; }
		private set { _waypoints = value; }
	}
#endregion

#region UnityMethods:

	/// <summary>Draws Gizmos on Editor mode when MoskarBossAIController's instance is selected.</summary>
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();

		if(waypoints == null) return;

		foreach(Vector3 waypoint in waypoints)
		{
			Gizmos.DrawWireSphere(waypoint, 0.2f);
		}
	}

	/// <summary>MoskarBossAIController's instance initialization.</summary>
	protected override void Awake()
	{
		if(character == null) return;		

		Vector3 scale = Vector3.one * scaleRange.Max();

		character.sightSensor.enabled = true;
		character.meshParent.transform.localScale = scale;
		reproductions = new Dictionary<int, MoskarBoss>();

		totalMoskars = 0.0f;
		speedScale = 1.0f; 	/// I Dunno What to do with it...

		for(float i = 0.0f; i < character.phases; i++)
		{
			totalMoskars += Mathf.Pow(2.0f, i);
		}

		base.Awake();
	}

	/// <summary>MoskarBossAIController's starting actions before 1st Update frame.</summary>
	protected override void Start ()
	{
		base.Start();
		character.sightSensor.gameObject.SetActive(false);
		character.EnablePhysicalColliders(false);
		character.EnableTriggerColliders(false);
		character.EnableHurtBoxes(false);
		this.StartCoroutine(IntroductionRoutine(character), ref behaviorCoroutine);
		Game.mateo.eventsHandler.onIDEvent += OnMateoIDEvent;
	}
	
	/// <summary>MoskarBossAIController's tick at each frame.</summary>
	protected override void Update ()
	{
		base.Update();
	}

	/// <summary>Updates MoskarBossAIController's instance at each Physics Thread's frame.</summary>
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		FlockBehavior();
		ContainReproductionsOnScenario();
	}
#endregion

#region Methods:
	/// <summary>Subscribes to Character's Events [Deactivation, ID and State events].</summary>
	/// <param name="_character">Target Character.</param>
	/// <param name="_subscribe">Subscribe? true by default.</param>
	protected override void SubscribeToCharacterEvents(MoskarBoss _character, bool _subscribe = true)
	{
		int instanceID = _character.GetInstanceID();

		if(_character == null) return;

		base.SubscribeToCharacterEvents(_character, _subscribe);

		switch(_subscribe)
		{
			case true:
			reproductions.Add(instanceID, _character);
			break;

			case false:
			reproductions.Remove(instanceID);
			break;
		}
	}

	/// <summary>Contains all Moskar reproductions inside boundaries.</summary>
	private void ContainReproductionsOnScenario()
	{
		if(boundaries == null) return;

		Debug.Log("[MoskarBossAIController] Current Reproductions' Size: " + reproductions.Count);

		foreach(MoskarBoss moskar in reproductions.Values)
		{
			boundaries.ContainInsideBoundaries(moskar.rigidbody, Axes3D.XAndY);
		}
	}

	/// <summary>Creates a Moskar Reproduction.</summary>
	/// <param name="_moskar">Moskar reference that preceeds the reproductions.</param>
	private void CreateMoskarReproductions(MoskarBoss _moskar)
	{
		MoskarBoss reproduction = null;
		Vector3[] forces = new Vector3[] { Vector3.left * reproductionPushForce, Vector3.right * reproductionPushForce };
		TimeConstrainedForceApplier2D[] reproductionPushes = new TimeConstrainedForceApplier2D[2];
		int phase = _moskar.currentPhase;
		float phases = 1.0f * _moskar.phases;
		
		phase++;

		float t = phase / (phases - 1.0f);
		float it = 1.0f - t;
		float sizeScale = scaleRange.Lerp(it);
		Vector3 scale = Vector3.one * sizeScale;

		
		_moskar.duplicateParticleEffect.EmitParticleEffects();

		for(int i = 0; i < 2; i++)
		{
			reproduction = PoolManager.RequestCharacter(moskarReference, _moskar.transform.position, _moskar.transform.rotation) as MoskarBoss;

			if(reproduction == null) return;

			SubscribeToCharacterEvents(reproduction, true);
			reproduction.ChangeState(0);
			reproduction.AddStates(IDs.STATE_ATTACKING);
			reproduction.AddStates(IDs.STATE_ALIVE);
			reproduction.currentPhase = phase;
			reproduction.health.BeginInvincibilityCooldown();
			reproduction.sightSensor.gameObject.SetActive(false);
			reproduction.meshParent.localScale = scale;
			reproduction.phaseProgress = t;
			Game.AddTargetToCamera(reproduction.cameraTarget);

			reproductionPushes[i] = new TimeConstrainedForceApplier2D(this, reproduction.rigidbody, forces[i], reproductionDuration, ForceMode.VelocityChange, reproduction.SimulateInteractionsAndResetVelocity);

			reproduction.StartCoroutine(reproduction.meshParent.RegularScale(sizeScale, reproductionDuration));
			reproductionPushes[i].ApplyForce();
		}
	}

	/// <summary>Makes all Moskar reproductions emulate a flocking steering behavior.</summary>
	private void FlockBehavior()
	{

	}

	/// <summary>Enters Wander State.</summary>
	/// <param name="_moskar">Moskar Reproduction that will enter that state.</param>
	private void EnterWanderState(MoskarBoss _moskar)
	{
		int instanceID = _moskar.GetInstanceID();

		_moskar.DispatchCoroutine(ref _moskar.attackCoroutine);

		_moskar.vehicle.maxSpeed = wanderSpeed.Lerp(phaseProgress) * speedScale;
		_moskar.sightSensor.gameObject.SetActive(true);
		//_moskar.animator.SetInteger(_moskar.locomotionIDCredential, ID_LOCOMOTION_WALK);

		//_moskar.StartCoroutine(_moskar.meshParent.PivotToRotation(_moskar.walkingRotation, _moskar.rotationDuration, TransformRelativeness.Local), ref _moskar.rotationCoroutine);
		_moskar.StartCoroutine(WanderBehaviour(_moskar), ref _moskar.behaviorCoroutine);
	}

	/// <summary>Enters Warning State.</summary>
	/// <param name="_moskar">Moskar Reproduction that will enter that state.</param>
	private void EnterWarningState(MoskarBoss _moskar)
	{
		_moskar.vehicle.maxSpeed = warningSpeed;

		_moskar.DispatchCoroutine(ref _moskar.attackCoroutine);

		/// \TODO Fix Warning's Behaviour...
		//_moskar.StartCoroutine(WarningBehavior(_moskar), ref _moskar.behaviorCoroutine);
		_moskar.StartCoroutine(WanderBehaviour(_moskar), ref _moskar.behaviorCoroutine);
	}

	/// <summary>Enters Attack State.</summary>
	/// <param name="_moskar">Moskar Reproduction that will enter that state.</param>
	private void EnterAttackState(MoskarBoss _moskar)
	{
		_moskar.vehicle.maxSpeed = evasionSpeed.Lerp(phaseProgress);
		_moskar.sightSensor.gameObject.SetActive(false);
		_moskar.animatorController.CrossFade(_moskar.flyCredential, _moskar.clipFadeDuration);

		_moskar.DispatchCoroutine(ref _moskar.behaviorCoroutine);

		_moskar.StartCoroutine(_moskar.meshParent.PivotToRotation(_moskar.flyingRotation, _moskar.rotationDuration, TransformRelativeness.Local), ref _moskar.rotationCoroutine);
		_moskar.StartCoroutine(ErraticFlyingBehavior(_moskar), ref _moskar.behaviorCoroutine);
		_moskar.StartCoroutine(AttackBehavior(_moskar), ref _moskar.attackCoroutine);
	}
#endregion

#region Callbacks:
	/// <summary>Callback invoked when a Character's state is changed.</summary>
	/// <param name="_character">Character that invokes the event.</param>
	/// <param name="_flags">State Flags.</param>
	/// <param name="_stateChange">Type of State Change.</param>
	protected override void OnCharacterStateChanged(Character _character, int _state, StateChange _stateChange)
	{
		MoskarBoss moskar = _character as MoskarBoss;

		if(moskar == null) return;

		int state = moskar.state;

		switch(_stateChange)
		{
			case StateChange.Added:
				if((_state | IDs.STATE_IDLE) == _state)
				{ /// Wander Coroutine:
					EnterWanderState(moskar);
				
				} else if((_state | IDs.STATE_TARGETONSIGHT) == _state)
				{ /// Warning Coroutine:
					EnterAttackState(moskar);
					//EnterWarningState();

				} else if((_state | IDs.STATE_ATTACKING) == _state)
				{ /// Attack Coroutine:
					EnterAttackState(moskar);
				} 
			break;

			case StateChange.Removed:
				if((_state | IDs.STATE_TARGETONSIGHT) == _state
				&& (state | IDs.STATE_ATTACKING) != state
				&& (state | IDs.STATE_IDLE) != state
				&& moskar.sightSensor.enabled)
				{ /// If the Player got out of sight, but Moskar is not Attacking and not on Wander:
					EnterWanderState(moskar);
				}

				if((_state | IDs.STATE_ALIVE) == _state)
				{
					Debug.Log("[MoskarBossAIController] DEAD. Dispatching Coroutines...");
					moskar.DispatchCoroutine(ref moskar.behaviorCoroutine);
					moskar.DispatchCoroutine(ref moskar.attackCoroutine);
				}

				if((_state | IDs.STATE_ATTACKING) == _state)
				{
					//Used to call the animations here...
				}
			break;
		}
	}

	/// <summary>Callback invoked when the character invokes an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected override void OnCharacterIDEvent(int _ID) { /*...*/ }

	/// <summary>Callback invoked when the character is deactivated.</summary>
	/// <param name="_character">Character that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	protected override void OnCharacterDeactivated(Character _character, DeactivationCause _cause, Trigger2DInformation _info)
	{
		if(_cause != DeactivationCause.Destroyed) return;

		MoskarBoss moskar = _character as MoskarBoss;
		int instanceID = moskar.GetInstanceID();

		if(moskar == null) return;

		Debug.Log("[MoskarBossAIController] Deactivated on its phase " + moskar.currentPhase);
		SubscribeToCharacterEvents(moskar, false);
		Game.RemoveTargetToCamera(moskar.cameraTarget);

		if(moskar.currentPhase < (moskar.phases - 1)) CreateMoskarReproductions(moskar);

		moskarsDestroyed++;
		if(onMoskarDeactivated != null) onMoskarDeactivated(_character, _cause, _info);
	}

	/// <summary>Callback invoked when Mateo invokes an ID Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnMateoIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_MEDITATION_BEGINS:
			foreach(MoskarBoss moskar in reproductions.Values)
			{
				moskar.RemoveStates(IDs.STATE_ATTACKING);
				moskar.AddStates(IDs.STATE_IDLE);
			}
			break;
		}
	}
#endregion

#region Coroutines:
	/// <summary>Introduction's Routine.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Introduction Behavior.</param>
	private IEnumerator IntroductionRoutine(MoskarBoss _moskar)
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);

		_moskar.animator.SetLayerWeight(_moskar.introAnimationLayer, 1.0f);
		_moskar.animator.SetLayerWeight(_moskar.mainAnimationLayer, 1.0f);
		_moskar.animatorController.Play(_moskar.introCredential, _moskar.introAnimationLayer);
		_moskar.animatorController.Play(_moskar.flyCredential, _moskar.mainAnimationLayer);
	
		wait.ChangeDurationAndReset(waitBeforeTaunt);
		while(wait.MoveNext()) yield return null;

		_moskar.animatorController.CrossFade(_moskar.taunt2Credential, _moskar.clipFadeDuration);

		wait.ChangeDurationAndReset(waitBeforeEndingTaunt);
		while(wait.MoveNext()) yield return null;

		_moskar.animatorController.CrossFade(_moskar.flyCredential, _moskar.clipFadeDuration);
	
		wait.ChangeDurationAndReset(waitBeforeIntro);
		while(wait.MoveNext()) yield return null;

		_moskar.animator.SetAllLayersWeight(0.0f);
		_moskar.AddStates(IDs.STATE_IDLE);
		_moskar.animatorController.CrossFade(_moskar.idleCredential, _moskar.clipFadeDuration);

		/// Previously called inside Start():
		if(_moskar.currentPhase == 0)
		_moskar.AddStates(IDs.STATE_IDLE);

		_moskar.transform.position = _moskar.animator.transform.position;
		_moskar.animator.transform.localPosition = Vector3.zero;
		character.sightSensor.gameObject.SetActive(true);
		character.EnablePhysicalColliders(true);
		character.EnableTriggerColliders(true);
		character.EnableHurtBoxes(true);
		Game.AddTargetToCamera(_moskar.cameraTarget);

		_moskar.StartCoroutine(WanderBehaviour(_moskar), ref _moskar.behaviorCoroutine);
	}

	/// <summary>Wander's Steering Beahviour Coroutine.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Wander Behavior.</param>
	private IEnumerator WanderBehaviour(MoskarBoss _moskar)
	{
		SecondsDelayWait wait = new SecondsDelayWait(wanderInterval.Random());
		Vector3 wanderForce = Vector3.zero;
		float minDistance = 0.5f * 0.5f;

		while(true)
		{
			wanderForce = _moskar.vehicle.GetWanderForce();
			Vector3 direction = wanderForce - _moskar.transform.position;
			while(wait.MoveNext())
			{
				if(direction.sqrMagnitude > minDistance)
				{
					Vector3 force = _moskar.vehicle.GetSeekForce(wanderForce);

					_moskar.rigidbody.MoveIn3D(force * Time.fixedDeltaTime);
					_moskar.transform.rotation = Quaternion.RotateTowards(_moskar.transform.rotation, VQuaternion.LookRotation(force), _moskar.rotationSpeed * Time.deltaTime);
					direction = wanderForce - _moskar.transform.position;
				}

				yield return VCoroutines.WAIT_PHYSICS_THREAD;
			}

			wait.ChangeDurationAndReset(wanderInterval.Random());
		}
	}

	/// <summary>Warning's Behavior.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Warning Behavior.</param>
	private IEnumerator WarningBehavior(MoskarBoss _moskar)
	{
		TransformDeltaCalculator deltaCalculator = Game.mateo.deltaCalculator;
		Vector3 projectedMateoPosition = Vector3.zero;
		Vector3 direction = Vector3.zero;
		Vector3 fleeForce = Vector3.zero;
		float radius = dangerRadius * dangerRadius;
		float distance = fleeDistance * fleeDistance;
		float magnitude = 0.0f;

		while(true)
		{
			projectedMateoPosition = Game.ProjectMateoPosition(projectionTime * Time.deltaTime);
			direction = projectedMateoPosition - transform.position;
			magnitude = direction.sqrMagnitude;

			if(magnitude < distance)
			{
				fleeForce = _moskar.vehicle.GetFleeForce(projectedMateoPosition);
				_moskar.rigidbody.MoveIn3D(fleeForce * Time.fixedDeltaTime);
				_moskar.transform.rotation = Quaternion.RotateTowards(_moskar.transform.rotation, VQuaternion.LookRotation(fleeForce), _moskar.rotationSpeed * Time.deltaTime);
			}

			if(magnitude <= radius) _moskar.AddStates(IDs.STATE_ATTACKING);

			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}
	}

	/// <summary>Performs Erratic Flying's Behavior.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Erratic-Flying Behavior.</param>
	private IEnumerator ErraticFlyingBehavior(MoskarBoss _moskar)
	{
		waypoints = new Vector3[waypointsGeneration.Random()];

		float distance = minDistanceToReachTarget * minDistanceToReachTarget;

		while(true)
		{
			for(int i = 0; i < waypoints.Length; i++)
			{
				waypoints[i] = boundaries.Random();
			}

			foreach(Vector3 waypoint in waypoints)
			{
				Vector3 direction = waypoint - _moskar.transform.position;
				
				while(direction.sqrMagnitude > distance)
				{
					Vector3 seekForce = _moskar.vehicle.GetSeekForce(waypoint);
					_moskar.rigidbody.MoveIn3D(seekForce * Time.fixedDeltaTime);
					_moskar.transform.rotation = Quaternion.RotateTowards(_moskar.transform.rotation, Quaternion.LookRotation(seekForce), _moskar.rotationSpeed * Time.deltaTime);
					direction = waypoint - _moskar.transform.position;

					yield return VCoroutines.WAIT_PHYSICS_THREAD;
				}
			}
		}	

		_moskar.AddStates(IDs.STATE_IDLE);
	}

	/// <summary>Attack Behavior's Coroutine.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Attack Behavior.</param>
	private IEnumerator AttackBehavior(MoskarBoss _moskar)
	{
		SecondsDelayWait shootWait = new SecondsDelayWait(0.0f);
		int bursts = 0;
		int i = 0;

		while(true)
		{
			bursts = fireBursts.Random();
			shootWait.ChangeDurationAndReset(shootInterval.Random());
			i = 0;

			while(shootWait.MoveNext()) yield return null;

			while(i < bursts)
			{
				Projectile crap = _moskar.ShootPoop(Vector3.down);
				shootWait.ChangeDurationAndReset(crap.cooldownDuration);

				while(shootWait.MoveNext()) yield return null;

				i++;
				yield return null;
			}
		}
	}
#endregion

}
}