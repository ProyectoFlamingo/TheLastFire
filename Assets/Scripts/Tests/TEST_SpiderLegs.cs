using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

using Random = UnityEngine.Random;

public enum LineRendererState
{
	None,
	Accelerating,
	Deccelerating
}

public delegate void OnPointFree(Vector3 point);

[Serializable]
public class Extremity
{
	public event OnPointFree onPointFree;

	public LineRenderer lineRenderer;
	public Vector3? currentPoint;
	public Vector3? previousPoint;
	public float time;
	public float speed;
	public float radius;
	public float tangentLength;
	public bool deccelerating;

	public Extremity(LineRenderer _lineRenderer, float _speed, float _radius, float _tangentLength)
	{
		lineRenderer = _lineRenderer;
		speed = _speed;
		radius = _radius;
		tangentLength = _tangentLength;
	}

	public void DeccelerateTime()
	{
		time -= speed * Time.deltaTime;
		time = Mathf.Max(time , 0.0f);

		if(time == 0.0f && previousPoint.HasValue)
		{
			InvokeOnFreeEvent(previousPoint.Value);
			previousPoint = null;
		}
	}

	public void AccelerateTime()
	{
		time += speed * Time.deltaTime;
 		time = Mathf.Min(time, 1.0f);
	}

	public void InvokeOnFreeEvent(Vector3 point)
	{
		if(onPointFree != null) onPointFree(point);
	}

	public Vector3 GetMainPoint()
	{
		return previousPoint.HasValue ? previousPoint.Value :
				 			currentPoint.HasValue ? currentPoint.Value :
				 								lineRenderer.transform.position;
	}
}

//[ExecuteInEditMode]
public class TEST_SpiderLegs : MonoBehaviour
{
	[InfoBox("@ToString()")]
	public LineRenderer[] lineRenderers;
	public SteeringVehicle vehicle;
	public float speed;
	public float radius;
	public float tangentLength;
	public Vector3[] points;
	private Extremity[] extremities;
	private bool hasCurrentPoint;
	private bool hasPreviousPoint;
	private HashSet<Vector3> pointsSet;

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(lineRenderers != null) foreach(LineRenderer lineRenderer in lineRenderers)
		{
			Gizmos.DrawWireSphere(lineRenderer.transform.position, radius);
		}

		if(points != null) foreach(Vector3 point in points)
		{
			Gizmos.DrawWireSphere(point, 0.5f);
			Gizmos.DrawRay(point, Vector3.up * tangentLength);
		}
	}

	[Button("Generate Random Points")]
	private void GenerateRandomPoints()
	{
		float x = 25.0f;

		for(int i = 0; i < points.Length; i++)
		{
			points[i] = new Vector3(
				Random.Range(-x, x),
				0.0f,
				Random.Range(-x, x)
			);
		}
	}

	[Button("Update Attributes")]
	private void UpdateAttributes()
	{
		extremities = new Extremity[lineRenderers.Length];
		int i = 0;

		foreach(LineRenderer lineRenderer in lineRenderers)
		{
			extremities[i] = new Extremity(lineRenderer, speed, radius, tangentLength);
			extremities[i].onPointFree += OnPointFree;
			i++;
		}
	}

	/// <summary>TEST_SpiderLegs's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		pointsSet = new HashSet<Vector3>();
		UpdateAttributes();
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		StartCoroutine(PointsIterator());
	}

	private bool PointOccupied(Extremity requester, Vector3 point)
	{
		foreach(Extremity extremity in extremities)
		{
			if(requester == extremity) continue;

			if(extremity.currentPoint.HasValue && extremity.currentPoint.Value ==  point)
			return true;
		}

		return false;
	}

	/// <summary>Updates Spider's instance at each frame.</summary>
	private void Update()
	{
	 	if(lineRenderers == null) return;

	 	foreach(Extremity extremity in extremities)
	 	{
	 		UpdateExtremity(extremity);
	 	}
	}

	private void OnPointFree(Vector3 point)
	{
		pointsSet.Remove(point);
	}

	private void UpdateExtremity(Extremity extremity)
	{
		Vector3 a = extremity.lineRenderer.transform.position;
		Vector3 b = extremity.GetMainPoint();
		int length = extremity.lineRenderer.positionCount;
		float radius = extremity.radius * extremity.radius;

		GetClosestPoint(extremity);

		float distance = (a - b).sqrMagnitude;

 		if((!extremity.currentPoint.HasValue && !extremity.previousPoint.HasValue)
 			|| extremity.previousPoint.HasValue
 			|| (extremity.currentPoint.HasValue && distance > radius))
 		{
 			extremity.DeccelerateTime();

 		} else if(distance <= radius)
 		{
 			extremity.AccelerateTime();
 		}

 		float t = extremity.time;
		Vector3 destiny = Vector3.Lerp(a, b, t);
 		Vector3 tangent = Vector3.Lerp(a, destiny + (Vector3.up * tangentLength), 0.5f);
 		float timeSplitInverse = (1f / (1f * length));

 		for(int i = 0; i < length - 1; i++)
 		{
 			Vector3 initialPoint =  VMath.CuadraticBeizer(a, destiny, tangent, i * timeSplitInverse);
 			Vector3 endPoint =  VMath.CuadraticBeizer(a, destiny, tangent, (i  + 1.0f) * timeSplitInverse);
 			
 			extremity.lineRenderer.SetPosition(i, initialPoint);
 			extremity.lineRenderer.SetPosition(i + 1, endPoint);
 		}
	}

	private void GetClosestPoint(Extremity extremity)
	{
		Vector3? closestPoint = null;
		Vector3 p = extremity.lineRenderer.transform.position;
		float minDistance = Mathf.Infinity;
		float distance = 0.0f;
		float radius = extremity.radius * extremity.radius;

		foreach(Vector3 point in points)
		{
			//if(pointsSet.Contains(point)) continue;
			if(PointOccupied(extremity, point)) continue;

			distance = (p - point).sqrMagnitude;

 			if(distance < radius && distance <= minDistance)
 			{
 				closestPoint = point;
 				minDistance = distance;
 			}
		}

		if(closestPoint.HasValue)
 		{
 			if(extremity.currentPoint.HasValue && closestPoint.Value != extremity.currentPoint.Value)
 			{
 				extremity.previousPoint = extremity.currentPoint;
 				extremity.InvokeOnFreeEvent(extremity.currentPoint.Value);
 			}
 			extremity.currentPoint = closestPoint;
 			pointsSet.Add(extremity.currentPoint.Value);
 		}
 		else
 		{
 			if(extremity.currentPoint.HasValue) extremity.previousPoint = extremity.currentPoint;
 			extremity.currentPoint = null;
 		}
	}

	private IEnumerator PointsIterator()
	{
		Vector3 direction = Vector3.zero;
		float distance = 0.25f;

		while(true)
		{
			foreach(Vector3 point in points)
			{
				direction = vehicle.transform.position - point;

				while(direction.sqrMagnitude > distance)
				{
					Vector3 force = vehicle.GetSeekForce(point);
					vehicle.ApplyForce(force);
					vehicle.Displace(Time.deltaTime);
					vehicle.Rotate(Time.deltaTime);
					direction = vehicle.transform.position - point;
					yield return null;
				}
			}
		}
	}

	public override string ToString()
	{
		return pointsSet != null ? pointsSet.HashSetToString() : string.Empty;
	}
}