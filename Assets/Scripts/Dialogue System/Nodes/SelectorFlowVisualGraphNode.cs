using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace Flamingo
{
[NodeName("Selector Flow Node")]
public class SelectorFlowVisualGraphNode : FlowVisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.Selector; } }

	/// <summary>Operates Node and returns a result.</summary>
	/// <param name="_graph">Graph's reference.</param>
	/// <returns>Result.</returns>
	public override bool Operate(VisualGraph _graph)
	{
		foreach(FlowVisualGraphNode child in GetChildrenNodes())
		{
			if(child.Operate(_graph)) return true;
		}

		return false;
	}

	/// <summary>Gets fittest child node.</summary>
	/// <param name="_graph">VisualGraph's Reference.</param>
	/// <returns>Fittest Node.</returns>
	public override FlowVisualGraphNode GetFittestChild(VisualGraph _graph)
	{
		foreach(FlowVisualGraphNode child in GetChildrenNodes())
		{
			if(child.Operate(_graph)) return child;
		}

		return null;
	}
}
}