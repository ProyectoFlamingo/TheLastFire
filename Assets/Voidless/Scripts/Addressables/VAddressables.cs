using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Object = UnityEngine.Object;

namespace Voidless
{
public static class VAddressables
{
	/// <summary>Loads Asset Asyncronously.</summary>
	/// <param name="key">Asset's Reference.</param>
	/// <param name="onTaskFinished">Optional callback invoked when the Task have been finished.</param>
	public static Task<T> LoadAssetAsync<T>(object key, Action onTaskFinished = null)
	{
		TaskCompletionSource<T> source = new TaskCompletionSource<T>();
		AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

		handle.Completed += (completedHandle)=>
		{
			if(completedHandle.OperationException != null)
				source.SetException(completedHandle.OperationException);
			else
				source.SetResult(completedHandle.Result);

			if(onTaskFinished != null) onTaskFinished();
		};

		return source.Task;
	}

	/// <summary>Instantiates Asset Asyncronously.</summary>
	/// <param name="key">Asset's Reference.</param>
	/// <param name="onTaskFinished">Optional callback invoked when the Task have been finished.</param>
	public static Task<GameObject> InstantiateAsync<T>(object key)
	{
		TaskCompletionSource<GameObject> source = new TaskCompletionSource<GameObject>();
		AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key);

		handle.Completed += (completedHandle)=>
		{
			if(completedHandle.OperationException != null)
				source.SetException(completedHandle.OperationException);
			else
				source.SetResult(completedHandle.Result);
		};

		return source.Task;
	}

	/// <summary>Generates a populated Dictionary of GameObjectPools with AssetReferences.</summary>
	/// <param name="_map">Mapping of AssetReferences and already-loaded Pool-GameObjects.</param>
	/// <returns>Populated Dictionary [if provided mapping had something].</returns>
	public static Dictionary<object, GameObjectPool<T>> PopulaledPoolsMapping<T>(Dictionary<AssetReference, T> _map) where T : MonoBehaviour, IPoolObject
	{
		if(_map == null || _map.Count == 0) return null;

		Dictionary<object, GameObjectPool<T>> map = new Dictionary<object, GameObjectPool<T>>();

		foreach(KeyValuePair<AssetReference, T> pair in _map)
		{
			map.Add(pair.Key.RuntimeKey, new GameObjectPool<T>(pair.Value));
		}

		return map;
	}

	/// <summary>Releases objects contained in Pool.</summary>
	/// <param name="_pool">Pool to release.</param>
	public static void ReleasePool<T>(ref GameObjectPool<T> _pool) where T : MonoBehaviour, IPoolObject
	{
		if(_pool == null) return;

		foreach(T poolObject in _pool)
		{
			poolObject.OnObjectDeactivation();
			poolObject.OnObjectDestruction();
			Addressables.Release(poolObject.gameObject);
		}

		_pool = null;
	}

	/// <summary>Loads Dictionary of Asset References and Component types.</summary>
	/// <param name="_references">AssetReferences.</param>
	/// <param name="onLoadingEnds">Callback returning the created Dictionary when the loading ends.</param>
	public static async void LoadComponentMapping<R, T>(R[] _references, Action<Dictionary<R, T>> onLoadingEnds) where T : Component where R : AssetReference
	{
		if(_references == null) return;

		Dictionary<R, T> map = new Dictionary<R, T>();

		foreach(R reference in _references)
		{
			if(reference.Empty()) continue;

			GameObject obj = null;
			T component = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) component = obj.GetComponent<T>();
			if(component != null) map.Add(reference, component);
		}

		if(onLoadingEnds != null) onLoadingEnds(map);
	}

	/// <summary>Loads Dictionary of Asset References and Object [Asset] types.</summary>
	/// <param name="_references">AssetReferences.</param>
	/// <param name="onLoadingEnds">Callback returning the created Dictionary when the loading ends.</param>
	public static async void LoadAssetMapping<R, T>(R[] _references, Action<Dictionary<R, T>> onLoadingEnds) where T : Object where R : AssetReference
	{
		if(_references == null) return;

		Dictionary<R, T> map = new Dictionary<R, T>();

		foreach(R reference in _references)
		{
			if(reference.Empty()) continue;

			T obj = null;

			try { obj = await VAddressables.LoadAssetAsync<T>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) map.Add(reference, obj);
		}

		if(onLoadingEnds != null) onLoadingEnds(map);
	}

	/// <returns>True if AssetReference has a Runtime-Key.</returns>
	public static bool Empty(this AssetReference _reference)
	{
		return string.IsNullOrEmpty(_reference.RuntimeKey.ToString());
	}
}
}