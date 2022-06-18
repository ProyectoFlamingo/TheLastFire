using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace Flamingo
{
public enum ArithmeticOperation
{
	Addition,
	Subtraction,
	Multiplication,
	Division,
	Set
}

[Serializable]
public struct FloatPropertyModifier
{
	public string propertyKey; 				/// <summary>Float Property's Key.</summary>
	public ArithmeticOperation operation; 	/// <summary>Arithmetic Operation to apply.</summary>
	public float x;							/// <summary>Input parameter to apply to the operation with the float property.</summary>

	/// <summary>Applies modifications from given VisualGraph.</summary>
	/// <param name="_graph">VisualGraph's reference.</param>
	public void ApplyModifier(VisualGraph _graph)
	{
		FloatBlackboardProperty floatProperty = null;

		if(!_graph.GetPropertyValue<FloatBlackboardProperty>(propertyKey, ref floatProperty)) return;

		switch(operation)
		{
			case ArithmeticOperation.Addition:
			floatProperty.Value += x;
			break;

			case ArithmeticOperation.Subtraction:
			floatProperty.Value -= x;
			break;

			case ArithmeticOperation.Multiplication:
			floatProperty.Value *= x;
			break;

			case ArithmeticOperation.Division:
			floatProperty.Value /= x;
			break;

			case ArithmeticOperation.Set:
			floatProperty.Value = x;
			break;
		}
	}
}
}