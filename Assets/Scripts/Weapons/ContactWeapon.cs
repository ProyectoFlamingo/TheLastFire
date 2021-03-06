using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
[RequireComponent(typeof(VCameraTarget))]
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(ImpactEventHandler))]
[RequireComponent(typeof(VirtualAnchorContainer))]
public class ContactWeapon : PoolGameObject
{
	[SerializeField] private string _name; 														/// <summary>Weapon's Name.</summary>
	[Space(5f)]
	[SerializeField] private GameObject _meshContainer; 										/// <summary>Mesh Container.</summary>
	[Space(5f)]
	[Header("Damage's Attributes:")]
	[SerializeField] private float _damage; 													/// <summary>Damage that this ContactWeapon applies.</summary>
	[TabGroup("Tags")][SerializeField] private GameObjectTag[] _healthAffectableTags; 			/// <summary>Tags of GameObjects whose Health is affected by this ContactWeapon.</summary>
	[Space(5f)]
	[Header("Impact's Attributes:")]
	[TabGroup("Tags")][SerializeField] private GameObjectTag[] _impactTags; 					/// <summary>Tags of GameObject affected by impact.</summary>
	[Space(5f)]
	[TabGroup("Tags")][SerializeField] private GameObjectTag[] _rejectionTags; 					/// <summary>Tags of GameObjects that reject interaction.</summary>
	[Space(5f)]
	[Header("Trail's References:")]
	[TabGroup("Visual Effects")][SerializeField] private TrailRenderer[] _trailRenderers; 		/// <summary>TrailRenderers' Component.</summary>
	[TabGroup("Visual Effects")][SerializeField] private ParticleEffect[] _particleEffects; 	/// <summary>ParticleEffects' Component.</summary>
	private bool _activated; 																	/// <summary>Can the projectile be activated?.</summary>
	private GameObject _owner; 																	/// <summary>Weapon's current owner.</summary>
	private HashSet<int> _objectsIDs; 															/// <summary>Set of GameObject's IDs that are already inside of HitBoxes [to avoid repetition of actions].</summary>
	private HashSet<int> _rejectionIDs; 														/// <summary>Set of GameObjects' IDs that are already  as rejecters.</summary>
	private EventsHandler _eventsHandler; 														/// <summary>EventsHandler's Component.</summary>
	private ImpactEventHandler _impactEventHandler; 											/// <summary>ImpactEventHandler's Component.</summary>
	private VirtualAnchorContainer _anchorContainer; 											/// <summary>VirtualAnchorContainer's Component.</summary>
	private VCameraTarget _cameraTarget; 														/// <summary>VCameraTarget's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets name property.</summary>
	public string name
	{
		get { return _name; }
		set { _name = value; }
	}

	/// <summary>Gets and Sets meshContainer property.</summary>
	public GameObject meshContainer
	{
		get { return _meshContainer; }
		set { _meshContainer = value; }
	}

	/// <summary>Gets and Sets healthAffectableTags property.</summary>
	public GameObjectTag[] healthAffectableTags
	{
		get { return _healthAffectableTags; }
		set { _healthAffectableTags = value; }
	}

	/// <summary>Gets and Sets impactTags property.</summary>
	public GameObjectTag[] impactTags
	{
		get { return _impactTags; }
		set { _impactTags = value; }
	}

	/// <summary>Gets and Sets rejectionTags property.</summary>
	public GameObjectTag[] rejectionTags
	{
		get { return _rejectionTags; }
		set { _rejectionTags = value; }
	}

	/// <summary>Gets and Sets damage property.</summary>
	public float damage
	{
		get { return _damage; }
		set { _damage = value; }
	}

	/// <summary>Gets and Sets trailRenderers property.</summary>
	public TrailRenderer[] trailRenderers
	{
		get { return _trailRenderers; }
		set { _trailRenderers = value; }
	}

	/// <summary>Gets and Sets particleEffects property.</summary>
	public ParticleEffect[] particleEffects
	{
		get { return _particleEffects; }
		set { _particleEffects = value; }
	}

	/// <summary>Gets and Sets activated property.</summary>
	public bool activated
	{
		get { return _activated; }
		set { _activated = value; }
	}

	/// <summary>Gets and Sets owner property.</summary>
	public GameObject owner
	{
		get { return _owner; }
		set { _owner = value; }
	}

	/// <summary>Gets and Sets objectsIDs property.</summary>
	public HashSet<int> objectsIDs
	{
		get { return _objectsIDs; }
		set { _objectsIDs = value; }
	}

	/// <summary>Gets and Sets rejectionIDs property.</summary>
	public HashSet<int> rejectionIDs
	{
		get { return _rejectionIDs; }
		set { _rejectionIDs = value; }
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

	/// <summary>Gets impactEventHandler Component.</summary>
	public ImpactEventHandler impactEventHandler
	{ 
		get
		{
			if(_impactEventHandler == null) _impactEventHandler = GetComponent<ImpactEventHandler>();
			return _impactEventHandler;
		}
	}

	/// <summary>Gets anchorContainer Component.</summary>
	public VirtualAnchorContainer anchorContainer
	{ 
		get
		{
			if(_anchorContainer == null) _anchorContainer = GetComponent<VirtualAnchorContainer>();
			return _anchorContainer;
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
#endregion

	/// <summary>ContactWeapon's instance initialization.</summary>
	protected virtual void Awake()
	{
		objectsIDs = new HashSet<int>();
		rejectionIDs = new HashSet<int>();
		ActivateHitBoxes(false);
		eventsHandler.contactWeapon = this;
		eventsHandler.onTriggerEvent += OnTriggerEvent;
	}

	/// <summary>Callback invoked when ContactWeapon's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		eventsHandler.onTriggerEvent -= OnTriggerEvent;
	}

	/// <summary>Activates HitBoxes contained within ContactWeapon.</summary>
	/// <param name="_activate">Activate? [true by default].</param>
	public virtual void ActivateHitBoxes(bool _activate = true)
	{
		if(trailRenderers != null)
		foreach(TrailRenderer trailRenderer in trailRenderers)
		{
			trailRenderer.emitting = _activate;
		}

		if(particleEffects != null)
		foreach(ParticleEffect particleEffect in particleEffects)
		{
			particleEffect.gameObject.SetActive(_activate);
		}

		activated = _activate;

		impactEventHandler.ActivateHitBoxes(_activate);
	}

	/// <summary>Sets Owner and adds jointed Hit-Boxes.</summary>
	/// <param name="_owner">Owner.</param>
	/// <param name="_jointedHitBoxes">Jointed Hit-Boxes.</param>
	public void SetOwner(GameObject _owner, params HitCollider2D[] _jointedHitBoxes)
	{
		owner = _owner;
		if(_jointedHitBoxes != null) impactEventHandler.AddExternalHitBoxes(_jointedHitBoxes);
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public virtual void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		GameObject obj = _info.collider.gameObject;
		int instanceID = obj.GetInstanceID();

		if(rejectionTags != null) foreach(GameObjectTag tag in rejectionTags)
		{
			if(obj.CompareTag(tag))
			{
				switch(_eventType)
				{
					case HitColliderEventTypes.Enter:
						rejectionIDs.Add(instanceID);
					break;

					case HitColliderEventTypes.Exit:
						rejectionIDs.Remove(instanceID);
					break;
				}
			}
		}

		/// \TODO Maybe separate into its own DamageApplier class?
		if(healthAffectableTags != null && !rejectionIDs.Contains(instanceID))
		foreach(GameObjectTag tag in healthAffectableTags)
		{
			if(obj.CompareTag(tag))
			{
				switch(_eventType)
				{
					case HitColliderEventTypes.Enter:
					{
						if(rejectionIDs.Count > 0 || objectsIDs.Contains(instanceID)) return;
					
						objectsIDs.Add(instanceID);

						Health health = obj.GetComponentInParent<Health>();
						
						if(health == null)
						{
							HealthLinker linker = obj.GetComponent<HealthLinker>();
							if(linker != null) health = linker.component;
						}

						if(health != null)
						{
							//Debug.Log("[ContactWeapon] GameObject " + gameObject.name + " Applying damage. Position: " + transform.position + ", Target's Position: " + health.transform.position);
							health.GiveDamage(damage, true, true, gameObject);
							OnHealthInstanceDamaged(health);
						}
					}
					break;

					case HitColliderEventTypes.Exit:
						objectsIDs.Remove(instanceID);
					break;
				}

				break;
			}
		}

		switch(_eventType)
		{
			case HitColliderEventTypes.Enter:
				if(impactTags != null) foreach(GameObjectTag tag in impactTags)
				{
					if(obj.CompareTag(tag))
					{
						OnImpact(_info, _ID);
						//Debug.Log("[ContactWeapon] Impacted with: " + obj.tag);
						break;
					}
				}
			break;
		}
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		owner = null;
		objectsIDs.Clear();
		rejectionIDs.Clear();
	}

	/// <summary>Callback invoked when the object is deactivated.</summary>
	public override void OnObjectDeactivation()
	{
		base.OnObjectDeactivation();
		owner = null;
		objectsIDs.Clear();
		rejectionIDs.Clear();
	}

	/// <summary>Callback internally invoked when a Health's instance was damaged.</summary>
	/// <param name="_health">Health's instance that was damaged.</param>
	protected virtual void OnHealthInstanceDamaged(Health _health) {/*...*/}

	/// <summary>Callback internally called when there was an impact.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_ID">ID of the HitCollider2D.</param>
	protected virtual void OnImpact(Trigger2DInformation _info, int _ID = 0) { /*...*/ }
}
}