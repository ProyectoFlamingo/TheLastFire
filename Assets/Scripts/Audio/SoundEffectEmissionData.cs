using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Voidless;

namespace Flamingo
{
[Serializable]
public struct SoundEffectEmissionData
{
	public int sourceIndex; 					/// <summary>Source's Index.</summary>
	public VAssetReference soundReference; 		/// <summary>Sound's Asset Reference.</summary>	
	[Range(0.0f, 1.0f)] public float volume; 	/// <summary>Sound's Volume.</summary>

	/// <summary>SoundEffectEmissionData's Constructor.</summary>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_soundReference">Sound's Asset Reference.</param>
	/// <param name="_volume">Sound's Volume.</param>
	public SoundEffectEmissionData(int _sourceIndex, VAssetReference _soundReference, float _volume = 1.0f) : this()
	{
		sourceIndex = _sourceIndex;
		soundReference = _soundReference;
		volume = _volume;
	}

	/// <summary>Plays Sound Effect.</summary>
	/// <returns>AudioClip played.</returns>
	public AudioClip Play()
	{
		return soundReference != null ? AudioController.PlayOneShot(SourceType.SFX, sourceIndex, soundReference, volume) : null;
	}
}
}