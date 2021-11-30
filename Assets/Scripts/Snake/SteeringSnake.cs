using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

/// \TODO CLEAN THIS FUCKING MESS!!!!!
namespace Flamingo
{
public class SteeringSnake : MonoBehaviour
{
	[SerializeField] private float _duration; 				/// <summary>Duration.</summary>
	private float _time; 									/// <summary>Current Time.</summary>
	private LinkedList<Projectile> _projectilesList; 		/// <summary>Projectiles' LinkedList.</summary>
	private List<Projectile> _projectiles; 					/// <summary>Projectiles' List.</summary>
	private Transform _target; 								/// <summary>Target's Function.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets duration property.</summary>
	public float duration
	{
		get { return _duration; }
		set { _duration = value; }
	}

	/// <summary>Gets and Sets time property.</summary>
	public float time
	{
		get { return _time; }
		protected set { _time = value; }
	}

	/// <summary>Gets and Sets projectilesList property.</summary>
	public LinkedList<Projectile> projectilesList
	{
		get { return _projectilesList; }
		protected set { _projectilesList = value; }
	}

	/// <summary>Gets and Sets projectiles property.</summary>
	public List<Projectile> projectiles
	{
		get { return _projectiles; }
		set { _projectiles = value; }
	}

	/// <summary>Gets and Sets target property.</summary>
	public Transform target
	{
		get { return _target; }
		set { _target = value; }
	}
#endregion

	/// <summary>SteeringSnake's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		projectilesList = new LinkedList<Projectile>();
	}
	
	/// <summary>Snake's tick at each frame.</summary>
	private void Update ()
	{
		if(projectilesList == null || projectilesList.Count == 0) return;

		time += Time.deltaTime;
		if(time >= duration) SetLastPositions();
	}

	/// <summary>Initializes Steering Snake's Behavior.</summary>
	/// <param name="_projectiles">New set of projectiles that will compose the Steering Snake.</param>
	public void InitializeLinkedList(Transform _target, params Projectile[] _projectiles)
	{
		if(_projectiles == null) return;

		projectiles = _projectiles.ToList();
		projectilesList = new LinkedList<Projectile>(_projectiles);
		target = _target;
		Projectile p = null;

		foreach(Projectile projectile in projectilesList)
		{
			if(projectile != projectilesList.First.Value)
			{
				projectile.target = projectilesList.First.Value.transform;
				projectile.parentProjectile = p;
			}
			else
			{
				projectile.target = target;
				projectile.parentProjectile = null;
			}
			
			projectile.onPoolObjectDeactivation += OnProjectileDeactivated;
			projectile.projectileType = ProjectileType.Homing;
			p = projectile;
		}

		SetLastPositions();
	}

	/// <summary>Sets LinkedList's Projectiles' Last Positions.</summary>
	private void SetLastPositions(float _time = 0.0f)
	{
		time = _time;

		foreach(Projectile projectile in projectilesList)
		{
			projectile.lastPosition = projectile.transform.position;
		}
	}

	/// <summary>Callback invoked when a projectile [as IPoolObject] gets deactivated.</summary>
	/// <param name="_object">IPoolObject .</param>
	private void OnProjectileDeactivated(IPoolObject _object)
	{
		Projectile projectile = _object as Projectile;
		projectile.onPoolObjectDeactivation -= OnProjectileDeactivated;

		if(projectile == null) return;

		projectiles.Remove(projectile);
		projectilesList = new LinkedList<Projectile>(projectiles);
		SetLastPositions(time);

		Projectile p = null;

		foreach(Projectile proj in projectilesList)
		{
			if(proj == projectilesList.First.Value)
			{
				proj.target = target;
				proj.parentProjectile = null;
			}
			else
			{
				proj.target = p.transform;
				proj.parentProjectile = p;
			}

			p = proj;
		}
	}
}
}