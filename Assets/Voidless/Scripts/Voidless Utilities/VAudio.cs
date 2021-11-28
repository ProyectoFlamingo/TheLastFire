using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/*
	Play VS PlayOneShot:
		- Play Stops the AudioSource, then plays the sound.
		- PlayOneShot stacks sounds, but previous stacked sounds are not stopped.
*/
public static class VAudio
{
	/// <summary>Stops AudioSource, then assigns and plays AudioClip.</summary>
	/// <param name="_audioSource">AudioSource to play sound.</param>
	/// <param name="_aucioClip">AudioClip to play.</param>
	/// <param name="_loop">Loop AudioClip? false as default.</param>
	public static void PlaySound(this AudioSource _audioSource, AudioClip _audioClip, bool _loop = false)
	{
		_audioSource.Stop();
		_audioSource.clip = _audioClip;
		_audioSource.Play();
		_audioSource.loop = _loop;
	}

	/// <summary>Stacks and plays AudioClip.</summary>
	/// <param name="_audioSource">AudioSource to play sound.</param>
	/// <param name="_aucioClip">AudioClip to play.</param>
	/// <param name="_volumeScale">Normalized Volume's Scale [1.0f by default].</param>
	public static void PlaySoundOneShot(this AudioSource _audioSource, AudioClip _audioClip, float _volumeScale = 1.0f)
	{
		_audioSource.PlayOneShot(_audioClip, _volumeScale);
	}

	/// <summary>Plays sound and waits until it finishes.</summary>
	/// <param name="_audioSource">AudioSource that will play the sound.</param>
	/// <param name="_audioClip">Clip to play.</param>
	/// <param name="_volumeScale">Normalized Volume's Scale [1.0f by default].</param>
	public static IEnumerator PlaySoundOneShotAndWait(this AudioSource _audioSource, AudioClip _audioClip, float _volumeScale = 1.0f, Action onWaitEnds = null)
	{
		_audioSource.PlayOneShot(_audioClip, _volumeScale);
		SecondsDelayWait wait = new SecondsDelayWait(_audioClip.length);
		while(wait.MoveNext()) yield return null;
		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Plays FiniteStateAudioClip's Clip.</summary>
	/// <param name="_mono">MonoBehaviour's reference for the Coroutine.</param>
	/// <param name="_source">AudioSource's that will be played reference.</param>
	/// <param name="_FSMClip">FiniteStateAudioClip's reference.</param>
	/// <param name="_coroutine">Coroutine's reference.</param>
	/// <param name="_loop">Loop the AudioClip? True by default.</param>
	/// <param name="_restartState">Restart State? False by default.</param>
	/// <param name="onClipEnded">Optional Callback invoked when the FSM AudioClip ends [reaching its final state].</param>
	public static void PlayFSMAudioClip(this MonoBehaviour _mono, AudioSource _source, FiniteStateAudioClip _FSMClip, ref Coroutine _coroutine, bool _loop = false, bool _resetState = false, Action<FiniteStateAudioClip> onCLipEndeded = null)
	{
		if(_mono != null && _source != null)
		_mono.StartCoroutine(_source.PlayFiniteStateClip(_FSMClip, _loop, _resetState, onCLipEndeded));
	}

	/// <summary>Stops FiniteStateAudioClip from playing on given AudioSource.</summary>
	/// <param name="_mono">MonoBehaviour's reference for the Coroutine.</param>
	/// <param name="_source">AudioSource's that will be played reference.</param>
	/// <param name="_coroutine">Coroutine's reference.</param>
	public static void StopFSMAudioClip(this MonoBehaviour _mono, AudioSource _source, ref Coroutine _coroutine)
	{
		if(_mono == null && _source == null) return;

		_source.Stop();
		_mono.DispatchCoroutine(ref _coroutine);
	}

	/// <summary>Gets Samples from AudioClip.</summary>
	/// <param name="_clip">AudioClip's Reference.</param>
	/// <param name="_offsetSamples">Clip's starting point [0 by default].</param>
	/// <returns>Samples from AudioClip.</returns>
	public static float[] GetAudioClipSamples(this AudioClip _clip, int _offsetSamples = 0)
	{
		if(_clip == null) return null;

		float[] samples = new float[_clip.samples * _clip.channels];
		
		_clip.GetData(samples, _offsetSamples);

		return samples;
	}

	/// \TODO Still unconvinced...
	/*public void NotifyAt(this AudioSource _source, float _time, Action onTimeReached = null)
	{
		if(_source.clip == null) return;

	}

	public IEnumerator*/

	/// <summary>Plays FiniteStateAudioClip's Clip.</summary>
	/// <param name="_mono">MonoBehaviour's reference for the Coroutine.</param>
	/// <param name="_source">AudioSource's that will be played reference.</param>
	/// <param name="_FSMClip">FiniteStateAudioClip's reference.</param>
	/// <param name="_coroutine">Coroutine's reference.</param>
	/// <param name="_loop">Loop the AudioClip? True by default.</param>
	/// <param name="_restartState">Restart State? False by default.</param>
	/// <param name="onClipEnded">Optional Callback invoked when the FSM AudioClip ends [reaching its final state].</param>
	public static IEnumerator PlayFiniteStateClip(this AudioSource _audioSource, FiniteStateAudioClip _FSMClip, bool _loop = false, bool _resetState = false, Action<FiniteStateAudioClip> onCLipEndeded = null)
	{
		if(_FSMClip == null || _FSMClip.clip == null) yield break;

		if(_resetState) _FSMClip.ResetState();

		float duration = _FSMClip.clip.length;

		_FSMClip.SetStateToCurrentTime();

		_audioSource.Stop();
		_audioSource.clip = _FSMClip.clip;
		_audioSource.time = _FSMClip.time;
		_audioSource.Play();
		_audioSource.loop = _loop;

		while(true)
		{ /// Just change FSM Clip's time value each frame, that's pretty much it...
			_FSMClip.time += Time.deltaTime;

			if(_FSMClip.time >= duration)
			{
				if(!_audioSource.loop) break;
				_FSMClip.time = 0.0f;
			}

			yield return null;
		}

		if(onCLipEndeded != null) onCLipEndeded(_FSMClip);
	}
}
}