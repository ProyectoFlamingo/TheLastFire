using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;


public class TEST_ShootProjectilesWithVector3Pair : MonoBehaviour
{
	[SerializeField] private int projectileIndex; 	/// <summary>Projectile's Index.</summary>
	[SerializeField] private float lifespan; 		/// <summary>Projectile's Lifespan.</summary>
	[SerializeField] private Vector3Pair[] pairs; 	/// <summary>Pairs [A = Origin, B = Target].</summary>

	/// <summary>Draws Gizmos on Editor mode when TEST_ShootProjectilesWithVector3Pair's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(pairs == null) return;

		float r = 0.25f;
		Gizmos.color = Color.magenta.WithAlpha(0.5f);

		foreach(Vector3Pair pair in pairs)
		{
			Gizmos.DrawSphere(pair.a, r);
			Gizmos.DrawSphere(pair.b, r);
		}
	}

	/// <summary>TEST_ShootProjectilesWithVector3Pair's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		if(pairs != null) foreach(Vector3Pair pair in pairs)
		{
			Vector3 d = pair.b - pair.a;
			Projectile p = PoolManager.RequestProjectile(Faction.Enemy, projectileIndex, pair.a, d);
			p.lifespan = lifespan;
		}
	}
}