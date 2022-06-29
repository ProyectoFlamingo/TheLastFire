using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(SelfMotionPerformer))]
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(ImpactEventHandler))]
[RequireComponent(typeof(VCameraTarget))]
public class BreakableTarget : PoolGameObject
{
	[SerializeField] private GameObjectTag[] _tags; 								/// <summary>Tags of GameObjects that can break this target.</summary>
	[SerializeField] private VAssetReference _destructionParticleEffectReference; 	/// <summary>Destruction's Particle Effect's Reference.</summary>
	[SerializeField] private SoundEffectEmissionData _destructionSoundEffect; 		/// <summary>Destruction's Sound-Effect's Data.</summary>
	private SelfMotionPerformer _selfMotionPerformer; 								/// <summary>SelfMotionPerformer's Component.</summary>
	private EventsHandler _eventsHandler; 											/// <summary>EventsHandler's Component.</summary>
	private VCameraTarget _cameraTarget; 											/// <summary>VCameraTarget's Component.</summary>

	/// <summary>Gets and Sets tags property.</summary>
	public GameObjectTag[] tags
	{
		get { return _tags; }
		set { _tags = value; }
	}

	/// <summary>Gets and Sets destructionParticleEffectReference property.</summary>
	public VAssetReference destructionParticleEffectReference
	{
		get { return _destructionParticleEffectReference; }
		set { _destructionParticleEffectReference = value; }
	}

	/// <summary>Gets and Sets destructionSoundEffect property.</summary>
	public SoundEffectEmissionData destructionSoundEffect
	{
		get { return _destructionSoundEffect; }
		set { _destructionSoundEffect = value; }
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

	/// <summary>Gets cameraTarget Component.</summary>
	public VCameraTarget cameraTarget
	{ 
		get
		{
			if(_cameraTarget == null) _cameraTarget = GetComponent<VCameraTarget>();
			return _cameraTarget;
		}
	}

	/// <summary>BreakableTarget's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		eventsHandler.onTriggerEvent += OnTriggerEvent;
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		selfMotionPerformer.Reset();
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	private void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		if(_eventType != HitColliderEventTypes.Enter) return;

		GameObject obj = _info.collider.gameObject;

		if(tags != null) foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				PoolManager.RequestParticleEffect(destructionParticleEffectReference, transform.position, Quaternion.identity);
				destructionSoundEffect.Play();
				eventsHandler.InvokeDeactivationEvent(DeactivationCause.Destroyed, _info);
				OnObjectDeactivation();
				return;
			}
		}
	}
}
}