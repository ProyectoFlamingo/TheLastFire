using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
using Voidless;

namespace Flamingo
{
[Serializable]
public struct ChoiceData
{
	public FloatPropertyModifier[] floatPropertiyModiffiers; 	/// <summary>Float Properties' Modiffiers.</summary> 
	[TextArea] public string choice; 							/// <summary>Choice.</summary>	
}

/// <summary>Event invoked when a Choice node is selected.</summary>
/// <param name="choice">Node selected.</param>
public delegate void OnChoiceSelected(ChoiceFlowVisualGraphNode choice);

[NodeName("Choice Node")]
[PortCapacity(PortCapacityAttribute.Capacity.Single)]
public class ChoiceFlowVisualGraphNode : FlowVisualGraphNode
{
	public OnChoiceSelected onChoiceSelected; 										/// <summary>OnChoiceSelected's event delegate.</summary>

	/// <summary>Gets Flow Node's Type.</summary>
	public override FlowNodeType nodeType { get { return FlowNodeType.Choice; } }

	//[SerializeField] private ChoiceData[] _choices; 								/// <summary>Possible Choices.</summary>
	[SerializeField] private FloatPropertyModifier[] _floatPropertiyModiffiers; 	/// <summary>Float Properties' Modiffiers.</summary> 
	[TextArea][SerializeField] public string _choice; 								/// <summary>Choice.</summary>

	/*/// <summary>Gets choices property.</summary>
	public ChoiceData[] choices { get { return _choices; } }*/

	/// <summary>Gets floatPropertiyModiffiers property.</summary>
	public FloatPropertyModifier[] floatPropertiyModiffiers { get { return _floatPropertiyModiffiers; } }

	/// <summary>Gets choice property.</summary>
	public string choice { get { return _choice; } }

	/// <summary>Applies modifications from given VisualGraph.</summary>
	/// <param name="_graph">VisualGraph's reference.</param>
	public void ApplyFloatPropertyModiffiers(VisualGraph _graph)
	{
		foreach(FloatPropertyModifier modifier in floatPropertiyModiffiers)
		{
			modifier.ApplyModifier(_graph);
		}
	}

	/// <returns>Gets Next Dialogue's Node.</returns>
	public DialogueFlowVisualGraphNode GetNextDialogueNode()
	{
		foreach(DialogueFlowVisualGraphNode node in GetChildrenNodes<DialogueFlowVisualGraphNode>())
		{
			if(node != null) return node;
		}

		return null;
	}

	/// <summary>Invokes selection event.</summary>
	public void InvokeSelectionEvent()
	{
		if(onChoiceSelected != null) onChoiceSelected(this);
	}
}
}