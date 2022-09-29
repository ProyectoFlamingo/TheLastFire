using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class Collider2DScaleByDepthAdjuster : MonoBehaviour
{
	[SerializeField] private Collider2D[] _colliders; 	/// <summary>Colliders to adjust.</summary>
	[SerializeField] private float _FOV; 				/// <summary>Field of Vision's argument.</summary>
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color originalColor; 		/// <summary>Original Color.</summary>
	[SerializeField] private Color scaledColor; 		/// <summary>Scaled Color.</summary>

	/// <summary>Gets and Sets colliders property.</summary>
	public Collider2D[] colliders
	{
		get { return _colliders; }
		set { _colliders = value; }
	}

	/// <summary>Gets and Sets FOV property.</summary>
	public float FOV
	{
		get { return _FOV; }
		set { _FOV = value; }
	}

	/// <summary>Draws Gizmos on Editor mode when Collider2DScaleByDepthAdjuster's instance is selected.</summary>
	private void OnDrawGizmos()
	{
		if(colliders == null) return;

		Camera camera = Camera.main;
		Matrix4x4 perspectiveMatrix = Matrix4x4.Perspective(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);
		Matrix4x4 cameraOriginalProjectionMatrix = camera.projectionMatrix;

		foreach(Collider2D collider in colliders)
		{
			if(collider == null) continue;

			Vector3 position = collider.transform.position;
			Vector3 scale = collider.transform.localScale;
			Vector3 size = collider.bounds.size;
			Vector3 inverseScale = collider.transform.localScale;
			size.z = 0.1f;
			inverseScale.x = 1.0f / inverseScale.x;
			inverseScale.y = 1.0f / inverseScale.y;
			inverseScale.z = 1.0f / inverseScale.z;

			DrawPerspectiveCube(perspectiveMatrix, position, scale, size, Color.green.WithAlpha(0.5f));
			DrawPerspectiveCube(cameraOriginalProjectionMatrix, position, scale, size, Color.white.WithAlpha(0.5f));
			
			position.z = 0.0f;

			Gizmos.color = originalColor;
			Gizmos.DrawCube(position, Vector3.Scale(size, inverseScale));

			if(collider.transform.localScale.sqrMagnitude == 1.0f) continue;

			Gizmos.color = scaledColor;
			Gizmos.DrawCube(position, size);
		}

		//if(!Application.isPlaying) UpdateCollidersScales();
	}

	private void DrawPerspectiveCube(Matrix4x4 p, Vector3 position, Vector3 scale, Vector3 size, Color color)
	{
		Matrix4x4 trs = Matrix4x4.TRS(position, Quaternion.identity, scale);
		Matrix4x4 t = p * trs;
		Vector4 positionColumn = p.GetColumn(3);
		position = (Vector3)positionColumn;
		position.z = 0.0f;
		size = t.lossyScale;

		Gizmos.color = color;
		Gizmos.DrawCube(position, Vector3.Scale(scale, size));
	}

	/// <summary>Collider2DScaleByDepthAdjuster's tick at each frame.</summary>
	private void Update ()
	{
		UpdateCollidersScales();
	}

	/// <summary>Updates Colliders' Scales.</summary>
	private void UpdateCollidersScales()
	{
		float dz = 0.0f;
		float s = 0.0f;
		float n = 0.0f;
		float d = 0.0f;
		float x = 0.0f;
		Camera camera = Camera.main;
		Matrix4x4 perspectiveMatrix = Matrix4x4.Perspective(camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane);

		/*
		1.0f / 
		*/
		foreach(Collider2D collider in colliders)
		{
			dz = Mathf.Abs(collider.transform.position.z);
			x = 1.0f + (FOV * dz);
			n = dz < 0.0f ? x : 1.0f;
			d = dz < 0.0f ? 1.0f : x;
			s = n / d;
			collider.transform.localScale = Vector3.one * s;
		}
	}
}
}