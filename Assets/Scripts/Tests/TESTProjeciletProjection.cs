using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public class TESTProjeciletProjection : MonoBehaviour
{
	[SerializeField] private Vector3 p0; 	/// <summary>Initial Position.</summary>
	[SerializeField] private Vector3 pf; 	/// <summary>Target Position.</summary>
	[SerializeField] private float t; 		/// <summary>Time.</summary>
	[SerializeField] private float s; 		/// <summary>Gravity's Scalar.</summary>

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;

		Vector3 g = Physics.gravity * s;
		Vector3 v0 = VPhysics.ProjectileDesiredVelocity(t, p0, pf, g);
		
		VGizmos.DrawParabolaProjection(p0, v0, g, t);
		Gizmos.DrawRay(p0, v0);
		Gizmos.DrawWireSphere(p0, 0.5f);
		Gizmos.DrawWireSphere(pf, 0.5f);
	}
}