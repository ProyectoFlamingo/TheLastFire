using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class Mandala : MonoBehaviour
{
    [SerializeField] private float _speed;           ///  <summary>Mandala's Speed.</summary>
    [SerializeField] private Bounds _boundaries;     /// <summary>Mandala's Boundaries.</summary>

    /// <summary>Gets speed property.</summary>
    public float speed { get { return _speed; } }

    /// <summary>Gets boundaries property.</summary>
    public Bounds boundaries { get { return _boundaries; } }

    /// <summary>Draws Gizmos on Editor mode when Mandala's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + boundaries.center, boundaries.size);
    }

    /// <summary>Updates Mandala's instance at each frame.</summary>
    private void Update()
    {
        Transform camera = Game.cameraController.transform;

        if(camera == null) return;

        Vector3 position = transform.position;
        float z = position.z;
        float boundsZ = (transform.position + boundaries.max).z;

        position = camera.position;
        z -= (Time.deltaTime * speed);
        position.z = z;

        transform.position = position;

        if(boundsZ < camera.position.z) gameObject.SetActive(false);
    }
}
}