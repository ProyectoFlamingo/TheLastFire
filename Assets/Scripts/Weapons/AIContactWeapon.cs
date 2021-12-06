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
	private AnimationCommandState _state; 										/// <summary>Animation's State.</summary>
	private AnimationCommandState _previousState; 								/// <summary>Previous Animation's State.</summary>

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

	/// <summary>Gets and Sets state property.</summary>
	public AnimationCommandState state
	{
		get { return _state; }
		set
		{
			previousState = _state;
			_state = value;
		}
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public AnimationCommandState previousState
	{
		get { return _previousState; }
		private set { _previousState = value; }
	}
}
}