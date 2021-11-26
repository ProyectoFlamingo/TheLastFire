using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Event invoked when the projectile is deactivated.</summary>
/// <param name="_enemy">Enemy that invoked the event.</param>
/// <param name="_cause">Cause of the deactivation.</param>
/// <param name="_info">Additional Trigger2D's information.</param>
public delegate void OnEnemyDeactivated(Enemy _enemy, DeactivationCause _cause, Trigger2DInformation _info);

/// <summary>Event invoked when a Enemy Event occurs.</summary>
/// <param name="_enemy">Enemy that invoked the event.</param>
/// <param name="_eventID">Event's ID.</param>
/// <param name="_info">Trigger2D's Information.</param>
public delegate void OnEnemyEvent(Enemy _enemy, int _eventID, Trigger2DInformation _info);

public class EnemyEventsHandler : EventsHandler
{
	public event OnEnemyDeactivated onEnemyDeactivated; 	/// <summary>OnEnemyDeactivated's delegate.</summary>
	public event OnEnemyEvent onEnemyEvent; 				/// <summary>OnEnemyEvent's delegate.</summary>

	/// <summary>Invokes OnDeactivation's Event and Deactivates itself [so it can be a free Pool Object resource].</summary>
	/// <param name="_enemy">Enemy that invokes the event.</param>
	/// <param name="_cause">Cause of the Deactivation.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void InvokeEnemyDeactivationEvent(Enemy _enemy, DeactivationCause _cause, Trigger2DInformation _info = default(Trigger2DInformation))
	{
#if UNITY_EDITOR
		VDebug.Log(
			LogType.Log,
			"[EnemyEventsHandler] ",
			gameObject.name,
			" invoked Deactivation Event. Cause: ",
			_cause.ToString(),
			", ",
			_info.ToString()
		);
#endif
		if(onEnemyDeactivated != null) onEnemyDeactivated(_enemy, _cause, _info);
	}

	/// <summary>Invokes OnEnemyEvent.</summary>
	/// <param name="_enemy">Enemy that invokes the event.</param>
	/// <param name="_ID">Event's ID.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	protected void InvokeEnemyEvent(Enemy _enemy, int _ID, Trigger2DInformation _info = default(Trigger2DInformation))
	{
#if UNITY_EDITOR
		VDebug.Log(
			LogType.Log,
			"[EnemyEventsHandler] ",
			gameObject.name,
			" invoked Enemy Event. ID: ",
			_ID.ToString(),
			", ",
			_info.ToString()
		);
#endif
		if(onEnemyEvent != null) onEnemyEvent(_enemy, _ID, _info);
	}
}
}