using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

using Random = UnityEngine.Random;

namespace Flamingo
{
public class DevilTower : Character
{
	[SerializeField] private int _projectileIndex; 									/// <summary>Arrow Projectile's Index.</summary>
	[SerializeField] private Vector3[] _muzzles; 									/// <summary>Muzzles.</summary>
	[SerializeField] private ParticleEffectEmissionData _landingParticleEffect; 	/// <summary>Landing Particle Effect's Emission Data.</summary>
	private Dictionary<int, ArrowProjectile> _indexProjectileMapping; 				/// <summary>Mapping of Index with arrow Projectile.</summary>
	private Dictionary<int, int> _projectileIDIndexMapping; 						/// <summary>Mapping of Projectile IDs with Index.</summary>

	/// <summary>Gets projectileIndex property.</summary>
	public int projectileIndex { get { return _projectileIndex; } }

	/// <summary>Gets muzzles property.</summary>
	public Vector3[] muzzles { get { return _muzzles; } }

	/// <summary>Gets landingParticleEffect property.</summary>
	public ParticleEffectEmissionData landingParticleEffect { get { return _landingParticleEffect; } }

	/// <summary>Gets and Sets indexProjectileMapping property.</summary>
	public Dictionary<int, ArrowProjectile> indexProjectileMapping
	{
		get { return _indexProjectileMapping; }
		private set { _indexProjectileMapping = value; }
	}

	/// <summary>Gets and Sets projectileIDIndexMapping property.</summary>
	public Dictionary<int, int> projectileIDIndexMapping
	{
		get { return _projectileIDIndexMapping; }
		private set { _projectileIDIndexMapping = value; }
	}

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

	/// <summary>DevilTower's instance initialization when loaded [Before scene loads].</summary>
	protected override void Awake()
	{
		base.Awake();
		indexProjectileMapping = new Dictionary<int, ArrowProjectile>();
		projectileIDIndexMapping = new Dictionary<int, int>();
		health.inmunities = new GameObjectTag[] { Game.data.playerWeaponTag, Game.data.playerProjectileTag };

		for(int i = 0; i < muzzles.Length; i++)
		{
			indexProjectileMapping.Add(i, null);
		}
	}

	/// <summary>Shoots Arrow Projectile.</summary>
	/// <param name="_ray">Ray's Parameter.</param>
	/// <returns>Whether it shoot an arrow [true if it did].</returns>
	public bool ShootArrow(Vector3 _point)
	{
		if(muzzles == null) return false;

		int index = 0;

		if(!GetRandomMuzzleIndex(out index)) return false;

		Vector3 origin = GetMuzzlePoint(index);
		Vector3 direction = _point - origin;
		//direction.z = origin.z;

		ArrowProjectile projectile = PoolManager.RequestProjectile(Faction.Enemy, projectileIndex, origin, direction) as ArrowProjectile;
		
		if(projectile == null) return false;

		int instanceID = projectile.GetInstanceID();

		projectile.impactTags = null;
		projectile.healthAffectableTags = new GameObjectTag[] { Game.data.playerTag };
		projectile.transform.rotation = VQuaternion.RightLookRotation(direction);
		projectile.eventsHandler.onContactWeaponIDEvent -= OnArrowProjectileEvent;
		projectile.eventsHandler.onContactWeaponIDEvent += OnArrowProjectileEvent;
		projectile.eventsHandler.onContactWeaponDeactivated -= OnProjectileDeactivated;
		projectile.eventsHandler.onContactWeaponDeactivated += OnProjectileDeactivated;

		indexProjectileMapping[index] = projectile;
		projectileIDIndexMapping.Add(instanceID, index);

		return true;
	}

	/// <returns>True if it has at least one available muzzle.</returns>
	public bool HasAvailableMuzzle()
	{
		foreach(ArrowProjectile projectile in indexProjectileMapping.Values)
		{
			if(projectile == null) return true;
		}

		return false;
	}

	/// <summary>Tries to get a random muzzle index out of the available ones.</summary>
	/// <param name="index">Index's reference.</param>
	/// <returns>Whether it was an available muzzle.</returns>
	private bool GetRandomMuzzleIndex(out int index)
	{
		List<int> availableIndices = new List<int>();
		index = -1;

		foreach(KeyValuePair<int, ArrowProjectile> pair in indexProjectileMapping)
		{
			if(pair.Value == null) availableIndices.Add(pair.Key);
		}

		if(availableIndices.Count == 0) return false;
		else index = availableIndices.Random();

		return true; 
	}

	/// <returns>Muzzle's Point [relative to the Tower's Transform].</returns>
	private Vector3 GetMuzzlePoint(int index)
	{
		return transform.TransformPoint(muzzles[index]);
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
			ArrowProjectile projectile = _projectile as ArrowProjectile;

			if(projectile == null) return;

			GameObjectTag[] tags = new GameObjectTag[] { Game.data.enemyTag };

			projectile.impactTags = tags;
			projectile.healthAffectableTags = tags;
			/*health.GiveDamage(_projectile.damage);
			Debug.Log("[DevilTower] Me taking too much damage. Current HP: " + health.ToString());*/
			break;
		}

		//Debug.Log("[DevilTower] Invoked ArrowProjectile's ID Event: " + _ID);
	}

	/// <summary>Event invoked when the ContactWeapon is deactivated.</summary>
	/// <param name="_contactWeapon">ContactWeapon that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	private void OnProjectileDeactivated(ContactWeapon _contactWeapon, DeactivationCause _cause, Trigger2DInformation _info)
	{
		ArrowProjectile projectile = _contactWeapon as ArrowProjectile;

		if(projectile == null) return;

		int instanceID = projectile.GetInstanceID();

		if(!projectileIDIndexMapping.ContainsKey(instanceID)) return;
		
		int index = projectileIDIndexMapping[instanceID];
		indexProjectileMapping[index] = null;
		projectileIDIndexMapping.Remove(instanceID);
	}

	/// <summary>Callback invoked when the object is deactivated.</summary>
	public override void OnObjectDeactivation()
	{
		base.OnObjectDeactivation();

		foreach(ArrowProjectile projectile in indexProjectileMapping.Values)
		{
			if(projectile != null) projectile.OnObjectDeactivation();
		}

		indexProjectileMapping.Clear();
		projectileIDIndexMapping.Clear();

		for(int i = 0; i < muzzles.Length; i++)
		{
			indexProjectileMapping.Add(i, null);
		}
	}
}
}