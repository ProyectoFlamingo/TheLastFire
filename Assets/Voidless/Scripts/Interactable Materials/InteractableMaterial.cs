using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Flags]
public enum MaterialID
{
	None = 0,
	Steel = 1 << 0,
	Wood = 1 << 1,
	Skin = 1 << 2,
	Cloth = 1 << 3,
	Rubber = 1 << 4,
	Fire = 1 << 5,
	Rock = 1 << 6,
	Water = 1 << 7,
	Lava = 1 << 8,
	Grass = 1 << 9,
	Snow = 1 << 10,
	Ice = 1 << 11,
	Soil = 1 << 12,
	_14 = 1 << 13,
	_15 = 1 << 14,
	_16 = 1 << 15,
	_17 = 1 << 16,
	_18 = 1 << 17,
	_19 = 1 << 18,
	_20 = 1 << 19,
	_21 = 1 << 20,
	_22 = 1 << 21,
	_23 = 1 << 22,
	_24 = 1 << 23,
	_25 = 1 << 24,
	_26 = 1 << 25,
	_27 = 1 << 26,
	_28 = 1 << 27,
	_29 = 1 << 28,
	_30 = 1 << 29,
	_31 = 1 << 30,
	_32 = 1 << 31,
}

[Serializable] public struct MaterialIDCollectionIndexPair { public MaterialID ID; public CollectionIndex index; }
[Serializable] public struct MaterialIDIntPair { public MaterialID ID; public int index; }

//[RequireComponent(typeof(EventsHandler))]
public class InteractableMaterial : MonoBehaviour
{
	[SerializeField] private MaterialID _ID; 								/// <summary>Material's ID.</summary>
	[SerializeField] private int _subID; 									/// <summary>Sub-ID.</summary>
	[SerializeField] private MaterialIDIntPair[] _particleEffectsMatrix; 	/// <summary>Matrix of Particle Effects emitted by each specific interaction.</summary>
	[SerializeField] private MaterialIDIntPair[] _soundEffectsMatrix; 		/// <summary>Matrix of Sound Effects emitted by each specific interaction.</summary>
	[SerializeField] private EventsHandler _eventsHandler; 					/// <summary>EventsHandler's Component.</summary>
	private Dictionary<MaterialID, List<int>> _particleEffectsDictionary; 	/// <summary>Dictionary of Particle Effects emitted by each specific interaction.</summary>
	private Dictionary<MaterialID, List<int>> _soundEffectsDictionary; 		/// <summary>Dictionary of Sound Effects emitted by each specific interaction.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ID property.</summary>
	public MaterialID ID
	{
		get { return _ID; }
		set { _ID = value; }
	}

	/// <summary>Gets and Sets subID property.</summary>
	public int subID
	{
		get { return _subID; }
		set { _subID = value; }
	}

	/// <summary>Gets and Sets particleEffectsMatrix property.</summary>
	public MaterialIDIntPair[] particleEffectsMatrix
	{
		get { return _particleEffectsMatrix; }
		set { _particleEffectsMatrix = value; }
	}

	/// <summary>Gets and Sets soundEffectsMatrix property.</summary>
	public MaterialIDIntPair[] soundEffectsMatrix
	{
		get { return _soundEffectsMatrix; }
		set { _soundEffectsMatrix = value; }
	}

	/// <summary>Gets and Sets particleEffectsDictionary property.</summary>
	public Dictionary<MaterialID, List<int>> particleEffectsDictionary
	{
		get { return _particleEffectsDictionary; }
		private set { _particleEffectsDictionary = value; }
	}

	/// <summary>Gets and Sets soundEffectsDictionary property.</summary>
	public Dictionary<MaterialID, List<int>> soundEffectsDictionary
	{
		get { return _soundEffectsDictionary; }
		private set { _soundEffectsDictionary = value; }
	}

	/// <summary>Gets eventsHandler Component.</summary>
	public EventsHandler eventsHandler
	{ 
		get
		{
			if(_eventsHandler == null) _eventsHandler = GetComponent<EventsHandler>();
			return _eventsHandler;
		}
	}
#endregion

	/// <summary>InteractableMaterial's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		if(particleEffectsMatrix != null) PopulateDictionary(particleEffectsMatrix, ref _particleEffectsDictionary);
		if(soundEffectsMatrix != null) PopulateDictionary(soundEffectsMatrix, ref _soundEffectsDictionary);

		if(eventsHandler != null) eventsHandler.onTriggerEvent += OnTriggerEvent;
	}

	/// <summary>Callback invoked when InteractableMaterial's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		if(eventsHandler != null) eventsHandler.onTriggerEvent -= OnTriggerEvent;
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	private void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		if(_eventType != HitColliderEventTypes.Enter) return;

		InteractableMaterial material = _info.collider.GetComponent<InteractableMaterial>();

		if(material == null) return;

		MaterialID materialID = material.ID;

		EmitParticleEffectFromMaterialID(materialID, _info);
		EmitSoundEffectFromMaterialID(materialID);
	}

	/// <summary>Populates Dictionary.</summary>
	/// <param name="matrix">Matrix' of Pair of MaterialIDs and CollectionIndices.</param>
	/// <param name="dictionary">Dictionary's Reference to Populate.</param>
	private void PopulateDictionary(MaterialIDIntPair[] matrix, ref Dictionary<MaterialID, List<int>> dictionary)
	{
		dictionary = new Dictionary<MaterialID, List<int>>();
		List<int> indices = null;
		MaterialID pairID = (MaterialID)(0);

		for(int i = 0; i < 31; i++)
		{
			pairID = (MaterialID)(1 << i);
			indices = new List<int>();
			
			foreach(MaterialIDIntPair pair in matrix)
			{
				if((pair.ID | pairID) == pair.ID) indices.Add(pair.index);
			}

			if(indices.Count > 0) dictionary.Add(pairID, indices);
		}
	}

	/// <summary>Emits Particle Effect from given InteractableMaterial's ID.</summary>
	/// <param name="_ID">InteractableMaterial's ID.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void EmitParticleEffectFromMaterialID(MaterialID _ID, Trigger2DInformation _info)
	{
		if(!particleEffectsDictionary.ContainsKey(_ID)) return;

		foreach(int index in particleEffectsDictionary[_ID])
		{
			PoolManager.RequestParticleEffect(index, _info.contactPoint, /*Quaternion.LookRotation(_info.direction)*/Quaternion.identity);
		}
	}

	/// <summary>Emits Sound Effect from given InteractableMaterial's ID.</summary>
	/// <param name="_ID">InteractableMaterial's ID.</param>
	/// <param name="_info">Trigger2D's Information.</param>
	public void EmitSoundEffectFromMaterialID(MaterialID _ID)
	{
		if(!soundEffectsDictionary.ContainsKey(_ID)) return;

		foreach(int index in soundEffectsDictionary[_ID])
		{
			AudioController.PlayOneShot(SourceType.SFX, 0, index);
		}
	}

	/// <returns>String Representing this InteractableMaterial.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Interactable Material: Material ID = ");
		builder.Append(ID.ToString());

		return builder.ToString();
	}
}
}