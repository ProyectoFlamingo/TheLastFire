using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Projectile))]
[RequireComponent(typeof(SelfMotionPerformer))]
[RequireComponent(typeof(EventsHandler))]
public class BreakableTarget : PoolGameObject
{
	private Projectile _projectile; 					/// <summary>Projectile's Component.</summary>
	private SelfMotionPerformer _selfMotionPerformer; 	/// <summary>SelfMotionPerformer's Component.</summary>
	private EventsHandler _eventsHandler; 				/// <summary>EventsHandler's Component.</summary>

	/// <summary>Gets projectile Component.</summary>
	public Projectile projectile
	{ 
		get
		{
			if(_projectile == null) _projectile = GetComponent<Projectile>();
			return _projectile;
		}
	}

	/// <summary>Gets selfMotionPerformer Component.</summary>
	public SelfMotionPerformer selfMotionPerformer
	{ 
		get
		{
			if(_selfMotionPerformer == null) _selfMotionPerformer = GetComponent<SelfMotionPerformer>();
			return _selfMotionPerformer;
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

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		selfMotionPerformer.Reset();
	}
}
}