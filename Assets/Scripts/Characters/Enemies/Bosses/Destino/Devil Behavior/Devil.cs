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
    [SerializeField] private Vector3 _leftEyePoint;                     /// <summary>Left-Eye's Point.</summary>
    [SerializeField] private Vector3 _rightEyePoint;                    /// <summary>Right-Eye's Point.</summary>
    [SerializeField] private VAssetReference _rayProjectileReference;   /// <summary>Ray Projectile's Reference.</summary>
    [SerializeField] private VAssetReference _projectileReference;      /// <summary>Projectile's Reference.</summary>
    [SerializeField] private FloatRange _waitRange;                     /// <summary>[Random] Wait Interval Before Each Shooting.</summary>
    [SerializeField] private float _waitBetweenShots;                   /// <summary>Wait between pair of shots.</summary>
    [SerializeField] private float _projectionTime;                     /// <summary>Projection Time used to get Mateo's Extrapolation.</summary>
    private Coroutine laserRoutine;                                     /// <summary>Lasers' Coroutine reference.</summary>
    private HashSet<Projectile> _projectiles;                           /// <summary>Projectiles that the Devil shoots.</summary>

    /// <summary>Gets leftEyePoint property.</summary>
    public Vector3 leftEyePoint { get { return _leftEyePoint; } }

    /// <summary>Gets rightEyePoint property.</summary>
    public Vector3 rightEyePoint { get { return _rightEyePoint; } }

    /// <summary>Gets rayProjectileReference property.</summary>
    public VAssetReference rayProjectileReference { get { return _rayProjectileReference; } }

    /// <summary>Gets projectileReference property.</summary>
    public VAssetReference projectileReference { get { return _projectileReference; } }

    /// <summary>Gets waitRange property.</summary>
    public FloatRange waitRange { get { return _waitRange; } }

    /// <summary>Gets waitBetweenShots property.</summary>
    public float waitBetweenShots { get { return _waitBetweenShots; } }

    /// <summary>Gets projectionTime property.</summary>
    public float projectionTime { get { return _projectionTime; } }

    /// <summary>Gets and Sets projectiles property.</summary>
    public HashSet<Projectile> projectiles
    {
        get { return _projectiles; }
        private set { _projectiles = value; }
    }

    /// <summary>Draws Gizmos on Editor mode when Devil's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(leftEyePoint), 0.25f);
        Gizmos.DrawWireSphere(transform.TransformPoint(rightEyePoint), 0.25f);
    }

    /// <summary>Devil's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        projectiles = new HashSet<Projectile>();
        //Initialize();
    }

    /// <summary>Initializes Devil.</summary>
    public void Initialize()
    {
        this.DispatchCoroutine(ref laserRoutine);
        this.StartCoroutine(AIRoutine(), ref behaviorCoroutine);
        BeginLaserRoutine();
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

        if(projectiles != null)
        {
            foreach(Projectile projectile in projectiles)
            {
                projectile.onPoolObjectDeactivation -= OnProjectileDeactivated;
                projectile.OnObjectDeactivation();
            }

            projectiles.Clear();
        }
        
        base.OnObjectDeactivation();
    }

    /// <summary>Shoots lasers at Mateo.</summary>
    private void ShootLasers()
    {
        Vector3 a = transform.TransformPoint(leftEyePoint);
        Vector3 b = transform.TransformPoint(rightEyePoint);
        Vector3 c = Vector3.Lerp(a, b, 0.5f);
        Vector3 direction = (Vector3)Game.ProjectMateoPosition(projectionTime) - c;

        Projectile pA = PoolManager.RequestProjectile(Faction.Enemy, rayProjectileReference, a, direction);
        Projectile pB = PoolManager.RequestProjectile(Faction.Enemy, rayProjectileReference, b, direction);

        pA.onPoolObjectDeactivation -= OnProjectileDeactivated;
        pB.onPoolObjectDeactivation -= OnProjectileDeactivated;
        pA.onPoolObjectDeactivation += OnProjectileDeactivated;
        pB.onPoolObjectDeactivation += OnProjectileDeactivated;
        projectiles.Add(pA);
        projectiles.Add(pB);
    }

    /// <summary>Shoots Nail from given hand position.</summary>
    /// <param name="_handPosition">Hand's Position.</param>
    private void ShootNail(Vector3 _handPosition)
    {
        Vector3 direction = (Vector3)Game.ProjectMateoPosition(projectionTime) - _handPosition;
        Projectile projectile = PoolManager.RequestProjectile(Faction.Enemy, projectileReference, _handPosition, direction);
    
        if(projectile != null)
        {
            projectile.onPoolObjectDeactivation -= OnProjectileDeactivated;
            projectile.onPoolObjectDeactivation += OnProjectileDeactivated;
            projectiles.Add(projectile);
        }
    }

    /// <summary>Callback invoked when a Projectile Deactivates.</summary>
    /// <param name="_poolObject">Projectile [as IPoolObject].</param>
    private void OnProjectileDeactivated(IPoolObject _poolObject)
    {
        Projectile projectile = _poolObject as Projectile;

        if(projectile != null)
        {
            projectile.onPoolObjectDeactivation -= OnProjectileDeactivated;
            projectiles.Remove(projectile);
        }
    }

    /// <summary>Devil's AI Routine.</summary>
    private IEnumerator AIRoutine()
    {
        SecondsDelayWait wait = new SecondsDelayWait(0.0f);
        Transform firstHand = null;
        Transform secondHand = null;

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

            ShootNail(firstHand.position);

            wait.ChangeDurationAndReset(waitBetweenShots);
            while(wait.MoveNext()) yield return null;

            ShootNail(secondHand.position);
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