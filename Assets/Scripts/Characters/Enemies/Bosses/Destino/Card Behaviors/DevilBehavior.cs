using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class DevilBehavior : DestinoScriptableCoroutine
{
	[Space(5f)]
	[SerializeField] private FloatRange _xLimits; 						/// <summary>Mateo's Limits on the X-Axis.</summary>
	[Space(5f)]
	[Header("Characters:")]
	[SerializeField] private Devil _devil; 								/// <summary>Devil.</summary>
	[SerializeField] private DevilTower _leftDevilTower; 				/// <summary>Left Tower.</summary>
	[SerializeField] private DevilTower _rightDevilTower; 				/// <summary>Right Tower.</summary>
	[Space(5f)]
	[SerializeField] private int _arrowProjectileIndex; 				/// <summary>Arrow Projectile's Index.</summary>
	[SerializeField] private IntRange _rounds; 							/// <summary>Arrows Rounds per-routine.</summary>
	[SerializeField] private IntRange _limits; 							/// <summary>Arrows' Limits per round.</summary>
	[SerializeField] private float[] _projectilesSpawnRates; 			/// <summary>Arrow Projectile's Spawn Rate.</summary>
	[SerializeField] private float _projectionTime; 					/// <summary>Mateo Position's Projection's Time.</summary>
	[SerializeField] private float _roundCooldown; 						/// <summary>Cooldown duration per-round.</summary>
	[Space(5f)]
	[SerializeField] private Vector3[] _floorWaypoints; 				/// <summary>Floor's Waypoints.</summary>
	[Space(5f)]
	[Header("Devil Scenery's Attributes:")]
	[SerializeField] private Vector3 _devilSpawnPoint; 					/// <summary>Devil's Spawn Point.</summary>
	[SerializeField] private Vector3 _devilDestinyPoint; 				/// <summary>Devil's Destiny Point.</summary>
	[SerializeField] private Vector3 _leftTowerSpawnPoint; 				/// <summary>Left Tower's Spawn Point.</summary>
	[SerializeField] private Vector3 _rightTowerSpawnPoint; 			/// <summary>Right Tower's Spawn Point.</summary>
	[SerializeField] private Vector3 _leftTowerDestinyPoint; 			/// <summary>Left Tower's Destiny Point.</summary>
	[SerializeField] private Vector3 _rightTowerDestinyPoint; 			/// <summary>Right Tower's Destiny Point.</summary>
	[SerializeField] private float _towerInterpolationDuration; 		/// <summary>Towers' Fall Duration.</summary>
	[SerializeField] private float _towerShakeDuration; 				/// <summary>Towers' Shake Duration.</summary>
	[SerializeField] private float _towerShakeSpeed; 					/// <summary>Towers' Shake Speed.</summary>
	[SerializeField] private float _towerShakeMagnitude; 				/// <summary>Towers' Shake Megnitude.</summary>
	[SerializeField] private float _towerHP; 							/// <summary>Towers' starting HP.</summary>
	[SerializeField] private float _devilHP; 							/// <summary>Devil's HP.</summary>

#region Getters/Setters:
	/// <summary>Gets xLimits property.</summary>
	public FloatRange xLimits { get { return _xLimits; } }

	/// <summary>Gets devil property.</summary>
	public Devil devil { get { return _devil; } }

	/// <summary>Gets leftDevilTower property.</summary>
	public DevilTower leftDevilTower { get { return _leftDevilTower; } }

	/// <summary>Gets rightDevilTower property.</summary>
	public DevilTower rightDevilTower { get { return _rightDevilTower; } }

	/// <summary>Gets arrowProjectileIndex property.</summary>
	public int arrowProjectileIndex { get { return _arrowProjectileIndex; } }

	/// <summary>Gets rounds property.</summary>
	public IntRange rounds { get { return _rounds; } }

	/// <summary>Gets limits property.</summary>
	public IntRange limits { get { return _limits; } }

	/// <summary>Gets projectilesSpawnRates property.</summary>
	public float[] projectilesSpawnRates { get { return _projectilesSpawnRates; } }

	/// <summary>Gets projectionTime property.</summary>
	public float projectionTime { get { return _projectionTime; } }

	/// <summary>Gets roundCooldown property.</summary>
	public float roundCooldown { get { return _roundCooldown; } }

	/// <summary>Gets floorWaypoints property.</summary>
	public Vector3[] floorWaypoints { get { return _floorWaypoints; } }

	/// <summary>Gets devilSpawnPoint property.</summary>
	public Vector3 devilSpawnPoint { get { return _devilSpawnPoint; } }

	/// <summary>Gets devilDestinyPoint property.</summary>
	public Vector3 devilDestinyPoint { get { return _devilDestinyPoint; } }

	/// <summary>Gets leftTowerSpawnPoint property.</summary>
	public Vector3 leftTowerSpawnPoint { get { return _leftTowerSpawnPoint; } }

	/// <summary>Gets leftTowerDestinyPoint property.</summary>
	public Vector3 leftTowerDestinyPoint { get { return _leftTowerDestinyPoint; } }

	/// <summary>Gets rightTowerSpawnPoint property.</summary>
	public Vector3 rightTowerSpawnPoint { get { return _rightTowerSpawnPoint; } }

	/// <summary>Gets rightTowerDestinyPoint property.</summary>
	public Vector3 rightTowerDestinyPoint { get { return _rightTowerDestinyPoint; } }

	/// <summary>Gets towerInterpolationDuration property.</summary>
	public float towerInterpolationDuration { get { return _towerInterpolationDuration; } }

	/// <summary>Gets towerShakeDuration property.</summary>
	public float towerShakeDuration { get { return _towerShakeDuration; } }

	/// <summary>Gets towerShakeSpeed property.</summary>
	public float towerShakeSpeed { get { return _towerShakeSpeed; } }

	/// <summary>Gets towerShakeMagnitude property.</summary>
	public float towerShakeMagnitude { get { return _towerShakeMagnitude; } }

	/// <summary>Gets towerHP property.</summary>
	public float towerHP { get { return _towerHP; } }

	/// <summary>Gets devilHP property.</summary>
	public float devilHP { get { return _devilHP; } }
#endregion

#if UNITY_EDITOR
	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
		base.DrawGizmos();
		
		Gizmos.color = gizmosColor;

		Gizmos.DrawWireSphere(devilSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(devilDestinyPoint, gizmosRadius);
		Gizmos.DrawWireSphere(leftTowerSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(rightTowerSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(leftTowerDestinyPoint, gizmosRadius);
		Gizmos.DrawWireSphere(rightTowerDestinyPoint, gizmosRadius);
		Gizmos.DrawWireSphere(Vector3.right * xLimits.Min(), gizmosRadius);
		Gizmos.DrawWireSphere(Vector3.right * xLimits.Max(), gizmosRadius);

		if(floorWaypoints != null) foreach(Vector3 waypoint in floorWaypoints)
		{
			Gizmos.DrawWireSphere(waypoint, gizmosRadius);
		}
	}
#endif

	/// <returns>Middle point between random floor point and Mateo's current projection.</returns>
	private Vector3 GetTargetPoint()
	{
		Vector3 mateoPosition = Game.ProjectMateoPosition(projectionTime);
		Vector3 floorWaypoint = floorWaypoints.Random();
		Vector3 destiny = Vector3.Lerp(floorWaypoint, mateoPosition, Random.Range(0.0f, 1.0f));

		return destiny;
	}

	[Button("Position Devil's Scenery at Spawn Points")]
	/// <summary>Positions Devil's Scenery on Spawn Points.</summary>
	private void PositionTowersAndDevilAtSpawnPoints()
	{
		devil.transform.position = devilSpawnPoint;
		leftDevilTower.transform.position = leftTowerSpawnPoint;
		rightDevilTower.transform.position = rightTowerSpawnPoint;
	}

	[Button("Position Devil's Scenery at Destiny Points")]
	/// <summary>Positions Devil's Scenery on Destiny Points.</summary>
	private void PositionTowersAndDevilAtDestinyPoints()
	{
		devil.transform.position = devilDestinyPoint;
		leftDevilTower.transform.position = leftTowerDestinyPoint;
		rightDevilTower.transform.position = rightTowerDestinyPoint;
	}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{
		List<DevilTower> towers = new List<DevilTower>();
		DevilTower tower = null;
		int length = limits.Random();
		int count = 3; // For left and right tower and the devil (1 + 1 + 1 duh?).
		float spawnRate = projectilesSpawnRates[Mathf.Clamp(boss.currentStage - 1, 0, projectilesSpawnRates.Length - 1)];
		float t = 0.0f;
		float inverseDuration = 1.0f / towerInterpolationDuration;
		bool devilAlive = true;
		bool leftTowerAlive = true;
		bool rightTowerAlive = true;
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		OnHealthInstanceEvent onHealthEvent = (_health, _event, _amount, _object)=>
		{
			switch(_event)
			{
				case HealthEvent.FullyDepleted:
				count--;

				/// BEGIN Rather quick solution:
				Vector3 destiny = Vector3.zero;

				/// END Rather quick solution...
				if(_health == devil.health) destiny = devilSpawnPoint;
				if(_health == leftDevilTower.health) destiny = leftTowerSpawnPoint;
				if(_health == rightDevilTower.health) destiny = rightTowerSpawnPoint;

				boss.StartCoroutine(_health.transform.DisplaceToPosition(destiny, towerInterpolationDuration,
				()=>
				{
					if(_health == devil.health)
					{
						devil.OnObjectDeactivation();
						devilAlive = false;
					}
					if(_health == leftDevilTower.health)
					{
						leftDevilTower.OnObjectDeactivation();
						leftTowerAlive = false;
					}
					if(_health == rightDevilTower.health)
					{
						rightDevilTower.OnObjectDeactivation();
						rightTowerAlive = false;
					}
				
					//_health.gameObject.SetActive(false);

				}));

				break;
			}

			if(count == 1 && devil.health.hp > 0.0f)
			{
				devil.Initialize();

			} else if(devil.health.hp <= 0.0f) count = 0;
		};

		// Invoke Devil & Towers:
		devil.gameObject.SetActive(true);
		leftDevilTower.OnObjectReset();
		rightDevilTower.OnObjectReset();
		leftDevilTower.gameObject.SetActive(true);
		rightDevilTower.gameObject.SetActive(true);
		devil.transform.position = devilSpawnPoint;
		leftDevilTower.transform.position = leftTowerSpawnPoint;
		rightDevilTower.transform.position = rightTowerSpawnPoint;
		devil.health.SetMaxHP(devilHP, true);
		leftDevilTower.health.SetMaxHP(towerHP, true);
		rightDevilTower.health.SetMaxHP(towerHP, true);
		devil.health.onHealthInstanceEvent -= onHealthEvent;
		leftDevilTower.health.onHealthInstanceEvent -= onHealthEvent;
		rightDevilTower.health.onHealthInstanceEvent -= onHealthEvent;
		devil.health.onHealthInstanceEvent += onHealthEvent;
		leftDevilTower.health.onHealthInstanceEvent += onHealthEvent;
		rightDevilTower.health.onHealthInstanceEvent += onHealthEvent;

		/// Lerp Devil & Towers:
		while(t < 1.0f)
		{
			float st = t * t;
			Transform mateo = Game.mateo.transform;
			Vector3 mateoPosition = mateo.position;
			
			devil.transform.position = Vector3.Lerp(devilSpawnPoint, devilDestinyPoint, st);
			leftDevilTower.transform.position = Vector3.Lerp(leftTowerSpawnPoint, leftTowerDestinyPoint, st);
			rightDevilTower.transform.position = Vector3.Lerp(rightTowerSpawnPoint, rightTowerDestinyPoint, st);
			mateoPosition.x = Mathf.Clamp(mateoPosition.x, xLimits.Min(), xLimits.Max());
			mateo.position = Vector3.Lerp(mateo.position, mateoPosition, st);

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		leftDevilTower.transform.position = leftTowerDestinyPoint;
		rightDevilTower.transform.position = rightTowerDestinyPoint;
		leftDevilTower.landingParticleEffect.EmitParticleEffects();
		rightDevilTower.landingParticleEffect.EmitParticleEffects();

		/// Shake Devil & Towers when they reach their destiny positions.
		IEnumerator devilShake = devil.transform.ShakePosition(towerShakeDuration, towerShakeSpeed, towerShakeMagnitude);
		IEnumerator leftTowerShake = leftDevilTower.transform.ShakePosition(towerShakeDuration, towerShakeSpeed, towerShakeMagnitude);
		IEnumerator rightTowerShake = rightDevilTower.transform.ShakePosition(towerShakeDuration, towerShakeSpeed, towerShakeMagnitude);

		while(devilShake.MoveNext()
		|| leftTowerShake.MoveNext()
		|| rightTowerShake.MoveNext()) yield return null;

		while(count > 0)
		{
			/// Invoke Devils' Projectiles
			for(int i = 0; i < length; i++)
			{
				if(count == 0) break;

				towers.Clear();

				if(leftDevilTower.health.hp > 0.0f && leftDevilTower.HasAvailableMuzzle()) towers.Add(leftDevilTower);
				if(rightDevilTower.health.hp > 0.0f && rightDevilTower.HasAvailableMuzzle()) towers.Add(rightDevilTower);

				if(towers.Count > 0)
				{
					tower = towers.Random();
					tower.ShootArrow(GetTargetPoint());
				}

				wait.ChangeDurationAndReset(spawnRate);
				while(wait.MoveNext() && count > 0) yield return null;
			}

			wait.ChangeDurationAndReset(roundCooldown);
			while(wait.MoveNext() && count > 0) yield return null;
		}

		t = 0.0f;

		/// FORCE Lerp back Devil & Towers:
		if(devilAlive || leftTowerAlive || rightTowerAlive) while(t < 1.0f)
		{
			devil.transform.position = Vector3.Lerp(devilSpawnPoint, devilDestinyPoint, 1.0f - t);
			leftDevilTower.transform.position = Vector3.Lerp(leftTowerSpawnPoint, leftTowerDestinyPoint, 1.0f - t);
			rightDevilTower.transform.position = Vector3.Lerp(rightTowerSpawnPoint, rightTowerDestinyPoint, 1.0f - t);

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		devil.gameObject.SetActive(false);
		leftDevilTower.gameObject.SetActive(false);
		rightDevilTower.gameObject.SetActive(false);

		devil.health.onHealthInstanceEvent -= onHealthEvent;
		leftDevilTower.health.onHealthInstanceEvent -= onHealthEvent;
		rightDevilTower.health.onHealthInstanceEvent -= onHealthEvent;

		InvokeCoroutineEnd();
	}
}
}