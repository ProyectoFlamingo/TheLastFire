using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public enum StateChange
{
	Entered,
	Left,
	Added,
	Removed
}

/// <summary>Event invoked when a Character's state is changed.</summary>
/// <param name="_character">Character that invokes the event.</param>
/// <param name="_state">State Flags.</param>
/// <param name="_stateChange">Type of State Change.</param>
public delegate void OnStateChanged(Character _character, int _state, StateChange _stateChange);

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(Skeleton))]
[RequireComponent(typeof(VCameraTarget))]
[RequireComponent(typeof(HealthEventReceiver))]
[RequireComponent(typeof(TransformDeltaCalculator))]
[RequireComponent(typeof(VirtualAnchorContainer))]
public class Character : PoolGameObject, IStateMachine
{
	public event OnStateChanged onStateChanged; 														/// <summary>OnStateChanged event's delegate.</summary>

	[InfoBox("@ToString()")]
	[SerializeField] private Faction _faction; 															/// <summary>Character's Faction.</summary>
	[Header("Animation's Attributes:")]	
	[TabGroup("Animations")][SerializeField] private Transform _animatorParent; 						/// <summary>Animator's Parent.</summary>
	[TabGroup("Animations")][SerializeField] private Transform _meshParent; 							/// <summary>Mesh's Parent.</summary>
	[TabGroup("Animations")][SerializeField] private Animator _animator; 								/// <summary>Animator's Component.</summary>
	[TabGroup("Animations")][SerializeField] private VAnimatorController _animatorController; 			/// <summary>VAnimatorController's Component.</summary>
	[TabGroup("Animations")][SerializeField] private AnimationEventInvoker _animationEventInvoker; 		/// <summary>AnimationEventInvoker's Component.</summary>
	[TabGroup("Animations")][SerializeField] private Animation _animation; 								/// <summary>Animation's Component.</summary>
	[TabGroup("Animations")][SerializeField] private OnAnimatorMoveOverrider _onAnimatorMoveOverrider; 	/// <summary>OnAnimatorMoveOverrider component attached to the Animator's GameObject.</summary>
	[Space(5f)]
	[TabGroup("Animations")][SerializeField] private int _mainAnimationLayer; 							/// <summary>Main Animations' Layer.</summary>
	[TabGroup("Animations")][SerializeField] private int _attackAnimationLayer; 						/// <summary>Attack Animations' Layer.</summary>
	[Space(5f)]
	[Range(0.0f, 1.0f)]
	[TabGroup("Animations")][SerializeField] private float _clipFadeDuration; 							/// <summary>Default's AnimationClip Fade's Duration.</summary>
	[Space(5f)]
	[Header("Colliders & Hurt-Boxes:")]
	[TabGroup("Colliders & Hurt-Boxes")][SerializeField] private HitCollider2D[] _hurtBoxes; 			/// <summary>Hurt-Boxes.</summary>
	[TabGroup("Colliders & Hurt-Boxes")][SerializeField] private HitCollider2D[] _jointedHitBoxes; 		/// <summary>Jointed Hit-Boxes [for Contact-Weapons].</summary>
	[TabGroup("Colliders & Hurt-Boxes")][SerializeField] private Collider2D[] _physicalColliders; 		/// <summary>Physical Colliders [Collider2Ds that don't have onTrigger enabled].</summary>
	[TabGroup("Colliders & Hurt-Boxes")][SerializeField] private Collider2D[] _triggerColliders; 		/// <summary>Trigger Colliders [Collider2Ds that have onTrigger enabled, they can or cannot be the same Hurt-Boxes' triggers].</summary>
//#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[TabGroup("Gizmos")][SerializeField] protected Color gizmosColor; 									/// <summary>Gizmos' Color.</summary>
	[TabGroup("Gizmos")][SerializeField] protected float gizmosRadius; 									/// <summary>Gizmos' Radius.</summary>
//#endif
	private int _state; 																				/// <summary>Character's Current State.</summary>
	private int _previousState; 																		/// <summary>Character's Previous Current State.</summary>
	public int ignoreResetMask { get; set; } 															/// <summary>Mask that selectively contains state to ignore resetting if they were added again [with AddState's method]. As it is 0 by default, it won't ignore resetting any state [~0 = 11111111]</summary>
	private Health _health; 																			/// <summary>Health's Component.</summary>
	private EventsHandler _eventsHandler; 																/// <summary>EventsHandler's Component.</summary>
	private VCameraTarget _cameraTarget; 																/// <summary>VCameraTarget's Component.</summary>
	private Skeleton _skeleton; 																		/// <summary>Skeleton's Component.</summary>
	private Rigidbody2D _rigidbody; 																	/// <summary>Rigidbody2D's Component.</summary>
	private HealthEventReceiver _healthEventReceiver; 													/// <summary>HealthEventReceiver's Component.</summary>
	private TransformDeltaCalculator _deltaCalculator; 													/// <summary>TransformDeltaCalculator's Component.</summary>
	private VirtualAnchorContainer _anchorContainer; 													/// <summary>VirtualAnchorContainer's Component.</summary>
	public Coroutine behaviorCoroutine; 																/// <summary>Main Behavior Coroutine's reference.</summary>
	public Dictionary<int, Coroutine> coroutinesMap; 													/// <summary>Coroutines' Mapping.</summary>

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

	/// <summary>Gets and Sets jointedHitBoxes property.</summary>
	public HitCollider2D[] jointedHitBoxes
	{
		get { return _jointedHitBoxes; }
		set { _jointedHitBoxes = value; }
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

	/// <summary>Gets and Sets animationEventInvoker property.</summary>
	public AnimationEventInvoker animationEventInvoker
	{
		get { return _animationEventInvoker; }
		set { _animationEventInvoker = value; }
	}

	/// <summary>Gets and Sets onAnimatorMoveOverrider property.</summary>
	public OnAnimatorMoveOverrider onAnimatorMoveOverrider
	{
		get { return _onAnimatorMoveOverrider; }
		set { _onAnimatorMoveOverrider = value; }
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

	/// <summary>Gets anchorContainer Component.</summary>
	public VirtualAnchorContainer anchorContainer
	{ 
		get
		{
			if(_anchorContainer == null) _anchorContainer = GetComponent<VirtualAnchorContainer>();
			return _anchorContainer;
		}
	}

	/// <summary>Gets animation Component.</summary>
	public Animation animation
	{ 
		get
		{
			if(_animation == null) _animation = GetComponent<Animation>();
			return _animation;
		}
	}

	/// <summary>Gets and Sets clipFadeDuration property.</summary>
	public float clipFadeDuration
	{
		get { return _clipFadeDuration; }
		set { _clipFadeDuration = value; }
	}

	/// <summary>Gets mainAnimationLayer property.</summary>
	public int mainAnimationLayer { get { return _mainAnimationLayer; } }

	/// <summary>Gets attackAnimationLayer property.</summary>
	public int attackAnimationLayer { get { return _attackAnimationLayer; } }

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

	/// <summary>Gets healthEventReceiver Component.</summary>
	public HealthEventReceiver healthEventReceiver
	{ 
		get
		{
			if(_healthEventReceiver == null) _healthEventReceiver = GetComponent<HealthEventReceiver>();
			return _healthEventReceiver;
		}
	}
#endregion


	/// <summary>Draws Gizmos [On Editor Mode].</summary>
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;
	}

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
		this.ChangeState(IDs.STATE_ALIVE);
		health.Reset();
	}

	/// <summary>Callback invoked when Enemy's script is instantiated.</summary>
	protected virtual void Awake()
	{
		health.onHealthEvent += OnHealthEvent;
		this.AddStates(IDs.STATE_ALIVE);

		/// Add Character reference to self-contained EventsHandler:
		eventsHandler.character = this;
		coroutinesMap = new Dictionary<int, Coroutine>();
		coroutinesMap.Add(IDs.COROUTINE_DEFAULT, null);
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	protected virtual void Start()
	{
		if(animationEventInvoker != null) animationEventInvoker.AddIntActionListener(OnAnimationIntEvent);
	}

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
	public virtual void OnEnterState(int _state)
	{
		if(onStateChanged != null) onStateChanged(this, _state, StateChange.Entered);
	}

	/// <summary>Exits int State.</summary>
	/// <param name="_state">int State that will be left.</param>
	public virtual void OnExitState(int _state)
	{
		if(onStateChanged != null) onStateChanged(this, _state, StateChange.Left);
	}

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public virtual void OnStatesAdded(int _state)
	{
		if(onStateChanged != null) onStateChanged(this, _state, StateChange.Added);
	}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public virtual void OnStatesRemoved(int _state)
	{
		if(onStateChanged != null) onStateChanged(this, _state, StateChange.Removed);
	}
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
				this.AddStates(IDs.STATE_HURT);
			break;

			case HealthEvent.HitStunEnds:
				this.RemoveStates(IDs.STATE_HURT);
			break;

			case HealthEvent.InvincibilityEnds:
				this.RemoveStates(IDs.STATE_HURT);
			break;

			case HealthEvent.FullyDepleted:
				this.RemoveStates(IDs.STATE_ALIVE);
			//OnObjectDeactivation();
			break;
		}
	}

	/// <summary>Callback invoked when an Animation Event is invoked.</summary>
	/// <param name="_ID">Int argument.</param>
	protected virtual void OnAnimationIntEvent(int _ID) { /*...*/ }
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

#region StringDebugging:
	/// <returns>String representing enemy's stats.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(name);
		builder.AppendLine(" Character: ");
		builder.Append("Faction: ");
		builder.AppendLine(faction.ToString());
		builder.AppendLine();
		builder.Append("Previous States: ");
		builder.AppendLine(IDs.GetStates(previousState));
		builder.Append("States: ");
		builder.AppendLine(IDs.GetStates(state));
		builder.AppendLine();
		builder.AppendLine(health.ToString());

		return builder.ToString();
	}
#endregion
}
}