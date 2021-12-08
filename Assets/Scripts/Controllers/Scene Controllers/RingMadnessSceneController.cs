using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;

namespace Flamingo
{
public class RingMadnessSceneController : Singleton<RingMadnessSceneController>
{
	[SerializeField] private int _soundEffectIndex; 	/// <summary>Particle Effect's Index on the Game's Data.</summary>
	[SerializeField] private int _particleEffectIndex; 	/// <summary>Particle Effect's Index on the Game's Data.</summary>
	[SerializeField] private Ring[] _rings; 			/// <summary>Ring on the Scene.</summary>
	[Space(5f)]
	[Header("UI:")]
	[SerializeField] private Text _ringsScoreText; 		/// <summary>Ring Score's Text.</summary>
	private int _ringsScore; 							/// <summary>Rings' Score.</summary>

	/// <summary>Gets rings property.</summary>
	public Ring[] rings { get { return _rings; } }

	/// <summary>Gets ringsScoreText property.</summary>
	public Text ringsScoreText { get { return _ringsScoreText; } }

	/// <summary>Gets and Sets ringsScore property.</summary>
	public int ringsScore
	{
		get { return _ringsScore; }
		private set { _ringsScore = value; }
	}

	/// <summary>Gets and Sets soundEffectIndex property.</summary>
	public int soundEffectIndex
	{
		get { return _soundEffectIndex; }
		set { _soundEffectIndex = value; }
	}

		/// <summary>Gets and Sets particleEffectIndex property.</summary>
	public int particleEffectIndex
	{
		get { return _particleEffectIndex; }
		set { _particleEffectIndex = value; }
	}

	/// <summary>RingMadnessSceneController's instance initialization.</summary>
	private void Awake()
	{
		if(rings != null) foreach(Ring ring in rings)
		{
			ring.onRingPassed += OnRingPassed;
		}
	}

	/// <summary>Callback invoked when a Collider passes a ring.</summary>
	/// <param name="_collider">Collider that passed the ring.</param>
	public void OnRingPassed(Collider2D _collider)
	{
		ringsScore++;
		int index = particleEffectIndex;
		Vector3 point = _collider.transform.position;
		Vector3 direction =  _collider.transform.position;
		AudioController.PlayOneShot(SourceType.Scenario, 0, 40);
		ParticleEffect particleEffect = PoolManager.RequestParticleEffect(index, point, VQuaternion.RightLookRotation(direction));
		if(ringsScore >= rings.Length) OnRingScoreCompleted();
		if(ringsScoreText != null) ringsScoreText.text = ringsScore.ToString();
	}

	/// <summary>Callback internally invoked when the Ring Score reaches its maximum limit.</summary>
	private void OnRingScoreCompleted()
	{
		int index = soundEffectIndex;
		AudioController.PlayOneShot(SourceType.Scenario, 0, index);
		/// Do what it must be made in order for the players (a.k.a. Rodo's friends) know they are champs.
	}
}
}