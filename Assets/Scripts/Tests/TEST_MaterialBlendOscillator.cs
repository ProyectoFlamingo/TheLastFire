using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flamingo;
using Voidless;

public class TEST_MaterialBlendOscillator : MonoBehaviour
{
	[SerializeField] private Renderer renderer; 	/// <summary>Renderer's Component.</summary>
	[SerializeField] private string blendProperty; 	/// <summary>Blend's MAterial Property.</summary>
	[SerializeField] private float speed; 			/// <summary>Material Blending's Oscillation Speed.</summary>
	private float time;

	/// <summary>TEST_MaterialBlendOscillator's instance initialization.</summary>
	private void Awake()
	{
		time = 0.0f;
	}
	
	/// <summary>TEST_MaterialBlendOscillator's tick at each frame.</summary>
	private void Update ()
	{
		if(renderer == null) return;

		float t = VMath.RemapValueToNormalizedRange(Mathf.Sin(time * speed), -1.0f, 1.0f);

		renderer.material.SetFloat(blendProperty, t);

		time += Time.deltaTime;	
	}
}