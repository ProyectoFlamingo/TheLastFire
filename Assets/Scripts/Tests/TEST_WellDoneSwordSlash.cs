using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flamingo;
using Voidless;

public class TEST_WellDoneSwordSlash : MonoBehaviour
{
	[SerializeField] private ContactWeapon weapon; 		/// <summary>Contact Weapon.</summary>
	[SerializeField] private Vector3 rotationAxis; 		/// <summary>RotationAxis on Slash [applied locally].</summary>
	[SerializeField] private float rotationDuration; 	/// <summary>Rotation's Duration.</summary>
	private Coroutine routine;

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		SwordSlash();
	}

	public void SwordSlash()
	{
		this.StartCoroutine(SwordSlashRoutine(), ref routine);
	}

	private IEnumerator SwordSlashRoutine()
	{
		Quaternion initialRotation = weapon.transform.localRotation;
		Quaternion rotation = Quaternion.Euler(rotationAxis);
		float t = 0.0f;
		float iD = 1.0f / rotationDuration;

		weapon.ActivateHitBoxes(true);

		while(t < 1.0f)
		{
			weapon.transform.localRotation = Quaternion.Lerp(initialRotation, rotation, t * t);

			t += (Time.deltaTime * iD);
			yield return null;
		}

		t = 0.0f;
		weapon.ActivateHitBoxes(false);

		while(t < 1.0f)
		{
			weapon.transform.localRotation = Quaternion.Lerp(rotation, initialRotation, t * t);

			t += (Time.deltaTime * iD);
			yield return null;
		}

		weapon.transform.localRotation = initialRotation;

		SwordSlash();
	}
}