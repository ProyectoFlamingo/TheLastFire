using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voidless;

namespace Flamingo
{
[CanEditMultipleObjects]
[CustomEditor(typeof(SelfMotionPerformer))]
public class SelfMotionPerformerInspector : Editor
{
	private SelfMotionPerformer selfMotionPerformer; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		selfMotionPerformer = target as SelfMotionPerformer;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		if(GUILayout.Button("Position Children")) selfMotionPerformer.PositionChildren();		
	}
}
}