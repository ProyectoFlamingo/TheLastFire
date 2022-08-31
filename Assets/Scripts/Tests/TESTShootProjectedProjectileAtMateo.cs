using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;

public class TESTShootProjectedProjectileAtMateo : MonoBehaviour
{
    [SerializeField] private Transform muzzle;                        /// <summary>Muzzle's Transform.</summary>
    [SerializeField] private float shootInterval;                     /// <summary>Shooting's Interval.</summary>
    [SerializeField] private float projectionTime;                    /// <summary>Projection's Time.</summary>
    [SerializeField] private VAssetReference projectileReference;     /// <summary>Projectile's Reference.</summary>
    private TransformDeltaCalculator mateoDeltaCalculator;            /// <summary>Mateo's TransformDeltaCalculator's Component.</summary>
    private float time;                                               /// <summary>Current Time.</summary>

    /// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
    private void Start()
    {
        mateoDeltaCalculator = Game.mateo.deltaCalculator;
    }

    /// <summary>Updates TESTShootProjectedProjectileAtMateo's instance at the end of each frame.</summary>
    private void LateUpdate()
    {
        Vector3 direction = mateoDeltaCalculator.ProjectPosition(projectionTime).WithY(Game.mateo.transform.position.y) - transform.position;

        transform.rotation = VQuaternion.RightLookRotation(direction);

        if(time >= shootInterval)
        {
            PoolManager.RequestProjectile(Faction.Enemy, projectileReference, muzzle.position, direction);
            time = 0.0f;
        }
        else time += Time.deltaTime;
    }
}