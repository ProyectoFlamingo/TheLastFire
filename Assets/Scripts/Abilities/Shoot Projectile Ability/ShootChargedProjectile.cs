using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class ShootChargedProjectile : ShootProjectile
{
	public const int STATE_ID_UNCHARGED = 0; 									/// <summary>Uncharged's State ID.</summary>
	public const int STATE_ID_CHARGING = 1; 									/// <summary>Charging's State ID.</summary>
	public const int STATE_ID_CHARGED = 2; 										/// <summary>Fully Charged's State ID.</summary>
	public const int STATE_ID_RELEASED = 3; 									/// <summary>Charge Released's State ID.</summary>

	[Space(5f)]
	[Header("ShootChargedProjectile's Attributes:")]
	[SerializeField] private FloatRange _speedRange; 							/// <summary>Projectile's Speed Range.</summary>
	[SerializeField] private CollectionIndex _chargedProjectileID; 				/// <summary>Charged Projectile's ID.</summary>
	[SerializeField] private float _minimumCharge; 								/// <summary>Minimum charge required for the projectile to be propeled.</summary>
	[SerializeField] private float _chargeDuration; 							/// <summary>Charge's Duration.</summary>
	[SerializeField] private float _speedChargeDuration; 						/// <summary>Speed Charge's Duration.</summary>
	[Space(5f)]
	[Header("Sounds' Settings:")]
	[SerializeField] private CollectionIndex _projectileCreationSoundIndex; 	/// <summary>Projectile Creation's Sound Index.</summary>
	[SerializeField] private CollectionIndex _maxChargeSoundIndex; 				/// <summary>Max Charge's Sound Index.</summary>
	[SerializeField] private CollectionIndex _releaseSoundIndex; 				/// <summary>Shoot Release's Sound Index.</summary>
	private float _currentCharge; 												/// <summary>Current Charge's Value.</summary>
	private float _currentSpeedCharge; 											/// <summary>Current Speed Charge's Value.</summary>
	private CollectionIndex _ID; 												/// <summary>Current Projectile's ID.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets speedRange property.</summary>
	public FloatRange speedRange
	{
		get { return _speedRange; }
		set { _speedRange = value; }
	}

	/// <summary>Gets and Sets chargedProjectileID property.</summary>
	public CollectionIndex chargedProjectileID
	{
		get { return _chargedProjectileID; }
		set { _chargedProjectileID = value; }
	}

	/// <summary>Gets and Sets minimumCharge property.</summary>
	public float minimumCharge
	{
		get { return _minimumCharge; }
		set { _minimumCharge = value; }
	}

	/// <summary>Gets and Sets chargeDuration property.</summary>
	public float chargeDuration
	{
		get { return _chargeDuration; }
		set { _chargeDuration = value; }
	}

	/// <summary>Gets and Sets speedChargeDuration property.</summary>
	public float speedChargeDuration
	{
		get { return _speedChargeDuration; }
		set { _speedChargeDuration = value; }
	}

	/// <summary>Gets and Sets currentCharge property.</summary>
	public float currentCharge
	{
		get { return _currentCharge; }
		set { _currentCharge = value; }
	}

	/// <summary>Gets and Sets currentSpeedCharge property.</summary>
	public float currentSpeedCharge
	{
		get { return _currentSpeedCharge; }
		set { _currentSpeedCharge = value; }
	}

	/// <summary>Gets and Sets projectileCreationSoundIndex property.</summary>
	public CollectionIndex projectileCreationSoundIndex
	{
		get { return _projectileCreationSoundIndex; }
		set { _projectileCreationSoundIndex = value; }
	}

	/// <summary>Gets and Sets maxChargeSoundIndex property.</summary>
	public CollectionIndex maxChargeSoundIndex
	{
		get { return _maxChargeSoundIndex; }
		set { _maxChargeSoundIndex = value; }
	}

	/// <summary>Gets and Sets releaseSoundIndex property.</summary>
	public CollectionIndex releaseSoundIndex
	{
		get { return _releaseSoundIndex; }
		set { _releaseSoundIndex = value; }
	}

	/// <summary>Gets and Sets ID property.</summary>
	public CollectionIndex ID
	{
		get { return _ID; }
		set { _ID = value; }
	}
#endregion

	/// <summary>Gets fullyCharged property.</summary>
	public bool fullyCharged { get { return currentCharge >= chargeDuration; } }

	/// <summary>Resets 's instance to its default values.</summary>
	private void Reset()
	{
		currentCharge = 0.0f;
		currentSpeedCharge = 0.0f;
		ID = projectileID;

		if(projectile != null)
		{
			projectile.OnObjectDeactivation();
			projectile = null;
		}
	}

	/// <summary>Callback internally called aftrer Awake.</summary>
	protected override void OnAwake()
	{
		Reset();
	}

	/// <summary>Callback invoked when the shot is charging.</summary>
	/// <param name="_axes">Additional Axes' Argument.</param>
	/// <returns>State's ID of the charge.</returns>
	public int OnCharge(Vector3 _axes)
	{
		if(onCooldown)
		{
			OnDischarge();
			return STATE_ID_RELEASED;
		}

		if(currentCharge == 0.0f && muzzle != null) CreateProjectile();

		currentCharge += Time.deltaTime;
		currentCharge = Mathf.Min(currentCharge, chargeDuration);

		if(currentCharge >= minimumCharge)
		{
			currentSpeedCharge += Time.deltaTime;
			currentSpeedCharge = Mathf.Min(currentSpeedCharge, speedChargeDuration);
		}

		if(currentCharge >= chargeDuration && ID != chargedProjectileID)
		{
			ID = chargedProjectileID;

			if(muzzle != null)
			{
				if(projectile != null)
				{
					projectile.transform.parent = null;
					projectile.OnObjectDeactivation();
				}

				CreateProjectile();
			}
		}

		if(projectile != null) projectile.transform.UpLookAt(projectile.transform.position + _axes);

		return fullyCharged ? STATE_ID_CHARGED : STATE_ID_CHARGING;
	}

	/// <summary>Creates Projectile and parents it to the muzzle [if such exists].</summary>
	public void CreateProjectile()
	{
		if(ID == projectileID) AudioController.PlayOneShot(SourceType.SFX, 0, projectileCreationSoundIndex);
		else if(ID == chargedProjectileID) AudioController.PlayOneShot(SourceType.SFX, 0, maxChargeSoundIndex);

		projectile = PoolManager.RequestProjectile(faction, ID, muzzle.position, Vector3.zero);
		projectile.transform.parent = muzzle;
		projectile.activated = false;
		projectile.ActivateHitBoxes(false);
	}

	/// <summary>Callback invoked when this shot must be discharged.</summary>
	public void OnDischarge()
	{
		Reset();
	}

	/// <summary>Shoots Projectile from pool of given Projectile's ID.</summary>
	/// <param name="_origin">Shoot's Origin.</param>
	/// <param name="_direction">Shoot's Direction.</param>
	/// <returns>True if projectile could be shot.</returns>
	public override bool Shoot(Vector3 _origin, Vector3 _direction, float? _speed = default(float?))
	{
		if(projectile != null)
		{
			projectile.transform.parent = null;
			projectile.OnObjectDeactivation();
		}

		if(currentCharge < minimumCharge) return false;

		AudioController.PlayOneShot(SourceType.SFX, 0, releaseSoundIndex);

		float t = currentSpeedCharge / speedChargeDuration;

		return Shoot(ID, _origin, _direction, speedRange.Lerp(t));
	}
}
}