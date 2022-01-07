using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class DevilTower : Character
{
	[SerializeField] private int _projectileIndex; 									/// <summary>Arrow Projectile's Index.</summary>
	[SerializeField] private Vector3[] _muzzles; 									/// <summary>Muzzles.</summary>
	[SerializeField] private ParticleEffectEmissionData _landingParticleEffect; 	/// <summary>Landing Particle Effect's Emission Data.</summary>

	/// <summary>Gets projectileIndex property.</summary>
	public int projectileIndex { get { return _projectileIndex; } }

	/// <summary>Gets muzzles property.</summary>
	public Vector3[] muzzles { get { return _muzzles; } }

	/// <summary>Gets landingParticleEffect property.</summary>
	public ParticleEffectEmissionData landingParticleEffect { get { return _landingParticleEffect; } }

	/// <summary>Draws Gizmos on Editor mode when DevilTower's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta.WithAlpha(0.5f);

		if(muzzles != null) foreach(Vector3 muzzle in muzzles)
		{
			Gizmos.DrawSphere(transform.TransformPoint(muzzle), 0.5f);
		}

		if(landingParticleEffect != null) landingParticleEffect.DrawGizmos();
	}

	/// <summary>Shoots Arrow Projectile.</summary>
	/// <param name="_ray">Ray's Parameter.</param>
	public void ShootArrow(Vector3 _point)
	{
		Vector3 origin = RandomMuzzlePoint();
		Vector3 direction = _point - origin;
		//direction.z = origin.z;

		ArrowProjectile projectile = PoolManager.RequestProjectile(Faction.Enemy, projectileIndex, origin, direction) as ArrowProjectile;
		
		if(projectile == null) return;

		projectile.transform.rotation = VQuaternion.RightLookRotation(direction);
		projectile.eventsHandler.onContactWeaponIDEvent -= OnArrowProjectileEvent;
		projectile.eventsHandler.onContactWeaponIDEvent += OnArrowProjectileEvent;
	}

	/// <returns>Random Muzzle's point relative to this Trasnform.</returns>
	public Vector3 RandomMuzzlePoint()
	{
		return transform.TransformPoint(muzzles.Random());
	}


	/// <summary>Event invoked when a ContactWeapon ID Event occurs.</summary>
	/// <param name="_projectile">ArrowProjectile that was inverted.</param>
	/// <param name="_eventID">Event's ID.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	private void OnArrowProjectileEvent(ContactWeapon _projectile, int _ID, Trigger2DInformation _info)
	{
		switch(_ID)
		{
			case IDs.EVENT_REPELLED:
			health.GiveDamage(_projectile.damage);
			Debug.Log("[DevilTower] Me taking too much damage. Current HP: " + health.ToString());
			break;
		}

		Debug.Log("[DevilTower] Invoked ArrowProjectile's ID Event: " + _ID);
	}
}
}