using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
using Voidless;

namespace Flamingo
{
[NodeName("Dialogue Node")]
//[PortCapacity(PortCapacityAttribute.Capacity.Single)]
public class DialogueFlowVisualGraphNode : FlowVisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.Dialogue; } }

	[SerializeField] private bool _playOnce; 				/// <summary>Play this dialogue just once?.</summary>
	[SerializeField] private string[] _animations; 			/// <summary>Possible Animations to play while the dialogue is happening.</summary>
	[SerializeField] private string _speaker; 				/// <summary>Dialogue's Speaker.</summary>
	[TextArea][SerializeField] public string _dialogue; 	/// <summary>Dialogue's text.</summary>

	/// <summary>Gets playOnce property.</summary>
	public bool playOnce { get { return _playOnce; } }

	/// <summary>Gets speaker property.</summary>
	public string speaker { get { return _speaker; } }

	/// <summary>Gets dialogue property.</summary>
	public string dialogue { get { return _dialogue; } }
}
}