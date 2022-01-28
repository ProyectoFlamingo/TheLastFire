﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.AddressableAssets;

namespace Flamingo
{
public class PoolManager : Singleton<PoolManager>
{
	[SerializeField] private int[] _indices; 										/// <summary>Indices of Projectiles that must be loaded for this scene.</summary>
	private Dictionary<Hash128, GameObjectPool<Projectile>> _projectilesPoolsMap;
	private GameObjectPool<Projectile>[] _projectilesPools; 						/// <summary>Pools Projectiles.</summary>
	private GameObjectPool<PoolGameObject>[] _gameObjectsPools; 					/// <summary>PoolGameObjects' Pools.</summary>
	private GameObjectPool<ParticleEffect>[] _particleEffectsPools; 				/// <summary>Pools of Particle's Effects.</summary>
	private GameObjectPool<Explodable>[] _explodablesPools; 						/// <summary>Pools of Explodables.</summary>
	private GameObjectPool<SoundEffectLooper> _loopersPool; 						/// <summary>Pool of Sound-Effects' Loopers.</summary>

#region Getters/Setters:
	/// <summary>Gets indices property.</summary>
	public int[] indices { get { return _indices; } }

	/// <summary>Gets and Sets projectilesPools property.</summary>
	public GameObjectPool<Projectile>[] projectilesPools
	{
		get { return _projectilesPools; }
		private set { _projectilesPools = value; }
	}

	/// <summary>Gets and Sets gameObjectsPools property.</summary>
	public GameObjectPool<PoolGameObject>[] gameObjectsPools
	{
		get { return _gameObjectsPools; }
		private set { _gameObjectsPools = value; }
	}

	/// <summary>Gets and Sets particleEffectsPools property.</summary>
	public GameObjectPool<ParticleEffect>[] particleEffectsPools
	{
		get { return _particleEffectsPools; }
		private set { _particleEffectsPools = value; }
	}

	/// <summary>Gets and Sets explodablesPools property.</summary>
	public GameObjectPool<Explodable>[] explodablesPools
	{
		get { return _explodablesPools; }
		private set { _explodablesPools = value; }
	}

	/// <summary>Gets and Sets loopersPool property.</summary>
	public GameObjectPool<SoundEffectLooper> loopersPool
	{
		get { return _loopersPool; }
		private set { _loopersPool = value; }
	}
#endregion

	/// <summary>PoolManager's instance initialization.</summary>
	protected override void OnAwake()
	{
		projectilesPools = GameObjectPool<Projectile>.PopulatedPools(Game.data.projectiles);
		gameObjectsPools = GameObjectPool<PoolGameObject>.PopulatedPools(Game.data.poolObjects);
		particleEffectsPools = GameObjectPool<ParticleEffect>.PopulatedPools(Game.data.particleEffects);
		explodablesPools = GameObjectPool<Explodable>.PopulatedPools(Game.data.explodables);
		loopersPool = new GameObjectPool<SoundEffectLooper>(Game.data.looper);
	}

	/// <summary>Called after Awake, before the first Update.</summary>
	protected void Start()
	{
		/*Addressables.InitializeAsync().Completed += obj =>
		{
			Debug.Log("[PoolManager] Completed: " + obj.ToString());
			Game.data.InitializeProjectiles(()=>{ projectilesPools = GameObjectPool<Projectile>.PopulatedPools(Game.data.projectiles); }, indices);
		};
		

		gameObjectsPools = GameObjectPool<PoolGameObject>.PopulatedPools(Game.data.poolObjects);
		particleEffectsPools = GameObjectPool<ParticleEffect>.PopulatedPools(Game.data.particleEffects);
		explodablesPools = GameObjectPool<Explodable>.PopulatedPools(Game.data.explodables);
		loopersPool = new GameObjectPool<SoundEffectLooper>(Game.data.looper);*/
	}

	/// <summary>Gets a Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_ID">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_direction">Spawn direction for the Projectile.</param>
	/// <param name="_object">GameObject that requests the Parabola [treated as the shooter]. Null by default.</param>
	/// <returns>Requested Projectile.</returns>
	public static Projectile RequestProjectile(Faction _faction, int _ID, Vector3 _position, Vector3 _direction, GameObject _object = null)
	{
		if(_ID < 0) return null;

		GameObjectPool<Projectile> pool = Instance.projectilesPools[_ID];
		Projectile projectile = pool.Recycle(_position, Quaternion.identity);
		string tag = _faction == Faction.Ally ? Game.data.playerProjectileTag : Game.data.enemyProjectileTag;

		if(projectile == null) return null;

		projectile.rigidbody.gravityScale = 0.0f;
		projectile.rigidbody.isKinematic = true;
		projectile.rigidbody.bodyType = RigidbodyType2D.Kinematic;
		projectile.projectileType = ProjectileType.Normal;
		projectile.direction = _direction.normalized;
		projectile.gameObject.tag = tag;
		projectile.owner = _object;
		if(projectile.transform.parent != pool.poolGroup) projectile.transform.parent = null;

		foreach(HitCollider2D hitBox in projectile.impactEventHandler.hitBoxes)
		{
			hitBox.gameObject.tag = tag;
		}

		return projectile;
	}

	/// <summary>Gets a Homing Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_ID">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_direction">Spawn direction for the Projectile.</param>
	/// <param name="target">Target's Function [null by default].</param>
	/// <param name="_object">GameObject that requests the Parabola [treated as the shooter]. Null by default.</param>
	/// <returns>Requested Homing Projectile.</returns>
	public static Projectile RequestHomingProjectile(Faction _faction, int _ID, Vector3 _position, Vector3 _direction, Transform _target, GameObject _object = null)
	{
		Projectile projectile = RequestProjectile(_faction, _ID, _position, _direction, _object);

		if(projectile == null) return null;

		projectile.projectileType = ProjectileType.Homing;
		projectile.target = _target;
		
		return projectile;
	}

	/// <summary>Gets a Parabola Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_ID">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_target">Target that will determine the ParabolaProjectile's velocity.</param>
	/// <param name="t">Time it should take the projectile to reach from its position to the given target.</param>
	/// <param name="_object">GameObject that requests the Parabola [treated as the shooter]. Null by default.</param>
	/// <returns>Requested Parabola Projectile.</returns>
	public static Projectile RequestParabolaProjectile(Faction _faction, int _ID, Vector3 _position, Vector3 _target, float t, GameObject _object = null)
	{
		Projectile projectile = RequestProjectile(_faction, _ID, _position, Vector3.zero, _object);

		if(projectile == null) return null;

		Vector3 velocity = VPhysics.ProjectileDesiredVelocity(t, _position, _target, Physics.gravity, projectile.speedMode == SpeedMode.Accelerating);
		float magnitude = velocity.magnitude;

		projectile.projectileType = ProjectileType.Parabola;
		projectile.direction = velocity;
		projectile.speed = magnitude;
		projectile.parabolaTime = t;

		Debug.DrawRay(_position, velocity, Color.magenta, 3.0f);

		return projectile;
	}

	/// <summary>Gets a PoolGameObject from the PoolGameObjects' Pools.</summary>
	/// <param name="_index">GameObject's ID.</param>
	/// <param name="_position">Spawn position for the GameObject.</param>
	/// <param name="_rotation">Spawn rotation for the GameObject.</param>
	/// <returns>Requested PoolGameObject.</returns>
	public static PoolGameObject RequestPoolGameObject(int _index, Vector3 _position, Quaternion _rotation)
	{
		return _index > -1 ? Instance.gameObjectsPools[_index].Recycle(_position, _rotation) : null;
	}

	/// <summary>Gets a ParticleEffect from the ParticleEffects' Pools.</summary>
	/// <param name="_index">ParticleEffect's index on the pools.</param>
	/// <param name="_position">Spawn's Position.</param>
	/// <param name="_rotation">Spawn's Rotation.</param>
	/// <returns>Requested ParticleEffect.</returns>
	public static ParticleEffect RequestParticleEffect(int _index, Vector3 _position, Quaternion _rotation)
	{
		return _index > -1 ? Instance.particleEffectsPools[_index].Recycle(_position, _rotation) : null;
	}

	/// <summary>Gets a Explodable from the Explodables' Pools.</summary>
	/// <param name="_index">Explodable's index on the pools.</param>
	/// <param name="_position">Spawn's Position.</param>
	/// <param name="_rotation">Spawn's Rotation.</param>
	/// <param name="onExplosionEnds">Optional Callback invoked when the explosion ends.</param>
	public static Explodable RequestExplodable(int _index, Vector3 _position, Quaternion _rotation, Action onExplosionEnds = null)
	{
		if(_index < 0) return null;

		Explodable explodable = Instance.explodablesPools[_index].Recycle(_position, _rotation);
		explodable.Explode(onExplosionEnds);
		return explodable;
	}

	/// <returns>Requested Sound-Effect's Looper.</returns>
	public static SoundEffectLooper RequestSoundEffectLooper()
	{
		return Instance.loopersPool.Recycle(Vector3.zero, Quaternion.identity);
	}
}
}