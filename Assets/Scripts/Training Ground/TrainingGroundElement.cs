using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class TrainingGroundElement : MonoBehaviour
{
	[SerializeField] private VAssetReference _worldSpaceTextReference; 				/// <summary>World-Space Text's Reference.</summary>
	[SerializeField] private Vector3 _textPositionOffset; 			/// <summary>Text's Position's Offset.</summary>
	[TextArea(1, 5)] [SerializeField] private string _text; 		/// <summary>Default Text.</summary>
	private WorldSpaceText _worldSpaceText; 						/// <summary>World-Space's Text.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] protected Color gizmosColor; 					/// <summary>Gizmos' Color.</summary>
	[SerializeField] protected float gizmosRadius; 					/// <summary>Gizmos' Radius.</summary>
#endif

	/// <summary>Gets worldSpaceTextReference property.</summary>
	public VAssetReference worldSpaceTextReference { get { return _worldSpaceTextReference; } }

	/// <summary>Gets textPositionOffset property.</summary>
	public Vector3 textPositionOffset { get { return transform.position + _textPositionOffset; } }

	/// <summary>Gets text property.</summary>
	public string text { get { return _text; } }

	/// <summary>Gets and Sets worldSpaceText property.</summary>
	public WorldSpaceText worldSpaceText
	{
		get { return _worldSpaceText; }
		protected set { _worldSpaceText = value; }
	}

#if UNITY_EDITOR
	/// <summary>Resets TrainingGroundElement's instance to its default values.</summary>
	private void Reset()
	{
		gizmosColor = Color.magenta.WithAlpha(0.5f);
		gizmosRadius = 0.25f;
	}

	/// <summary>Draws Gizmos on Editor mode when TrainingGroundElement's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = gizmosColor;

		Vector3 offset = _textPositionOffset;

		Gizmos.DrawSphere(textPositionOffset, gizmosRadius);
		/*Gizmos.DrawRay(transform.position, Vector3.right * offset.x);
		Gizmos.DrawRay(transform.position, Vector3.up * offset.y);
		Gizmos.DrawRay(transform.position, Vector3.forward * offset.z);*/
	}
#endif

	/// <summary>TrainingGroundElement's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		worldSpaceText = PoolManager.RequestPoolGameObject(worldSpaceTextReference, textPositionOffset, Quaternion.identity) as WorldSpaceText;

		if(worldSpaceText != null) UpdateText();
	}

	/// <summary>Updates Text.</summary>
	protected virtual void UpdateText()
	{
		worldSpaceText.text.text = text;
	}
}
}