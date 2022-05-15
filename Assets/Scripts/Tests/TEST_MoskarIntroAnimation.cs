using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TEST_MoskarIntroAnimation : MonoBehaviour
{
	[SerializeField] private Transform parent;
	[SerializeField] private Animator animator;
	private PlayableDirector director;

	/// <summary>TEST_MoskarIntroAnimation's instance initialization.</summary>
	private void Awake()
	{
		director = GetComponent<PlayableDirector>();
	}

	/// <summary>TEST_MoskarIntroAnimation's starting actions before 1st Update frame.</summary>
	private IEnumerator Start ()
	{
		director.Play();
		
		float duration = (float)director.duration;
		WaitForSeconds wait = new WaitForSeconds(duration);

		yield return wait;

		Debug.Log("[TEST_MoskarIntroAnimation] Sequence is finished...");

		animator.transform.SetParent(null);
		parent.position = animator.transform.position;
		animator.transform.SetParent(parent);
		animator.transform.localPosition = Vector3.zero;
		animator.transform.localRotation = Quaternion.identity;
	}
}