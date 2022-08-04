using System;
using UnityEngine;
using UnityEngine.UI;

namespace Flamingo
{
[Serializable]
public struct ImageSettings
{
	public Sprite sprite; 	/// <summary>Source's Image.</summary>
	public Color color; 	/// <summary>Image's Color.</summary>
	public float width; 	/// <summary>Image's Width.</summary>
	public float height; 	/// <summary>Image's Height.</summary>

	/// <summary>ImageSettings' Constructor.</summary>
	/// <param name="_sprite">Source Image.</param>
	/// <param name="_color">Image's Color.</param>
	/// <param name="_width">Image's Width.</param>
	/// <param name="_height">Image's Height.</param>
	public ImageSettings(Sprite _sprite, Color _color, float _width, float _height)
	{
		sprite = _sprite;
		color = _color;
		width = _width;
		height = _height;
	}
}
}