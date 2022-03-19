using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Voidless;
using Sirenix.OdinInspector;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

namespace Flamingo
{
/// <summary>Event invoked when all the resources are loaded.</summary>
public delegate void OnResourcesLoaded();

public class ResourcesManager : Singleton<ResourcesManager>
{
	public static event OnResourcesLoaded onResourcesLoaded; 														/// <summary>OnResourcesLoaded's Event Delegate.</summary>

	[Space(5f)]
	[TabGroup("Characters")][SerializeField] private AssetReference[] _charactersReferences; 						/// <summary>Characters' References.</summary>
	[TabGroup("Projectiles")][SerializeField] private AssetReference[] _projectilesReferences; 						/// <summary>Projectiles' References.</summary>
	[TabGroup("FXs", "Particle Effects")][SerializeField] private AssetReference[] _particleEffectsReferences; 		/// <summary>Particle-Effets' References.</summary>
	[TabGroup("FXs", "Explodables")][SerializeField] private AssetReference[] _explodablesReferences; 				/// <summary>Explodables' References.</summary>
	[TabGroup("Pool GameObjects")][SerializeField] private AssetReference[] _poolObjectsReferences; 				/// <summary>[Other] PoolGameObjects' references.</summary>
	[Space(5f)]
	[Header("Audio:")]
	[TabGroup("Audio", "Audio")][SerializeField] private FiniteStateAudioClipAssetReference[] _FSMLoopsReferences; 	/// <summary>FSM-AudioClips' References.</summary>
	[TabGroup("Audio", "Audio")][SerializeField] private AudioClipAssetReference[] _loopsReferences; 				/// <summary>Loops' References.</summary>
	[TabGroup("Audio", "Audio")][SerializeField] private AudioClipAssetReference[] _soundEffectsReferences; 		/// <summary>Sound-Effects' References.</summary>
	private Dictionary<AssetReference, Character> _charactersMap; 													/// <summary>Characters' Mapping.</summary>
	private Dictionary<AssetReference, Projectile> _projectilesMap; 												/// <summary>Projectiles' Mapping.</summary>
	private Dictionary<AssetReference, ParticleEffect> _particleEffectsMap; 										/// <summary>Particle-Effects' Mapping.</summary>
	private Dictionary<AssetReference, Explodable> _explodablesMap; 												/// <summary>Explodables' Mapping.</summary>
	private Dictionary<AssetReference, PoolGameObject> _poolObjectsMap; 											/// <summary>PoolGameObjects' Mapping.</summary>
	private Dictionary<AssetReference, FiniteStateAudioClip> _FSMLoopsMap; 											/// <summary>FSM Loops' Mapping.</summary>
	private Dictionary<AssetReference, AudioClip> _loopsMap; 														/// <summary>Loops' Mapping.</summary>
	private Dictionary<AssetReference, AudioClip> _soundEffectsMap; 												/// <summary>Sound-Effects' Map.</summary>

#region Getters/Setters:
	/// <summary>Gets charactersReferences property.</summary>
	public AssetReference[] charactersReferences { get { return _charactersReferences; } }

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

	/// <summary>Gets charactersMap property.</summary>
	public Dictionary<AssetReference, Character> charactersMap { get { return _charactersMap; } }

	/// <summary>Gets projectilesMap property.</summary>
	public Dictionary<AssetReference, Projectile> projectilesMap { get { return _projectilesMap; } }

	/// <summary>Gets particleEffectsMap property.</summary>
	public Dictionary<AssetReference, ParticleEffect> particleEffectsMap { get { return _particleEffectsMap; } }

	/// <summary>Gets explodablesMap property.</summary>
	public Dictionary<AssetReference, Explodable> explodablesMap { get { return _explodablesMap; } }

	/// <summary>Gets poolObjectsMap property.</summary>
	public Dictionary<AssetReference, PoolGameObject> poolObjectsMap { get { return _poolObjectsMap; } }

	/// <summary>Gets FSMLoopsMap property.</summary>
	public Dictionary<AssetReference, FiniteStateAudioClip> FSMLoopsMap { get { return _FSMLoopsMap; } }

	/// <summary>Gets loopsMap property.</summary>
	public Dictionary<AssetReference, AudioClip> loopsMap { get { return _loopsMap; } }

	/// <summary>Gets soundEffectsMap property.</summary>
	public Dictionary<AssetReference, AudioClip> soundEffectsMap { get { return _soundEffectsMap; } }
#endregion

	/// <summary>ResourcesManager's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		Addressables.Initialize();
		Addressables.InitializeAsync().Completed += (result)=>
		{
			//Debug.Log("[ResourcesManager] Result: " + result.ToString());
			InitializeResourcesMappings();
		};
	}

	/// <summary>Callback invoked when ResourcesManager's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		ReleaseMemory();
	}

	/// <summary>Initializes Recources' Mappings.</summary>
	private void InitializeResourcesMappings()
	{
		int totalProcesses = 8;
		int processes = 0;

		Action onProcessEnds = ()=>
		{
			processes++;
			//Debug.Log("[ResourcesManager] Process Ended. Current progress: " + processes + "/" + totalProcesses);

			if(processes == totalProcesses)
			{
				if(onResourcesLoaded != null) onResourcesLoaded();
				Debug.Log(ToString());
			}
		};

		VAddressables.LoadComponentMapping<AssetReference, Character>(charactersReferences, (map)=> { _charactersMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<AssetReference, Projectile>(projectilesReferences, (map)=> { _projectilesMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<AssetReference, ParticleEffect>(particleEffectsReferences, (map)=> { _particleEffectsMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<AssetReference, PoolGameObject>(poolObjectsReferences, (map)=> { _poolObjectsMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<AssetReference, Explodable>(explodablesReferences, (map)=> { _explodablesMap = map; onProcessEnds(); });
		VAddressables.LoadAssetMapping<AssetReference, FiniteStateAudioClip>(FSMLoopsReferences, (map)=> { _FSMLoopsMap = map; onProcessEnds(); });
		VAddressables.LoadAssetMapping<AssetReference, AudioClip>(loopsReferences, (map)=> { _loopsMap = map; onProcessEnds(); });
		VAddressables.LoadAssetMapping<AssetReference, AudioClip>(soundEffectsReferences, (map)=> { _soundEffectsMap = map; onProcessEnds(); });
	}

	/// <summary>Releases Memory.</summary>
	private void ReleaseMemory()
	{
		ReleaseComponentMapping<Character>(ref _charactersMap);
		ReleaseComponentMapping<Projectile>(ref _projectilesMap);
		ReleaseComponentMapping<ParticleEffect>(ref _particleEffectsMap);
		ReleaseComponentMapping<Explodable>(ref _explodablesMap);
		ReleaseComponentMapping<PoolGameObject>(ref _poolObjectsMap);
		ReleaseMapping<FiniteStateAudioClip>(ref _FSMLoopsMap);
		ReleaseMapping<AudioClip>(ref _loopsMap);
		ReleaseMapping<AudioClip>(ref _soundEffectsMap);
	}

	/// <summary>Releases Objects contained in given Mapping.</summary>
	/// <param name="map">Mapping's Reference.</param>
	private void ReleaseComponentMapping<T>(ref Dictionary<AssetReference, T> map) where T : MonoBehaviour
	{
		if(map == null) return;

		foreach(T component in map.Values)
		{
			if(component != null)
			Addressables.Release(component.gameObject);
		}
	}

	/// <summary>Releases Objects contained in given Mapping.</summary>
	/// <param name="map">Mapping's Reference.</param>
	private void ReleaseMapping<T>(ref Dictionary<AssetReference, T> map) where T : UnityEngine.Object
	{
		if(map == null) return;

		foreach(T obj in map.Values)
		{
			if(obj != null)
			Addressables.Release(obj);
		}
	}

#region Getters:
	/// <summary>Gets Character stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Character stored on Dictionary [if there is any].</returns>
	public static Character GetCharacter(AssetReference _key)
	{
		if(Instance.charactersMap == null) return null;

		Character character = null;

		Instance.charactersMap.TryGetValue(_key, out character);

		return character;
	}

	/// <summary>Gets Projectile stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Projectile stored on Dictionary [if there is any].</returns>
	public static Projectile GetProjectile(AssetReference _key)
	{
		if(Instance.projectilesMap == null) return null;

		Projectile projectile = null;

		Instance.projectilesMap.TryGetValue(_key, out projectile);

		return projectile;
	}

	/// <summary>Gets Particle-Effect stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Particle-Effect stored on Dictionary [if there is any].</returns>
	public static ParticleEffect GetParticleEffect(AssetReference _key)
	{
		if(Instance.particleEffectsMap == null) return null;

		ParticleEffect effect = null;

		Instance.particleEffectsMap.TryGetValue(_key, out effect);

		return effect;
	}

	/// <summary>Gets Explodable stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Explodable stored on Dictionary [if there is any].</returns>
	public static Explodable GetExplodable(AssetReference _key)
	{
		if(Instance.explodablesMap == null) return null;

		Explodable explodable = null;

		Instance.explodablesMap.TryGetValue(_key, out explodable);

		return explodable;
	}

	/// <summary>Gets PoolGameObject stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>PoolGameObject stored on Dictionary [if there is any].</returns>
	public static PoolGameObject GetPoolGameObject(AssetReference _key)
	{
		if(Instance.poolObjectsMap == null) return null;

		PoolGameObject poolObject = null;

		Instance.poolObjectsMap.TryGetValue(_key, out poolObject);

		return poolObject;
	}

	/// <summary>Gets FSMAudioClip stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>FSMAudioClip stored on Dictionary [if there is any].</returns>
	public static FiniteStateAudioClip GetFSMClip(AssetReference _key)
	{
		if(Instance.FSMLoopsMap == null) return null;

		FiniteStateAudioClip clip = null;

		Instance.FSMLoopsMap.TryGetValue(_key, out clip);

		return clip;
	}

	/// <summary>Gets AudioClip stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>AudioClip stored on Dictionary [if there is any].</returns>
	public static AudioClip GetAudioClip(AssetReference _key, SourceType _type = SourceType.Default)
	{
		Dictionary<AssetReference, AudioClip> map = null;
		AudioClip clip = null;

		switch(_type)
		{
			case SourceType.Loop:
			map = Instance.loopsMap;
			break;

			case SourceType.Default:
			case SourceType.Scenario:
			case SourceType.SFX:
			map = Instance.soundEffectsMap;
			break;
		}

		map.TryGetValue(_key, out clip);

		return clip;
	}
#endregion

	/// <returns>String representing all resources.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();
		string EMPTY = "Empty...";

		builder.AppendLine("Resources' Manager: \n");
		builder.AppendLine("Characters' Map: ");
		builder.AppendLine(charactersMap != null && charactersMap.Count > 0 ? charactersMap.DictionaryToString() : EMPTY);
		builder.AppendLine("Projectiles' Map: ");
		builder.AppendLine(projectilesMap != null && projectilesMap.Count > 0 ? projectilesMap.DictionaryToString() : EMPTY);
		builder.AppendLine("Particle-Effects' Map: ");
		builder.AppendLine(particleEffectsMap != null && particleEffectsMap.Count > 0 ? particleEffectsMap.DictionaryToString() : EMPTY);
		builder.AppendLine("Explodables' Map: ");
		builder.AppendLine(explodablesMap != null && explodablesMap.Count > 0 ? explodablesMap.DictionaryToString() : EMPTY);
		builder.AppendLine("Pool-GameObjects' Map: ");
		builder.AppendLine(poolObjectsMap != null && poolObjectsMap.Count > 0 ? poolObjectsMap.DictionaryToString() : EMPTY);
		builder.AppendLine("FSM-AudioClips' Map: ");
		builder.AppendLine(FSMLoopsMap != null && FSMLoopsMap.Count > 0 ? FSMLoopsMap.DictionaryToString() : EMPTY);
		builder.AppendLine("Loops' Map: ");
		builder.AppendLine(loopsMap != null && loopsMap.Count > 0 ? loopsMap.DictionaryToString() : EMPTY);
		builder.AppendLine("Sound-Effects' Map: ");
		builder.AppendLine(soundEffectsMap != null && soundEffectsMap.Count > 0 ? soundEffectsMap.DictionaryToString() : EMPTY);

		return builder.ToString();
	}
}
}