using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
public class TESTDoesItTrigger2D : MonoBehaviour
{
	/// <summary>Event triggered when this Collider2D enters another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		GameObject obj = col.gameObject;
	
#if UNITY_EDITOR
		StringBuilder builder = new StringBuilder();
		builder.Append("Triggered with GameObject: ");
		builder.AppendLine(obj.name);
		builder.Append("Tag: ");
		builder.AppendLine(obj.tag);
		builder.Append("Layer: ");
		builder.Append(obj.layer);

		Debug.Log(builder.ToString());
#endif
	}
}
}