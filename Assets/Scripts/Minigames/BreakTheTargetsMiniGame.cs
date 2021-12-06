using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using System;

using Random = UnityEngine.Random;

namespace Flamingo
{
[Serializable]
public class BreakTheTargetsMiniGame : MiniGame
{
    [SerializeField] private BreakableTargetsContainer _targetsContainers;      /// <summary>Breakable Targets' Containers.</summary>
    [SerializeField] private Clock _clock;                                      /// <summary>Mini-Game's Clock.</summary>

#region Getters/Setters:
    /// <summary>Gets and Sets targetsContainers property.</summary>
    public BreakableTargetsContainer targetsContainers
    {
        get { return _targetsContainers; }
        set { _targetsContainers = value; }
    }

    /// <summary>Gets and Sets clock property.</summary>
    public Clock clock
    {
        get { return _clock; }
        set { _clock = value; }
    }
#endregion

    /// <summary>Initializes Mini-Game.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour that will start the coroutine.</param>
    /// <param name="onMiniGameEvent">Optional callback invoked then the Mini-Game invokes Events.</param>
    public override void Initialize(MonoBehaviour _monoBehaviour, OnMiniGameEvent onMiniGameEvent = null)
    {
        base.Initialize(_monoBehaviour, onMiniGameEvent);
    }

    /// <summary>Mini-Game's Coroutine.</summary>
    protected override IEnumerator MiniGameCoroutine() { yield return null; }
}
}