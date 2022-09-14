using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Flamingo
{
public class PoolManager : Singleton<PoolManager>
{
	public static event OnResourcesLoaded onPoolsCreated; 											/// <summary>OnResourcesLoaded's Event Delegate.</summary>

	private Dictionary<VAssetReference, GameObjectPool<Character>> _charactersPoolsMap; 			/// <summary>Mapping of Characters' Pools.</summary>
	private Dictionary<VAssetReference, GameObjectPool<Projectile>> _projectilesPoolsMap; 			/// <summary>Mapping of Projectiles' Pools.</summary>
	private Dictionary<VAssetReference, GameObjectPool<PoolGameObject>> _gameObjectsPoolsMap; 		/// <summary>Mapping of Pool-GameObjects' Pools.</summary>
	private Dictionary<VAssetReference, GameObjectPool<ParticleEffect>> _particleEffectsPoolsMap; 	/// <summary>Mapping of Particle-Effects' Pools.</summary>
	private Dictionary<VAssetReference, GameObjectPool<Explodable>> _explodablesPoolsMap; 			/// <summary>Mapping of Explodables' Pools.</summary>
	private GameObjectPool<SoundEffectLooper> _loopersPool; 										/// <summary>Pool of Sound-Effects' Loopers.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets charactersPoolsMap property.</summary>
	public Dictionary<VAssetReference, GameObjectPool<Character>> charactersPoolsMap
	{
		get { return _charactersPoolsMap; }
		private set { _charactersPoolsMap = value; }
	}

	/// <summary>Gets and Sets projectilesPoolsMap property.</summary>
	public Dictionary<VAssetReference, GameObjectPool<Projectile>> projectilesPoolsMap
	{
		get { return _projectilesPoolsMap; }
		private set { _projectilesPoolsMap = value; }
	}

	/// <summary>Gets and Sets particleEffectsPoolsMap property.</summary>
	public Dictionary<VAssetReference, GameObjectPool<ParticleEffect>> particleEffectsPoolsMap
	{
		get { return _particleEffectsPoolsMap; }
		private set { _particleEffectsPoolsMap = value; }
	}

	/// <summary>Gets and Sets explodablesPoolsMap property.</summary>
	public Dictionary<VAssetReference, GameObjectPool<Explodable>> explodablesPoolsMap
	{
		get { return _explodablesPoolsMap; }
		private set { _explodablesPoolsMap = value; }
	}

	/// <summary>Gets and Sets gameObjectsPoolsMap property.</summary>
	public Dictionary<VAssetReference, GameObjectPool<PoolGameObject>> gameObjectsPoolsMap
	{
		get { return _gameObjectsPoolsMap; }
		private set { _gameObjectsPoolsMap = value; }
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
		base.OnAwake();

		loopersPool = new GameObjectPool<SoundEffectLooper>(Game.data.looper);
	}

	/// <summary>Called after Awake, before the first Update.</summary>
	protected void Start()
	{
		ResourcesManager.onResourcesLoaded += OnResourcesLoaded;
	}

	/// <summary>Callback invoked when PoolManager's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		ResourcesManager.onResourcesLoaded -= OnResourcesLoaded;
	}

	/// <summary>Callback invoked when resources haven been loaded by the ResourcesLoader.</summary>
	protected void OnResourcesLoaded()
	{
		charactersPoolsMap = VAddressables.PopulaledPoolsMapping(ResourcesManager.Instance.charactersMap);
		projectilesPoolsMap = VAddressables.PopulaledPoolsMapping(ResourcesManager.Instance.projectilesMap);
		gameObjectsPoolsMap = VAddressables.PopulaledPoolsMapping(ResourcesManager.Instance.poolObjectsMap);
		particleEffectsPoolsMap = VAddressables.PopulaledPoolsMapping(ResourcesManager.Instance.particleEffectsMap);
		explodablesPoolsMap = VAddressables.PopulaledPoolsMapping(ResourcesManager.Instance.explodablesMap);

		//Test();
		if(onPoolsCreated != null) onPoolsCreated();
	}

#region AddressableFunctions:
	/// <summary>Gets a Character from the Characters' Pool.</summary>
	/// <param name="_reference">Character's Asset Reference.</param>
	/// <param name="_position">Character's Position.</param>
	/// <param name="_rotation">Character's Rotation.</param>
	public static Character RequestCharacter(VAssetReference _reference, Vector3 _position, Quaternion _rotation)
	{
		if(_reference.Empty()) return null;

		GameObjectPool<Character> pool = null;

		try
		{
			if(!Instance.charactersPoolsMap.TryGetValue(_reference, out pool))
			Game.ShowErrorWindow(_reference, "Character");
		}
		catch(Exception e)
		{
			Game.ShowErrorWindow(_reference, "Character", e.Message);
			return null;
		}

		return pool.Recycle(_position, _rotation);
	}

	/// <summary>Gets a Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_reference">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_direction">Spawn direction for the Projectile.</param>
	/// <param name="_object">GameObject that requests the Parabola [treated as the shooter]. Null by default.</param>
	/// <returns>Requested Projectile.</returns>
	public static Projectile RequestProjectile(Faction _faction, VAssetReference _reference, Vector3 _position, Vector3 _direction, GameObject _object = null)
	{
		if(_reference.Empty()) return null;
		
		GameObjectPool<Projectile> pool = null;

		try
		{
			if(!Instance.projectilesPoolsMap.TryGetValue(_reference, out pool))
			Game.ShowErrorWindow(_reference, "Projectile");
		}
		catch(Exception e)
		{
			Game.ShowErrorWindow(_reference, "Projectile", e.Message);
			return null;
		}

		Projectile projectile = pool.Recycle(_position, Quaternion.identity);
		
		if(projectile == null) return null;

		string tag = _faction == Faction.Ally ? Game.data.playerProjectileTag : Game.data.enemyProjectileTag;

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
	/// <param name="_reference">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_direction">Spawn direction for the Projectile.</param>
	/// <param name="target">Target's Function [null by default].</param>
	/// <param name="_object">GameObject that requests the Parabola [treated as the shooter]. Null by default.</param>
	/// <returns>Requested Homing Projectile.</returns>
	public static Projectile RequestHomingProjectile(Faction _faction, VAssetReference _reference, Vector3 _position, Vector3 _direction, Transform _target, GameObject _object = null)
	{
		if(_reference.Empty()) return null;

		Projectile projectile = RequestProjectile(_faction, _reference, _position, _direction, _object);

		if(projectile == null) return null;

		projectile.projectileType = ProjectileType.Homing;
		projectile.target = _target;
		
		return projectile;
	}

	/// <summary>Gets a Parabola Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_reference">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_target">Target that will determine the ParabolaProjectile's velocity.</param>
	/// <param name="t">Time it should take the projectile to reach from its position to the given target.</param>
	/// <param name="_object">GameObject that requests the Parabola [treated as the shooter]. Null by default.</param>
	/// <returns>Requested Parabola Projectile.</returns>
	public static Projectile RequestParabolaProjectile(Faction _faction, VAssetReference _reference, Vector3 _position, Vector3 _target, float t, GameObject _object = null, float _gravityScale = 1.0f)
	{
		if(_reference.Empty()) return null;

		Projectile projectile = RequestProjectile(_faction, _reference, _position, Vector3.zero, _object);

		if(projectile == null) return null;

		projectile.gravityScale = _gravityScale;
		Vector3 velocity = VPhysics.ProjectileDesiredVelocity(
			t,
			_position,
			_target,
			Physics.gravity * projectile.gravityScale,
			projectile.speedMode == SpeedMode.Accelerating
		);
		float magnitude = velocity.magnitude;

		projectile.projectileType = ProjectileType.Parabola;
		projectile.direction = velocity;
		projectile.speed = magnitude;
		projectile.parabolaTime = t;

		Debug.DrawRay(_position, velocity, Color.magenta, 3.0f);

		return projectile;
	}

	/// <summary>Gets a PoolGameObject from the PoolGameObjects' Pools.</summary>
	/// <param name="_reference">GameObject's ID.</param>
	/// <param name="_position">Spawn position for the GameObject.</param>
	/// <param name="_rotation">Spawn rotation for the GameObject.</param>
	/// <returns>Requested PoolGameObject.</returns>
	public static PoolGameObject RequestPoolGameObject(VAssetReference _reference, Vector3 _position, Quaternion _rotation)
	{
		if(_reference.Empty()) return null;

		GameObjectPool<PoolGameObject> pool = null;
		
		try
		{
			if(!Instance.gameObjectsPoolsMap.TryGetValue(_reference, out pool))
			Game.ShowErrorWindow(_reference, "Pool Object");
		}
		catch(Exception e)
		{
			Game.ShowErrorWindow(_reference, "Pool Object", e.Message);
			return null;
		}

		return pool.Recycle(_position, _rotation);
	}

	/// <summary>Gets a ParticleEffect from the ParticleEffects' Pools.</summary>
	/// <param name="_reference">ParticleEffect's index on the pools.</param>
	/// <param name="_position">Spawn's Position.</param>
	/// <param name="_rotation">Spawn's Rotation.</param>
	/// <param name="_scale">Particle-Effect's Scale [1.0f by default].</param>
	/// <returns>Requested ParticleEffect.</returns>
	public static ParticleEffect RequestParticleEffect(VAssetReference _reference, Vector3 _position, Quaternion _rotation, float _scale = 1.0f)
	{
		if(_reference.Empty()) return null;

		GameObjectPool<ParticleEffect> pool = null;

		try
		{
			if(!Instance.particleEffectsPoolsMap.TryGetValue(_reference, out pool))
			Game.ShowErrorWindow(_reference, "Particle-Effect");
		}
		catch(Exception e)
		{
			Game.ShowErrorWindow(_reference, "Particle-Effect", e.Message);
			return null;
		}

		ParticleEffect effect = pool.Recycle(_position, _rotation);

		effect.transform.localScale = Vector3.one * _scale;

		return effect;
	}

	/// <summary>Gets a Explodable from the Explodables' Pools.</summary>
	/// <param name="_reference">Explodable's index on the pools.</param>
	/// <param name="_position">Spawn's Position.</param>
	/// <param name="_rotation">Spawn's Rotation.</param>
	/// <param name="onExplosionEnds">Optional Callback invoked when the explosion ends.</param>
	public static Explodable RequestExplodable(VAssetReference _reference, Vector3 _position, Quaternion _rotation, Action onExplosionEnds = null)
	{
		GameObjectPool<Explodable> pool = null;

		try
		{
			if(!Instance.explodablesPoolsMap.TryGetValue(_reference, out pool))
			Game.ShowErrorWindow(_reference, "Explodable");
		}
		catch(Exception e)
		{
			Game.ShowErrorWindow(_reference, "Explodable", e.Message);
			return null;
		}

		Explodable explodable = pool.Recycle(_position, _rotation);
		explodable.Explode(onExplosionEnds);

		return explodable;
	}
#endregion

	/// <returns>Requested Sound-Effect's Looper.</returns>
	public static SoundEffectLooper RequestSoundEffectLooper()
	{
		return Instance.loopersPool.Recycle(Vector3.zero, Quaternion.identity);
	}

	/// <returns>String representing PoolManager.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("PoolManager: \n");
		builder.AppendLine(charactersPoolsMap.DictionaryToString());

		return builder.ToString();
	}

	/// <summary>Debugs Pools.</summary>
	private void Test()
	{
		int i = 0;

		foreach(KeyValuePair<VAssetReference, GameObjectPool<Character>> pairs in charactersPoolsMap)
		{
			Debug.Log("[PoolManager] Character-Pool #" + i + " of VAssetReference " + pairs.Key + ": " + pairs.Value.ToString());
			i++;
		}
	}
}
}