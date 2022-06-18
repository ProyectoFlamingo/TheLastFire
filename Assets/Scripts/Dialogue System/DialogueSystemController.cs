using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;
using VisualGraphRuntime;

namespace Flamingo
{
[RequireComponent(typeof(FlowVisualGraphMonoBehaviour))]
public class DialogueSystemController : Singleton<DialogueSystemController>
{
	[SerializeField] private StringFloatDictionary _statsMapping; 			/// <summary>Stats' Mapping.</summary>
	[SerializeField] private RandomDistributionSystem _distributionSystem; 	/// <summary>Random's Distribution System.</summary>
	[Space(5f)]
	[Header("TEST UI:")]
	[SerializeField] private GameObject dialogueBoxContainer; 				/// <summary>Dialogue Box's Container.</summary>
	[SerializeField] private GameObject optionsButtonsContainer; 			/// <summary>Buttons' Container.</summary>
	[SerializeField] private Text dialogueBox; 								/// <summary>Dialogue Box's Text.</summary>
	[SerializeField] private Text[] buttonsTexts; 							/// <summary>Buttons' Texts.</summary>
	[Space(5f)]
	[Header("TESTING (Non-Dialogue-System Related):")]
	[SerializeField] private FuzzySystem _fuzzySystem; 						/// <summary>Fuzzy's Sytem.</summary>
	private FlowVisualGraphMonoBehaviour _flowVisualGraphContainer; 		/// <summary>FlowVisualGraphMonoBehaviour's Component.</summary>

	/// <summary>Gets and Sets statsMapping property.</summary>
	public StringFloatDictionary statsMapping
	{
		get { return _statsMapping; }
		set { _statsMapping = value; }
	}

	/// <summary>Gets and Sets fuzzySystem property.</summary>
	public FuzzySystem fuzzySystem
	{
		get { return _fuzzySystem; }
		set { _fuzzySystem = value; }
	}

	/// <summary>Gets and Sets distributionSystem property.</summary>
	public RandomDistributionSystem distributionSystem
	{
		get { return _distributionSystem; }
		set { _distributionSystem = value; }
	}

	/// <summary>Gets flowVisualGraphContainer Component.</summary>
	public FlowVisualGraphMonoBehaviour flowVisualGraphContainer
	{ 
		get
		{
			if(_flowVisualGraphContainer == null) _flowVisualGraphContainer = GetComponent<FlowVisualGraphMonoBehaviour>();
			return _flowVisualGraphContainer;
		}
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		StartCoroutine(TEST());
	}

	/// <summary>Resets values.</summary>
	private void Reset()
	{
		foreach(string key in statsMapping.Keys)
		{
			statsMapping[key] = 0.0f;
		}
	}

	/// <returns>Random EventType.</returns>
	private EventType GetRandomEventType()
	{
		int index = Mathf.Min(distributionSystem.GetRandomIndex(), 3);
		return (EventType)(index);
	}

	private IEnumerator TEST()
	{
		FlowVisualGraph graph = flowVisualGraphContainer.Graph;

		if(graph == null)
		{
			Debug.Log("[DialogueSystemController] No Graph!");
			yield break;
		}

		VisualGraphNode startingNode = graph.StartingNode;
		FlowVisualGraphNode node = startingNode.Ports[0].Connections[0].Node as FlowVisualGraphNode;
		List<FlowVisualGraphNode> nodes = null;

		while(node != null)
		{
			switch(node.nodeType)
			{
				case FlowNodeType.Dialogue:
				Debug.Log("[DialogueSystemController] Dialogue First " + Time.time);
					DialogueFlowVisualGraphNode dialogueNode = node as DialogueFlowVisualGraphNode;
					string dialogue = dialogueNode.dialogue;
					float typingSpeed = 0.1f;
					SecondsDelayWait wait  = new SecondsDelayWait(typingSpeed);
					StringBuilder builder = new StringBuilder();
					int i = 0;

					dialogueBoxContainer.SetActive(true);
					optionsButtonsContainer.SetActive(false);

					while(i < dialogue.Length)
					{
						while(wait.MoveNext()) yield return null;
						wait.Reset();
						builder.Append(dialogue[i]);
						dialogueBox.text = builder.ToString();
						Debug.Log("[DialogueSystemController] i: " + i);
						i++;
					}
				break;

				case FlowNodeType.Choice:
					//ChoiceFlowVisualGraphNode choiceNode
				break;
			}

			Debug.Log("[DialogueSystemController] Buttons Latter " + Time.time);
			nodes = new List<FlowVisualGraphNode>();

			foreach(VisualGraphPort port in node.Outputs)
			{
				foreach(VisualGraphPort.VisualGraphPortConnection connection in port.Connections)
				{
					nodes.Add(connection.Node as FlowVisualGraphNode);
				}
			}

			int j = 0;
			dialogueBoxContainer.SetActive(false);
			optionsButtonsContainer.SetActive(true);

			foreach(FlowVisualGraphNode n in nodes)
			{
				switch(n.nodeType)
				{
					case FlowNodeType.Choice:
						ChoiceFlowVisualGraphNode choiceNode = n as ChoiceFlowVisualGraphNode;
						buttonsTexts[j].text = choiceNode.choice;
						j++;
					break;
				}
			}

			node = null;
			yield return null;
		}

		Debug.Log("[DialogueSystemController] No Node!");
	}
}
}