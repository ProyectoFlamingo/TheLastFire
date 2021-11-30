using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class DestinoScriptableCoroutine : CoroutineBehavior<DestinoBoss>
{
	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="boss">Object of type T's argument.</param>
	public override IEnumerator Routine(DestinoBoss boss) { yield return null; }
}
}