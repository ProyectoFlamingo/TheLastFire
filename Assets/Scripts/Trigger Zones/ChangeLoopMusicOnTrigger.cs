using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class ChangeLoopMusicOnTrigger : MonoBehaviour
{
	[SerializeField] private string _tag; 					/// <summary>Tag that triggers the event.</summary>
	[SerializeField] private int _loopSource; 				/// <summary>Loop Music's AudioSource's Index.</summary>
	[SerializeField] private int _loopIndex; 				/// <summary>Loop Music's Index.</summary>

	/// <summary>Gets tag property.</summary>
	public string tag { get { return _tag; } }

	/// <summary>Gets loopSource property.</summary>
	public int loopSource { get { return _loopSource; } }

	/// <summary>Gets loopIndex property.</summary>
	public int loopIndex { get { return _loopIndex; } }

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		GameObject obj = col.gameObject;
	
		if(obj.CompareTag(tag))
		AudioController.Play(SourceType.Loop, 0, loopIndex, true);
	}
}
}