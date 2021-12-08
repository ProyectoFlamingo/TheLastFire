using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Event invoked when a Card is selected on the most recen shuffle [or selection process].</summary>
/// <param name="_card">Card that was selected on the selection process.</param>
public delegate void OnCardSelected(DestinoCard _card);

public class DestinoDeckController : DestinoScriptableCoroutine
{
	public event OnCardSelected onCardSelected; 					/// <summary>OnCardSelected's Event Delegate.</summary>

	[Space(5f)]
	[SerializeField] private DestinoCard[] _cards; 					/// <summary>Deck's Cards.</summary>
	[Space(5f)]
	[Header("Card Rotation's Attributes:")]
	[SerializeField] private EulerRotation _faceUpRotation; 		/// <summary>Card's Face-Up Rotation.</summary>
	[SerializeField] private EulerRotation _faceDownRotation; 		/// <summary>Card's Face-Down Rotation.</summary>
	[SerializeField] private float _rotationDuration; 				/// <summary>Card's Rotation Duration.</summary>
	[Space(5f)]
	[Header("Selected Card's Attributes:")]
	[SerializeField] private Vector3 _selectedCardPoint; 			/// <summary>Selected Card's Point.</summary>
	[SerializeField] private float _selectedCardDuration; 			/// <summary>Selected Card's Duration towards its point.</summary>
	[Space(5f)]
	[SerializeField] private Vector3 _deckPoint; 					/// <summary>Deck's Origin Point.</summary>
	[SerializeField] private float _cardOffset; 					/// <summary>Card's offset when stacked on the deck.</summary>
	[Space(5f)]
	[Header("Deck Spawning's Attributes:")]
	[SerializeField] private float _positioningDuration; 			/// <summary>Card Positioning's Duration.</summary>
	[Space(5f)]
	[Header("Deck Presentation's Attributes:")]
	[SerializeField] private int _spawnEffectIndex; 				/// <summary>Deck's apparition Particle Effect's index.</summary>
	[SerializeField] private Vector3 _presentationPoint; 			/// <summary>Deck Presentation's Point.</summary>
	[SerializeField] private float _cardSpacing; 					/// <summary>Spacing between each card when the deck is presented.</summary>
	[SerializeField] private float _deckPositioningDuration; 		/// <summary>Deck Positioning's Duration.</summary>
	[SerializeField] private float _presentationDuration; 			/// <summary>Presentation's Duration.</summary>
	[Space(5f)]
	[Header("Deck Storing's Attributes:")]
	[SerializeField] private float _storeDuration; 					/// <summary>Deck Storing's Duration.</summary>
	[Space(5f)]
	[Header("Deck Shuffling Attributes:")]
	[SerializeField] private float _zOffset; 						/// <summary>Depth's offset when each card is organized into a deck.</summary>
	[SerializeField] private float _shufflingDuration; 				/// <summary>Shuffling's Duration.</summary>
	[SerializeField] private float _shufflingRadius; 				/// <summary>Shuffling's Radius.</summary>
	[SerializeField] private float _shufflingSpeed; 				/// <summary>Shuffling's Speed.</summary>
	[SerializeField] private float _shufflingCycles; 				/// <summary>Times a card passes through the center of the deck [treated semantically as cycles].</summary>
	[Space(5f)]
	[Header("Orbiting Attributes:")]
	[SerializeField] private float _yOffset; 						/// <summary>Offset on the Y-Axis when orbiting around Destino.</summary>
	[SerializeField] private float _orbitDuration; 					/// <summary>Orbiting's Duration.</summary>
	[SerializeField] private float _orbitRadius; 					/// <summary>Orbiting's Radius.</summary>
	[SerializeField] private float _orbitSpeed; 					/// <summary>Orbiting's Speed.</summary>
/*#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 				/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 				/// <summary>Gizmos' Radius.</summary>
#endif*/
	private StackQueue<DestinoCard> _deck; 							/// <summary>Deck's StackQueue.</summary>
	private DestinoCard _selectedCard; 								/// <summary>Currently Selected Card.</summary>

#region Getters/Setters:
	/// <summary>Gets cards property.</summary>
	public DestinoCard[] cards { get { return _cards; } }

	/// <summary>Gets faceUpRotation property.</summary>
	public EulerRotation faceUpRotation { get { return _faceUpRotation; } }

	/// <summary>Gets faceDownRotation property.</summary>
	public EulerRotation faceDownRotation { get { return _faceDownRotation; } }

	/// <summary>Gets selectedCardPoint property.</summary>
	public Vector3 selectedCardPoint { get { return _selectedCardPoint; } }

	/// <summary>Gets deckPoint property.</summary>
	public Vector3 deckPoint { get { return _deckPoint; } }

	/// <summary>Gets presentationPoint property.</summary>
	public Vector3 presentationPoint { get { return _presentationPoint; } }

	/// <summary>Gets spawnEffectIndex property.</summary>
	public int spawnEffectIndex { get { return _spawnEffectIndex; } }

	/// <summary>Gets rotationDuration property.</summary>
	public float rotationDuration { get { return _rotationDuration; } }

	/// <summary>Gets selectedCardDuration property.</summary>
	public float selectedCardDuration { get { return _selectedCardDuration; } }

	/// <summary>Gets cardOffset property.</summary>
	public float cardOffset { get { return _cardOffset; } }

	/// <summary>Gets positioningDuration property.</summary>
	public float positioningDuration { get { return _positioningDuration; } }

	/// <summary>Gets cardSpacing property.</summary>
	public float cardSpacing { get { return _cardSpacing; } }

	/// <summary>Gets deckPositioningDuration property.</summary>
	public float deckPositioningDuration { get { return _deckPositioningDuration; } }

	/// <summary>Gets presentationDuration property.</summary>
	public float presentationDuration { get { return _presentationDuration; } }

	/// <summary>Gets storeDuration property.</summary>
	public float storeDuration { get { return _storeDuration; } }

	/// <summary>Gets zOffset property.</summary>
	public float zOffset { get { return _zOffset; } }

	/// <summary>Gets shufflingDuration property.</summary>
	public float shufflingDuration { get { return _shufflingDuration; } }

	/// <summary>Gets shufflingRadius property.</summary>
	public float shufflingRadius { get { return _shufflingRadius; } }

	/// <summary>Gets shufflingSpeed property.</summary>
	public float shufflingSpeed { get { return _shufflingSpeed; } }

	/// <summary>Gets shufflingCycles property.</summary>
	public float shufflingCycles { get { return _shufflingCycles; } }

	/// <summary>Gets yOffset property.</summary>
	public float yOffset { get { return _yOffset; } }

	/// <summary>Gets orbitDuration property.</summary>
	public float orbitDuration { get { return _orbitDuration; } }

	/// <summary>Gets orbitRadius property.</summary>
	public float orbitRadius { get { return _orbitRadius; } }

	/// <summary>Gets orbitSpeed property.</summary>
	public float orbitSpeed { get { return _orbitSpeed; } }

	/// <summary>Gets and Sets deck property.</summary>
	public StackQueue<DestinoCard> deck
	{
		get { return _deck; }
		private set { _deck = value; }
	}

	/// <summary>Gets and Sets selectedCard property.</summary>
	public DestinoCard selectedCard
	{
		get { return _selectedCard; }
		private set { _selectedCard = value; }
	}
#endregion

	/// <summary>Draws Gizmos [only called internally if drawGizmos' flag is turned on].</summary>
	protected override void DrawGizmos()
	{
/*#if UNITY_EDITOR
		base.DrawGizmos();

		Gizmos.color = gizmosColor;

		Gizmos.DrawWireSphere(selectedCardPoint, gizmosRadius);
		Gizmos.DrawWireSphere(deckPoint, gizmosRadius);
		Gizmos.DrawWireSphere(presentationPoint, gizmosRadius);
#endif*/
	}

	/// <summary>Resets DestinoDeckController's instance to its default values.</summary>
	public void Reset()
	{
		deck = null;
		onCardSelected = null;
		selectedCard = null;
	}

	/// <summary>Creates and shuffles deck from the cards available on the scene.</summary>
	/// <param name="boss">Boss' Reference.</param>
	public void CreateDeck(DestinoBoss boss)
	{
		
		/// Internal Shuffling:
		int deckLength = cards.Length;
		int[] cardsIndices = VMath.GetUniqueRandomSet(deckLength);		/// Shuffled indices.
		if(deck == null) deck = new StackQueue<DestinoCard>(/*deckLength*/);
		DestinoCard card = null;

		for(int i = 0; i < deckLength; i++)
		{
			card = cards[cardsIndices[i]];

			if(!card.run) continue;

			deck.Push(card);
			card.transform.position = boss.transform.position;
			card.transform.rotation = faceDownRotation;
			card.transform.localScale = Vector3.zero;
			card.gameObject.SetActive(true);
			//card. transform.position = deckPoint.WithAddedZ(_cardOffset);
		}
		
	}

	/// <summary>Returns card into deck.</summary>
	/// <param name="boss">Destino's Reference.</param>
	/// <param name="_card">Card to return into deck.</param>
	/// <param name="onCardReturned">Callback invoked when the card returns to the deck.</param>
	public void ReturnCard(DestinoBoss boss, DestinoCard _card, Action onCardReturned)
	{
		if(_card == null) return;
		
		deck.Enqueue(_card);
		boss.StartCoroutine(ReturnCardRoutine(boss, _card, onCardReturned));
	}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{
		/// Create and shuffle deck:
		if(deck == null || deck.Count == 0) CreateDeck(boss);

		if(!DestinoSceneController.Instance.deckPresented)
		{
			/// Spawn and present cards:
			IEnumerator deckPresentation = SpawnAndPresentCards(boss);
			while(deckPresentation.MoveNext()) yield return null;			
			DestinoSceneController.Instance.deckPresented = true;
		}
		else
		{
			IEnumerator deckSpawn = SpawnCards(boss);
			while(deckSpawn.MoveNext()) yield return null;
		}

		/// Shuffle deck:
		IEnumerator deckShuffling = ShuffleDeck();
		while(deckShuffling.MoveNext()) yield return null;

		/// Position Cards around Destino:
		IEnumerator spawning = PositionCardsAroundDestino(boss);
		while(spawning.MoveNext()) yield return null;

		/// Orbit Cards around Destino:
		SecondsDelayWait wait = new SecondsDelayWait(_orbitDuration);
		Vector3 offset = Vector3.up * yOffset;
		IEnumerator rotationRoutine = VCoroutines.RotateAroundTransform(deck.ToArray<DestinoCard>(), boss.transform, Vector3.up, offset, orbitSpeed, orbitRadius, wait.MoveNext); 
		while(rotationRoutine.MoveNext()) yield return null;

		/// Present Selected Card:
		IEnumerator presentSelectedCard = PresentSelectedCard(boss);
		while(presentSelectedCard.MoveNext()) yield return null;

		/// Store Deck:
		IEnumerator storeDeck = StoreDeck(boss);
		while(storeDeck.MoveNext()) yield return null;
	}

	/// <summary>Spawns and scales cards before orbiting them around Destino.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator PositionCardsAroundDestino(Boss boss)
	{
		int size = deck.Count;
		int i = 0;
		Vector3[] spawnPositions = new Vector3[size];
		Vector3 originalPosition = boss.transform.position;
		Vector3 destinyPosition = Vector3.zero;
		float fCount = (float)deck.Count;
		float angleDistribution = 360.0f / fCount;
		float t = 0.0f;
		float inverseDuration = 1.0f / positioningDuration;
		Quaternion rotation = Quaternion.Euler(Vector3.up * angleDistribution);
		Quaternion currentRotation = boss.transform.rotation * rotation;

		originalPosition.y += yOffset;

		foreach(DestinoCard card in deck)
		{
			spawnPositions[i] = originalPosition + (currentRotation * (Vector3.forward * orbitRadius));
			currentRotation *= rotation;
			i++;
		}

		while(t < 1.0f)
		{
			i = 0;

			foreach(DestinoCard card in deck)
			{
				originalPosition = card.transform.position;
				destinyPosition = spawnPositions[i];

				card.transform.position = Vector3.Lerp(originalPosition, destinyPosition, t);
				i++;
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		i = 0;

		foreach(DestinoCard card in deck)
		{
			destinyPosition = spawnPositions[i];
			card.transform.position = destinyPosition;
			i++;
		}
	}

	/// <summary>Presents and positions selected Card.</summary>
	/// <param name="boss">Destino's reference.</param>
	private IEnumerator PresentSelectedCard(DestinoBoss boss)
	{
		selectedCard = deck.Pop();
		float t = 0.0f;
		float inverseRotationDuration = 1.0f / rotationDuration;
		float inverseDuration = 1.0f / selectedCardDuration;
		Vector3 initialPosition = selectedCard.transform.position;

		while(t < 1.0f)
		{
			selectedCard.transform.position = Vector3.Lerp(initialPosition, selectedCardPoint, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		t = 0.0f;

		while(t < 1.0f)
		{
			selectedCard.transform.rotation = Quaternion.Lerp(faceDownRotation, faceUpRotation, t);
			t += (Time.deltaTime * inverseRotationDuration);
			yield return null;
		}

		selectedCard.transform.rotation = faceUpRotation;

		if(onCardSelected != null) onCardSelected(selectedCard);
	}

	/// <summary>Return Card into deck.</summary>
	/// <param name="boss">Destino's Reference.</param>
	/// <param name="_card">Card to return.</param>
	/// <param name="onCardReturned">Callback invoked when the card returns to the deck.</param>
	private IEnumerator ReturnCardRoutine(DestinoBoss boss, DestinoCard _card, Action onCardReturned)
	{
		Quaternion cardRotation = _card.transform.rotation;
		float inverseDuration = 1.0f / storeDuration;
		float t = 0.0f;

		while(t < 1.0f)
		{
			_card.transform.position = Vector3.Lerp(_card.transform.position, boss.transform.position, t);
			_card.transform.rotation = Quaternion.Lerp(cardRotation, faceDownRotation, t);
			_card.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_card.transform.localScale = Vector3.zero;
		_card.transform.position = boss.transform.position;
	
		if(onCardReturned != null) onCardReturned();
	}

	/// <summary>Stores Deck into its original place.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator StoreDeck(DestinoBoss boss)
	{
		if(deck.Count <= 0) yield break;

		float t = 0.0f;
		float inverseDuration = 1.0f / storeDuration;

		while(t < 1.0f)
		{
			foreach(DestinoCard card in deck)
			{
				card.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
				card.transform.position = Vector3.Lerp(card.transform.position, boss.transform.position, t);
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		foreach(DestinoCard card in deck)
		{
			card.transform.localScale = Vector3.zero;
			card.transform.position = boss.transform.position;
		}
	}

	/// <summary>Shuffles Deck [Visually].</summary>
	private IEnumerator ShuffleDeck()
	{
		Vector3[] originalCardsPositions = new Vector3[deck.Count];
		float radiusDivision = shufflingRadius / (float)deck.Count;
		float inverseShufflingDuration = 1.0f / shufflingDuration;
		float inverseDeckPositioning = 1.0f / deckPositioningDuration;
		float t = 0.0f;
		float x = shufflingCycles * 360.0f * Mathf.Deg2Rad;
		Vector3 offsetedDeckPoint = deckPoint;
		int index = 0;

		foreach(DestinoCard card in deck)
		{
			originalCardsPositions[index] = card.transform.position;
			index++;
		}

		while(t < (1.0f + Mathf.Epsilon))
		{
			index = 0;

			foreach(DestinoCard card in deck)
			{
				card.transform.position = Vector3.Lerp(originalCardsPositions[index], offsetedDeckPoint, t);
				offsetedDeckPoint.z += zOffset;
				index++;
			}

			offsetedDeckPoint = deckPoint;
			t += (Time.deltaTime * inverseDeckPositioning);
			yield return null;
		}

		t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			IEnumerator<DestinoCard> deckIterator = deck.IterateAsQueue();
			float i = 1.0f;
			float sign = 1.0f;

			while(deckIterator.MoveNext())
			{
				Vector3 cardPosition = deckPoint;
				float sinusoidalDisplacement = Mathf.Sin(t * x) * (i * shufflingRadius);
				cardPosition.x = (deckPoint.x + (sinusoidalDisplacement * sign));
				cardPosition.z += (zOffset * i);
				deckIterator.Current.transform.position = cardPosition;
				i++;
				sign *= -1.0f;
			}

			t += (Time.deltaTime * inverseShufflingDuration);
			yield return null;
		}

		foreach(DestinoCard card in deck)
		{
			card.transform.position = deckPoint;
		}
	}

	/// <summary>Spawns and Presents Cards before Shuffling.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator SpawnAndPresentCards(DestinoBoss boss)
	{
		if(deck == null || deck.Count == 0) yield break;

		Vector3 position = presentationPoint;
		Vector3 originalScale = Vector3.zero;
		Vector3 destinyScale = Vector3.one;
		float cardWidth = deck.PeekStack().cardRenderer.bounds.size.x;
		float cardHalfWidth = cardWidth * 0.5f;
		float fCount = (float)deck.Count;
		float spaces = fCount - 1.0f;
		float totalWidth = (cardWidth * fCount) + (cardSpacing * spaces);
		float halfWidth = totalWidth * 0.5f;
		float i = 0.0f;
		float t = 0.0f;
		float inversePositioningDuration = 1.0f / positioningDuration;
		float inverseRotationDuration = 1.0f / rotationDuration;

		position.x -= halfWidth;

		foreach(DestinoCard card in deck)
		{
			card.transform.localScale = originalScale;
			card.transform.rotation = faceUpRotation;
		}

		while(t < 1.0f)
		{
			i = 0.0f;

			foreach(DestinoCard card in deck)
			{
				Vector3 cardPosition = position;

				cardPosition.x += ((cardHalfWidth + cardSpacing) * i);
				card.transform.position = Vector3.Lerp(boss.transform.position, cardPosition, t);
				card.transform.localScale = Vector3.Lerp(originalScale, destinyScale, t);

				i++;
			}

			t += (Time.deltaTime * inversePositioningDuration);
			yield return null;
		}

		foreach(DestinoCard card in deck)
		{
			card.transform.localScale = destinyScale;
		}

		SecondsDelayWait wait = new SecondsDelayWait(presentationDuration);

		while(wait.MoveNext()) yield return null;

		t = 0.0f;

		while(t < 1.0f)
		{
			foreach(DestinoCard card in deck)
			{
				card.transform.rotation = Quaternion.Lerp(faceUpRotation, faceDownRotation, t);
			}

			t += (Time.deltaTime * inverseRotationDuration);
			yield return null;
		}

		foreach(DestinoCard card in deck)
		{
			card.transform.rotation = faceDownRotation;
		}
	}

	/// <summary>Spawns [Scales also] cards into deck point.</summary>
	/// <param name="boss">Destino's Reference.</param>
	private IEnumerator SpawnCards(DestinoBoss boss)
	{
		Vector3 originalScale = Vector3.zero;
		Vector3 destinyScale = Vector3.one;
		float inverseDuration = 1.0f / positioningDuration;
		float t = 0.0f;

		foreach(DestinoCard card in deck)
		{
			card.transform.position = boss.transform.position;
			//card.transform.rotation = faceUpRotation;
			card.transform.rotation = faceDownRotation;

			
		}

		while(t < 1.0f)
		{
			foreach(DestinoCard card in deck)
			{
				card.transform.localScale = Vector3.Lerp(originalScale, destinyScale, t);
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		foreach(DestinoCard card in deck)
		{
			card.transform.localScale = destinyScale;
		}

		PoolManager.RequestParticleEffect(spawnEffectIndex, deckPoint, Quaternion.identity);
	}
}
}