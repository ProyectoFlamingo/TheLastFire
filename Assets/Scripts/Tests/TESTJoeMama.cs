using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TESTJoeMama : MonoBehaviour
{
	[SerializeField] private Transform reference; 	/// <summary>Reference Transform.</summary>

	/// <summary>Updates TESTJoeMama's instance at the end of each frame.</summary>
	private void LateUpdate()
	{
		if(reference == null) return;

		Vector3 euler = reference.rotation.eulerAngles;
		euler.x = 0.0f;
		euler.y = 0.0f;

		transform.rotation = Quaternion.Euler(euler);
	}
}