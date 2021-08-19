﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/*
	/// Declarations:
	PoolGameObject[] targets;
	CollectionIndex targetIndex;

	///When spawning:
	PoolGameObject target = PoolManager.RequestPoolGameObject(targetIndex, position, rotation);
	target.onPoolObjectDeactivation += OnTargetDestroyed;

	/// Callback when target is destroyed:
	void OnTargetDestroyed(IPoolObject poolObject)
	{
		poolObject.onPoolObjectDeactivation -= OnTargetDestroyed;
		score++;
	}
*/

[RequireComponent(typeof(ImpactEventHandler))]
public class DestroyOnImpact : PoolGameObject
{
	[SerializeField] private GameObjectTag[] _impactTags; 	/// <summary>Impacts' Tags.</summary>
	private ImpactEventHandler _impactHandler; 				/// <summary>ImpactEventHandler's Component.</summary>

	/// <summary>Gets and Sets impactTags property.</summary>
	public GameObjectTag[] impactTags
	{
		get { return _impactTags; }
		set { _impactTags = value; }
	}

	/// <summary>Gets impactHandler Component.</summary>
	public ImpactEventHandler impactHandler
	{ 
		get
		{
			if(_impactHandler == null) _impactHandler = GetComponent<ImpactEventHandler>();
			return _impactHandler;
		}
	}

	/// <summary>DestroyOnImpact's instance initialization.</summary>
	private void Awake()
	{
		impactHandler.eventsHandler.onTriggerEvent += OnTriggerEvent;	
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		Debug.Log("[DestroyOnImpact] ONTRIGGEREVENT!");
		if(impactTags == null) return;

		GameObject obj = _info.collider.gameObject;

		foreach(GameObjectTag tag in impactTags)
		{
			if(obj.CompareTag(tag))
			{
				impactHandler.eventsHandler.InvokeDeactivationEvent(DeactivationCause.Destroyed, _info);
				OnObjectDeactivation();
				return;
			}
		}
	}
}
}