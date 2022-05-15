using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public class TESTFixMoskarRotation : MonoBehaviour
{
    [SerializeField] private Transform moskar;      /// <summary>Moskar's Transform.</summary>
    [SerializeField] private Vector3 target;        /// <summary>Rotation Target.</summary>
    [SerializeField] private Vector3 up;            /// <summary>Reference Up Vector.</summary>
    private bool invertUp;                          /// <summary>Invert Up's Rotation?.</summary>

    /// <summary>Draws Gizmos on Editor mode.</summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white.WithAlpha(0.5f);
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void OnGUI()
    {
        if(moskar == null) return;

        invertUp = GUILayout.Toggle(invertUp, invertUp ? "Up Reverted" : "Up Not Reverted");
        GUILayout.Label("Rotation (Euler): " + moskar.rotation.eulerAngles.ToString());
    }

    // Update is called once per frame
    private void Update()
    {
        if(moskar == null) return;
        moskar.rotation = VQuaternion.LookRotation(transform.position - moskar.position, up * (invertUp ? -1.0f : 1.0f));
    }
}
