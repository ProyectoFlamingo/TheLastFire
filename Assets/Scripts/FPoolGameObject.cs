using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

/*============================================================
**
** Class:  FPoolGameObject
**
** Purpose: This class inherits from PoolGameObject, it is a Flamingo's PoolGameObject (specifically for The Last Fire's Pipeline).
** Makes certain optional routines when commanded to be deactivated or recycled before activating/deactivating.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/
namespace Flamingo
{
public class FPoolGameObject : PoolGameObject
{
	[SerializeField] private Fragmentable _fragmentable; 	/// <summary>Fragmentable's Component.</summary>

	/// <summary>Gets and Sets fragmentable property.</summary>
	public Fragmentable fragmentable
	{
		get { return _fragmentable; }
		set { _fragmentable = value; }
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		if(fragmentable != null) fragmentable.Defragmentate();
		base.OnObjectReset();
	}

	public void Deactivate(Vector3 _direction)
	{
		if(fragmentable != null) fragmentable.Fragmentate(_direction, OnObjectDeactivation);
		else OnObjectDeactivation();
	}

	/*private IEnumerator FragmentationRoutine()
	{
		float t = 0.0f;
	}*/
}
}