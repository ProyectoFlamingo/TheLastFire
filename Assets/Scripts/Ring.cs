using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Event invoked when a Collider passes a ring.</summary>
/// <param name="_collider">Collider that passed the ring.</param>
public delegate void OnRingPassed(Collider2D _collider);

[RequireComponent(typeof(VCameraTarget))]
[RequireComponent(typeof(SelfMotionPerformer))]
public class Ring : PoolGameObject
{
	public event OnRingPassed onRingPassed; 									/// <summary>OnRingPassed event's delegate.</summary>

	[SerializeField] private HitCollider2D _triggerA; 							/// <summary>Trigger A.</summary>
	[SerializeField] private HitCollider2D _triggerB; 							/// <summary>Trigger B.</summary>
	[Space(5f)]
	[SerializeField] private ParticleEffectEmissionData _passedParticleEffect; 	/// <summary>Particle Effect Emission Data when the ring is passed.</summary>
	[SerializeField] private int _particleEffectIndex; 							/// <summary>ParticleEffect index to emit when ring is passed on?.</summary>
	[SerializeField] private int _soundEffectIndex; 							/// <summary>Sound Effect index to emit when ring is passed on.</summary>
	[Space(5f)]
	[SerializeField] private bool _deactivateWhenPassedOn; 						/// <summary>Deactivate ring when passed on it?.</summary>
	[SerializeField] private GameObjectTag[] _detectableTags; 					/// <summary>Tags of GameObjects that are detectable by the ring.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _dotProductTolerance; 					/// <summary>Minimium Dot Product's Tolerance value to be considered passed.</summary>
	[SerializeField] private Renderer _renderer; 								/// <summary>Ring Mesh's Renderer.</summary>
	[SerializeField] private HitCollider2D[] _hitBoxes; 						/// <summary>Ring's HitBoxes.</summary>
	[Space(5f)]
	[Header("VERY SPECIAL CASE:")]
	[SerializeField] private float _speedThreshold; 							/// <summary>Speed Threshold for TransformDeltaCalculators passing on Ring.</summary>
	private Dictionary<int, Vector2> _directionsMapping; 						/// <summary>Direction's Mapping for each possible GameObject.</summary>
	private HashSet<int> _passedOnMapping; 										/// <summary>Mapping of GameObjects that have passed through this Ring.</summary>
	private HashSet<int> _passedOnTriggerA; 									/// <summary>GaameObjects that have passed on Trigger A.</summary>
	private HashSet<int> _passedOnTriggerB; 									/// <summary>GaameObjects that have passed on Trigger B.</summary>
	private VCameraTarget _cameraTarget; 										/// <summary>VCamera's Component.</summary>
	private SelfMotionPerformer _selfMotionPerformer; 							/// <summary>SelfMotionPerformer's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets triggerA property.</summary>
	public HitCollider2D triggerA { get { return _triggerA; } }

	/// <summary>Gets triggerB property.</summary>
	public HitCollider2D triggerB { get { return _triggerB; } }

	/// <summary>Gets passedParticleEffect property.</summary>
	public ParticleEffectEmissionData passedParticleEffect { get { return _passedParticleEffect; } }

	/// <summary>Gets particleEffectIndex property.</summary>
	public int particleEffectIndex { get { return _particleEffectIndex; } }

	/// <summary>Gets soundEffectIndex property.</summary>
	public int soundEffectIndex { get { return _soundEffectIndex; } }

	/// <summary>Gets and Sets deactivateWhenPassedOn property.</summary>
	public bool deactivateWhenPassedOn
	{
		get { return _deactivateWhenPassedOn; }
		set { _deactivateWhenPassedOn = value; }
	}

	/// <summary>Gets and Sets detectableTags property.</summary>
	public GameObjectTag[] detectableTags
	{
		get { return _detectableTags; }
		set { _detectableTags = value; }
	}

	/// <summary>Gets and Sets dotProductTolerance property.</summary>
	public float dotProductTolerance
	{
		get { return _dotProductTolerance; }
		set { _dotProductTolerance = value; }
	}

	/// <summary>Gets and Sets speedThreshold property.</summary>
	public float speedThreshold
	{
		get { return _speedThreshold; }
		set { _speedThreshold = value; }
	}

	/// <summary>Gets renderer property.</summary>
	public Renderer renderer { get { return _renderer; } }

	/// <summary>Gets hitBoxes property.</summary>
	public HitCollider2D[] hitBoxes { get { return _hitBoxes; } }

	/// <summary>Gets and Sets directionsMapping property.</summary>
	public Dictionary<int, Vector2> directionsMapping
	{
		get { return _directionsMapping; }
		private set { _directionsMapping = value; }
	}

	/// <summary>Gets and Sets passedOnMapping property.</summary>
	public HashSet<int> passedOnMapping
	{
		get { return _passedOnMapping; }
		private set { _passedOnMapping = value; }
	}

	/// <summary>Gets and Sets passedOnTriggerA property.</summary>
	public HashSet<int> passedOnTriggerA
	{
		get { return _passedOnTriggerA; }
		private set { _passedOnTriggerA = value; }
	}

	/// <summary>Gets and Sets passedOnTriggerB property.</summary>
	public HashSet<int> passedOnTriggerB
	{
		get { return _passedOnTriggerB; }
		private set { _passedOnTriggerB = value; }
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

	/// <summary>Gets selfMotionPerformer Component.</summary>
	public SelfMotionPerformer selfMotionPerformer
	{ 
		get
		{
			if(_selfMotionPerformer == null) _selfMotionPerformer = GetComponent<SelfMotionPerformer>();
			return _selfMotionPerformer;
		}
	}
#endregion

	/// <summary>Draws Gizmos on Editor mode when Ring's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, (transform.rotation) * Vector3.right);
	}

	/// <summary>Resets Ring's instance to its default values.</summary>
	public void Reset()
	{
		if(passedOnMapping != null) passedOnMapping.Clear();
	}

	/// <summary>Ring's instance initialization.</summary>
	private void Awake()
	{
		directionsMapping = new Dictionary<int, Vector2>();
		passedOnMapping = new HashSet<int>();
		passedOnTriggerA = new HashSet<int>();
		passedOnTriggerB = new HashSet<int>();

		/// Ye olde dot-product way:
		if(hitBoxes != null)
		{
			int i = 0;
			foreach(HitCollider2D hitBox in hitBoxes)
			{
				hitBox.detectableHitEvents = HitColliderEventTypes.Enter | HitColliderEventTypes.Exit;
				hitBox.onTriggerEvent2D += OnTriggerEvent2D;
				hitBox.ID = i;
				i++;
			}
		}

		/// The gay way:
		/*if(triggerA != null && triggerB != null)
		{
			HitColliderEventTypes detectableEvents = HitColliderEventTypes.Enter | HitColliderEventTypes.Exit;
			
			triggerA.detectableHitEvents = detectableEvents;
			triggerB.detectableHitEvents = detectableEvents;
			triggerA.onHitColliderTriggerEvent += OnHitColliderTriggerEvent;
			triggerB.onHitColliderTriggerEvent += OnHitColliderTriggerEvent;
		}*/
	}

	/// <summary>Callback invoked when Ring's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		if(hitBoxes != null) foreach(HitCollider2D hitBox in hitBoxes)
		{
			hitBox.onTriggerEvent2D -= OnTriggerEvent2D;
		}
	}

	/// <summary>Event invoked when this Hit Collider2D intersects with another GameObject.</summary>
	/// <param name="_hitCollider">HitCollider2D that invoked the event.</param>
	/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
	/// <param name="_eventType">Type of the event.</param>
	public void OnHitColliderTriggerEvent(HitCollider2D _hitCollider, Collider2D _collider, HitColliderEventTypes _eventType)
	{
		if(_eventType == HitColliderEventTypes.Stays && detectableTags == null) return;

		GameObject obj = _collider.gameObject;
		int instanceID = obj.GetInstanceID();
		bool detectable = false;

		foreach(GameObjectTag tag in detectableTags)
		{
			if(obj.CompareTag(tag))
			{
				detectable = true;
				break;
			}
		}

		if(!detectable) return;

		switch(_eventType)
		{
			case HitColliderEventTypes.Enter:
			if(_hitCollider == triggerA)
			{
				passedOnTriggerA.Add(instanceID);

			} else if(_hitCollider == triggerB)
			{
				passedOnTriggerB.Add(instanceID);
			}
			break;

			case HitColliderEventTypes.Exit:
			if(passedOnTriggerA.Contains(instanceID) && passedOnTriggerB.Contains(instanceID))
			{
				passedOnTriggerA.Remove(instanceID);
				passedOnTriggerB.Remove(instanceID);
				InvokeRingPassedEvent(_collider);
			}
			else
			{
				if(_hitCollider == triggerA)
				{
					passedOnTriggerA.Remove(instanceID);

				} else if(_hitCollider == triggerB)
				{
					passedOnTriggerB.Remove(instanceID);
				}
			}
			break;
		}
	}

	/// \TODO REPAIR IT (But it works for the sake of quick example...)
	/// <summary>Callback invoked when this Hit Collider2D intersects with another GameObject.</summary>
	/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_hitColliderID">Optional ID of the HitCollider2D.</param>
	public void OnTriggerEvent2D(Collider2D _collider, HitColliderEventTypes _eventType, int _hitColliderID = 0)
	{
		if(_eventType == HitColliderEventTypes.Stays || detectableTags == null) return;

		GameObject obj = _collider.gameObject;
		int instanceID = obj.GetInstanceID();
		HitCollider2D hitBox = hitBoxes[_hitColliderID];
		bool detectable = false;

		foreach(GameObjectTag tag in detectableTags)
		{
			if(obj.CompareTag(tag))
			{
				detectable = true;
				break;
			}
		}

		if(!detectable) return;

		Vector2 direction = hitBox.transform.position - obj.transform.position;
		Quaternion rotation = hitBox.transform.rotation;
		direction.Normalize();
		direction = ToRelativeOrientationVector(rotation, direction);

		switch(_eventType)
		{
			case HitColliderEventTypes.Enter:
				if(directionsMapping.ContainsKey(instanceID)) return;		
				
				directionsMapping.Add(instanceID, direction);
				passedOnMapping.Remove(instanceID);

				Debug.DrawRay(transform.position, direction.normalized * 5.0f, Color.cyan, 10.0f);
			break;

			case HitColliderEventTypes.Exit:
				if(!directionsMapping.ContainsKey(instanceID)) return;

				float dot = Vector2.Dot(direction, directionsMapping[instanceID]);

				if(dot <= (-1.0f + dotProductTolerance) && !passedOnMapping.Contains(instanceID))
				{
					passedOnMapping.Add(instanceID);
					InvokeRingPassedEvent(_collider);

#region TEMPORAL_PATCH
				} else if(!passedOnMapping.Contains(instanceID))
				{
					TransformDeltaCalculator deltaCalculator = obj.GetComponentInParent<TransformDeltaCalculator>();

					if(deltaCalculator != null)
					{
						/// Convert velocity vector from world to local space.
						Vector2 i = Quaternion.Inverse(rotation) * deltaCalculator.velocity;
						float x = i.x;
						bool differentOrientation  = Mathf.Sign(x) != Mathf.Sign(direction.x);

						if(differentOrientation && Mathf.Abs(x) >= speedThreshold)
						{
							passedOnMapping.Add(instanceID);
							InvokeRingPassedEvent(_collider);			
						}
						else Debug.Log("[Ring] Sign Different: " + differentOrientation + ". TransformDeltaCalculator's Speed: " + Mathf.Abs(x));
					}
				}
#endregion

				directionsMapping.Remove(instanceID);

				Debug.DrawRay(transform.position, direction.normalized * 5.0f, Color.yellow, 10.0f);
			break;
		}
	}

	/// <summary>Transforms local space vector into world space.</summary>
	/// <param name="v">Vector to convert.</param>
	/// <returns>Converted Vector.</returns>
	private Vector2 ToRelativeOrientationVector(Quaternion r, Vector2 v)
	{
		Vector2 i = Quaternion.Inverse(r) * v;

		Debug.DrawRay(transform.position, i * 5.0f, Color.red, 10.0f);

		return r * (i.x < 0.0f ? Vector2.left : Vector2.right);
	}

	/// <summary>Invokes onRingPassed's Event.</summary>
	/// <param name="_collider">Collider that passed the ring.</param>
	private void InvokeRingPassedEvent(Collider2D _collider)
	{
		//PoolManager.RequestParticleEffect(particleEffectIndex, transform.position, Quaternion.identity);
		passedParticleEffect.EmitParticleEffects();
		AudioController.PlayOneShot(SourceType.SFX, 0, soundEffectIndex);
		Reset();

		if(onRingPassed != null) onRingPassed(_collider);
		if(deactivateWhenPassedOn) OnObjectDeactivation();
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		if(directionsMapping != null) directionsMapping.Clear();
		if(passedOnMapping != null) passedOnMapping.Clear();
		selfMotionPerformer.Reset();
		Reset();
	}
}
}