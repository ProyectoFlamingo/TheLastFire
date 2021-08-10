﻿using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum ArrowProjectileState
{
	NotIntersectedWithIncrustable,
	IntersectedWithFirstIncrustable,
	Incrusted
}

/// <summary>Event invoked when the arrow is inverted.</summary>
/// <param name="_projectile">ArrowProjectile that was inverted.</param>
public delegate void OnInverted(ArrowProjectile _projectile);

[RequireComponent(typeof(LineRenderer))]
public class ArrowProjectile : Projectile
{
	public event OnInverted onInverted; 					/// <summary>OnInverted's event delegate.</summary>

	[Space(5f)]
	[Header("Arrow Projectile's Attributes:")]
	[SerializeField] private GameObjectTag[] _incrustTags; 	/// <summary>Tags of GameObjects that can be incrusted by the Arrow Projectile.</summary>
	[SerializeField] private HitCollider2D _tipHitBox; 		/// <summary>Tip's HitBox.</summary>
	[SerializeField] private BoxCollider2D _chainCollider; 	/// <summary>Chain's BoxCollider.</summary>
	private LineRenderer _lineRenderer; 					/// <summary>LineRenderer's Component.</summary>
	private Vector3 _spawnPosition; 						/// <summary>Spawn Position.</summary>
	private ArrowProjectileState _state; 					/// <summary>Arrow Projectile's State.</summary>
	private bool inverted;

#region Getter/Setters:
	/// <summary>Gets and Sets incrustTags property.</summary>
	public GameObjectTag[] incrustTags
	{
		get { return _incrustTags; }
		set { _incrustTags = value; }
	}

	/// <summary>Gets tipHitBox property.</summary>
	public HitCollider2D tipHitBox { get { return _tipHitBox; } }

	/// <summary>Gets chainCollider property.</summary>
	public BoxCollider2D chainCollider { get { return _chainCollider; } }

	/// <summary>Gets and Sets state property.</summary>
	public ArrowProjectileState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets lineRenderer Component.</summary>
	public LineRenderer lineRenderer
	{ 
		get
		{
			if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
			return _lineRenderer;
		}
	}

	/// <summary>Gets and Sets spawnPosition property.</summary>
	public Vector3 spawnPosition
	{
		get { return _spawnPosition; }
		set { _spawnPosition = value; }
	}
#endregion

	/// <summary>Callback internally invoked inside Update.</summary>
	protected override void Update()
	{
		switch(state)
		{
			case ArrowProjectileState.Incrusted:
			base.Update(); /// Only tick its lifespan when it is incrusted
			break;
		}

		UpdateChain();
	}

	/// <summary>Callback internally invoked inside FixedUpdate.</summary>
	protected override void FixedUpdate()
	{
		if(state == ArrowProjectileState.Incrusted) return;

		base.FixedUpdate();
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public virtual void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
#region Debug:
		StringBuilder builder = new StringBuilder();

		builder.Append("OnTriggerEvent invoked to class ");
		builder.AppendLine(name);
		builder.Append("State: ");
		builder.Append(state.ToString());

		Debug.Log(builder.ToString());
#endregion

		if(state == ArrowProjectileState.Incrusted) return;

		Collider2D collider = _info.collider;

		base.OnTriggerEvent(_info, _eventType, _ID);
		
		GameObject obj = collider.gameObject;

		if(incrustTags != null) foreach(GameObjectTag tag in incrustTags)
		{
			if(obj.CompareTag(tag))
			{
				switch(state)
				{
					case ArrowProjectileState.NotIntersectedWithIncrustable:
					state = ArrowProjectileState.IntersectedWithFirstIncrustable;
					break;

					case ArrowProjectileState.IntersectedWithFirstIncrustable:
					state = ArrowProjectileState.Incrusted;
					tipHitBox.SetTrigger(false);
					break;
				}

				Debug.Log("[ArrowProjectile] Interacted with uncrustable object, new state: " + state.ToString());
			}
		}
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		spawnPosition = transform.position;
		state = ArrowProjectileState.NotIntersectedWithIncrustable;
		tipHitBox.SetTrigger(true);
		lineRenderer.enabled = true;
		inverted = false;
	}

	/// <summary>Callback invoked when the object is deactivated.</summary>
	public override void OnObjectDeactivation()
	{
		state = ArrowProjectileState.NotIntersectedWithIncrustable;
		tipHitBox.SetTrigger(true);
		lineRenderer.enabled = false;
		base.OnObjectDeactivation();
	}

	/// <summary>Updates Chain.</summary>
	private void UpdateChain()
	{
		lineRenderer.SetPosition(0, spawnPosition);
		lineRenderer.SetPosition(1, transform.position);

		if(chainCollider == null) return;

		Vector3 a = lineRenderer.GetPosition(0);
		Vector3 b = lineRenderer.GetPosition(1);
		Vector3 d = b - a;
		float m = d.magnitude;

		chainCollider.size = new Vector2(
			m,
			lineRenderer.GetWidth() * 0.5f
		);
		chainCollider.transform.position = Vector3.Lerp(a, b, 0.5f);
		chainCollider.transform.rotation = VQuaternion.RightLookRotation(d);
	}
}
}