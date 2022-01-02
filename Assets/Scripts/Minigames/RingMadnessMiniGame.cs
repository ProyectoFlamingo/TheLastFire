using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Voidless;
using System;

using Random = UnityEngine.Random;

namespace Flamingo
{
[Serializable]
public class RingMadnessMiniGame : MiniGame
{
    [Space(5f)]
    [Header("Ring-Madness' Attributes:")]
    [SerializeField] private RingsContainer _ringsContainer;            /// <summary>Rings' Container.</summary>
    [SerializeField] private bool _deactivateRingsWhenMiniGameEnds;     /// <summary>Deactivate Rings when Mini-Game Ends?.</summary>

    /// <summary>Gets and Sets ringsContainer property.</summary>
    public RingsContainer ringsContainer
    {
        get { return _ringsContainer; }
        set { _ringsContainer = value; }
    }

    /// <summary>Gets and Sets deactivateRingsWhenMiniGameEnds property.</summary>
    public bool deactivateRingsWhenMiniGameEnds
    {
        get { return _deactivateRingsWhenMiniGameEnds; }
        set { _deactivateRingsWhenMiniGameEnds = value; }
    }

     /// <summary>Initializes Mini-Game.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour that will start the coroutine.</param>
    /// <param name="onMiniGameEvent">Optional callback invoked then the Mini-Game invokes Events.</param>
    public override void Initialize(MonoBehaviour _monoBehaviour, OnMiniGameEvent onMiniGameEvent = null)
    {
        base.Initialize(_monoBehaviour, onMiniGameEvent);
        ringsContainer.ActivateRings(true);
        ringsContainer.SubscribeToRingsDeactivations(true, OnRingPassed);
        maxScore = ringsContainer.rings.Length;
        clock.Reset(timeLimit);
    }

    /// <summary>Terminates Mini-Game.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour that will request the Coroutine's dispatchment.</param>
    public override void Terminate()
    {
        base.Terminate();
        ringsContainer.SubscribeToRingsDeactivations(false);
    }

    /// <summary>Event invoked when a Collider passes a ring.</summary>
    /// <param name="_collider">Collider that passed the ring.</param>
    public void OnRingPassed(Collider2D _collider)
    {
        score++;
        if(score >= maxScore)
        {
            Terminate();
            InvokeEvent(ID_EVENT_MINIGAME_SUCCESS);
        }
    }

    /// <summary>Mini-Game's Coroutine.</summary>
    protected override IEnumerator MiniGameCoroutine()
    {
        while(clock.ellapsedTime > 0.0f)
        {
            clock.Update(-Time.deltaTime);
            yield return null;
        }

        Terminate();
        InvokeEvent(ID_EVENT_MINIGAME_FAILURE);
    }
}
}