using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class BlackboardManager : Singleton<BlackboardManager>
{
	private Blackboard<string, HashSet<Projectile>> _projectilesBlackboards; 	/// <summary>Projectiles' Blackboard.</summary>

	/// <summary>Gets and Sets projectilesBlackboards property.</summary>
	public static Blackboard<string, HashSet<Projectile>> projectilesBlackboards
	{
		get { return Instance._projectilesBlackboards; }
		set { Instance._projectilesBlackboards = value; }
	}

	/// <summary>Adds Projectile to Blackboard of Projectiles.</summary>
	/// <param name="_key">Key on Blackboard.</param>
	/// <param name="_projectile">Projectile to add.</param>
	public static void AddProjectile(string _key, Projectile _projectile)
	{
		if(projectilesBlackboards == null) projectilesBlackboards = new Blackboard<string, HashSet<Projectile>>();
		
		bool containsKey = projectilesBlackboards.ContainsKey(_key);
		HashSet<Projectile> projectiles = containsKey ? projectilesBlackboards[_key] : new HashSet<Projectile>();
		
		projectiles.Add(_projectile);
		projectilesBlackboards.AddEntry(_key, projectiles);
	}

	/// <summary>Gets Projectiles' Hashset from given key.</summary>
	/// <param name="_key">Projectiles' Hashset's key.</param>
	/// <returns>Projectiles' Hashset from given key.</returns>
	public static HashSet<Projectile> GetProjectiles(string _key)
	{
		return projectilesBlackboards != null ? projectilesBlackboards[_key] : null;
	}

	/// <summary>Callbaack invoked when a projectile is deactivated.</summary>
	/// <param name="_poolObject">PoolObject of the Projectile that was deactivated.</param>
	private void OnProjectileDeactivated(IPoolObject _poolObject)
	{
		if(projectilesBlackboards == null) return;

		Projectile projectile = _poolObject as Projectile;

		if(projectile == null) return;

		string key = projectile.name;

		if(!projectilesBlackboards.ContainsKey(key)) return;

		projectilesBlackboards[key].Remove(projectile);
	}
}
}