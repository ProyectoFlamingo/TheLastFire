using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class BlackboardManager : Singleton<BlackboardManager>
{
	private Blackboard<string, HashSet<Projectile>> _projectilesBlackboards;

	/// <summary>Gets and Sets projectilesBlackboards property.</summary>
	public static Blackboard<string, HashSet<Projectile>> projectilesBlackboards
	{
		get { return Instance._projectilesBlackboards; }
		set { Instance._projectilesBlackboards = value; }
	}

	private static void AddProjectile(string _key, Projectile _projectile)
	{
		/*if(projectilesBlackboards == null) projectilesBlackboards = new Blackboard<string, HashSet<Projectile>>();
		if(projectilesBlackboards.ContainsKey)*/

	}
}
}