using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public class TESTRaycaster : MonoBehaviour
{
	public LayerMask mask;
	public Vector3 direction;

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawRay(transform.position, direction);
		Gizmos.color = Gizmos.color.WithAlpha(0.5f);
		Gizmos.DrawSphere(transform.position + direction, 0.5f);
	}
	
	/// <summary>TESTRaycaster's tick at each frame.</summary>
	private void Update ()
	{
		RaycastHit2D hitInfo = default(RaycastHit2D);
		hitInfo = Physics2D.Raycast(transform.position, direction, direction.magnitude, mask);

		if(hitInfo.transform != null)
		{
			Debug.DrawRay(hitInfo.point, hitInfo.normal * 5.0f, Color.yellow, 5.0f);
			//hitInfo.transform.gameObject.SetActive(false);
		}
	}
}