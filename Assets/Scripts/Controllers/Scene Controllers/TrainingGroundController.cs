using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;

namespace Flamingo
{
public class TrainingGroundController : Singleton<TrainingGroundController>
{
	[SerializeField] private SoundEffectEmissionData _soundEffect; 		/// <summary>Sound-Effect's Reference.</summary>
	[SerializeField] private Mateo _mateo; 								/// <summary>Mateo's Reference.</summary>
	[SerializeField] private EventsHandler[] _targetImpactHandlers; 	/// <summary>Target's EventsHandlers' Components.</summary>
	[SerializeField] private EventsHandler[] _marbleImpactHandlers; 	/// <summary>Marble's EventsHandlers' Components.</summary>
	[SerializeField] private EventsHandler[] _petrolImpactHandlers; 	/// <summary>Petrol's EventsHandlers' Components.</summary>
	[Space(5f)]
	[Header("UI:")]
	[SerializeField] private Text _targetsScoreText; 					/// <summary>Targets' Score's Text.</summary>
	[SerializeField] private Text _marblesScoreText; 					/// <summary>Marbles' Score's Text.</summary>
	[SerializeField] private Text _petrolsScoreText; 					/// <summary>Petrols' Score's Text.</summary>
	[SerializeField] private int _targetsScore; 						/// <summary>Targets' Score.</summary>
	private int _marblesScore; 											/// <summary>Marbles' Score.</summary>
	[SerializeField] private int _petrolsScore; 						/// <summary>Petrols' Score.</summary>

	/// <summary>Gets and Sets mateo property.</summary>
	public Mateo mateo
	{
		get { return _mateo; }
		set { _mateo = value; }
	}

	/// <summary>Gets targetImpactHandlers property.</summary>
	public EventsHandler[] targetImpactHandlers { get { return _targetImpactHandlers; } }

	/// <summary>Gets marbleImpactHandlers property.</summary>
	public EventsHandler[] marbleImpactHandlers { get { return _marbleImpactHandlers; } }

	/// <summary>Gets petrolImpactHandlers property.</summary>
	public EventsHandler[] petrolImpactHandlers { get { return _petrolImpactHandlers; } }

	/// <summary>Gets and Sets targetsScore property.</summary>
	public int targetsScore
	{
		get { return _targetsScore; }
		private set
		{
			_targetsScore = value;
			if(targetsScoreText != null) targetsScoreText.text = _targetsScore.ToString();
		}
	}

	/// <summary>Gets and Sets soundEffect property.</summary>
	public SoundEffectEmissionData soundEffect
	{
		get { return _soundEffect; }
		set { _soundEffect = value; }
	}

	/// <summary>Gets and Sets marblesScore property.</summary>
	public int marblesScore
	{
		get { return _marblesScore; }
		private set
		{
			_marblesScore = value;
			if(marblesScoreText != null) marblesScoreText.text = _marblesScore.ToString();
		}
	}

	/// <summary>Gets and Sets petrolsScore property.</summary>
	public int petrolsScore
	{
		get { return _petrolsScore; }
		private set
		{
			_petrolsScore = value;
			if(petrolsScoreText != null) petrolsScoreText.text = _petrolsScore.ToString();
		}
	}

	/// <summary>Gets targetsScoreText property.</summary>
	public Text targetsScoreText { get { return _targetsScoreText; } }

	/// <summary>Gets marblesScoreText property.</summary>
	public Text marblesScoreText { get { return _marblesScoreText; } }

	/// <summary>Gets petrolsScoreText property.</summary>
	public Text petrolsScoreText { get { return _petrolsScoreText; } }

	/// <summary>TrainingGroundController's instance initialization.</summary>
	protected override void OnAwake()
	{
		//mateo.PerformPose(true);
		//mateo.StareTowards(StareTarget.Player);

		if(targetImpactHandlers != null) foreach(EventsHandler handler in targetImpactHandlers)
		{
			if(handler == null) continue;
			handler.onDeactivated += OnTargetImpactEvent;
		}

		if(marbleImpactHandlers != null) foreach(EventsHandler handler in marbleImpactHandlers)
		{
			if(handler == null) continue;
			handler.onDeactivated += OnMarbleImpactEvent;
		}

		if(petrolImpactHandlers != null) foreach(EventsHandler handler in petrolImpactHandlers)
		{
			if(handler == null) continue;
			handler.onDeactivated += OnPetrolImpactEvent;
		}

		_targetsScore = 32;
		_petrolsScore = 35;
	}

	/// <summary>Event invoked when an impact is received.</summary>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	private void OnTargetImpactEvent(DeactivationCause _cause, Trigger2DInformation _info)
	{
		targetsScore--;
		Cheer();
	}

	/// <summary>Event invoked when an impact is received.</summary>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	private void OnMarbleImpactEvent(DeactivationCause _cause, Trigger2DInformation _info)
	{
		marblesScore--;
	}

	/// <summary>Event invoked when an impact is received.</summary>
	/// <param name="_cause">Cause of the deactivation.</param>
	/// <param name="_info">Additional Trigger2D's information.</param>
	private void OnPetrolImpactEvent(DeactivationCause _cause, Trigger2DInformation _info)
	{
		petrolsScore--;
		Cheer2();
	}

	private void Cheer()
	{
		if(targetsScore == 0) 
		{
			/*int index = soundEffectIndex;
			AudioController.PlayOneShot(SourceType.Scenario, 0, index);*/
			soundEffect.Play();
		}
	}

	private void Cheer2()
	{
		if(petrolsScore == 0) 
		{
			/*int index = soundEffectIndex;
			AudioController.PlayOneShot(SourceType.Scenario, 0, index);*/
			soundEffect.Play();
		}
		
	}
}
}