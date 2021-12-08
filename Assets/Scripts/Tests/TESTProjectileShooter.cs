using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;

public class TESTProjectileShooter : MonoBehaviour
{
	[SerializeField] private KeyCode routineInput; 					/// <summary>Input that triggers the routine.</summary>
	[SerializeField] private int[] playerProjectilesIndices; 		/// <summary>Player Projectiles' Indices.</summary>
	[SerializeField] private int[] enemyProjectilesIndices; 		/// <summary>Enemy Projectiles' Indices.</summary>
	[SerializeField] private int[] enemyHomingProjectilesIndices; 	/// <summary>Enemy Homing Proejctiles' Indices.</summary>
	[SerializeField] private Transform target; 						/// <summary>Projectiles' Target.</summary>
	[SerializeField] private float cooldown; 						/// <summary>Cooldown duration after each shot.</summary>
	private Coroutine coroutine; 									/// <summary>Coroutine's Reference.</summary>
#if UNITY_EDITOR
	[SerializeField] private Color color; 							/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float radius; 							/// <summary>Gizmos' Radius.</summary>
#endif

	/// <summary>Draws Gizmos on Editor mode when TESTProjectileShooter's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
#if UNITY_EDITOR
		Gizmos.color = color;
		Gizmos.DrawWireSphere(target.position, radius);
#endif
	}

	/// <summary>TESTProjectileShooter's instance initialization.</summary>
	private void Awake()
	{
		BeginProjectilesRoutine();
	}

	/// <summary>Updates TESTProjectileShooter's instance at each frame.</summary>
	private void Update()
	{
		if(Input.GetKeyUp(routineInput)) BeginProjectilesRoutine();
	}

	/// <returns>Target.</returns>
	private Vector2 GetTarget() { return target.position; }

	/// <summary>Begins Projectiles' Routine.</summary>
	private void BeginProjectilesRoutine()
	{
		this.StartCoroutine(ProjectilesRoutine(), ref coroutine);
	}

	/// <summary>Projectiles' Routine.</summary>
	private IEnumerator ProjectilesRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(cooldown);
		Vector3 direction = Vector3.zero;

		/// Player Projectiles' Routine:
		foreach(int index in playerProjectilesIndices)
		{
			direction = target.position - transform.position;
			PoolManager.RequestProjectile(Faction.Ally, index, transform.position, direction);
			while(wait.MoveNext()) yield return null;
			wait.Reset();
		}

		/// Enemy Projectiles' Routine:
		foreach(int index in enemyProjectilesIndices)
		{
			direction = target.position - transform.position;
			PoolManager.RequestProjectile(Faction.Enemy, index, transform.position, direction);
			while(wait.MoveNext()) yield return null;
			wait.Reset();
		}

		/// Enemy Homing Projectiles' Routine:
		foreach(int index in enemyHomingProjectilesIndices)
		{
			direction = target.position - transform.position;
			PoolManager.RequestHomingProjectile(Faction.Enemy, index, transform.position, direction, target);
			while(wait.MoveNext()) yield return null;
			wait.Reset();
		}
	}
}