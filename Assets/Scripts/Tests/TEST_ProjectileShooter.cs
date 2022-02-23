using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;

public class TEST_ProjectileShooter : MonoBehaviour
{
	[SerializeField] private Transform target; 	/// <summary>Target.</summary>
	[SerializeField] private int projectileID; 	/// <summary>Projectile's ID.</summary>
	[SerializeField] private float fireRatio; 	/// <summary>Fire's Ratio.</summary>
	private float time;
	
	/// <summary>TEST_ProjectileShooter's tick at each frame.</summary>
	private void Update ()
	{
		if(target == null) return;

		if(time < fireRatio) time += Time.deltaTime;
		else
		{
			Vector3 direction = target.position - transform.position;
			PoolManager.RequestProjectile(Faction.Enemy, projectileID, transform.position, direction);
			time = 0.0f;
		}
	}
}