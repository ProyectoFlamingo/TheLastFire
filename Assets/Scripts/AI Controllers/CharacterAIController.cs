using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public abstract class CharacterAIController<T> : MonoBehaviour where T : Character
{
	[SerializeField] private T _character; 			/// <summary>Character that this AI Controller controls [I know, redundant huh?].</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes: ")]
	[SerializeField] protected Color gizmosColor; 	/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 	/// <summary>Gizmos' Radius.</summary>
#endif
	protected Coroutine behaviorCoroutine; 	/// <summary>Behavior's Coroutine's Reference.</summary>

	/// <summary>Gets and Sets character property.</summary>
	public T character
	{
		get { return _character; }
		set
		{
			if(value == null && _character != null)
			{
				_character.eventsHandler.onIDEvent -= OnCharacterIDEvent;
			
			} else if (value != null)
			{
				if(_character != null && _character != value)
				{
					_character.eventsHandler.onIDEvent -= OnCharacterIDEvent;
					value.eventsHandler.onIDEvent += OnCharacterIDEvent;		
				}
			}

			_character = value;
		}
	}

	/// <summary>Draws Gizmos on Editor mode when CharacterAIController's instance is selected.</summary>
	protected virtual void OnDrawGizmosSelected()
	{
#if UNITY_EDITOR
		Gizmos.color = gizmosColor;
#endif
	}

	/// <summary>Resets CharacterAIController's instance to its default values.</summary>
	public virtual void Reset()
	{
#if UNITY_EDITOR
		gizmosColor = Color.cyan;
		gizmosRadius = 0.25f;
#endif
	}

	/// <summary>CharacterAIController's instance initialization.</summary>
	protected virtual void Awake()
	{
		if(character != null) character.eventsHandler.onIDEvent += OnCharacterIDEvent;
	}

	/// <summary>CharacterAIController's starting actions before 1st Update frame.</summary>
	protected virtual void Start () { /*...*/ }
	
	/// <summary>CharacterAIController's tick at each frame.</summary>
	protected virtual void Update () { /*...*/ }

	/// <summary>Updates CharacterAIController's instance at each Physics Thread's frame.</summary>
	protected virtual void FixedUpdate() { /*...*/ }

	/// <summary>Callback invoked when the character invokes an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected virtual void OnCharacterIDEvent(int _ID) { /*...*/ }
}
}