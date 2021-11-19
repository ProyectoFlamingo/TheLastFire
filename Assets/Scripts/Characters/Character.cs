using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(Skeleton))]
public class Character : PoolGameObject, IStateMachine
{
	public event OnIDEvent onIDEvent; 						/// <summary>OnIDEvent's delegate.</summary>

	public const int STATE_FLAG_ALIVE = 0; 					/// <summary>Alive's State Flag.</summary>
	public const int STATE_FLAG_HURT = 1; 					/// <summary>Hurt's State Flag.</summary>
	public const int STATE_FLAG_DEAD = 2; 					/// <summary>Dead's State Flag.</summary>
	public const int ID_STATE_DEAD = 0; 					/// <summary>Dead State's ID.</summary>
	public const int ID_STATE_ALIVE = 1 << 0; 				/// <summary>Alive State's ID.</summary>
	public const int ID_STATE_IDLE = 1 << 1; 				/// <summary>Idle State's ID.</summary>
	public const int ID_STATE_HURT = 1 << 2; 				/// <summary>Hurt State's ID.</summary>
	public const int ID_STATE_COLLIDED = 1 << 3; 			/// <summary>Collider State's ID.</summary>
	public const int ID_STATE_ATTACKING = 1 << 4; 			/// <summary>Attacking's State's ID.</summary>

	[Header("Animator's Attributes:")]
	[SerializeField] private Transform _animatorParent; 	/// <summary>Animator's Parent.</summary>
	[SerializeField] private Animator _animator; 			/// <summary>Animator's Component.</summary>
	[SerializeField] private float _clipFadeDuration; 		/// <summary>Default's AnimationClip Fade's Duration.</summary>
	private int _state; 									/// <summary>Character's Current State.</summary>
	private int _previousState; 							/// <summary>Character's Previous Current State.</summary>
	public int ignoreResetMask { get; set; } 				/// <summary>Mask that selectively contains state to ignore resetting if they were added again [with AddState's method]. As it is 0 by default, it won't ignore resetting any state [~0 = 11111111]</summary>
	private Health _health; 								/// <summary>Health's Component.</summary>
	private EventsHandler _eventsHandler; 					/// <summary>EventsHandler's Component.</summary>
	private Skeleton _skeleton; 							/// <summary>Skeleton's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets animatorParent property.</summary>
	public Transform animatorParent
	{
		get { return _animatorParent; }
		set { _animatorParent = value; }
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
#endregion

	/// <summary>Callback invoked when Enemy's script is instantiated.</summary>
	protected virtual void Awake()
	{
		health.onHealthEvent += OnHealthEvent;
		this.AddStates(ID_STATE_ALIVE);
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	protected virtual void Start()
	{

	}

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

		//Debug.Log(name + " Received Health Event: " + _event.ToString());
	}

	/// <summary>Invokes onIDEvent's delegate.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected virtual void InvokeIDEvent(int _ID)
	{
		eventsHandler.InvokeIDEvent(_ID);
	}

	public void EmitSoundEffect(CollectionIndex _index, int _source = 0, float _volumeScale = 1.0f)
	{
		AudioController.PlayOneShot(SourceType.SFX, _source, _index, _volumeScale);
	}
}
}