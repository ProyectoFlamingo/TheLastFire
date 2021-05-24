﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class PoolManager : Singleton<PoolManager>
{
	private GameObjectPool<Projectile>[] _playerProjectilesPools; 					/// <summary>Pool of Player's Projectiles.</summary>
	private GameObjectPool<Projectile>[] _enemyProjectilesPools; 					/// <summary>Pool of Enemy's Projectiles.</summary>
	private GameObjectPool<HomingProjectile>[] _enemyHomingProjectilesPools; 		/// <summary>Pool of Enemy's Homing Projectiles.</summary>
	private GameObjectPool<ParabolaProjectile>[] _enemyParabolaProjectilesPools; 	/// <summary>Pool of Enemy's Parabola Projectiles.</summary>
	private GameObjectPool<PoolGameObject>[] _gameObjectsPools; 					/// <summary>PoolGameObjects' Pools.</summary>
	private GameObjectPool<ParticleEffect>[] _particleEffectsPools; 				/// <summary>Pools of Particle's Effects.</summary>

	/// <summary>Gets and Sets playerProjectilesPools property.</summary>
	public GameObjectPool<Projectile>[] playerProjectilesPools
	{
		get { return _playerProjectilesPools; }
		set { _playerProjectilesPools = value; }
	}

	/// <summary>Gets and Sets enemyProjectilesPools property.</summary>
	public GameObjectPool<Projectile>[] enemyProjectilesPools
	{
		get { return _enemyProjectilesPools; }
		set { _enemyProjectilesPools = value; }
	}

	/// <summary>Gets and Sets enemyHomingProjectilesPools property.</summary>
	public GameObjectPool<HomingProjectile>[] enemyHomingProjectilesPools
	{
		get { return _enemyHomingProjectilesPools; }
		set { _enemyHomingProjectilesPools = value; }
	}

	/// <summary>Gets and Sets enemyParabolaProjectilesPools property.</summary>
	public GameObjectPool<ParabolaProjectile>[] enemyParabolaProjectilesPools
	{
		get { return _enemyParabolaProjectilesPools; }
		set { _enemyParabolaProjectilesPools = value; }
	}

	/// <summary>Gets and Sets gameObjectsPools property.</summary>
	public GameObjectPool<PoolGameObject>[] gameObjectsPools
	{
		get { return _gameObjectsPools; }
		set { _gameObjectsPools = value; }
	}

	/// <summary>Gets and Sets particleEffectsPools property.</summary>
	public GameObjectPool<ParticleEffect>[] particleEffectsPools
	{
		get { return _particleEffectsPools; }
		set { _particleEffectsPools = value; }
	}

	/// <summary>PoolManager's instance initialization.</summary>
	protected override void OnAwake()
	{
		playerProjectilesPools = GameObjectPool<Projectile>.PopulatedPools(Game.data.playerProjectiles);
		enemyProjectilesPools = GameObjectPool<Projectile>.PopulatedPools(Game.data.enemyProjectiles);
		enemyHomingProjectilesPools = GameObjectPool<HomingProjectile>.PopulatedPools(Game.data.enemyHomingProjectiles);
		enemyParabolaProjectilesPools = GameObjectPool<ParabolaProjectile>.PopulatedPools(Game.data.enemyParabolaProjectiles);
		gameObjectsPools = GameObjectPool<PoolGameObject>.PopulatedPools(Game.data.poolObjects);
		particleEffectsPools = GameObjectPool<ParticleEffect>.PopulatedPools(Game.data.particleEffects);
	}

	/// <summary>Gets a Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_ID">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_direction">Spawn direction for the Projectile.</param>
	/// <returns>Requested Projectile.</returns>
	public static Projectile RequestProjectile(Faction _faction, int _ID, Vector3 _position, Vector3 _direction)
	{
		GameObjectPool<Projectile> factionPool = _faction == Faction.Ally ?
			Instance.playerProjectilesPools[_ID] : Instance.enemyProjectilesPools[_ID];
		Projectile projectile = factionPool.Recycle(_position, Quaternion.identity);
		projectile.direction = _direction.normalized;

		//return Instance.playerProjectilesPools[_ID].Recycle(_position, Quaternion.identity);
		return projectile;
	}

	/// <summary>Gets a Homing Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_ID">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_direction">Spawn direction for the Projectile.</param>
	/// <param name="target">Target's Function [null by default].</param>
	/// <returns>Requested Homing Projectile.</returns>
	public static HomingProjectile RequestHomingProjectile(Faction _faction, int _ID, Vector3 _position, Vector3 _direction, Func<Vector2> _target)
	{
		/*GameObjectPool<HomingProjectile> factionPool = _faction == Faction.Ally ?
			Instance.playerHomingProjectilesPools[_ID] : Instance.enemyHomingProjectilesPools[_ID];
		HomingProjectile projectile = factionPool.Recycle(_position, Quaternion.identity);*/
		HomingProjectile projectile = Instance.enemyHomingProjectilesPools[_ID].Recycle(_position, Quaternion.identity);
		projectile.target = _target;
		projectile.direction = _direction.normalized;
		
		return projectile;
	}

	/// <summary>Gets a Parabola Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_ID">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_target">Target that will determine the ParabolaProjectile's velocity.</param>
	/// <param name="t">Time it should take the projectile to reach from its position to the given target.</param>
	/// <returns>Requested Parabola Projectile.</returns>
	public static ParabolaProjectile RequestParabolaProjectile(Faction _faction, int _ID, Vector3 _position, Vector3 _target, float t)
	{
		Vector3 velocity = VPhysics.ProjectileDesiredVelocity(t, _position, _target, Physics.gravity);
		float speed = velocity.magnitude;
		ParabolaProjectile projectile = Instance.enemyParabolaProjectilesPools[_ID].Recycle(_position, Quaternion.identity);
		projectile.direction = velocity.normalized;
		projectile.speed = speed;

		return projectile;
	}

	/// <summary>Gets a PoolGameObject from the PoolGameObjects' Pools.</summary>
	/// <param name="_index">GameObject's ID.</param>
	/// <param name="_position">Spawn position for the GameObject.</param>
	/// <param name="_rotation">Spawn rotation for the GameObject.</param>
	/// <returns>Requested PoolGameObject.</returns>
	public static PoolGameObject RequestPoolGameObject(int _index, Vector3 _position, Quaternion _rotation)
	{
		return Instance.gameObjectsPools[_index].Recycle(_position, _rotation);
	}

	/// <summary>Gets a ParticleEffect from the ParticleEffects' Pools.</summary>
	/// <param name="_index">ParticleEffect's index on the pools.</param>
	/// <param name="_position">Spawn's Position.</param>
	/// <param name="_rotation">Spawn's Rotation.</param>
	/// <returns>Requested ParticleEffect.</returns>
	public static ParticleEffect RequestParticleEffect(int _index, Vector3 _position, Quaternion _rotation)
	{
		return Instance.particleEffectsPools[_index].Recycle(_position, _rotation);
	}
}
}