using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;
using UnityEngine.SceneManagement;

namespace Flamingo
{
public class LoadingSceneController : MonoBehaviour
{
	[SerializeField] private RectTransform _loadingBar; 	/// <summary>Loading Bar's UI.</summary>
	[SerializeField] private float _additionalWait; 		/// <summary>Additional Wait.</summary>
	[SerializeField] private float _fadeInDuration; 		/// <summary>Fade-In's Duration.</summary>
	[SerializeField] private float _fadeOutDuration; 		/// <summary>Fade-Out's Duration.</summary>

	/// <summary>Gets loadingBar property.</summary>
	public RectTransform loadingBar { get { return _loadingBar; } }

	/// <summary>Gets additionalWait property.</summary>
	public float additionalWait { get { return _additionalWait; } }

	/// <summary>Gets fadeInDuration property.</summary>
	public float fadeInDuration { get { return _fadeInDuration; } }

	/// <summary>Gets fadeOutDuration property.</summary>
	public float fadeOutDuration { get { return _fadeOutDuration; } }

	/// <summary>LoadingSceneController's instance initialization.</summary>
	private void Awake()
	{
		if(loadingBar != null) loadingBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
	}

	/// <summary>LoadingSceneController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		string scenePath = PlayerPrefs.GetString(GameData.PATH_SCENE_TOLOAD, GameData.PATH_SCENE_DEFAULT);

		Game.ActivateMateo(false);
		Game.FadeOutScreen(Color.black, fadeOutDuration,
		()=>
		{
			this.StartCoroutine(AsynchronousSceneLoader.LoadSceneAndDoWhileWaiting(
				scenePath,
				OnSceneLoading,
				OnLoadEnds,
				LoadSceneMode.Single,
				additionalWait
			));
		});

		//AsynchronousSceneLoader.LoadScene(scenePath);		
	}
	
	/// <summary>LoadingSceneController's tick at each frame.</summary>
	/// <param name="progress">Normalized progress of the loading.</param>
	private void OnSceneLoading(float progress)
	{
		if(loadingBar == null) return;

		Vector3 scale = loadingBar.localScale;
		scale.x = progress;
		loadingBar.localScale = scale;
	}

	/// <summary>Callback invoked when scene loads at 90%.</summary>
	private void OnLoadEnds()
	{
		AsynchronousSceneLoader.AllowSceneActivation(false);
		Game.FadeInScreen(Color.black, fadeInDuration,
		()=>
		{
			AsynchronousSceneLoader.AllowSceneActivation(true);
		});
	}
}
}