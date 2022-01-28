using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Voidless;
using Sirenix.OdinInspector;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Flamingo
{
public class ResourcesManager : Singleton<ResourcesManager>
{
	[Header("Addressables:")]
	[SerializeField] private AssetReference[] _projectilesReferences; 							/// <summary>Projectiles' References.</summary>
	[SerializeField] private AssetReference[] _particleEffectsReferences; 						/// <summary>Particle-Effets' References.</summary>
	[SerializeField] private AssetReference[] _explodablesReferences; 							/// <summary>Explodables' References.</summary>
	[SerializeField] private AssetReference[] _poolObjectsReferences; 							/// <summary>[Other] PoolGameObjects' references.</summary>
	[SerializeField] private FiniteStateAudioClipAssetReference[] _FSMLoopsReferences; 			/// <summary>FSM-AudioClips' References.</summary>
	[SerializeField] private AudioClipAssetReference[] _loopsReferences; 						/// <summary>Loops' References.</summary>
	[SerializeField] private AudioClipAssetReference[] _soundEffectsReferences; 				/// <summary>Sound-Effects' References.</summary>
	private Dictionary<AssetReference, Projectile> _projectilesMap; 									/// <summary>Projectiles' Mapping.</summary>
	private Dictionary<AssetReference, ParticleEffect> _particleEffectsMap; 							/// <summary>Particle-Effects' Mapping.</summary>
	private Dictionary<AssetReference, Explodable> _explodablesMap; 									/// <summary>Explodables' Mapping.</summary>
	private Dictionary<AssetReference, PoolGameObject> _poolObjectsMap; 								/// <summary>PoolGameObjects' Mapping.</summary>
	private Dictionary<AssetReference, FiniteStateAudioClip> _FSMLoopsMap; 							/// <summary>FSM Loops' Mapping.</summary>
	private Dictionary<AssetReference, AudioClip> _loopsMap; 											/// <summary>Loops' Mapping.</summary>
	private Dictionary<AssetReference, AudioClip> _soundEffectsMap; 									/// <summary>Sound-Effects' Map.</summary>

	/// <summary>Gets projectilesReferences property.</summary>
	public AssetReference[] projectilesReferences { get { return _projectilesReferences; } }

	/// <summary>Gets particleEffectsReferences property.</summary>
	public AssetReference[] particleEffectsReferences { get { return _particleEffectsReferences; } }

	/// <summary>Gets explodablesReferences property.</summary>
	public AssetReference[] explodablesReferences { get { return _explodablesReferences; } }

	/// <summary>Gets poolObjectsReferences property.</summary>
	public AssetReference[] poolObjectsReferences { get { return _poolObjectsReferences; } }

	/// <summary>Gets FSMLoopsReferences property.</summary>
	public FiniteStateAudioClipAssetReference[] FSMLoopsReferences { get { return _FSMLoopsReferences; } }

	/// <summary>Gets loopsReferences property.</summary>
	public AudioClipAssetReference[] loopsReferences { get { return _loopsReferences; } }

	/// <summary>Gets soundEffectsReferences property.</summary>
	public AudioClipAssetReference[] soundEffectsReferences { get { return _soundEffectsReferences; } }

	/// <summary>Gets projectilesMap property.</summary>
	public Dictionary<AssetReference, Projectile> projectilesMap { get { return _projectilesMap; } }

	/// <summary>Gets particleEffectsMap property.</summary>
	public Dictionary<AssetReference, ParticleEffect> particleEffectsMap { get { return _particleEffectsMap; } }

	/// <summary>ResourcesManager's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		Addressables.Initialize();
		Addressables.InitializeAsync().Completed += (result)=>
		{
			Debug.Log("[ResourcesManager] Result: " + result.ToString());
			InitializeResourcesMappings();
		};
	}

	/// <summary>Initializes Recources' Mappings.</summary>
	private void InitializeResourcesMappings()
	{
		int totalProcesses = 1;
		int processes = 0;

		Action onProcessEnds = ()=>
		{
			processes++;
		};

		InitializeProjectilesMapping(onProcessEnds);
	}

	private async void InitializeProjectilesMapping(Action onProjectilesInitialized = null)
	{
		if(projectilesReferences == null) return;

		int length = projectilesReferences.Length;
		int completed = 0;
		Action onProjectileAdded = ()=>
		{
			completed++;
			if(completed == length && onProjectilesInitialized != null)
			{
				Debug.Log("[ResourcesManager] Projectiles' Mapping: " + projectilesMap.DictionaryToString());
				onProjectilesInitialized();
			}
		};

		_projectilesMap = new Dictionary<AssetReference, Projectile>();

		foreach(AssetReference reference in projectilesReferences)
		{
			GameObject obj = null;
			Projectile projectile = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) projectile = obj.GetComponent<Projectile>();

			if(projectile != null)
			{
				Debug.Log("[ResourcesManager] Projectile: " + projectile.name);
				projectilesMap.Add(reference, projectile);
			}
		}

		Debug.Log("[ResourcesManager] Projectiles' Mapping: " + projectilesMap.DictionaryToString());
		if(onProjectilesInitialized != null) onProjectilesInitialized();
	}

	/// <summary>Initializes Projectiles.</summary>
	/// <param name="onProjectilesInitialized">Callback invoked when the projectiles have been initialized.</param>
	/// <param name="indices">Indices.</param>
	public void InitializeProjectiles(Action onProjectilesInitialized = null, params int[] indices)
	{
		if(indices == null) return;

		int length = indices.Length;
		int index = 0;
		int completed = 0;

		Action f = ()=>
		{
			completed++;
			if(completed == length && onProjectilesInitialized != null) onProjectilesInitialized();
		};

		//_projectiles = new Projectile[projectilesReferences.Length];

		for(int i  = 0; i < length; i++)
		{
			index = Mathf.Clamp(indices[i], 0, length - 1);
			//AssignProjectile(index, f);			
		}
	}

	/// <summary>Gets Projectile stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Projectile stored on Dictionary [if there is any].</returns>
	public Projectile GetProjectile(AssetReference _key)
	{
		Projectile projectile = null;

		projectilesMap.TryGetValue(_key, out projectile);

		return projectile;
	}

	/// <summary>Gets Particle-Effect stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Particle-Effect stored on Dictionary [if there is any].</returns>
	public ParticleEffect GetParticleEffect(AssetReference _key)
	{
		ParticleEffect effect = null;

		particleEffectsMap.TryGetValue(_key, out effect);

		return effect;
	}
}
}