using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VDebug
{
	/// <summary>Debugs Message on given Format and Color.</summary>
	/// <param name="_message">Message to debug to the console.</param>
	/// <param name="_format">Text's Format [Normal by default].</param>
	/// <param name="_color">Text's Color [Black by default].</param>
	public static string Log(string _message, Format _format = Format.Normal, Color _color = default(Color))
	{
		string text = string.Empty;

#if UNITY_EDITOR
		StringBuilder builder = new StringBuilder();
		if(_color == default(Color)) _color = Color.gray;

		builder.Append("<color=#");
		builder.Append(ColorUtility.ToHtmlStringRGBA(_color));
		builder.Append(">");
		if((_format | Format.Bold) == _format) builder.Append("<b>");
		if((_format | Format.Italic) == _format) builder.Append("<i>");
		builder.Append(_message);
		if((_format | Format.Bold) == _format) builder.Append("</b>");
		if((_format | Format.Italic) == _format) builder.Append("</i>");
		builder.Append("</color>");

		text = builder.ToString();
		Debug.Log(text);
#endif

		return text;
	}

	/// <summary>Debug Log, but without concatenation.</summary>
	/// <param name="data">Data represented as object [to use each object's ToString()], each data is separated with a space.</param>
	/// <returns>Debugged Text.</returns>
	public static string Log(params object[] data)
	{
		string text = string.Empty;

#if UNITY_EDITOR
		if(data == null) return string.Empty;

		StringBuilder builder = new StringBuilder();

		foreach(object obj in data)
		{
			builder.Append(obj.ToString());
		}

		text = builder.ToString();

		Debug.Log(text);
#endif

		return text;
	}
}
}