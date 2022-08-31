using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public abstract class SceneController<T> : Singleton<T> where T : SceneController
{
	/// <summary>SceneController's instance initialization.</summary>
	protected virtual void Awake()
	{
		ResourcesManager.onResourcesLoaded += OnResourcesLoaded;
	}

	/// <summary>SceneController's starting actions before 1st Update frame.</summary>
	protected virtual void Start() { /*...*/ }
	
	/// <summary>SceneController's tick at each frame.</summary>
	protected virtual void Update() { /*...*/ }

	/// <summary>Callback invoked when SceneController's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	protected virtual void OnDestroy()
	{
		ResourcesManager.onResourcesLoaded -= OnResourcesLoaded;
	}

	/// <summary>Callback invoked when resources are loaded.</summary>
	protected virtual void OnResourcesLoaded() { /*...*/ }
}
}