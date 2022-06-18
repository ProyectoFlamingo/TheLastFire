using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;
using Voidless;
using System.Linq;

namespace Flamingo
{
public class FlowVisualGraphMonoBehaviour : VisualGraphMonoBehaviour<FlowVisualGraph>
{
	private FlowVisualGraphNode _currentNode;
	private LinkedList<FlowVisualGraphNode> _currentNodes;
	//private List<FlowVisualGraphNode> _currentNodes;

	/// <summary>Gets and Sets currentNode property.</summary>
	public FlowVisualGraphNode currentNode
	{
		get
		{
			if(_currentNode == null)
			_currentNode = Graph.StartingNode.Ports[0].Connections[0].Node as FlowVisualGraphNode;

			return _currentNode;
		}
		set { _currentNode = value; }
	}

	/// <summary>Gets and Sets currentNodes property.</summary>
	public LinkedList<FlowVisualGraphNode> currentNodes
	{
		get
		{
			if(_currentNodes == null)
			_currentNodes = new LinkedList<FlowVisualGraphNode>(GetChildrenNodes(Graph.StartingNode));

			return _currentNodes;
		}
		set { _currentNodes = value; }
	}

	/// <summary>Rests Flow Visual-Graph class.</summary>
	public void Reset()
	{
		currentNode = null;
	}

	/// <returns>Children Nodes' Iterator to a List.</returns>
	public List<FlowVisualGraphNode> GetChildrenNodesList()
	{
		return GetChildrenNodes().ToList();
	}

	/// <summary>Sets BlaackboardProperty's Value.</summary>
	/// <param name="_blackboardProperties">Blackboard Properties' List.</param>
	/// <param name="_key">BlaackboardProperty's Key.</param>
	/// <param name="_value">New value for the BlackboardProperty.</param>
	public void SetBlackboardPropertyValue<T>(List<AbstractBlackboardProperty> _blackboardProperties, string _key, T _value)
	{
		for (int i = 0; i < _blackboardProperties.Count; i++)
        {
            if(_blackboardProperties[i].Name == _key)
            {
				AbstractBlackboardProperty<T> prop = (AbstractBlackboardProperty<T>)_blackboardProperties[i];
				if (prop != null) prop.Value = _value;
				
				return;
			}
        }
        Debug.LogWarning($"Unable to find property {_key}");
	}

	/// <summary>Sets BlaackboardProperty's Value.</summary>
	/// <param name="_key">BlaackboardProperty's Key.</param>
	/// <param name="_value">New value for the BlackboardProperty.</param>
	public void SetGraphBlackboardPropertyValue<T>(string _key, T _value)
	{
		if(Graph == null || Graph.BlackboardProperties == null) return;
		SetBlackboardPropertyValue(Graph.BlackboardProperties, _key, _value);
	}

	/// <summary>Applies modifiers to BlackboardProperties.</summary>
	/// <param name="_modifiers">Modifiers to apply.</param>
	public void ApplyFloatPropertyModiffiers(FloatPropertyModifier[] _modifiers)
	{
		VisualGraph graph = Graph;

		foreach(FloatPropertyModifier modifier in _modifiers)
		{
			float y = 0.0f;

			if(!graph.GetPropertyValue<float>(modifier.propertyKey, ref y)) continue;

			float x = modifier.x;

			switch(modifier.operation)
			{
				case ArithmeticOperation.Addition:
				y += x;
				break;

				case ArithmeticOperation.Subtraction:
				y -= x;
				break;

				case ArithmeticOperation.Multiplication:
				y *= x;
				break;

				case ArithmeticOperation.Division:
				y /= x;
				break;

				case ArithmeticOperation.Set:
				y = x;
				break;
			}

			SetGraphBlackboardPropertyValue(modifier.propertyKey, y);
		}

		UpdateProperties();
		Debug.Log(ToString());
	}

	/// <summary>Gets Children Nodes.</summary>
	public IEnumerable<FlowVisualGraphNode> GetChildrenNodes(VisualGraphNode node)
	{
		FlowVisualGraphNode flowNode = null;

		foreach(VisualGraphPort port in node.Outputs)
		{
			foreach(VisualGraphPort.VisualGraphPortConnection connection in port.Connections)
			{
				flowNode = connection.Node as FlowVisualGraphNode;
				if(flowNode != null) yield return flowNode;
			}
		}
	}

	/// <summary>Gets Children Nodes.</summary>
	public IEnumerable<FlowVisualGraphNode> GetChildrenNodes()
	{
		return GetChildrenNodes(currentNode);
	}

	/// <returns>String representing this VisualGraphMonobheaviour.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Blackboard Properties: ");

		for (int i = 0; i < BlackboardProperties.Count; i++)
        {
        	AbstractBlackboardProperty<float> prop = (AbstractBlackboardProperty<float>)BlackboardProperties[i];

        	builder.Append("{ Key: ");
        	builder.Append(BlackboardProperties[i].Name);
        	builder.Append(", Value: ");
        	builder.Append(prop.Value);
        	builder.Append("} ");
        	if(i < BlackboardProperties.Count - 1) builder.Append(", ");
        }

		return builder.ToString();
	}
}
}