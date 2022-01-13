using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;

public class TEST_SwitchLightMaps : MonoBehaviour
{
	[SerializeField] private ScriptableLightMapData data1; 	/// <summary>Lightmap Data's 1.</summary>
	[SerializeField] private ScriptableLightMapData data2; 	/// <summary>Lightmap Data's 2.</summary>
	Texture2D texture;

#region UnityMethods:
	private void OnGUI()
	{
		if(texture == null) return;

		int size = data1.GetTextureSize();

		Rect rect  = new Rect(0, 0, size, size);
		GUI.DrawTexture(rect, texture);
	}

	/// <summary>TEST_SwitchLightMaps's instance initialization.</summary>
	private void Awake()
	{
		
	}

	/// <summary>TEST_SwitchLightMaps's starting actions before 1st Update frame.</summary>
	private IEnumerator Start ()
	{
		yield return null;

		LightmapSettings.lightmaps = new LightmapData[] { data1.ToLightmapData() };
		ScriptableLightMapData data = data2;
		int size = data1.GetTextureSize();
		Debug.Log("[TEST_SwitchLightMaps] Size: " + size);
		IEnumerator<Texture2D> colorIterator = data1.lightmapColor.InterpolateToTexture2D(data2.lightmapColor, 0.5f);
		IEnumerator<Texture2D> dirIterator = data1.lightmapDir.InterpolateToTexture2D(data2.lightmapDir, 0.5f);

		float t = 0.0f;
		float inverseDuration = 1.0f / 5.0f;

		/*while(true)
		{
			while(t < 1.0f)
			{
				t += (Time.deltaTime * inverseDuration);
				yield return null;
			}

			LightmapSettings.lightmaps = new LightmapData[] { data.ToLightmapData() };

			t = 0.0f;
			data = data == data1 ? data2 : data1;
		}*/

		LightmapData[] datas = new LightmapData[] { new LightmapData() };
		Debug.Log("[TEST_SwitchLightMaps] Current Time: " + Time.time);

		while(colorIterator.MoveNext() && dirIterator.MoveNext())
		{
			texture = colorIterator.Current;
			datas[0].lightmapColor = colorIterator.Current;
			datas[0].lightmapDir = dirIterator.Current;

			LightmapSettings.lightmaps = datas;
			
			yield return null;
		}

		Debug.Log("[TEST_SwitchLightMaps] Final Time: " + Time.time);
	}
	
	/// <summary>TEST_SwitchLightMaps's tick at each frame.</summary>
	private void Update ()
	{
		
	}
#endregion
}