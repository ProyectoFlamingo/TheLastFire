using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Voidless;

namespace Flamingo
{
public class AudioLoopSourceMixerData
{
	[SerializeField] private AudioClip _clip; 			/// <summary>AudioClip.</summary>
	[SerializeField] private AudioSource _source; 		/// <summary>AudioSource.</summary>
	[SerializeField] private AudioMixer _mixer; 		/// <summary>AudioMixer.</summary>
	[SerializeField] private AudioLoopData _loopData; 	/// <summary>AudioLoop's Data.</summary>

	/// <summary>Gets and Sets clip property.</summary>
	public AudioClip clip
	{
		get { return _clip; }
		set { _clip = value; }
	}

	/// <summary>Gets and Sets source property.</summary>
	public AudioSource source
	{
		get { return _source; }
		set { _source = value; }
	}

	/// <summary>Gets and Sets mixer property.</summary>
	public AudioMixer mixer
	{
		get { return _mixer; }
		set { _mixer = value; }
	}

	/// <summary>Gets and Sets loopData property.</summary>
	public AudioLoopData loopData
	{
		get { return _loopData; }
		set { _loopData = value; }
	}

	/// <summary>AudioLoopSourceMixerData's Constructor.</summary>
	/// <param name="_clip">AudioClip.</param>
	/// <param name="_source">AudioSource.</param>
	/// <param name="_mixer">AudioMixer.</param>
	/// <param name="_loopData">AudioLoop's Data.</param>
	public AudioLoopSourceMixerData(AudioClip _clip, AudioSource _source, AudioMixer _mixer, AudioLoopData _loopData)
	{
		clip = _clip;
		source = _source;
		mixer = _mixer;
		loopData = _loopData;
	}
}
}