using System;
using System.Collections;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum AnimationFunction
{
	Play,
	CrossFade,
	PlayAndWait,
	CrossFadeAndWait
}

[Serializable]
public struct AnimationFunctionData
{
	public AnimationFunction function; 		/// <summary>Animation's Function.</summary>
	public AnimatorCredential animation; 	/// <summary>AnimationToPlay.</summary>
	public int layer; 						/// <summary>Animation's Layer.</summary>
	public float fadeDuration; 				/// <summary>Animation's Fade Duration.</summary>
	public float additionalWait; 			/// <summary>Additional Wait.</summary>

	/// <summary>Executes VAnimatorController's Animation given this AnimationFunctionData's parameters.</summary>
	/// <param name="_animatorController">VAnimatorController's reference.</param>
	public IEnumerator Execute(VAnimatorController _animatorController)
	{
		if(_animatorController == null) yield break;

		int hash = animation.ID;
		bool finished = false;
		Action onAnimationEnds = ()=> { finished = true; };

		switch(function)
		{
			case AnimationFunction.Play:
				_animatorController.Play(hash, layer);
				finished = true;
			break;

			case AnimationFunction.CrossFade:
				_animatorController.CrossFade(hash, fadeDuration, layer);
				finished = true;
			break;

			case AnimationFunction.PlayAndWait:
				_animatorController.PlayAndWait(hash, layer, Mathf.NegativeInfinity, 0.0f, onAnimationEnds);
			break;

			case AnimationFunction.CrossFadeAndWait:
				_animatorController.CrossFadeAndWait(hash, fadeDuration, layer, Mathf.NegativeInfinity, 0.0f, onAnimationEnds);
			break;
		}

		while(!finished) yield return null;

		if(additionalWait > 0.0f)
		{
			SecondsDelayWait wait = new SecondsDelayWait(additionalWait);
			while(wait.MoveNext()) yield return null;
		}
	}
}
}