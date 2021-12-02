using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(SteeringVehicle2D))]
public class AIContactWeapon : MonoBehaviour
{
	[SerializeField] private ContactWeapon _weapon; 							/// <summary>ContactWeapon's Component.</summary>
	[SerializeField] private Animator _animator; 								/// <summary>Animator's Component.</summary>
	[SerializeField] private VAnimatorController _animatorController; 			/// <summary>VAnimatorController's Component.</summary>
	[SerializeField] private AnimationEventInvoker _animationsEventInvoker; 	/// <summary>AnimationEventInvoker's Component.</summary>
	private SteeringVehicle2D _vehicle; 										/// <summary>SteeringVehicle2D's Component.</summary>

	/// <summary>Gets and Sets weapon property.</summary>
	public ContactWeapon weapon
	{
		get { return _weapon; }
		set { _weapon = value; }
	}

	/// <summary>Gets and Sets animator property.</summary>
	public Animator animator
	{
		get { return _animator; }
		set { _animator = value; }
	}

	/// <summary>Gets and Sets animatorController property.</summary>
	public VAnimatorController animatorController
	{
		get { return _animatorController; }
		set { _animatorController = value; }
	}

	/// <summary>Gets and Sets animationsEventInvoker property.</summary>
	public AnimationEventInvoker animationsEventInvoker
	{
		get { return _animationsEventInvoker; }
		set { _animationsEventInvoker = value; }
	}

	/// <summary>Gets vehicle Component.</summary>
	public SteeringVehicle2D vehicle
	{ 
		get
		{
			if(_vehicle == null) _vehicle = GetComponent<SteeringVehicle2D>();
			return _vehicle;
		}
	}
}
}