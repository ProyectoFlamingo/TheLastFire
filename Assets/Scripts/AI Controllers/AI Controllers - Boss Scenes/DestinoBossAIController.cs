using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(DestinoDeckController))]
public class DestinoBossAIController : CharacterAIController<DestinoBoss>
{
	[Space(5f)]
	[Header("Deck:")]
	[SerializeField] private DestinoCard[] _cards; 				/// <summary>Destino's Cards.</summary>
	[Space(5f)]
	[SerializeField] private TransformData _headFallPointData; 	/// <summary>Fall Point's TransformData for the Removable Head.</summary>
	[Space(5f)]
	[SerializeField] private float _fallDuration; 				/// <summary>Removable Head's Falling Duration.</summary>
	[SerializeField] private float _fallenDuration; 			/// <summary>Duration Destino's Head is on the floor after it falls.</summary>
	[SerializeField] private float _headReturnDuration; 		/// <summary>Removable Head's return duration.</summary>
//#if UNITY_EDITOR
	[Space(5f)]
	[Header("Destino's Test:")]
	[SerializeField] private bool test; 						/// <summary>Test?.</summary>
	[SerializeField] private int testCardIndex; 				/// <summary>Test's Card Index.</summary>
	[Space(5f)]
	[SerializeField] private MeshFilter headMeshFilter; 		/// <summary>Removable Head's MeshFilter Component.</summary>
	private IEnumerator iterator; 								/// <summary>[Test] Iterator.</summary>
//#endif
	private DestinoDeckController _deckController; 				/// <summary>DestinoDeckController's Component.</summary>
	private Coroutine cardRoutine; 								/// <summary>Card Coroutine's Reference.</summary>
	private Coroutine fallenTolerance; 							/// <summary>Removable Head's Fallen Tolerance Coroutine reference.</summary>

#region Getters/Setters:
	/// <summary>Gets headFallPointData property.</summary>
	public TransformData headFallPointData { get { return _headFallPointData; } }

	/// <summary>Gets fallDuration property.</summary>
	public float fallDuration { get { return _fallDuration; } }

	/// <summary>Gets fallenDuration property.</summary>
	public float fallenDuration { get { return _fallenDuration; } }

	/// <summary>Gets headReturnDuration property.</summary>
	public float headReturnDuration { get { return _headReturnDuration; } }

	/// <summary>Gets deckController Component.</summary>
	public DestinoDeckController deckController
	{ 
		get
		{
			if(_deckController == null) _deckController = GetComponent<DestinoDeckController>();
			return _deckController;
		}
	}
#endregion

	/// <summary>Draws Gizmos on Editor mode when DestinoBossAIController's instance is selected.</summary>
	protected override void OnDrawGizmosSelected()
	{
//#if UNITY_EDITOR
		base.OnDrawGizmosSelected();

		if(headMeshFilter != null) Gizmos.DrawMesh(headMeshFilter.sharedMesh, headFallPointData.position, headFallPointData.rotation);
		VGizmos.DrawTransformData(headFallPointData);
//#endif
	}

	/// <summary>CharacterAIController's instance initialization.</summary>
	protected virtual void Awake()
	{
		base.Awake();

		deckController.Reset();
		deckController.CreateDeck(character);
		deckController.onCardSelected += OnCardSelected;

		foreach(DestinoCard card in deckController.cards)
		{
			card.onCardEvent += OnCardEvent;
		}
	}

	/// <summary>CharacterAIController's starting actions before 1st Update frame.</summary>
	protected virtual void Start()
	{
		base.Start();

//#if UNITY_EDITOR
		if(test)
		{
			int index = testCardIndex;
			iterator = deckController.cards[index].behavior.Routine(character);
		}
//#endif
	}
	
	/// <summary>CharacterAIController's tick at each frame.</summary>
	protected virtual void Update()
	{ 
		base.Update();

//#if UNITY_EDITOR
		if(!test) return;

		if(iterator != null && !iterator.MoveNext())
		{
			iterator = deckController.cards[testCardIndex].behavior.Routine(character);
		}
//#endif
	}

	/// <summary>Requests card to the DeckController.</summary>
	public void RequestCard()
	{
		this.StartCoroutine(deckController.Routine(character), ref behaviorCoroutine);
	}

	/// <summary>Throws Removable Head into floor.</summary>
	private void ThrowHeadIntoFloor()
	{
		character.rigHead.gameObject.SetActive(false);
		character.removableHead.gameObject.SetActive(true);
		character.removableHead.SetParent(null);
		this.StartCoroutine(character.removableHead.LerpTowardsData(headFallPointData, fallDuration, TransformProperties.PositionAndRotation, Space.World, OnHeadFalled, VMath.EaseInQuad), ref fallenTolerance);
	}

#region Callbacks:
	/// <summary>Callback invoked when a Card is selected from the most recent shuffling.</summary>
	/// <param name="_card">Selected Card.</param>
	public void OnCardSelected(DestinoCard _card)
	{
		if(_card != null && _card.behavior != null)
		this.StartCoroutine(_card.behavior.Routine(character), ref cardRoutine);
	}

	/// <summary>Callback invoked when the fallen's tolerance duration of a card reached its end.</summary>
	/// <param name="_card">Card's reference.</param>
	/// <param name="_event">Event Type.</param>
	private void OnCardEvent(DestinoCard _card, DestinoCardEvent _event)
	{
		switch(_event)
		{
			case DestinoCardEvent.FallenToleranceFinished:
			deckController.ReturnCard(character, _card, OnCardStored);
			break;

			case DestinoCardEvent.Hit:
			this.StartCoroutine(SlashCardIntoHead(_card));
			break;
		}
	}

	/// <summary>Callback invoked when the card has been stored [after the fallen tolerance has ended].</summary>
	private void OnCardStored()
	{
		RequestCard();
	}

	/// <summary>Callback invoked after the Removable Head falls into the floor.</summary>
	private void OnHeadFalled()
	{
		if(character.headHurtBox == null) return;

		Vector3 hurtBoxPosition = character.removableHead.position;
		hurtBoxPosition.z = 0.0f;

		character.headHurtBox.transform.position = hurtBoxPosition;
		character.headHurtBox.Activate(true);

		this.StartCoroutine(this.WaitSeconds(fallenDuration, OnFallenToleranceEnds), ref fallenTolerance);
	}

	/// <summary>Callback invoked when the Removable Head's Fallen Tolerance ends.</summary>
	private void OnFallenToleranceEnds()
	{
		character.headHurtBox.Activate(false);
		this.StartCoroutine(character.removableHead.LerpTowardsTransform(character.headPivot, headReturnDuration, TransformProperties.PositionAndRotation, Space.World, InterpolationType.Slerp, OnHeadReturned, VMath.EaseInQuad), ref fallenTolerance);
	}

	/// <summary>Callback internally called when the Head returns after its fall.</summary>
	private void OnHeadReturned()
	{
		character.removableHead.SetParent(character.headPivot);
		character.removableHead.gameObject.SetActive(false);
		character.rigHead.gameObject.SetActive(true);
		RequestCard();
	}
#endregion

#region Coroutines
	/// <summary>Makes card slash into Destino's Head.</summary>
	/// <param name="_card">Card to slassh into destino' head.</param>
	private IEnumerator SlashCardIntoHead(DestinoCard _card)
	{
		Transform rigHead = character.rigHead;
		Quaternion lookRotation = Quaternion.identity;
		Vector3 direction = (rigHead.position - _card.transform.position).normalized;
		float inverseRotationDuration = 1.0f / _card.rotationDuration;
		float inverseSlashDuration = 1.0f / _card.slashDuration;
		float t = 0.0f;
		float minDistance = _card.distance;
		float distance = 0.0f;
		bool activatedEvent = false;

		while(t < 1.0f)
		{
			distance = (rigHead.position - _card.transform.position).sqrMagnitude;
			lookRotation = Quaternion.LookRotation(rigHead.up);
			_card.transform.rotation = Quaternion.Slerp(_card.transform.rotation, lookRotation, Time.deltaTime * _card.rotationDuration);
			_card.transform.position += (direction.normalized * _card.slashSpeed * Time.deltaTime);

			if(!activatedEvent && distance <= minDistance)
			{ /// Call the head's throwing routine just once:
				activatedEvent = true;
				ThrowHeadIntoFloor();
			}

			t += (Time.deltaTime * inverseSlashDuration);
			yield return null;
		}

		_card.gameObject.SetActive(false);
	}
#endregion

}
}