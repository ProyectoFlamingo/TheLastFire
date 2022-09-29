using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ImpactEventHandler))]
[RequireComponent(typeof(EventsHandler))]
public class BouncingBall : MonoBehaviour
{
	[SerializeField] private GameObjectTag[] _impactTags; 	/// <summary>Impacts' Tags.</summary>
	[SerializeField] private Vector3 _force; 				/// <summary>Extra Force Applied when impacting.</summary>
	private Rigidbody2D _rigidbody; 						/// <summary>Rigidbody's Component.</summary>
	private ImpactEventHandler _impactHandler; 				/// <summary>ImpactEventHandler's Component.</summary>
	private EventsHandler _eventsHandler; 					/// <summary>EventsHandler's Component.</summary>

	/// <summary>Gets and Sets impactTags property.</summary>
	public GameObjectTag[] impactTags
	{
		get { return _impactTags; }
		set { _impactTags = value; }
	}

	/// <summary>Gets and Sets force property.</summary>
	public Vector3 force
	{
		get { return _force; }
		set { _force = value; }
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody2D rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
			return _rigidbody;
		}
	}

	/// <summary>Gets impactHandler Component.</summary>
	public ImpactEventHandler impactHandler
	{ 
		get
		{
			if(_impactHandler == null) _impactHandler = GetComponent<ImpactEventHandler>();
			return _impactHandler;
		}
	}

	/// <summary>Gets eventsHandler Component.</summary>
	public EventsHandler eventsHandler
	{ 
		get
		{
			if(_eventsHandler == null) _eventsHandler = GetComponent<EventsHandler>();
			return _eventsHandler;
		}
	}

	/// <summary>BouncingBall's instance initialization.</summary>
	private void Awake()
	{
		eventsHandler.onTriggerEvent += OnImpactEvent;
	}

	/// <summary>BouncingBall's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		//Game.AddTargetToCamera(transform);
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	private void OnImpactEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		/*if(impactTags == null || impactTags.Length == 0 || _eventType != HitColliderEventTypes.Enter) return;

		GameObject obj = _info.collider.gameObject;

		foreach(GameObjectTag tag in impactTags)
		{
			if(obj.CompareTag(tag))
			{
				Debug.DrawRay(_info.contactPoint, _info.direction * 5.0f, Color.magenta, 5.0f);
				//RaycastHit2D[] hitInfos = Physics2D.SphereCastAll(transform.position, )

				rigidbody.Sleep();
				rigidbody.AddForce(Vector2.Scale(-_info.direction.normalized, force), ForceMode2D.Impulse);
				break;
			}
		}*/
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("[BouncingBall] Collided");
		if(impactTags == null || impactTags.Length == 0) return;

		GameObject obj = col.gameObject;
		Vector2 f = Vector2.zero;

		foreach(GameObjectTag tag in impactTags)
		{
			if(obj.CompareTag(tag))
			{
				foreach(ContactPoint2D point in col.contacts)
				{
					f += point.normal;
				}
				break;
			}
		}

		if(f.sqrMagnitude == 0.0f) return;

		rigidbody.Sleep();
		rigidbody.AddForce(f.normalized * force.magnitude, ForceMode2D.Impulse);
	}
}
}