using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Serializable]
public class SoundAndParticleEffectsPair
{
	[SerializeField] private ParticleEffectEmissionData _particleEffect; 	/// <summary>Particle Effect to emit.</summary>
	[Space(5f)]
	[SerializeField] private SoundEffectEmissionData _soundEffect; 			/// <summary>Sound Effect to emit.</summary>
	[Space(5f)]
	[SerializeField] private float _cooldown; 								/// <summary>Cooldown after emision.</summary>
	private MonoBehaviour _monoBehaviour; 									/// <summary>MonoBehaviour's reference.</summary>
	private Coroutine coroutine; 											/// <summary>Coroutine's reference.</summary>

	/// <summary>Gets and Sets particleEffect property.</summary>
	public ParticleEffectEmissionData particleEffect
	{
		get { return _particleEffect; }
		set { _particleEffect = value; }
	}

	/// <summary>Gets and Sets soundEffect property.</summary>
	public SoundEffectEmissionData soundEffect
	{
		get { return _soundEffect; }
		set { _soundEffect = value; }
	}

	/// <summary>Gets and Sets cooldown property.</summary>
	public float cooldown
	{
		get { return _cooldown; }
		set { _cooldown = value; }
	}

	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		set { _monoBehaviour = value; }
	}

	/// <summary>Reproduces Schedule's Routine.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour that will play the routine.</param>
	public void PlayScheduleRoutine(MonoBehaviour _monoBehaviour)
	{
		monoBehaviour = _monoBehaviour;
		monoBehaviour.StartCoroutine(ScheduleRoutine(), ref coroutine);
	}

	/// <summary>Stops Scheduled Routine.</summary>
	public void StopScheduleRoutine()
	{
		if(monoBehaviour != null)
		monoBehaviour.DispatchCoroutine(ref coroutine);
	}

	/// <summary>Schedule's Routine.</summary>
	public IEnumerator ScheduleRoutine()
	{
		if(cooldown > 0.0f)
		{
			SecondsDelayWait wait = new SecondsDelayWait(cooldown);
			while(wait.MoveNext()) yield return null;
		}

		particleEffect.EmitParticleEffects();
		soundEffect.Play();
	}
}
}