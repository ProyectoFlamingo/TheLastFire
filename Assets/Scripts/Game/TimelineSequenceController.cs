using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Voidless;

namespace Flamingo
{
[Serializable] public class StringTimelineAssetDictionary : StringKeyDictionary<TimelineAsset> { /*...*/ }

/// <summary>Event invoked when a Timeline-Sequence is over.</summary>
/// <param name="_name">Name of the sequence.</param>
/// <param name="_timelineAsset">TimelineAsset associated with the Timeline-Sequence.</param>
public delegate void OnTimelineSequenceFinished(string _name, TimelineAsset _timelineAsset);

[RequireComponent(typeof(PlayableDirector))]
public class TimelineSequenceController : Singleton<TimelineSequenceController>
{
	[SerializeField] private StringTimelineAssetDictionary _sequencesMap; 	/// <summary>Mapping of TimelineAssets.</summary>
	private PlayableDirector _playableDirector; 							/// <summary>PlayableDirector's Component.</summary>

	/// <summary>Gets sequencesMap property.</summary>
	public StringTimelineAssetDictionary sequencesMap { get { return _sequencesMap; } }

	/// <summary>Gets playableDirector Component.</summary>
	public PlayableDirector playableDirector
	{ 
		get
		{
			if(_playableDirector == null) _playableDirector = GetComponent<PlayableDirector>();
			return _playableDirector;
		}
	}

	/// <summary>Plays Timeline-Sequence.</summary>
	/// <param name="_name">Timeline-Sequence's name.</param>
	/// <param name="onTimelineSequenceFinished">Optional Callback invoked when the sequence is over.</param>
	public static void PlaySequence(string _name, Action<string, TimelineAsset> onTimelineSequenceFinished = null)
	{
		TimelineSequenceController controller = Instance;

		if(controller == null) return;

		if(controller.sequencesMap == null || !controller.sequencesMap.ContainsKey(_name))
		{
			Debug.LogError("[TimelineSequenceController] There is no entry with key " + _name + " registered on the sequences' mapping.");
			return;
		}

		controller.StartCoroutine(controller.PlaySequenceCoroutine(_name, onTimelineSequenceFinished));
	}

	/// <summary>Plays Timeline-Sequence.</summary>
	/// <param name="_name">Timeline-Sequence's name.</param>
	/// <param name="onTimelineSequenceFinished">Optional Callback invoked when the sequence is over.</param>
	private IEnumerator PlaySequenceCoroutine(string _name, Action<string, TimelineAsset> onTimelineSequenceFinished = null)
	{
		TimelineAsset sequence = sequencesMap[_name];

		playableDirector.Play(sequence);

		SecondsDelayWait wait = new SecondsDelayWait((float)sequence.duration);

		while(wait.MoveNext()) yield return null;

		playableDirector.Stop();
		if(onTimelineSequenceFinished != null) onTimelineSequenceFinished(_name, sequence);
	}
}
}