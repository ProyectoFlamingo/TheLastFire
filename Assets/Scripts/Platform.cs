using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(TransformDeltaCalculator))]
public class Platform : MonoBehaviour
{
	[SerializeField] private GameObjectTag[] _tags; 		/// <summary>GameObjects' Tags.</summary>
	private HashSet<DisplacementAccumulator2D> _bodies; 	/// <summary>Transforms Components contained on the platform.</summary>
	private TransformDeltaCalculator _deltaCalculator; 		/// <summary>TransformDeltaCalculator's Component.</summary>

	/// <summary>Gets tags property.</summary>
	public GameObjectTag[] tags { get { return _tags; } }

	/// <summary>Gets and Sets bodies property.</summary>
	public HashSet<DisplacementAccumulator2D> bodies
	{
		get { return _bodies; }
		set { _bodies = value; }
	}

	/// <summary>Gets deltaCalculator Component.</summary>
	public TransformDeltaCalculator deltaCalculator
	{ 
		get
		{
			if(_deltaCalculator == null) _deltaCalculator = GetComponent<TransformDeltaCalculator>();
			return _deltaCalculator;
		}
	}

	/// <summary>Callback invoked when Platform's instance is disabled.</summary>
	private void OnDisable()
	{
		/*if(!gameObject.activeSelf) return;

		foreach(Transform child in transform)
		{
			foreach(GameObjectTag tag in tags)
			{
				if(child.gameObject.CompareTag(tag))
				{ 
					child.parent = null;
					break;
				}
			}
		}*/
		bodies.Clear();
	}

	/// <summary>Platform's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		bodies = new HashSet<DisplacementAccumulator2D>();
	}

	/// <summary>Updates Platform's instance at the end of each frame.</summary>
	private void FixedUpdate()
	{
		if(bodies != null)
		{
			Vector3 velocity = deltaCalculator.velocity;

			if(velocity.sqrMagnitude == 0.0f) return;

			foreach(DisplacementAccumulator2D body in bodies)
			{
				//body.MovePosition(body.transform.position + velocity);
				body.AddDisplacement(velocity);
			}
		}
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		GameObject obj = col.gameObject;
	
		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				DisplacementAccumulator2DLinker linker = obj.GetComponent<DisplacementAccumulator2DLinker>();

				if(linker != null) bodies.Add(linker.component);
				return;
			}
		}
	}

	/// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerExit2D(Collider2D col)
	{
		GameObject obj = col.gameObject;
	
		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				DisplacementAccumulator2DLinker linker = obj.GetComponent<DisplacementAccumulator2DLinker>();

				if(linker != null) bodies.Remove(linker.component);
				return;
			}
		}
	}
}
}