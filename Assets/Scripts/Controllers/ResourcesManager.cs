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
	public event OnResourcesLoaded onResourcesLoaded; 															/// <summary>OnResourcesLoaded's Event Delegate.</summary>

	[Space(5f)]
	[TabGroup("Characters")][SerializeField] private AssetReference[] _charactersReferences; 					/// <summary>Characters' References.</summary>
	[TabGroup("Projectiles")][SerializeField] private AssetReference[] _projectilesReferences; 					/// <summary>Projectiles' References.</summary>
	[TabGroup("FXs", "Particle Effects")][SerializeField] private AssetReference[] _particleEffectsReferences; 	/// <summary>Particle-Effets' References.</summary>
	[TabGroup("FXs", "Explodables")][SerializeField] private AssetReference[] _explodablesReferences; 			/// <summary>Explodables' References.</summary>
	[TabGroup("Pool GameObjects")][SerializeField] private AssetReference[] _poolObjectsReferences; 			/// <summary>[Other] PoolGameObjects' references.</summary>
	[Space(5f)]
	[Header("Audio:")]
	[TabGroup("Audio")][SerializeField] private FiniteStateAudioClipAssetReference[] _FSMLoopsReferences; 		/// <summary>FSM-AudioClips' References.</summary>
	[TabGroup("Audio")][SerializeField] private AudioClipAssetReference[] _loopsReferences; 					/// <summary>Loops' References.</summary>
	[TabGroup("Audio")][SerializeField] private AudioClipAssetReference[] _soundEffectsReferences; 				/// <summary>Sound-Effects' References.</summary>
	private Dictionary<AssetReference, Character> _charactersMap; 												/// <summary>Characters' Mapping.</summary>
	private Dictionary<AssetReference, Projectile> _projectilesMap; 											/// <summary>Projectiles' Mapping.</summary>
	private Dictionary<AssetReference, ParticleEffect> _particleEffectsMap; 									/// <summary>Particle-Effects' Mapping.</summary>
	private Dictionary<AssetReference, Explodable> _explodablesMap; 											/// <summary>Explodables' Mapping.</summary>
	private Dictionary<AssetReference, PoolGameObject> _poolObjectsMap; 										/// <summary>PoolGameObjects' Mapping.</summary>
	private Dictionary<AssetReference, FiniteStateAudioClip> _FSMLoopsMap; 										/// <summary>FSM Loops' Mapping.</summary>
	private Dictionary<AssetReference, AudioClip> _loopsMap; 													/// <summary>Loops' Mapping.</summary>
	private Dictionary<AssetReference, AudioClip> _soundEffectsMap; 											/// <summary>Sound-Effects' Map.</summary>

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
			Debug.Log("[ResourcesManager] Result: " + result.ToString());
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
			Debug.Log("[ResourcesManager] Process Ended. Current progress: " + processes + "/" + totalProcesses);

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

#region AsyncInitializations[DEPRECATE]:
	/// <summary>Initializes Characters' Mappings.</summary>
	/// <param name="onCharactersInitialized">Callback invoked when the Characters have been initialized.</param>
	private async void InitializeCharactersMapping(Action onCharactersInitialized = null)
	{
		if(charactersReferences == null)
		{
			if(onCharactersInitialized != null) onCharactersInitialized();
			return;
		}

		int length = charactersReferences.Length;
		int completed = 0;
		Action onCharacterAdded = ()=>
		{
			completed++;

			if(completed == length && onCharactersInitialized != null)
			onCharactersInitialized();
		};

		_charactersMap = new Dictionary<AssetReference, Character>();

		foreach(AssetReference reference in charactersReferences)
		{
			if(reference == null) continue;

			GameObject obj = null;
			Character character = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) character = obj.GetComponent<Character>();
			if(character != null) charactersMap.Add(reference, character);
		}

		if(onCharactersInitialized != null) onCharactersInitialized();
	}

	/// <summary>Initializes Projectiles' Mappings.</summary>
	/// <param name="onProjectilesInitialized">Callback invoked when the Projectiles have been initialized.</param>
	private async void InitializeProjectilesMapping(Action onProjectilesInitialized = null)
	{
		if(projectilesReferences == null)
		{
			if(onProjectilesInitialized != null) onProjectilesInitialized();
			return;
		}

		int length = projectilesReferences.Length;
		int completed = 0;
		Action onProjectileAdded = ()=>
		{
			completed++;

			if(completed == length && onProjectilesInitialized != null)
			onProjectilesInitialized();
		};

		_projectilesMap = new Dictionary<AssetReference, Projectile>();

		foreach(AssetReference reference in projectilesReferences)
		{
			if(reference == null) continue;

			GameObject obj = null;
			Projectile projectile = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) projectile = obj.GetComponent<Projectile>();
			if(projectile != null) projectilesMap.Add(reference, projectile);
		}

		if(onProjectilesInitialized != null) onProjectilesInitialized();
	}

	/// <summary>Initializes Particle-Effects' Mappings.</summary>
	/// <param name="onParticle-EffectsInitialized">Callback invoked when the Projectiles have been initialized.</param>
	private async void InitializeParticleEffectsMapping(Action onParticleEffectsInitialized = null)
	{
		if(particleEffectsReferences == null)
		{
			if(onParticleEffectsInitialized != null) onParticleEffectsInitialized();
			return;
		}

		int length = particleEffectsReferences.Length;
		int completed = 0;
		Action onParticleEffectAdded = ()=>
		{
			completed++;

			if(completed == length && onParticleEffectsInitialized != null)
			onParticleEffectsInitialized();
		};

		_particleEffectsMap = new Dictionary<AssetReference, ParticleEffect>();

		foreach(AssetReference reference in particleEffectsReferences)
		{
			if(reference == null) continue;

			GameObject obj = null;
			ParticleEffect effect = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) effect = obj.GetComponent<ParticleEffect>();
			if(effect != null) particleEffectsMap.Add(reference, effect);
		}

		if(onParticleEffectsInitialized != null) onParticleEffectsInitialized();
	}

	/// <summary>Initializes Explodables' Mappings.</summary>
	/// <param name="onExplodablesInitialized">Callback invoked when the Explodables have been initialized.</param>
	private async void InitializeExplodablesMapping(Action onExplodablesInitialized = null)
	{
		if(projectilesReferences == null)
		{
			if(onExplodablesInitialized != null) onExplodablesInitialized();
			return;
		}

		int length = explodablesReferences.Length;
		int completed = 0;
		Action onExplodableAdded = ()=>
		{
			completed++;

			if(completed == length && onExplodablesInitialized != null)
			onExplodablesInitialized();
		};

		_explodablesMap = new Dictionary<AssetReference, Explodable>();

		foreach(AssetReference reference in explodablesReferences)
		{
			if(reference == null) continue;

			GameObject obj = null;
			Explodable explodable = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) explodable = obj.GetComponent<Explodable>();
			if(explodable != null) explodablesMap.Add(reference, explodable);
		}

		if(onExplodablesInitialized != null) onExplodablesInitialized();
	}

	/// <summary>Initializes PoolObjects' Mappings.</summary>
	/// <param name="onPoolObjectsInitialized">Callback invoked when the PoolObjects have been initialized.</param>
	private async void InitializePoolObjectsMapping(Action onPoolObjectssInitialized = null)
	{
		if(poolObjectsReferences == null)
		{
			if(onPoolObjectssInitialized != null) onPoolObjectssInitialized();
			return;
		}

		int length = poolObjectsReferences.Length;
		int completed = 0;
		Action onPoolObjectsAdded = ()=>
		{
			completed++;

			if(completed == length && onPoolObjectssInitialized != null)
			onPoolObjectssInitialized();
		};

		_poolObjectsMap = new Dictionary<AssetReference, PoolGameObject>();

		foreach(AssetReference reference in poolObjectsReferences)
		{
			if(reference == null) continue;

			GameObject obj = null;
			PoolGameObject poolObject = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) poolObject = obj.GetComponent<PoolGameObject>();
			if(poolObject != null) poolObjectsMap.Add(reference, poolObject);
		}

		if(onPoolObjectssInitialized != null) onPoolObjectssInitialized();
	}

	/// <summary>Initializes FSMLoops' Mappings.</summary>
	/// <param name="onFSMLoopsInitialized">Callback invoked when the FSMLoops have been initialized.</param>
	private async void InitializeFSMLoopsMapping(Action onFSMLoopsInitialized = null)
	{
		if(FSMLoopsReferences == null)
		{
			if(onFSMLoopsInitialized != null) onFSMLoopsInitialized();
			return;
		}

		int length = FSMLoopsReferences.Length;
		int completed = 0;
		Action onFSMLoopAdded = ()=>
		{
			completed++;

			if(completed == length && onFSMLoopsInitialized != null)
			onFSMLoopsInitialized();
		};

		_FSMLoopsMap = new Dictionary<AssetReference, FiniteStateAudioClip>();

		foreach(AssetReference reference in FSMLoopsReferences)
		{
			if(reference == null) continue;

			UnityEngine.Object obj = null;
			FiniteStateAudioClip FSMLoop = null;

			try { obj = await VAddressables.LoadAssetAsync<FiniteStateAudioClip>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) FSMLoop = obj as FiniteStateAudioClip;
			if(FSMLoop != null) FSMLoopsMap.Add(reference, FSMLoop);
		}

		if(onFSMLoopsInitialized != null) onFSMLoopsInitialized();
	}

	/// <summary>Initializes Loops' Mappings.</summary>
	/// <param name="onLoopsInitialized">Callback invoked when the Loops have been initialized.</param>
	private async void InitializeLoopsMapping(Action onLoopsInitialized = null)
	{
		if(loopsReferences == null)
		{
			if(onLoopsInitialized != null) onLoopsInitialized();
			return;
		}

		int length = loopsReferences.Length;
		int completed = 0;
		Action onProjectileAdded = ()=>
		{
			completed++;

			if(completed == length && onLoopsInitialized != null)
			onLoopsInitialized();
		};

		_loopsMap = new Dictionary<AssetReference, AudioClip>();

		foreach(AssetReference reference in loopsReferences)
		{
			if(reference == null) continue;

			GameObject obj = null;
			AudioClip loop = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) loop = obj.GetComponent<AudioClip>();
			if(loop != null) loopsMap.Add(reference, loop);
		}

		if(onLoopsInitialized != null) onLoopsInitialized();
	}

	/// <summary>Initializes Sound-Effects' Mappings.</summary>
	/// <param name="onSoundEffectsInitialized">Callback invoked when the Sound-Effects have been initialized.</param>
	private async void InitializeSoundEffectsMapping(Action onSoundEffectsInitialized = null)
	{
		if(soundEffectsReferences == null)
		{
			if(onSoundEffectsInitialized != null) onSoundEffectsInitialized();
			return;
		}

		int length = soundEffectsReferences.Length;
		int completed = 0;
		Action onProjectileAdded = ()=>
		{
			completed++;

			if(completed == length && onSoundEffectsInitialized != null)
			onSoundEffectsInitialized();
		};

		_soundEffectsMap = new Dictionary<AssetReference, AudioClip>();

		foreach(AssetReference reference in soundEffectsReferences)
		{
			if(reference == null) continue;

			GameObject obj = null;
			AudioClip soundEffect = null;

			try { obj = await VAddressables.LoadAssetAsync<GameObject>(reference); }
			catch(Exception exception) { Debug.LogException(exception); }

			if(obj != null) soundEffect = obj.GetComponent<AudioClip>();
			if(soundEffect != null) soundEffectsMap.Add(reference, soundEffect);
		}

		if(onSoundEffectsInitialized != null) onSoundEffectsInitialized();
	}
#endregion

#region Getters:
	/// <summary>Gets Character stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Character stored on Dictionary [if there is any].</returns>
	public Character GetCharacter(AssetReference _key)
	{
		if(charactersMap == null) return null;

		Character character = null;

		charactersMap.TryGetValue(_key, out character);

		return character;
	}

	/// <summary>Gets Projectile stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Projectile stored on Dictionary [if there is any].</returns>
	public Projectile GetProjectile(AssetReference _key)
	{
		if(projectilesMap == null) return null;

		Projectile projectile = null;

		projectilesMap.TryGetValue(_key, out projectile);

		return projectile;
	}

	/// <summary>Gets Particle-Effect stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Particle-Effect stored on Dictionary [if there is any].</returns>
	public ParticleEffect GetParticleEffect(AssetReference _key)
	{
		if(particleEffectsMap == null) return null;

		ParticleEffect effect = null;

		particleEffectsMap.TryGetValue(_key, out effect);

		return effect;
	}

	/// <summary>Gets Explodable stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>Explodable stored on Dictionary [if there is any].</returns>
	public Explodable GetExplodable(AssetReference _key)
	{
		if(explodablesMap == null) return null;

		Explodable explodable = null;

		explodablesMap.TryGetValue(_key, out explodable);

		return explodable;
	}

	/// <summary>Gets PoolGameObject stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>PoolGameObject stored on Dictionary [if there is any].</returns>
	public PoolGameObject GetPoolGameObject(AssetReference _key)
	{
		if(poolObjectsMap == null) return null;

		PoolGameObject poolObject = null;

		poolObjectsMap.TryGetValue(_key, out poolObject);

		return poolObject;
	}

	/// <summary>Gets FSMAudioClip stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>FSMAudioClip stored on Dictionary [if there is any].</returns>
	public FiniteStateAudioClip GetFSMClip(AssetReference _key)
	{
		if(FSMLoopsMap == null) return null;

		FiniteStateAudioClip clip = null;

		FSMLoopsMap.TryGetValue(_key, out clip);

		return clip;
	}

	/// <summary>Gets AudioClip stored on Game's Data by given AssetReference's Key.</summary>
	/// <param name="_key">AssetReference's Key [as AssetReference].</param>
	/// <returns>AudioClip stored on Dictionary [if there is any].</returns>
	public AudioClip GetAudioClip(AssetReference _key, SourceType _type = SourceType.Default)
	{
		Dictionary<AssetReference, AudioClip> map = null;
		AudioClip clip = null;

		switch(_type)
		{
			case SourceType.Loop:
			map = loopsMap;
			break;

			case SourceType.Default:
			case SourceType.Scenario:
			case SourceType.SFX:
			map = soundEffectsMap;
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