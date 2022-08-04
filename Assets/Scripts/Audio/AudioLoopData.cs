using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum AudioLoopType
{
	Loop = SourceType.Loop,
	Scenario = SourceType.Scenario
}

[Serializable]
public struct AudioLoopData
{
	public AudioLoopType loopType; 			/// <summary>Loop's Type.</summary>
	public int sourceIndex; 				/// <summary>Source's Index.</summary>
	public VAssetReference soundReference; 	/// <summary>Sound's Reference.</summary>
	public bool loop; 						/// <summary>Loop?.</summary>

	/// <summary>AudioLoopData's Constructor.</summary>
	/// <param name="_loopType">Loops's Type.</param>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_soundReference">Sound's Index.</param>
	/// <param name="_loop">Loop? true by default.</param>
	public AudioLoopData(AudioLoopType _loopType, int _sourceIndex, VAssetReference _soundReference, bool _loop = true)
	{
		loopType = _loopType;
		sourceIndex = _sourceIndex;
		soundReference = _soundReference;
		loop = _loop;
	}

	/// <summary>AudioLoopData's Constructor.</summary>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_soundReference">Sound's Index.</param>
	/// <param name="_loop">Loop? true by default.</param>
	public AudioLoopData(int _sourceIndex, VAssetReference _soundReference, bool _loop = true)
	{
		loopType = AudioLoopType.Loop;
		sourceIndex = _sourceIndex;
		soundReference = _soundReference;
		loop = _loop;
	}

	/// <returns>SourceType.</returns>
	public SourceType GetSourceType()
	{
		return (SourceType)loopType;
	}

	/// <summary>Plays Loop given this data's arguments.</summary>
	public AudioClip Play()
	{
		return soundReference != null ? AudioController.Play(GetSourceType(), sourceIndex, soundReference, loop) : null;
	}

	/// <summary>Stops Loop [using the Source data].</summary>
	/// <param name="onStopEnds">Optional callback invoked when the stopping reaches an end.</param>
	public void Stop(Action onStopEnds = null)
	{
		AudioController.Stop(GetSourceType(), sourceIndex, onStopEnds);
	}
}
}