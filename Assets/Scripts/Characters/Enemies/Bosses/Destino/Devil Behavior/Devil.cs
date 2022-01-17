using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

using Random = UnityEngine.Random;

namespace Flamingo
{
public class Devil : Character
{
    [Space(5f)]
    [Header("Devil's Attributes:")]
    [SerializeField] private Vector3 _leftEyePoint;     /// <summary>Left-Eye's Point.</summary>
    [SerializeField] private Vector3 _rightEyePoint;    /// <summary>Right-Eye's Point.</summary>
    [SerializeField] private int _rayProjectileIndex;   /// <summary>Ray Projectile's Index.</summary>
    [SerializeField] private int _projectileIndex;      /// <summary>Projectile's Index.</summary>
    [SerializeField] private FloatRange _waitRange;     /// <summary>[Random] Wait Interval Before Each Shooting.</summary>
    [SerializeField] private float _waitBetweenShots;   /// <summary>Wait between pair of shots.</summary>
    [SerializeField] private float _projectionTime;     /// <summary>Projection Time used to get Mateo's Extrapolation.</summary>
    private Coroutine laserRoutine;                     /// <summary>Lasers' Coroutine reference.</summary>

    /// <summary>Gets leftEyePoint property.</summary>
    public Vector3 leftEyePoint { get { return _leftEyePoint; } }

    /// <summary>Gets rightEyePoint property.</summary>
    public Vector3 rightEyePoint { get { return _rightEyePoint; } }

    /// <summary>Gets rayProjectileIndex property.</summary>
    public int rayProjectileIndex { get { return _rayProjectileIndex; } }

    /// <summary>Gets projectileIndex property.</summary>
    public int projectileIndex { get { return _projectileIndex; } }

    /// <summary>Gets waitRange property.</summary>
    public FloatRange waitRange { get { return _waitRange; } }

    /// <summary>Gets waitBetweenShots property.</summary>
    public float waitBetweenShots { get { return _waitBetweenShots; } }

    /// <summary>Gets projectionTime property.</summary>
    public float projectionTime { get { return _projectionTime; } }

    /// <summary>Draws Gizmos on Editor mode when Devil's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(leftEyePoint), 0.25f);
        Gizmos.DrawWireSphere(transform.TransformPoint(rightEyePoint), 0.25f);
    }

    /// <summary>Devil's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        Initialize();
    }

    /// <summary>Initializes Devil.</summary>
    public void Initialize()
    {
        this.DispatchCoroutine(ref laserRoutine);
        this.StartCoroutine(AIRoutine(), ref behaviorCoroutine);
    }

    /// <summary>Initializes Lasers' Routine.</summary>
    public void BeginLaserRoutine()
    {
        this.StartCoroutine(EyeLaserRoutine(), ref laserRoutine);
    }

    /// <summary>Callback invoked when the object is deactivated.</summary>
    public override void OnObjectDeactivation()
    {
        this.DispatchCoroutine(ref behaviorCoroutine);
        this.DispatchCoroutine(ref laserRoutine);
        base.OnObjectDeactivation();
    }

    /// <summary>Shoots lasers at Mateo.</summary>
    private void ShootLasers()
    {
        Vector3 a = transform.TransformPoint(leftEyePoint);
        Vector3 b = transform.TransformPoint(rightEyePoint);
        Vector3 c = Vector3.Lerp(a, b, 0.5f);
        Vector3 direction = (Vector3)Game.ProjectMateoPosition(projectionTime) - c;

        PoolManager.RequestProjectile(Faction.Enemy, rayProjectileIndex, a, direction);
        PoolManager.RequestProjectile(Faction.Enemy, rayProjectileIndex, b, direction);
    }

    /// <summary>Devil's AI Routine.</summary>
    private IEnumerator AIRoutine()
    {
        SecondsDelayWait wait = new SecondsDelayWait(0.0f);
        Transform firstHand = null;
        Transform secondHand = null;
        Vector3 direction = Vector3.zero;

        while(true)
        {
            wait.ChangeDurationAndReset(waitRange.Random());

            while(wait.MoveNext()) yield return null;

            switch(Random.Range(0, 2))
            {
                case 0:
                firstHand = skeleton.leftHand;
                secondHand = skeleton.rightHand;
                break;

                case 1:
                firstHand = skeleton.rightHand;
                secondHand = skeleton.leftHand;
                break;
            }

            direction = (Vector3)Game.ProjectMateoPosition(projectionTime) - firstHand.position;
            PoolManager.RequestProjectile(Faction.Enemy, projectileIndex, firstHand.position, direction);

            wait.ChangeDurationAndReset(waitBetweenShots);
            while(wait.MoveNext()) yield return null;

            direction = (Vector3)Game.ProjectMateoPosition(projectionTime) - secondHand.position;
            PoolManager.RequestProjectile(Faction.Enemy, projectileIndex, secondHand.position, direction);
        }
    }

    /// <summary>Lasers' Routine.</summary>
    private IEnumerator EyeLaserRoutine()
    {
        SecondsDelayWait wait = new SecondsDelayWait(0.0f);
        
        while(true)
        {
            wait.ChangeDurationAndReset(waitRange.Random());
            while(wait.MoveNext()) yield return null;

            ShootLasers();
        }
    }
}
}