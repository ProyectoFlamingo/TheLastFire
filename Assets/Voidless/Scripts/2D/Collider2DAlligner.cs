using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class Collider2DAlligner : MonoBehaviour
{
	[SerializeField] private Transform _transformA; 	/// <summary>Transform A's reference.</summary>
	[SerializeField] private Transform _transformB; 	/// <summary>Transform B's reference.</summary>
	[SerializeField] private Vector3 _a; 				/// <summary>Vector A [relative to transform].</summary>
	[SerializeField] private Vector3 _b; 				/// <summary>Vector B [relative to transform].</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 		/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 		/// <summary>Gizmos' Radius.</summary>
#endif

	/// <summary>Gets and Sets transformA property.</summary>
	public Transform transformA
	{
		get { return _transformA; }
		set { _transformA = value; }
	}

	/// <summary>Gets and Sets transformB property.</summary>
	public Transform transformB
	{
		get { return _transformB; }
		set { _transformB = value; }
	}

	/// <summary>Gets and Sets a property.</summary>
	public Vector3 a
	{
		get { return _a; }
		set { _a = value; }
	}

	/// <summary>Gets and Sets b property.</summary>
	public Vector3 b
	{
		get { return _b; }
		set { _b = value; }
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when Collider2DAlligner's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(transformA == null || transformB == null) return;

		Gizmos.color = gizmosColor;
		Gizmos.DrawSphere(transformA.TransformPoint(a), gizmosRadius);
		Gizmos.DrawSphere(transformB.TransformPoint(b), gizmosRadius);

		if(!Application.isPlaying) UpdateCollider();
	}

	/// <summary>Resets Collider2DAlligner's instance to its default values.</summary>
	private void Reset()
	{
		gizmosColor = Color.white.WithAlpha(0.5f);
		gizmosRadius = 0.1f;
	}
#endif
	
	/// <summary>Collider2DAlligner's tick at each frame.</summary>
	private void Update ()
	{
		UpdateCollider();
	}

	/// <summary>Updates Collider2D.</summary>
	protected virtual void UpdateCollider() { /*...*/ }
}
}