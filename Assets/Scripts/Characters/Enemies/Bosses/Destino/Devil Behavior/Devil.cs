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
    [SerializeField] private int _projectileIndex;      /// <summary>Projectile's Index.</summary>
    [SerializeField] private FloatRange _waitRange;     /// <summary>[Random] Wait Interval Before Each Shooting.</summary>
    [SerializeField] private float _waitBetweenShots;   /// <summary>Wait between pair of shots.</summary>
    [SerializeField] private float _projectionTime;     /// <summary>Projection Time used to get Mateo's Extrapolation.</summary>

    /// <summary>Gets projectileIndex property.</summary>
    public int projectileIndex { get { return _projectileIndex; } }

    /// <summary>Gets waitRange property.</summary>
    public FloatRange waitRange { get { return _waitRange; } }

    /// <summary>Gets waitBetweenShots property.</summary>
    public float waitBetweenShots { get { return _waitBetweenShots; } }

    /// <summary>Gets projectionTime property.</summary>
    public float projectionTime { get { return _projectionTime; } }

    /// <summary>Devil's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        Initialize();
    }

    /// <summary>Initializes Devil.</summary>
    public void Initialize()
    {
        this.StartCoroutine(AIRoutine(), ref behaviorCoroutine);
    }

    /// <summary>Callback invoked when the object is deactivated.</summary>
    public override void OnObjectDeactivation()
    {
        this.DispatchCoroutine(ref behaviorCoroutine);
        base.OnObjectDeactivation();
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
}
}