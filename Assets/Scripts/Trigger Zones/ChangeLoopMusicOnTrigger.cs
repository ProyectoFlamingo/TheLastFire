using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class ChangeLoopMusicOnTrigger : MonoBehaviour
{
	[SerializeField] private string _tag; 				/// <summary>Tag that triggers the event.</summary>
	[SerializeField] private AudioLoopData _loopData; 	/// <summary>Loop's Sound.</summary>

	/// <summary>Gets tag property.</summary>
	public string tag { get { return _tag; } }

	/// <summary>Gets loopData property.</summary>
	public AudioLoopData loopData { get { return _loopData; } }

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		GameObject obj = col.gameObject;
	
		if(obj.CompareTag(tag))
		AudioController.Play(loopData.GetSourceType(), loopData.sourceIndex, loopData.soundReference, loopData.loop);
	}
}
}