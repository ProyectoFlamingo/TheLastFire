using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public class TESTRandomSystem : MonoBehaviour
{
	[SerializeField] private int iterations; 					/// <summary>Number of iterations.</summary>
	[SerializeField] private RandomDistributionSystem system; 	/// <summary>Random System.</summary>

	/// <summary>TESTRandomSystem's instance initialization.</summary>
	private void Awake()
	{
		system.Redistribute();
		//StartCoroutine(Test());

		for(int i = 0; i < iterations; i++)
		{
			system.GetRandomIndex();
		}
	}

	private IEnumerator Test()
	{
		SecondsDelayWait wait = new SecondsDelayWait(1.0f);

		while(true)
		{
			int index = system.GetRandomIndex();

			while(wait.MoveNext()) yield return null;
			wait.Reset();
		}
	}
}