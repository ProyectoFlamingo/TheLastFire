using System;
using System.Reflection;
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
	[TabGroup("Characters")][SerializeField] private VAssetReference[] _charactersReferences; 						/// <summary>Characters' References.</summary>
	[TabGroup("Projectiles")][SerializeField] private VAssetReference[] _projectilesReferences; 					/// <summary>Projectiles' References.</summary>
	[TabGroup("FXs", "Particle Effects")][SerializeField] private VAssetReference[] _particleEffectsReferences; 	/// <summary>Particle-Effets' References.</summary>
	[TabGroup("FXs", "Explodables")][SerializeField] private VAssetReference[] _explodablesReferences; 				/// <summary>Explodables' References.</summary>
	[TabGroup("Pool GameObjects")][SerializeField] private VAssetReference[] _poolObjectsReferences; 				/// <summary>[Other] PoolGameObjects' references.</summary>
	[Space(5f)]
	[Header("Audio:")]
	[TabGroup("Audio", "Audio")][SerializeField] private VAssetReference[] _FSMLoopsReferences; 					/// <summary>FSM-AudioClips' References.</summary>
	[TabGroup("Audio", "Audio")][SerializeField] private VAssetReference[] _loopsReferences; 						/// <summary>Loops' References.</summary>
	[TabGroup("Audio", "Audio")][SerializeField] private VAssetReference[] _soundEffectsReferences; 				/// <summary>Sound-Effects' References.</summary>
	[Space(5f)]
	[Header("TEST:")]
	[SerializeField] private bool test; 																			/// <summary>Test?.</summary>
	private Dictionary<VAssetReference, Character> _charactersMap; 													/// <summary>Characters' Mapping.</summary>
	private Dictionary<VAssetReference, Projectile> _projectilesMap; 												/// <summary>Projectiles' Mapping.</summary>
	private Dictionary<VAssetReference, ParticleEffect> _particleEffectsMap; 										/// <summary>Particle-Effects' Mapping.</summary>
	private Dictionary<VAssetReference, Explodable> _explodablesMap; 												/// <summary>Explodables' Mapping.</summary>
	private Dictionary<VAssetReference, PoolGameObject> _poolObjectsMap; 											/// <summary>PoolGameObjects' Mapping.</summary>
	private Dictionary<VAssetReference, FiniteStateAudioClip> _FSMLoopsMap; 										/// <summary>FSM Loops' Mapping.</summary>
	private Dictionary<VAssetReference, AudioClip> _loopsMap; 														/// <summary>Loops' Mapping.</summary>
	private Dictionary<VAssetReference, AudioClip> _soundEffectsMap; 												/// <summary>Sound-Effects' Map.</summary>

#region Getters/Setters:
	/// <summary>Gets charactersReferences property.</summary>
	public VAssetReference[] charactersReferences { get { return _charactersReferences; } }

	/// <summary>Gets projectilesReferences property.</summary>
	public VAssetReference[] projectilesReferences { get { return _projectilesReferences; } }

	/// <summary>Gets particleEffectsReferences property.</summary>
	public VAssetReference[] particleEffectsReferences { get { return _particleEffectsReferences; } }

	/// <summary>Gets explodablesReferences property.</summary>
	public VAssetReference[] explodablesReferences { get { return _explodablesReferences; } }

	/// <summary>Gets poolObjectsReferences property.</summary>
	public VAssetReference[] poolObjectsReferences { get { return _poolObjectsReferences; } }

	/// <summary>Gets FSMLoopsReferences property.</summary>
	public VAssetReference[] FSMLoopsReferences { get { return _FSMLoopsReferences; } }

	/// <summary>Gets loopsReferences property.</summary>
	public VAssetReference[] loopsReferences { get { return _loopsReferences; } }

	/// <summary>Gets soundEffectsReferences property.</summary>
	public VAssetReference[] soundEffectsReferences { get { return _soundEffectsReferences; } }

	/// <summary>Gets charactersMap property.</summary>
	public Dictionary<VAssetReference, Character> charactersMap { get { return _charactersMap; } }

	/// <summary>Gets projectilesMap property.</summary>
	public Dictionary<VAssetReference, Projectile> projectilesMap { get { return _projectilesMap; } }

	/// <summary>Gets particleEffectsMap property.</summary>
	public Dictionary<VAssetReference, ParticleEffect> particleEffectsMap { get { return _particleEffectsMap; } }

	/// <summary>Gets explodablesMap property.</summary>
	public Dictionary<VAssetReference, Explodable> explodablesMap { get { return _explodablesMap; } }

	/// <summary>Gets poolObjectsMap property.</summary>
	public Dictionary<VAssetReference, PoolGameObject> poolObjectsMap { get { return _poolObjectsMap; } }

	/// <summary>Gets FSMLoopsMap property.</summary>
	public Dictionary<VAssetReference, FiniteStateAudioClip> FSMLoopsMap { get { return _FSMLoopsMap; } }

	/// <summary>Gets loopsMap property.</summary>
	public Dictionary<VAssetReference, AudioClip> loopsMap { get { return _loopsMap; } }

	/// <summary>Gets soundEffectsMap property.</summary>
	public Dictionary<VAssetReference, AudioClip> soundEffectsMap { get { return _soundEffectsMap; } }
#endregion

	/// <summary>ResourcesManager's instance initialization when loaded [Before scene loads].</summary>
	protected override void OnAwake()
	{
		base.OnAwake();

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
				Debug.Log(ToString());
				if(onResourcesLoaded != null) onResourcesLoaded();
#if UNITY_EDITOR
				if(test) Test();
#endif
			}
		};

		VAddressables.LoadComponentMapping<VAssetReference, Character>(charactersReferences, (map)=> { _charactersMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<VAssetReference, Projectile>(projectilesReferences, (map)=> { _projectilesMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<VAssetReference, ParticleEffect>(particleEffectsReferences, (map)=> { _particleEffectsMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<VAssetReference, PoolGameObject>(poolObjectsReferences, (map)=> { _poolObjectsMap = map; onProcessEnds(); });
		VAddressables.LoadComponentMapping<VAssetReference, Explodable>(explodablesReferences, (map)=> { _explodablesMap = map; onProcessEnds(); });
		VAddressables.LoadAssetMapping<VAssetReference, FiniteStateAudioClip>(FSMLoopsReferences, (map)=> { _FSMLoopsMap = map; onProcessEnds(); });
		VAddressables.LoadAssetMapping<VAssetReference, AudioClip>(loopsReferences, (map)=> { _loopsMap = map; onProcessEnds(); });
		VAddressables.LoadAssetMapping<VAssetReference, AudioClip>(soundEffectsReferences, (map)=> { _soundEffectsMap = map; onProcessEnds(); });
	}

	/// <summary>Releases Memory.</summary>
	private void ReleaseMemory()
	{
		charactersMap.Values.ReleaseComponents();
		projectilesMap.Values.ReleaseComponents();
		particleEffectsMap.Values.ReleaseComponents();
		explodablesMap.Values.ReleaseComponents();
		poolObjectsMap.Values.ReleaseComponents();
		FSMLoopsMap.Values.ReleaseObjects();
		loopsMap.Values.ReleaseObjects();
		soundEffectsMap.Values.ReleaseObjects();
	}

#region Getters:
	/// <summary>Gets Character stored on Game's Data by given VAssetReference's Key.</summary>
	/// <param name="_key">VAssetReference's Key [as VAssetReference].</param>
	/// <returns>Character stored on Dictionary [if there is any].</returns>
	public static Character GetCharacter(VAssetReference _key)
	{
		if(Instance.charactersMap == null) return null;

		Character character = null;

		Instance.charactersMap.TryGetValue(_key, out character);

		return character;
	}

	/// <summary>Gets Projectile stored on Game's Data by given VAssetReference's Key.</summary>
	/// <param name="_key">VAssetReference's Key [as VAssetReference].</param>
	/// <returns>Projectile stored on Dictionary [if there is any].</returns>
	public static Projectile GetProjectile(VAssetReference _key)
	{
		if(Instance.projectilesMap == null) return null;

		Projectile projectile = null;

		Instance.projectilesMap.TryGetValue(_key, out projectile);

		return projectile;
	}

	/// <summary>Gets Particle-Effect stored on Game's Data by given VAssetReference's Key.</summary>
	/// <param name="_key">VAssetReference's Key [as VAssetReference].</param>
	/// <returns>Particle-Effect stored on Dictionary [if there is any].</returns>
	public static ParticleEffect GetParticleEffect(VAssetReference _key)
	{
		if(Instance.particleEffectsMap == null) return null;

		ParticleEffect effect = null;

		Instance.particleEffectsMap.TryGetValue(_key, out effect);

		return effect;
	}

	/// <summary>Gets Explodable stored on Game's Data by given VAssetReference's Key.</summary>
	/// <param name="_key">VAssetReference's Key [as VAssetReference].</param>
	/// <returns>Explodable stored on Dictionary [if there is any].</returns>
	public static Explodable GetExplodable(VAssetReference _key)
	{
		if(Instance.explodablesMap == null) return null;

		Explodable explodable = null;

		Instance.explodablesMap.TryGetValue(_key, out explodable);

		return explodable;
	}

	/// <summary>Gets PoolGameObject stored on Game's Data by given VAssetReference's Key.</summary>
	/// <param name="_key">VAssetReference's Key [as VAssetReference].</param>
	/// <returns>PoolGameObject stored on Dictionary [if there is any].</returns>
	public static PoolGameObject GetPoolGameObject(VAssetReference _key)
	{
		if(Instance.poolObjectsMap == null) return null;

		PoolGameObject poolObject = null;

		Instance.poolObjectsMap.TryGetValue(_key, out poolObject);

		return poolObject;
	}

	/// <summary>Gets FSMAudioClip stored on Game's Data by given VAssetReference's Key.</summary>
	/// <param name="_key">VAssetReference's Key [as VAssetReference].</param>
	/// <returns>FSMAudioClip stored on Dictionary [if there is any].</returns>
	public static FiniteStateAudioClip GetFSMClip(VAssetReference _key)
	{
		if(Instance.FSMLoopsMap == null) return null;

		FiniteStateAudioClip clip = null;

		Instance.FSMLoopsMap.TryGetValue(_key, out clip);

		return clip;
	}

	/// <summary>Gets AudioClip stored on Game's Data by given VAssetReference's Key.</summary>
	/// <param name="_key">VAssetReference's Key [as VAssetReference].</param>
	/// <returns>AudioClip stored on Dictionary [if there is any].</returns>
	public static AudioClip GetAudioClip(VAssetReference _key, SourceType _type = SourceType.Default)
	{
		Dictionary<VAssetReference, AudioClip> map = null;
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

	[Button("Debug VAssetReferences")]
	/// <summary>Debug Purposes only....</summary>
	private void DebugVAssetReferences()
	{
		VAssetReference a = null;

		foreach(VAssetReference reference in soundEffectsReferences)
		{
			if(a != null) 
			Debug.Log("[ResourcesManager] Is Reference { " + a + " } different than Reference { " + reference + " }? " + (a != reference));
			a = reference;
			/*Type t = typeof(VAssetReference);
			PropertyInfo[] infos = t.GetProperties();

			Debug.Log("[ResourcesManager] VAssetReference to String: " + reference.ToString());
			Debug.Log("[ResourcesManager] RuntimeKey: " + reference.GetKey());

			foreach(PropertyInfo info in infos)
			{
				Debug.Log(reference.ToString() + ": " + info.PropertyInfoToString());
			}*/
		}
	}

	/// <summary>Tests all Resources.</summary>
	private void Test()
	{
		/*foreach(Character character in charactersMap.Values)
		{
			Instantiate(character, Vector3.zero, Quaternion.identity);
		}*/

		foreach(VAssetReference reference in charactersReferences)
		{
			Instantiate(charactersMap[reference], Vector3.zero, Quaternion.identity);	
		}
	}
}
}