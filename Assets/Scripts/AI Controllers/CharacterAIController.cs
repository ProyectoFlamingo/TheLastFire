using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public abstract class CharacterAIController<T> : MonoBehaviour, IStateMachine where T : Character
{
	[InfoBox("@ToString()")]
	[SerializeField] private T _character; 											/// <summary>Character that this AI Controller controls [I know, redundant huh?].</summary>
	[Space(5f)]
	[SerializeField] private float _projectionTime; 								/// <summary>Default Projection Time.</summary>
	[SerializeField] private float _minDistanceToReachTarget; 						/// <summary>Minimum Distance to reach a target.</summary>
//#if UNITY_EDITOR
	[Space(5f)]
	[TabGroup("Gizmos", "Gizmos")][Header("Gizmos' Attributes: ")]
	[TabGroup("Gizmos", "Gizmos")][SerializeField] protected Color gizmosColor; 	/// <summary>Gizmos' Color.</summary>
	[TabGroup("Gizmos", "Gizmos")][SerializeField] private float gizmosRadius; 		/// <summary>Gizmos' Radius.</summary>
//#endif
	protected Coroutine behaviorCoroutine; 											/// <summary>Behavior's Coroutine's Reference.</summary>
	private int _state; 															/// <summary>Character's Current State.</summary>
	private int _previousState; 													/// <summary>Character's Previous Current State.</summary>
	public int ignoreResetMask { get; set; } 										/// <summary>Mask that selectively contains state to ignore resetting if they were added again [with AddState's method]. As it is 0 by default, it won't ignore resetting any state [~0 = 11111111]</summary>
#if UNITY_EDITOR
	protected StringBuilder debugBuilder; 											/// <summary>Additional Debug's String Builder.</summary>
#endif

	/// <summary>Gets and Sets character property.</summary>
	public T character
	{
		get { return _character; }
		set
		{
			if(value == null && _character != null)
			{
				SubscribeToCharacterEvents(_character, false);
			
			} else if (value != null)
			{
				if(_character != null && _character != value)
				{
					SubscribeToCharacterEvents(_character, false);
					SubscribeToCharacterEvents(value, true);	
				}
			}

			_character = value;
		}
	}

	/// <summary>Gets and Sets projectionTime property.</summary>
	public float projectionTime
	{
		get { return _projectionTime; }
		set { _projectionTime = value; }
	}

	/// <summary>Gets and Sets minDistanceToReachTarget property.</summary>
	public float minDistanceToReachTarget
	{
		get { return _minDistanceToReachTarget; }
		set { _minDistanceToReachTarget = value; }
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

	/// <summary>Draws Gizmos on Editor mode when CharacterAIController's instance is selected.</summary>
	protected virtual void OnDrawGizmosSelected()
	{
//#if UNITY_EDITOR
		Gizmos.color = gizmosColor;
//#endif
	}

	/// <summary>Resets CharacterAIController's instance to its default values.</summary>
	public virtual void Reset()
	{
//#if UNITY_EDITOR
		gizmosColor = Color.cyan;
		gizmosRadius = 0.25f;
//#endif
	}

	/// <summary>CharacterAIController's instance initialization.</summary>
	protected virtual void Awake()
	{
		if(character != null) SubscribeToCharacterEvents(character, true);

#if UNITY_EDITOR
		debugBuilder = new StringBuilder();
#endif
	}

	/// <summary>CharacterAIController's starting actions before 1st Update frame.</summary>
	protected virtual void Start () { /*...*/ }
	
	/// <summary>CharacterAIController's tick at each frame.</summary>
	protected virtual void Update () { /*...*/ }

	/// <summary>Updates CharacterAIController's instance at each Physics Thread's frame.</summary>
	protected virtual void FixedUpdate() { /*...*/ }

	/// <summary>Subscribes to Character's Events [Deactivation, ID and State events].</summary>
	/// <param name="_character">Target Character.</param>
	/// <param name="_subscribe">Subscribe? true by default.</param>
	protected virtual void SubscribeToCharacterEvents(T _character, bool _subscribe = true)
	{
		switch(_subscribe)
		{
			case true:
			_character.eventsHandler.onIDEvent += OnCharacterIDEvent;
			_character.eventsHandler.onCharacterDeactivated += OnCharacterDeactivated;
			_character.onStateChanged += OnCharacterStateChanged;
			break;

			case false:
			_character.eventsHandler.onIDEvent -= OnCharacterIDEvent;
			_character.eventsHandler.onCharacterDeactivated -= OnCharacterDeactivated;
			_character.onStateChanged -= OnCharacterStateChanged;
			break;
		}
	}

	/// <summary>Callback invoked when a Character's state is changed.</summary>
	/// <param name="_character">Character that invokes the event.</param>
	/// <param name="_flags">State Flags.</param>
	/// <param name="_stateChange">Type of State Change.</param>
	protected virtual void OnCharacterStateChanged(Character _character, int _state, StateChange _stateChange)
	{
#if UNITY_EDITOR
		StringBuilder builder = new StringBuilder();

		builder.Append("Character ");
		builder.Append(_character.gameObject.name);
		builder.Append(" ");
		builder.Append(_stateChange.ToString());
		builder.Append(" the following state(s): ");
		builder.Append(IDs.GetStates(_state));

		Debug.Log(builder.ToString());
#endif
	}

	/// <summary>Callback invoked when the character invokes an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected virtual void OnCharacterIDEvent(int _ID) { /*...*/ }

	/// <summary>Callback invoked when the character is deactivated.</summary>
	/// <param name="_character">Character that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	protected virtual void OnCharacterDeactivated(Character _character, DeactivationCause _cause, Trigger2DInformation _info) { /*...*/ }

#region IFiniteStateMachine:
	/// <summary>Enters int State.</summary>
	/// <param name="_state">int State that will be entered.</param>
	public virtual void OnEnterState(int _state) { /*...*/ }

	/// <summary>Exits int State.</summary>
	/// <param name="_state">int State that will be left.</param>
	public virtual void OnExitState(int _state) { /*...*/ }

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public virtual void OnStatesAdded(int _state) { /*...*/ }

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public virtual void OnStatesRemoved(int _state) { /*...*/ }
#endregion

	/// <returns>String representing this Character AI's Controller.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Previous State(s: )");
		builder.AppendLine(IDs.GetStates(previousState));
		builder.Append("State(s: )");
		builder.AppendLine(IDs.GetStates(state));

#if UNITY_EDITOR
		if(debugBuilder != null)
		{
			builder.AppendLine("-------------------------");
			builder.AppendLine(debugBuilder.ToString());
			builder.AppendLine("-------------------------");
		}
#endif

		return builder.ToString();
	}
}
}