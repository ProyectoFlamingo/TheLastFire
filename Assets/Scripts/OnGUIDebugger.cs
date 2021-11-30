using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

using Object = UnityEngine.Object;

namespace Flamingo
{
public class OnGUIDebugger : MonoBehaviour
{
	[SerializeField] private bool _show; 				/// <summary>Show OnGUI?.</summary>
	[SerializeField] private Rect _rect; 				/// <summary>OnGUI's Rect.</summary>
	[SerializeField] private MonoBehaviour[] _objects; 	/// <summary>MonoBehaviours.</summary>
	private Vector2 scrollPosition; 					/// <summary>Scroll's Position.</summary>

	/// <summary>Gets and Sets show property.</summary>
	public bool show
	{
		get { return _show; }
		private set { _show = value; }
	}

	/// <summary>Gets and Sets rect property.</summary>
	public Rect rect
	{
		get { return _rect; }
		private set { _rect = value; }
	}

	/// <summary>Gets objects property.</summary>
	public MonoBehaviour[] objects { get { return _objects; } }

	/// <summary>OnGUI is called for rendering and handling GUI events.</summary>
	private void OnGUI()
	{
		if(GUILayout.Button(show ? "Hide" : "Show")) show = !show;

		if(!show) return;

		string text = string.Empty;

		foreach(MonoBehaviour obj in objects)
		{
			text += obj.ToString();
			text += "\n";
		}

		//scrollPosition = GUI.BeginScrollView();
		GUI.Box(rect, text);
		//GUI.EndScrollView();
	}
}
}