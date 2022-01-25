using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;

public class TEST_Rotate : MonoBehaviour
{
	[SerializeField] private Vector3 rotationAxis; 	/// <summary>Rotation's Axes.</summary>

	/// <summary>TEST_Rotate's tick at each frame.</summary>
	private void Update ()
	{
		transform.Rotate(rotationAxis * Time.deltaTime);		
	}
}