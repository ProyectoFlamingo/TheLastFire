using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[Serializable] public class MaterialIDParticleEffectEmissionDataDictionary : SerializableDictionary<MaterialID, ParticleEffectEmissionData> { /*...*/ }

public class ImpactParticleEffectEmitter : ImpactEffectEmitter
{
	[SerializeField] private MaterialIDParticleEffectEmissionDataDictionary _interactionMatrix; 	/// <summary>Interaction's Matrix.</summary>
	
	/// <summary>Gets and Sets interactionMatrix property.</summary>
	public MaterialIDParticleEffectEmissionDataDictionary interactionMatrix
	{
		get { return _interactionMatrix; }
		set { _interactionMatrix = value; }
	}

	/// <summary>Emits Effect.</summary>
	/// <param name="ID">Material ID of the GameObject that this GameObject impacted with.</param>
	/// <param name="t">Normalized t value [calculated from the displacement velocity and the range].</param>
	protected override void Emit(MaterialID ID, float t)
	{
		ParticleEffectEmissionData data = null;

		if(!interactionMatrix.TryGetValue(ID, out data)) return;

		data.EmitParticleEffects(t);
	}
}
}