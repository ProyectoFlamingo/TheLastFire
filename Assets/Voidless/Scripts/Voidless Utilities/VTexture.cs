using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless
{
public static class VTexture
{
	/// <summary>Converts Sprite to Texture2D.</summary>
	/// <param name="_sprite">Sprite to copy pixels from.</param>
	/// <returns>Texture2D with Sprite's pixels.</returns>
	public static Texture2D ToTexture(this Sprite _sprite)
	{
		Texture2D newTexture = new Texture2D((int)_sprite.rect.width, (int)_sprite.rect.height);

		Color[] pixels = _sprite.texture.GetPixels
		( 
			(int)_sprite.textureRect.x, 
            (int)_sprite.textureRect.y, 
            (int)_sprite.textureRect.width, 
            (int)_sprite.textureRect.height
        );

        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
	}

	/// <summary>Marks Texture as readable.</summary>
	/// <param name="_texture">Texture to modify.</param>
	/// <param name="_readable">Mark as Readable? true by default.</param>
	public static void SetTextureImporterFormat(this Texture2D _texture, bool isReadable = true)
	{
#if UNITY_EDITOR
	    if (null == _texture) return;

	    string path = AssetDatabase.GetAssetPath(_texture);
	    TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
	    
	    if (importer != null)
	    {
	        importer.textureType = TextureImporterType.Advanced;

	        importer.isReadable = isReadable;

	        AssetDatabase.ImportAsset(path);
	        AssetDatabase.Refresh();
	    }
#endif
	}

	/// <summary>Interpolates 2 Texture2Ds.</summary>
	/// <param name="a">Texture A.</param>
	/// <param name="b">Texture B.</param>
	/// <param name="duration">Interpolation Duration.</param>
	/// <param name="w">Width.</param>
	/// <param name="h">Height.</param>
	/// <param name="onInterpolationEnds">Optional Callback invoked when the interpolation ends.</param>
	/// <param name="f">Optional Normalized Time t function.</param>
	public static IEnumerator<Texture2D> InterpolateToTexture2D(this Texture2D a, Texture2D b, float duration, int w, int h, Action onInterpolationEnds = null, Func<float, float> f = null)
	{
		int lengthA = a.GetPixels().Length;
		int lengthB = b.GetPixels().Length;

		if(lengthA != lengthB) yield break;

		Texture2D texture = new Texture2D(w, h);
		Color[] pixelsA = a.GetPixels();
		Color[] pixelsB = b.GetPixels();
		Color[] pixels = new Color[lengthA];
		float t = 0.0f;
		float inverseDuration = 1.0f / duration;

		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;

		yield return a;

		while(t < 1.0f)
		{
			for(int i = 0; i < lengthA; i++)
			{
				pixels[i] = Color.Lerp(pixelsA[i], pixelsB[i], f(t));
			}

			Debug.Log("[VTexture] Pixel at " + f(t) + ": " + pixels[0].ToString());

			texture.SetPixels(pixels);
			texture.Apply();

			t += (Time.deltaTime * inverseDuration);
			yield return texture;
		}

		yield return b;

		/*SecondsDelayWait wait = new SecondsDelayWait(1.0f);
		while(wait.MoveNext()) yield return b;

		yield return a;*/
	}										
}
}