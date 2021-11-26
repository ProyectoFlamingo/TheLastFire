using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Voidless;

namespace Flamingo
{
public enum SourceType
{
	Default,
	Loop,
	Scenario,
	SFX
}

[RequireComponent(typeof(AudioSource))]
public class AudioController : Singleton<AudioController>
{
	[Header("Audio's Settings:")]
	[SerializeField] private string _exposedVolumeParameterName; 	/// <summary>Exposed Volume Parameter's Name.</summary>
	[SerializeField] private float _fadeOutDuration; 				/// <summary>Fade-Out's Duration.</summary>
	[SerializeField] private float _fadeInDuration; 				/// <summary>Fade-In's Duration.</summary>
	[Space(5f)]
	[Header("Audio Sources:")]
	[SerializeField] private AudioSource[] _loopSources; 			/// <summary>Loops' AudioSources.</summary>
	[SerializeField] private AudioSource[] _scenarioSources; 		/// <summary>Scenario's AudioSources.</summary>
	[SerializeField] private AudioSource[] _soundEffectSources; 	/// <summary>Mateo's AudioSources.</summary>
	private AudioSource _audioSource; 								/// <summary>AudioSource's Component.</summary>
	private Coroutine volumeFading; 								/// <summary>Volume Fading's Coroutine Reference.</summary>
	private Coroutine[] loopFSMCoroutines; 							/// <summary>Coroutines' references for the Loops' FSM AudioClips.</summary>
	private Coroutine[] loopVolumeFadings; 							/// <summary>Coroutines' references for the Loops' volume's fading.</summary>
	private StringBuilder _builder; 								/// <summary>String Builder used for name concatenations.</summary>

	/// <summary>Gets exposedVolumeParameterName property.</summary>
	public string exposedVolumeParameterName { get { return _exposedVolumeParameterName; } }

	/// <summary>Gets fadeOutDuration property.</summary>
	public float fadeOutDuration { get { return _fadeOutDuration; } }

	/// <summary>Gets fadeInDuration property.</summary>
	public float fadeInDuration { get { return _fadeInDuration; } }

	/// <summary>Gets loopSources property.</summary>
	public static AudioSource[] loopSources { get { return Instance._loopSources; } }

	/// <summary>Gets scenarioSources property.</summary>
	public static AudioSource[] scenarioSources { get { return Instance._scenarioSources; } }

	/// <summary>Gets soundEffectSources property.</summary>
	public static AudioSource[] soundEffectSources { get { return Instance._soundEffectSources; } }

	/// <summary>Gets audioSource Component.</summary>
	public AudioSource audioSource
	{ 
		get
		{
			if(_audioSource == null) _audioSource = GetComponent<AudioSource>();
			return _audioSource;
		}
	}

	/// <summary>Gets and Sets builder property.</summary>
	public StringBuilder builder
	{
		get { return _builder; }
		private set { _builder = value; }
	}

	/// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
   	protected override void OnAwake()
	{
		if(loopSources == null) return;

		int length = loopSources.Length;

		loopFSMCoroutines = new Coroutine[length];
		loopVolumeFadings = new Coroutine[length];

		builder = new StringBuilder();
	}

	/// <summary>Gets AudioSource according to the provided type, on the located index.</summary>
	/// <param name="_type">AudioSource's Type [SourceType.Loop by default].</param>
	/// <param name="_index">Optional index on the set of AudioSources [0 by default].</param>
	/// <returns>AudioSource.</returns>
	private static AudioSource GetAudioSource(SourceType _type = SourceType.Default, int _index = 0)
	{
		switch(_type)
		{
			case SourceType.Default: 	return Instance.audioSource;
			case SourceType.Loop: 		return GetLoopSource(_index);
			case SourceType.Scenario: 	return GetScenarioSource(_index);
			case SourceType.SFX: 		return GetSoundEffectSource(_index);
			default: 					return null;
		}
	}

	/// <summary>Gets Loop's AudioSource on the given index.</summary>
	/// <param name="index">Index [0 by default].</param>
	/// <returns>Loop's AudioSource.</returns>
	public static AudioSource GetLoopSource(int index = 0)
	{
		AudioSource[] sources = Instance._loopSources;
		return sources != null ? sources[Mathf.Clamp(index, 0, sources.Length - 1)] : null;
	}

	/// <summary>Gets Scenario's AudioSource on the given index.</summary>
	/// <param name="index">Index [0 by default].</param>
	/// <returns>Scenario's AudioSource.</returns>
	public static AudioSource GetScenarioSource(int index = 0)
	{
		AudioSource[] sources = Instance._scenarioSources;
		return sources != null ? sources[Mathf.Clamp(index, 0, sources.Length - 1)] : null;
	}

	/// <summary>Gets Mateo's AudioSource on the given index.</summary>
	/// <param name="index">Index [0 by default].</param>
	/// <returns>Mateo's AudioSource.</returns>
	public static AudioSource GetSoundEffectSource(int index = 0)
	{
		AudioSource[] sources = Instance._soundEffectSources;
		return sources != null ? sources[Mathf.Clamp(index, 0, sources.Length - 1)] : null;
	}

	/// <summary>Stops AudioSource, then assigns and plays AudioClip.</summary>
	/// <param name="_audioSource">AudioSource to play sound.</param>
	/// <param name="_aucioClip">AudioClip to play.</param>
	/// <param name="_loop">Loop AudioClip? false as default.</param>
	/// <returns>Playing AudioClip.</returns>
	public static AudioClip Play(SourceType _type, int _sourceIndex, int _index, bool _loop = false)
	{
		AudioSource source = GetAudioSource(_type, _sourceIndex);
		AudioClip clip = Game.data.loops[_index];
		AudioMixer mixer = source.outputAudioMixerGroup.audioMixer;

		/// I Still don't know if this is a correct answer...
		if(source.clip != null && source.clip == clip) return clip;

		if(mixer != null && (source.clip != null && source.clip != clip))
		{ /// If there is an AudioMixer and there is a current AudioClip being played on the selected source that is not this Clip, fade the prior one before playing the new one.

			string parameterName = Instance.GetExposedParameterName(_type, _sourceIndex);

			/// Fade-Out last piece -> Set new piece -> Fade-In new piece.
			Instance.StartCoroutine(mixer.FadeVolume(parameterName, Instance.fadeOutDuration, 0.0f, 
			()=>
			{
				source.PlaySound(clip, _loop);
				Instance.StartCoroutine(mixer.FadeVolume(parameterName, Instance.fadeOutDuration, 1.0f, 
				()=>
				{
					Instance.DispatchCoroutine(ref Instance.volumeFading);
				}));

			}), ref Instance.volumeFading);
		}
		else source.PlaySound(clip, _loop);

		return clip;
	}

	/// <summary>Stops default AudioSource, then assigns and plays AudioClip.</summary>
	/// <param name="_index">Index of AudioClip to play.</param>
	/// <param name="_loop">Loop AudioClip? false as default.</param>
	/// <returns>Playing AudioClip.</returns>
	public static AudioClip Play(int _index, bool _loop = false)
	{
		return Play(SourceType.Default, 0, _index, _loop);
	}

	/// <summary>Plays FiniteStateAudioClip's Loop on the selected AudioSource.</summary>
	/// <param name="_sourceIndex">Index of the AudioSource that will play this FSM's AudioClip.</param>
	/// <param name="_index">FSM's AudioClip's Index.</param>
	/// <param name="_loop">Loop the FSM's AudioClip? True by default.</param>
	public static AudioClip PlayFSMLoop(int _sourceIndex, int _index, bool _loop = true)
	{
		AudioSource source = loopSources[_sourceIndex];
		FiniteStateAudioClip FSMClip = Game.data.FSMLoops[_index];
		AudioClip clip = FSMClip.clip;
		AudioMixer mixer = source.outputAudioMixerGroup.audioMixer;

#if UNITY_EDITOR
		VDebug.Log(
			LogType.Log,
			"PlayFSM()'s Report: ",
			"\n",
			"Source's Index: \n",
			_sourceIndex.ToString(),
			"\n",
			"FSM's AudioClip: ",
			FSMClip.ToString(),
			"\n",
			"Source's current AudioClip: ",
			source.clip != null ? source.clip.name : "NONE"
		);
#endif

		if(mixer != null && (source.clip != null && source.clip != clip))
		{ /// If there is an AudioMixer and there is a current AudioClip being played on the selected source that is not this Clip, fade the prior one before playing the new one.

			float mixerVolume = 0.0f;
			string parameterName = Instance.GetExposedParameterName(SourceType.Loop, _sourceIndex);

			mixer.GetFloat(parameterName, out mixerVolume);

			/// Fade-Out last piece -> Set new piece -> Fade-In new piece.
			/*if(mixerVolume > 0.0f) */Instance.StartCoroutine(mixer.FadeVolume(parameterName, Instance.fadeOutDuration, 0.0f, 
			()=>
			{
				Instance.PlayFSMAudioClip(source, FSMClip, ref Instance.loopFSMCoroutines[_sourceIndex], _loop, false, null);
				Instance.StartCoroutine(mixer.FadeVolume(parameterName, Instance.fadeOutDuration, 1.0f, 
				()=>
				{
					Instance.DispatchCoroutine(ref Instance.loopVolumeFadings[_sourceIndex]);
				}));

			}), ref Instance.loopVolumeFadings[_sourceIndex]);
			/*else
			{
				mixer.SetFloat(parameterName, 1.0f);
				Instance.PlayFSMAudioClip(source, FSMClip, ref Instance.loopFSMCoroutines[_sourceIndex], _loop, false, null);
			}*/
		}
		else Instance.PlayFSMAudioClip(source, FSMClip, ref Instance.loopFSMCoroutines[_sourceIndex], _loop, false, null);

		return clip;
	}

	/// <summary>Stops AudioSource, Fades-Out if there is an AudioMixer.</summary>
	/// <param name="_source">AudioSource to Stop.</param>
	/// <param name="onStopEnds">Optional callback invoked when the stop process reaches itrs end [null by default].</param>
	public static void Stop(SourceType _type, int _sourceIndex, Action onStopEnds = null)
	{
		AudioSource source = GetAudioSource(_type, _sourceIndex);

		if(source.clip == null || !source.isPlaying) return;

		AudioMixer mixer = source.outputAudioMixerGroup.audioMixer;
		//lastLoopIndex = -1;

		if(mixer != null)
		{
			string parameterName = Instance.GetExposedParameterName(_type, _sourceIndex);

			Instance.StartCoroutine(mixer.FadeVolume(parameterName, Instance.fadeOutDuration, 0.0f,
			()=>
			{
				source.Stop();
				//source.clip = null;
				mixer.SetVolume(parameterName,  1.0f);
				source.time = 0.0f;
				if(onStopEnds != null) onStopEnds();

			}), ref Instance.loopVolumeFadings[_sourceIndex]);
		}
		else
		{
			source.Stop();
			source.clip = null;
			//mixer.SetVolume(parameterName,  1.0f);
			source.time = 0.0f;
			if(onStopEnds != null) onStopEnds();
		}
	}

	/// <summary>Stops FSM's AudioClip Loop on the given AudioSource [if it is playing].</summary>
	/// <param name="_sourceIndex">Index of the AudioSource.</param>
	/// <param name="onStopEnds">Optional callback invoked after the stop ends [null by default].</param>
	public static void StopFSMLoop(int _sourceIndex, Action onStopEnds = null)
	{
		AudioSource source = loopSources[_sourceIndex];

		if(source.clip == null || !source.isPlaying) return;

		AudioMixer mixer = source.outputAudioMixerGroup.audioMixer;
		Instance.DispatchCoroutine(ref Instance.loopFSMCoroutines[_sourceIndex]);

		if(mixer != null)
		{
			string parameterName = Instance.GetExposedParameterName(SourceType.Loop, _sourceIndex);

			Instance.StartCoroutine(mixer.FadeVolume(parameterName, Instance.fadeOutDuration, 0.0f,
			()=>
			{
				source.clip = null;
				//Instance.StopFSMAudioClip(source, ref Instance.loopFSMCoroutines[_sourceIndex]);
				source.Stop();
				mixer.SetVolume(parameterName,  1.0f);
				source.time = 0.0f;
				if(onStopEnds != null) onStopEnds();
			}));
		}
		else
		{
			source.clip = null;
			//Instance.StopFSMAudioClip(source, ref Instance.loopFSMCoroutines[_sourceIndex]);
			source.Stop();
			//mixer.SetVolume(Instance.GetExposedParameterName(_type, _sourceIndex),  1.0f);
			source.time = 0.0f;
			if(onStopEnds != null) onStopEnds();
		}
	}

	/// <summary>Sets the volume of given AudioMixer located on given SourceIndex.</summary>
	/// <param name="_type">Source's Type.</param>
	/// <param name="_sourceIndex">Source's Index.</param>
	/// <param name="_volume">New Volume [1.0f by default].</param>
	public static void SetVolume(SourceType _type, int _sourceIndex, float _volume = 1.0f)
	{
		AudioSource source = GetAudioSource(_type, _sourceIndex);

		if(source == null) return;
		//if(source.clip == null || !source.isPlaying) return;

		AudioMixer mixer = source.outputAudioMixerGroup.audioMixer;

		if(mixer == null) return;
		string parameterName = Instance.GetExposedParameterName(_type, _sourceIndex);
		float x = 0.0f;
		mixer.GetFloat(parameterName, out x);
		mixer.SetVolume(parameterName, _volume);
	}

	/// <summary>Sets pitch foall AudioSources.</summary>
	/// <param name="_pitch">New Pitch.</param>
	public static void SetPitch(float _pitch)
	{
		Instance.audioSource.pitch = _pitch;

		if(loopSources != null) foreach(AudioSource source in loopSources)
		{
			source.pitch = _pitch;
		}

		if(scenarioSources != null) foreach(AudioSource source in scenarioSources)
		{
			source.pitch = _pitch;
		}

		if(soundEffectSources != null) foreach(AudioSource source in soundEffectSources)
		{
			source.pitch = _pitch;
		}
	}

	/// <summary>Stacks and plays AudioClip on the given AudioSource.</summary>
	/// <param name="_source">Source to use.</param>
	/// <param name="_indeex">AudioClip's index on the Game's Data to play.</param>
	/// <param name="_volumeScale">Normalized Volume's Scale [1.0f by default].</param>
	/// <returns>Playing AudioClip.</returns>
	public static AudioClip PlayOneShot(SourceType _type, int _sourceIndex, int _index, float _volumeScale = 1.0f)
	{
		AudioClip clip = Game.data.soundEffects[_index];
		GetAudioSource(_type, _sourceIndex).PlayOneShot(clip, _volumeScale);

		return clip;
	}

	/// <summary>Stacks and plays AudioClip on the default AudioSource.</summary>
	/// <param name="_indeex">AudioClip's index on the Game's Data to play.</param>
	/// <param name="_volumeScale">Normalized Volume's Scale [1.0f by default].</param>
	/// <returns>Playing AudioClip.</returns>
	public static AudioClip PlayOneShot(int _index, float _volumeScale = 1.0f)
	{
		return PlayOneShot(SourceType.Default, 0, _index, _volumeScale);
	}

	/// <summary>Gets proper exposed parameter name given the sourcetype and source index.</summary>
	/// <param name="_type">Source's Type [Default by default].</param>
	/// <param name="_sourceIndex">Source's Index [0 by default].</param>
	/// <returns>Proper Exposed Parameter Name for the Volume.</returns>
	public string GetExposedParameterName(SourceType _type = SourceType.Default, int _sourceIndex = 0)
	{
		builder.Clear();

		builder.Append(exposedVolumeParameterName);

		if(_type != SourceType.Default)
		{
			builder.Append("_");
			builder.Append(_type.ToString());
			builder.Append("_");
			builder.Append(_sourceIndex.ToString());
		}

		return builder.ToString();
	}
}
}

/*

	Create timestamps, each timeStamp defines a state and the beginning of each state

	if(currentTime > timeStamp[i] && currentTime timeStamp[i + 1]) state = i;

	OnnRebeginState() {
		
		ContinueFromState(state);

	}

	//Play Audio on Editor:
	//https://answers.unity.com/questions/36388/how-to-play-audioclip-in-edit-mode.html#:~:text=The%20audio%20preview%20mode%20must,and%20your%20audio%20should%20work.

*/