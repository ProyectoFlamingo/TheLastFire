using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Voidless;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class RinocircusBossAIController : CharacterAIController<RinocircusBoss>
{
	/// <summary>RinocircusBossAIController's instance initialization when loaded [Before scene loads].</summary>
	protected virtual void Awake()
	{
		base.Awake();
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	protected override void Start()
	{
		base.Start();
		StartCoroutine(SpawnTest());
	}

	private IEnumerator SpawnTest()
	{
		SecondsDelayWait wait = new SecondsDelayWait(1.5f);
		yield return wait;

		//bool result = character.SpawnBall();
		bool result = character.SpawnTricycle();
		Debug.Log("[RinocircusBossAIController] Result of Spawning Ball: " + result);
	}
}
}