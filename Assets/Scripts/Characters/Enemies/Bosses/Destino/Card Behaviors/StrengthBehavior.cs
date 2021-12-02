using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;


using Random = UnityEngine.Random;

namespace Flamingo
{
public class StrengthBehavior : DestinoScriptableCoroutine
{
	[SerializeField] private AIContactWeapon _leftDrumstick; 					/// <summary>Left Drumstick's renderer.</summary>
	[SerializeField] private AIContactWeapon _rightDrumstick; 					/// <summary>Right Drumstick's renderer.</summary>
	[SerializeField] private AIContactWeapon _trumpet; 							/// <summary>Trumpet's reference.</summary>
	[SerializeField] private AIContactWeapon _cymbals; 							/// <summary>Cymbals' Reference.</summary>
	[Space(5f)]
	[SerializeField] private IntRange _setSizeRange; 							/// <summary>Set Size's Range.</summary>
	[Space(5f)]
	[Header("Drumsticks' Attributes:")]
	[SerializeField] private CollectionIndex _drumstickSoundIndex; 				/// <summary>Drumsticks Sound's Index.</summary>
	[SerializeField] private AnimatorCredential _drumstickPhaseCredential; 		/// <summary>Activate Drumstick's Animator Credential.</summary>
	[SerializeField] private IntRange _drumBeatsSequence; 						/// <summary>Drumstickes' beats sequence's range.</summary>
	[SerializeField] private Vector3 _leftDrumstickSpawnPoint; 					/// <summary>Left Drumstick's Spawn Position.</summary>
	[SerializeField] private Vector3 _rightDrumstickSpawnPoint; 				/// <summary>Right Drumstick's Spawn Position.</summary>
	[SerializeField] private NormalizedVector3 _orientationVector; 				/// <summary>Drumstick's Orientation Vector.</summary>
	[SerializeField] private NormalizedVector3 _rotationAxis; 					/// <summary>Drumstick's Rotation Axis.</summary>
	[SerializeField] private float _drumstickOffsetX;							/// <summaty>Drumstrick spawn point offset  </summary>
	[SerializeField] [Range(0.0f, 1.0f)] private float _dotTolerance; 			/// <summary>Dot Product's Tolerance.</summary>
	[SerializeField] private float _drumstickShakeDuration; 					/// <summary>Drumstick's ShakeDuration.</summary>
	[SerializeField] private float _drumstickShakeSpeed; 						/// <summary>Drumstick's Shake Speed.</summary>
	[SerializeField] private float _drumstickShakeMagnitude; 					/// <summary>Drumstick's Shake Magnitude.</summary>
	[SerializeField] private RotationDataSet[] _leftDrumstickRotationsSet; 		/// <summary>Left Drumstick's Rotations' Data Set.</summary>
	[SerializeField] private RotationDataSet[] _rightDrumstickRotationsSet; 	/// <summary>Right Drumstick's Rotations' Data Set.</summary>
	[Space(5f)]
	[Header("Trumpet's Attributes:")]
	[SerializeField] private CollectionIndex _trumpetSoundIndex; 				/// <summary>Trumpet Sound's Index.</summary>
	[SerializeField] private AnimatorCredential _activateTrumpetCredential; 	/// <summary>Activate Trumpet's Credential.</summary>
	[SerializeField] private Vector3 _trumpetSpawnPoint; 						/// <summary>Trumpet's Spawn Position.</summary>
	[SerializeField] private Vector3 _trumpetDestinyPoint; 						/// <summary>Trumpet's Destiny Position [considering Mateo's X position].</summary>
	[SerializeField] private float _entranceDuration; 							/// <summary>Trumpet's Entrance Duration.</summary>
	[SerializeField] private float _exitDuration; 								/// <summary>Trumpet's Exit Duration.</summary>
	[SerializeField] private float _soundEmissionDuration; 						/// <summary>Trumpet's Sound Emission Duration.</summary>
	[SerializeField] private float _trumpetShakeDuration; 						/// <summary>Trumpet's ShakeDuration.</summary>
	[SerializeField] private float _trumpetShakeSpeed; 							/// <summary>Trumpet's Shake Speed.</summary>
	[SerializeField] private float _trumpetShakeMagnitude; 						/// <summary>Trumpet's Shake Magnitude.</summary>
	[Space(5f)]
	[Header("Cymbals' Attributes:")]
	[SerializeField] private Vector3 _cymbalsSpawnPosition; 					/// <summary>Cymbals' Spawn's Position.</summary>
	[SerializeField] private CollectionIndex _cymbalSoundIndex; 				/// <summary>Cymbal Sound's Index.</summary>
	[SerializeField] private CollectionIndex _cymbalIndex; 						/// <summary>Cymbal Projectile's Index.</summary>
	[SerializeField] private AnimatorCredential _activateCymbalsCredential; 	/// <summary>Activate Cymbals' Animator Credential.</summary>
	[SerializeField] private TransformData _leftCymbalSpawnPoint; 				/// <summary>Left Cymbal's Spawn Point [as TransformData].</summary>
	[SerializeField] private TransformData _rightCymbalSpawnPoint; 				/// <summary>Right Cymbal's Spawn Point [as TransformData].</summary>
	[Space(5f)]
	[Header("Sounds' Attributes:")]
	[SerializeField] private float _cooldownAfterSoundNote; 					/// <summary>Cooldown's Duration after a sing note is played.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Test's Settings:")]
	[SerializeField] private bool testDebugIndex; 								/// <summary>Test Debug's Index.</summary>
	[HideInInspector] public int debugIndex; 									/// <summary>RotationsDataSet's on given index to debug.</summary>
	[HideInInspector] public int debugSubIndex; 								/// <summary>RotationData on given sub-index to debug.</summary>
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Vector2 gizmosTextOffset; 							/// <summary>Gizmos' Text Offset.</summary>
	[SerializeField] private Color buildUpColor; 								/// <summary>Build-Up's Color.</summary>
	[SerializeField] private Color swingColor; 									/// <summary>Swing's Color.</summary>
	[SerializeField] private float gizmosRayLength; 							/// <summary>Gizmos' Ray Length.</summary>
#endif

#region Getters:
	/// <summary>Gets and Sets leftDrumstick property.</summary>
	public AIContactWeapon leftDrumstick
	{
		get { return _leftDrumstick; }
		set { _leftDrumstick = value; }
	}

	/// <summary>Gets and Sets rightDrumstick property.</summary>
	public AIContactWeapon rightDrumstick
	{
		get { return _rightDrumstick; }
		set { _rightDrumstick = value; }
	}

	/// <summary>Gets and Sets trumpet property.</summary>
	public AIContactWeapon trumpet
	{
		get { return _trumpet; }
		set { _trumpet = value; }
	}

	/// <summary>Gets and Sets cymbals property.</summary>
	public AIContactWeapon cymbals
	{
		get { return _cymbals; }
		set { _cymbals = value; }
	}

	/// <summary>Gets setSizeRange property.</summary>
	public IntRange setSizeRange { get { return _setSizeRange; } }

	/// <summary>Gets drumBeatsSequence property.</summary>
	public IntRange drumBeatsSequence { get { return _drumBeatsSequence; } }

	/// <summary>Gets leftDrumstickSpawnPoint property.</summary>
	public Vector3 leftDrumstickSpawnPoint { get { return _leftDrumstickSpawnPoint; } }

	/// <summary>Gets rightDrumstickSpawnPoint property.</summary>
	public Vector3 rightDrumstickSpawnPoint { get { return _rightDrumstickSpawnPoint; } }

	/// <summary>Gets trumpetSpawnPoint property.</summary>
	public Vector3 trumpetSpawnPoint { get { return _trumpetSpawnPoint; } }

	/// <summary>Gets trumpetDestinyPoint property.</summary>
	public Vector3 trumpetDestinyPoint { get { return _trumpetDestinyPoint; } }

	/// <summary>Gets cymbalsSpawnPosition property.</summary>
	public Vector3 cymbalsSpawnPosition { get { return _cymbalsSpawnPosition; } }

	/// <summary>Gets orientationVector property.</summary>
	public NormalizedVector3 orientationVector { get { return _orientationVector; } }

	/// <summary>Gets rotationAxis property.</summary>
	public NormalizedVector3 rotationAxis { get { return _rotationAxis; } }

	/// <summary>Gets drumstickOffsetX property.</summary>
	public float drumstickOffsetX { get { return _drumstickOffsetX;}}

	/// <summary>Gets dotTolerance property.</summary>
	public float dotTolerance { get { return _dotTolerance; } }

	/// <summary>Gets drumstickShakeDuration property.</summary>
	public float drumstickShakeDuration { get { return _drumstickShakeDuration; } }

	/// <summary>Gets drumstickShakeSpeed property.</summary>
	public float drumstickShakeSpeed { get { return _drumstickShakeSpeed; } }

	/// <summary>Gets drumstickShakeMagnitude property.</summary>
	public float drumstickShakeMagnitude { get { return _drumstickShakeMagnitude; } }

	/// <summary>Gets entranceDuration property.</summary>
	public float entranceDuration { get { return _entranceDuration; } }

	/// <summary>Gets exitDuration property.</summary>
	public float exitDuration { get { return _exitDuration; } }

	/// <summary>Gets soundEmissionDuration property.</summary>
	public float soundEmissionDuration { get { return _soundEmissionDuration; } }

	/// <summary>Gets trumpetShakeDuration property.</summary>
	public float trumpetShakeDuration { get { return _trumpetShakeDuration; } }

	/// <summary>Gets trumpetShakeSpeed property.</summary>
	public float trumpetShakeSpeed { get { return _trumpetShakeSpeed; } }

	/// <summary>Gets trumpetShakeMagnitude property.</summary>
	public float trumpetShakeMagnitude { get { return _trumpetShakeMagnitude; } }

	/// <summary>Gets cooldownAfterSoundNote property.</summary>
	public float cooldownAfterSoundNote { get { return _cooldownAfterSoundNote; } }

	/// <summary>Gets leftDrumstickRotationsSet property.</summary>
	public RotationDataSet[] leftDrumstickRotationsSet { get { return _leftDrumstickRotationsSet; } }

	/// <summary>Gets rightDrumstickRotationsSet property.</summary>
	public RotationDataSet[] rightDrumstickRotationsSet { get { return _rightDrumstickRotationsSet; } }

	/// <summary>Gets drumstickPhaseCredential property.</summary>
	public AnimatorCredential drumstickPhaseCredential { get { return _drumstickPhaseCredential; } }

	/// <summary>Gets activateTrumpetCredential property.</summary>
	public AnimatorCredential activateTrumpetCredential { get { return _activateTrumpetCredential; } }

	/// <summary>Gets activateCymbalsCredential property.</summary>
	public AnimatorCredential activateCymbalsCredential { get { return _activateCymbalsCredential; } }

	/// <summary>Gets drumstickSoundIndex property.</summary>
	public CollectionIndex drumstickSoundIndex { get { return _drumstickSoundIndex; } }

	/// <summary>Gets trumpetSoundIndex property.</summary>
	public CollectionIndex trumpetSoundIndex { get { return _trumpetSoundIndex; } }

	/// <summary>Gets cymbalSoundIndex property.</summary>
	public CollectionIndex cymbalSoundIndex { get { return _cymbalSoundIndex; } }

	/// <summary>Gets cymbalIndex property.</summary>
	public CollectionIndex cymbalIndex { get { return _cymbalIndex; } }

	/// <summary>Gets leftCymbalSpawnPoint property.</summary>
	public TransformData leftCymbalSpawnPoint { get { return _leftCymbalSpawnPoint; } }

	/// <summary>Gets rightCymbalSpawnPoint property.</summary>
	public TransformData rightCymbalSpawnPoint { get { return _rightCymbalSpawnPoint; } }
#endregion

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
#if UNITY_EDITOR
		base.DrawGizmos();

		Gizmos.color = gizmosColor;

		Gizmos.DrawWireSphere(leftDrumstickSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(rightDrumstickSpawnPoint, gizmosRadius);
		/*VGizmos.DrawText("Left Drumstick's Spawn Position", leftDrumstickSpawnPoint, gizmosTextOffset, Color.white);
		VGizmos.DrawText("Right Drumstick's Spawn Position", rightDrumstickSpawnPoint, gizmosTextOffset, Color.white);*/

		if(leftDrumstickRotationsSet != null && leftDrumstickRotationsSet.Length > 0)
		{
			RotationData data = leftDrumstickRotationsSet[debugIndex].rotationDataSet[debugSubIndex];

			Gizmos.color = buildUpColor;
			Gizmos.DrawRay(leftDrumstickSpawnPoint, data.buildUpDirection.normalized * gizmosRayLength);
			Gizmos.color = swingColor;
			Gizmos.DrawRay(leftDrumstickSpawnPoint, data.swingDirection.normalized * gizmosRayLength);
		}
		if(rightDrumstickRotationsSet != null && rightDrumstickRotationsSet.Length > 0)
		{
			RotationData data = rightDrumstickRotationsSet[debugIndex].rotationDataSet[debugSubIndex];

			Gizmos.color = buildUpColor;
			Gizmos.DrawRay(rightDrumstickSpawnPoint, data.buildUpDirection.normalized * gizmosRayLength);
			Gizmos.color = swingColor;
			Gizmos.DrawRay(rightDrumstickSpawnPoint, data.swingDirection.normalized * gizmosRayLength);
		}

		Gizmos.DrawWireSphere(trumpetSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(trumpetDestinyPoint, gizmosRadius);
		Gizmos.DrawWireSphere(cymbalsSpawnPosition, gizmosRadius);

		/// \TODO Correct the positioning on the Text...
		/*VGizmos.DrawText("Trumpet's Spawn Position", trumpetSpawnPoint, gizmosTextOffset, Color.white);
		VGizmos.DrawText("Trumpet's Destiny Position", trumpetDestinyPoint, gizmosTextOffset, Color.white);*/

		VGizmos.DrawTransformData(leftCymbalSpawnPoint);
		VGizmos.DrawTransformData(rightCymbalSpawnPoint);
#endif
	}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{		
		int setSize = setSizeRange.Random();
		IEnumerator[] routines = new IEnumerator[setSize];

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);
		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		for(int i = 0; i < setSize; i++)
		{
			IEnumerator routine = null;
			int index = Random.Range(0, 3);

			switch(index)
			{
				case 0: routine = DrumsticksRoutine(boss); break;
				case 1: routine = TrumpetRoutine(boss); break;
				case 2: routine = CymbalsRoutine(boss); break;
			}

			routines[i] = routine;
		}

		/// Temp for mere Testing:
		/*routines = new IEnumerator[3] { DrumsticksRoutine(boss), DrumsticksRoutine(boss), DrumsticksRoutine(boss) };
		yield return null;*/

		foreach(IEnumerator routine in routines)
		{
			while(routine.MoveNext()) yield return null;
		}

		AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
		AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex);
		boss.Sing();

		InvokeCoroutineEnd();
	}

	/// <summary>Drumsticks' Routine.</summary>
	/// <param name="boss">Destino's reference.</param>
	private IEnumerator DrumsticksRoutine(DestinoBoss boss)
	{
		AudioClip clip = AudioController.PlayOneShot(SourceType.SFX, 1, boss.reFaNoteIndex);
		Animator drumstickAnimator = rightDrumstick.GetComponent<Animator>();
		AnimationAttacksHandler drumstickAttacksHandler = rightDrumstick.GetComponent<AnimationAttacksHandler>();
		Renderer leftDrumstickRenderer = leftDrumstick.transform.GetChild(0).GetComponent<Renderer>();
		float drumstickLength = leftDrumstickRenderer.bounds.size.y;
		FloatRange leftDrumstickLimits = new FloatRange(-drumstickLength, -drumstickLength * 2.0f);
		FloatRange rightDrumstickLimits = new FloatRange(drumstickLength, drumstickLength * 2.0f);
		IEnumerator[] sequence = new IEnumerator[drumBeatsSequence.Random()];
		
		//Left Drumstick Attack Action
		Action<RotationEvent, int> onLeftDrumstickRotationEvent = (rotationEvent, ID)=>
		{
			switch(rotationEvent)
			{
				case RotationEvent.BuildUpEnds:
				Vector3 projectedMateoPosition = Game.ProjectMateoPosition(1.0f);
		
				//leftDrumstick.transform.position = leftDrumstickSpawnPoint.WithX(leftDrumstickLimits.Clamp(projectedMateoPosition.x - drumstickLength));
				break;

				case RotationEvent.BuildingUp:
				leftDrumstick.weapon.ActivateHitBoxes(false);
				break;

				case RotationEvent.Swinging:
				leftDrumstick.weapon.ActivateHitBoxes(true);
				break;

				case RotationEvent.SwingEnds:
				leftDrumstick.StartCoroutine(leftDrumstick.transform.GetChild(0).ShakePosition(drumstickShakeDuration, drumstickShakeSpeed, drumstickShakeMagnitude));
				break;
			}
		};
		Action<RotationEvent, int> onRightDrumstickRotationEvent = (rotationEvent, ID)=>
		{
			switch(rotationEvent)
			{
				case RotationEvent.BuildUpEnds:
				Vector3 projectedMateoPosition = Game.ProjectMateoPosition(1.0f);
				
				//rightDrumstick.transform.position = rightDrumstickSpawnPoint.WithX(rightDrumstickLimits.Clamp(projectedMateoPosition.x + drumstickLength));
				break;

				case RotationEvent.BuildingUp:
				rightDrumstick.weapon.ActivateHitBoxes(false);
				break;

				case RotationEvent.Swinging:
				rightDrumstick.weapon.ActivateHitBoxes(true);
				break;

				case RotationEvent.SwingEnds:
				rightDrumstick.StartCoroutine(rightDrumstick.transform.GetChild(0).ShakePosition(drumstickShakeDuration, drumstickShakeSpeed, drumstickShakeMagnitude));
				break;
			}
		};
		int[] drumstickCombo = VArray.RandomSet(1, 2);
		int index = 0;
		bool drumstickAttackFinished = false;

		OnAnimationAttackEvent onAnimationAttackEvent = (_state)=>
		{
			switch(_state)
			{
				case AnimationCommandState.None:
				break;

			    case AnimationCommandState.Startup:
			    AudioController.PlayOneShot(SourceType.SFX, 1, drumstickSoundIndex);
			    break;

			    case AnimationCommandState.Active:
			    break;

			    case AnimationCommandState.Recovery:
			    break;

			    case AnimationCommandState.End:
			    if(index < drumstickCombo.Length)
			    {
			 
			    	drumstickAnimator.SetInteger(drumstickPhaseCredential, drumstickCombo[index]);
			    	index++;
			    }
			    else
			    {

			    	drumstickAnimator.SetInteger(drumstickPhaseCredential, 0);
			    	drumstickAttackFinished = true;
			    }
			    break;
			}
		};

		boss.animatorController.CrossFade(boss.lalaCredential, boss.clipFadeDuration);

		drumstickAttacksHandler.onAnimationAttackEvent -= onAnimationAttackEvent;
		drumstickAttacksHandler.onAnimationAttackEvent += onAnimationAttackEvent;
		rightDrumstick.weapon.ActivateHitBoxes(true);
		leftDrumstick.weapon.ActivateHitBoxes(true);

		SecondsDelayWait wait = new SecondsDelayWait(clip.length);
		while(wait.MoveNext()) yield return null;

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);

		wait.ChangeDurationAndReset(cooldownAfterSoundNote);
		while(wait.MoveNext()) yield return null;

#region SequenceCombo:
		for(int i = 0; i < drumstickCombo.Length; i++)
		{
			//Set Position of drumstrick acording to mateo position
			Vector3 _drumstickSpawnPoint = new Vector3();
			_drumstickSpawnPoint.y = Game.ProjectMateoPosition(1.0f).y + drumstickLength / 2.5f;

			switch(drumstickCombo[i])
			{
				case 1: 
				_drumstickSpawnPoint.x = leftDrumstickLimits.Clamp( Game.ProjectMateoPosition(1.0f).x - (drumstickLength - drumstickOffsetX));
				//_drumstickSpawnPoint.x = Game.ProjectMateoPosition(1.0f).x - drumstickLength;
					break;

				case 2:
				_drumstickSpawnPoint.x = rightDrumstickLimits.Clamp( Game.ProjectMateoPosition(1.0f).x + (drumstickLength - drumstickOffsetX));
					break;
			}
				
		
				
				drumstickAnimator.transform.position = _drumstickSpawnPoint;

				//
			rightDrumstick.SetActive(true);
			drumstickAnimator.SetInteger(drumstickPhaseCredential, drumstickCombo[i]);

			yield return null;

			AnimatorStateInfo info = drumstickAnimator.GetCurrentAnimatorStateInfo(0);
			wait.ChangeDurationAndReset(info.length);
			while(wait.MoveNext()) yield return null;

			drumstickAnimator.SetInteger(drumstickPhaseCredential, 0);
			yield return null;
		}
#endregion

#region WithoutSequence:
		//leftDrumstick.SetActive(true);
		/*rightDrumstick.SetActive(true);
		drumstickAnimator.SetInteger(drumstickPhaseCredential, drumstickCombo[index]);
		index++;

		yield return null;

		AnimatorStateInfo info = drumstickAnimator.GetCurrentAnimatorStateInfo(0);
		//clip = AudioController.PlayOneShot(SourceType.SFX, 1, drumstickSoundIndex);
		
		/// Decide the duration of the next wait based on the Max between the note and the animation, so there is assurance that both will end before this routine does.
		//wait.ChangeDurationAndReset(Mathf.Max(info.length, clip.length));
		wait.ChangeDurationAndReset(info.length);

		while(wait.MoveNext()) yield return null;*/
#endregion

		/*for(int i = 0; i < sequence.Length; i++)
		{
			sequence[i] = UnityEngine.Random.Range(0, 2) == 0?
				RotationDataSet.BuildUpAndSwing(leftDrumstickRotationsSet, leftDrumstick.transform, rotationAxis, orientationVector, dotTolerance, onLeftDrumstickRotationEvent)
				: RotationDataSet.BuildUpAndSwing(rightDrumstickRotationsSet, rightDrumstick.transform, rotationAxis, orientationVector, dotTolerance, onRightDrumstickRotationEvent);
		}

		foreach(IEnumerator rotateRoutine in sequence)
		{
			while(rotateRoutine.MoveNext()) yield return null;
		}*/

		//while(!drumstickAttackFinished) yield return null;
	
		//leftDrumstick.SetActive(false);
		rightDrumstick.SetActive(false);
	}

	/// <summary>Trumpet's Routine.</summary>
	/// <param name="boss">Destino's reference.</param>
	private IEnumerator TrumpetRoutine(DestinoBoss boss)
	{
		AudioClip clip = AudioController.PlayOneShot(SourceType.SFX, 1, boss.laReNoteIndex);
		Animator trumpetAnimator = trumpet.GetComponent<Animator>();
		AnimationAttacksHandler trumpetAttacksHandler = trumpet.GetComponent<AnimationAttacksHandler>();
		SecondsDelayWait wait = new SecondsDelayWait(soundEmissionDuration);
		Vector3 projectedMateoPosition = Game.ProjectMateoPosition(1.0f);
		Vector3 entryPosition = new Vector3();
		float t = 0.0f;
		float inverseDuration = 1.0f / entranceDuration;
		bool trumpetAttackFinished = false;
		trumpet.SetActive(true);
		trumpet.weapon.ActivateHitBoxes(false);
		OnAnimationAttackEvent onAnimationAttackEvent = (_state)=>
		{
			switch(_state)
			{
				case AnimationCommandState.Startup:
				AudioController.PlayOneShot(SourceType.SFX, 1, trumpetSoundIndex);
				break;

				case AnimationCommandState.Active:
				trumpet.weapon.ActivateHitBoxes(true);
				break;

				case AnimationCommandState.Recovery:
				break;

				case AnimationCommandState.End:
				trumpet.weapon.ActivateHitBoxes(false);
				trumpetAttackFinished = true;
				break;
			}
		};

		boss.animatorController.CrossFade(boss.lalaCredential, boss.clipFadeDuration);
		/*trumpetAttacksHandler.onAnimationAttackEvent -= onAnimationAttackEvent;
		trumpetAttacksHandler.onAnimationAttackEvent += onAnimationAttackEvent;*/
		trumpet.weapon.ActivateHitBoxes(true);

		wait.ChangeDurationAndReset(clip.length);
		while(wait.MoveNext()) yield return null;

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);

		wait.ChangeDurationAndReset(cooldownAfterSoundNote);
		while(wait.MoveNext()) yield return null;

		//<summary> Initialize values for spawn point </summary>
		entryPosition.y = trumpetSpawnPoint.y;
		entryPosition.x = projectedMateoPosition.x;


		trumpetAnimator.SetTrigger(activateTrumpetCredential);
		trumpetAnimator.transform.position = entryPosition;
		

	
		//AudioController.PlayOneShot(SourceType.SFX, 1, trumpetSoundIndex);

		yield return null;

		AnimatorStateInfo info = trumpetAnimator.GetCurrentAnimatorStateInfo(0);
		//clip = AudioController.PlayOneShot(SourceType.SFX, 1, trumpetSoundIndex);
		
		/// Decide the duration of the next wait based on the Max between the note and the animation, so there is assurance that both will end before this routine does.
		//wait.ChangeDurationAndReset(Mathf.Max(info.length, clip.length));
		wait.ChangeDurationAndReset(info.length);

		while(wait.MoveNext()) yield return null;

		//while(!trumpetAttackFinished) yield return null;

#region Before:
		/*while(t < 1.0f)
		{
			trumpet.transform.position = Vector3.Lerp(spawnPosition, destinyPoint, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		trumpet.transform.position = destinyPoint;
		t = 1.0f;
		inverseDuration = 1.0f / exitDuration;
		trumpet.weapon.ActivateHitBoxes(true);

		trumpet.StartCoroutine(trumpet.transform.ShakePosition(trumpetShakeDuration, trumpetShakeSpeed, trumpetShakeMagnitude));
		while(wait.MoveNext()) yield return null;

		trumpet.weapon.ActivateHitBoxes(false);

		while(t > 0.0f)
		{
			trumpet.transform.position = Vector3.Lerp(spawnPosition, destinyPoint, t);
			t -= (Time.deltaTime * inverseDuration);
			yield return null;
		}

		trumpet.transform.position = spawnPosition;
		t = 0.0f;
		trumpet.SetActive(false);*/
#endregion
	}

	/// <summary>Cymbals' Routine.</summary>
	/// <param name="boss">Boss' reference.</param>
	private IEnumerator CymbalsRoutine(DestinoBoss boss)
	{
		AudioClip clip = AudioController.PlayOneShot(SourceType.SFX, 1, boss.siMiNoteIndex);
		Animator cymbalsAnimator = cymbals.GetComponent<Animator>();
		AnimationAttacksHandler cymbalsAttacksHandler = cymbals.GetComponent<AnimationAttacksHandler>();
		SecondsDelayWait wait = new SecondsDelayWait(clip.length);
		bool cymbalAttackEnded = false;
		Vector3 projectedMateoPosition = Game.ProjectMateoPosition(1.0f);
		Vector3 initialPos = new Vector3();


		OnAnimationAttackEvent onAnimationAttackEvent = (_state)=>
		{
			switch(_state)
			{
				case AnimationCommandState.Startup:
				cymbals.gameObject.SetActive(true);
				cymbals.weapon.ActivateHitBoxes(false);
				//AudioController.PlayOneShot(SourceType.SFX, 1, cymbalSoundIndex);
				break;

				case AnimationCommandState.Active:
				cymbals.weapon.ActivateHitBoxes(true);
				break;

				case AnimationCommandState.Recovery:
				cymbals.weapon.ActivateHitBoxes(false);
				break;

				case AnimationCommandState.End:
				cymbals.gameObject.SetActive(false);
				cymbals.weapon.ActivateHitBoxes(false);
				cymbalAttackEnded = true;
				break;
			}
		};

		boss.animatorController.CrossFade(boss.lalaCredential, boss.clipFadeDuration);
		/*cymbalsAttacksHandler.onAnimationAttackEvent -= onAnimationAttackEvent;
		cymbalsAttacksHandler.onAnimationAttackEvent += onAnimationAttackEvent;*/
		cymbals.weapon.ActivateHitBoxes(true);

		cymbals.gameObject.SetActive(true);
		//cymbals.transform.position = cymbalsSpawnPosition;

		while(wait.MoveNext()) yield return null;

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);

		wait.ChangeDurationAndReset(cooldownAfterSoundNote);
		while(wait.MoveNext()) yield return null;

		//<summary> Set initial position </summary>
		initialPos.y = projectedMateoPosition.y;

		cymbalsAnimator.SetTrigger(activateCymbalsCredential);
		cymbalsAnimator.transform.position = initialPos;

		//AudioController.PlayOneShot(SourceType.SFX, 1, cymbalSoundIndex);

		yield return null;

		AnimatorStateInfo info = cymbalsAnimator.GetCurrentAnimatorStateInfo(0);
		//clip = AudioController.PlayOneShot(SourceType.SFX, 1, cymbalSoundIndex);
		
		/// Decide the duration of the next wait based on the Max between the note and the animation, so there is assurance that both will end before this routine does.
		//wait.ChangeDurationAndReset(Mathf.Max(info.length, clip.length));
		wait.ChangeDurationAndReset(info.length);

		while(wait.MoveNext()) yield return null;

		//while(!cymbalAttackEnded) yield return null;

#region Before:
		/*int count = 2;
		bool keepWaiting = true;
		OnPoolObjectDeactivation onCymbalDeactivated = (poolObject)=>
		{
			count--;
			if(count <= 0) keepWaiting = false;
		};

		Projectile leftCymbal = PoolManager.RequestProjectile(Faction.Enemy, cymbalIndex, leftCymbalSpawnPoint.position, leftCymbalSpawnPoint.right);
		Projectile rightCymbal = PoolManager.RequestProjectile(Faction.Enemy, cymbalIndex, rightCymbalSpawnPoint.position, rightCymbalSpawnPoint.right);

		leftCymbal.onPoolObjectDeactivation += onCymbalDeactivated;
		rightCymbal.onPoolObjectDeactivation += onCymbalDeactivated;

		while(keepWaiting) yield return null;

		leftCymbal.onPoolObjectDeactivation -= onCymbalDeactivated;
		rightCymbal.onPoolObjectDeactivation -= onCymbalDeactivated;*/
#endregion
	}
}
}