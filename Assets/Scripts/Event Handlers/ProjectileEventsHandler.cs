using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Event invoked when the projectile is deactivated.</summary>
/// <param name="_projectile">Projectile that invoked the event.</param>
/// <param name="_cause">Cause of the deactivation.</param>
/// <param name="_info">Additional Trigger2D's information.</param>
public delegate void OnProjectileDeactivated(Projectile _projectile, DeactivationCause _cause, Trigger2DInformation _info);

/// <summary>Event invoked when a Projectile Event occurs.</summary>
/// <param name="_projectile">Projectile that invoked the event.</param>
/// <param name="_eventID">Event's ID.</param>
/// <param name="_info">Trigger2D's Information.</param>
public delegate void OnProjectileEvent(Projectile _projectile, int _eventID, Trigger2DInformation _info);

public class ProjectileEventsHandler : EventsHandler
{
	public event OnProjectileDeactivated onProjectileDeactivated; 	/// <summary>OnProjectileDeactivated's delegate.</summary>
	public event OnProjectileEvent onProjectileEvent; 				/// <summary>OnProjectileEvent's delegate.</summary>

	/// <summary>Invokes OnDeactivation's Event and Deactivates itself [so it can be a free Pool Object resource].</summary>
	/// <param name="_projectile">Projectile that invokes the event.</param>
	/// <param name="_cause">Cause of the Deactivation.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void InvokeProjectileDeactivationEvent(Projectile _projectile, DeactivationCause _cause, Trigger2DInformation _info = default(Trigger2DInformation))
	{
#if UNITY_EDITOR
		if(debug) VDebug.Log(
			"[ProjectileEventsHandler] ",
			gameObject.name,
			" invoked Deactivation Event. Cause: ",
			_cause.ToString(),
			", ",
			_info.ToString()
		);
#endif
		if(onProjectileDeactivated != null) onProjectileDeactivated(_projectile, _cause, _info);
	}

	/// <summary>Invokes OnProjectileEvent.</summary>
	/// <param name="_projectile">Projectile that invokes the event.</param>
	/// <param name="_ID">Event's ID.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void InvokeProjectileEvent(Projectile _projectile, int _ID, Trigger2DInformation _info = default(Trigger2DInformation))
	{
#if UNITY_EDITOR
		if(debug) VDebug.Log(
			"[ProjectileEventsHandler] ",
			gameObject.name,
			" invoked Projectile Event. ID: ",
			_ID.ToString(),
			", ",
			_info.ToString()
		);
#endif
		if(onProjectileEvent != null) onProjectileEvent(_projectile, _ID, _info);
	}
}
}