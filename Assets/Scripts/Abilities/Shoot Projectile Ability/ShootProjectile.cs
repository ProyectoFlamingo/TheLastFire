using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class ShootProjectile : MonoBehaviour
{
	[SerializeField] private Transform _muzzle; 					/// <summary>Muzzle's Transform.</summary>
	[SerializeField] private Faction _faction; 						/// <summary>Shooter's Faction.</summary>
	[SerializeField] private VAssetReference _projectileReference; 	/// <summary>Projectile's Reference.</summary>
	[SerializeField] private int _projectileID; 					/// <summary>Pprojectile's ID.</summary>
	private Projectile _projectile; 								/// <summary>Projectile to shoot.</summary>
	private Cooldown _cooldown; 									/// <summary>Cooldown's Reference.</summary>

	/// <summary>Gets and Sets muzzle property.</summary>
	public Transform muzzle
	{
		get { return _muzzle; }
		set { _muzzle = value; }
	}

	/// <summary>Gets and Sets faction property.</summary>
	public Faction faction
	{
		get { return _faction; }
		set { _faction = value; }
	}

	/// <summary>Gets and Sets projectileReference property.</summary>
	public VAssetReference projectileReference
	{
		get { return _projectileReference; }
		set { _projectileReference = value; }
	}

	/// <summary>Gets and Sets projectileID property.</summary>
	public int projectileID
	{
		get { return _projectileID; }
		set { _projectileID = value; }
	}

	/// <summary>Gets and Sets projectile property.</summary>
	public Projectile projectile
	{
		get { return _projectile; }
		protected set { _projectile = value; }
	}

	/// <summary>Gets and Sets cooldown property.</summary>
	public Cooldown cooldown
	{
		get { return _cooldown; }
		set { _cooldown = value; }
	}

	/// <summary>Gets onCooldown property.</summary>
	public bool onCooldown { get { return cooldown != null ? cooldown.onCooldown : true; } }

	/// <summary>ShootProjectile's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		cooldown = new Cooldown(this, 0.0f);
		OnAwake();
	}

	/// <summary>Callback internally called aftrer Awake.</summary>
	protected virtual void OnAwake() { /*...*/ }

	/// <summary>Shoots Projectile from pool of given ID.</summary>
	/// <param name="_reference">Projectile's Reference.</param>
	/// <param name="_origin">Shoot's Origin.</param>
	/// <param name="_direction">Shoot's Direction.</param>
	/// <returns>True if projectile could be shot.</returns>
	public bool Shoot(VAssetReference _reference, Vector3 _origin, Vector3 _direction, float? _speed = default(float?))
	{
		if(onCooldown) return false;

		//_direction *= 90.0f;
		//_direction.z = 0.0f;
		_origin.z = 0.0f;
		/*float angle = Vector2.SignedAngle(_origin, _direction);
		Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, angle /*Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg);*/

		if(projectile == null) projectile = PoolManager.RequestProjectile(faction, _reference, _origin, _direction);
		else projectile.OnObjectReset();
		
		projectile.direction = _direction;
		projectile.activated = true;
		projectile.ActivateHitBoxes(true);
		if(_speed.HasValue) projectile.speed = _speed.Value;
		//projectile.transform.right = _direction.normalized;

		float cooldownDuration = projectile.cooldownDuration;

		if(cooldownDuration <= 0.0f) return true;

		cooldown.duration = cooldownDuration;
		cooldown.Begin();

		projectile = null;

		return true;
	}

	/// <summary>Shoots Projectile from pool of given Projectile's ID.</summary>
	/// <param name="_origin">Shoot's Origin.</param>
	/// <param name="_direction">Shoot's Direction.</param>
	/// <returns>True if projectile could be shot.</returns>
	public virtual bool Shoot(Vector3 _origin, Vector3 _direction, float? _speed = default(float?))
	{
		return Shoot(projectileReference, _origin, _direction);
	}
}
}