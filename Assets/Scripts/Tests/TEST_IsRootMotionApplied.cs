using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_IsRootMotionApplied : MonoBehaviour
{
	Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	void OnAnimatorMove()
	{
		animator.applyRootMotion = false;
		Debug.Log("[MoskarBoss] OnAnimatorMove invoked. Position: " + transform.position + ", has Root Motion: " + animator.applyRootMotion);
	}
}