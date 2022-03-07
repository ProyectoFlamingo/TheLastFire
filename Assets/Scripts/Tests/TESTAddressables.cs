using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Flamingo
{
/*[Serializable]
public class ProjectileAssetReference : AssetReferenceT<Projectile>
{
	public ProjectileAssetReference(string guid) : base(guid) {}

	public override bool ValidateAsset(UnityEngine.Object obj)
	{
		Type type = obj.GetType();
		return typeof(TObject).IsAssignableFrom(type);
	}

	public override bool ValidateAsset(string path)
    {
#if UNITY_EDITOR
        Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
        return typeof(TObject).IsAssignableFrom(type);
#else
        return false;
#endif
    }
}*/

public class TESTAddressables : MonoBehaviour
{
	[SerializeField] private AssetReference[] references;
	[SerializeField] private Projectile[] projectiles;

	private void Start ()
	{
		if(references == null) return;

		projectiles = new Projectile[references.Length];

		Addressables.InitializeAsync().Completed += (result)=>
		{
			int i = 0;

			foreach(AssetReference reference in references)
			{
				AssignProjectile(i, reference);
				i++;
			}
		};
	}

	/// <summary>Callback invoked when TESTAddressables's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		foreach(Projectile projectile in projectiles)
		{
			Addressables.Release(projectile.gameObject);
		}
	}

	private void AssignProjectile(int index, AssetReference reference)
	{
		Addressables.LoadAssetAsync<GameObject>(reference).Completed += (obj)=>
		{
			GameObject g = obj.Result;
			projectiles[index] = g.GetComponent<Projectile>();

			Instantiate(g, Vector3.zero, Quaternion.identity);
		};
	}
}
}