using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum ShootingOrder
{
	SteeringSnake,
	LeftToRight,
	RightToLeft,
	LeftAndRightOscillation
}

[RequireComponent(typeof(SteeringSnake))]
public class ChariotBehavior : DestinoScriptableCoroutine
{
	[SerializeField] private Vector3 _projectileSpawnPosition; 			/// <summary>Projectiles' Spawn Position.</summary>
	[Header("Projectiles:")]
	[SerializeField] private int _petrolSphereID; 						/// <summary>Petrol Sphere's ID.</summary>
	[SerializeField] private int _marbleSphereID; 						/// <summary>Marble Sphere's ID.</summary>
	[Space(5f)]
	[Header("Projectiles' Distribution Settings:")]
	[SerializeField] private IntRange _sequenceLimits; 					/// <summary>Sequence's Limits.</summary>
	[SerializeField] private float _spheresAccomodationDuration; 		/// <summary>Sphere's Accomodation Duration.</summary>
	[SerializeField] private float _sphereSpace; 						/// <summary>Space between spheres when they spawn.</summary>
	[Space(5f)]
	[SerializeField] private bool _randomizeOrder; 						/// <summary>Randomize Order?.</summary>
	private SteeringSnake _steeringSnake; 								/// <summary>SteeringSnake's Component.</summary>
	private Projectile[] _spheres; 										/// <summary>Spheres' Projectiles.</summary>
	private int _sequenceLength; 										/// <summary>Sequence's Length.</summary>

	/// <summary>Gets projectileSpawnPosition property.</summary>
	public Vector3 projectileSpawnPosition { get { return _projectileSpawnPosition; } }

	/// <summary>Gets petrolSphereID property.</summary>
	public int petrolSphereID { get { return _petrolSphereID; } }

	/// <summary>Gets marbleSphereID property.</summary>
	public int marbleSphereID { get { return _marbleSphereID; } }

	/// <summary>Gets sequenceLimits property.</summary>
	public IntRange sequenceLimits { get { return _sequenceLimits; } }

	/// <summary>Gets spheresAccomodationDuration property.</summary>
	public float spheresAccomodationDuration { get { return _spheresAccomodationDuration; } }

	/// <summary>Gets sphereSpace property.</summary>
	public float sphereSpace { get { return _sphereSpace; } }

	/// <summary>Gets randomizeOrder property.</summary>
	public bool randomizeOrder { get { return _randomizeOrder; } }

	/// <summary>Gets steeringSnake Component.</summary>
	public SteeringSnake steeringSnake
	{ 
		get
		{
			if(_steeringSnake == null) _steeringSnake = GetComponent<SteeringSnake>();
			return _steeringSnake;
		}
	}

	/// <summary>Gets and Sets spheres property.</summary>
	public Projectile[] spheres
	{
		get { return _spheres; }
		set { _spheres = value; }
	}

	/// <summary>Gets and Sets sequenceLength property.</summary>
	public int sequenceLength
	{
		get { return _sequenceLength; }
		set { _sequenceLength = value; }
	}

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
		base.DrawGizmos();

		Gizmos.DrawWireSphere(projectileSpawnPosition, 0.25f);
	}

	/// <summary>Callback invoked when a Projectile is destroyed.</summary>
	/// <param name="_poolObject">Projectile, as IPoolObject that was destroyed.</param>
	private void OnContactWeaponDeactivated(ContactWeapon _contactWeapon, DeactivationCause _cause, Trigger2DInformation _info)
	{
		Projectile projectile = _contactWeapon as Projectile;
		
		if(projectile == null) return;

		sequenceLength--;
		Game.RemoveTargetToCamera(projectile.cameraTarget);
	}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{
		if(spheres != null) foreach(Projectile sphere in spheres)
		{
			sphere.eventsHandler.onContactWeaponDeactivated -= OnContactWeaponDeactivated;
		}

		sequenceLength = sequenceLimits.Random();
		spheres = new Projectile[sequenceLength];

		float durationSplit = (spheresAccomodationDuration) / (1.0f * sequenceLength);
		float inverseSplit = 1.0f / durationSplit;
		float t = 0.0f;
		HashSet<int> indexSet = new HashSet<int>();
		Vector3 center = projectileSpawnPosition;

		for(int i = 0; i < sequenceLength; i++)
		{
			/// Distribute the Random Sequence:
			Projectile sphere = null; 
			int index = Random.Range(0, 2) == 0 ? petrolSphereID : marbleSphereID;
			indexSet.Add(index);

			if(i == (sequenceLength - 1))
			{ /// At the final iteration. Evaluate if the sequence has at least one of each sphere:
				if(!indexSet.Contains(petrolSphereID)) index = petrolSphereID;
				if(!indexSet.Contains(marbleSphereID)) index = marbleSphereID;
			}

			sphere = PoolManager.RequestHomingProjectile(Faction.Enemy, index, center, Vector3.zero, null);
			sphere.eventsHandler.onContactWeaponDeactivated += OnContactWeaponDeactivated;
			sphere.gameObject.name += ("_" + i);
			sphere.activated = false;
			sphere.ActivateHitBoxes(false);
			spheres[i] = sphere;

			Game.AddTargetToCamera(sphere.cameraTarget);

			while(t < 1.0f)
			{
				for(int j = 0; j < (i + 1); j++)
				{
					sphere = spheres[j];

					float space = ((float)i * sphereSpace) * 0.5f;
					Vector3 position = center + (Vector3.left * space) + (Vector3.right * ((float)j * sphereSpace));
					sphere.transform.position = Vector3.Lerp(sphere.transform.position, position, t);
				}

				t += (Time.deltaTime * inverseSplit);
				
				yield return null;
			}

			t = 0.0f;
		}

		foreach(Projectile sphere in spheres)
		{
			sphere.projectileType = ProjectileType.Homing;
			sphere.activated = true;
			sphere.ActivateHitBoxes(true);
		}

		steeringSnake.InitializeLinkedList(Game.mateo.transform, spheres);

		while(sequenceLength > 0)
		{
			yield return VCoroutines.WAIT_PHYSICS_THREAD;

			foreach(Projectile sphere in spheres)
			{
				DestinoSceneController.Instance.boundaries.ContainInsideBoundaries(sphere.rigidbody);
			}
		}

		InvokeCoroutineEnd();
	}
}
}