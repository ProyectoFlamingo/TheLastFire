using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

namespace Flamingo
{
public enum FlowNodeType
{
	None,
	Dialogue,
	ChoicesContainer,
	Choice,
	Multimedia,
	MiniGame,
	FloatPropertyEvaluation,
	Selector,
	Sequencer,
	Interrupt
}

[NodeName("Base Flow Node")]
public class FlowVisualGraphNode : VisualGraphNode
{
	/// <summary>Gets Flow Node's Type.</summary>
	public virtual FlowNodeType nodeType { get { return FlowNodeType.None; } }

	/// <summary>Resets FlowVisualGraphNode's instance to its default values.</summary>
	public virtual void Reset() { /*...*/ }

	/// <summary>Called when created. Use this to add required ports and additional setup/// </summary>
    public override void Init()
    {
    	base.Init();
    }

	/// <summary>Operates Node and returns a result.</summary>
	/// <param name="_graph">Graph's reference.</param>
	/// <returns>Result.</returns>
	public virtual bool Operate(VisualGraph _graph) { return true; }

	/// <summary>Gets Children Nodes.</summary>
	public IEnumerable<FlowVisualGraphNode> GetChildrenNodes()
	{
		FlowVisualGraphNode flowNode = null;

		foreach(VisualGraphPort port in Outputs)
		{
			foreach(VisualGraphPort.VisualGraphPortConnection connection in port.Connections)
			{
				flowNode = connection.Node as FlowVisualGraphNode;
				if(flowNode != null) yield return flowNode;
			}
		}
	}

	/// <summary>Gets children of specific Generic T type.</summary>
	public IEnumerable<T> GetChildrenNodes<T>() where T : FlowVisualGraphNode
	{
		T node = null;

		foreach(FlowVisualGraphNode child in GetChildrenNodes())
		{
			node = child as T;
			if(node != null) yield return node;
		}
	}

	/// <returns>Children Nodes' Iterator to LinkedList.</returns>
    public LinkedList<FlowVisualGraphNode> GetChildrenNodesList()
    {
    	return new LinkedList<FlowVisualGraphNode>(GetChildrenNodes());
    }

    /// <summary>Gets fittest child node.</summary>
	/// <returns>Fittest Node.</returns>
	public virtual FlowVisualGraphNode GetFittestChild() { return GetFirstChildNode(); }

	/// <summary>Gets fittest child node.</summary>
	/// <param name="_grapg">Visual Graph's Reference.</param>
	/// <returns>Fittest Node.</returns>
	public virtual FlowVisualGraphNode GetFittestChild(VisualGraph _graph) { return GetFirstChildNode(); }

	/// <returns>Firts FlowVisualGraphNode contained on this Node's Ports.</returns>
	public FlowVisualGraphNode GetFirstChildNode()
	{
		foreach(VisualGraphPort port in Outputs)
		{
			foreach(VisualGraphPort.VisualGraphPortConnection connection in port.Connections)
			{
				FlowVisualGraphNode flowNode = connection.Node as FlowVisualGraphNode;
				if(flowNode != null) return flowNode;
			}	
		}

		return null;
	}

	/// <summary>Gets Node from Port with specified name.</summary>
	/// <param name="name">Name of the Port.</param>
	/// <returns>First node found on port with given name.</returns>
    public FlowVisualGraphNode GetNodeFromPortWithName(string name)
    {
    	VisualGraphPort port = FindPortByName(name);

    	if(port == null) return null;

    	FlowVisualGraphNode flowNode = null;

    	foreach(VisualGraphPort.VisualGraphPortConnection connection in port.Connections)
		{
			flowNode = connection.Node as FlowVisualGraphNode;
			if(flowNode != null) return flowNode;
		}

		return null;
    }

    /// <summary>Gets child node on Success output port [if it exists].</summary>
    /// <returns>Child node from Success Output port.</returns>
    public FlowVisualGraphNode GetSuccessChildNode()
    {
    	return GetNodeFromPortWithName("Success");
    }

    /// <summary>Gets child node on Failure output port [if it exists].</summary>
    /// <returns>Child node from Failure Output port.</returns>
    public FlowVisualGraphNode GetFailureChildNode()
    {
    	return GetNodeFromPortWithName("Failure");
    }

    /// <returns>TEMPORAL String representing a LinkedList of Nodes.</returns>
    public static string NodeLinkedListToString(LinkedList<FlowVisualGraphNode> nodes)
    {
    	StringBuilder builder = new StringBuilder();
    	int i = 0;

    	builder.Append("Linked List: { ");

    	foreach(FlowVisualGraphNode node in nodes)
    	{
    		builder.Append(node.ToString());
    		if(i < nodes.Count - 1) builder.Append(", ");
    		i++;
    	}

    	builder.Append(" }");

    	return builder.ToString();
    }

    /// <returns>String representing this Node.</returns>
    public override string ToString()
    {
    	StringBuilder builder = new StringBuilder();
    	LinkedList<FlowVisualGraphNode> nodes = GetChildrenNodesList();

    	builder.Append("FlowVisualGraphNode = { Flow Type: ");
    	builder.Append(nodeType.ToString());
    	builder.Append(", Children: ");

    	if(nodes.Count > 0)
	    {
	    	int i = 0;

	    	builder.Append("[ ");

	    	foreach(FlowVisualGraphNode node in nodes)
	    	{
	    		builder.Append(node.nodeType.ToString());
	    		if(i < nodes.Count - 1) builder.Append(", ");
	    		i++;
	    	}

	    	builder.Append(" ]");
	    }
		else builder.Append("No Children");

		builder.Append(" }");

    	return builder.ToString();
    }
}
}