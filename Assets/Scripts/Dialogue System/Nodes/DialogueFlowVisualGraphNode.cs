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

	[SerializeField] private bool _playOnce; 						/// <summary>Play this dialogue just once?.</summary>
	[SerializeField] private AnimationFunctionData[] _animations; 	/// <summary>Possible Animations' Data to play while the dialogue is happening.</summary>
	[SerializeField] private string _speaker; 						/// <summary>Dialogue's Speaker.</summary>
	[TextArea][SerializeField] public string _dialogue; 			/// <summary>Dialogue's text.</summary>
	private bool _played; 											/// <summary>Has this Dialogued been already played?.</summary>

	/// <summary>Gets playOnce property.</summary>
	public bool playOnce { get { return _playOnce; } }

	/// <summary>Gets animations property.</summary>
	public AnimationFunctionData[] animations { get { return _animations; } }

	/// <summary>Gets speaker property.</summary>
	public string speaker { get { return _speaker; } }

	/// <summary>Gets dialogue property.</summary>
	public string dialogue { get { return _dialogue; } }

	/// <summary>Gets and Sets played property.</summary>
	public bool played
	{
		get { return _played; }
		set { _played = value; }
	}

	/// <summary>Resets FlowVisualGraphNode's instance to its default values.</summary>
	public override void Reset()
	{
		base.Reset();
		played = false;
	}

	/*/// <summary>Gets fittest child node.</summary>
	/// <param name="_grapg">Visual Graph's Reference.</param>
	/// <returns>Fittest Node.</returns>
	public override FlowVisualGraphNode GetFittestChild(VisualGraph _graph)
	{
		FlowVisualGraphNode child = base.GetFittestChild(_graph);

		if(played && playOnce)
		{
			while(child != null)
			{
				if(child.nodeType == FlowNodeType.Dialogue)
				{
					DialogueFlowVisualGraphNode node = child as DialogueFlowVisualGraphNode;
					if(!played || !playOnce) return child;				
				}

				child = child.GetFittestChild(_graph);
			}
		}

		return child;
	}*/
}
}