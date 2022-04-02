using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public abstract class CharacterAIController<T> : MonoBehaviour where T : Character
{
	[SerializeField] private T _character; 						/// <summary>Character that this AI Controller controls [I know, redundant huh?].</summary>
	[Space(5f)]
	[SerializeField] private float _projectionTime; 			/// <summary>Default Projection Time.</summary>
	[SerializeField] private float _minDistanceToReachTarget; 	/// <summary>Minimum Distance to reach a target.</summary>
//#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes: ")]
	[SerializeField] protected Color gizmosColor; 				/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 				/// <summary>Gizmos' Radius.</summary>
//#endif
	protected Coroutine behaviorCoroutine; 						/// <summary>Behavior's Coroutine's Reference.</summary>

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
	protected virtual void OnCharacterStateChanged(Character _character, int _state, StateChange _stateChange) { /*...*/ }

	/// <summary>Callback invoked when the character invokes an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected virtual void OnCharacterIDEvent(int _ID) { /*...*/ }

	/// <summary>Callback invoked when the character is deactivated.</summary>
	/// <param name="_character">Character that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	protected virtual void OnCharacterDeactivated(Character _character, DeactivationCause _cause, Trigger2DInformation _info) { /*...*/ }
}
}