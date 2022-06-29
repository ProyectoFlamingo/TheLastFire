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
}
}