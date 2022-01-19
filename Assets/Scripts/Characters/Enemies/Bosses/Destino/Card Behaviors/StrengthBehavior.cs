using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using  Sirenix.OdinInspector;

using Random = UnityEngine.Random;

namespace Flamingo
{
public class StrengthBehavior : DestinoScriptableCoroutine
{
	private const int LEFT = 1; 																							/// <summary>Left's Index.</summary>
	private const int RIGHT = 2; 																							/// <summary>Right's Index.</summary>

	[SerializeField] private IntRange _setSizeRange; 																		/// <summary>Set Size's Range.</summary>
	[SerializeField] private float _cooldownAfterSoundNote; 																/// <summary>Cooldown's Duration after a sing note is played.</summary>
	[SerializeField] private int _sourceIndex; 																				/// <summary>Sound Effect's Sound Index.</summary>
	[Space(5f)]
	[SerializeField] private float _entranceLerpDuration; 																	/// <summary>Entrance's Interpolation Duration.</summary>
	[SerializeField] private float _exitLerpDuration; 																		/// <summary>Exit's Interpolation Duration.</summary>
	[SerializeField] private Vector3Pair[] _destinoSpawnPointsPairs; 														/// <summary>Destino Spawn-Points' Pairs.</summary>
	[Space(5f)]
	[Header("Drumsticks' Attributes:")]
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private AIContactWeapon _leftDrumstick; 						/// <summary>Left Drumstick's AIContactWeapon.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private AIContactWeapon _rightDrumstick; 						/// <summary>Right Drumstick's AIContactWeapon.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private EulerRotation _rightDrumstickRotation; 				/// <summary>Right Drumstick's Rotation.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private EulerRotation _leftDrumstickRotation; 				/// <summary>Left Drumstick's Rotation.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private int _drumstickSoundIndex; 							/// <summary>Drumsticks Sound's Index.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private AnimatorCredential _drumstickAnimationCredential; 	/// <summary>Drumstick's AnimatorCredential.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private IntRange _drumBeatsSequence; 							/// <summary>Drumstickes' beats sequence's range.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private Vector3 _leftDrumstickSpawnPoint; 					/// <summary>Left Drumstick's Spawn Position.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private Vector3 _rightDrumstickSpawnPoint; 					/// <summary>Right Drumstick's Spawn Position.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private float _maxDrumstickSteeringScalar; 					/// <summary>Max Drumstick's Steering Scalr.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private float _maxDrumstickAnimationSpeed; 					/// <summary>Max Drumstick's Animation Speed.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private float _drumstickOffsetX;								/// <summaty>Drumstrick spawn point offset  </summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private float _drumstickShakeDuration; 						/// <summary>Drumstick's ShakeDuration.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private float _drumstickShakeSpeed; 							/// <summary>Drumstick's Shake Speed.</summary>
	[TabGroup("Weapons Group", "Drumsticks")][SerializeField] private float _drumstickShakeMagnitude; 						/// <summary>Drumstick's Shake Magnitude.</summary>
	[Space(5f)]
	[Header("Trumpet's Attributes:")]
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private AIContactWeapon _trumpet; 								/// <summary>Trumpet's reference.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private int _trumpetSoundIndex; 									/// <summary>Trumpet Sound's Index.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private AnimatorCredential _trumpetAnimationCredential; 			/// <summary>Trumpet's AnimatorCredential.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private Vector3 _trumpetSpawnPoint; 								/// <summary>Trumpet's Spawn Position.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _maxTrumpetSteeringScalar; 						/// <summary>Max Trumpet's Steering Scalar.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _maxTrumpetAnimationSpeed; 						/// <summary>Max Trumpet's Animation Speed.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _entranceDuration; 								/// <summary>Trumpet's Entrance Duration.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _exitDuration; 									/// <summary>Trumpet's Exit Duration.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _soundEmissionDuration; 							/// <summary>Trumpet's Sound Emission Duration.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _trumpetShakeDuration; 							/// <summary>Trumpet's ShakeDuration.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _trumpetShakeSpeed; 								/// <summary>Trumpet's Shake Speed.</summary>
	[TabGroup("Weapons Group", "Trumpet")][SerializeField] private float _trumpetShakeMagnitude; 							/// <summary>Trumpet's Shake Magnitude.</summary>
	[Space(5f)]
	[Header("Cymbals' Attributes:")]
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private AIContactWeapon _cymbals; 								/// <summary>Cymbals' Reference.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private int _cymbalSoundIndex; 									/// <summary>Cymbal Sound's Index.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private AnimatorCredential _cymbalsAnimationCredential; 			/// <summary>Cymbals' AnimatorCredential.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private float _maxCymbalsSteeringScalar; 						/// <summary>Max Cymbals' Steering Scalar.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private float _maxCymbalsAnimationSpeed; 						/// <summary>Max Cymbals' Animation Speed.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private float _cymbalsShakeDuration; 							/// <summary>Cymbals' ShakeDuration.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private float _cymbalsShakeSpeed; 								/// <summary>Cymbals' Shake Speed.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private float _cymbalsShakeMagnitude; 							/// <summary>Cymbals' Shake Magnitude.</summary>
	[TabGroup("Weapons Group", "Cymbals")][SerializeField] private float _cymbalsYOffset; 									/// <summary>Cymbals' Y Offset.</summary>

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

	/// <summary>Gets rightDrumstickRotation property.</summary>
	public EulerRotation rightDrumstickRotation { get { return _rightDrumstickRotation; } }

	/// <summary>Gets leftDrumstickRotation property.</summary>
	public EulerRotation leftDrumstickRotation { get { return _leftDrumstickRotation; } }

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

	/// <summary>Gets sourceIndex property.</summary>
	public int sourceIndex { get { return _sourceIndex; } }

	/// <summary>Gets maxDrumstickSteeringScalar property.</summary>
	public float maxDrumstickSteeringScalar { get { return _maxDrumstickSteeringScalar; } }

	/// <summary>Gets maxDrumstickAnimationSpeed property.</summary>
	public float maxDrumstickAnimationSpeed { get { return _maxDrumstickAnimationSpeed; } }

	/// <summary>Gets drumstickOffsetX property.</summary>
	public float drumstickOffsetX { get { return _drumstickOffsetX;}}

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

	/// <summary>Gets maxTrumpetSteeringScalar property.</summary>
	public float maxTrumpetSteeringScalar { get { return _maxTrumpetSteeringScalar; } }

	/// <summary>Gets maxTrumpetAnimationSpeed property.</summary>
	public float maxTrumpetAnimationSpeed { get { return _maxTrumpetAnimationSpeed; } }

	/// <summary>Gets trumpetShakeDuration property.</summary>
	public float trumpetShakeDuration { get { return _trumpetShakeDuration; } }

	/// <summary>Gets trumpetShakeSpeed property.</summary>
	public float trumpetShakeSpeed { get { return _trumpetShakeSpeed; } }

	/// <summary>Gets trumpetShakeMagnitude property.</summary>
	public float trumpetShakeMagnitude { get { return _trumpetShakeMagnitude; } }

	/// <summary>Gets maxCymbalsSteeringScalar property.</summary>
	public float maxCymbalsSteeringScalar { get { return _maxCymbalsSteeringScalar; } }

	/// <summary>Gets maxCymbalsAnimationSpeed property.</summary>
	public float maxCymbalsAnimationSpeed { get { return _maxCymbalsAnimationSpeed; } }

	/// <summary>Gets cymbalsShakeDuration property.</summary>
	public float cymbalsShakeDuration { get { return _cymbalsShakeDuration; } }

	/// <summary>Gets cymbalsShakeSpeed property.</summary>
	public float cymbalsShakeSpeed { get { return _cymbalsShakeSpeed; } }

	/// <summary>Gets cymbalsYOffset property.</summary>
	public float cymbalsYOffset { get { return _cymbalsYOffset; } }

	/// <summary>Gets cymbalsShakeMagnitude property.</summary>
	public float cymbalsShakeMagnitude { get { return _cymbalsShakeMagnitude; } }

	/// <summary>Gets cooldownAfterSoundNote property.</summary>
	public float cooldownAfterSoundNote { get { return _cooldownAfterSoundNote; } }

	/// <summary>Gets entranceLerpDuration property.</summary>
	public float entranceLerpDuration { get { return _entranceLerpDuration; } }

	/// <summary>Gets exitLerpDuration property.</summary>
	public float exitLerpDuration { get { return _exitLerpDuration; } }

	/// <summary>Gets drumstickSoundIndex property.</summary>
	public int drumstickSoundIndex { get { return _drumstickSoundIndex; } }

	/// <summary>Gets trumpetSoundIndex property.</summary>
	public int trumpetSoundIndex { get { return _trumpetSoundIndex; } }

	/// <summary>Gets cymbalSoundIndex property.</summary>
	public int cymbalSoundIndex { get { return _cymbalSoundIndex; } }

	/// <summary>Gets destinoSpawnPointsPairs property.</summary>
	public Vector3Pair[] destinoSpawnPointsPairs { get { return _destinoSpawnPointsPairs; } }

	/// <summary>Gets drumstickAnimationCredential property.</summary>
	public AnimatorCredential drumstickAnimationCredential { get { return _drumstickAnimationCredential; } }

	/// <summary>Gets trumpetAnimationCredential property.</summary>
	public AnimatorCredential trumpetAnimationCredential { get { return _trumpetAnimationCredential; } }

	/// <summary>Gets cymbalsAnimationCredential property.</summary>
	public AnimatorCredential cymbalsAnimationCredential { get { return _cymbalsAnimationCredential; } }
#endregion

	/// <summary>Callback invoked when drawing Gizmos.</summary>
	protected override void DrawGizmos()
	{
#if UNITY_EDITOR
		base.DrawGizmos();

		Gizmos.color = gizmosColor;

		Gizmos.DrawWireSphere(leftDrumstickSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(rightDrumstickSpawnPoint, gizmosRadius);
		Gizmos.DrawWireSphere(trumpetSpawnPoint, gizmosRadius);

		if(destinoSpawnPointsPairs != null) foreach(Vector3Pair pair in destinoSpawnPointsPairs)
		{
			Gizmos.DrawWireSphere(pair.a, gizmosRadius);
			Gizmos.DrawWireSphere(pair.b, gizmosRadius);
		}

		/// \TODO Correct the positioning on the Text...
		/*VGizmos.DrawText("Trumpet's Spawn Position", trumpetSpawnPoint, gizmosTextOffset, Color.white);
		VGizmos.DrawText("Trumpet's Destiny Position", trumpetDestinyPoint, gizmosTextOffset, Color.white);*/
#endif
	}

	/// <summary>StrengthBehavior's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		leftDrumstick.gameObject.SetActive(false);
		rightDrumstick.gameObject.SetActive(false);
		cymbals.gameObject.SetActive(false);
		trumpet.gameObject.SetActive(false);

		leftDrumstick.animationsEventInvoker.AddIntActionListener(OnLeftDrumstickAnimationEvent);
		rightDrumstick.animationsEventInvoker.AddIntActionListener(OnRightDrumstickAnimationEvent);
		cymbals.animationsEventInvoker.AddIntActionListener(OnCymbalsAnimationEvent);
		trumpet.animationsEventInvoker.AddIntActionListener(OnTrumpetAnimationEvent);
	}

#region Callbacks:
	/// <summary>Callback invoked when the Left Drumstick's Animation sends an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnLeftDrumstickAnimationEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.ANIMATIONEVENT_DEACTIVATEHITBOXES:
			leftDrumstick.weapon.ActivateHitBoxes(false);
			break;

			case IDs.ANIMATIONEVENT_ACTIVATEHITBOXES:
			leftDrumstick.weapon.ActivateHitBoxes(true);
			leftDrumstick.state = AnimationCommandState.Active;
			break;

			case IDs.ANIMATIONEVENT_EMITSOUND_0:
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, drumstickSoundIndex);
			break;
		}
	}

	/// <summary>Callback invoked when the Right Drumstick's Animation sends an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnRightDrumstickAnimationEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.ANIMATIONEVENT_DEACTIVATEHITBOXES:
			rightDrumstick.weapon.ActivateHitBoxes(false);
			break;

			case IDs.ANIMATIONEVENT_ACTIVATEHITBOXES:
			rightDrumstick.weapon.ActivateHitBoxes(true);
			rightDrumstick.state = AnimationCommandState.Active;
			break;

			case IDs.ANIMATIONEVENT_EMITSOUND_0:
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, drumstickSoundIndex);
			break;
		}
	}

	/// <summary>Callback invoked when the Cymbals' Animation sends an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnCymbalsAnimationEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.ANIMATIONEVENT_DEACTIVATEHITBOXES:
			cymbals.weapon.ActivateHitBoxes(false);
			break;

			case IDs.ANIMATIONEVENT_ACTIVATEHITBOXES:
			cymbals.weapon.ActivateHitBoxes(true);
			cymbals.state = AnimationCommandState.Active;
			break;

			case IDs.ANIMATIONEVENT_EMITSOUND_0:
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, cymbalSoundIndex);
			break;
		}
	}

	/// <summary>Callback invoked when the Trumpet's Animation sends an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnTrumpetAnimationEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.ANIMATIONEVENT_DEACTIVATEHITBOXES:
			trumpet.weapon.ActivateHitBoxes(false);
			break;

			case IDs.ANIMATIONEVENT_ACTIVATEHITBOXES:
			trumpet.weapon.ActivateHitBoxes(true);
			trumpet.state = AnimationCommandState.Active;
			break;

			case IDs.ANIMATIONEVENT_EMITSOUND_0:
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, trumpetSoundIndex);
			break;
		}
	}
#endregion

#region Coroutines:
	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss)
	{		
		int setSize = setSizeRange.Random();
		IEnumerator[] routines = new IEnumerator[setSize];
		IEnumerator routine = null;

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);
		AudioController.StopFSMLoop(0);
		AudioController.StopFSMLoop(1);

		for(int i = 0; i < setSize; i++)
		{
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

		foreach(IEnumerator instrumentRoutine in routines)
		{
			while(instrumentRoutine.MoveNext()) yield return null;
		}

		AudioController.PlayFSMLoop(0, DestinoSceneController.Instance.mainLoopIndex);
		AudioController.PlayFSMLoop(1, DestinoSceneController.Instance.mainLoopVoiceIndex, true, boss.Sing);

		routine = DestinoSceneController.TakeDestinoToInitialPoint();

		//boss.animatorController.Play(boss.idleCredential);

		while(routine.MoveNext()) yield return null;

		InvokeCoroutineEnd();
	}

	/// <summary>Drumsticks' Routine.</summary>
	/// <param name="boss">Destino's reference.</param>
	private IEnumerator DrumsticksRoutine(DestinoBoss boss)
	{
		Renderer drumstickRenderer = leftDrumstick.weapon.meshContainer.GetComponent<Renderer>();
		float drumstickLength = drumstickRenderer.bounds.size.y;
		int[] drumstickCombo = VArray.RandomSet(LEFT, RIGHT);
		float s = boss.stageScale;
		float scalar = Mathf.Lerp(1.0f, maxDrumstickSteeringScalar, s);
		IEnumerator[] drumsticksRoutines = new IEnumerator[]
		{
			DrumstickRoutine(leftDrumstick, leftDrumstickSpawnPoint, Math.Min, -1.0f, scalar, s, drumstickLength),
			DrumstickRoutine(rightDrumstick, rightDrumstickSpawnPoint, Math.Max, 1.0f, scalar, s, drumstickLength)
		};
		IEnumerator noteRoutine = PlayNote(boss, boss.reFaNoteIndex);

		boss.animatorController.Play(boss.lalaCredential);

		leftDrumstick.vehicle.ResetVelocity();
		rightDrumstick.vehicle.ResetVelocity();
		leftDrumstick.transform.rotation = leftDrumstickRotation;
		rightDrumstick.transform.rotation = rightDrumstickRotation;
		leftDrumstick.transform.position = leftDrumstickSpawnPoint;
		rightDrumstick.transform.position = rightDrumstickSpawnPoint;

		//SecondsDelayWait wait = new SecondsDelayWait(clip.length);
		while(noteRoutine.MoveNext()) yield return null;		//while(wait.MoveNext()) yield return null;

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);

/*#region ComboBullshit:
		for(int i = 0; i < drumstickCombo.Length; i++)
		{

			//Set Position of drumstrick acording to mateo position
			AIContactWeapon drumstick = null;
			Vector3 mateoPosition = Vector3.zero;
			Vector3 spawnPosition = Vector3.zero;
			bool animationEnded = false;
			Func<float, float, float> f = null;
			float sign = 0.0f;

			leftDrumstick.state = AnimationCommandState.None;
			rightDrumstick.state = AnimationCommandState.None;

			switch(drumstickCombo[i])
			{
				case LEFT: 
					spawnPosition = leftDrumstickSpawnPoint;
					drumstick = leftDrumstick;
					leftDrumstick.gameObject.SetActive(true);
					rightDrumstick.gameObject.SetActive(false);
					f = Mathf.Min;
					sign = -1.0f;
				break;

				case RIGHT:
					spawnPosition = rightDrumstickSpawnPoint;
					drumstick = rightDrumstick;
					leftDrumstick.gameObject.SetActive(false);
					rightDrumstick.gameObject.SetActive(true);
					f = Mathf.Max;
					sign = 1.0f;
				break;
			}
			
			drumstick.animator.speed = Mathf.Lerp(1.0f, maxDrumstickAnimationSpeed, s);
			drumstick.animatorController.CrossFadeAndWait(drumstickAnimationCredential, 0.3f, 0, 0.0f, 0.0f, ()=>{ animationEnded = true; });

			while(!animationEnded)
			{
				if(drumstick.state != AnimationCommandState.Active)
				{
					mateoPosition = Game.mateo.transform.position;
					mateoPosition.x = f(mateoPosition.x + (drumstickLength * sign), 0.0f);
					mateoPosition.y = spawnPosition.y;

					drumstick.transform.position += (Vector3)(drumstick.vehicle.GetSeekForce(mateoPosition)) * Time.deltaTime * scalar;
				}

				yield return null;
			}

			drumstick.gameObject.SetActive(false);
		}
#endregion*/

		while(drumsticksRoutines[0].MoveNext() && drumsticksRoutines[1].MoveNext()) yield return null;

		leftDrumstick.SetActive(false);
		rightDrumstick.SetActive(false);
	}

	/// <summary>Trumpet's Routine.</summary>
	/// <param name="boss">Destino's reference.</param>
	private IEnumerator TrumpetRoutine(DestinoBoss boss)
	{
		/*
			- Destino Plays "La-Re" Soundbit
			- Wait until the soundbit is finished
			- Wait another cooldown
			- Play the animation, while it plays, steer towards Astro Boy
			- Stop steering once the Hit-Boxes are active
		*/
		Vector3 projectedMateoPosition = Vector3.zero;
		Vector3 initialPos = trumpetSpawnPoint;
		float s = boss.stageScale;
		float scalar = Mathf.Lerp(1.0f, maxTrumpetSteeringScalar, s);
		bool animationEnded = false;
		IEnumerator noteRoutine = PlayNote(boss, boss.laReNoteIndex);

		boss.animatorController.Play(boss.lalaCredential);
		trumpet.state = AnimationCommandState.None;
		trumpet.animator.speed = Mathf.Lerp(1.0f, maxTrumpetSteeringScalar, s);
		trumpet.gameObject.SetActive(true);
		trumpet.transform.position = trumpetSpawnPoint;

		while(noteRoutine.MoveNext()) yield return null;

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);
		trumpet.animatorController.CrossFadeAndWait(trumpetAnimationCredential, 0.3f, 0, 0.0f, 0.0f, ()=> { animationEnded = true; });

		while(!animationEnded)
		{
			if(trumpet.state != AnimationCommandState.Active)
			{
				projectedMateoPosition = new Vector3(
					Game.mateo.transform.position.x,
					trumpetSpawnPoint.y,
					0.0f
				);
				trumpet.transform.position += (Vector3)(trumpet.vehicle.GetSeekForce(projectedMateoPosition)) * Time.deltaTime * scalar;
			}
			yield return null;
		}

		trumpet.gameObject.SetActive(false);
	}

	/// <summary>Cymbals' Routine.</summary>
	/// <param name="boss">Boss' reference.</param>
	private IEnumerator CymbalsRoutine(DestinoBoss boss)
	{
		/*
			- Destino Plays "Si-Mi" Soundbit
			- Wait until the soundbit is finished
			- Wait another cooldown
			- Play the animation, while it plays, steer towards Astro Boy
			- Stop steering once the Hit-Boxes are active
		*/
		Vector3 projectedMateoPosition = Vector3.zero;
		Vector3 initialPos = Vector3.zero;
		float s = boss.stageScale;
		float scalar = Mathf.Lerp(1.0f, maxCymbalsSteeringScalar, s);
		bool animationEnded = false;
		IEnumerator noteRoutine = PlayNote(boss, boss.siMiNoteIndex);

		boss.animatorController.Play(boss.lalaCredential);
		cymbals.state = AnimationCommandState.None;
		cymbals.animator.speed = Mathf.Lerp(1.0f, maxCymbalsAnimationSpeed, s);
		cymbals.gameObject.SetActive(true);
		cymbals.transform.position = initialPos;

		while(noteRoutine.MoveNext()) yield return null;

		boss.animatorController.CrossFade(boss.idleCredential, boss.clipFadeDuration);
		cymbals.animatorController.CrossFadeAndWait(cymbalsAnimationCredential, 0.3f, 0, 0.0f, 0.0f, ()=> { animationEnded = true; });

		while(!animationEnded)
		{
			if(cymbals.state != AnimationCommandState.Active)
			{
				projectedMateoPosition = new Vector3(
					0.0f,
					Mathf.Max(Game.mateo.transform.position.y, cymbalsYOffset),
					0.0f
				);
				cymbals.transform.position += (Vector3)(cymbals.vehicle.GetSeekForce(projectedMateoPosition)) * Time.deltaTime * scalar;
			}
			yield return null;
		}

		cymbals.gameObject.SetActive(false);
	}

	/// <summary>Individual Drumstick Routine.</summary>
	/// <param name="_drumstick">Drumstick's Reference as AIContactWeapon.</param>
	/// <param name="_spawnPosition">Drumstick's Spawn Position.</param>
	/// <param name="f">Function to clamp the positioning of the Drumstick.</param>
	/// <param name="sign">Sign multiplier.</param>
	/// <param name="steeringScalar">Drumstick's Steering Scalar.</param>
	/// <param name="stageScalar">Boss Stage's Scalar.</param>
	private IEnumerator DrumstickRoutine(AIContactWeapon _drumstick, Vector3 _spawnPosition, Func<float, float, float> f, float sign, float steeringScalar, float stageScalar, float drumstickLength)
	{
		Vector3 mateoPosition = Vector3.zero;
		bool animationEnded = false;

		_drumstick.gameObject.SetActive(true);
		_drumstick.state = AnimationCommandState.None;
		_drumstick.animator.speed = Mathf.Lerp(1.0f, maxDrumstickAnimationSpeed, stageScalar);
		_drumstick.animatorController.CrossFadeAndWait(drumstickAnimationCredential, 0.3f, 0, 0.0f, 0.0f, ()=>{ animationEnded = true; });

		while(!animationEnded)
		{
			if(_drumstick.state != AnimationCommandState.Active)
			{
				mateoPosition = Game.mateo.transform.position;
				mateoPosition.x = f(mateoPosition.x + (drumstickLength * sign), 0.0f);
				mateoPosition.y = _spawnPosition.y;

				_drumstick.transform.position += (Vector3)(_drumstick.vehicle.GetSeekForce(mateoPosition)) * Time.deltaTime * steeringScalar;
			}

			yield return null;
		}

		_drumstick.gameObject.SetActive(false);
	}

	/// <summary>Lerps through a waypoint pair and plays a note.</summary>
	/// <param name="boss">Destino's Reference.</param>
	/// <param name="noteCredential">Note's Credential's Index.</param>
	private IEnumerator PlayNote(DestinoBoss boss, int noteCredential)
	{
		Vector3Pair pair  = destinoSpawnPointsPairs.Random();
		float t = 0.0f;
		Vector3 up = (pair.b - pair.a).normalized;
		float inverseDuration = 1.0f / entranceLerpDuration;
		bool playFinished = false;
		Action onPlayFinished = ()=> { playFinished = true; };

		boss.transform.rotation = Quaternion.LookRotation(Vector3.forward, up);

		while(t < 1.0f)
		{
			boss.transform.position = Vector3.Lerp(pair.a, pair.b, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		AudioClip clip = AudioController.PlayOneShot(SourceType.SFX, 1, noteCredential);
		SecondsDelayWait wait = new SecondsDelayWait(clip.length);
		boss.transform.position = pair.b;
		boss.animatorController.PlayAndWait(noteCredential, 0, Mathf.NegativeInfinity, 0.0f, onPlayFinished);

		while(!playFinished) yield return null;

		t = 0.0f;
		inverseDuration = 1.0f / exitLerpDuration;

		while(t < 1.0f)
		{
			boss.transform.position = Vector3.Lerp(pair.b, pair.a, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		boss.transform.position = pair.a;

		wait.ChangeDurationAndReset(cooldownAfterSoundNote);
		while(wait.MoveNext()) yield return null;
	}
#endregion

}
}