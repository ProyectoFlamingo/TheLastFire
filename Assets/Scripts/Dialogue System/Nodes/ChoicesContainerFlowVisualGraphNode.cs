using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
using Voidless;

namespace Flamingo
{
[NodeName("Choices' Container Node")]
public class ChoicesContainerFlowVisualGraphNode : FlowVisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.ChoicesContainer; } }

	/// <returns>Choices contained.</returns>
	public List<ChoiceFlowVisualGraphNode> GetChoices(VisualGraph _graph)
	{
		List<ChoiceFlowVisualGraphNode> choices = new List<ChoiceFlowVisualGraphNode>();

		foreach(FlowVisualGraphNode child in GetChildrenNodes())
		{
			switch(child.nodeType)
			{
				case FlowNodeType.Choice:
					choices.Add(child as ChoiceFlowVisualGraphNode);
				break;

				case FlowNodeType.FloatPropertyEvaluation:
					FloatPropertyPredicateVisualGraphNode predicate = child as FloatPropertyPredicateVisualGraphNode;
					FlowVisualGraphNode node = predicate.GetFittestChild(_graph);

					if(node.nodeType == FlowNodeType.Choice) choices.Add(node as ChoiceFlowVisualGraphNode);
				break;
			}
		}

		return choices.Count > 0 ? choices : null;
	}
}
}