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
public enum EventType
{
	Question,
	Show,
	MiniGame,
	Random
}

public class OxfordTheFoxFlowSystemController : Singleton<OxfordTheFoxFlowSystemController>
{
	[SerializeField] private OxfordTheFoxBoss _oxfordTheFox; 							/// <summary>Oxford the Boss' Reference.</summary>
	[Space(5f)]
	[SerializeField] private StringFloatDictionary _floatProperties; 					/// <summary>Float Properties [for Graph's Blackboard].</summary>
	[SerializeField] private RandomDistributionSystem _distributionSystem; 				/// <summary>Random's Distribution System.</summary>
	[Space(5f)]
	[Header("Graph Containers:")]
	[SerializeField] private FlowVisualGraphMonoBehaviour _dialogueGraphContainer; 		/// <summary>FlowVisualGraphMonoBehaviour's Component that contains the Dialogue's Graph.</summary>
	[SerializeField] private FlowVisualGraphMonoBehaviour _showGraphContainer; 			/// <summary>FlowVisualGraphMonoBehaviour's Component that contains the Show's Graph.</summary>
	[SerializeField] private FlowVisualGraphMonoBehaviour _miniGameGraphContainer; 		/// <summary>FlowVisualGraphMonoBehaviour's Component that contains the Mini-Game's Graph.</summary>
	[SerializeField] private FlowVisualGraphMonoBehaviour _randomEventGraphContainer; 	/// <summary>FlowVisualGraphMonoBehaviour's Component that contains the Random Event's Graph.</summary>
	[Space(5f)]
	[Header("Dialogue's Attributes:")]
	[SerializeField] private float _textWriteSpeed; 									/// <summary>Text's Writting Speed.</summary>
	[SerializeField] private float _waitAfterTextIsWritten; 							/// <summary>Additional Wait after the text is written before doing the next step.</summary>
	private Coroutine graphCoroutine; 													/// <summary>Graph's Coroutine.</summary>
	private bool _skipDialogue; 														/// <summary>Skip Dialogue?.</summary>

#region Getters/Setters:
	/// <summary>Gets oxfordTheFox property.</summary>
	public OxfordTheFoxBoss oxfordTheFox { get { return _oxfordTheFox; } }

	/// <summary>Gets and Sets floatProperties property.</summary>
	public StringFloatDictionary floatProperties
	{
		get { return _floatProperties; }
		set { _floatProperties = value; }
	}

	/// <summary>Gets and Sets distributionSystem property.</summary>
	public RandomDistributionSystem distributionSystem
	{
		get { return _distributionSystem; }
		set { _distributionSystem = value; }
	}

	/// <summary>Gets and Sets skipDialogue property.</summary>
	public bool skipDialogue
	{
		get { return _skipDialogue; }
		set { _skipDialogue = value; }
	}

	/// <summary>Gets dialogueGraphContainer property.</summary>
	public FlowVisualGraphMonoBehaviour dialogueGraphContainer { get { return _dialogueGraphContainer; } }

	/// <summary>Gets showGraphContainer property.</summary>
	public FlowVisualGraphMonoBehaviour showGraphContainer { get { return _showGraphContainer; } }

	/// <summary>Gets miniGameGraphContainer property.</summary>
	public FlowVisualGraphMonoBehaviour miniGameGraphContainer { get { return _miniGameGraphContainer; } }

	/// <summary>Gets randomEventGraphContainer property.</summary>
	public FlowVisualGraphMonoBehaviour randomEventGraphContainer { get { return _randomEventGraphContainer; } }

	/// <summary>Gets textWriteSpeed property.</summary>
	public float textWriteSpeed { get { return _textWriteSpeed; } }

	/// <summary>Gets waitAfterTextIsWritten property.</summary>
	public float waitAfterTextIsWritten { get { return _waitAfterTextIsWritten; } }
#endregion

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		DialogueGUIController.Instance.skipButton.button.onClick.AddListener(OnSkipSelected);
		UpdateBlackboardProperties();
		Game.AddTargetToCamera(DialogueGUIController.Instance.GetComponent<VCameraTarget>());
		ResourcesManager.onResourcesLoaded += OnResourcesLoaded;
	}

	/// <summary>OxfordTheFoxFlowSystemController's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		skipDialogue = false;
		Reset();
	}

	/// <summary>Resets values.</summary>
	private void Reset()
	{
		/*foreach(string key in floatProperties.Keys)
		{
			floatProperties[key] = 0.0f;
		}*/
	}

	/// <summary>Callback invoked when OxfordTheFoxFlowSystemController's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		ResourcesManager.onResourcesLoaded -= OnResourcesLoaded;
	}

	/// <summary>Callback invoked when the resorces are loaded.</summary>
	private void OnResourcesLoaded()
	{
		StartCoroutine(IterateTroughDialogueGraph());
	}

	/// <returns>Random EventType.</returns>
	private EventType GetRandomEventType()
	{
		int index = Mathf.Min(distributionSystem.GetRandomIndex(), 3);
		return (EventType)(index);
	}

	public void CreateNewFlowEvent()
	{
		IEnumerator graphIteration = null;

		switch(GetRandomEventType())
		{
			case EventType.Question:
				graphIteration = IterateTroughDialogueGraph();
			break;

			case EventType.Show:
				graphIteration = IterateTroughShowGraph();
			break;

			case EventType.MiniGame:
				graphIteration = IterateTroughMiniGameGraph();
			break;

			case EventType.Random:
				graphIteration = IterateTroughRandomEventGraph();
			break;
		}

		this.StartCoroutine(graphIteration, ref graphCoroutine);
	}

	/// <summary>Updates Blackboard Properties to all Graph Containers.</summary>
	public void UpdateBlackboardProperties()
	{
		foreach(KeyValuePair<string, float> property in floatProperties)
		{
			dialogueGraphContainer.SetGraphBlackboardPropertyValue(property.Key, property.Value);
			showGraphContainer.SetGraphBlackboardPropertyValue(property.Key, property.Value);
			miniGameGraphContainer.SetGraphBlackboardPropertyValue(property.Key, property.Value);
			randomEventGraphContainer.SetGraphBlackboardPropertyValue(property.Key, property.Value);
		}

		dialogueGraphContainer.UpdateProperties();
		showGraphContainer.UpdateProperties();
		miniGameGraphContainer.UpdateProperties();
		randomEventGraphContainer.UpdateProperties();
	}

	/// <summary>Callback invoked when a choice is made.</summary>
	/// <param name="_choiceNode">Choice Node associated with the button that invoked the callback.</param>
	private void OnChoiceSelected(ChoiceFlowVisualGraphNode _choiceNode)
	{
		FlowVisualGraph graph = dialogueGraphContainer.Graph;
		DialogueGUIController UI = DialogueGUIController.Instance;
		
		UI.ActivateButtons(false);
		UI.dialogueBoxContainer.gameObject.SetActive(false);
		UI.choicesContainer.gameObject.SetActive(false);
		dialogueGraphContainer.currentNode = _choiceNode.GetFittestChild();
		dialogueGraphContainer.ApplyFloatPropertyModiffiers(_choiceNode.floatPropertiyModiffiers);
		//_choiceNode.ApplyFloatPropertyModiffiers(graph);

		this.StartCoroutine(IterateTroughDialogueGraph(), ref graphCoroutine);
	}

	/// <summary>Callbaack invoked when the Skip button is selected.</summary>
	private void OnSkipSelected()
	{
		skipDialogue = true;
	}

	/// <summary>Iterates through Dialogue's Graph.</summary>
	private IEnumerator IterateTroughDialogueGraph()
	{
		DialogueGUIController UI = DialogueGUIController.Instance;
		FlowVisualGraph graph = dialogueGraphContainer.Graph;

		if(graph == null || UI == null)
		{
			Debug.Log("[DialogueSystemController] No Graph found on container! (that, or there was no DialogueGUIController...)");
			yield break;
		}

		LinkedList<FlowVisualGraphNode> nodes = dialogueGraphContainer.currentNodes;
		FlowVisualGraphNode node = dialogueGraphContainer.currentNode;
		bool condition = true;

		skipDialogue = false;
		UI.gameObject.SetActive(true);
		UI.dialogueBoxContainer.gameObject.SetActive(false);
		UI.choicesContainer.gameObject.SetActive(false);

		yield return null;

		//UpdateBlackboardProperties();

		while(condition && node != null)
		{
			switch(node.nodeType)
			{
				case FlowNodeType.Dialogue:
					//Debug.Log("[OxfordTheFoxFlowSystemController] Processig a Dialogue's Node...");
					DialogueFlowVisualGraphNode dialogueNode = node as DialogueFlowVisualGraphNode;
					
					//if(dialogueNode.played && dialogueNode.playOnce) break;

					IEnumerator dialogueIteration = DialogueIteration(dialogueNode);

					UI.dialogueBoxContainer.gameObject.SetActive(true);

					while(dialogueIteration.MoveNext()) yield return null;

					UI.dialogueBoxContainer.gameObject.SetActive(false);
					nodes = node.GetChildrenNodesList();
				break;

				case FlowNodeType.ChoicesContainer:
					//Debug.Log("[OxfordTheFoxFlowSystemController] Processig a ChoicesContainer's Node...");
					ChoicesContainerFlowVisualGraphNode choicesContainer = node as ChoicesContainerFlowVisualGraphNode;
					List<ChoiceFlowVisualGraphNode> choices = choicesContainer.GetChoices(graph);
					ChoiceFlowVisualGraphNode choice = null;
					int length = Mathf.Min(choices.Count, UI.choicesButtons.Length);
					WorldSpaceButton wsButton = null;
					Button button = null;
					Text buttonText = null;

					UI.choicesContainer.SetActive(true);
					UI.ActivateButtons(false);

					for(int i = 0; i < length; i++)
					{
						choice = choices[i];
						wsButton = UI.choicesButtons[i];
						button = wsButton.button;
						wsButton.gameObject.SetActive(true);
						buttonText = wsButton.text;

						buttonText.text = choice.choice;
						button.onClick.AddListener(choice.InvokeSelectionEvent);
						choice.onChoiceSelected = OnChoiceSelected;				
					}

					condition = false;
				break;

				case FlowNodeType.Interrupt:
				break;

				case FlowNodeType.Choice:
				case FlowNodeType.Multimedia:
				case FlowNodeType.MiniGame:
				case FlowNodeType.FloatPropertyEvaluation:
				case FlowNodeType.Selector:
				case FlowNodeType.Sequencer:
					Debug.Log("[OxfordTheFoxFlowSystemController] Not making an explicit flow for " + node.nodeType.ToString() +  ". Will just process it by default");
				break;
			}

			node = node.GetFittestChild(graph);
		}

		if(node == null && condition)
		{
			dialogueGraphContainer.currentNode = null;
			StartCoroutine(IterateTroughDialogueGraph());
		}
	}

	/// <summary>Dialogue's Iteration.</summary>
	/// <param name="node">Dialogue's Node that will be shown in the GUI.</param>
	private IEnumerator DialogueIteration(DialogueFlowVisualGraphNode node)
	{
		DialogueGUIController UI = DialogueGUIController.Instance;
		StringBuilder builder = new StringBuilder();
		SecondsDelayWait wait = new SecondsDelayWait(textWriteSpeed);
		string dialogue = node.dialogue;
		int i = 0;

		//Debug.Log("[OxfordTheFoxFlowSystemController] Dialogue Length: " + dialogue.Length);

		UI.speakerText.text = node.speaker;

		while(i < dialogue.Length && !skipDialogue)
		{
			while(wait.MoveNext()) yield return null;
			wait.Reset();

			char c = dialogue[i];

/// Begins Animation Evaluation:
			if(c == '<')
			{
				StringBuilder chain = new StringBuilder();
				i++;

				while(true)
				{
					//Debug.Log("[OxfordTheFoxFlowSystemController] Current Index [Animation Evaluation]: " + i);
					c = dialogue[i];
					i++;

					if(c == '>') break;
					else chain.Append(c);
				}

				int index = 0;

				if(Int32.TryParse(chain.ToString(), out index))
				{
					IEnumerator animationIteration = node.animations[index].Execute(oxfordTheFox.animatorController);
					while(animationIteration.MoveNext()) yield return null;
				}

				continue;
			}
/// Ends Animation Evaluation

			builder.Append(c);
			UI.dialogueText.text = builder.ToString();
			i++;
		}

		if(!skipDialogue)
		{
			wait.ChangeDurationAndReset(waitAfterTextIsWritten);
			while(wait.MoveNext()) yield return null;
		}

		//node.played = true;
		skipDialogue = false;
	}

	/// <summary>Iterates through Show's Graph.</summary>
	private IEnumerator IterateTroughShowGraph()
	{
		yield return null;
	}

	/// <summary>Iterates through Mini-Game's Graph.</summary>
	private IEnumerator IterateTroughMiniGameGraph()
	{
		yield return null;
	}

	/// <summary>Iterates through Random Event's Graph.</summary>
	private IEnumerator IterateTroughRandomEventGraph()
	{
		yield return null;
	}
}
}