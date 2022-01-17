using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum DestinoCardEvent
{
	FallenToleranceFinished,
	Hit
}

/// <summary>Event invoked when the fallen's tolerance duration of a card reached its end.</summary>
/// <param name="_card">Card's reference.</param>
/// <param name="_event">Event Type.</param>
public delegate void OnDestinoCardEvent(DestinoCard _card, DestinoCardEvent _event);

public class DestinoCard : MonoBehaviour
{
	public event OnDestinoCardEvent onCardEvent; 									/// <summary>OnDestinoCardEvent' event delegate.</summary>

	[SerializeField] private bool _run; 											/// <summary>Can this card be runnable?.</summary>
	[Space(5f)]
	[SerializeField] private DestinoScriptableCoroutine _behavior; 					/// <summary>Card's Behavior.</summary>
	[SerializeField] private int _entranceSoundIndex; 								/// <summary>Entrace SFX's Index.</summary>
	[SerializeField] private Renderer _cardRenderer; 								/// <summary>Card's Renderer.</summary>
	[SerializeField] private HitCollider2D _hurtBox; 								/// <summary>Card's HurtBox.</summary>
	[SerializeField] private GameObjectTag[] _hitTags; 								/// <summary>Tags that can hit the card.</summary>
	[Space(5f)]
	[Header("Card Travelling's Attributes:")]
	[Space(2.5f)]
	[Header("Towards Falling Point:")]
	[SerializeField] private TransformData _fallPointData; 							/// <summary>Fall's point data [with position and rotation].</summary>
	[SerializeField] private float _fallDuration; 									/// <summary>Falling's Duration.</summary>
	[SerializeField] private float _fallenDuration; 								/// <summary>Fallen tolerance's duration.</summary>
	[Space(2.5f)]
	[Header("Towards Destino's Head:")]
	[SerializeField] private Distance _distance; 									/// <summary>Minimum Distance for Card to Slash Destino's Head.</summary>
	[SerializeField] private float _rotationDuration; 								/// <summary>Duration to rotate so the card can slash Destino's Head.</summary>
	[SerializeField] private float _slashDuration; 									/// <summary>Slash's Duration.</summary>
	[SerializeField] private float _slashSpeed; 									/// <summary>Slash's Speed.</summary>
	[Space(5f)]
	[Header("FXs:")]
	[SerializeField] private ParticleEffectEmissionData _hitParticleEffect; 		/// <summary>Hit Particle-Effect's Emission Data.</summary>
	[SerializeField] private ParticleEffectEmissionData _headHitParticleEffect; 	/// <summary>Head Hit Particle-Effect's Emission Data.</summary>
	[SerializeField] private int _hitSoundIndex; 									/// <summary>Hit's Sound Index.</summary>
	[SerializeField] private int _headHitSoundIndex; 								/// <summary>Head Hit's Sound Index.</summary>
#if UNITY_EDITOR
	[SerializeField] private MeshFilter cardMeshFilter; 							/// <summary>Card's Mesh Filter.</summary>
	[SerializeField] private Color color; 											/// <summary>Gizmos' Color.</summary>
#endif
	private Coroutine fallenTolerance; 												/// <summary>Fallen Tolerance's Coroutine reference.</summary>

#region Getters/Setters:
	/// <summary>Gets run property.</summary>
	public bool run { get { return _run; } }

	/// <summary>Gets behavior Component.</summary>
	public DestinoScriptableCoroutine behavior
	{ 
		get
		{
			if(_behavior == null) _behavior = GetComponent<DestinoScriptableCoroutine>();
			return _behavior;
		}
		set { _behavior = value; }
	}

	/// <summary>Gets entranceSoundIndex property.</summary>
	public int entranceSoundIndex { get { return _entranceSoundIndex; } }

	/// <summary>Gets cardRenderer property.</summary>
	public Renderer cardRenderer { get { return _cardRenderer; } }

	/// <summary>Gets hurtBox property.</summary>
	public HitCollider2D hurtBox { get { return _hurtBox; } }

	/// <summary>Gets hitTags property.</summary>
	public GameObjectTag[] hitTags { get { return _hitTags; } }

	/// <summary>Gets fallPointData property.</summary>
	public TransformData fallPointData { get { return _fallPointData; } }

	/// <summary>Gets fallDuration property.</summary>
	public float fallDuration { get { return _fallDuration; } }

	/// <summary>Gets fallenDuration property.</summary>
	public float fallenDuration { get { return _fallenDuration; } }

	/// <summary>Gets rotationDuration property.</summary>
	public float rotationDuration { get { return _rotationDuration; } }

	/// <summary>Gets slashDuration property.</summary>
	public float slashDuration { get { return _slashDuration; } }

	/// <summary>Gets slashSpeed property.</summary>
	public float slashSpeed { get { return _slashSpeed; } }

	/// <summary>Gets distance property.</summary>
	public Distance distance { get { return _distance; } }

	/// <summary>Gets hitParticleEffect property.</summary>
	public ParticleEffectEmissionData hitParticleEffect { get { return _hitParticleEffect; } }

	/// <summary>Gets headHitParticleEffect property.</summary>
	public ParticleEffectEmissionData headHitParticleEffect { get { return _headHitParticleEffect; } }

	/// <summary>Gets hitSoundIndex property.</summary>
	public int hitSoundIndex { get { return _hitSoundIndex; } }

	/// <summary>Gets headHitSoundIndex property.</summary>
	public int headHitSoundIndex { get { return _headHitSoundIndex; } }
#endregion

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		//if(behavior != null) behavior.DrawGizmos();

		if(cardMeshFilter == null) return;

		Gizmos.color = color;
		Gizmos.DrawWireMesh(cardMeshFilter.sharedMesh, fallPointData.position, fallPointData.rotation);
		VGizmos.DrawTransformData(fallPointData);

		hitParticleEffect.DrawGizmos();
		headHitParticleEffect.DrawGizmos();
#endif
	}

	/// <summary>Callback invoked when DestinoCard's instance is disabled.</summary>
	private void OnDisable()
	{
		//Debug.Log("[DestinoCard] Card " + gameObject.name + " disabled.");
	}

	/// <summary>DestinoCard's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		if(behavior != null) behavior.onCoroutineEnds += OnCardBehaviorEnds;
		if(hurtBox != null) hurtBox.onTriggerEvent2D += OnTriggerEvent2D;
	}

	/// <summary>Callback invoked when DestinoCard's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		if(behavior != null) behavior.onCoroutineEnds -= OnCardBehaviorEnds;
	}

	/// <summary>Callback invoked when the card's behavior reaches an end.</summary>
	private void OnCardBehaviorEnds()
	{
		this.StartCoroutine(transform.LerpTowardsData(fallPointData, fallDuration, TransformProperties.PositionAndRotation, Space.World, OnCardFalled));
	}

	/// <summary>Callback invoked when the card finished falling.</summary>
	private void OnCardFalled()
	{
		if(hurtBox == null) return;

		Vector3 hurtBoxPosition = transform.position;
		hurtBoxPosition.z = 0.0f;

		hurtBox.transform.position = hurtBoxPosition;
		hurtBox.Activate(true);

		this.StartCoroutine(this.WaitSeconds(fallenDuration, OnFallenToleranceFinished), ref fallenTolerance);
	}

	/// <summary>Callback invoked when the Fallen tolerance finishes.</summary>
	private void OnFallenToleranceFinished()
	{
		hurtBox.Activate(false);
		if(onCardEvent != null) onCardEvent(this, DestinoCardEvent.FallenToleranceFinished);
	}

	/// <summary>Callback invoked when this Hit Collider2D intersects with another GameObject.</summary>
	/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_hitColliderID">Optional ID of the HitCollider2D.</param>
	public void OnTriggerEvent2D(Collider2D _collider, HitColliderEventTypes _eventType, int _hitColliderID = 0)
	{
		GameObject obj = _collider.gameObject;

		foreach(GameObjectTag tag in hitTags)
		{
			if(obj.CompareTag(tag))
			{
				this.DispatchCoroutine(ref fallenTolerance);
				if(onCardEvent != null) onCardEvent(this, DestinoCardEvent.Hit);
				hurtBox.Activate(false);
				hitParticleEffect.EmitParticleEffects();
				AudioController.PlayOneShot(SourceType.SFX, 0, hitSoundIndex);

				break;
			}
		}
	}
}
}