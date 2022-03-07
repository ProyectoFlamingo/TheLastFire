using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class Collision2DIgnorer : MonoBehaviour
{
	[SerializeField] private GameObjectTag[] _tags; 	/// <summary>Tags of GameObjects to ignore when on collision.</summary>

	/// <summary>Gets and Sets tags property.</summary>
	public GameObjectTag[] tags
	{
		get { return _tags; }
		set { _tags = value; }
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionEnter2D(Collision2D col)
	{
		GameObject obj = col.gameObject;
		
		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				Physics2D.IgnoreCollision(col.collider, col.otherCollider);
				return;
			}
		}
	}
}
}