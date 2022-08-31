using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Flags]
public enum DeactivationCause
{
	Impacted = 1,
	Destroyed = 2,
	LifespanOver = 4,
	Other = 8,
	LeftBoundaries = Other,

	ImpactedAndDestroyed = Impacted | Destroyed,
	ImpactedAndLifespanOver = Impacted | LifespanOver,
	All = Impacted | Destroyed | LifespanOver
}

/// <summary>Event invoked when an ID event occurs.</summary>
/// <param name="_ID">Event's ID.</param>
public delegate void OnIDEvent(int _ID);

/// <summary>Event invoked when a Character ID Event occurs.</summary>
/// <param name="_character">Character that invoked the event.</param>
/// <param name="_eventID">Event's ID.</param>
/// <param name="_info">Trigger2D's Information.</param>
public delegate void OnCharacterIDEvent(Character _character, int _eventID);

/// <summary>Event invoked when a ContactWeapon ID Event occurs.</summary>
/// <param name="_contactWeapon">ContactWeapon that invoked the event.</param>
/// <param name="_eventID">Event's ID.</param>
/// <param name="_info">Trigger2D's Information.</param>
public delegate void OnContactWeaponIDEvent(ContactWeapon _contactWeapon, int _eventID, Trigger2DInformation _info);

/// <summary>Event invoked when a Collision2D intersection is received.</summary>
/// <param name="_info">Trigger2D's Information.</param>
/// <param name="_eventType">Type of the event.</param>
/// <param name="_ID">Optional ID of the HitCollider2D.</param>
public delegate void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0);

/// <summary>Event invoked when the GameObject is deactivated.</summary>
/// <param name="_cause">Cause of the deactivation.</param>
/// <param name="_info">Additional Trigger2D's information.</param>
public delegate void OnDeactivated(DeactivationCause _cause, Trigger2DInformation _info);

/// <summary>Event invoked when the Character is deactivated.</summary>
/// <param name="_character">Character that invoked the event.</param>
/// <param name="_cause">Cause of the deactivation.</param>
/// <param name="_info">Additional Trigger2D's information.</param>
public delegate void OnCharacterDeactivated(Character _character, DeactivationCause _cause, Trigger2DInformation _info);

/// <summary>Event invoked when the ContactWeapon is deactivated.</summary>
/// <param name="_contactWeapon">ContactWeapon that invoked the event.</param>
/// <param name="_cause">Cause of the deactivation.</param>
/// <param name="_info">Additional Trigger2D's information.</param>
public delegate void OnContactWeaponDeactivated(ContactWeapon _contactWeapon, DeactivationCause _cause, Trigger2DInformation _info);

/// <summary>Event invoked when the PoolGameObject is deactivated.</summary>
/// <param name="_contactWeapon">PoolGameObject that invoked the event.</param>
/// <param name="_cause">Cause of the deactivation.</param>
/// <param name="_info">Additional Trigger2D's information.</param>
public delegate void OnPoolGameObjectDeactivated(PoolGameObject _object, DeactivationCause _cause, Trigger2DInformation _info);

public class EventsHandler : MonoBehaviour
{
	[SerializeField] private PoolGameObject _poolObject; 					/// <summary>PoolGameObject's Reference.</summary>
	[SerializeField] private Character _character; 							/// <summary>Character's Reference.</summary>
	[SerializeField] private ContactWeapon _contactWeapon; 					/// <summary>ContactWeapon's Reference.</summary>
	public event OnIDEvent onIDEvent; 										/// <summary>OnIDEvent's delegate.</summary>
	public event OnCharacterIDEvent onCharacterIDEvent; 					/// <summary>OnCharacterIDEvent's Delegate.</summary>
	public event OnContactWeaponIDEvent onContactWeaponIDEvent; 			/// <summary>OnContactWeaponIDEvent's Delegate.</summary>
	public event OnTriggerEvent onTriggerEvent; 							/// <summary>OnTriggerEvent's delegate.</summary>
	public event OnDeactivated onDeactivated; 								/// <summary>OnDeactivated's delegate.</summary>
	public event OnCharacterDeactivated onCharacterDeactivated; 			/// <summary>OnCharacterDeactivated's delegate.</summary>
	public event OnContactWeaponDeactivated onContactWeaponDeactivated; 	/// <summary>OnContactWeaponDeactivated's delegate.</summary>
	public event OnPoolGameObjectDeactivated onPoolGameObjectDeactivated; 	/// <summary>OnPoolGameObjectDeactivated's delegate.</summary>
	public event OnHealthInstanceEvent onHealthEvent; 						/// <summary>OnHealthInstanceEvent's delegate.</summary>

#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] protected bool debug; 									/// <summary>Debug?.</summary>
#endif

	/// <summary>Gets and Sets poolObject property.</summary>
	public PoolGameObject poolObject
	{
		get { return _poolObject; }
		set { _poolObject = value; }
	}

	/// <summary>Gets and Sets character property.</summary>
	public Character character
	{
		get { return _character; }
		set { _character = value; }
	}

	/// <summary>Gets and Sets contactWeapon property.</summary>
	public ContactWeapon contactWeapon
	{
		get { return _contactWeapon; }
		set { _contactWeapon = value; }
	}

	/// <summary>Invokes ID's Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	public void InvokeIDEvent(int _ID)
	{
#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", gameObject.name, " invoked ID Event. ", _ID.ToString());
#endif

		if(onIDEvent != null) onIDEvent(_ID);
	}

	/// <summary>Invokes Character ID's Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	public void InvokeCharacterIDEvent(int _ID)
	{
		if(character == null) return;

#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", character.gameObject.name, " invoked ID Event. ", _ID.ToString());
#endif

		if(onCharacterIDEvent != null) onCharacterIDEvent(character, _ID);
	}

	/// <summary>Invokes ContactWeapon ID's Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	/// <param name="_info">Trigger2D's Information [default by default].</param>
	public void InvokeContactWeaponIDEvent(int _ID, Trigger2DInformation _info = default(Trigger2DInformation))
	{
		if(contactWeapon == null) return;
		
#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", contactWeapon.gameObject.name, " invoked ID Event. ", _ID.ToString());
#endif

		if(onContactWeaponIDEvent != null) onContactWeaponIDEvent(contactWeapon, _ID, _info);
	}

	/// <summary>Invokes Impact's Event.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public void InvokeTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", gameObject.name, " invoked Impact Event. Interaction Type: ", _eventType.ToString(), ", ID: ", _ID.ToString(), ", ", _info.ToString());
#endif

		if(onTriggerEvent != null) onTriggerEvent(_info, _eventType, _ID);
	}

	/// <summary>Invokes OnDeactivation's Event and Deactivates itself [so it can be a free Pool Object resource].</summary>
	/// <param name="_cause">Cause of the Deactivation.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void InvokeDeactivationEvent(DeactivationCause _cause, Trigger2DInformation _info = default(Trigger2DInformation))
	{
#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", gameObject.name, " invoked Deactivation Event. Cause: ", _cause.ToString(), ", ", _info.ToString());
#endif

		if(onDeactivated != null) onDeactivated(_cause, _info);
	}

	/// <summary>Invokes Character's OnDeactivation's Event and Deactivates itself [so it can be a free Pool Object resource].</summary>
	/// <param name="_cause">Cause of the Deactivation.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void InvokeCharacterDeactivationEvent(DeactivationCause _cause, Trigger2DInformation _info = default(Trigger2DInformation))
	{
		if(character == null) return;

#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", character.gameObject.name, " invoked Deactivation Event. Cause: ", _cause.ToString(), ", ", _info.ToString());
#endif

		if(onCharacterDeactivated != null) onCharacterDeactivated(character, _cause, _info);
	}

	/// <summary>Invokes ContactWeapon's OnDeactivation's Event and Deactivates itself [so it can be a free Pool Object resource].</summary>
	/// <param name="_cause">Cause of the Deactivation.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void InvokeContactWeaponDeactivationEvent(DeactivationCause _cause, Trigger2DInformation _info = default(Trigger2DInformation))
	{
		if(contactWeapon == null) return;

#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", contactWeapon.gameObject.name, " invoked Deactivation Event. Cause: ", _cause.ToString(), ", ", _info.ToString());
#endif

		if(onContactWeaponDeactivated != null) onContactWeaponDeactivated(contactWeapon, _cause, _info);
	}

	/// <summary>Invokes PoolGameObject's OnDeactivation's Event and Deactivates itself [so it can be a free Pool Object resource].</summary>
	/// <param name="_cause">Cause of the Deactivation.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void InvokePoolGameObjectDeactivationEvent(DeactivationCause _cause, Trigger2DInformation _info = default(Trigger2DInformation))
	{
		if(poolObject == null) return;

#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", poolObject.gameObject.name, " invoked Deactivation Event. Cause: ", _cause.ToString(), ", ", _info.ToString());
#endif

		if(onPoolGameObjectDeactivated != null) onPoolGameObjectDeactivated(poolObject, _cause, _info);
	}

	/// <summary>Invokes OnHealthInstanceEvent's Event.</summary>
	/// <param name="_health">Health's Instance.</param>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
	public void InvokeHealthEvent(Health _health, HealthEvent _event, float _amount = 0.0f)
	{
#if UNITY_EDITOR
		if(debug) VDebug.Log(LogType.Log, "[EventsHandler] ", _health.name, " invoked OnHealthInstanceEvent. Event: ", _event.ToString(), ", Amount: ", _amount, ", Health's Data: ", _health.ToString());
#endif

		if(onHealthEvent != null) onHealthEvent(_health, _event, _amount);
	}
}
}