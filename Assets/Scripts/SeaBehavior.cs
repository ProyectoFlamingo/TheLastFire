using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
public class SeaBehavior : MonoBehaviour
{
	[SerializeField] private float _displacementSpeed; 		/// <summary>Displacement's Speed.</summary>
	[SerializeField] private float _displacementMagnitude; 	/// <summary>Displacement's Magnitude.</summary>
	private float y; 										/// <summary>Original Y's Value.</summary>

	/// <summary>Gets and Sets displacementSpeed property.</summary>
	public float displacementSpeed
	{
		get { return _displacementSpeed; }
		set { _displacementSpeed = value; }
	}

	/// <summary>Gets and Sets displacementMagnitude property.</summary>
	public float displacementMagnitude
	{
		get { return _displacementMagnitude; }
		set { _displacementMagnitude = value; }
	}

	/// <summary>SeaBehavior's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		y = transform.position.y;
	}

	/// <summary>SeaBehavior's tick at each frame.</summary>
	private void Update ()
	{
		Vector3 position = transform.position;
		position.y = y + Mathf.Sin(Time.time * displacementSpeed) * displacementMagnitude;
		transform.position = position;
	}
}
}