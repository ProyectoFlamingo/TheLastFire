using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class PoolManager : Singleton<PoolManager>
{
	private GameObjectPool<Projectile>[] _projectilesPools; 						/// <summary>Pools Projectiles.</summary>
	private GameObjectPool<Projectile>[] _playerProjectilesPools; 					/// <summary>Pool of Player's Projectiles.</summary>
	private GameObjectPool<Projectile>[] _enemyProjectilesPools; 					/// <summary>Pool of Enemy's Projectiles.</summary>
	private GameObjectPool<PoolGameObject>[] _gameObjectsPools; 					/// <summary>PoolGameObjects' Pools.</summary>
	private GameObjectPool<ParticleEffect>[] _particleEffectsPools; 				/// <summary>Pools of Particle's Effects.</summary>
	private GameObjectPool<Explodable>[] _explodablesPools; 						/// <summary>Pools of Explodables.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets projectilesPools property.</summary>
	public GameObjectPool<Projectile>[] projectilesPools
	{
		get { return _projectilesPools; }
		private set { _projectilesPools = value; }
	}

	/// <summary>Gets and Sets playerProjectilesPools property.</summary>
	public GameObjectPool<Projectile>[] playerProjectilesPools
	{
		get { return _playerProjectilesPools; }
		private set { _playerProjectilesPools = value; }
	}

	/// <summary>Gets and Sets enemyProjectilesPools property.</summary>
	public GameObjectPool<Projectile>[] enemyProjectilesPools
	{
		get { return _enemyProjectilesPools; }
		private set { _enemyProjectilesPools = value; }
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
#endregion

	/// <summary>PoolManager's instance initialization.</summary>
	protected override void OnAwake()
	{
		projectilesPools = GameObjectPool<Projectile>.PopulatedPools(Game.data.projectiles);
		playerProjectilesPools = GameObjectPool<Projectile>.PopulatedPools(Game.data.playerProjectiles);
		enemyProjectilesPools = GameObjectPool<Projectile>.PopulatedPools(Game.data.enemyProjectiles);
		gameObjectsPools = GameObjectPool<PoolGameObject>.PopulatedPools(Game.data.poolObjects);
		particleEffectsPools = GameObjectPool<ParticleEffect>.PopulatedPools(Game.data.particleEffects);
		explodablesPools = GameObjectPool<Explodable>.PopulatedPools(Game.data.explodables);
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
		/*GameObjectPool<Projectile> factionPool = _faction == Faction.Ally ?
			Instance.playerProjectilesPools[_ID] : Instance.enemyProjectilesPools[_ID];
		Projectile projectile = factionPool.Recycle(_position, Quaternion.identity);*/
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

		Vector3 velocity = VPhysics.ProjectileDesiredVelocity(t, _position, _target, Physics.gravity);
		float magnitude = velocity.magnitude;

		projectile.projectileType = ProjectileType.Parabola;
		projectile.direction = velocity;
		projectile.speed = magnitude;
		projectile.parabolaTime = t;

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

	/// <summary>Gets a Explodable from the Explodables' Pools.</summary>
	/// <param name="_index">Explodable's index on the pools.</param>
	/// <param name="_position">Spawn's Position.</param>
	/// <param name="_rotation">Spawn's Rotation.</param>
	/// <param name="onExplosionEnds">Optional Callback invoked when the explosion ends.</param>
	public static Explodable RequestExplodable(int _index, Vector3 _position, Quaternion _rotation, Action onExplosionEnds = null)
	{
		Explodable explodable = Instance.explodablesPools[_index].Recycle(_position, _rotation);
		explodable.Explode(onExplosionEnds);
		return explodable;
	}
}
}