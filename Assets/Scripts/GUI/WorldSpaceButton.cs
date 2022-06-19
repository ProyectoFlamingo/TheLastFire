using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(ImpactEventHandler))]
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(Button))]
public class WorldSpaceButton : MonoBehaviour
{
	[SerializeField] private GameObjectTag[] _tags; 	/// <summary>Tags that interact with this button.</summary>
	[SerializeField] private Text _text; 				/// <summary>Button's Text Component.</summary>
	private HashSet<int> _onInteraction; 				/// <summary>Objects that are on interaction with this button.</summary>
	private EventsHandler _eventsHandler; 				/// <summary>EventsHandler's Component.</summary>
	private Button _button; 							/// <summary>BButton's Component.</summary>

	/// <summary>Gets tags property.</summary>
	public GameObjectTag[] tags { get { return _tags; } }

	/// <summary>Gets text property.</summary>
	public Text text { get { return _text; } }

	/// <summary>Gets and Sets onInteraction property.</summary>
	public HashSet<int> onInteraction
	{
		get { return _onInteraction; }
		set { _onInteraction = value; }
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
	
	/// <summary>Gets button Component.</summary>
	public Button button
	{ 
		get
		{
			if(_button == null) _button = GetComponent<Button>();
			return _button;
		}
	}

	/// <summary>Callback invoked when WorldSpaceButton's instance is disabled.</summary>
	private void OnDisable()
	{
		if(onInteraction != null) onInteraction.Clear();
	}

	/// <summary>WorldSpaceButton's instance initialization.</summary>
	private void Awake()
	{
		eventsHandler.onTriggerEvent += OnTriggerEvent;
		onInteraction = new HashSet<int>();
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		GameObject obj = _info.collider.gameObject;
		bool hasTag = false;

		foreach(GameObjectTag tag in tags)
		{
			if(obj.tag == tag)
			{
				hasTag = true;
				break;
			}
		}

		if(!hasTag) return;

		int instanceID = obj.GetInstanceID();

		switch(_eventType)
		{
			case HitColliderEventTypes.Enter:
				button.onClick.Invoke();
				//onInteraction.Add(instanceID);
			break;

			case HitColliderEventTypes.Exit:
				/*if(onInteraction.Contains(instanceID))
				{
					onInteraction.Remove(instanceID);
					button.onClick.Invoke();
				}*/
			break;
		}
	}
}
}