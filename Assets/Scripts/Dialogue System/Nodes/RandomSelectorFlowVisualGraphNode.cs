using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using VisualGraphRuntime;

namespace Flamingo
{
[NodeName("Random Selector Flow Node")]
public class RandomSelectorFlowVisualGraphNode : SelectorFlowVisualGraphNode
{
	/// <summary>Gets fittest child node.</summary>
	/// <param name="_graph">VisualGraph's Reference.</param>
	/// <returns>Fittest Node.</returns>
	public override FlowVisualGraphNode GetFittestChild(VisualGraph _graph)
	{
		return GetChildrenNodes().RandomElement();
	}
}
}