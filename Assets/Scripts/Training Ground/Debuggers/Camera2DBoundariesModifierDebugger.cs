using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class Camera2DBoundariesModifierDebugger : MonoBehaviour
{
	[SerializeField] private LineRenderer _lineRenderer; 				/// <summary>LineRenderer's Prefab Reference.</summary>
	[SerializeField] private Camera2DBoundariesModifier[] _modifiers; 	/// <summary>Camera Boundaries' Modifiers.</summary>
	private List<LineRenderer> _lineRenderers; 								/// <summary>LineRenderers associated to each Camera2DBoundariesModifier.</summary>

	/// <summary>Gets lineRenderer property.</summary>
	public LineRenderer lineRenderer { get { return _lineRenderer; } }

	/// <summary>Gets and Sets modifiers property.</summary>
	public Camera2DBoundariesModifier[] modifiers
	{
		get { return _modifiers; }
		set { _modifiers = value; }
	}

	/// <summary>Gets and Sets lineRenderers property.</summary>
	public List<LineRenderer> lineRenderers
	{
		get { return _lineRenderers; }
		set { _lineRenderers = value; }
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		UpdateLineRenderers();
	}

	/// <summary>Camera2DBoundariesModifierDebugger's instance initialization.</summary>
	private void Awake()
	{
		CreateLineRenderers();
	}

	/// <summary>Updates LineRenderers.</summary>
	private void UpdateLineRenderers()
	{
		if(modifiers == null) return;

		int count = modifiers.Length;
		int length = lineRenderers == null ? 0 : lineRenderers.Count;
		
		if((count - length) != 0 && count > 0) CreateLineRenderers();

		for(int i = 0; i < count; i++)
		{
			if(modifiers[i] == null) continue;

			Boundaries2DContainer container = modifiers[i].boundariesContainer;
			LineRenderer renderer = lineRenderers[i];

			if(renderer == null)
			{
				renderer = Instantiate(lineRenderer, Vector3.zero, Quaternion.identity) as LineRenderer;
				lineRenderers[i] = renderer;
				renderer.transform.parent = transform;
			}

			Vector3 min = container.min;
			Vector3 max = container.max;
			Vector3 a = min;
			Vector3 b = new Vector3(min.x, max.y, min.z);
			Vector3 c = max;
			Vector3 d = new Vector3(max.x, min.y, max.z);

			renderer.SetPosition(0, a);
			renderer.SetPosition(1, b);
			renderer.SetPosition(2, c);
			renderer.SetPosition(3, d);
		}
	}

	/// <summary>Fetches for all Camera2DBoundariesModifiers.</summary>
	public void FetchCamera2DBoundariesModifiers()
	{
		modifiers = FindObjectsOfType<Camera2DBoundariesModifier>();
	}

	/// <summary>Creates LineRenderers.</summary>
	public void CreateLineRenderers()
	{
		if(lineRenderers == null) lineRenderers = new List<LineRenderer>();

		int length = modifiers.Length;
		int count = lineRenderers.Count;
		int difference = length - count;

		if(difference == 0)
		{
			return;

		} else if(difference > 0)
		{
			for(int i = 0; i < difference; i++)
			{
				LineRenderer renderer = Instantiate(lineRenderer, Vector3.zero, Quaternion.identity) as LineRenderer;
				lineRenderers.Add(renderer);
				renderer.transform.parent = transform;
			}

		} else if (difference < 0)
		{
			difference = Mathf.Abs(difference);
			int x = Mathf.Max(count - difference - 1, 0);

			for(int i = count - 1; i > x - 1; i--)
			{
				DestroyImmediate(lineRenderers[i].gameObject);
			}
				
			lineRenderers.RemoveRange(x, difference);
		}

	}
}
}