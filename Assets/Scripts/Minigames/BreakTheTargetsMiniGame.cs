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
    [Space(5f)]
    [Header("Break-the-Targets' Attributes:")]
    [SerializeField] private BreakableTargetsContainer _targetsContainer;      /// <summary>Breakable Targets' Containers.</summary>
    [SerializeField] private bool _deactivateTargetsWhenMinigameEnds;           /// <summary>Deactivate targets when the Mini-Game ends?.</summary>

    /// <summary>Gets and Sets targetsContainer property.</summary>
    public BreakableTargetsContainer targetsContainer
    {
        get { return _targetsContainer; }
        set { _targetsContainer = value; }
    }

    /// <summary>Gets and Sets deactivateTargetsWhenMinigameEnds property.</summary>
    public bool deactivateTargetsWhenMinigameEnds
    {
        get { return _deactivateTargetsWhenMinigameEnds; }
        set { _deactivateTargetsWhenMinigameEnds = value; }
    }

    /// <summary>Initializes Mini-Game.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour that will start the coroutine.</param>
    /// <param name="onMiniGameEvent">Optional callback invoked then the Mini-Game invokes Events.</param>
    public override void Initialize(MonoBehaviour _monoBehaviour, OnMiniGameEvent onMiniGameEvent = null)
    {
        base.Initialize(_monoBehaviour, onMiniGameEvent);
        targetsContainer.ActivateTargets(true);
        targetsContainer.SubscribeToTargetsDeactivations(true, OnTargetDeactivation);
        maxScore = targetsContainer.targets.Length;
        clock.Reset(timeLimit);
    }

    /// <summary>Terminates Mini-Game.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour that will request the Coroutine's dispatchment.</param>
    public override void Terminate()
    {
        base.Terminate();
        targetsContainer.SubscribeToTargetsDeactivations(false);
    }

    /// <summary>Callback invoked when a target is deactivated.</summary>
    /// <param name="_cause">Target's Deactivation Cause.</param>
    /// <param name="_info">Trigger2D's Information [if there's one].</param>
    private void OnTargetDeactivation(DeactivationCause _cause, Trigger2DInformation _info)
    {
        score++;
        //Debug.Log("[BreakTheTargetsMiniGame] Score: " + score + ", Max Score: " + maxScore);
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