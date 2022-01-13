using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
[RequireComponent(typeof(SensorSystem2D))]
[RequireComponent(typeof(RigidbodyMovementAbility))]
[RequireComponent(typeof(RotationAbility))]
[RequireComponent(typeof(JumpAbility))]
public class DestinoBoss : Boss
{
	[Space(5f)]
	[Header("Animator's Credentials:")]
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _emptyCredential; 							/// <summary>Empty's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _idleCredential; 							/// <summary>Idle's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _singCredential; 							/// <summary>Sing's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _laughCredential; 							/// <summary>Laugh's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _lalaCredential; 							/// <summary>Lala's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _laaaCredential; 							/// <summary>Laaa's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _deadCredential; 							/// <summary>Dead's Animator Credential.</summary>
	[Space(5f)]
	[TabGroup("Animations")][SerializeField] private FloatRange _laughFrequency; 									/// <summary>Laughing's Frequency.</summary>
	[Header("Heads' Attributes:")]
	[TabGroup("Group A", "Head")][SerializeField] private Transform _headPivot; 									/// <summary>Head's Pivot [for both Heads].</summary>
	[TabGroup("Group A", "Head")][SerializeField] private Transform _rigHead; 										/// <summary>Destino's Rig Head.</summary>
	[TabGroup("Group A", "Head")][SerializeField] private Transform _removableHead; 								/// <summary>Destino's Removable Head.</summary>
	[TabGroup("Group A", "Head")][SerializeField] private HitCollider2D _headHurtBox; 								/// <summary>Removable Head's HutrBox.</summary>
	[Space(5f)]
	[Header("Sound FXs' References:")]
	[Header("Destino's Sounds:")]
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _damageTakenSoundIndex; 						/// <summary>Damage taken's Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _laughSoundIndex; 							/// <summary>Laugh's Sound's Index.</summary>
	[Space(5f)]
	[Header("Death's Sounds:")]
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _buildUpSoundIndex; 							/// <summary>Build-Up's Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _swingSoundIndex; 							/// <summary>Swing's Sound's Index.</summary>
	[Space(5f)]
	[Header("Voice Notes:")]
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _doNoteIndex; 								/// <summary>Do's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _faNoteIndex; 								/// <summary>Fa's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _laNoteIndex; 								/// <summary>La's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _miNoteIndex; 								/// <summary>Mi's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _reNoteIndex; 								/// <summary>Re's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _siNoteIndex; 								/// <summary>Si's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _laReNoteIndex; 								/// <summary>La-Re's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _reFaNoteIndex; 								/// <summary>Re-Fa's Note Sound's Index.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private int _siMiNoteIndex; 								/// <summary>Si-Mi's Note Sound's Index.</summary>
//#if UNITY_EDITOR
	[Space(5f)]
	[Header("Destino's Test:")]
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] private bool test; 					/// <summary>Test?.</summary>
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] private int testCardIndex; 			/// <summary>Test's Card Index.</summary>
	[Space(5f)]
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] private MeshFilter headMeshFilter; 	/// <summary>Removable Head's MeshFilter Component.</summary>
//#endif
	private RigidbodyMovementAbility _movementAbility; 																/// <summary>RigidbodyMovementAbility's Component.</summary>
	private RotationAbility _rotationAbility; 																		/// <summary>RotationAbility's Component.</summary>
	private JumpAbility _jumpAbility; 																				/// <summary>JumpAbility's Component.</summary>
	
	private Vector2 axes;

#region Getters/Setters:
	/// <summary>Gets emptyCredential property.</summary>
	public AnimatorCredential emptyCredential { get { return _emptyCredential; } }

	/// <summary>Gets idleCredential property.</summary>
	public AnimatorCredential idleCredential { get { return _idleCredential; } }

	/// <summary>Gets singCredential property.</summary>
	public AnimatorCredential singCredential { get { return _singCredential; } }

	/// <summary>Gets laughCredential property.</summary>
	public AnimatorCredential laughCredential { get { return _laughCredential; } }

	/// <summary>Gets lalaCredential property.</summary>
	public AnimatorCredential lalaCredential { get { return _lalaCredential; } }

	/// <summary>Gets laaaCredential property.</summary>
	public AnimatorCredential laaaCredential { get { return _laaaCredential; } }

	/// <summary>Gets deadCredential property.</summary>
	public AnimatorCredential deadCredential { get { return _deadCredential; } }

	/// <summary>Gets laughFrequency property.</summary>
	public FloatRange laughFrequency { get { return _laughFrequency; } }

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

	/// <summary>Gets damageTakenSoundIndex property.</summary>
	public int damageTakenSoundIndex { get { return _damageTakenSoundIndex; } }

	/// <summary>Gets laughSoundIndex property.</summary>
	public int laughSoundIndex { get { return _laughSoundIndex; } }

	/// <summary>Gets buildUpSoundIndex property.</summary>
	public int buildUpSoundIndex { get { return _buildUpSoundIndex; } }

	/// <summary>Gets swingSoundIndex property.</summary>
	public int swingSoundIndex { get { return _swingSoundIndex; } }

	/// <summary>Gets doNoteIndex property.</summary>
	public int doNoteIndex { get { return _doNoteIndex; } }

	/// <summary>Gets faNoteIndex property.</summary>
	public int faNoteIndex { get { return _faNoteIndex; } }

	/// <summary>Gets laNoteIndex property.</summary>
	public int laNoteIndex { get { return _laNoteIndex; } }

	/// <summary>Gets miNoteIndex property.</summary>
	public int miNoteIndex { get { return _miNoteIndex; } }

	/// <summary>Gets reNoteIndex property.</summary>
	public int reNoteIndex { get { return _reNoteIndex; } }

	/// <summary>Gets siNoteIndex property.</summary>
	public int siNoteIndex { get { return _siNoteIndex; } }

	/// <summary>Gets laReNoteIndex property.</summary>
	public int laReNoteIndex { get { return _laReNoteIndex; } }

	/// <summary>Gets reFaNoteIndex property.</summary>
	public int reFaNoteIndex { get { return _reFaNoteIndex; } }

	/// <summary>Gets siMiNoteIndex property.</summary>
	public int siMiNoteIndex { get { return _siMiNoteIndex; } }

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

	/// <summary>Callback internally called right after Awake.</summary>
	protected override void Awake()
	{
		base.Awake();
		
		EnablePhysics(false);

		animatorController.CrossFade(idleCredential, clipFadeDuration);
	}

	/// <summary>DestinoBoss's starting actions before 1st Update frame.</summary>
	protected override void Start()
	{
		base.Start();

		BeginDeathRoutine();
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

	/// <summary>Makes Destino Laugh.</summary>
	public void Laugh()
	{
		animatorController.CrossFade(_laughCredential, clipFadeDuration);
	}

	/// <summary>Makes Destino Sing.</summary>
	public void Sing()
	{
		FiniteStateAudioClip clip = Game.data.FSMLoops[DestinoSceneController.Instance.mainLoopVoiceIndex];
		animatorController.Play(_singCredential, 0, clip.normalizedTime);
		clip.SetStateToCurrentTime();
	}

	public void SingLalaSoundBit(Action onSoundBitEnds = null)
	{

	}

	public void SingLaaaSoundBit(Action onSoundBitEnds = null)
	{
		
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

	/// <summary>Callback invoked when the DeadFX's routine ends.</summary>
	protected override void OnDeadFXsFinished()
	{
		base.OnDeadFXsFinished();
		OnObjectDeactivation();
	}

	/// <summary>Death's Routine.</summary>
	/// <param name="onDeathRoutineEnds">Callback invoked when the routine ends.</param>
	protected override IEnumerator DeathRoutine(Action onDeathRoutineEnds)
	{
		EnablePhysicalColliders(false);
		EnableTriggerColliders(false);
		animatorController.CrossFade(_deadCredential, clipFadeDuration);

		yield return null;

		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		SecondsDelayWait wait = new SecondsDelayWait(info.length);

		while(wait.MoveNext()) yield return null;
		yield return base.DeathRoutine(onDeathRoutineEnds);
	}

	/// <summary>Idle's Routine [normal idle and random laughs].</summary>
	protected IEnumerator IdleRoutine()
	{
		animatorController.CrossFade(_idleCredential, clipFadeDuration);

		yield return null;

		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		AnimatorStateInfo info = default(AnimatorStateInfo);

		while(true)
		{
			wait.ChangeDurationAndReset(laughFrequency.Random());
			while(wait.MoveNext()) yield return null;

			animatorController.CrossFade(_laughCredential, clipFadeDuration);
			info = animator.GetCurrentAnimatorStateInfo(0);
			yield return null;

			wait.ChangeDurationAndReset(info.length);
			while(wait.MoveNext()) yield return null;

			animatorController.CrossFade(_idleCredential, clipFadeDuration);
			yield return null;
		}
	}
}
}