using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace Flamingo
{
[NodeName("Float Evaluations' Node")]
[PortCapacity(PortCapacityAttribute.Capacity.Single)]
public class FloatPropertyPredicateVisualGraphNode : FlowVisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.FloatPropertyEvaluation; } }

	[SerializeField] private FloatPropertyPredicateSet[] _predicateSets; 	/// <summary>Sets of Predicates.</summary>

	/// <summary>Gets predicateSets property.</summary>
	public FloatPropertyPredicateSet[] predicateSets { get { return _predicateSets; } }

	/// <summary>Called when created. Use this to add required ports and additional setup/// </summary>
    public override void Init()
    {
    	base.Init();

    	AddPort("Success", VisualGraphPort.PortDirection.Output);
    	AddPort("Failure", VisualGraphPort.PortDirection.Output);
    }

	/// <summary>Operates Node and returns a result.</summary>
	/// <param name="_graph">Graph's reference.</param>
	/// <returns>Result.</returns>
	public override bool Operate(VisualGraph _graph)
	{
		foreach(FloatPropertyPredicateSet predicateSet in predicateSets)
		{
			if(predicateSet.Evaluate(_graph)) return true;
		}

		return false;
	}


	/// <summary>Gets fittest child node.</summary>
	/// <param name="_grapg">Visual Graph's Reference.</param>
	/// <returns>Fittest Node.</returns>
	public override FlowVisualGraphNode GetFittestChild(VisualGraph _graph)
	{
		FloatPropertyPredicateVisualGraphNode predicate = this;
		FlowVisualGraphNode node = null;

		node = GetNodeFromPortWithName(Operate(_graph) ? "Success" : "Failure");

		if(node == null) return null;

		switch(node.nodeType)
		{
			case FlowNodeType.FloatPropertyEvaluation:
				predicate = node as FloatPropertyPredicateVisualGraphNode;
				return predicate.GetFittestChild(_graph);
			break;
		}

		return node;
	}
}
}