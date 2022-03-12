using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Flamingo
{
[Serializable]
public struct SoundEffectEmissionData
{
	public int sourceIndex; 					/// <summary>Source's Index.</summary>
	public AssetReference soundReference; 		/// <summary>Sound's Asset Reference.</summary>	
	public int soundIndex; 						/// <summary>Sound's Index.</summary>
	[Range(0.0f, 1.0f)] public float volume; 	/// <summary>Sound's Volume.</summary>

	/// <summary>SoundEffectEmissionData's Constructor.</summary>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_soundIndex">Sound's Index.</param>
	/// <param name="_volume">Sound's Volume.</param>
	public SoundEffectEmissionData(int _sourceIndex, int _soundIndex, float _volume = 1.0f) : this()
	{
		sourceIndex = _sourceIndex;
		soundIndex = _soundIndex;
		volume = _volume;
		soundReference = null;
	}

	/// <summary>SoundEffectEmissionData's Constructor.</summary>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_soundReference">Sound's Asset Reference.</param>
	/// <param name="_volume">Sound's Volume.</param>
	public SoundEffectEmissionData(int _sourceIndex, AssetReference _soundReference, float _volume = 1.0f) : this()
	{
		sourceIndex = _sourceIndex;
		soundReference = _soundReference;
		volume = _volume;
		soundIndex = -1;
	}

	/// <summary>Plays Sound Effect.</summary>
	public void Play()
	{
		if(sourceIndex < 0)
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, ResourcesManager.GetAudioClip(soundReference, SourceType.SFX), volume);
		else
			AudioController.PlayOneShot(SourceType.SFX, sourceIndex, soundIndex, volume);
	}
}
}