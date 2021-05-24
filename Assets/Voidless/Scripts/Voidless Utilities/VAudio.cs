using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/*
	Play VS PlayOneShot:
		- Play Stops the Audiosource, then plays the sound.
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

	/// \TODO Really do the coroutine...
	public static IEnumerator PlayFiniteStateClip(this AudioSource _audioSource, FiniteStateAudioClip _FSMClip, bool _loop = false, Action<FiniteStateAudioClip> onCLipEndeded = null)
	{
		while(true)
		{
			_FSMClip.time += Time.deltaTime;
			yield return null;
		}

		if(onCLipEndeded != null) onCLipEndeded(_FSMClip);
	}
}
}