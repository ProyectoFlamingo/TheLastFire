using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
using Voidless;

namespace Flamingo
{
[NodeName("Mini-Game Node")]
public class MiniGameFlowVisualGraphNode : FlowVisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.MiniGame; } }

	[SerializeField] private StringFloatDictionary _statsModiffications; 
	[TextArea][SerializeField] public string _name; 		/// <summary>Mini-Game's Name [Temporal property for debugging purposes only].</summary>

	/// <summary>Gets name property.</summary>
	public string name { get { return _name; } }
}
}