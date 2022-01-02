using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using System;


namespace Flamingo
{   
[Serializable]    
public class BasketBallMiniGame : MiniGame
{
    [Header("Time's Attributes:")]
    [SerializeField] public Clock _clock;           /// <summary>Mini-Game's Internal Clock.</summary>
    [SerializeField] private float _timeLimit;      /// <summary>Limit time for the minigame.</summary>
    [Space(5f)]
    [Header("Rings' References:")]
    [SerializeField] private Ring _localRing;       /// <summary>Basket for local Player.</summary>
    [SerializeField] private Ring _visitorRing;     /// <summary>Basket for visitor Player.</summary>
    [Space(5f)]
    [SerializeField] private BouncingBall _ball;    /// <summary>Ball's Reference.</summary>
    private int _localScore;                        /// <summary>Local Player's score.</summary>
    private int _visitorScore;                      /// <summary>Visitor Player's score.</summary>

#region Getters/Setters:
    /// <summary>Gets and Sets clock property.</summary>
    public Clock clock
    {
        get { return _clock; }
        set { _clock = value; }
    }

    /// <summary>Gets and Sets limitMinutes for minigame property.</summary>
    public float timeLimit
    {
        get { return _timeLimit; }
        set { _timeLimit = value; }
    }

    /// <summary>Gets and Sets localRing property.</summary>
    public Ring localRing
    {
        get { return _localRing; }
        set { _localRing = value; }
    }

    /// <summary>Gets and Sets visitorRing property.</summary>
    public Ring visitorRing
    {
        get { return _visitorRing; }
        set { _visitorRing = value; }
    }

    /// <summary>Gets and Sets ball property.</summary>
    public BouncingBall ball
    {
        get { return _ball; }
        set { _ball = value; }
    }

    /// <summary>Gets and Sets localScore property.</summary>
    public int localScore
    {
        get { return _localScore; }
        set { _localScore = value; }
    }

    /// <summary>Gets and Sets visitorScore property.</summary>
    public int visitorScore
    {
        get { return _visitorScore; }
        set { _visitorScore = value; }
    }
#endregion

#region Events Callbacks  
    /// <summary>Callback for onRingPassed's Event if its the visitor basket.</summary>
    /// <param name="_collider">Collider that passed the ring.</param>
    public void OnVisitorRingPassed(Collider2D _collider)
    {
        localScore++;
        InvokeEvent(ID_EVENT_MINIGAME_SCOREUPDATE_LOCAL);
    }
    /// <summary>Callback for onRingPassed's Event if its the local basket.</summary>
    /// <param name="_collider">Collider that passed the ring.</param>
    public void OnLocalRingPassed(Collider2D _collider)
    {
        visitorScore++;
        InvokeEvent(ID_EVENT_MINIGAME_SCOREUPDATE_VISITOR);
    }
#endregion

    /// <summary>Basket-Ball Mini-Game's Coroutine.</summary>
    protected override IEnumerator MiniGameCoroutine()
    {
        /// Subscribe:
        visitorRing.onRingPassed += OnVisitorRingPassed;
        localRing.onRingPassed += OnLocalRingPassed;
  
        while (clock.minutes < timeLimit)
        {
            /// Static way to access WaitForEndOfFrame, without a new constructor (memory allocation) each frame.
            yield return VCoroutines.WAIT_MAIN_THREAD;

            clock.Update(Time.deltaTime);
        }

        /// Unsubscribe:
        visitorRing.onRingPassed -= OnVisitorRingPassed;
        localRing.onRingPassed -= OnLocalRingPassed;

        Terminate();
    }

    /// <returns>Current's Winner [regardless if the time is over].</returns>
    public string CurrentWinner()
    {
        return visitorScore > localScore ? "Visitor" : localScore > visitorScore ? "Local" : "Tie";
    }
}
}