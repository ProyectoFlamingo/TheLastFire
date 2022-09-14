using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VLineRenderer
{
	public static void DrawCone(this LineRenderer _lineRenderer, float a, float r)
	{
		int points = 3;
	    float radian = a * Mathf.Deg2Rad;
	    float radianFract = radian / (float)points;
	    Vector3 center = _lineRenderer.transform.position;    //Where unit stands
	     
	    _lineRenderer.SetVertexCount(points + 3);    //Add start/finish points
	    Vector3 vect = center;    //Start point is center point.
	    _lineRenderer.SetPosition(0, vect);

	    for(int x = 0; x < points + 1; x++)
	    {
	        vect = center;
	        vect.x += (float)(Math.Cos(radianFract * x) * r);
	        vect.z += (float)(Math.Sin(radianFract * x) * r);
	        _lineRenderer.SetPosition(x + 1, vect);    //Skip first/last points.
	    }
	     
	    vect = center;
	    _lineRenderer.SetPosition(points + 2, vect);    //Last point is center point.											
	}

	/// <summary>Draws Projectile Projection using the LineRenderer.</summary>
	/// <param name="_lineRenderer">LineRenderer's reference.</param>
	/// <param name="p0">Initial position.</param>
	/// <param name="v0">Initial velocity.</param>
	/// <param name="g">Gravity.</param>
	/// <param name="t">Projection's Time.</param>
	public static void DrawProjectileProjection(this LineRenderer _lineRenderer, Vector3 p0, Vector3 v0, Vector3 g, float t, int s = 50)
	{
		if(s <= 0) return;

		float segments = (float)(s);
		Vector3 lastVertexPosition = Vector3.zero;
		float inverse = 1.0f / segments;

		_lineRenderer.SetVertexCount(s);

		for(float i = 0.0f; i < segments; i++)
		{
			int index = (int)i;
			Vector3 position = VPhysics.ProjectileProjection((i * inverse * t), v0, p0, g);

			_lineRenderer.SetPosition(index, position);
		}

		_lineRenderer.SetPosition(s - 1, VPhysics.ProjectileProjection(t, v0, p0, g));
	}

	/// <summary>Sets Start and End Width of LineRenderer equal to the same value.</summary>
	/// <param name="_lineRenderer">LineRenderer's reference.</param>
	/// <param name="_width">Width for Start and End points.</param>
	public static void SetWidth(this LineRenderer _lineRenderer, float _width)
	{
		_lineRenderer.startWidth = _width;
		_lineRenderer.endWidth = _width;
	}

	/// <summary>Gets interpolation of both Start and End points' width.</summary>
	/// <param name="_lineRenderer">LineRenderer's reference.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Interpolation of  Start and Ends' width.</returns>
	public static float GetWidth(this LineRenderer _lineRenderer, float t = 1.0f)
	{
		return  Mathf.Lerp(_lineRenderer.startWidth, _lineRenderer.endWidth, t);
	}
}
}