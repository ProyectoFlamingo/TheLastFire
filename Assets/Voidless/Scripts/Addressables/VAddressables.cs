using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Voidless
{
public class VAddressables
{
	public static Task<T> LoadAssetAsync<T>(object key)
	{
		TaskCompletionSource<T> source = new TaskCompletionSource<T>();
		AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);

		handle.Completed += (completedHandle)=>
		{
			if(completedHandle.OperationException != null)
				source.SetException(completedHandle.OperationException);
			else
				source.SetResult(completedHandle.Result);
		};

		return source.Task;
	}

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
}
}