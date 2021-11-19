using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(CapsuleCollider2D))]
public class CapsuleCollider2DAlligner : Collider2DAlligner
{
	[SerializeField] private CapsuleCollider2D _capsuleCollider; 	/// <summary>CapsuleCollider2D's Component.</summary>
	[SerializeField] private TransformProperties _properties; 		/// <summary>Transform Properties to change.</summary>

	/// <summary>Gets capsuleCollider Component.</summary>
	public CapsuleCollider2D capsuleCollider
	{ 
		get
		{
			if(_capsuleCollider == null) _capsuleCollider = GetComponent<CapsuleCollider2D>();
			return _capsuleCollider;
		}
	}

	/// <summary>Gets and Sets properties property.</summary>
	public TransformProperties properties
	{
		get { return _properties; }
		set { _properties = value; }
	}

	/// <summary>Resets CapsuleCollider2DAlligner's instance to its default values.</summary>
	private void Reset()
	{
		properties = TransformProperties.All;
	}

	/// <summary>Updates Collider2D.</summary>
	protected override void UpdateCollider()
	{
		if(transformA == null || transformB == null || properties == TransformProperties.None) return;

		Vector3 pointA = transformA.TransformPoint(a);
		Vector3 pointB = transformB.TransformPoint(b);
		Vector2 d = pointB - pointA;
		Vector2 size = Vector2.zero;
		float m = d.magnitude;

		switch(capsuleCollider.direction)
		{
			case CapsuleDirection2D.Vertical:
			size = capsuleCollider.size.WithY(m);
			break;
		
			case CapsuleDirection2D.Horizontal:
			size = capsuleCollider.size.WithX(m);
			break;
		}

		if((properties | TransformProperties.Position) == properties) transform.position = Vector3.Lerp(pointA, pointB, 0.5f);
		if((properties | TransformProperties.Rotation) == properties) transform.rotation = VQuaternion.RightLookRotation(d, Vector3.forward);
		if((properties | TransformProperties.Scale) == properties) capsuleCollider.size = size;
	}
}
}