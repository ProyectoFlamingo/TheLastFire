using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
[Flags]
public enum Override
{
	None = 0,
	DontOffsetFromParent = 1,
	ApplyRootMotion = 2,
	ApplyBuiltinRootMotion = 4
}

[RequireComponent(typeof(Animator))]
public class OnAnimatorMoveOverrider : MonoBehaviour
{
	[SerializeField] private Override _overrideActions; 	/// <summary>Things to override on OnAnimatorMove.</summary>
	private Animator _animator; 							/// <summary>Animator's Component.</summary>

	/// <summary>Gets and Sets overrideActions property.</summary>
	public Override overrideActions
	{
		get { return _overrideActions; }
		set { _overrideActions = value; }
	}

	/// <summary>Gets animator Component.</summary>
	public Animator animator
	{ 
		get
		{
			if(_animator == null) _animator = GetComponent<Animator>();
			return _animator;
		}
	}

	/// <summary>Callback for processing animation movements for modifying root motion.</summary>
	private void OnAnimatorMove()
	{
		if((overrideActions | Override.DontOffsetFromParent) == overrideActions)
		{
			Transform parent = transform.parent;

			if(parent == null) return;

			Vector3 position = parent.position;
			position = animator.deltaPosition;
			parent.position = position;
		}
		if((overrideActions | Override.ApplyBuiltinRootMotion) == overrideActions) animator.ApplyBuiltinRootMotion();

		animator.applyRootMotion = ((overrideActions | Override.ApplyRootMotion) == overrideActions);
	}
}
}