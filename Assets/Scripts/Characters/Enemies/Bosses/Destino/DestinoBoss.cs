using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/* States' IDs:
 0: Idle
 1: Laugh
 2: Chant State
 3: Lala
 4: Laaa
*/

[RequireComponent(typeof(SensorSystem2D))]
[RequireComponent(typeof(RigidbodyMovementAbility))]
[RequireComponent(typeof(RotationAbility))]
[RequireComponent(typeof(JumpAbility))]
[RequireComponent(typeof(SteeringSnake))]
public class DestinoBoss : Boss
{
	public const int ID_EVENT_NONE = 0; 								/// <summary>Null Event's ID.</summary>
	public const int ID_STATE_IDLE_NORMAL = 0; 							/// <summary>Normal Idle's State ID on the AnimatorController.</summary>
	public const int ID_STATE_IDLE_LAUGH = 1; 							/// <summary>Laugh Idle's State ID on the AnimatorController.</summary>
	public const int ID_STATE_CHANT = 2; 								/// <summary>Chant's State ID on the AnimatorController.</summary>
	public const int ID_STATE_NOTE_LALA = 3; 							/// <summary>Lala Note's State ID on the AnimatorController.</summary>
	public const int ID_STATE_NOTE_LAAA = 4; 							/// <summary>Laaa Note's State ID on the AnimatorController.</summary>
	public const int ID_STATE_DEAD = 5; 								/// <summary>Dead's State ID on the AnimatorController.</summary>

	[SerializeField] private FloatRange _laughFrequency; 				/// <summary>Laughing's Frequency.</summary>
	[SerializeField] private AnimatorCredential _stateIDCredential; 	/// <summary>State ID's Credential.</summary>
	[Header("Heads' Attributes:")]
	[SerializeField] private Transform _headPivot; 						/// <summary>Head's Pivot [for both Heads].</summary>
	[SerializeField] private Transform _rigHead; 						/// <summary>Destino's Rig Head.</summary>
	[SerializeField] private Transform _removableHead; 					/// <summary>Destino's Removable Head.</summary>
	[SerializeField] private HitCollider2D _headHurtBox; 				/// <summary>Removable Head's HutrBox.</summary>
	[SerializeField] private TransformData _headFallPointData; 			/// <summary>Fall Point's TransformData for the Removable Head.</summary>
	[SerializeField] private float _fallDuration; 						/// <summary>Removable Head's Falling Duration.</summary>
	[SerializeField] private float _fallenDuration; 					/// <summary>Duration Destino's Head is on the floor after it falls.</summary>
	[SerializeField] private float _headReturnDuration; 				/// <summary>Removable Head's return duration.</summary>
	[Space(5f)]
	[SerializeField] private DestinoDeckController _deckController; 	/// <summary>Deck's Controller.</summary>
	[SerializeField] private DestinoCard[] _cards; 						/// <summary>Destino's Cards.</summary>
	[Space(5f)]
	[Header("Props:")]
	[SerializeField] private ContactWeapon _scythe; 					/// <summary>Scythe's reference.</summary>
	[SerializeField] private ContactWeapon _leftDrumstick; 				/// <summary>Left Drumstick's renderer.</summary>
	[SerializeField] private ContactWeapon _rightDrumstick; 			/// <summary>Right Drumstick's renderer.</summary>
	[SerializeField] private ContactWeapon _trumpet; 					/// <summary>Trumpet's reference.</summary>
	[SerializeField] private ContactWeapon _cymbals; 					/// <summary>Cymbals' Reference.</summary>
	[Space(5f)]
	[Header("Sound FXs' References:")]
	[Space(5f)]
	[Header("Destino's Sounds:")]
	[SerializeField] private CollectionIndex _damageTakenSoundIndex; 	/// <summary>Damage taken's Sound's Index.</summary>
	[Space(5f)]
	[Header("Death's Sounds:")]
	[SerializeField] private CollectionIndex _buildUpSoundIndex; 		/// <summary>Build-Up's Sound's Index.</summary>
	[SerializeField] private CollectionIndex _swingSoundIndex; 			/// <summary>Swing's Sound's Index.</summary>
	[Space(5f)]
	[Header("Voice Notes:")]
	[SerializeField] private CollectionIndex _doNoteIndex; 				/// <summary>Do's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _faNoteIndex; 				/// <summary>Fa's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _laNoteIndex; 				/// <summary>La's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _miNoteIndex; 				/// <summary>Mi's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _reNoteIndex; 				/// <summary>Re's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _siNoteIndex; 				/// <summary>Si's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _laReNoteIndex; 			/// <summary>La-Re's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _reFaNoteIndex; 			/// <summary>Re-Fa's Note Sound's Index.</summary>
	[SerializeField] private CollectionIndex _siMiNoteIndex; 			/// <summary>Si-Mi's Note Sound's Index.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Testing Settings:")]
	[SerializeField] private bool test; 								/// <summary>Test?.</summary>
	[SerializeField] private int testCardIndex; 						/// <summary>Test's Card Index.</summary>
	[Space(5f)]
	[SerializeField] private MeshFilter headMeshFilter; 				/// <summary>Removable Head's MeshFilter Component.</summary>
#endif
	private RigidbodyMovementAbility _movementAbility; 					/// <summary>RigidbodyMovementAbility's Component.</summary>
	private RotationAbility _rotationAbility; 							/// <summary>RotationAbility's Component.</summary>
	private JumpAbility _jumpAbility; 									/// <summary>JumpAbility's Component.</summary>
	private SteeringSnake _steeringSnake; 								/// <summary>SteeringSnake's Component.</summary>
	private IEnumerator iterator; 										/// <summary>[Test] Iterator.</summary>
	private Coroutine cardRoutine; 										/// <summary>Card's Coroutine reference.</summary>
	private Coroutine fallenTolerance; 									/// <summary>Removable Head's Fallen Tolerance Coroutine reference.</summary>
	private Vector2 axes;

#region Getters/Setters:
	/// <summary>Gets laughFrequency property.</summary>
	public FloatRange laughFrequency { get { return _laughFrequency; } }

	/// <summary>Gets stateIDCredential property.</summary>
	public AnimatorCredential stateIDCredential { get { return _stateIDCredential; } }

	/// <summary>Gets and Sets headPivot property.</summary>
	public Transform headPivot
	{
		get { return _headPivot; }
		set { _headPivot = value; }
	}

	/// <summary>Gets and Sets rigHead property.</summary>
	public Transform rigHead
	{
		get { return _rigHead; }
		set { _rigHead = value; }
	}

	/// <summary>Gets and Sets removableHead property.</summary>
	public Transform removableHead
	{
		get { return _removableHead; }
		set { _removableHead = value; }
	}

	/// <summary>Gets headHurtBox property.</summary>
	public HitCollider2D headHurtBox { get { return _headHurtBox; } }

	/// <summary>Gets headFallPointData property.</summary>
	public TransformData headFallPointData { get { return _headFallPointData; } }

	/// <summary>Gets fallDuration property.</summary>
	public float fallDuration { get { return _fallDuration; } }

	/// <summary>Gets fallenDuration property.</summary>
	public float fallenDuration { get { return _fallenDuration; } }

	/// <summary>Gets headReturnDuration property.</summary>
	public float headReturnDuration { get { return _headReturnDuration; } }

	/// <summary>Gets and Sets deckController property.</summary>
	public DestinoDeckController deckController
	{
		get { return _deckController; }
		set { _deckController = value; }
	}

	/// <summary>Gets and Sets cards property.</summary>
	public DestinoCard[] cards
	{
		get { return _cards; }
		set { _cards = value; }
	}

	/// <summary>Gets and Sets scythe property.</summary>
	public ContactWeapon scythe
	{
		get { return _scythe; }
		set { _scythe = value; }
	}

	/// <summary>Gets and Sets leftDrumstick property.</summary>
	public ContactWeapon leftDrumstick
	{
		get { return _leftDrumstick; }
		set { _leftDrumstick = value; }
	}

	/// <summary>Gets and Sets rightDrumstick property.</summary>
	public ContactWeapon rightDrumstick
	{
		get { return _rightDrumstick; }
		set { _rightDrumstick = value; }
	}

	/// <summary>Gets and Sets trumpet property.</summary>
	public ContactWeapon trumpet
	{
		get { return _trumpet; }
		set { _trumpet = value; }
	}

	/// <summary>Gets and Sets cymbals property.</summary>
	public ContactWeapon cymbals
	{
		get { return _cymbals; }
		set { _cymbals = value; }
	}

	/// <summary>Gets damageTakenSoundIndex property.</summary>
	public CollectionIndex damageTakenSoundIndex { get { return _damageTakenSoundIndex; } }

	/// <summary>Gets buildUpSoundIndex property.</summary>
	public CollectionIndex buildUpSoundIndex { get { return _buildUpSoundIndex; } }

	/// <summary>Gets swingSoundIndex property.</summary>
	public CollectionIndex swingSoundIndex { get { return _swingSoundIndex; } }

	/// <summary>Gets doNoteIndex property.</summary>
	public CollectionIndex doNoteIndex { get { return _doNoteIndex; } }

	/// <summary>Gets faNoteIndex property.</summary>
	public CollectionIndex faNoteIndex { get { return _faNoteIndex; } }

	/// <summary>Gets laNoteIndex property.</summary>
	public CollectionIndex laNoteIndex { get { return _laNoteIndex; } }

	/// <summary>Gets miNoteIndex property.</summary>
	public CollectionIndex miNoteIndex { get { return _miNoteIndex; } }

	/// <summary>Gets reNoteIndex property.</summary>
	public CollectionIndex reNoteIndex { get { return _reNoteIndex; } }

	/// <summary>Gets siNoteIndex property.</summary>
	public CollectionIndex siNoteIndex { get { return _siNoteIndex; } }

	/// <summary>Gets laReNoteIndex property.</summary>
	public CollectionIndex laReNoteIndex { get { return _laReNoteIndex; } }

	/// <summary>Gets reFaNoteIndex property.</summary>
	public CollectionIndex reFaNoteIndex { get { return _reFaNoteIndex; } }

	/// <summary>Gets siMiNoteIndex property.</summary>
	public CollectionIndex siMiNoteIndex { get { return _siMiNoteIndex; } }

	/// <summary>Gets steeringSnake Component.</summary>
	public SteeringSnake steeringSnake
	{ 
		get
		{
			if(_steeringSnake == null) _steeringSnake = GetComponent<SteeringSnake>();
			return _steeringSnake;
		}
	}

	/// <summary>Gets movementAbility Component.</summary>
	public RigidbodyMovementAbility movementAbility
	{ 
		get
		{
			if(_movementAbility == null) _movementAbility = GetComponent<RigidbodyMovementAbility>();
			return _movementAbility;
		}
	}

	/// <summary>Gets rotationAbility Component.</summary>
	public RotationAbility rotationAbility
	{ 
		get
		{
			if(_rotationAbility == null) _rotationAbility = GetComponent<RotationAbility>();
			return _rotationAbility;
		}
	}

	/// <summary>Gets jumpAbility Component.</summary>
	public JumpAbility jumpAbility
	{ 
		get
		{
			if(_jumpAbility == null) _jumpAbility = GetComponent<JumpAbility>();
			return _jumpAbility;
		}
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(cards == null) return;

		if(headMeshFilter != null) Gizmos.DrawMesh(headMeshFilter.sharedMesh, headFallPointData.position, headFallPointData.rotation);
		VGizmos.DrawTransformData(headFallPointData);
	}
#endif

	/// <summary>Callback internally called right after Awake.</summary>
	protected override void Awake()
	{
		base.Awake();
		
		EnablePhysics(false);

		animator.SetInteger(stateIDCredential, ID_STATE_IDLE_NORMAL);

		if(scythe != null) scythe.gameObject.SetActive(false);
		if(leftDrumstick != null) leftDrumstick.gameObject.SetActive(false);
		if(rightDrumstick != null) rightDrumstick.gameObject.SetActive(false);
		if(trumpet != null) trumpet.gameObject.SetActive(false);
		if(cymbals != null) cymbals.gameObject.SetActive(false);

		if(deckController != null)
		{
			deckController.Reset();
			deckController.onCardSelected += OnCardSelected;
		}

		if(cards != null) foreach(DestinoCard card in cards)
		{
			card.onCardEvent += OnCardEvent;
		}

		if(headHurtBox != null) headHurtBox.onTriggerEvent2D += OnHeadTriggerEvent2D;
	}

	/// <summary>DestinoBoss's starting actions before 1st Update frame.</summary>
	protected override void Start()
	{
		base.Start();

		int length = cards.Length;

		if(cards == null || length == 0) return;

/*#if UNITY_EDITOR
		int index = UnityEngine.Random.Range(0, cards.Length);
		if(test) index = testCardIndex;
		cards[index].behavior.BeginRoutine(this);
		iterator = cards[index].behavior.Routine(this);
#endif*/
	}

	/// <summary>Updates DestinoBoss's instance at each frame.</summary>
	private void Update()
	{
#if UNITY_EDITOR
		if(iterator != null && !iterator.MoveNext())
		{
			iterator = cards[testCardIndex].behavior.Routine(this);
		}
#endif
	}

	/// <summary>Enables Physics.</summary>
	/// <param name="_enable">Enable? true by default.</param>
	public override void EnablePhysics(bool _enable)
	{
		base.EnablePhysics(_enable);
		if(jumpAbility != null) jumpAbility.gravityApplier.useGravity = _enable;
	}

	public void OnLeftAxesChange(Vector2 _axes)
	{
		if(axes.x == 0.0f && axes == _axes) movementAbility.Stop();
		axes = _axes;

		if(axes.x != 0.0f)
		{
			Vector3 direction = new Vector3(
				axes.x,
				0.0f,
				axes.y
			);
			rotationAbility.RotateTowardsDirection(meshParent, direction);
		}
	}

	/// <summary>Moves Destino.</summary>
	/// <param name="_axes">Displacement axes.</param>
	/// <param name="_scale">Displacement's Scale [1.0f by default].</param>
	public void Move(Vector2 _axes, float _scale = 1.0f)
	{
		movementAbility.Move(_axes, _scale, Space.World);
	}

	/// <summary>Performs Jump.</summary>
	/// <param name="_axes">Additional Direction's Axes.</param>
	public void Jump(Vector2 _axes)
	{
		jumpAbility.Jump(_axes);
	}

	public void CancelJump()
	{
		jumpAbility.CancelJump();
	}

	/// <summary>Requests card to the DeckController.</summary>
	public void RequestCard()
	{
		if(deckController != null) StartCoroutine(deckController.Routine(this));
	}

	/// \TODO Deprecate?
	/// <summary>Callback invoked when a Scythe's Rotation Event happens.</summary>
	/// <param name="_event">Type of Rotation Event.</param>
	/// <param name="_ID">Event's ID.</param>
	public void OnScytheRotationEvent(RotationEvent _event, int _ID = 0)
	{
		switch(_event)
		{
			case RotationEvent.BuildUpBegins:
			scythe.ActivateHitBoxes(false);
			AudioController.PlayOneShot(SourceType.SFX, 0, buildUpSoundIndex);
			break;

			case RotationEvent.BuildUpEnds:

			break;

			case RotationEvent.SwingBegins:
			scythe.ActivateHitBoxes(true);
			AudioController.PlayOneShot(SourceType.SFX, 0, swingSoundIndex);
			break;

			case RotationEvent.SwingEnds:
			scythe.ActivateHitBoxes(false);
			break;
		}

		switch(_ID)
		{
			case ID_EVENT_NONE:
			break;
		}
	}

	/// <summary>Callback invoked whan an Animation Attack event occurs.</summary>
	/// <param name="_state">Animation Attack's Event/State.</param>
	public void OnScytheAnimationAttackEvent(AnimationCommandState _state)
	{
		switch(_state)
		{
			case AnimationCommandState.None:
			scythe.ActivateHitBoxes(false);
			break;

		    case AnimationCommandState.Startup:
		    scythe.ActivateHitBoxes(false);
		    AudioController.PlayOneShot(buildUpSoundIndex);
		    break;

		    case AnimationCommandState.Active:
		    scythe.ActivateHitBoxes(true);
		    AudioController.PlayOneShot(swingSoundIndex);
		    break;

		    case AnimationCommandState.Recovery:
		    break;

		    case AnimationCommandState.End:
		    break;
		}
	}

	/// <summary>Callback invoked when a Card is selected from the most recent shuffling.</summary>
	/// <param name="_card">Selected Card.</param>
	public void OnCardSelected(DestinoCard _card)
	{
		if(_card != null && _card.behavior != null)
		this.StartCoroutine(_card.behavior.Routine(this), ref cardRoutine);
	}

	/// <summary>Makes Destino Laugh.</summary>
	public void Laugh()
	{
		animator.SetInteger(stateIDCredential, ID_STATE_IDLE_LAUGH);
	}

	/// <summary>Makes Destino Sing.</summary>
	public void Sing()
	{
		FiniteStateAudioClip clip = Game.data.FSMLoops[DestinoSceneController.Instance.mainLoopVoiceIndex];
		animator.SetInteger(stateIDCredential, ID_STATE_CHANT);
		animator.Play("Song_Full", 0, clip.normalizedTime /*clip.GetCurrentStateTime()*/);
		clip.SetStateToCurrentTime();
		

	}

	/// <summary>Callback invoked when the fallen's tolerance duration of a card reached its end.</summary>
	/// <param name="_card">Card's reference.</param>
	/// <param name="_event">Event Type.</param>
	private void OnCardEvent(DestinoCard _card, DestinoCardEvent _event)
	{
		switch(_event)
		{
			case DestinoCardEvent.FallenToleranceFinished:
			deckController.ReturnCard(this, _card, OnCardStored);
			break;

			case DestinoCardEvent.Hit:
			this.StartCoroutine(SlashCardIntoHead(_card));
			break;
		}
	}

	/// <summary>Throws Removable Head into floor.</summary>
	private void ThrowHeadIntoFloor()
	{
		rigHead.gameObject.SetActive(false);
		removableHead.gameObject.SetActive(true);
		removableHead.SetParent(null);
		this.StartCoroutine(removableHead.LerpTowardsData(headFallPointData, fallDuration, TransformProperties.PositionAndRotation, Space.World, OnHeadFalled), ref fallenTolerance);
	}

	/// <summary>Callback invoked when a Health's event has occured.</summary>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		base.OnHealthEvent(_event, _amount);

		switch(_event)
		{
			case HealthEvent.Depleted:
			AudioController.PlayOneShot(SourceType.SFX, 0, damageTakenSoundIndex);
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
		if(headHurtBox == null) return;

		Vector3 hurtBoxPosition = removableHead.position;
		hurtBoxPosition.z = 0.0f;

		headHurtBox.transform.position = hurtBoxPosition;
		headHurtBox.Activate(true);

		this.StartCoroutine(this.WaitSeconds(fallenDuration, OnFallenToleranceEnds), ref fallenTolerance);
	}

	/// <summary>Callback invoked when the Head's HitCollider2D intersects with another GameObject.</summary>
	/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_hitColliderID">Optional ID of the HitCollider2D.</param>
	private void OnHeadTriggerEvent2D(Collider2D _collider, HitColliderEventTypes _eventType, int _hitColliderID = 0)
	{

	}

	/// <summary>Callback invoked when the Removable Head's Fallen Tolerance ends.</summary>
	private void OnFallenToleranceEnds()
	{
		headHurtBox.Activate(false);
		this.StartCoroutine(removableHead.LerpTowardsTransform(headPivot, headReturnDuration, TransformProperties.PositionAndRotation, Space.World, OnHeadReturned), ref fallenTolerance);
	}

	/// <summary>Callback internally called when the Head returns after its fall.</summary>
	private void OnHeadReturned()
	{
		removableHead.SetParent(headPivot);
		removableHead.gameObject.SetActive(false);
		rigHead.gameObject.SetActive(true);
		RequestCard();
	}

	/// <summary>Makes card slash into Destino's Head.</summary>
	/// <param name="_card">Card to slassh into destino' head.</param>
	private IEnumerator SlashCardIntoHead(DestinoCard _card)
	{
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

	/// <summary>Death's Routine.</summary>
	/// <param name="onDeathRoutineEnds">Callback invoked when the routine ends.</param>
	protected override IEnumerator DeathRoutine(Action onDeathRoutineEnds)
	{
		animator.SetInteger(stateIDCredential, ID_STATE_DEAD);

		yield return null;

		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		SecondsDelayWait wait = new SecondsDelayWait(info.length);

		while(wait.MoveNext()) yield return null;
		yield return base.DeathRoutine(onDeathRoutineEnds);
	}

	/// <summary>Idle's Routine [normal idle and random laughs].</summary>
	protected IEnumerator IdleRoutine()
	{
		animator.SetInteger(stateIDCredential, ID_STATE_IDLE_NORMAL);

		yield return null;

		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		AnimatorStateInfo info = default(AnimatorStateInfo);

		while(true)
		{
			wait.ChangeDurationAndReset(laughFrequency.Random());
			while(wait.MoveNext()) yield return null;

			animator.SetInteger(stateIDCredential, ID_STATE_IDLE_LAUGH);
			info = animator.GetCurrentAnimatorStateInfo(0);
			yield return null;

			wait.ChangeDurationAndReset(info.length);
			while(wait.MoveNext()) yield return null;

			animator.SetInteger(stateIDCredential, ID_STATE_IDLE_NORMAL);
			yield return null;
		}
	}
}
}