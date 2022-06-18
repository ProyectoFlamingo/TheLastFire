using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
using Voidless;

namespace Flamingo
{
[NodeName("Multimedia Showcase Node")]
public class MultimediaShowcaseFlowVisualGraphNode : FlowVisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.Multimedia; } }

	[SerializeField] private StringFloatDictionary _statsModiffications; 
	[TextArea][SerializeField] public string _name; 		/// <summary>Multimedia's Name [Temporal property for debugging purposes only].</summary>

	/// <summary>Gets name property.</summary>
	public string name { get { return _name; } }
}
}