using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(Animator))]
public class VAnimatorController : MonoBehaviour
{
	private List<int> _activeAnimations; 	/// <summary>Active Animations.</summary>
	private Coroutine[] layerRoutines; 		/// <summary>Layer's Coroutines [for Cross-Fading].</summary>
	private Animator _animator; 			/// <summary>Animator's Component.</summary>

	/// <summary>Gets and Sets activeAnimations property.</summary>
	public List<int> activeAnimations
	{
		get { return _activeAnimations; }
		protected set { _activeAnimations = value; }
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

	/// <summary>VAnimatorController's instance initialization.</summary>
	private void Awake()
	{
		EvaluateActiveList(2);
		EvaluateLayerRoutines(2);
	}

	/// <summary>Deactivates Layer.</summary>
	/// <param name="_layer">Layer to deactivate [0 by default].</param>
	public void DeactivateLayer(int _layer = 0)
	{
		_layer = Mathf.Max(_layer, 0);
		activeAnimations[_layer] = 0;
	}

	/// <summary>Evaluates if Animation Hash is active on given layer.</summary>
	/// <param name="_hash">Animation's Hash.</param>
	/// <param name="_layer">Animation's Layer [0 by default].</param>
	/// <returns>True if given Animation Hash is currently active on the layer.</returns>
	public bool GetActive(int _hash, int _layer = 0)
	{
		EvaluateActiveList(_layer);
		return _hash == activeAnimations[_layer];
	}

	/// <summary>Sets Active Animation Hash on Animation Layer.</summary>
	/// <param name="_hash">Animation's Hash.</param>
	/// <param name="_layer">Animation's Layer [0 by default].</param>
	public void SetActive(int _hash, int _layer = 0)
	{
		EvaluateActiveList(_layer);
		activeAnimations[_layer] = _hash;
	}

	/// <summary>Plays Animator's State.</summary>
	/// <param name="_hash">State's Hash.</param>
	/// <param name="_layer">AnimatorController's Layer.</param>
	/// <param name="t">Normalized Time Offset [where does the Animation start].</param>
	/// <returns>True if the Animation could be played, false if it was already active.</returns>
	public bool Play(int _hash, int _layer = 0, float t = Mathf.NegativeInfinity)
	{
		if(GetActive(_hash, _layer)) return false;

		SetActive(_hash, _layer);
		animator.Play(_hash, _layer, t);
		return true;
	}

	/// <summary>Plays Animator's State and waits for that state to end.</summary>
	/// <param name="_hash">State's Hash.</param>
	/// <param name="_layer">AnimatorController's Layer.</param>
	/// <param name="t">Normalized Time Offset [where does the Animation start].</param>
	/// <param name="_additionalWait">AdditionalWait [0.0f by default].</param>
	/// <param name="onPlayEnds">Callback invoked when the wait ends [null by default].</param>
	/// <returns>True if the Animation could be played, false if it was already active.</returns>
	public bool PlayAndWait(int _hash, int _layer = 0, float t = Mathf.NegativeInfinity, float _additionalWait = 0.0f, Action onPlayEnds = null)
	{
		_layer = Mathf.Max(_layer, 0);

		if(GetActive(_hash, _layer)) return false;

		EvaluateLayerRoutines(_layer);
		SetActive(_hash, _layer);
		this.StartCoroutine(
			animator.PlayAndWait(
				_hash,
				_layer,
				t,
				_additionalWait,
				()=>
				{
					this.DispatchCoroutine(ref layerRoutines[_layer]);
					DeactivateLayer(_layer);
					if(onPlayEnds != null) onPlayEnds();
				}
			),
			ref layerRoutines[_layer]
		);
		return true;
	}

	/// <summary>Cross-Fades towards Animation.</summary>
	/// <param name="_hash">AnimationState's Hash.</param>
	/// <param name="_fadeDuration">Cross-Fade's Duration.</param>
	/// <param name="_layerIndex">Layer's Index [0 by default].</param>
	/// <param name="_offset">Normalized Time Offset [where does the Animation start].</param>
	/// <param name="_transitionTime">Optional Additional Wait [0.0f by default].</param>
	/// <returns>True if the Animation could be played, false if it was already active.</returns>
	public bool CrossFade(int _hash, float _fadeDuration, int _layer = 0, float _offset = Mathf.NegativeInfinity, float _transitionTime = 0.0f)
	{
		if(GetActive(_hash, _layer)) return false;

		SetActive(_hash, _layer);
		animator.CrossFade(_hash, _fadeDuration, _layer, _offset, _transitionTime);
		return true;
	}

	/// <summary>Cross-Fades towards Animation and waits until that next animation is finished.</summary>
	/// <param name="_hash">AnimationState's Hash.</param>
	/// <param name="_fadeDuration">Cross-Fade's Duration.</param>
	/// <param name="_layerIndex">Layer's Index [0 by default].</param>
	/// <param name="_offset">Normalized Time Offset [where does the Animation start].</param>
	/// <param name="_transitionTime">Optional Additional Wait [0.0f by default].</param>
	/// <param name="onAnimationEnds">Callback invoked when the animation ends.</param>
	/// <returns>True if the Animation could be played, false if it was already active.</returns>
	public bool CrossFadeAndWait(int _hash, float _fadeDuration, int _layer = 0, float _offset = Mathf.NegativeInfinity, float _additionalWait = 0.0f, Action onAnimationEnds = null)
	{
		if(GetActive(_hash, _layer)) return false;

		EvaluateLayerRoutines(_layer);
		SetActive(_hash, _layer);

		this.StartCoroutine(
			animator.CrossFadeAndWait(
				_hash,
				_fadeDuration,
				_layer,
				_offset,
				_additionalWait,
				()=>
				{
					this.DispatchCoroutine(ref layerRoutines[_layer]);
					DeactivateLayer(_layer);
					if(onAnimationEnds != null) onAnimationEnds();
				}
			),
			ref layerRoutines[_layer]
		);

		return true;
	}

	/// <summary>Cancels Cross-Fading's Routie on given layer.</summary>
	/// <param name="_layer">Animation's Layer.</param>
	/// <returns>True if there was an active Cross-Fading to cancel.</returns>
	public bool CancelCrossFading(int _layer = 0)
	{
		_layer = Mathf.Max(_layer, 0);

		if(layerRoutines[_layer] == null) return false;

		this.DispatchCoroutine(ref layerRoutines[_layer]);
		return true;
	}

	/// <summary>Evaluates possible resizing Active Animation Hashes' List.</summary>
	/// <param name="_layer">Layer reference useful for possible resizing.</param>
	private void EvaluateActiveList(int _layer)
	{
		if(activeAnimations == null) activeAnimations = new List<int>(2);

		_layer = Mathf.Max(_layer, 0);

		if(_layer > activeAnimations.Count - 1) while((activeAnimations.Count - 1) < _layer)
		{
			activeAnimations.Add(0);
		}
	}

	/// <summary>Evaluates possible resizing Layer's Coroutines.</summary>
	/// <param name="_layer">Layer reference useful for possible resizing.</param>
	private void EvaluateLayerRoutines(int _layer)
	{
		if(layerRoutines == null) layerRoutines = new Coroutine[2];

		_layer = Mathf.Max(_layer, 0);
		if(_layer > layerRoutines.Length - 1) Array.Resize(ref layerRoutines, _layer + 1);
	}
}
}