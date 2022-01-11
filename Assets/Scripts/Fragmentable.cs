using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
[RequireComponent(typeof(ImpactEventHandler))]
[RequireComponent(typeof(EventsHandler))]
[RequireComponent(typeof(VCameraTarget))]
[RequireComponent(typeof(Rigidbody2D))]
public class Fragmentable : PoolGameObject
{
	[SerializeField] private GameObjectTag[] _tags; 						/// <summary>Tags of GameObjects that fragment this Object.</summary>
	[SerializeField] private HitCollider2D[] _pieces; 						/// <summary>Fragmentable's Pieces.</summary>
	[SerializeField] private float _forceScalar; 							/// <summary>Additional Force's Scalar.</summary>
	[SerializeField] private float _fragmentationDuration; 					/// <summary>Fragmentation's Duration.</summary>
	[Space(5f)]
	[SerializeField] private ParticleEffectEmissionData _particleEffect; 	/// <summary>Particle Effect Emitted when this Fragmentable is fragmented.</summary>
	[SerializeField] private int _soundIndex; 								/// <summary>Sound Index.</summary>
	private TransformData[] _piecesData; 									/// <summary>Fragmentable Pieces' Bake data.</summary>
	private bool _fragmented; 												/// <summary>Is the object fragmented?.</summary>
	private Rigidbody2D _rigidbody; 										/// <summary>Rigidbody's Component.</summary>
	private EventsHandler _eventsHandler; 									/// <summary>EventsHandler's Component.</summary>
	private VCameraTarget _cameraTarget; 									/// <summary>VCameraTarget's Component.</summary>
	private ImpactEventHandler _impactHandler; 								/// <summary>ImpactEventsHandler's Component.</summary>
	private Coroutine defragmentation; 										/// <summary>Defragmentation's Coroutine reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets tags property.</summary>
	public GameObjectTag[] tags
	{
		get { return _tags; }
		set { _tags = value; }
	}

	/// <summary>Gets and Sets pieces property.</summary>
	public HitCollider2D[] pieces
	{
		get { return _pieces; }
		set { _pieces = value; }
	}

	/// <summary>Gets and Sets piecesData property.</summary>
	public TransformData[] piecesData
	{
		get { return _piecesData; }
		set { _piecesData = value; }
	}

	/// <summary>Gets and Sets forceScalar property.</summary>
	public float forceScalar
	{
		get { return _forceScalar; }
		set { _forceScalar = value; }
	}

	/// <summary>Gets and Sets fragmentationDuration property.</summary>
	public float fragmentationDuration
	{
		get { return _fragmentationDuration; }
		set { _fragmentationDuration = value; }
	}

	/// <summary>Gets and Sets fragmented property.</summary>
	public bool fragmented
	{
		get { return _fragmented; }
		set { _fragmented = value; }
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

	/// <summary>Gets eventsHandler Component.</summary>
	public EventsHandler eventsHandler
	{ 
		get
		{
			if(_eventsHandler == null) _eventsHandler = GetComponent<EventsHandler>();
			return _eventsHandler;
		}
	}

	/// <summary>Gets cameraTarget Component.</summary>
	public VCameraTarget cameraTarget
	{ 
		get
		{
			if(_cameraTarget == null) _cameraTarget = GetComponent<VCameraTarget>();
			return _cameraTarget;
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

	/// <summary>Gets particleEffect property.</summary>
	public ParticleEffectEmissionData particleEffect { get { return _particleEffect; } }

	/// <summary>Gets soundIndex property.</summary>
	public int soundIndex { get { return _soundIndex; } }
#endregion

	/// <summary>Draws Gizmos on Editor mode when Fragmentable's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		particleEffect.DrawGizmos();
	}

	/// <summary>Callback invoked when Fragmentable's instance is disabled.</summary>
	private void OnDisable()
	{
		//Defragmentate();
	}

	/// <summary>Fragmentable's instance initialization.</summary>
	private void Awake()
	{
		BakePiecesData();
		eventsHandler.onTriggerEvent += OnTriggerEvent;
		fragmented = false;
	}

	[Button("Bake Pieces' Data")]
	/// <summary>Bakes Pieces' Data.</summary>
	public void BakePiecesData()
	{
		if(pieces == null) return;

		int length = pieces.Length;

		if(piecesData == null || piecesData.Length != length) piecesData = new TransformData[length];

		for(int i = 0; i < length; i++)
		{
			piecesData[i] = pieces[i].transform;
			pieces[i].transform.parent = transform;
			pieces[i].ID = i;
		}

		fragmented = true;
	}

	/// <summary>Fragmentates Pieces.</summary>
	public void Fragmentate(Vector3 _force, Action onFragmentationEnds = null)
	{
		if(fragmented) return;

		particleEffect.EmitParticleEffects();
		AudioController.PlayOneShot(SourceType.SFX, 0, soundIndex);

		this.StartCoroutine(Fragmentation(_force, onFragmentationEnds), ref defragmentation);
		fragmented = true;
	}

	/// <summary>Gathers pieces into their baked data.</summary>
	public void Defragmentate()
	{
		if(!fragmented) return;

		TransformData pieceData = default(TransformData);
		int i = 0;

		foreach(HitCollider2D piece in pieces)
		{
			if(piece != null)
			{
				pieceData = piecesData[i];
				piece.transform.position = pieceData.position;
				piece.transform.rotation = pieceData.rotation;
				piece.transform.localScale = pieceData.scale;
				piece.transform.parent = transform;
				piece.rigidbody.SetForTrigger();
			}

			i++;
		}

		fragmented = false;
	}

	/// <summary>Gathers pieces into their baked data at given duration.</summary>
	/// <param name="_duration">Defragmentation's Duration.</param>
	/// <param name="onDefragmentationEnds">Optional callback invoked when the defragmentation ends [null by default].</param>
	public void Defragmentate(float _duration, Action onDefragmentationEnds = null)
	{
		/*if(fragmented)
		this.StartCoroutine(Defragmentation(_duration, onDefragmentationEnds), ref defragmentation);*/
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	private void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		if(fragmented) return;

		GameObject obj = _info.collider.gameObject;

		if(tags != null) foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				Fragmentate(_info.direction, null);
				return;
			}
		}
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		Defragmentate();
		base.OnObjectReset();
		impactHandler.ActivateHitBoxes(true);
	}

	/// <summary>Fragments pieces for some time.</summary>
	private IEnumerator Fragmentation(Vector3 f, Action onFragmentationEnds = null)
	{
		float t = 0.0f;
		float inverseDuration = 1.0f / fragmentationDuration;
		Rigidbody2D body = null;
		SecondsDelayWait wait = new SecondsDelayWait(fragmentationDuration);

		Debug.DrawRay(transform.position, f.normalized * 5.0f, Color.cyan, 5.0f);

		foreach(HitCollider2D piece in pieces)
		{
			body = piece.rigidbody;
			piece.SetTrigger(false);
			body.transform.parent = null;
			body.SetForDynamicBody();
			body.AddForce(f * forceScalar, ForceMode2D.Impulse);
			body.AddTorque(Mathf.Sign(f.x) * forceScalar, ForceMode2D.Impulse);

		}

		while(wait.MoveNext()) yield return null;

		foreach(HitCollider2D piece in pieces)
		{
			piece.gameObject.SetActive(false);
		}

		gameObject.SetActive(false);

		if(onFragmentationEnds != null) onFragmentationEnds();
	}

	/// <summary>Gathers pieces into their baked data at given duration.</summary>
	/// <param name="_duration">Defragmentation's Duration.</param>
	/// <param name="onDefragmentationEnds">Optional callback invoked when the defragmentation ends [null by default].</param>
	private IEnumerator Defragmentation(float _duration, Action onDefragmentationEnds = null)
	{
		int length = pieces.Length;
		int i = 0;
		TransformData[] currentPiecesData = new TransformData[length];
		TransformData pieceData = default(TransformData);
		TransformData currentData = default(TransformData);
		float t = 0.0f;
		float inverseDuration = 1.0f / _duration;

		for(i = 0; i < length; i++)
		{
			if(pieces[i] != null) currentPiecesData[i] = pieces[i].transform;
		}

		i = 0;

		while(t < 1.0f)
		{
			foreach(HitCollider2D piece in pieces)
			{
				if(piece != null)
				{
					pieceData = piecesData[i];
					currentData = currentPiecesData[i];

					piece.transform.position = Vector3.Lerp(currentData.position, pieceData.position, t);
					piece.transform.rotation = Quaternion.Lerp(currentData.rotation, pieceData.rotation, t);
					piece.transform.localScale = Vector3.Lerp(currentData.localScale, pieceData.scale, t);
				}

				i++;
			}
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		Defragmentate();
		if(onDefragmentationEnds != null) onDefragmentationEnds();
	}
}
}