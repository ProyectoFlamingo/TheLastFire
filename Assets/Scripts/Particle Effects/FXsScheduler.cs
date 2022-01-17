using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Serializable]
public class FXsScheduler
{
	[SerializeField] private SoundAndParticleEffectsPair[] _FXsPairs; 	/// <summary>Pairs of Sound & Particle Effects' Emission Data.</summary>
	private MonoBehaviour _monoBehaviour; 								/// <summary>MonoBehaviour's reference.</summary>
	private Coroutine coroutine; 										/// <summary>Coroutine's reference.</summary>

	/// <summary>Gets and Sets FXsPairs property.</summary>
	public SoundAndParticleEffectsPair[] FXsPairs
	{
		get { return _FXsPairs; }
		set { _FXsPairs = value; }
	}

	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		set { _monoBehaviour = value; }
	}

	/// <summary>Draws Gizmos.</summary>
	public void DrawGizmos()
	{
		if(FXsPairs == null) return;

		foreach(SoundAndParticleEffectsPair pair in FXsPairs)
		{
			pair.particleEffect.DrawGizmos();
		}
	}

	/// <summary>Reproduces Schedule's Routine.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour that will play the routine.</param>
	/// <param name="onScheduleEnds">Callback invoked when the Schedule's coroutine ends.</param>
	public void PlayScheduleRoutine(MonoBehaviour _monoBehaviour, Action onScheduleEnds = null)
	{
		if(FXsPairs == null) return;

		monoBehaviour = _monoBehaviour;
		monoBehaviour.StartCoroutine(ScheduleRoutine(onScheduleEnds), ref coroutine);
	}

	/// <summary>Stops Scheduled Routine.</summary>
	public void StopScheduleRoutine()
	{
		if(monoBehaviour != null)
		monoBehaviour.DispatchCoroutine(ref coroutine);
	}

	/// <summary>Schedule's Routine.</summary>
	/// <param name="onScheduleEnds">Callback invoked when the Schedule's coroutine ends.</param>
	private IEnumerator ScheduleRoutine(Action onScheduleEnds = null)
	{
		IEnumerator routine = null;

		foreach(SoundAndParticleEffectsPair pair in FXsPairs)
		{
			routine = pair.ScheduleRoutine();
			while(routine.MoveNext()) yield return null;
		}

		if(onScheduleEnds != null) onScheduleEnds();
	}
}
}