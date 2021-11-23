using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(LineRenderer))]
public class MinionEnemy : Enemy
{
	[SerializeField] private Vector3 _muzzlePoint; 				/// <summary>Muzzle's Point.</summary>
	[SerializeField] private ProjectileType _projectileType; 	/// <summary>Projectile's Type to shoot.</summary>
	[SerializeField] private CollectionIndex _projectileIndex; 	/// <summary>Projectile's Index.</summary>
	[SerializeField] private float _projectionTime; 			/// <summary>Parabola Projectile's Projection Time.</summary>
	[SerializeField] private FOVSight2D _FOVSight; 				/// <summary>FOVSight2D's Component.</summary>
/*#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 				/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 				/// <summary>Gizmos' Radius.</summary>
#endif*/
	private LineRenderer _laserRenderer; 						/// <summary>Laser's LineRenderer.</summary>
	protected Coroutine stateCoroutine; 						/// <summary>State Coroutine.</summary>

	/// <summary>Gets and Sets muzzlePoint property.</summary>
	public Vector3 muzzlePoint
	{
		get { return transform.position + (transform.rotation * _muzzlePoint); }
		set { _muzzlePoint = value; }
	}

	/// <summary>Gets and Sets projectileType property.</summary>
	public ProjectileType projectileType
	{
		get { return _projectileType; }
		set { _projectileType = value; }
	}

	/// <summary>Gets and Sets projectileIndex property.</summary>
	public CollectionIndex projectileIndex
	{
		get { return _projectileIndex; }
		set { _projectileIndex = value; }
	}

	/// <summary>Gets and Sets projectionTime property.</summary>
	public float projectionTime
	{
		get { return _projectionTime; }
		set { _projectionTime = value; }
	}

	/// <summary>Gets FOVSight Component.</summary>
	public FOVSight2D FOVSight
	{ 
		get
		{
			if(_FOVSight == null) _FOVSight = GetComponent<FOVSight2D>();
			return _FOVSight;
		}
	}

	/// <summary>Gets laserRenderer Component.</summary>
	public LineRenderer laserRenderer
	{ 
		get
		{
			if(_laserRenderer == null) _laserRenderer = GetComponent<LineRenderer>();
			return _laserRenderer;
		}
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when MinionEnemy's instance is selected.</summary>
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(muzzlePoint, gizmosRadius);
	}

	/// <summary>Resets MinionEnemy's instance to its default values.</summary>
	protected virtual void Reset()
	{
		gizmosColor = Color.white;
		gizmosRadius = 0.25f;
	}
#endif

	/// <summary>MinionEnemy's instance initialization when loaded [Before scene loads].</summary>
	protected virtual void Awake()
	{
		base.Awake();

		FOVSight.onSightEvent += OnSightEvent;
	}

	/// <summary>Callback invoked when MinionEnemy's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		base.OnDestroy();

		FOVSight.onSightEvent -= OnSightEvent;
	}

	/// <summary>Callback invoked when this FOV Sight leaves another collider.</summary>
	/// <param name="_collider">Collider sighted.</param>
	/// <param name="_eventType">Type of interaction.</param>
	protected virtual void OnSightEvent(Collider2D _collider, HitColliderEventTypes _eventType)
	{
		GameObject obj = _collider.gameObject;

		if(obj.CompareTag(Game.data.playerTag))
		{
			switch(_eventType)
			{
				case HitColliderEventTypes.Enter:
				this.AddStates(ID_STATE_PLAYERONSIGHT);
				break;

				case HitColliderEventTypes.Exit:
				this.RemoveStates(ID_STATE_PLAYERONSIGHT);
				break;
			}
		}
	}

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public override void OnStatesAdded(int _state)
	{
		if((_state | ID_STATE_PLAYERONSIGHT) == _state)
		{
			this.StartCoroutine(AttackCoroutine(), ref stateCoroutine);
		}
	}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public override void OnStatesRemoved(int _state)
	{
		if((_state | ID_STATE_PLAYERONSIGHT) == _state)
		{
			this.DispatchCoroutine(ref stateCoroutine);
		}
	}

	/// <summary>Attack's Coroutine.</summary>
	protected   IEnumerator AttackCoroutine()
	{
		while(true)
		{
			SecondsDelayWait wait = new SecondsDelayWait(0.0f);
			Vector3 projectedMateoPosition = Game.ProjectMateoPosition(1.0f);
			Vector3 direction = projectedMateoPosition - muzzlePoint;
			Projectile projectile = null;

			switch(projectileType)
			{
				case ProjectileType.Normal:
				projectile = PoolManager.RequestProjectile(Faction.Enemy, projectileIndex, muzzlePoint, direction);
				projectile.rigidbody.MoveRotation(VQuaternion.RightLookRotation(direction));
				break;

				case ProjectileType.Homing:
				projectile = PoolManager.RequestHomingProjectile(Faction.Enemy, projectileIndex, muzzlePoint, direction, Game.mateo.transform);
				break;

				case ProjectileType.Parabola:
				projectile = PoolManager.RequestParabolaProjectile(Faction.Enemy, projectileIndex, muzzlePoint, projectedMateoPosition, projectionTime);
				break;
			}

			wait.ChangeDurationAndReset(projectile.cooldownDuration > 0.0f ? projectile.cooldownDuration : 1.0f);

			while(projectile.active) yield return null;
			while(wait.MoveNext()) yield return null;
		}
	}
}
}