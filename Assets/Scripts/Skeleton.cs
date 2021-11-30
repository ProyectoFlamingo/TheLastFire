using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class Skeleton : MonoBehaviour
{
	[SerializeField] private Transform _head; 		/// <summary>Head's Transform.</summary>
	[SerializeField] private Transform _leftHand; 	/// <summary>Left Hand.</summary>
	[SerializeField] private Transform _rightHand; 	/// <summary>Right Hand.</summary>
	[SerializeField] private Transform _leftFoot; 	/// <summary>Left Foot.</summary>
	[SerializeField] private Transform _rightFoot; 	/// <summary>Right Foot.</summary>

	/// <summary>Gets head property.</summary>
	public Transform head { get { return _head; } }

	/// <summary>Gets leftHand property.</summary>
	public Transform leftHand { get { return _leftHand; } }

	/// <summary>Gets rightHand property.</summary>
	public Transform rightHand { get { return _rightHand; } }

	/// <summary>Gets leftFoot property.</summary>
	public Transform leftFoot { get { return _leftFoot; } }

	/// <summary>Gets rightFoot property.</summary>
	public Transform rightFoot { get { return _rightFoot; } }
}
}