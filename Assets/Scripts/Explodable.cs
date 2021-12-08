using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// \TODO Make a DamageApplier Component:
/*
	- That component must be implemented by the Weapons
	- That component must be implemented by the Projectiles
	- Implemented by this Component.
*/

public class Explodable : PoolGameObject
{
	[SerializeField] private LayerMask _healthAffectableMask; 		/// <summary>Mask that contains GameObjects affected by the explosion.</summary>
	[SerializeField] private GameObjectTag[] _affectableTags; 		/// <summary>Tags of GameObjects potentially affected by the explosion.</summary>
	[SerializeField] private int _particleEffectIndex; 				/// <summary>Particle Effect's Index.</summary>
	[SerializeField] private int _soundEffectIndex; 				/// <summary>Sound Effect's Index.</summary>
	[SerializeField] private float _radius; 						/// <summary>Blast's Radius.</summary>
	[SerializeField] private float _expansionDuration; 				/// <summary>Radius Expansion's Duration.</summary>
	[SerializeField] private float _maxRadiusDuration; 				/// <summary>How much does the explosion at its maximum radius lasts.</summary>
	[SerializeField] private float _damage; 						/// <summary>Damage that this explosion applies.</summary>
	private float _currentRadius; 									/// <summary>Current Radius' Value.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 					/// <summary>Gizmos' Color.</summary>
#endif
	private Coroutine explosionExpansion; 							/// <summary>Explosion's Coroutine reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets healthAffectableMask property.</summary>
	public LayerMask healthAffectableMask
	{
		get { return _healthAffectableMask; }
		set { _healthAffectableMask = value; }
	}

	/// <summary>Gets and Sets affectableTags property.</summary>
	public GameObjectTag[] affectableTags
	{
		get { return _affectableTags; }
		set { _affectableTags = value; }
	}

	/// <summary>Gets and Sets particleEffectIndex property.</summary>
	public int particleEffectIndex
	{
		get { return _particleEffectIndex; }
		set { _particleEffectIndex = value; }
	}

	/// <summary>Gets and Sets soundEffectIndex property.</summary>
	public int soundEffectIndex
	{
		get { return _soundEffectIndex; }
		set { _soundEffectIndex = value; }
	}

	/// <summary>Gets and Sets radius property.</summary>
	public float radius
	{
		get { return _radius; }
		set { _radius = value; }
	}

	/// <summary>Gets and Sets expansionDuration property.</summary>
	public float expansionDuration
	{
		get { return _expansionDuration; }
		set { _expansionDuration = value; }
	}

	/// <summary>Gets and Sets maxRadiusDuration property.</summary>
	public float maxRadiusDuration
	{
		get { return _maxRadiusDuration; }
		set { _maxRadiusDuration = value; }
	}

	/// <summary>Gets and Sets damage property.</summary>
	public float damage
	{
		get { return _damage; }
		set { _damage = value; }
	}

	/// <summary>Gets and Sets currentRadius property.</summary>
	public float currentRadius
	{
		get { return _currentRadius; }
		set { _currentRadius = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;

		Gizmos.DrawSphere(transform.position, !Application.isPlaying ? radius : currentRadius);
	}
#endif

	/// <summary>Performs the explosion.</summary>
	/// <param name="onExplosionEnds">Optional callback invoked when the explosion expansion reaches its end [null by default].</param>
	public void Explode(Action onExplosionEnds = null)
	{
		if(explosionExpansion != null) return;

		PoolManager.RequestParticleEffect(particleEffectIndex, transform.position, transform.rotation);
		AudioController.PlayOneShot(SourceType.SFX, 0,soundEffectIndex);
		this.StartCoroutine(ExplosionExpansion(onExplosionEnds), ref explosionExpansion);
	}

	/// <summary>Resets Explosion.</summary>
	public void Reset()
	{
#if UNITY_EDITOR
		gizmosColor = Color.white.WithAlpha(0.2f);
#endif

		currentRadius = 0.0f;
		this.DispatchCoroutine(ref explosionExpansion);
	}

	/// <summary>Evaluates the explosion given its current radius [passed as parameter].</summary>
	/// <param name="r">Current Explosion's Radius.</param>
	private void Evaluate(float r)
	{
		if(affectableTags == null || affectableTags.Length == 0) return;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, r, healthAffectableMask);

		if(colliders == null || colliders.Length == 0) return;

		GameObject obj = null;
		Health health = null;

		foreach(Collider2D collider in colliders)
		{
			obj = collider.gameObject;

			foreach(GameObjectTag tag in affectableTags)
			{
				if(obj.CompareTag(tag))
				{
					health = obj.GetComponentInParent<Health>();

					if(health == null)
					{
						HealthLinker linker = collider.GetComponent<HealthLinker>();
						if(linker != null) health = linker.component;
					}

					if(health != null) health.GiveDamage(damage, true, true, gameObject);

					return;
				}
			}
		}
	}

	/// <summary>Explosion Expansion's Coroutine.</summary>
	/// <param name="onExplosionEnds">Optional callback invoked when the explosion expansion reaches its end [null by default].</param>
	private IEnumerator ExplosionExpansion(Action onExplosionEnds = null)
	{
		if(expansionDuration > 0.0f)
		{
			float t = 0.0f;
			float inverseDuration = 1.0f / expansionDuration;
			currentRadius = 0.0f;

			while(t < 1.0f)
			{
				currentRadius = Mathf.Lerp(0.0f, radius, t);
				Evaluate(currentRadius);
				t += (Time.deltaTime * inverseDuration);
				yield return null;
			}
		}

		currentRadius = radius;

		if(maxRadiusDuration > 0.0f)
		{
			SecondsDelayWait wait = new SecondsDelayWait(maxRadiusDuration);
			
			while(wait.MoveNext())
			{
				Evaluate(radius);
				yield return null;
			}
		}

		Evaluate(radius);
		if(onExplosionEnds != null) onExplosionEnds();
		OnObjectDeactivation();
		Reset();
	}
}
}