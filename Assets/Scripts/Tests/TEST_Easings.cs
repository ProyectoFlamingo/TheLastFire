using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Flamingo;

[Serializable]
public class TEST_EasingsGroup
{
	public Transform transform;
	public Vector3Pair pair;

	public void Lerp(float t, Func<float, float> f = null)
	{
		if(transform == null) return;

		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;

		transform.position = Vector3.Lerp(pair.a, pair.b, f(t));
	}
}

public class TEST_Easings : MonoBehaviour
{
	[SerializeField] private float duration; 					/// <summary>Interpolation Duration.</summary>
	[SerializeField] private TEST_EasingsGroup linear; 			/// <summary>Linear.</summary>
	[SerializeField] private TEST_EasingsGroup easeIn; 			/// <summary>Ease-In.</summary>
	[SerializeField] private TEST_EasingsGroup easeInBack; 		/// <summary>Ease-In Back.</summary>
	[SerializeField] private TEST_EasingsGroup easeOutBounce; 	/// <summary>Ease-Out Bounce.</summary>

	/// <summary>TEST_Easings's starting actions before 1st Update frame.</summary>
	private IEnumerator Start ()
	{
		float t = 0.0f;
		float at = 0.0f;
		float inverseDuration = 1.0f / duration;
		float s = 1.0f;

		while(true)
		{
			linear.Lerp(at);
			easeIn.Lerp(at, VMath.EaseInQuad);
			easeInBack.Lerp(at, VMath.EaseInBack);
			easeOutBounce.Lerp(at, VMath.EaseOutBounce);

			t += (Time.deltaTime * inverseDuration);
			at = s == 1.0f ? t : 1.0f - t;

			if(t >= 1.0f)
			{
				t = 0.0f;
				s *= -1.0f;
			}

			yield return null;
		}
	}
}