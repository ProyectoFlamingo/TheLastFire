using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
[Serializable]
public struct SoundEffectEmissionData
{
	public int sourceIndex; 					/// <summary>Source's Index.</summary>	
	public int soundIndex; 						/// <summary>Sound's Index.</summary>
	[Range(0.0f, 1.0f)] public float volume; 	/// <summary>Sound's Volume.</summary>

	/// <summary>SoundEffectEmissionData's Constructor.</summary>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_soundIndex">Sound's Index.</param>
	/// <param name="_volume">Sound's Volume.</param>
	public SoundEffectEmissionData(int _sourceIndex, int _soundIndex, float _volume = 1.0f)
	{
		sourceIndex = _sourceIndex;
		soundIndex = _soundIndex;
		volume = _volume;
	}

	/// <summary>Plays Sound Effect.</summary>
	public void Play()
	{
		AudioController.PlayOneShot(SourceType.SFX, sourceIndex, soundIndex, volume);
	}
}
}