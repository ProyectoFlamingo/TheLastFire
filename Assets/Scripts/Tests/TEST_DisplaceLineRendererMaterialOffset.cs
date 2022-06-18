using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TEST_DisplaceLineRendererMaterialOffset : MonoBehaviour
{
	[SerializeField] private float displacementSpeed; 	/// <summary>Displacement Speed.</summary>
	[SerializeField] private string propertyName; 		/// <summary>Property's Name.</summary>
	private int hash;
	private LineRenderer _lineRenderer;

	/// <summary>Gets lineRenderer Component.</summary>
	public LineRenderer lineRenderer
	{ 
		get
		{
			if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
			return _lineRenderer;
		}
	}

#region UnityMethods:
	/// <summary>TEST_DisplaceLineRendererMaterialOffset's instance initialization.</summary>
	private void Awake()
	{
		hash = Shader.PropertyToID(propertyName);
	}

	/// <summary>TEST_DisplaceLineRendererMaterialOffset's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		
	}
	
	/// <summary>TEST_DisplaceLineRendererMaterialOffset's tick at each frame.</summary>
	private void Update ()
	{
		Vector4 v = lineRenderer.material.GetVector(hash);
		v.z += displacementSpeed * Time.deltaTime;
		lineRenderer.material.SetVector(hash, v);
	}
#endregion
}