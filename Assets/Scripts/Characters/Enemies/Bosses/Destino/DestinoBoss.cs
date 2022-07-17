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
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _emptyCredential; 								/// <summary>Empty's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _idleCredential; 								/// <summary>Idle's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _singCredential; 								/// <summary>Sing's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _laughCredential; 								/// <summary>Laugh's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _lalaCredential; 								/// <summary>Lala's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _laaaCredential; 								/// <summary>Laaa's Animator Credential.</summary>
	[TabGroup("Animations")][SerializeField] private AnimatorCredential _deadCredential; 								/// <summary>Dead's Animator Credential.</summary>
	[Space(5f)]
	[TabGroup("Animations")][SerializeField] private FloatRange _laughFrequency; 										/// <summary>Laughing's Frequency.</summary>
	[Header("Heads' Attributes:")]
	[TabGroup("Group A", "Head")][SerializeField] private Transform _headPivot; 										/// <summary>Head's Pivot [for both Heads].</summary>
	[TabGroup("Group A", "Head")][SerializeField] private Transform _rigHead; 											/// <summary>Destino's Rig Head.</summary>
	[TabGroup("Group A", "Head")][SerializeField] private Transform _removableHead; 									/// <summary>Destino's Removable Head.</summary>
	[TabGroup("Group A", "Head")][SerializeField] private HitCollider2D _headHurtBox; 									/// <summary>Removable Head's HutrBox.</summary>
	[Space(5f)]
	[Header("Sound FXs' References:")]
	[Header("Destino's Sounds:")]
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _damageTakenSoundEffect; 	/// <summary>Damage taken's Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _laughSoundEffect; 			/// <summary>Laugh's Sound-Effect's Data.</summary>
	[Space(5f)]
	[Header("Death's Sounds:")]
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _buildUpSoundEffect; 		/// <summary>Build-Up's Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _swingSoundEffect; 			/// <summary>Swing's Sound-Effect's Data.</summary>
	[Space(5f)]
	[Header("Voice Notes:")]
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _doNote; 					/// <summary>Do's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _faNote; 					/// <summary>Fa's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _laNote; 					/// <summary>La's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _miNote; 					/// <summary>Mi's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _reNote; 					/// <summary>Re's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _siNote; 					/// <summary>Si's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _laReNote; 					/// <summary>La-Re's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _reFaNote; 					/// <summary>Re-Fa's Note Sound-Effect's Data.</summary>
	[TabGroup("Group B", "Sound Effects")][SerializeField] private SoundEffectEmissionData _siMiNote; 					/// <summary>Si-Mi's Note Sound-Effect's Data.</summary>
//#if UNITY_EDITOR
	[Space(5f)]
	[Header("Destino's Test:")]
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] private bool test; 						/// <summary>Test?.</summary>
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] private int testCardIndex; 				/// <summary>Test's Card Index.</summary>
	[Space(5f)]
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] private MeshFilter headMeshFilter; 		/// <summary>Removable Head's MeshFilter Component.</summary>
//#endif
	private RigidbodyMovementAbility _movementAbility; 																	/// <summary>RigidbodyMovementAbility's Component.</summary>
	private RotationAbility _rotationAbility; 																			/// <summary>RotationAbility's Component.</summary>
	private JumpAbility _jumpAbility; 																					/// <summary>JumpAbility's Component.</summary>
	
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

	/// <summary>Gets damageTakenSoundEffect property.</summary>
	public SoundEffectEmissionData damageTakenSoundEffect { get { return _damageTakenSoundEffect; } }

	/// <summary>Gets laughSoundEffect property.</summary>
	public SoundEffectEmissionData laughSoundEffect { get { return _laughSoundEffect; } }

	/// <summary>Gets buildUpSoundEffect property.</summary>
	public SoundEffectEmissionData buildUpSoundEffect { get { return _buildUpSoundEffect; } }

	/// <summary>Gets swingSoundEffect property.</summary>
	public SoundEffectEmissionData swingSoundEffect { get { return _swingSoundEffect; } }

	/// <summary>Gets doNote property.</summary>
	public SoundEffectEmissionData doNote { get { return _doNote; } }

	/// <summary>Gets faNote property.</summary>
	public SoundEffectEmissionData faNote { get { return _faNote; } }

	/// <summary>Gets laNote property.</summary>
	public SoundEffectEmissionData laNote { get { return _laNote; } }

	/// <summary>Gets miNote property.</summary>
	public SoundEffectEmissionData miNote { get { return _miNote; } }

	/// <summary>Gets reNote property.</summary>
	public SoundEffectEmissionData reNote { get { return _reNote; } }

	/// <summary>Gets siNote property.</summary>
	public SoundEffectEmissionData siNote { get { return _siNote; } }

	/// <summary>Gets laReNote property.</summary>
	public SoundEffectEmissionData laReNote { get { return _laReNote; } }

	/// <summary>Gets reFaNote property.</summary>
	public SoundEffectEmissionData reFaNote { get { return _reFaNote; } }

	/// <summary>Gets siMiNote property.</summary>
	public SoundEffectEmissionData siMiNote { get { return _siMiNote; } }

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

	/// <summary>Callback invoked when DestinoBoss's instance is disabled.</summary>
	private void OnDisable()
	{
		//Debug.Log("[DestinoBoss] DEACTIVATED");
	}

	/// <summary>Callback internally called right after Awake.</summary>
	protected override void Awake()
	{
		base.Awake();
		
		EnablePhysics(false);
		removableHead.gameObject.SetActive(false);
		animatorController.CrossFade(idleCredential, clipFadeDuration);
	}

	/// <summary>DestinoBoss's starting actions before 1st Update frame.</summary>
	protected override void Start()
	{
		base.Start();

		//BeginDeathRoutine();
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
		FiniteStateAudioClip clip = ResourcesManager.GetFSMClip(DestinoSceneController.Instance.mainLoopLoopData.soundReference);
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
			//AudioController.PlayOneShot(SourceType.SFX, 0, damageTakenSoundIndex);
			damageTakenSoundEffect.Play();
			break;
		}
	}

	/// <summary>Callback invoked when the DeadFX's routine ends.</summary>
	protected override void OnDeadFXsFinished()
	{
		base.OnDeadFXsFinished();
		OnObjectDeactivation();
		InvokeIDEvent(IDs.EVENT_DEATHROUTINE_ENDS);
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

		IEnumerator routine = base.DeathRoutine(onDeathRoutineEnds);
		
		while(routine.MoveNext()) yield return null;
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