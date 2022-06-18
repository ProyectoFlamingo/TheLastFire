using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace Flamingo
{
public enum NumberComparison
{
	Equals,
	NotEquals,
	LessThan,
	LessOrEqualThan,
	MoreThan,
	MoreOrEqualThan
}

[Serializable]
public struct FloatPropertyPredicate
{
	public string propertyKey; 				/// <summary>Float Property's Key.</summary>
	public NumberComparison evaluation; 	/// <summary>Type of comparison.</summary>
	public float x; 						/// <summary>Parameter to evaluate agains FloatProperty's value.</summary>

	/// <summary>Evaluates Graph's Values predicate.</summary>
	/// <param name="_graph">VisualGraph's reference.</param>
	/// <returns>Evaluation.</returns>
	public bool Evaluate(VisualGraph _graph)
	{
		FloatBlackboardProperty floatProperty = null;

		if(!_graph.GetPropertyValue<FloatBlackboardProperty>(propertyKey, ref floatProperty)) return false;

		return Evaluate(floatProperty.Value);
	}

	/// <summary>Evaluates value against predicates.</summary>
	/// <param name="y">Value to evaluate.</param>
	/// <returns>Evaluation.</returns>
	public bool Evaluate(float y)
	{
		switch(evaluation)
		{
			case NumberComparison.Equals: 			return y == x;
			case NumberComparison.NotEquals: 		return y != x;
			case NumberComparison.LessThan: 		return y < x;
			case NumberComparison.LessOrEqualThan: 	return y <= x;
			case NumberComparison.MoreThan: 		return y > x;
			case NumberComparison.MoreOrEqualThan: 	return y >= x;
		}

		return false;
	}

	/// <returns>String representing this FloatPredicate.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("FloatPropertyPredicate: { ");
		builder.Append(propertyKey);
		builder.Append(" ");
		builder.Append(evaluation.ToString());
		builder.Append(" ");
		builder.Append(x.ToString());
		builder.Append(" }");

		return builder.ToString();
	}
}

[Serializable]
public struct FloatPropertyPredicateSet
{
	public FloatPropertyPredicate[] predicates;  	/// <summary>Set of Predicates.</summary>

	/// <summary>Evaluates Graph's Values against predicates.</summary>
	/// <param name="_graph">VisualGraph's reference.</param>
	/// <returns>Evaluation.</returns>
	public bool Evaluate(VisualGraph _graph)
	{
		foreach(FloatPropertyPredicate predicate in predicates)
		{
			float floatProperty = 0.0f;

			if(!_graph.GetPropertyValue<float>(predicate.propertyKey, ref floatProperty)) return false;

			float y = floatProperty;
			bool result = predicate.Evaluate(y);

/*#if UNITY_EDITOR
			StringBuilder builder = new StringBuilder();

			builder.Append("Predicate ");
			builder.Append(predicate.ToString());
			builder.Append(" result = ");
			builder.Append(result);

			Debug.Log(builder.ToString());
#endif*/

			if(!result) return false;
		}

		return  true;
	}
}
}