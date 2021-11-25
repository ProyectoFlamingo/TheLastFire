using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(Skeleton))]
[RequireComponent(typeof(VCameraTarget))]
public class Character : PoolGameObject, IStateMachine
{
	public event OnIDEvent onIDEvent; 																/// <summary>OnIDEvent's delegate.</summary>
		
	public const int ID_STATE_DEAD = 0; 															/// <summary>Dead State's ID.</summary>
	public const int ID_STATE_ALIVE = 1 << 0; 														/// <summary>Alive State's ID.</summary>
	public const int ID_STATE_IDLE = 1 << 1; 														/// <summary>Idle State's ID.</summary>
	public const int ID_STATE_HURT = 1 << 2; 														/// <summary>Hurt State's ID.</summary>
	public const int ID_STATE_COLLIDED = 1 << 3; 													/// <summary>Collider State's ID.</summary>
	public const int ID_STATE_ATTACKING = 1 << 4; 													/// <summary>Attacking's State's ID.</summary>
	
	[SerializeField] private Faction _faction; 														/// <summary>Character's Faction.</summary>
	[Header("Animation's Attributes:")]	
	[TabGroup("Animations")][SerializeField] private Transform _animatorParent; 					/// <summary>Animator's Parent.</summary>
	[TabGroup("Animations")][SerializeField] private Transform _meshParent; 						/// <summary>Mesh's Parent.</summary>
	[TabGroup("Animations")][SerializeField] private Animator _animator; 							/// <summary>Animator's Component.</summary>
	[TabGroup("Animations")][SerializeField] private VAnimatorController _animatorController; 		/// <summary>VAnimatorController's Component.</summary>
	[Space(5f)]
	[TabGroup("Animations")][SerializeField] private float _clipFadeDuration; 						/// <summary>Default's AnimationClip Fade's Duration.</summary>
	[Space(5f)]
	[Header("Colliders & Hurt-Boxes:")]
	[TabGroup("Colliders & Hurt-Boxes")][SerializeField] private HitCollider2D[] _hurtBoxes; 		/// <summary>Hurt-Boxes.</summary>
	[TabGroup("Colliders & Hurt-Boxes")][SerializeField] private Collider2D[] _physicalColliders; 	/// <summary>Physical Colliders [Collider2Ds that don't have onTrigger enabled].</summary>
	[TabGroup("Colliders & Hurt-Boxes")][SerializeField] private Collider2D[] _triggerColliders; 	/// <summary>Trigger Colliders [Collider2Ds that have onTrigger enabled, they can or cannot be the same Hurt-Boxes' triggers].</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[TabGroup("Gizmos")][SerializeField] protected Color gizmosColor; 								/// <summary>Gizmos' Color.</summary>
	[TabGroup("Gizmos")][SerializeField] protected float gizmosRadius; 								/// <summary>Gizmos' Radius.</summary>
#endif
	private int _state; 																			/// <summary>Character's Current State.</summary>
	private int _previousState; 																	/// <summary>Character's Previous Current State.</summary>
	public int ignoreResetMask { get; set; } 														/// <summary>Mask that selectively contains state to ignore resetting if they were added again [with AddState's method]. As it is 0 by default, it won't ignore resetting any state [~0 = 11111111]</summary>
	private Health _health; 																		/// <summary>Health's Component.</summary>
	private EventsHandler _eventsHandler; 															/// <summary>EventsHandler's Component.</summary>
	private VCameraTarget _cameraTarget; 															/// <summary>VCameraTarget's Component.</summary>
	private Skeleton _skeleton; 																	/// <summary>Skeleton's Component.</summary>
	private Rigidbody2D _rigidbody; 																
	protected Coroutine behaviorCoroutine; 															/// <summary>Main Behavior Coroutine's reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets faction property.</summary>
	public Faction faction
	{
		get { return _faction; }
		set { _faction = value; }
	}

	/// <summary>Gets and Sets animatorParent property.</summary>
	public Transform animatorParent
	{
		get { return _animatorParent; }
		set { _animatorParent = value; }
	}

	/// <summary>Gets and Sets meshParent property.</summary>
	public Transform meshParent
	{
		get { return _meshParent; }
		set { _meshParent = value; }
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

	/// <summary>Gets and Sets physicalColliders property.</summary>
	public Collider2D[] physicalColliders
	{
		get { return _physicalColliders; }
		set { _physicalColliders = value; }
	}

	/// <summary>Gets and Sets triggerColliders property.</summary>
	public Collider2D[] triggerColliders
	{
		get { return _triggerColliders; }
		set { _triggerColliders = value; }
	}

	/// <summary>Gets and Sets hurtBoxes property.</summary>
	public HitCollider2D[] hurtBoxes
	{
		get { return _hurtBoxes; }
		set { _hurtBoxes = value; }
	}

	/// <summary>Gets animatorController Component.</summary>
	public VAnimatorController animatorController
	{ 
		get
		{
			if(_animatorController == null) _animatorController = GetComponent<VAnimatorController>();
			return _animatorController;
		}
	}

	/// <summary>Gets and Sets clipFadeDuration property.</summary>
	public float clipFadeDuration
	{
		get { return _clipFadeDuration; }
		set { _clipFadeDuration = value; }
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
	public EventsHandler eventsHandler
	{ 
		get
		{
			if(_eventsHandler == null) _eventsHandler = GetComponent<EventsHandler>();
			return _eventsHandler;
		}
	}

	/// <summary>Gets skeleton Component.</summary>
	public Skeleton skeleton
	{ 
		get
		{
			if(_skeleton == null) _skeleton = GetComponent<Skeleton>();
			return _skeleton;
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

//---------------------------------------
//	 		UNITY-CALLBACKS: 			|
//---------------------------------------
	/// <summary>Resets Character's instance to its default values.</summary>
	public virtual void Reset()
	{
#if UNITY_EDITOR
		gizmosColor = Color.cyan;
		gizmosRadius = 0.25f;
#endif
		this.ChangeState(ID_STATE_ALIVE);
		health.Reset();
	}

	/// <summary>Callback invoked when Enemy's script is instantiated.</summary>
	protected virtual void Awake()
	{
		health.onHealthEvent += OnHealthEvent;
		this.AddStates(ID_STATE_ALIVE);

		/// Add Character reference to self-contained EventsHandler:
		eventsHandler.character = this;
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	protected virtual void Start() { /*...*/ }

	/// <summary>Updates Character's instance at each frame.</summary>
	protected virtual void Update() { /*...*/ }

	/// <summary>Updates Character's instance at each Physics Thread's frame.</summary>
	protected virtual void FixedUpdate() { /*...*/ }

	/// <summary>Callback invoked when Enemy's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	protected virtual void OnDestroy()
	{
		health.onHealthEvent -= OnHealthEvent;
	}

#region IFiniteStateMachine:
	/// <summary>Enters int State.</summary>
	/// <param name="_state">int State that will be entered.</param>
	public virtual void OnEnterState(int _state) {/*...*/}

	/// <summary>Exits int State.</summary>
	/// <param name="_state">int State that will be left.</param>
	public virtual void OnExitState(int _state) {/*...*/}

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public virtual void OnStatesAdded(int _state) {/*...*/}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public virtual void OnStatesRemoved(int _state) {/*...*/}
#endregion

#region Colliders&HurtBoxesEnabling:
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

	/// <summary>Enables Trigger Colliders.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public virtual void EnableTriggerColliders(bool _enable = true)
	{
		if(triggerColliders == null) return;

		foreach(Collider2D collider in triggerColliders)
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
#endregion

#region Callbacks:
	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected virtual void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		switch(_event)
		{
			case HealthEvent.Depleted:
				this.AddStates(ID_STATE_HURT);
			break;

			case HealthEvent.HitStunEnds:
				this.RemoveStates(ID_STATE_HURT);
			break;

			case HealthEvent.InvincibilityEnds:
				this.RemoveStates(ID_STATE_HURT);
			break;

			case HealthEvent.FullyDepleted:
				this.RemoveStates(ID_STATE_ALIVE);
			//OnObjectDeactivation();
			break;
		}
	}
#endregion

#region EventInvoking:
	/// <summary>Invokes onIDEvent's delegate.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected virtual void InvokeIDEvent(int _ID)
	{
		eventsHandler.InvokeIDEvent(_ID);
	}

	/// <summary>Invokes onCharacterIDEvent's delegate.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected virtual void InvokeCharacterIDEvent(int _ID)
	{
		eventsHandler.InvokeCharacterIDEvent(_ID);
	}
#endregion

	public void EmitSoundEffect(CollectionIndex _index, int _source = 0, float _volumeScale = 1.0f)
	{
		AudioController.PlayOneShot(SourceType.SFX, _source, _index, _volumeScale);
	}

#region StringDebugging:
	/// <returns>States to string</returns>
	public virtual string StatesToString()
	{
		StringBuilder builder = new StringBuilder();

		/*builder.AppendLine("State Mask:");
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
		builder.Append(this.HasState(ID_STATE_HURT).ToString());*/

		return builder.ToString();
	}

	/// <returns>String representing enemy's stats.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(name);
		builder.AppendLine(" Character: ");
		builder.Append("Faction: ");
		builder.AppendLine(faction.ToString());
		builder.AppendLine(StatesToString());
		builder.AppendLine();
		builder.AppendLine(health.ToString());

		return builder.ToString();
	}
#endregion
}
}