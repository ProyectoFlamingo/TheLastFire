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
	[SerializeField] private AssetReference _particleEffectReference; 	/// <summary>Particle Effect's Reference.</summary>
	[SerializeField] private int _particleEffectIndex; 					/// <summary>ParticleEffect's Index.</summary>
	[SerializeField] private Vector3[] _points; 						/// <summary>Points of ParticleEmission relative to the Transform.</summary>

	/// <summary>Gets and Sets transform property.</summary>
	public Transform transform
	{
		get { return _transform; }
		set { _transform = value; }
	}

	/// <summary>Gets and Sets particleEffectReference property.</summary>
	public AssetReference particleEffectReference
	{
		get { return _particleEffectReference; }
		set { _particleEffectReference = value; }
	}

	/// <summary>Gets and Sets particleEffectIndex property.</summary>
	public int particleEffectIndex
	{
		get { return _particleEffectIndex; }
		set { _particleEffectIndex = value; }
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
	/// <param name="_particleEffectIbdex">ParticleEffect's Index.</param>
	/// <param name="_points">Spawn Points [relative to the Transform].</param>
	public ParticleEffectEmissionData(Transform _transform, int _particleEffectIndex, params Vector3[] _points)
	{
		transform = _transform;
		particleEffectIndex = _particleEffectIndex;
		points = _points;
	}

	/// <summary>Emits ParticleEffects.</summary>
	public void EmitParticleEffects()
	{
		if(transform == null || points == null) return;

		foreach(Vector3 point in points)
		{
			/*if(particleEffectReference.Empty()) PoolManager.RequestParticleEffect(particleEffectReference, transform.TransformPoint(point), Quaternion.identity);
			else */PoolManager.RequestParticleEffect(particleEffectIndex, transform.TransformPoint(point), Quaternion.identity);
		}	
	}
}
}