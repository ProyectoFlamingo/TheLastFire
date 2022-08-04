using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Voidless;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;

namespace Flamingo
{
[RequireComponent(typeof(Boundaries2DContainer))]
public class MoskarSceneController : Singleton<MoskarSceneController>
{
	[SerializeField] private MoskarBossAIController _moskarAIController; 	/// <summary>Moskar's AI Controller.</summary>
	[Space(5f)]
	[SerializeField] private float _fadeOutDuration; 						/// <summary>Fade-Out's Duration.</summary>
	[Space(5f)]
	[Header("Music's Attributes:")]
	[SerializeField] private FloatRange _waitBetweenPiece; 					/// <summary>Wait Duration Between each piece.</summary>
	[SerializeField] private VAssetReference[] _piecesReferences; 			/// <summary>Pieces' Asset-References.</summary>
	[SerializeField] private VAssetReference _flyLoopReference; 			/// <summary>Fly Loop's Asset-Reference.</summary>
	[SerializeField] private int _remainingMoskarsForLastPiece; 			/// <summary>Maximum required of remaining Moskars for the last piece's loop to be reproduced.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _moskarSFXVolume; 					/// <summary>Volume for Moskar's SFXs.</summary>
	[Space(5f)]
	[Header("Mandala's Attributes:")]
	[SerializeField] private float _exitMandalaWaitBeforeEndingScene; 		/// <summary>Wait from Exit-Mandala's spawning to end the scene.</summary>
	[SerializeField] private Mandala _enterMandala;							/// <summary>Enter Mandala's Reference.</summary>
	[SerializeField] private Mandala _exitMandala;							/// <summary>Exit Mandala's Reference.</summary>
	[SerializeField] private Vector3 _enterMandalaSpawnPosition; 			/// <summary>Enter Mandala's spawn Position.</summary>
	[SerializeField] private Vector3 _exitMandalaSpawnPosition; 			/// <summary>Exit Mandala's spawn Position.</summary>
	private Boundaries2DContainer _moskarBoundaries; 						/// <summary>Moskar's Boundaries.</summary>
	private HashSet<MoskarBoss> _moskarReproductions; 						/// <summary>Moskar's Reproductions.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 							/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 							/// <summary>Gizmos' Radius.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets moskarAIController property.</summary>
	public MoskarBossAIController moskarAIController { get { return _moskarAIController; } }

	/// <summary>Gets fadeOutDuration property.</summary>
	public float fadeOutDuration { get { return _fadeOutDuration; } }

	/// <summary>Gets and Sets moskarSFXVolume property.</summary>
	public float moskarSFXVolume
	{
		get { return _moskarSFXVolume; }
		set { _moskarSFXVolume = value; }
	}

	/// <summary>Gets exitMandalaWaitBeforeEndingScene property.</summary>
	public float exitMandalaWaitBeforeEndingScene { get { return _exitMandalaWaitBeforeEndingScene; } }

	/// <summary>Gets remainingMoskarsForLastPiece property.</summary>
	public int remainingMoskarsForLastPiece { get { return _remainingMoskarsForLastPiece; } }

	/// <summary>Gets piecesReferences property.</summary>
	public VAssetReference[] piecesReferences { get { return _piecesReferences; } }

	/// <summary>Gets flyLoopReference property.</summary>
	public VAssetReference flyLoopReference { get { return _flyLoopReference; } }

	/// <summary>Gets moskarBoundaries Component.</summary>
	public Boundaries2DContainer moskarBoundaries
	{ 
		get
		{
			if(_moskarBoundaries == null) _moskarBoundaries = GetComponent<Boundaries2DContainer>();
			return _moskarBoundaries;
		}
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
		enterMandala.gameObject.SetActive(false);
		exitMandala.gameObject.SetActive(false);
		moskarAIController.boundaries = moskarBoundaries;
		moskarAIController.onMoskarDeactivated += OnMoskarDeactivated;
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		Game.EnablePlayerControl(false);
		/*Game.FadeOutScreen(Color.black, fadeOutDuration,
		()=>
		{
			Game.EnablePlayerControl(true);
		});	*/

		ResourcesManager.onResourcesLoaded += OnResourcesLoaded;
		PoolManager.onPoolsCreated += OnPoolsCreated;
	}

	/// <summary>Callback invoked when MoskarSceneController's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		ResourcesManager.onResourcesLoaded -= OnResourcesLoaded;
		PoolManager.onPoolsCreated -= OnPoolsCreated;
		AudioController.ResetAllSourcesVolume();
	}

	/// <summary>Updates MoskarSceneController's instance at each frame.</summary>
	private void Update()
	{
		//...
	}

	/// <summary>Event invoked when all the resources are loaded.</summary>
	private void OnResourcesLoaded()
	{
		if(piecesReferences != null)
		{
			AudioClip piece = null;

			for(int i = 0; i < piecesReferences.Length; i++)
			{
				piece = ResourcesManager.GetAudioClip(piecesReferences[i], SourceType.Loop);
				AudioController.Play(SourceType.Loop, i, piece, true);
				AudioController.SetVolume(SourceType.Loop, i, 0.0f);
			}
		}

		AudioClip flyLoopClip = ResourcesManager.GetAudioClip(flyLoopReference, SourceType.Loop);
		AudioController.Play(SourceType.Scenario, 0, flyLoopClip, true);

		this.StartCoroutine(this.WaitSeconds(1.5f, ()=>{ Game.EnablePlayerControl(true); }));
	}

	/// <summary>Event invoked when all the pools are loaded.</summary>
	private void OnPoolsCreated()
	{
		
	}

	/// <summary>Event invoked when the projectile is deactivated.</summary>
	/// <param name="_enemy">Enemy that invoked the event.</param>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	public void OnMoskarDeactivated(Character _enemy, DeactivationCause _cause, Trigger2DInformation _info)
	{
		float moskarsDestroyed = moskarAIController.moskarsDestroyed;
		float totalMoskars = moskarAIController.totalMoskars;
		float t = moskarsDestroyed / Mathf.Max(totalMoskars - (float)remainingMoskarsForLastPiece, 1.0f);
		int pieces = piecesReferences.Length - 1;
		int index = (int)Mathf.Lerp(0, pieces, t);

/* Debugging Pieces Activation:
		StringBuilder builder = new StringBuilder();

		builder.Append("{ Moskars Destroyed: ");
		builder.Append(moskarsDestroyed.ToString());
		builder.Append(", Total Moskars: ");
		builder.Append(totalMoskars.ToString());
		builder.Append(", Activate until index: [ ");
		builder.Append(moskarsDestroyed.ToString());
		builder.Append(" / (");
		builder.Append(totalMoskars.ToString());
		builder.Append(" - ");
		builder.Append(remainingMoskarsForLastPiece.ToString());
		builder.Append(") = ");
		builder.Append(t.ToString());
		builder.Append(" => L(0, ");
		builder.Append(pieces.ToString());
		builder.Append(", ");
		builder.Append(t.ToString());
		builder.Append(") = ");
		builder.Append(index.ToString());
		builder.Append(" ] }");

		Debug.Log(builder.ToString());
*/

		/// As more Moskar reproductions faints, more loop channels get open:
		AudioController.SetVolume(SourceType.Loop, index, 1.0f);

		if(moskarsDestroyed >= totalMoskars)
		{
			AudioController.Stop(SourceType.Scenario, 0);
			Game.EnablePlayerControl(false);
			Game.mateo.Meditate(true);
			exitMandala.gameObject.SetActive(true);
			exitMandala.transform.position = exitMandalaSpawnPosition;
			exitMandala.enabled = true;

			this.StartCoroutine(this.WaitSeconds(exitMandalaWaitBeforeEndingScene, 
			()=>
			{
				Game.LoadScene(Game.data.overworldSceneName);
			}));
		}
	}
}
}