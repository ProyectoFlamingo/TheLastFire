using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_OrbitCameraAroundPoint : MonoBehaviour
{
	[SerializeField] private Vector3 point; 		/// <summary>Interest Point.</summary>
	[SerializeField] private float rotationSpeed; 	/// <summary>Rotation Speed.</summary>

	/// <summary>Draws Gizmos on Editor mode when TEST_OrbitCameraAroundPoint's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(point, 1.0f);
	}
	
	/// <summary>TEST_OrbitCameraAroundPoint's tick at each frame.</summary>
	private void Update ()
	{
		transform.LookAt(point);
		transform.RotateAround(point, Vector3.up, rotationSpeed * Time.deltaTime);
	}
}