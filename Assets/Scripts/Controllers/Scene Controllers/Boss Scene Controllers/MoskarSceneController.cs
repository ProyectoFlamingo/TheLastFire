using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Voidless;
using UnityEngine.Audio;

/*
 - Calculate the total of Moskars to destroy:
 
	2 ^ 0 = 1
	2 ^ 1 = 2
	2 ^ 2 = 4
	2 ^ 3 = 8
	2 ^ 4 = 16
	Total = 31
*/

namespace Flamingo
{
[RequireComponent(typeof(Boundaries2DContainer))]
public class MoskarSceneController : Singleton<MoskarSceneController>
{
	[Space(5f)]
	[SerializeField] private float _fadeOutDuration; 				/// <summary>Fade-Out's Duration.</summary>
	[Space(5f)]
	[SerializeField] private MoskarBoss _main; 						/// <summary>Initial Moskar's Reference.</summary>
	[Space(5f)]
	[Header("Reproductions' Atrributes:")]
	[SerializeField] private int _moskarIndex; 						/// <summary>Moskar's Index.</summary>
	[SerializeField] private float _reproductionDuration; 			/// <summary>Reproduction Duration. Determines how long it lasts the reproduction's displacement and scaling.</summary>
	[SerializeField] private float _reproductionPushForce; 			/// <summary>Reproduction's Push Force.</summary>
	[SerializeField] private float _reproductionCountdown; 			/// <summary>Duration before creating another round of moskars.</summary>
	[SerializeField] private float _speedScale; 					/// <summary>Remaining Moskars Speed Scale.</summary>
	[SerializeField] private int _remainingMoskarsForSpeedScale; 	/// <summary>Minimum of Remaining Moskars needed for an additional speed scale.</summary>
	[Space(5f)]
	[Header("Music's Attributes:")]
	[SerializeField] private FloatRange _waitBetweenPiece; 			/// <summary>Wait Duration Between each piece.</summary>
	[SerializeField] private int[] _piecesIndices; 					/// <summary>Pieces' Indices.</summary>
	[SerializeField] private int _flyLoop; 							/// <summary>Fly's Loop Index.</summary>
	[SerializeField] private int _remainingMoskarsForLastPiece; 	/// <summary>Maximum required of remaining Moskars for the last piece's loop to be reproduced.</summary>
	[SerializeField] private int _sampleJumping; 					/// <summary>Sample Jumping by each iteration.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _moskarSFXVolume; 			/// <summary>Volume for Moskar's SFXs.</summary>
	[Space(5f)]
	[Header("Mandala's Attributes:")]
	[SerializeField] private Mandala _enterMandala;					/// <summary>Enter Mandala's Reference.</summary>
	[SerializeField] private Mandala _exitMandala;					/// <summary>Exit Mandala's Reference.</summary>
	[SerializeField] private Vector3 _enterMandalaSpawnPosition; 	/// <summary>Enter Mandala's spawn Position.</summary>
	[SerializeField] private Vector3 _exitMandalaSpawnPosition; 	/// <summary>Exit Mandala's spawn Position.</summary>
	private Boundaries2DContainer _moskarBoundaries; 				/// <summary>Moskar's Boundaries.</summary>
	private HashSet<MoskarBoss> _moskarReproductions; 				/// <summary>Moskar's Reproductions.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 					/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 					/// <summary>Gizmos' Radius.</summary>
#endif
	private Coroutine moskarReproductionsCountdown; 				/// <summary>MoskarReproductionsCountdown's Coroutine reference.</summary>
	private float _totalMoskars; 									/// <summary>Total of Moskars.</summary>
	private float _moskarsDestroyed; 								/// <summary>Moskars Destroyed.</summary>

#region Getters/Setters:
	/// <summary>Gets fadeOutDuration property.</summary>
	public float fadeOutDuration { get { return _fadeOutDuration; } }

	/// <summary>Gets and Sets main property.</summary>
	public MoskarBoss main
	{
		get { return _main; }
		set { _main = value; }
	}

	/// <summary>Gets moskarIndex property.</summary>
	public int moskarIndex { get { return _moskarIndex; } }

	/// <summary>Gets and Sets reproductionDuration property.</summary>
	public float reproductionDuration
	{
		get { return _reproductionDuration; }
		set { _reproductionDuration = value; }
	}

	/// <summary>Gets and Sets reproductionPushForce property.</summary>
	public float reproductionPushForce
	{
		get { return _reproductionPushForce; }
		set { _reproductionPushForce = value; }
	}

	/// <summary>Gets and Sets reproductionCountdown property.</summary>
	public float reproductionCountdown
	{
		get { return _reproductionCountdown; }
		set { _reproductionCountdown = value; }
	}

	/// <summary>Gets and Sets moskarSFXVolume property.</summary>
	public float moskarSFXVolume
	{
		get { return _moskarSFXVolume; }
		set { _moskarSFXVolume = value; }
	}

	/// <summary>Gets and Sets speedScale property.</summary>
	public float speedScale
	{
		get { return _speedScale; }
		set { _speedScale = value; }
	}

	/// <summary>Gets and Sets remainingMoskarsForLastPiece property.</summary>
	public int remainingMoskarsForLastPiece
	{
		get { return _remainingMoskarsForLastPiece; }
		set { _remainingMoskarsForLastPiece = value; }
	}

	/// <summary>Gets and Sets sampleJumping property.</summary>
	public int sampleJumping
	{
		get { return _sampleJumping; }
		set { _sampleJumping = value; }
	}

	/// <summary>Gets and Sets remainingMoskarsForSpeedScale property.</summary>
	public int remainingMoskarsForSpeedScale
	{
		get { return _remainingMoskarsForSpeedScale; }
		set { _remainingMoskarsForSpeedScale = value; }
	}

	/// <summary>Gets and Sets waitBetweenPiece property.</summary>
	public FloatRange waitBetweenPiece
	{
		get { return _waitBetweenPiece; }
		set { _waitBetweenPiece = value; }
	}

	/// <summary>Gets and Sets piecesIndices property.</summary>
	public int[] piecesIndices
	{
		get { return _piecesIndices; }
		set { _piecesIndices = value; }
	}

	/// <summary>Gets and Sets flyLoop property.</summary>
	public int flyLoop
	{
		get { return _flyLoop; }
		set { _flyLoop = value; }
	}

	/// <summary>Gets moskarBoundaries Component.</summary>
	public Boundaries2DContainer moskarBoundaries
	{ 
		get
		{
			if(_moskarBoundaries == null) _moskarBoundaries = GetComponent<Boundaries2DContainer>();
			return _moskarBoundaries;
		}
	}

	/// <summary>Gets and Sets moskarReproductions property.</summary>
	public HashSet<MoskarBoss> moskarReproductions
	{
		get { return _moskarReproductions; }
		private set { _moskarReproductions = value; }
	}

	/// <summary>Gets and Sets totalMoskars property.</summary>
	public float totalMoskars
	{
		get { return _totalMoskars; }
		set { _totalMoskars = value; }
	}

	/// <summary>Gets and Sets moskarsDestroyed property.</summary>
	public float moskarsDestroyed
	{
		get { return _moskarsDestroyed; }
		set { _moskarsDestroyed = value; }
	}

	/// <summary>Gets enterMandala property.</summary>
	public Mandala enterMandala { get { return _enterMandala; } }

	/// <summary>Gets exitMandala property.</summary>
	public Mandala exitMandala { get { return _exitMandala; } }

	/// <summary>Gets enterMandalaSpawnPosition property.</summary>
	public Vector3 enterMandalaSpawnPosition { get { return _enterMandalaSpawnPosition; } }

	/// <summary>Gets exitMandalaSpawnPosition property.</summary>
	public Vector3 exitMandalaSpawnPosition { get { return _exitMandalaSpawnPosition; } }
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when MoskarSceneController's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireSphere(enterMandalaSpawnPosition, 0.5f);
		Gizmos.DrawWireSphere(exitMandalaSpawnPosition, 0.5f);
	}
#endif

	/// <summary>Callback internally invoked after Awake.</summary>
	protected override void OnAwake()
	{
		moskarReproductions = new HashSet<MoskarBoss>();

// --- Begins New Implementation: ---
		if(main != null)
		{
			moskarReproductions.Add(main);
			SubscribeToMoskarEvents(main);

			totalMoskars = 0.0f;

			for(float i = 0; i < main.phases; i++)
			{
				totalMoskars += Mathf.Pow(2.0f, i);
			}
		}

		enterMandala.gameObject.SetActive(false);
		exitMandala.gameObject.SetActive(false);

		Game.mateo.eventsHandler.onIDEvent += OnMateoIDEvent;
// --- Ends New Implementation: ---

// --- Old Implementation: ---
		//this.StartCoroutine(MoskarReproductionsCountdown(), ref moskarReproductionsCountdown);
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		if(piecesIndices != null)
		{
			int index = default(int);

			for(int i = 0; i < piecesIndices.Length; i++)
			{
				index = piecesIndices[i];
				AudioController.Play(SourceType.Loop, i, index, true);
				AudioController.SetVolume(SourceType.Loop, i, 0.0f);
			}
		}

		AudioController.Play(SourceType.Scenario, 0, flyLoop, true);
		AudioController.SetVolume(SourceType.SFX, main.sourceIndex, moskarSFXVolume);

		Game.EnablePlayerControl(false);
		Game.FadeOutScreen(Color.black, fadeOutDuration,
		()=>
		{
			Game.EnablePlayerControl(true);
		});		
	}

	/// <summary>Callback invoked when MoskarSceneController's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		//Game.mateo.eventsHandler.onIDEvent -= OnMateoIDEvent;
	}

	/// <summary>Updates MoskarSceneController's instance at each frame.</summary>
	private void Update()
	{
		Vector3 min = moskarBoundaries.min;
		Vector3 max = moskarBoundaries.max;

		/// Temporal, constraint Moskars on the boundaires.
		foreach(MoskarBoss moskar in moskarReproductions)
		{
			Vector3 position = moskar.transform.position;

			moskar.transform.position = new Vector3
			(
				Mathf.Clamp(position.x, min.x, max.x),
				Mathf.Clamp(position.y, min.y, max.y),
				position.z
			);
		}
	}

	/// <summary>Callback invoked when any Moskar Reproduction invokes an ID Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnMoskarIDEvent(int _ID)
	{
		switch(_ID)
		{
			/*case Boss.ID_EVENT_PLAYERSIGHTED_BEGINS:
			foreach(MoskarBoss moskar in moskarReproductions)
			{
				if(!moskar.HasStates(IDs.STATE_ATTACKING))
				moskar.AddStates(IDs.STATE_ATTACKING);
			}
			break;*/

			case IDs.EVENT_DEATHROUTINE_ENDS:
			/*if(moskarsDestroyed < totalMoskars) return;

			Game.EnablePlayerControl(false);
			Game.mateo.Meditate(true);
			exitMandala.gameObject.SetActive(true);
			exitMandala.transform.position = exitMandalaSpawnPosition;
			exitMandala.enabled = true;*/
			break;
		}
	}

	/// <summary>Event invoked when the projectile is deactivated.</summary>
	/// <param name="_enemy">Enemy that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	public void OnMoskarDeactivated(Character _enemy, DeactivationCause _cause, Trigger2DInformation _info)
	{
		if(_cause != DeactivationCause.Destroyed) return;

		MoskarBoss moskar = _enemy as MoskarBoss;

		if(moskar == null) return;

		if(!moskarReproductions.Remove(moskar)) return;

		SubscribeToMoskarEvents(moskar, false);

// --- Begins New Implementation: --- 
		if(moskar.currentPhase < (moskar.phases - 1))
		{
			MoskarBoss reproduction = null;
			Vector3[] forces = new Vector3[] { Vector3.left * reproductionPushForce, Vector3.right * reproductionPushForce };
			TimeConstrainedForceApplier2D[] reproductionPushes = new TimeConstrainedForceApplier2D[2];
			Vector3 scale = moskar.transform.localScale;
			int phase = moskar.currentPhase;
			float phases = 1.0f * moskar.phases;
			float t = ((1.0f * phase) / phases);
			float it = 1.0f - t;
			float sizeScale = moskar.scaleRange.Lerp(it);
			float sphereColliderSize = moskar.sphereColliderSizeRange.Lerp(it);

			phase++;

			PoolManager.RequestParticleEffect(moskar.duplicateParticleEffectIndex, moskar.transform.position, Quaternion.identity);

			for(int i = 0; i < 2; i++)
			{
				reproduction = PoolManager.RequestPoolGameObject(moskarIndex, moskar.transform.position, moskar.transform.rotation) as MoskarBoss;
				SubscribeToMoskarEvents(reproduction);
				reproduction.state = 0;
				reproduction.AddStates(IDs.STATE_ATTACKING);
				reproduction.AddStates(IDs.STATE_ALIVE);
				reproduction.currentPhase = phase;
				reproduction.health.BeginInvincibilityCooldown();
				reproduction.meshParent.localScale = scale;
				reproduction.phaseProgress = t;
				reproduction.hurtBox.radius = sphereColliderSize;
				moskarReproductions.Add(reproduction);

				//reproduction.rigidbody.simulated = false;
				reproductionPushes[i] = new TimeConstrainedForceApplier2D(this, reproduction.rigidbody, forces[i], reproductionDuration, ForceMode.VelocityChange, reproduction.SimulateInteractionsAndResetVelocity);

				this.StartCoroutine(reproduction.meshParent.RegularScale(sizeScale, reproductionDuration));
				reproductionPushes[i].ApplyForce();
			}
		}

		moskarsDestroyed++;

		float mt = moskarsDestroyed / (totalMoskars - (float)remainingMoskarsForLastPiece);

		for(int i = 0; i < Mathf.Lerp(0, piecesIndices.Length, mt); i++)
		{
			AudioController.SetVolume(SourceType.Loop, i, 1.0f);
		}

		if(Mathf.Abs(moskarsDestroyed - totalMoskars) <= remainingMoskarsForSpeedScale)
		{
			foreach(MoskarBoss reproduction in moskarReproductions)
			{
				reproduction.speedScale = speedScale;
				reproduction.vehicle.maxSpeed *= speedScale;
				reproduction.vehicle.maxForce *= speedScale;
			}
		}

		if(moskarsDestroyed >= totalMoskars)
		{
			AudioController.Stop(SourceType.Scenario, 0);
			Game.EnablePlayerControl(false);
			Game.mateo.Meditate(true);
			exitMandala.gameObject.SetActive(true);
			exitMandala.transform.position = exitMandalaSpawnPosition;
			exitMandala.enabled = true;
		}

// --- Ends New Implementation ---

// --- Begins Old Implementation: ---
		/*if(moskarReproductions.Count <= 0)
		{
			this.DispatchCoroutine(ref moskarReproductionsCountdown);
		}*/
// --- Ends Old Implementation: ---
	}

	/// <summary>Callback invoked when Mateo invokes an ID Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnMateoIDEvent(int _ID)
	{
		switch(_ID)
		{
			case IDs.EVENT_MEDITATION_BEGINS:
			foreach(MoskarBoss moskar in moskarReproductions)
			{
				//moskar.ChangeState(IDs.STATE_IDLE);
				moskar.RemoveStates(IDs.STATE_ATTACKING);
				moskar.AddStates(IDs.STATE_IDLE);
			}
			break;
		}
	}

	/// <summary>Subscribes to Moskar's Events.</summary>
	/// <param name="_moskar">Moskar that contains the events.</param>
	/// <param name="_subscribe">Subscribe? true by default.</param>
	private void SubscribeToMoskarEvents(MoskarBoss _moskar, bool _subscribe = true)
	{
		if(_moskar == null) return;

		EventsHandler eventsHandler = _moskar.eventsHandler;

		eventsHandler.onCharacterDeactivated -= OnMoskarDeactivated;
		eventsHandler.onIDEvent -= OnMoskarIDEvent;

		if(!_subscribe) return;

		eventsHandler.onCharacterDeactivated += OnMoskarDeactivated;
		eventsHandler.onIDEvent += OnMoskarIDEvent;
	}

#region DEPRECATED
	/// <summary>Pieces' Routine.</summary>
	private IEnumerator PiecesRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(0.0f);
		AudioClip clip = null;
		float[] samples = null;
		int index = 0;
		//float speed = 

		while(true)
		{
			index = piecesIndices.Random();
			clip = AudioController.Play(SourceType.Loop, 0, index, false);
			samples = clip.GetAudioClipSamples();
			wait.ChangeDurationAndReset(clip.length);
			
			while(wait.MoveNext()) yield return null;

			// 44100hz samples per-second...
			/*for(int i = 0; i < samples.Length;  i += Mathf.Max(sampleJumping, 1))
			{
				float sample = samples[i];

				foreach(MoskarBoss moskar in moskarReproductions)
				{
					//moskar
				}
				yield return null;
			}*/
		}
	}

	/// <summary>Moskar Reproduction's Countdown Coroutine.</summary>
	private IEnumerator MoskarReproductionsCountdown()
	{
		SecondsDelayWait wait = new SecondsDelayWait(reproductionCountdown);

		while(true)
		{
			while(wait.MoveNext()) yield return null;
			MoskarBoss firstMoskar = moskarReproductions.First();
			int remainingMoskars = moskarReproductions.Count;
			int moskarsToReproduce = 0;
			int difference = 0;
			float health = 0.0f;

			if(remainingMoskars < 2)
			{
				difference = 2 - remainingMoskars;
				health = 4.0f;

			} else if(remainingMoskars < 4)
			{
				difference = 4 - remainingMoskars;
				health = 3.0f;

			} else if(remainingMoskars < 8)
			{
				difference = 8 - remainingMoskars;
				health = 2.0f;

			} else if(remainingMoskars < 16)
			{
				difference = 1 - remainingMoskars;
				health = 1.0f;
			}

			if(difference > 0)
			{
				for(int i = 0; i < difference; i++)
				{
					MoskarBoss moskar = PoolManager.RequestPoolGameObject(moskarIndex, firstMoskar.transform.position, firstMoskar.transform.rotation) as MoskarBoss;

					if(moskar == null) yield break;

					SubscribeToMoskarEvents(moskar);
					moskar.state = 0;
					moskar.AddStates(IDs.STATE_IDLE);
					moskarReproductions.Add(moskar);
				}

				foreach(MoskarBoss moskar in moskarReproductions)
				{
					moskar.health.SetMaxHP(health, true);
				}
			}

			wait.ChangeDurationAndReset(reproductionCountdown);
		}
	}
#endregion
}
}