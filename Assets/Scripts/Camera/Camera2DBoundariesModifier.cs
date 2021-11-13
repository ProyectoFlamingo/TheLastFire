using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Boundaries2DContainer))]
[RequireComponent(typeof(BoxCollider2D))]
//[ExecuteInEditMode]
public class Camera2DBoundariesModifier : MonoBehaviour
{
	[SerializeField] private GameObjectTag _playerTag; 							/// <summary>Player's Tag.</summary>
	[Space(5f)]
	[SerializeField] private float _interpolationDuration; 						/// <summary>Interpolation's Duration.</summary>
	[Space(5f)]
	[Header("Distance Settings:")]
	[SerializeField] private bool _setDistance; 								/// <summary>Set Camera's Distance?.</summary>
	[SerializeField] private FloatRange _distanceRange; 						/// <summary>Camera's Distance Range.</summary>
	private Boundaries2DContainer _boundariesContainer; 						/// <summary>Boundaries2DContainer's Component.</summary>
	private BoxCollider2D _boxCollider; 										/// <summary>BoxCollider2D's Component.</summary>
	private bool _entered; 														/// <summary>Has the GameObject of interest already entered the zone?.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets playerTag property.</summary>
	public GameObjectTag playerTag
	{
		get { return _playerTag; }
		private set { _playerTag = value; }
	}

	/// <summary>Gets and Sets interpolationDuration property.</summary>
	public float interpolationDuration
	{
		get { return _interpolationDuration; }
		set { _interpolationDuration = value; }
	}

	/// <summary>Gets and Sets setDistance property.</summary>
	public bool setDistance
	{
		get { return _setDistance; }
		set { _setDistance = value; }
	}

	/// <summary>Gets and Sets distanceRange property.</summary>
	public FloatRange distanceRange
	{
		get { return _distanceRange; }
		set { _distanceRange = value; }
	}

	/// <summary>Gets boundariesContainer Component.</summary>
	public Boundaries2DContainer boundariesContainer
	{ 
		get
		{
			if(_boundariesContainer == null) _boundariesContainer = GetComponent<Boundaries2DContainer>();
			return _boundariesContainer;
		}
	}

	/// <summary>Gets boxCollider Component.</summary>
	public BoxCollider2D boxCollider
	{ 
		get
		{
			if(_boxCollider == null) _boxCollider = GetComponent<BoxCollider2D>();
			return _boxCollider;
		}
	}

	/// <summary>Gets and Sets entered property.</summary>
	public bool entered
	{
		get { return _entered; }
		set { _entered = value; }
	}
#endregion

	/// <summary>Camera2DBoundariesModifier's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		entered = false;
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
		UpdateBoxCollider();

		if(!setDistance) return;

		Vector3 center = boundariesContainer.GetPosition();
		Vector3 minPoint = center + (Vector3.back * distanceRange.Min());
		Vector3 maxPoint = center + (Vector3.back * distanceRange.Max());

		Gizmos.DrawLine(center, minPoint);
		Gizmos.DrawLine(center, maxPoint);
		Gizmos.DrawWireSphere(minPoint, 0.05f);
		Gizmos.DrawWireSphere(maxPoint, 0.05f);
	}

	/// <summary>Resets Camera2DBoundariesModifier's instance to its default values.</summary>
	private void Reset()
	{
		boxCollider.isTrigger = true;
		playerTag = Game.data.playerTag;
	}

	/// <summary>Updates BoxCollider2D.</summary>
	private void UpdateBoxCollider()
	{
		boxCollider.size = boundariesContainer.size;
		//boundariesContainer.size = boxCollider.size;
	}

	/// <summary>Event triggered when this Collider2D enters another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(entered) return;

		GameObject obj = col.gameObject;
	
		if(obj.CompareTag(playerTag))
		{
			entered = true;
			Game.OnCamera2DBoundariesModifierEnter(this);

			return;
		}
	}

	/// <summary>Event triggered when this Collider2D exits another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerExit2D(Collider2D col)
	{
		if(!entered) return;

		GameObject obj = col.gameObject;
	
		if(obj.CompareTag(playerTag))
		{
			entered = false;
			Game.OnCamera2DBoundariesModifierExit(this);

			return;
		}
	}
}
}