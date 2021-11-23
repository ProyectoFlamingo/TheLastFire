using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Voidless
{
[Serializable]
public class RandomDistributionSystem
{
	private const float MIN = 0.0f; 									/// <summary>Range's Minimum Value.</summary>
	private const float MAX = 1.0f; 									/// <summary>Range's Maximum Value.</summary>

	[SerializeField] private ProbabilityRange[] _probabilityRanges; 	/// <summary>Set of ProbabilityRanges.</summary>

	/// <summary>Gets and Sets probabilityRanges property.</summary>
	public ProbabilityRange[] probabilityRanges
	{
		get { return _probabilityRanges; }
		set { _probabilityRanges = value; }
	}

	/// <summary>RandomDistributionSystem default constructor.</summary>
	/// <param name="_probabilityRanges">Set of ProbabilityRanges.</param>
	public RandomDistributionSystem(ProbabilityRange[] _probabilityRanges)
	{
		probabilityRanges = _probabilityRanges;
	}

	/// <summary>Redistributes the Probability Ranges' Set.</summary>
	public void Redistribute()
	{
		float min = 0.0f;

		for(int i = 0; i < probabilityRanges.Length; i++)
		{
			ProbabilityRange probabilityRange = probabilityRanges[i];

			probabilityRange.min = Mathf.Clamp(probabilityRange.min, min, MAX);
			probabilityRange.max = Mathf.Clamp(probabilityRange.max, MIN, MAX);
			min = probabilityRange.max;

			probabilityRanges[i] = probabilityRange;
		}
	}

	/// <returns>Random Index.</returns>
	public int GetRandomIndex()
	{
		ProbabilityRange probabilityRange = default(ProbabilityRange);
		int length = probabilityRanges.Length;
		float min = Mathf.Infinity;
		float max = 0.0f;
		float random = 0.0f;

		/// Get the Minimum repetitions' value:
		for(int i = 0; i < length; i++)
		{
			probabilityRange = probabilityRanges[i];
			if(probabilityRange.repetitions < min) min = probabilityRange.repetitions;
		}

		/// Update each ProbabilityRange's scale and modify the Maximum value's reference:
		for(int i = 0; i < length; i++)
		{
			probabilityRange = probabilityRanges[i];
			probabilityRange.scale = (min + 1.0f) / (probabilityRange.repetitions + 1.0f);
			max += (probabilityRange.range.GetLength() * probabilityRange.scale);
			probabilityRanges[i] = probabilityRange;
		}

		random = Random.Range(0.0f, max);
		min = 0.0f;

		/// Evaluate if the random value is within any of the ProbabilityRanges:
		for(int i = 0; i < length; i++)
		{
			probabilityRange = probabilityRanges[i];
			float scale = probabilityRange.scale;
			max = (min + (probabilityRange.range.GetLength() * scale));

			if(random >= min && random <= max)
			{
				probabilityRange.repetitions++;
				probabilityRanges[i] = probabilityRange;
				return i;
			}
		
			min = max;
		}

		/// CONTINGENCY IN CASE THE ALGORITHM ISN'T PERFECT:
		int gayRandom = Random.Range(0, 2);

		probabilityRange =  probabilityRanges[gayRandom];
		probabilityRange.repetitions++;
		probabilityRanges[gayRandom] = probabilityRange;

		return gayRandom;
	}

	/// <returns>String representing set of Probability Ranges.</returns>
	public override string ToString()
	{
		if(probabilityRanges == null) return string.Empty;

		StringBuilder builder = new StringBuilder();
		int i = 0;
		int length = probabilityRanges.Length;

		if(length == 0) return string.Empty;

		builder.AppendLine("Set of Probability Ranges: ");
		builder.AppendLine();

		foreach(ProbabilityRange probabilityRange in probabilityRanges)
		{
			builder.Append(i.ToString());
			builder.Append(": ");
			builder.Append(probabilityRange.ToString());
			
			if(i < length) builder.AppendLine();
			i++;
		}

		return builder.ToString();
	}
}
}