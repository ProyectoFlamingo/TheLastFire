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

//[CreateAssetMenu]
[RequireComponent(typeof(SteeringSnake))]
public class ChariotBehavior : DestinoScriptableCoroutine
{
	[SerializeField] private Vector3 _projectileSpawnPosition; 			/// <summary>Projectiles' Spawn Position.</summary>
	[Header("Projectiles:")]
	[SerializeField] private CollectionIndex _petrolSphereID; 			/// <summary>Petrol Sphere's ID.</summary>
	[SerializeField] private CollectionIndex _marbleSphereID; 			/// <summary>Marble Sphere's ID.</summary>
	[SerializeField] private CollectionIndex _homingPetrolSphereID; 	/// <summary>Petrol Homing Sphere's ID.</summary>
	[SerializeField] private CollectionIndex _homingMarbleSphereID; 	/// <summary>Marble Homing Sphere's ID.</summary>
	[Space(5f)]
	[Header("Projectiles' Distribution Settings:")]
	[SerializeField] private ShootingOrder _order; 						/// <summary>Shooting's Order.</summary>
	[SerializeField] private IntRange _sequenceLimits; 					/// <summary>Sequence's Limits.</summary>
	[SerializeField] private float _spheresAccomodationDuration; 		/// <summary>Sphere's Accomodation Duration.</summary>
	[SerializeField] private float _sphereSpace; 						/// <summary>Space between spheres when they spawn.</summary>
	[Space(5f)]
	[SerializeField] private bool _randomizeOrder; 						/// <summary>Randomize Order?.</summary>
	private SteeringSnake _steeringSnake; 								/// <summary>SteeringSnake's Component.</summary>

	/// <summary>Gets projectileSpawnPosition property.</summary>
	public Vector3 projectileSpawnPosition { get { return _projectileSpawnPosition; } }

	/// <summary>Gets petrolSphereID property.</summary>
	public CollectionIndex petrolSphereID { get { return _petrolSphereID; } }

	/// <summary>Gets marbleSphereID property.</summary>
	public CollectionIndex marbleSphereID { get { return _marbleSphereID; } }

	/// <summary>Gets homingPetrolSphereID property.</summary>
	public CollectionIndex homingPetrolSphereID { get { return _homingPetrolSphereID; } }

	/// <summary>Gets homingMarbleSphereID property.</summary>
	public CollectionIndex homingMarbleSphereID { get { return _homingMarbleSphereID; } }

	/// <summary>Gets and Sets order property.</summary>
	public ShootingOrder order
	{
		get { return _order; }
		set { _order = value; }
	}

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

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
/*#if UNITY_EDITOR
		base.DrawGizmos();

		Gizmos.DrawWireSphere(projectileSpawnPosition, 0.25f);
#endif*/
	}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{
		if(!run)
		{
			InvokeCoroutineEnd();
			yield break;
		}

		if(randomizeOrder)
		{
			order = (ShootingOrder)Random.Range(1, 4);
		}

		int sequenceLength = sequenceLimits.Random();
		StackQueue<Projectile> projectiles = new StackQueue<Projectile>();
		Projectile[] spheres = new Projectile[sequenceLength];
		CollectionIndex[] distribution = new CollectionIndex[sequenceLength];
		float durationSplit = (spheresAccomodationDuration) / (1.0f * sequenceLength);
		float inverseSplit = 1.0f / durationSplit;
		float t = 0.0f;
		HashSet<int> indexSet = new HashSet<int>();
		Vector3 center = projectileSpawnPosition;
		OnPoolObjectDeactivation onPoolObjectDeactivation = (poolObject)=>
		{
			PoolGameObject obj = poolObject as PoolGameObject;
			Projectile proj = obj.GetComponent<Projectile>();
			Game.RemoveTargetToCamera(proj.cameraTarget);
		};

		for(int i = 0; i < sequenceLength; i++)
		{
			/// Distribute the Random Sequence:
			Projectile sphere = null; 
			CollectionIndex index = Random.Range(0, 2) == 0 ? GetPetrolProjectileIndex() : GetMarbleProjectileIndex();
			indexSet.Add(index);

			if(i == (sequenceLength - 1))
			{ /// At the final iteration. Evaluate if the sequence has at least one of each sphere:
				if(!indexSet.Contains(GetPetrolProjectileIndex()))index = GetPetrolProjectileIndex();
				if(!indexSet.Contains(GetMarbleProjectileIndex())) index = GetMarbleProjectileIndex();
			}

			switch(order)
			{
				case ShootingOrder.SteeringSnake:
				sphere = PoolManager.RequestHomingProjectile(Faction.Enemy, index, center, Vector3.zero, null);
				break;

				case ShootingOrder.LeftToRight:
				case ShootingOrder.RightToLeft:
				case ShootingOrder.LeftAndRightOscillation:
				sphere = PoolManager.RequestProjectile(Faction.Enemy, index, center, Vector3.zero);
				break;

				sphere.onPoolObjectDeactivation -= onPoolObjectDeactivation;
				sphere.onPoolObjectDeactivation += onPoolObjectDeactivation;
				Game.AddTargetToCamera(sphere.cameraTarget);
			}

			sphere.gameObject.name += ("_" + i);

			sphere.activated = false;
			sphere.ActivateHitBoxes(false);
			spheres[i] = sphere;
			projectiles.Enqueue(sphere);

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

		if(order == ShootingOrder.SteeringSnake)
		{
			Projectile[] homingProjectiles = new Projectile[spheres.Length];

			for(int i = 0; i < spheres.Length; i++)
			{
				homingProjectiles[i] = spheres[i];
				homingProjectiles[i].projectileType = ProjectileType.Homing;
				homingProjectiles[i].activated = true;
				homingProjectiles[i].ActivateHitBoxes(true);
			}

			steeringSnake.InitializeLinkedList(Game.mateo.transform, homingProjectiles);
		}
		else
		{
			IEnumerator<Projectile> projectilesIterator = null;

			switch(order)
			{
				case ShootingOrder.LeftToRight:
				projectilesIterator = projectiles.IterateAsQueue();
				break;
				
				case ShootingOrder.RightToLeft:
				projectilesIterator = projectiles.IterateAsStack();
				break;
				
				case ShootingOrder.LeftAndRightOscillation:
				projectilesIterator = projectiles.IterateAsQueueAndStack();
				break;
			}

			Projectile proj = null;

			while(projectilesIterator.MoveNext())
			{
				proj = projectilesIterator.Current;
				Vector3 direction = Game.mateo.transform.position - proj.transform.position;
				proj.transform.rotation = VQuaternion.RightLookRotation(direction);
				proj.activated = true;
				proj.ActivateHitBoxes(true);

				SecondsDelayWait wait = new SecondsDelayWait(proj.lifespan);
				while(wait.MoveNext()) yield return null;
			}
		}

		bool projectilesInactive = false;

		while(!projectilesInactive)
		{
			int count = spheres.Length;

			foreach(Projectile p in spheres)
			{
				if(!p.active) count--;
			}

			if(count == 0) projectilesInactive = true;

			yield return null;
		}

		yield return null;
		InvokeCoroutineEnd();
	}

	/// <returns>Appropiate Petrol Projectile Index.</returns>
	private CollectionIndex GetPetrolProjectileIndex()
	{
		return order == ShootingOrder.SteeringSnake ? homingPetrolSphereID : petrolSphereID;
	}

	/// <returns>Appropiate Marble Projectile Index.</returns>
	private CollectionIndex GetMarbleProjectileIndex()
	{
		return order == ShootingOrder.SteeringSnake ? homingMarbleSphereID : marbleSphereID;
	}
}
}