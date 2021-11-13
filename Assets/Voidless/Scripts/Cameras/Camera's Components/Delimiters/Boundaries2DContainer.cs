using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Voidless
{
public class Boundaries2DContainer : MonoBehaviour
{
	[SerializeField] private Space _space; 					/// <summary>Space Relativeness.</summary>
	[SerializeField] private Vector3 _size; 				/// <summary>Boundaries' Size.</summary>
	[SerializeField] private Vector3 _center; 				/// <summary>Boundaries' Center.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private Color color; 					/// <summary>Gizmos' Color.</summary>
#endif
	private Coroutine boundaries2DInterpolation; 			/// <summary>Coroutine's reference.</summary>

	/// <summary>Gets and Sets space property.</summary>
	public Space space
	{
		get { return _space; }
		set { _space = value; }
	}

	/// <summary>Gets and Sets size property.</summary>
	public Vector3 size
	{
		get { return _size; }
		set { _size = value; }
	}

	/// <summary>Gets and Sets center property.</summary>
	public Vector3 center
	{
		get { return space == Space.World ? _center : (transform.position + _center); }
		set { _center = value; }
	}

	/// <summary>Gets min property.</summary>
	public Vector3 min { get { return center - (size * 0.5f); } }

	/// <summary>Gets max property.</summary>
	public Vector3 max { get { return center + (size * 0.5f); } }

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = color;

		/// Draw Boundary's Limits:
		Vector3 bottomLeftPoint = min;
		Vector3 bottomRightPoint = new Vector3(max.x, min.y, min.z);
		Vector3 topLeftPoint = new Vector3(min.x, max.y, min.z);
		Vector3 topRightPoint = max;
		
		Gizmos.DrawLine(bottomLeftPoint, bottomRightPoint);
		Gizmos.DrawLine(bottomLeftPoint, topLeftPoint);
		Gizmos.DrawLine(topLeftPoint, topRightPoint);
		Gizmos.DrawLine(bottomRightPoint, topRightPoint);
		Gizmos.DrawCube(center, size);
	}

	/// <summary>Resets VCamera2DBoundariesContainer's instance to its default values.</summary>
	private void Reset()
	{
		space = Space.Self;
		color = Color.white.WithAlpha(0.5f);
	}
#endif

	/// <returns>Position of the Boundaries 2D in World-Space [center relative to the position of the container].</returns>
	public Vector3 GetPosition() { return transform.position + _center; }

	/// <returns>Random point inside boundaries.</returns>
	public Vector3 Random()
	{
		Vector3 m = min;
		Vector3 M = max;
		
		return new Vector3
		(
			UnityEngine.Random.Range(m.x, M.x),
			UnityEngine.Random.Range(m.y, M.y),
			UnityEngine.Random.Range(m.z, M.z)
		);
	}

	/// <param name="_space">Space Relativeness.</param>
	/// <returns>Data to Boundaries2D.</returns>
	public Boundaries2D ToBoundaries2D(Space _space = Space.World)
	{
		Vector3 c = Vector3.zero;

		if(space != _space)
		{
			switch(_space)
			{
				case Space.World:
				/// Convert from Self to World:
				c = center;
				break;

				case Space.Self:
				/// Convert from World to Self:
				c = _center;
				break;
			}
		}

		return new Boundaries2D(size, c);
	}

	/// <summary>Sets Boundaries2D's data.</summary>
	/// <param name="b">Boundaries2D's data.</param>
	public void Set(Boundaries2D b)
	{
		size = b.size;
		center = b.center;
	}

	/// <summary>Interpolates towards Boundaries2D.</summary>
	/// <param name="b">Boundaries2D's data.</param>
	/// <param name="d">Duration.</param>
	public void InterpolateTowards(Boundaries2D b, float d)
	{
		this.StartCoroutine(InterpolateTowardsBoundaries(b, d, OnInterpolationEnds), ref boundaries2DInterpolation);
	}

	/// <summary>Callback invoked when the Boundaries2D's interpolation ends.</summary>
	public void OnInterpolationEnds()
	{
		this.DispatchCoroutine(ref boundaries2DInterpolation);
	}

	/// <summary>Interpolation's Coroutine.</summary>
	/// <param name="b">Boundaries2D's data.</param>
	/// <param name="d">Duration.</param>
	/// <param name="onInterpolationEnds">Callback invoked when interpolation ends.</param>
	private IEnumerator InterpolateTowardsBoundaries(Boundaries2D b, float d, Action onInterpolationEnds = null)
	{
		Boundaries2D a = ToBoundaries2D();
		float t = 0.0f;
		float iD = 1.0f / d;

		while(t < 1.0f)
		{
			Set(Boundaries2D.Lerp(a, b, t));

			t += (iD * Time.deltaTime);
			yield return null;
		}

		Set(b);
		if(onInterpolationEnds != null) onInterpolationEnds();
	}
}
}