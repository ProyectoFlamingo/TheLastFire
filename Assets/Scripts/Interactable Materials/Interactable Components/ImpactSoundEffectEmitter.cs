using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Serializable] public class MaterialIDSoundEffectEmissionDataDictionary : SerializableDictionary<MaterialID, SoundEffectEmissionData[]> { /*...*/ }

public class ImpactSoundEffectEmitter : ImpactEffectEmitter
{
	[SerializeField] private MaterialIDSoundEffectEmissionDataDictionary _interactionMatrix; 	/// <summary>Interaction's Matrix.</summary>
	
	/// <summary>Gets and Sets interactionMatrix property.</summary>
	public MaterialIDSoundEffectEmissionDataDictionary interactionMatrix
	{
		get { return _interactionMatrix; }
		set { _interactionMatrix = value; }
	}

	/// <summary>Emits Effect.</summary>
	/// <param name="ID">Material ID of the GameObject that this GameObject impacted with.</param>
	/// <param name="t">Normalized t value [calculated from the displacement velocity and the range].</param>
	protected override void Emit(MaterialID ID, float t)
	{
		SoundEffectEmissionData[] data = null;

		if(!interactionMatrix.TryGetValue(ID, out data)) return;

		foreach(SoundEffectEmissionData emissionData in data)
		{
			SoundEffectEmissionData copy = emissionData;	
			copy.volume = t;
			copy.Play();
		}
	}
}
}