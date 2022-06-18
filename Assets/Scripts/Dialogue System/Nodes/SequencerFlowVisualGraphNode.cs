using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace Flamingo
{
[NodeName("Sequencer Flow Node")]
public class SequencerFlowVisualGraphNode : FlowVisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.Sequencer; } }

	/// <summary>Operates Node and returns a result.</summary>
	/// <param name="_graph">Graph's reference.</param>
	/// <returns>Result.</returns>
	public override bool Operate(VisualGraph _graph)
	{
		foreach(FlowVisualGraphNode child in GetChildrenNodes())
		{
			if(child.Operate(_graph)) return false;
		}

		return true;
	}
}
}