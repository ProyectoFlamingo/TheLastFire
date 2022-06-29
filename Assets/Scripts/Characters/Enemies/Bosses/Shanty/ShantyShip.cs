using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimationEventInvoker))]
[RequireComponent(typeof(Animation))]
public class ShantyShip : MonoBehaviour
{
	public const int ID_ANIMATIONEVENT_SHOOTCANNONS = 0; 				/// <summary>Shoot Cannons' AnimatorEvent ID.</summary>
	public const int ID_STATE_DOCKED = 0; 								/// <summary>Docked's State ID.</summary>
	public const int ID_STATE_SAIL_0 = 1; 								/// <summary>Sail 0's State ID.</summary>
	public const int ID_STATE_CANNON = 2; 								/// <summary>Cannon's State ID.</summary>
	public const int ID_STATE_SAIL_1 = 3; 								/// <summary>Sail 1's State ID.</summary>

	[SerializeField] private AnimatorCredential _stateIDCredential; 	/// <summary>State ID's Animator Credential.</summary>
	[SerializeField] private HitCollider2D _ropeHitBox; 				/// <summary>Rope's HitBox.</summary>
	[Space(5f)]
	[SerializeField] private VAssetReference _projectileReference; 		/// <summary>Projectile's Reference.</summary>
	[SerializeField] private Transform[] _cannons; 						/// <summary>Cannons.</summary>
	[Space(5f)]
	[SerializeField] private AnimationClip dockedAnimation; 			/// <summary>Docked's AnimationClip.</summary>
	[SerializeField] private AnimationClip sail0Animation; 				/// <summary>Docked's AnimationClip.</summary>
	[SerializeField] private AnimationClip sail1Animation; 				/// <summary>Docked's AnimationClip.</summary>
	[SerializeField] private AnimationClip cannonAnimation; 			/// <summary>Docked's AnimationClip.</summary>
	private Animator _animator; 										/// <summary>Animator's Component.</summary>
	private Animation _animation; 										/// <summary>Animation's Component.</summary>
	private AnimationEventInvoker _animationEventInvoker; 				/// <summary>AnimatorEventInvoker's Component.</summary>

	/// <summary>Gets stateIDCredential property.</summary>
	public AnimatorCredential stateIDCredential { get { return _stateIDCredential; } }

	/// <summary>Gets ropeHitBox property.</summary>
	public HitCollider2D ropeHitBox { get { return _ropeHitBox; } }

	/// <summary>Gets projectileReference property.</summary>
	public VAssetReference projectileReference { get { return _projectileReference; } }

	/// <summary>Gets cannons property.</summary>
	public Transform[] cannons { get { return _cannons; } }

	/// <summary>Gets animator Component.</summary>
	public Animator animator
	{ 
		get
		{
			if(_animator == null) _animator = GetComponent<Animator>();
			return _animator;
		}
	}

	/// <summary>Gets animation Component.</summary>
	public Animation animation
	{ 
		get
		{
			if(_animation == null) _animation = GetComponent<Animation>();
			return _animation;
		}
	}

	/// <summary>Gets animationEventInvoker Component.</summary>
	public AnimationEventInvoker animationEventInvoker
	{ 
		get
		{
			if(_animationEventInvoker == null) _animationEventInvoker = GetComponent<AnimationEventInvoker>();
			return _animationEventInvoker;
		}
	}

	/// <summary>ShantyShip's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		animationEventInvoker.AddIntActionListener(OnAnimationIntEvent);

		animation.AddClips(
			dockedAnimation,
			sail0Animation,
			sail1Animation,
			cannonAnimation
		);
	}

	/// <summary>Goes to given [Animator's] state.</summary>
	/// <param name="_ID">State's ID.</param>
	public void GoToState(int _ID)
	{
		//animator.SetInteger(stateIDCredential, _ID);
		AnimationClip clip = null;

		switch(_ID)
		{
			case ID_STATE_DOCKED:
			clip = dockedAnimation;
			break;

			case ID_STATE_SAIL_0:
			clip = sail0Animation;
			break;

			case ID_STATE_CANNON:
			clip = cannonAnimation;
			break;

			case ID_STATE_SAIL_1:
			clip = sail1Animation;
			break;		 
		}

		if(clip != null) animation.Play(clip);
	}

	/// <summary>Activates Cannons.</summary>
	/// <param name="_activate">Activate? true by default.</param>
	public void ActivateCannons(bool _activate = true)
	{
		if(cannons == null) return;
		
		foreach(Transform cannon in cannons)
		{
			cannon.gameObject.SetActive(_activate);
		}
	}

	/// <summary>Shoots Cannons.</summary>
	private void ShootCannons()
	{
		if(cannons == null) return;

		foreach(Transform cannon in cannons)
		{
			PoolManager.RequestProjectile(Faction.Enemy, projectileReference, cannon.position, cannon.forward);
		}
	}

	/// <summary>Callback invoked when an Animation Event is invoked.</summary>
	/// <param name="_ID">Int argument.</param>
	private void OnAnimationIntEvent(int _ID)
	{
		switch(_ID)
		{
			case ID_ANIMATIONEVENT_SHOOTCANNONS:
			ShootCannons();
			break;
		}
	}
}
}