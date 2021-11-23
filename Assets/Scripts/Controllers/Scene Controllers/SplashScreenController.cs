using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class SplashScreenController : MonoBehaviour
{
	[SerializeField] private string _sceneName; 		/// <summary>Scene's Name.</summary>
	[SerializeField] private float _fadeOutDuration; 	/// <summary>Screen's Fade-Out Duration.</summary>
	[SerializeField] private float _defaultWait; 		/// <summary>Default Wait's Duration before changing scene.</summary>

	/// <summary>Gets sceneName property.</summary>
	public string sceneName { get { return _sceneName; } }

	/// <summary>Gets fadeOutDuration property.</summary>
	public float fadeOutDuration { get { return _fadeOutDuration; } }

	/// <summary>Gets defaultWait property.</summary>
	public float defaultWait { get { return _defaultWait; } }

	/// <summary>SplashScreenController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		Game.ActivateMateo(false);
		//Game.ActivateGameplayCamera(false);
		Game.FadeOutScreen(Color.black, fadeOutDuration,
		()=>
		{
			this.StartCoroutine(this.WaitSeconds(defaultWait,
			()=>
			{
				Game.LoadScene(sceneName);
			}));
		});
	}
}
}