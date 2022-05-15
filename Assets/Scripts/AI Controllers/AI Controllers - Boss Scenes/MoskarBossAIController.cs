using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Voidless;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

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
	public event OnCharacterDeactivated onMoskarDeactivated; 									/// <summary>OnMoskarDeactivated's event. Triggered when any moskar invokes the event of the same type.</summary>

	[Space(5f)]
	[SerializeField] private VAssetReference _moskarReference; 									/// <summary>Moskar's Reference.</summary>
	[Header("Introduction's Attributes:")]
	[SerializeField] private Vector3 _initialPosition; 											/// <summary>Moskar's Initial Position [after the introduction sequence].</summary>
	[SerializeField] private string _introSequenceKey; 											/// <summary>Intro's Sequence Key.</summary>
	[SerializeField] private float _waitBeforeTaunt; 											/// <summary>Seconds to wait before Taunting.</summary>
	[SerializeField] private float _waitBeforeEndingTaunt; 										/// <summary>Seconds to wait before ceasing taunt.</summary>
	[SerializeField] private float _waitBeforeIntro; 											/// <summary>Seconds to wait before beginning the Introduction.</summary>
	[SerializeField] private float _positioningDuration; 										/// <summary>Positioning's Duration [after the intro].</summary>
	[SerializeField] private IntRange _waypointsGeneration; 									/// <summary>Waypoints Generation's Range.</summary>
	[Space(5f)]
	[Header("Reproductions' Attributes:")]
	[SerializeField] private FloatRange _scaleRange; 											/// <summary>Scale's Range.</summary>
	[SerializeField] private float _reproductionDuration; 										/// <summary>Reproduction Duration. Determines how long it lasts the reproduction's displacement and scaling.</summary>
	[SerializeField] private float _reproductionPushForce; 										/// <summary>Reproduction's Push Force.</summary>
	[Space(5f)]
	[Header("Shooting's Attributes:")]
	[TabGroup("Attacking", "Shooting")][SerializeField] private FloatRange _shootInterval; 		/// <summary>Shooting's Interval.</summary>
	[TabGroup("Attacking", "Shooting")][SerializeField] private IntRange _fireBursts; 			/// <summary>Fire Burst per Shooting.</summary>
	[Space(5f)]
	[TabGroup("Attacking", "Tackling")][SerializeField] private FloatRange _tacklingSpeed; 		/// <summary>Tackling's Speed's Range.</summary>
	[TabGroup("Attacking", "Tackling")][SerializeField] private FloatRange _tacklingForce; 		/// <summary>Tackling's Force's Range.</summary>
	[TabGroup("Attacking", "Tackling")][SerializeField] private int _minimumAmountForTackle; 	/// <summary>Minimum amount of Moskars required to consider a tackle.</summary>
	[TabGroup("Attacking", "Tackling")][SerializeField] private float _tacklingDistance; 		/// <summary>Tackling's Required Distance.</summary>
	[TabGroup("Attacking", "Tackling")][SerializeField] private float _tacklingProjectionTime; 	/// <summary>Tackling's Projection Time.</summary>
	[TabGroup("Attacking", "Tackling")][SerializeField] private FloatRange _tacklingInterval; 	/// <summary>Tackling's Interval.</summary>
	[Space(5f)]
	[Header("Warning's Attributes:")]
	[TabGroup("Steering", "Warning")][SerializeField] private FloatRange _warningSpeed; 		/// <summary>Warning's Steering Speed's Range.</summary>
	[TabGroup("Steering", "Warning")][SerializeField] private FloatRange _warningForce; 		/// <summary>Warning's Steering Force's Range.</summary>
	[TabGroup("Steering", "Warning")][SerializeField] private float _dangerRadius; 				/// <summary>Danger's Radius.</summary>
	[TabGroup("Steering", "Warning")][SerializeField] private float _fleeDistance; 				/// <summary>Flee distance between Moskar and Mateo.</summary>
	[Space(5f)]
	[Header("Evasion Attributes: ")]
	[TabGroup("Steering", "Evasion")][SerializeField] private FloatRange _evasionSpeed; 		/// <summary>Evasion's Speed's Range.</summary>
	[TabGroup("Steering", "Evasion")][SerializeField] private FloatRange _evasionForce; 		/// <summary>Evasion's Force's Range.</summary>
	[Space(5f)]
	[Header("Wander Attributes: ")]
	[TabGroup("Steering", "Wander")][SerializeField] private FloatRange _wanderSpeed; 			/// <summary>Wander's Max Speed's Range.</summary>
	[TabGroup("Steering", "Wander")][SerializeField] private FloatRange _wanderForce; 			/// <summary>Wander's Max Force's Range.</summary>
	[TabGroup("Steering", "Wander")][SerializeField] private FloatRange _wanderInterval; 		/// <summary>Wander interval between each angle change [as a range].</summary>
	[Space(5f)]
	[Header("Flocking Weights:")]
	[TabGroup("Steering", "Flocking")][Range(0.0f, 10.0f)]
	[SerializeField] private float _waypointSeekWeight; 										/// <summary>Waypoint-Seek's Weight.</summary>
	[TabGroup("Steering", "Flocking")][Range(0.0f, 10.0f)]
	[SerializeField] private float _leaderSeekWeight; 											/// <summary>Leader-Seek's Weight.</summary>
	[TabGroup("Steering", "Flocking")][Range(0.0f, 10.0f)]
	[SerializeField] private float _separationWeight; 											/// <summary>Separation's Weight.</summary>
	[TabGroup("Steering", "Flocking")][Range(0.0f, 10.0f)]
	[SerializeField] private float _cohesionWeight; 											/// <summary>Cohesion's Weight.</summary>
	[TabGroup("Steering", "Flocking")][Range(0.0f, 10.0f)]
	[SerializeField] private float _allignmentWeight; 											/// <summary>Allingment's Weight.</summary>
	[TabGroup("Steering", "Flocking")][Range(0.0f, 10.0f)]
	[SerializeField] private float _containmentWeight; 											/// <summary>Containment's Weight.</summary>
	[Space(5f)]
	[Header("Rubberbanding's Attributes:")]
	[TabGroup("Steering", "Rubberbanding")][SerializeField] private bool _applyRubberbanding; 				/// <summary>ApplyRubberbanding?.</summary>
	[TabGroup("Steering", "Rubberbanding")][SerializeField] private FloatRange _rubberbandingSpeedScale; 	/// <summary>Rubberbanding's Speed Scale.</summary>
	[TabGroup("Steering", "Rubberbanding")][SerializeField] private FloatRange _rubberbandingForceScale; 	/// <summary>Rubberbanding's Force Scale.</summary>
	[TabGroup("Steering", "Rubberbanding")][SerializeField] private FloatRange _rubberbandingRadius; 		/// <summary>Rubberbanding's Radius.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Moskar Gizmos' Attributes:")]
	[TabGroup("Gizmos", "Gizmos")][SerializeField] private Color leaderColor; 					/// <summary>Leader's Color.</summary>
	[TabGroup("Gizmos", "Gizmos")][SerializeField] private Color leaderSeekForceColor; 			/// <summary>Leader-Seek Force's Color.</summary>
	[TabGroup("Gizmos", "Gizmos")][SerializeField] private Color separationForceColor; 			/// <summary>Separarion Force's Color.</summary>
	[TabGroup("Gizmos", "Gizmos")][SerializeField] private Color cohesionForceColor; 			/// <summary>Cohesion Force's Color.</summary>
	[TabGroup("Gizmos", "Gizmos")][SerializeField] private float leaderSphereRadius; 			/// <summary>Leader Sphere's Radius.</summary>
#endif
	private Dictionary<int, MoskarBoss> _reproductions; 										/// <summary>Moskar's Reproductions.</summary>
	private Dictionary<int, SteeringVehicle2D> _neighborhood; 									/// <summary>Moskars' group with the exception of the leader.</summary>
	private MoskarBoss _leader; 																/// <summary>Leader's Reference.</summary>
	private float _phaseProgress; 																/// <summary>Phase's Normalized Progress.</summary>
	private float _totalMoskars; 																/// <summary>Total Moskars that will be reproduced.</summary>
	private float _moskarsDestroyed; 															/// <summary>Moskar Reproductions destroyed.</summary>
	private Boundaries2DContainer _boundaries; 													/// <summary>Boundaries' Container.</summary>
	private Vector3[] _waypoints; 																/// <summary>Waypoints.</summary>
	private Quaternion _localAnimatorRotation; 													/// <summary>Local Animator's Rotation.</summary>
	private Coroutine flockingCoroutine; 														/// <summary>Flocking Coroutine's reference.</summary>
	private Coroutine attackCoroutine; 															/// <summary>AttackBehavior's Coroutine reference.</summary>
	private Coroutine rotationCoroutine; 														/// <summary>Rotation Coroutine's Reference.</summary>
	private Coroutine tacklingCoroutine; 														/// <summary>Tackling's Coroutine Reference.</summary>
	private Dictionary<int, Coroutine> attackCoroutines; 										/// <summary>Attack Coroutines for each Moskar reproduction.</summary>
	private Dictionary<int, Coroutine> rotationCoroutines; 										/// <summary>Rotation Coroutines for each Moskar reproduction.</summary>
	private IEnumerator<Vector3> waypointsIterator; 											/// <summary>Waypoints' Iterator.</summary>

#region Getters/Setters:
	/// <summary>Gets moskarReference property.</summary>
	public VAssetReference moskarReference { get { return _moskarReference; } }

	/// <summary>Gets waypointsGeneration property.</summary>
	public IntRange waypointsGeneration { get { return _waypointsGeneration; } }

	/// <summary>Gets fireBursts property.</summary>
	public IntRange fireBursts { get { return _fireBursts; } }

	/// <summary>Gets shootInterval property.</summary>
	public FloatRange shootInterval { get { return _shootInterval; } }

	/// <summary>Gets tacklingInterval property.</summary>
	public FloatRange tacklingInterval { get { return _tacklingInterval; } }

	/// <summary>Gets scaleRange property.</summary>
	public FloatRange scaleRange { get { return _scaleRange; } }

	/// <summary>Gets and Sets wanderSpeed property.</summary>
	public FloatRange wanderSpeed { get { return _wanderSpeed; } }

	/// <summary>Gets wanderForce property.</summary>
	public FloatRange wanderForce { get { return _wanderForce; } }

	/// <summary>Gets and Sets evasionSpeed property.</summary>
	public FloatRange evasionSpeed { get { return _evasionSpeed; } }

	/// <summary>Gets evasionForce property.</summary>
	public FloatRange evasionForce { get { return _evasionForce; } }

	/// <summary>Gets and Sets wanderInterval property.</summary>
	public FloatRange wanderInterval { get { return _wanderInterval; } }

	/// <summary>Gets rubberbandingSpeedScale property.</summary>
	public FloatRange rubberbandingSpeedScale { get { return _rubberbandingSpeedScale; } }

	/// <summary>Gets rubberbandingForceScale property.</summary>
	public FloatRange rubberbandingForceScale { get { return _rubberbandingForceScale; } }

	/// <summary>Gets rubberbandingRadius property.</summary>
	public FloatRange rubberbandingRadius { get { return _rubberbandingRadius; } }

	/// <summary>Gets warningSpeed property.</summary>
	public FloatRange warningSpeed { get { return _warningSpeed; } }

	/// <summary>Gets warningForce property.</summary>
	public FloatRange warningForce { get { return _warningForce; } }

	/// <summary>Gets tacklingSpeed property.</summary>
	public FloatRange tacklingSpeed { get { return _tacklingSpeed; } }

	/// <summary>Gets tacklingForce property.</summary>
	public FloatRange tacklingForce { get { return _tacklingForce; } }

	/// <summary>Gets applyRubberbanding property.</summary>
	public bool applyRubberbanding { get { return _applyRubberbanding; } }

	/// <summary>Gets waitBeforeTaunt property.</summary>
	public float waitBeforeTaunt { get { return _waitBeforeTaunt; } }

	/// <summary>Gets waitBeforeEndingTaunt property.</summary>
	public float waitBeforeEndingTaunt { get { return _waitBeforeEndingTaunt; } }

	/// <summary>Gets waitBeforeIntro property.</summary>
	public float waitBeforeIntro { get { return _waitBeforeIntro; } }

	/// <summary>Gets positioningDuration property.</summary>
	public float positioningDuration { get { return _positioningDuration; } }

	/// <summary>Gets tacklingDistance property.</summary>
	public float tacklingDistance { get { return _tacklingDistance; } }

	/// <summary>Gets tacklingProjectionTime property.</summary>
	public float tacklingProjectionTime { get { return _tacklingProjectionTime; } }

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

	/// <summary>Gets reproductionDuration property.</summary>
	public float reproductionDuration { get { return _reproductionDuration; } }

	/// <summary>Gets reproductionPushForce property.</summary>
	public float reproductionPushForce { get { return _reproductionPushForce; } }

	/// <summary>Gets waypointSeekWeight property.</summary>
	public float waypointSeekWeight { get { return _waypointSeekWeight; } }

	/// <summary>Gets leaderSeekWeight property.</summary>
	public float leaderSeekWeight { get { return _leaderSeekWeight; } }

	/// <summary>Gets separationWeight property.</summary>
	public float separationWeight { get { return _separationWeight; } }

	/// <summary>Gets cohesionWeight property.</summary>
	public float cohesionWeight { get { return _cohesionWeight; } }

	/// <summary>Gets allignmentWeight property.</summary>
	public float allignmentWeight { get { return _allignmentWeight; } }

	/// <summary>Gets containmentWeight property.</summary>
	public float containmentWeight { get { return _containmentWeight; } }

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

	/// <summary>Gets minimumAmountForTackle property.</summary>
	public int minimumAmountForTackle { get { return _minimumAmountForTackle; } }

	/// <summary>Gets introSequenceKey property.</summary>
	public string introSequenceKey { get { return _introSequenceKey; } }

	/// <summary>Gets and Sets reproductions property.</summary>
	public Dictionary<int, MoskarBoss> reproductions
	{
		get { return _reproductions; }
		private set { _reproductions = value; }
	}

	/// <summary>Gets and Sets neighborhood property.</summary>
	public Dictionary<int, SteeringVehicle2D> neighborhood
	{
		get { return _neighborhood; }
		private set { _neighborhood = value; }
	}

	/// <summary>Gets and Sets leader property.</summary>
	public MoskarBoss leader
	{
		get { return _leader; }
		private set { _leader = value; }
	}

	/// <summary>Gets and Sets boundaries property.</summary>
	public Boundaries2DContainer boundaries
	{
		get { return _boundaries; }
		set { _boundaries = value; }
	}

	/// <summary>Gets initialPosition property.</summary>
	public Vector3 initialPosition { get { return _initialPosition; } }

	/// <summary>Gets and Sets waypoints property.</summary>
	public Vector3[] waypoints
	{
		get { return _waypoints; }
		private set { _waypoints = value; }
	}

	/// <summary>Gets and Sets localAnimatorRotation property.</summary>
	public Quaternion localAnimatorRotation
	{
		get { return _localAnimatorRotation; }
		private set { _localAnimatorRotation = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>Draws Gizmos on Editor mode when MoskarBossAIController's instance is selected.</summary>
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();

		Gizmos.DrawWireSphere(initialPosition, 0.5f);

		if(waypoints == null) return;

		foreach(Vector3 waypoint in waypoints)
		{
			Gizmos.DrawWireSphere(waypoint, 0.2f);
		}

		if(leader == null) return;

		Gizmos.color = leaderColor;
		Gizmos.DrawWireSphere(leader.transform.position, rubberbandingRadius.Min());
		Gizmos.DrawWireSphere(leader.transform.position, rubberbandingRadius.Max());

		Gizmos.color = leaderColor.WithAlpha(0.5f);
		Gizmos.DrawSphere(leader.transform.position, leaderSphereRadius);
	}

	/// <summary>MoskarBossAIController's instance initialization.</summary>
	protected override void Awake()
	{
		if(character == null) return;

		reproductions = new Dictionary<int, MoskarBoss>();
		neighborhood = new Dictionary<int, SteeringVehicle2D>();
		leader = character;

		totalMoskars = 0.0f;

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
		//this.StartCoroutine(IntroductionRoutine(character), ref behaviorCoroutine);
		Game.mateo.eventsHandler.onIDEvent += OnMateoIDEvent;
		waypointsIterator = WaypointsIterator();

		BeginIntroductonSequence();
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
		//FlockBehavior();
		ContainReproductionsOnScenario();
	}
#endregion

#region Methods:
	/// <summary>Begins Introduction's Sequence.</summary>
	private void BeginIntroductonSequence()
	{
/* DEACTIVATED TIMELINE-SEQUENCE:
		Vector3 scale = Vector3.one * scaleRange.Max();

		character.sightSensor.enabled = true;
		character.meshParent.transform.localScale = scale;
		character.sightSensor.gameObject.SetActive(false);
		character.EnablePhysicalColliders(false);
		character.EnableTriggerColliders(false);
		character.EnableHurtBoxes(false);

		localAnimatorRotation = character.animator.transform.localRotation;

		//character.transform.position = Vector3.zero;
		//character.animator.enabled = false;
		//character.animator.transform.parent = null;
		//character.animator.transform.position = initialPosition;
		//character.gameObject.SetActive(false);

		//TimelineSequenceController.PlaySequence(introSequenceKey, OnTimelineSequenceFinished);
*/
		/// Temporarily go directly to wander:
		this.ChangeState(IDs.STATE_IDLE);
		Game.AddTargetToCamera(character.cameraTarget);
	}

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

		foreach(MoskarBoss moskar in reproductions.Values)
		{
			boundaries.ContainInsideBoundaries(moskar.rigidbody);
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
			reproduction.AddStates(IDs.STATE_ATTACKING_0);
			reproduction.AddStates(IDs.STATE_ALIVE);
			EnterAttackState(reproduction);
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

		UpdateLeader();
	}

	/// <summary>Updates who ought to be the leader and who are its subordinates [neighborhood].</summary>
	private void UpdateLeader()
	{
		/// Choose a Moskar leader...
		int minPhase = int.MaxValue;

		foreach(MoskarBoss moskar in reproductions.Values)
		{
			if(moskar.currentPhase < minPhase)
			{
				leader = moskar;
				minPhase = moskar.currentPhase;
			}
		}

		/// Add all Moskars except the boss as neighbors...
		neighborhood.Clear();

		foreach(MoskarBoss moskar in reproductions.Values)
		{
			if(leader != moskar) neighborhood.Add(moskar.GetInstanceID(), moskar.vehicle);
		}
	}

	/// <summary>Makes all Moskar reproductions emulate a flocking steering behavior.</summary>
	private void FlockBehavior()
	{
		waypointsIterator.MoveNext();

		foreach(MoskarBoss moskar in reproductions.Values)
		{
			if(moskar.HasStates(IDs.STATE_ATTACKING_1)) continue;

			SteeringVehicle2D vehicle = moskar.vehicle;
			bool isLeader = moskar == leader;
			float weight = isLeader ? waypointSeekWeight : leaderSeekWeight;
			Vector3 target = isLeader ? waypointsIterator.Current : leader.transform.position;
			Vector2 mainSeekForce = vehicle.GetSeekForce(target) * weight;
			Vector2 separationForce = Vector2.zero;
			Vector2 cohesionForce = Vector2.zero;
			Vector2 allignment = Vector2.zero;
			Vector2 sum = mainSeekForce;

			if(!isLeader)
			{
				separationForce = vehicle.GetSeparationForce(neighborhood.Values) * separationWeight;
				cohesionForce = vehicle.GetCohesionForce(neighborhood.Values) * cohesionWeight;
				allignment = vehicle.GetAllignment(neighborhood.Values) * allignmentWeight;

				sum += (separationForce + cohesionForce + allignment);
			}

			vehicle.ApplyForce(sum);
			vehicle.Displace(Time.fixedDeltaTime);
			vehicle.Rotate(Time.fixedDeltaTime, true);

#if UNITY_EDITOR
			Debug.DrawRay(moskar.transform.position, separationForce, separationForceColor);
			Debug.DrawRay(moskar.transform.position, cohesionForce, cohesionForceColor);
			Debug.DrawRay(moskar.transform.position, mainSeekForce, leaderSeekForceColor);
			Debug.DrawRay(moskar.transform.position, sum, Color.white);
#endif
		}
	}

	/// <summary>Rubberbanding's Adjustment.</summary>
	private void RubberbandingAdjustment()
	{
		if(leader == null || state == IDs.STATE_DEAD || state == IDs.STATE_IDLE) return;

		Debug.Log("[MoskarBossAIController] RubberbandingAdjustment...");

		Vector3 target = leader.transform.position;
		float t = 0.0f;
		FloatRange speedRange = default(FloatRange);
		FloatRange forceRange = default(FloatRange);

		switch(state)
		{
			case IDs.STATE_ATTACKING_0:
				speedRange = evasionSpeed;
				forceRange = evasionForce;
			break;

			case IDs.STATE_EVADE:
				speedRange = evasionSpeed;
				forceRange = evasionForce;
			break;
		}

#if UNITY_EDITOR
		int i = 0;

		debugBuilder.Clear();
		debugBuilder.AppendLine("Rubberbanding's Data: ");
		debugBuilder.AppendLine();
#endif

		foreach(SteeringVehicle2D vehicle in neighborhood.Values)
		{
			t = vehicle.GetArrivalWeight(target, rubberbandingRadius.Min(), rubberbandingRadius.Max());
			t = VMath.Sigmoid(t);

			vehicle.maxSpeed = speedRange.Lerp(t);
			vehicle.maxForce = forceRange.Lerp(t);

#if UNITY_EDITOR
			debugBuilder.Append("Vehicle #");
			debugBuilder.Append(i.ToString());
			debugBuilder.Append(": { Distance from Leader = ");
			debugBuilder.Append((leader.transform.position - vehicle.transform.position).magnitude.ToString());
			debugBuilder.Append(", Arrival Weight [f(t)] = ");
			debugBuilder.Append(t.ToString());
			debugBuilder.Append(", Adjusted Speed = ");
			debugBuilder.Append(speedRange.Lerp(t));
			debugBuilder.Append(", Adjusted Force = ");
			debugBuilder.Append(forceRange.Lerp(t));
			debugBuilder.AppendLine(" }");
			debugBuilder.AppendLine();

			i++;
#endif
		}
	}

	/// <summary>Enters Wander State.</summary>
	/// <param name="_moskar">Moskar Reproduction that will enter that state.</param>
	private void EnterWanderState(MoskarBoss _moskar)
	{
		int instanceID = _moskar.GetInstanceID();

		_moskar.RemoveStates(IDs.STATE_ATTACKING_0);
		_moskar.AddStates(IDs.STATE_IDLE);

		_moskar.DispatchCoroutine(ref _moskar.attackCoroutine);

		_moskar.animatorController.CrossFade(_moskar.walkCredential, _moskar.clipFadeDuration);
		_moskar.vehicle.maxSpeed = wanderSpeed.Lerp(phaseProgress);
		_moskar.vehicle.maxForce = wanderForce.Lerp(phaseProgress);
		_moskar.sightSensor.gameObject.SetActive(true);

		//_moskar.StartCoroutine(_moskar.meshParent.PivotToRotation(_moskar.walkingRotation, _moskar.rotationDuration, TransformRelativeness.Local), ref _moskar.rotationCoroutine);
		_moskar.StartCoroutine(WanderBehaviour(_moskar), ref _moskar.behaviorCoroutine);
	}

	/// <summary>Enters Warning State.</summary>
	/// <param name="_moskar">Moskar Reproduction that will enter that state.</param>
	private void EnterWarningState(MoskarBoss _moskar)
	{
		_moskar.vehicle.maxSpeed = warningSpeed.Lerp(phaseProgress);
		_moskar.vehicle.maxForce = warningForce.Lerp(phaseProgress);

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
		_moskar.vehicle.maxForce = evasionForce.Lerp(phaseProgress);
		_moskar.sightSensor.gameObject.SetActive(false);
		_moskar.animatorController.CrossFade(_moskar.flyCredential, _moskar.clipFadeDuration);

		_moskar.DispatchCoroutine(ref _moskar.behaviorCoroutine);

		_moskar.StartCoroutine(_moskar.meshParent.PivotToRotation(_moskar.flyingRotation, _moskar.rotationDuration, TransformRelativeness.Local), ref _moskar.rotationCoroutine);
		_moskar.StartCoroutine(AttackBehavior(_moskar), ref _moskar.attackCoroutine);
	}
#endregion

#region Callbacks:
//-------------------------------------------------------
//	 		IFiniteStateMachine Callbacks: 				|
//-------------------------------------------------------
	/// <summary>Enters int State.</summary>
	/// <param name="_state">int State that will be entered.</param>
	public override void OnEnterState(int _state)
	{
		/* States & Actions:
			- IDLE: Enter Wander State
			- EVADE: Enter Warning State
			- ATTACKING_0: Enter Attacking State
			- DEAD: Perform Post-Dead routine.
		*/

		switch(_state)
		{
			case IDs.STATE_IDLE:
				this.DispatchCoroutine(ref flockingCoroutine);
				this.DispatchCoroutine(ref tacklingCoroutine);

				foreach(MoskarBoss moskar in reproductions.Values)
				{
					EnterWanderState(moskar);
				}
			break;

			case IDs.STATE_ATTACKING_0:
				foreach(MoskarBoss moskar in reproductions.Values)
				{
					EnterAttackState(moskar);
				}

				this.StartCoroutine(FlockBehaviorRoutine(), ref flockingCoroutine);
			break;

			case IDs.STATE_EVADE:
			foreach(MoskarBoss moskar in reproductions.Values)
				{
					EnterAttackState(moskar);
				}

				this.StartCoroutine(FlockBehaviorRoutine(), ref flockingCoroutine);
			break;

			case IDs.STATE_DEAD:
				this.DispatchCoroutine(ref flockingCoroutine);
				this.DispatchCoroutine(ref tacklingCoroutine);
				this.DispatchCoroutine(ref rotationCoroutine);
				this.DispatchCoroutine(ref attackCoroutine);
			break;
		}
	}

	/// <summary>Exits int State.</summary>
	/// <param name="_state">int State that will be left.</param>
	public override void OnExitState(int _state)
	{
		switch(_state)
		{
			case IDs.STATE_IDLE:
			break;

			case IDs.STATE_ATTACKING_0:
			break;

			case IDs.STATE_EVADE:
			break;

			case IDs.STATE_DEAD:
			break;
		}
	}

	/// <summary>Callback invoked when a Timeline-Sequence is over.</summary>
	/// <param name="_name">Name of the sequence.</param>
	/// <param name="_timelineAsset">TimelineAsset associated with the Timeline-Sequence.</param>
	private void OnTimelineSequenceFinished(string _name, TimelineAsset _timelineAsset)
	{
		Debug.Log("[MoskarBossAIController] Timeline-Sequence " + _name + " finished.");

		if(_name == introSequenceKey)
		{
			this.StartCoroutine(IntroductionEndRoutine());
		}
	}

	/// <summary>Callback invoked when a Character's state is changed.</summary>
	/// <param name="_character">Character that invokes the event.</param>
	/// <param name="_flags">State Flags.</param>
	/// <param name="_stateChange">Type of State Change.</param>
	protected override void OnCharacterStateChanged(Character _character, int _state, StateChange _stateChange)
	{
		base.OnCharacterStateChanged(_character, _state, _stateChange);

		MoskarBoss moskar = _character as MoskarBoss;

		if(moskar == null) return;

		int state = moskar.state;

		UpdateLeader();

		switch(_stateChange)
		{
			case StateChange.Added:
				if((_state | IDs.STATE_IDLE) == _state && state != IDs.STATE_IDLE)
				{ /// Wander Coroutine:
					//EnterWanderState(moskar);
				
				} else if((_state | IDs.STATE_TARGETONSIGHT) == _state && state != IDs.STATE_ATTACKING_0)
				{ /// Warning Coroutine:
					this.ChangeState(IDs.STATE_ATTACKING_0);

				} else if((_state | IDs.STATE_ATTACKING_0) == _state && state != IDs.STATE_ATTACKING_0)
				{ /// Attack Coroutine:
					this.ChangeState(IDs.STATE_ATTACKING_0);
				} 
			break;

			case StateChange.Removed:
				if((_state | IDs.STATE_TARGETONSIGHT) == _state
				&& (state | IDs.STATE_ATTACKING_0) != state
				&& (state | IDs.STATE_IDLE) != state
				&& moskar.sightSensor.enabled)
				{ /// If the Player got out of sight, but Moskar is not Attacking and not on Wander:
					//EnterWanderState(moskar);
				}

				if((_state | IDs.STATE_ALIVE) == _state)
				{
					moskar.DispatchCoroutine(ref moskar.behaviorCoroutine);
					moskar.DispatchCoroutine(ref moskar.attackCoroutine);
				}

				if((_state | IDs.STATE_ATTACKING_0) == _state)
				{
					moskar.DispatchCoroutine(ref moskar.attackCoroutine);
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

		if(state != IDs.STATE_ATTACKING_0 || moskarsDestroyed == 0) this.ChangeState(IDs.STATE_ATTACKING_0);

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
				Debug.Log("[MoskarBossAIController] Astro Boy is meditating...");
				this.ChangeState(IDs.STATE_IDLE);
			break;
		}
	}
#endregion

#region Coroutines:
	/// <summary>Introduction ending's routine, played after te Timeline-Sequence of the introduction is over.</summary>
	private IEnumerator IntroductionEndRoutine()
	{
		Animator animator = character.animator;
		float inverseDuration = 1.0f / positioningDuration;
		float t = 0.0f;
		Vector3 pA = character.transform.position;
		Vector3 pB = animator.transform.position;
		Quaternion rA = character.transform.rotation;
		Quaternion rB = rA * Quaternion.Inverse(localAnimatorRotation);

		animator.transform.SetParent(null);

		while(t < 1.0f)
		{
			character.transform.position = Vector3.Lerp(pA, pB, t);
			character.transform.rotation = Quaternion.Lerp(rA, rB, t);

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		animator.enabled = false;
		character.transform.position = pB;
		character.transform.rotation = rB;
		animator.transform.SetParent(character.transform);
		animator.transform.localPosition = Vector3.zero;
		animator.transform.localRotation = localAnimatorRotation;
		animator.enabled = true;
		character.onAnimatorMoveOverrider.overrideActions |= Override.DontOffsetFromParent;
		character.sightSensor.gameObject.SetActive(true);
		character.EnablePhysicalColliders(true);
		character.EnableTriggerColliders(true);
		character.EnableHurtBoxes(true);
		Game.AddTargetToCamera(character.cameraTarget);

		//EnterWanderState(character);
	}

	/// \TODO DEPRECATE
	/// <summary>Introduction's Routine.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Introduction Behavior.</param>
	private IEnumerator IntroductionRoutine(MoskarBoss _moskar)
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		Vector3 initialPosition = _moskar.transform.position;
		Vector3 animatorPosition = Vector3.zero;
		Quaternion initialRotation = _moskar.transform.rotation;
		Quaternion animatorRotation = Quaternion.identity;
		float t = 0.0f;
		float inverseDuration = 1.0f / positioningDuration;

		_moskar.transform.position = Vector3.zero;
		_moskar.animator.transform.parent = null;
		_moskar.animator.transform.position = initialPosition;

		_moskar.gameObject.SetActive(false);
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

		/// Set Moskar's position to Animator's position:
		animatorPosition = _moskar.animator.transform.position;
		animatorRotation = _moskar.animator.transform.rotation;
		_moskar.animator.transform.parent = null;

		while(t < 1.0f)
		{
			_moskar.transform.position = Vector3.Lerp(initialPosition, animatorPosition, t);
			//_moskar.transform.rotation = Quaternion.Lerp(initialRotation, animatorRotation, t);
			t += (Time.deltaTime * inverseDuration);

			yield return null;
		}

		_moskar.gameObject.SetActive(true);
		_moskar.transform.position = animatorPosition;
		_moskar.animator.transform.parent = _moskar.transform;
		_moskar.animator.transform.localPosition = Vector3.zero;
		character.sightSensor.gameObject.SetActive(true);
		character.EnablePhysicalColliders(true);
		character.EnableTriggerColliders(true);
		character.EnableHurtBoxes(true);
		Game.AddTargetToCamera(_moskar.cameraTarget);

		EnterWanderState(_moskar);
	}

	/// <summary>Flock Behavior's Routine.</summary>
	private IEnumerator FlockBehaviorRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(tacklingInterval.Random());

		while(true)
		{
			FlockBehavior();
			RubberbandingAdjustment();
			//ContainReproductionsOnScenario();

			if(reproductions.Count >= minimumAmountForTackle && tacklingCoroutine == null && !wait.MoveNext())
			{
				float distance = tacklingDistance * tacklingDistance;
				Vector3 mateoPosition = Game.mateo.transform.position;
				Vector3 direction = Vector3.zero;

				wait.ChangeDurationAndReset(tacklingInterval.Random());

				foreach(MoskarBoss moskar in reproductions.Values)
				{
					if(moskar == leader) continue;

					direction = mateoPosition - moskar.transform.position;

					if(direction.sqrMagnitude >= distance)
					{
						this.StartCoroutine(TackleBehavior(moskar), ref tacklingCoroutine);
						break;
					}
				}
			}

			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}
	}

	/// <summary>Wander's Steering Beahviour Coroutine.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Wander Behavior.</param>
	private IEnumerator WanderBehaviour(MoskarBoss _moskar)
	{
		SteeringVehicle2D vehicle = _moskar.vehicle;
		SecondsDelayWait wait = new SecondsDelayWait(wanderInterval.Random());
		Vector3 target = Vector3.zero;
		Vector3 wanderForce = Vector3.zero;
		Vector3 direction = Vector3.zero;
		float minDistance = 0.5f * 0.5f;

		while(true)
		{
			target = vehicle.GetWanderPoint();
			direction = target - _moskar.transform.position;

			while(direction.sqrMagnitude > minDistance && wait.MoveNext())
			{
				wanderForce = boundaries.GetContainmentForce(vehicle, 1.0f) * containmentWeight;
				if(wanderForce.sqrMagnitude == 0.0f) wanderForce += (Vector3)vehicle.GetSeekForce(target);
				vehicle.ApplyForce(wanderForce);
				vehicle.Displace(Time.fixedDeltaTime);
				vehicle.Rotate(Time.fixedDeltaTime, true);
				direction = target - _moskar.transform.position;

				yield return VCoroutines.WAIT_PHYSICS_THREAD;
			}

			wait.ChangeDurationAndReset(wanderInterval.Random());
		}
	}

	/// <summary>Warning's Behavior.</summary>
	/// <param name="_moskar">Moskar reproduction that will perform the Warning Behavior.</param>
	private IEnumerator WarningBehavior(MoskarBoss _moskar)
	{
		SteeringVehicle2D vehicle = _moskar.vehicle;
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
				fleeForce = boundaries.GetContainmentForce(vehicle, 1.0f) * containmentWeight;
				if(fleeForce.sqrMagnitude == 0.0f) fleeForce += (Vector3)vehicle.GetFleeForce(projectedMateoPosition);

				vehicle.ApplyForce(fleeForce);
				vehicle.Displace(Time.fixedDeltaTime);
				vehicle.Rotate(Time.fixedDeltaTime, true);
			}

			if(magnitude <= radius) _moskar.AddStates(IDs.STATE_ATTACKING_0);

			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}
	}

	/// <summary>Waypoints' Iterator.</summary>
	private IEnumerator<Vector3> WaypointsIterator()
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
				Vector3 direction = waypoint - leader.transform.position;
				
				while(direction.sqrMagnitude > distance)
				{
					direction = waypoint - leader.transform.position;
					yield return waypoint;
				}
			}
		}
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

	/// <summary>Tackle Behavior.</summary>
	/// <param name="_moskar">Moskar reproduction that will make the tackle.</param>
	private IEnumerator TackleBehavior(MoskarBoss _moskar)
	{
		SteeringVehicle2D vehicle = _moskar.vehicle;
		int instanceID = _moskar.GetInstanceID();
		float distance = (minDistanceToReachTarget * minDistanceToReachTarget);
		Vector3 mateoPosition = boundaries.Clamp(Game.ProjectMateoPosition(tacklingProjectionTime));
		Vector3 direction = mateoPosition - _moskar.transform.position;
		Vector3 seekForce = Vector3.zero;

		/// While it tackles, it leaves the neighborhood.
		neighborhood.Remove(instanceID);
		_moskar.AddStates(IDs.STATE_ATTACKING_1);
		vehicle.maxSpeed = tacklingSpeed.Lerp(phaseProgress);
		vehicle.maxForce = tacklingForce.Lerp(phaseProgress);
		_moskar.tackleTrailRenderer.enabled = true;
		_moskar.tackleTrailRenderer.Clear();

		while(_moskar.HasStates(IDs.STATE_ALIVE) && direction.sqrMagnitude > distance)
		{
			seekForce = vehicle.GetSeekForce(mateoPosition);
			vehicle.ApplyForce(seekForce);
			vehicle.Displace(Time.fixedDeltaTime);
			vehicle.Rotate(Time.fixedDeltaTime, true);

			direction = mateoPosition - _moskar.transform.position;

			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}

		/// Come back to the neighborhood after the tackle.
		if(_moskar.HasStates(IDs.STATE_ALIVE) && !neighborhood.ContainsKey(instanceID)) neighborhood.Add(instanceID, vehicle);

		_moskar.tackleTrailRenderer.enabled = false;
		_moskar.RemoveStates(IDs.STATE_ATTACKING_1);
		vehicle.maxSpeed = evasionSpeed.Lerp(phaseProgress);
		vehicle.maxForce = evasionForce.Lerp(phaseProgress);

		this.DispatchCoroutine(ref tacklingCoroutine);
	}
#endregion

	/// <returns>String representing Moskar's AI Controller.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine(base.ToString());
		builder.AppendLine();
		builder.Append("Total Moskars: ");
		builder.AppendLine(totalMoskars.ToString());
		builder.Append("Moskars Destroyed: ");
		builder.AppendLine(moskarsDestroyed.ToString());
		builder.Append("Phase Progress: ");
		builder.AppendLine(phaseProgress.ToString());

		return builder.ToString();
	}

}
}