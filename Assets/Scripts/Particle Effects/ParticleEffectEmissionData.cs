using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.AddressableAssets;

namespace Flamingo
{
[Serializable]
public class ParticleEffectEmissionData
{
	[SerializeField] private Transform _transform; 						/// <summary>Transform's Reference.</summary>
	[SerializeField] private VAssetReference _particleEffectReference; 	/// <summary>Particle Effect's Reference.</summary>
	[SerializeField] private Vector3[] _points; 						/// <summary>Points of ParticleEmission relative to the Transform.</summary>

	/// <summary>Gets and Sets transform property.</summary>
	public Transform transform
	{
		get { return _transform; }
		set { _transform = value; }
	}

	/// <summary>Gets and Sets particleEffectReference property.</summary>
	public VAssetReference particleEffectReference
	{
		get { return _particleEffectReference; }
		set { _particleEffectReference = value; }
	}

	/// <summary>Gets and Sets points property.</summary>
	public Vector3[] points
	{
		get { return _points; }
		set { _points = value; }
	}

	/// <summary>Draws Gizmos [Editor Mode Only].</summary>
	public void DrawGizmos()
	{
#if UNITY_EDITOR
		if(transform == null || points == null) return;

		Gizmos.color = Color.white.WithAlpha(0.5f);

		foreach(Vector3 point in points)
		{
			Gizmos.DrawSphere(transform.TransformPoint(point), 0.05f);
		}
#endif
	}

	/// <summary>ParticleEffectEmissionData default constructor.</summary>
	/// <param name="_transform">Transform's Reference.</param>
	/// <param name="_particleEffectReference">ParticleEffect's Reference.</param>
	/// <param name="_points">Spawn Points [relative to the Transform].</param>
	public ParticleEffectEmissionData(Transform _transform, VAssetReference _particleEffectReference, params Vector3[] _points)
	{
		transform = _transform;
		particleEffectReference = _particleEffectReference;
		points = _points;
	}

	/// <summary>Emits ParticleEffects.</summary>
	/// <param name="scale">Particle Effect's Scale [1.0f by default].</param>
	public void EmitParticleEffects(float scale = 1.0f)
	{
		if(transform == null) return;

		if(points != null) foreach(Vector3 point in points)
		{
			PoolManager.RequestParticleEffect(particleEffectReference, transform.TransformPoint(point), Quaternion.identity, scale);
		}
		else PoolManager.RequestParticleEffect(particleEffectReference, transform.position, Quaternion.identity, scale);
	}
}
}