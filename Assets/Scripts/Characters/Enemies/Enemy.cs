using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyEventsHandler))]
[RequireComponent(typeof(VCameraTarget))]
public class Enemy : PoolGameObject, IStateMachine
{
	public const int ID_STATE_DEAD = 0; 						/// <summary>Dead's State Flag.</summary>
	public const int ID_STATE_ALIVE = 1 << 0; 					/// <summary>Alive's State Flag.</summary>
	public const int ID_STATE_IDLE = 1 << 1; 					/// <summary>Idle's State Flag.</summary>
	public const int ID_STATE_PLAYERONSIGHT = 1 << 2; 			/// <summary>Player On Sight's State Flag.</summary>
	public const int ID_STATE_FOLLOWPLAYER = 1 << 3; 			/// <summary>Follow Player's State Flag.</summary>
	public const int ID_STATE_ATTACK = 1 << 4; 					/// <summary>Attack's State Flag.</summary>
	public const int ID_STATE_VULNERABLE = 1 << 5; 				/// <summary>Vulnerable's State Flag (it means the enemy is available to be attacked).</summary>
	public const int ID_STATE_HURT = 1 << 6; 					/// <summary>Hurt's State Flag.</summary>

	[SerializeField] private Transform _meshParent; 			/// <summary>Mesh's Parent.</summary>
	[SerializeField] private HitCollider2D[] _hurtBoxes; 			/// <summary>Hurt-Boxes.</summary>
	[SerializeField] private Collider2D[] _physicalColliders; 	/// <summary>Physical Colliders [Collider2Ds that don't have onTrigger enabled].</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] protected Color gizmosColor; 				/// <summary>Gizmos' Color.</summary>
	[SerializeField] protected float gizmosRadius; 				/// <summary>Gizmos' Radius.</summary>
#endif
	private Health _health; 									/// <summary>Health's Component.</summary>
	private EnemyEventsHandler _eventsHandler; 					/// <summary>EnemyEventsHandler's Component.</summary>
	private VCameraTarget _cameraTarget; 						/// <summary>VCameraTarget's Component.</summary>
	private int _state; 										/// <summary>Agent's Current State.</summary>
	private int _previousState; 								/// <summary>Agent's Previous State.</summary>
	private int _ignoreResetMask; 								/// <summary>State flags to ignore.</summary>
	protected Coroutine behaviorCoroutine; 						/// <summary>Main Behavior Coroutine's reference.</summary>
	private Rigidbody2D _rigidbody; 							/// <summary>[Optional] Rigidbody2D's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets meshParent property.</summary>
	public Transform meshParent
	{
		get { return _meshParent; }
		set { _meshParent = value; }
	}

	/// <summary>Gets and Sets physicalColliders property.</summary>
	public Collider2D[] physicalColliders
	{
		get { return _physicalColliders; }
		set { _physicalColliders = value; }
	}

	/// <summary>Gets and Sets hurtBoxes property.</summary>
	public HitCollider2D[] hurtBoxes
	{
		get { return _hurtBoxes; }
		set { _hurtBoxes = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public int state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public int previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}

	/// <summary>Gets and Sets ignoreResetMask property.</summary>
	public int ignoreResetMask
	{
		get { return _ignoreResetMask; }
		set { _ignoreResetMask = value; }
	}

	/// <summary>Gets health Component.</summary>
	public Health health
	{
		get
		{
			if(_health == null) _health = GetComponent<Health>();
			return _health;
		}
	}

	/// <summary>Gets eventsHandler Component.</summary>
	public EnemyEventsHandler eventsHandler
	{ 
		get
		{
			if(_eventsHandler == null) _eventsHandler = GetComponent<EnemyEventsHandler>();
			return _eventsHandler;
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

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody2D rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
			return _rigidbody;
		}
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos [On Editor Mode].</summary>
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;
	}
#endif

	/// <summary>Resets Enemy's instance to its default values.</summary>
	public virtual void Reset()
	{
		this.ChangeState(ID_STATE_ALIVE);
		health.Reset();
	}

	/// <summary>Callback invoked when Enemy's script is instantiated.</summary>
	protected virtual void Awake()
	{
		health.onHealthEvent += OnHealthEvent;
		this.AddStates(ID_STATE_ALIVE);
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	protected virtual void Start() { /*...*/ }

	/// <summary>Callback invoked when Enemy's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	protected virtual void OnDestroy()
	{
		health.onHealthEvent -= OnHealthEvent;
	}

#region IFiniteStateMachineCallbacks:
	/// <summary>Enters int State.</summary>
	/// <param name="_state">int State that will be entered.</param>
	public virtual void OnEnterState(int _state)
	{
	}
	
	/// <summary>Leaves int State.</summary>
	/// <param name="_state">int State that will be left.</param>
	public virtual void OnExitState(int _state)
	{
	}

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public virtual void OnStatesAdded(int _state)
	{
	}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public virtual void OnStatesRemoved(int _state)
	{
	}
#endregion

#region Callbacks:
	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected virtual void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		switch(_event)
		{
			case HealthEvent.FullyDepleted:
			this.RemoveStates(ID_STATE_ALIVE);
			//OnObjectDeactivation();
			break;
		}
	}
#endregion

	/// <summary>Enables Physics.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public virtual void EnablePhysics(bool _enable)
	{
		EnablePhysicalColliders(_enable);
	}

	/// <summary>Enables Physical Colliders.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public virtual void EnablePhysicalColliders(bool _enable = true)
	{
		if(physicalColliders == null) return;

		foreach(Collider2D collider in physicalColliders)
		{
			collider.gameObject.SetActive(_enable);
		}
	}

	/// <summary>Enables Physical Colliders.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public virtual void EnableHurtBoxes(bool _enable = true)
	{
		if(hurtBoxes == null) return;

		foreach(HitCollider2D collider in hurtBoxes)
		{
			collider.gameObject.SetActive(_enable);
		}
	} 

	/// <returns>States to string</returns>
	public virtual string StatesToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("State Mask:");
		builder.Append("Alive: ");
		builder.AppendLine(this.HasState(ID_STATE_ALIVE).ToString());
		builder.Append("Idle: ");
		builder.AppendLine(this.HasState(ID_STATE_IDLE).ToString());
		builder.Append("PlayerOnSight: ");
		builder.AppendLine(this.HasState(ID_STATE_PLAYERONSIGHT).ToString());
		builder.Append("FollowPlayer: ");
		builder.AppendLine(this.HasState(ID_STATE_FOLLOWPLAYER).ToString());
		builder.Append("Attack: ");
		builder.AppendLine(this.HasState(ID_STATE_ATTACK).ToString());
		builder.Append("Vulnerable: ");
		builder.Append(this.HasState(ID_STATE_VULNERABLE).ToString());
		builder.Append("Hurt: ");
		builder.Append(this.HasState(ID_STATE_HURT).ToString());

		return builder.ToString();
	}

	/// <returns>String representing enemy's stats.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(name);
		builder.AppendLine(" Enemy: ");
		builder.AppendLine(StatesToString());
		builder.AppendLine();
		builder.AppendLine(health.ToString());

		return builder.ToString();
	}
}
}