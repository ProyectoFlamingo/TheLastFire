using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Voidless;
using UnityEditor;

using Object = UnityEngine.Object;

public static class TESTFindMeHideFlags
{
	[MenuItem("Flamingo / Find Don't Save Flags on the Scene's Hierarchy")]
	public static void FindDontSaveFlagsOnSceneHierarchy()
	{
		StringBuilder builder = new StringBuilder();
		Scene scene = SceneManager.GetActiveScene();
		List<GameObject> rootObjects = new List<GameObject>();
		scene.GetRootGameObjects(rootObjects);
		
		builder.AppendLine("Don't Save Flags' Report: ");
		builder.AppendLine();

		foreach(GameObject obj in rootObjects)
		{
			GetDontSaveFlagsOnObject(obj, builder);
		}

#if UNITY_EDITOR
		Debug.Log(builder.ToString());
#endif
	}

	private static void GetDontSaveFlagsOnObject(GameObject _gameObject, StringBuilder _builder)
	{
		Component[] components = null;
		HideFlags hideFlags = default(HideFlags);

		foreach(Transform child in _gameObject.transform)
		{
			_builder.Append("GameObject ");
			_builder.AppendLine(child.gameObject.name);

			components = child.GetComponents<Component>();

			if(components != null) foreach(Component component in components)
			{
				hideFlags = component.hideFlags;

				if((hideFlags | HideFlags.DontSave) == hideFlags)
				{
					_builder.AppendLine("HERE IS SOMETHING!!!");
					_builder.Append("<color=red>");
					_builder.Append("GameObject ");
					_builder.Append(component.gameObject.name);
					_builder.Append("s Component ");
					_builder.Append(component.GetType().Name);
					_builder.Append("</color>");
					_builder.AppendLine();
				}
			}

			GetDontSaveFlagsOnObject(child.gameObject, _builder);
		}
	}
}