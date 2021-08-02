﻿using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct FloatRange : IRange<float>, ISerializationCallbackReceiver
{
	[SerializeField] private float _min; 	/// <summary>Range's Minimum value.</summary>
	[SerializeField] private float _max; 	/// <summary>Range's maximum value.</summary>

	/// <summary>Gets and Sets min property.</summary>
	public float min
	{
		get { return _min; }
		set { _min = value; }
	}

	/// <summary>Gets and Sets max property.</summary>
	public float max
	{
		get { return _max; }
		set { _max = value; }
	}

	/// <summary>Implicit float value to FloatRange operator.</summary>
	public static implicit operator FloatRange(float _value) { return new FloatRange(_value); }

	/// <summary>FloatRange's Constructor.</summary>
	/// <param name="_min">Minimum's value.</param>
	/// <param name="_max">Maximum's value.</param>
	public FloatRange(float _min, float _max) : this()
	{
		min = _min;
		max = _max;
	}

	/// <summary>FloatRange's constructor.</summary>
	/// <param name="_value">Same minimum and maximum's value for the FloatRange.</param>
	public FloatRange(float _value) : this(_value, _value) { /*...*/ }

	/// <param name="t">Normalized Time t.</param>
	/// <returns>Interpolation between the Minimum and Maximum Value.</returns>
	public float Lerp(float t)
	{
		float m = Min();
		float M = Max();
		
		return m + ((M - m) * t);
	}

	/// <returns>Range's Median.</returns>
	public float GetMedian()
	{
		return (min + (GetLength() * 0.5f));
	}

	/// <returns>Range's Length.</returns>
	public float GetLength()
	{
		return (max - min/* + 1.0f*/);
	}

	/// <returns>Maximum Value.</returns>
	public float Max()
	{
		return min < max ? max : min;
	}

	/// <returns>Minimum Value.</returns>
	public float Min()
	{
		return min < max ? min : max;
	}

	/// <returns>Random value between the range [if both min and max are equal, no random calculations will be made].</returns>
	public float Random()
	{
		return min != max ? UnityEngine.Random.Range(Min(), Max()) : min;
	}

	/// <summary>Clamps given value between the FloatRange.</summary>
	/// <param name="x">Value to clamp.</param>
	/// <returns>Clamped value.</returns>
	public float Clamp(float x)
	{
		float min = Min();
		float max = Max();

		return x < min ? min : x > max ? max : x;
	}

	/// <summary>Evaluates if given value is inside the range.</summary>
	/// <param name="x">Value to evaluate.</param>
	/// <returns>True if given value is inside the range.</returns>
	public bool ValueInside(float x)
	{
		return x >= Min() && x <= Max();
	}

	/// <summary>Evaluates if given value is outside the range.</summary>
	/// <param name="x">Value to evaluate.</param>
	/// <returns>True if given value is outside the range.</returns>
	public bool ValueOutside(float x)
	{
		return x < Min() || x > Max();
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	/*min = Min();
    	max = Max();*/
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	
    }

	/// <returns>String representing Range.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("[ ");
		builder.Append("Min: ");
		builder.Append(min.ToString());
		builder.Append(", Max: ");
		builder.Append(max.ToString());
		builder.Append(", Length: ");
		builder.Append(GetLength().ToString());
		builder.Append(", Median: ");
		builder.Append(GetMedian().ToString());
		builder.Append("]");

		return builder.ToString();
	}
}
}