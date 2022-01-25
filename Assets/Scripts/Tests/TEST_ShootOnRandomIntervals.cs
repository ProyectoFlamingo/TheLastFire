using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;

public class TEST_ShootOnRandomIntervals : MonoBehaviour
{
	[SerializeField] private int projectileIndex; 	/// <summary>Projectile's Index.</summary>
	[SerializeField] private FloatRange interval; 	/// <summary>Shoot's Interval.</summary>

	/// <summary>TEST_ShootOnRandomIntervals's starting actions before 1st Update frame.</summary>
	private IEnumerator Start ()
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		
		while(true)
		{	
			wait.ChangeDurationAndReset(interval.Random());
			while(wait.MoveNext()) yield return null;

			PoolManager.RequestProjectile(Faction.Ally, projectileIndex, transform.position, transform.forward);
		}
	}
}