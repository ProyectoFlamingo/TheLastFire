using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum ProjectileType
{
	Normal,
	Parabola,
	Homing
}

public enum SpeedMode
{
	Lineal,
	Accelerating
}



public enum RepelReaction
{
	InvertDirection,
	TowardsPreviousShooter
}

/*
Events:
 - Deactivated
 - Inverted
 - Impacted
 - etc.
*/

[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(ProjectileEventsHandler))]
public class Projectile : ContactWeapon
{
	public event OnDeactivated onDeactivated; 									/// <summary>OnDeactivated's Event Delegate.</summary>

	[SerializeField] private ParticleEffect effect; 	/// <summary>Description.</summary>
	[Space(5f)]
	[Header("Projectile's Attributes:")]
	[SerializeField] private ProjectileType _projectileType; 					/// <summary>Projectile's Type.</summary>
	[SerializeField] private GameObjectTag[] _repelTags; 						/// <summary>Tags of GameObjects that can repel this projectile on impact.</summary>
	[SerializeField] private SpeedMode _speedMode; 								/// <summary>Speed's Mode.</summary>
	[SerializeField] private float _speed; 										/// <summary>Projectile's Speed.</summary>
	[SerializeField] private float _lifespan; 									/// <summary>Projectile's Lifespan.</summary>
	[SerializeField] private float _cooldownDuration; 							/// <summary>Cooldown's Duration.</summary>
	[SerializeField] private bool _rotateTowardsDirection; 						/// <summary>Make Projectile Rotate towards direction?.</summary>
	[Space(5f)]
	[Header("Steering Attributes:")]
	[SerializeField] private float _maxSteeringForce; 							/// <summary>Maximum's Steering Force.</summary>
	[SerializeField] private float _distance; 									/// <summary>Distance from Parent's Projectile.</summary>
	[Space(5f)]
	[Header("Particle Effects' Attributes:")]
	[SerializeField] private int _impactParticleEffectIndex; 					/// <summary>Index of ParticleEffect to emit when the projectile impacts.</summary>
	[SerializeField] private int _destroyedParticleEffectIndex; 				/// <summary>Index of ParticleEffect to emit when the projectile is destroyed.</summary>
	[Space(5f)]
	[Header("Sound Effects' Attributes:")]
	[SerializeField] private int _sourceIndex; 									/// <summary>Sound Effect's Source Index.</summary>
	[SerializeField] private int _impactSoundEffectIndex; 						/// <summary>Index of Sound Effect to emit when the projectile impacts.</summary>
	[SerializeField] private int _destroyedSoundEffectIndex; 					/// <summary>Index of Sound Effect to emit when the projectile is destroyed.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private Color gizmosColor; 								/// <summary>Gizmos' Color.</summary>
#endif
	
	private float _currentLifeTime; 											/// <summary>Current Life Time.</summary>
	private float _parabolaTime; 												/// <summary>Time parameter used for the projectile parabola's formula.</summary>
	private Vector3 _lastPosition; 												/// <summary>Last Position reference [for the Steering Snake].</summary>
	private Vector3 _direction; 												/// <summary>Projectilwe's direction that determines its displacement.</summary>
	private Vector3 _accumulatedVelocity; 										/// <summary>Accumulated Velocity.</summary>
	private Transform _target; 													/// <summary>Homing Target.</summary>
	private Rigidbody2D _rigidbody; 											/// <summary>Rigidbody2D's Component.</summary>
	private ProjectileEventsHandler _projectileEventsHandler; 					/// <summary>ProjectileEventsHandler's Component.</summary>
	private Projectile _parentProjectile; 										/// <summary>Parent Projectile.</summary>
	protected Vector2 velocity; 												/// <summary>Velocity's Vector.</summary>

	//Gil hash set
	private HashSet<int> _swordSet = new HashSet<int>();
#region Getters/Setters:
	/// <summary>Gets and Sets projectileType property.</summary>
	public ProjectileType projectileType
	{
		get { return _projectileType; }
		set { _projectileType = value; }
	}

	/// <summary>Gets and Sets repelTags property.</summary>
	public GameObjectTag[] repelTags
	{
		get { return _repelTags; }
		set { _repelTags = value; }
	}

	/// <summary>Gets and Sets speedMode property.</summary>
	public SpeedMode speedMode
	{
		get { return _speedMode; }
		set { _speedMode = value; }
	}

	/// <summary>Gets and Sets parentProjectile property.</summary>
	public Projectile parentProjectile
	{
		get { return _parentProjectile; }
		set { _parentProjectile = value; }
	}

	/// <summary>Gets and Sets speed property.</summary>
	public float speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	/// <summary>Gets and Sets lifespan property.</summary>
	public float lifespan
	{
		get { return _lifespan; }
		set { _lifespan = value; }
	}

	/// <summary>Gets and Sets cooldownDuration property.</summary>
	public float cooldownDuration
	{
		get { return _cooldownDuration; }
		set { _cooldownDuration = value; }
	}

	/// <summary>Gets and Sets maxSteeringForce property.</summary>
	public float maxSteeringForce
	{
		get { return _maxSteeringForce; }
		set { _maxSteeringForce = value; }
	}

	/// <summary>Gets and Sets distance property.</summary>
	public float distance
	{
		get { return _distance; }
		set { _distance = value; }
	}

	/// <summary>Gets and Sets currentLifeTime property.</summary>
	public float currentLifeTime
	{
		get { return _currentLifeTime; }
		set { _currentLifeTime = value; }
	}

	/// <summary>Gets and Sets parabolaTime property.</summary>
	public float parabolaTime
	{
		get { return _parabolaTime; }
		set { _parabolaTime = value; }
	}

	/// <summary>Gets and Sets lastPosition property.</summary>
	public Vector3 lastPosition
	{
		get { return _lastPosition; }
		set { _lastPosition = value; }
	}

	/// <summary>Gets and Sets direction property.</summary>
	public Vector3 direction
	{
		get { return _direction; }
		set
		{
			_direction = value.normalized;
			//if(rotateTowardsDirection) transform.rotation = VQuaternion.RightLookRotation(_direction);
		}
	}

	/// <summary>Gets and Sets accumulatedVelocity property.</summary>
	public Vector3 accumulatedVelocity
	{
		get { return _accumulatedVelocity; }
		set { _accumulatedVelocity = value; }
	}

	/// <summary>Gets and Sets target property.</summary>
	public Transform target
	{
		get { return _target; }
		set { _target = value; }
	}

	/// <summary>Gets and Sets rotateTowardsDirection property.</summary>
	public bool rotateTowardsDirection
	{
		get { return _rotateTowardsDirection; }
		set { _rotateTowardsDirection = value; }
	}

	/// <summary>Gets and Sets sourceIndex property.</summary>
	public int sourceIndex
	{
		get { return _sourceIndex; }
		set { _sourceIndex = value; }
	}

	/// <summary>Gets and Sets impactParticleEffectIndex property.</summary>
	public int impactParticleEffectIndex
	{
		get { return _impactParticleEffectIndex; }
		set { _impactParticleEffectIndex = value; }
	}

	/// <summary>Gets and Sets destroyedParticleEffectIndex property.</summary>
	public int destroyedParticleEffectIndex
	{
		get { return _destroyedParticleEffectIndex; }
		set { _destroyedParticleEffectIndex = value; }
	}

	/// <summary>Gets and Sets impactSoundEffectIndex property.</summary>
	public int impactSoundEffectIndex
	{
		get { return _impactSoundEffectIndex; }
		set { _impactSoundEffectIndex = value; }
	}

	/// <summary>Gets and Sets destroyedSoundEffectIndex property.</summary>
	public int destroyedSoundEffectIndex
	{
		get { return _destroyedSoundEffectIndex; }
		set { _destroyedSoundEffectIndex = value; }
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody2D rigidbody
	{
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
			return _rigidbody;
		}
	}

	/// <summary>Gets projectileEventsHandler Component.</summary>
	public ProjectileEventsHandler projectileEventsHandler
	{ 
		get
		{
			if(_projectileEventsHandler == null) _projectileEventsHandler = GetComponent<ProjectileEventsHandler>();
			return _projectileEventsHandler;
		}
	}

	/// <summary>Gets and Sets sword hashset property.</summary>
	public HashSet<int> swordSet
	{
		get { return _swordSet; }
		set { _swordSet = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;

		if(parentProjectile != null) Gizmos.DrawWireSphere(transform.position, distance);
		if(projectileType == ProjectileType.Homing) Gizmos.DrawRay(transform.position, velocity);
	}
#endif

	/// <summary>Callback invoked when Projectile's instance is disabled.</summary>
	private void OnDisable()
	{
	}

	/// <summary>Resets Projectile's instance to its default values.</summary>
	protected virtual void Reset()
	{
#if UNITY_EDITOR
		gizmosColor = Color.white;
#endif
	}

	/// <summary>Updates Projectile's instance at each frame.</summary>
	protected virtual void Update()
	{
		TickLifespan();
	}

	/// <summary>Callback called each Physics' Time Step.</summary>
	protected virtual void FixedUpdate()
	{
		if(!activated) return;

		Vector3 displacement = CalculateDisplacement();

		rigidbody.MoveIn3D(displacement);
		if(rotateTowardsDirection) rigidbody.MoveRotation(VQuaternion.RightLookRotation(displacement));

		//if(projectileType  == ProjectileType.Homing) HomingProjectileUpdate();
		
	}

#region Callbacks:
	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public override void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		GameObject obj = _info.collider.gameObject;

#region OutOfBoundsShiat:
		int layerMask = 1 << obj.layer;
		int outOfBoundsMask = Game.data.outOfBoundsLayer.ToLayerMask();
		
		/// \TODO Make an Out of Bounds Module...
		if((outOfBoundsMask | layerMask) == outOfBoundsMask)
		eventsHandler.InvokeContactWeaponDeactivationEvent(DeactivationCause.LeftBoundaries);
#endregion

		base.OnTriggerEvent(_info, _eventType, _ID);

		GameObject newOwner = null;
		int ID  = obj.GetInstanceID();


		//Gil testing attempt
		switch(_eventType)
		{
			case HitColliderEventTypes.Enter:
			if(swordSet.Contains(ID))
			{
				return;
			}else{
				//Evaluate for repelment
				if(repelTags != null) foreach(GameObjectTag tag in repelTags)
				{
					if(obj.CompareTag(tag))
					{
						RequestRepel(obj, _info);
						swordSet.Add(ID);
						return;

					}
				}
			}
			break;


			case HitColliderEventTypes.Exit :

			swordSet.Remove(ID);
			break;

		}

		/// Evaluate for repelment:
		/*if(repelTags != null) foreach(GameObjectTag tag in repelTags)
		{
			if(obj.CompareTag(tag))
			{
				RequestRepel(obj, _info);
				return;
			}
		}*/
	}

	/// <summary>Callback internally called when there was an impact.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_ID">ID of the HitCollider2D.</param>
	protected override void OnImpact(Trigger2DInformation _info, int _ID = 0)
	{
		InvokeDeactivationEvent(DeactivationCause.Impacted, _info);
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		ActivateHitBoxes();
		accumulatedVelocity = Vector3.zero;
		activated = true;
		currentLifeTime = 0.0f;
		lastPosition = transform.position;
		velocity = Vector2.zero;
		target = null;
		if(effect != null) effect.Play();
		if(trailRenderers != null) foreach(TrailRenderer trailRenderer in trailRenderers)
		{
			for(int i = 0; i < trailRenderer.positionCount; i++)
			{
				trailRenderer.SetPosition(i, transform.position);
			}
		}
	}
#endregion

	/// <summary>Request the projectile ro be repelled [without physical interaction].</summary>
	/// <param name="_requester">Requester and potential new owner.</param>
	/// <param name="_info">Additional Trigger2D's Information.</param>
	public void RequestRepel(GameObject _requester, Trigger2DInformation _info = default(Trigger2DInformation))
	{
		ContactWeapon weapon = _requester.GetComponentInParent<ContactWeapon>();
		GameObject newOwner = weapon != null  && weapon.owner != null ? weapon.owner : _requester;

		if(newOwner == null || newOwner == owner) return;

		switch(projectileType)
		{
			case ProjectileType.Normal:
			float inversion = -1.0f;

			direction *= inversion;
			accumulatedVelocity *= inversion;

			break;

			case ProjectileType.Parabola:
			accumulatedVelocity = Vector3.zero;

			if(owner == null)
			{
				direction *= -1.0f;
			
			} else if(newOwner != owner)
			{
				Vector3 velocity = VPhysics.ProjectileDesiredVelocity(parabolaTime, transform.position, owner.transform.position, Physics.gravity);
				float magnitude = velocity.magnitude;

				velocity /= magnitude; // Normalize
				direction = velocity;
				speed = magnitude;
			}
			break;

			case ProjectileType.Homing:
			if(owner == null) return;

			target = owner.transform;
			break;
		}

		owner = newOwner;
		eventsHandler.InvokeContactWeaponIDEvent(IDs.EVENT_REPELLED, _info);

		/// \TODO Deprecate this call (and delete ProjectileEventsHandler's Component)
		projectileEventsHandler.InvokeProjectileEvent(this, IDs.EVENT_REPELLED);
	}

	/// <returns>Projectile's Position.</returns>
	public Vector2 GetPosition()
	{
		return rigidbody.position;
	}

	/// <summary>Update  Method for Homing Projectile.</summary>
	protected virtual void HomingProjectileUpdate()
	{
		if(parentProjectile != null)
		{
			Vector3 direction = transform.position - parentProjectile.transform.position;

			if(direction.sqrMagnitude < (distance * distance))
			rigidbody.MoveIn3D(Vector3.Lerp(direction, direction.normalized * distance, 0.9f));
		}
	}

	/// <summary>Ticks Lifespan.</summary>
	protected virtual void TickLifespan()
	{
		if(!activated || lifespan <= 0.0f) return;

		if(currentLifeTime < lifespan)
		{
			currentLifeTime += Time.deltaTime;

		} else InvokeDeactivationEvent(DeactivationCause.LifespanOver);
	}

	/// <returns>Displacement acoording to the Projectile's Type.</returns>
	protected virtual Vector3 CalculateDisplacement()
	{
		Vector3 displacement = Vector3.zero;

		switch(projectileType)
		{
			case ProjectileType.Normal:
				switch(speedMode)
				{
					case SpeedMode.Lineal:
					displacement = (direction * speed);
					break;

					case SpeedMode.Accelerating:
					accumulatedVelocity += (direction * speed * Time.fixedDeltaTime);
					displacement = accumulatedVelocity;
					break;
				}
			break;

			case ProjectileType.Parabola:
			{
				switch(speedMode)
				{
					case SpeedMode.Lineal:
					accumulatedVelocity = Physics.gravity;
					break;

					case SpeedMode.Accelerating:
					accumulatedVelocity += (Physics.gravity * Time.fixedDeltaTime);
					break;
				}

				displacement = ((direction * speed) + accumulatedVelocity);
			}
			break;

			case ProjectileType.Homing:
			{
				Vector3 targetPosition = Vector3.zero;

				if(parentProjectile != null)
				{
					Vector3 parentPosition = parentProjectile.transform.position;
					Vector3 direction = transform.position - parentPosition;

					if(direction.sqrMagnitude < (distance * distance))
					{
						direction = direction.normalized * distance;
						targetPosition = parentPosition + direction;
					}
					else targetPosition = parentPosition;
				
				} else if(target != null)
				{
					targetPosition = target.position;
				
				} else return Vector3.zero; 

				Vector3 steeringForce = SteeringVehicle2D.GetSeekForce(rigidbody.position, targetPosition, ref velocity, speed, maxSteeringForce);

				switch(speedMode)
				{
					case SpeedMode.Lineal:
					displacement = steeringForce;
					break;

					case SpeedMode.Accelerating:
					accumulatedVelocity += (steeringForce * Time.fixedDeltaTime);
					displacement = accumulatedVelocity;
					break;
				}
			}
			break;
		}

		return displacement * Time.fixedDeltaTime;
	}

	/// <summary>Invokes OnDeactivation's Event and Deactivates itself [so it can be a free Pool Object resource].</summary>
	/// <param name="_cause">Cause of the Deactivation.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public virtual void InvokeDeactivationEvent(DeactivationCause _cause, Trigger2DInformation _info = default(Trigger2DInformation))
	{
		switch(_cause)
		{
			case DeactivationCause.Impacted:
			PoolManager.RequestParticleEffect(impactParticleEffectIndex, transform.position, Quaternion.identity);
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, impactSoundEffectIndex);
			break;

			case DeactivationCause.Destroyed:
			PoolManager.RequestParticleEffect(destroyedParticleEffectIndex, transform.position, Quaternion.identity);
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, destroyedSoundEffectIndex);
			break;
		}

		//projectileEventsHandler.InvokeProjectileDeactivationEvent(this, _cause, _info);
		eventsHandler.InvokeContactWeaponDeactivationEvent(_cause, _info);
		OnObjectDeactivation();
	}
}
}