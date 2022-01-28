using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum MiniGameState
{
	NotRunning,
	Running,
	Paused
}

public enum Result
{
	None,
	Failure,
	Success
}

/// <summary>Event invoked when a Mini-Game Event occurs.</summary>
/// <param name="_miniGame">Mini-Game that invoked the event.</param>
/// <param name="_eventID">Event's ID.</param>
public delegate void OnMiniGameEvent(MiniGame _miniGame, int _eventID);

[Serializable]
public abstract class MiniGame
{
	public const int ID_EVENT_MINIGAME_ENDED = 0; 					/// <summary>Mini-Game's Ended Event ID.</summary>
	public const int ID_EVENT_MINIGAME_SCOREUPDATE_LOCAL = 1;       /// <summary>Local Score's Update Event ID.</summary>
    public const int ID_EVENT_MINIGAME_SCOREUPDATE_VISITOR = 2;     /// <summary>Visitor Score's Update Event ID.</summary>
    public const int ID_EVENT_MINIGAME_UPDATE = 3; 					/// <summary>Mini-Game's Update Event ID.</summary>
    public const int ID_EVENT_MINIGAME_SUCCESS = 4; 				/// <summary>Mini-Game's Success Event ID.</summary>
    public const int ID_EVENT_MINIGAME_FAILURE = 5; 				/// <summary>Mini-Game's Failure Event ID.</summary>

	protected OnMiniGameEvent onMiniGameEvent; 						/// <summary>OnMiniGameEvent's Delegate.</summary>

    [SerializeField] private Clock _clock;                          /// <summary>Mini-Game's Clock.</summary>
    [SerializeField] private float _timeLimit;                      /// <summary>Time's Limit.</summary>
    private int _score;                                             /// <summary>Current Score.</summary>
    private int _maxScore; 											/// <summary>Maximum's Score.</summary>
	private MiniGameState _state; 									/// <summary>Current Mini-Game's State.</summary>
	private MonoBehaviour _monoBehaviour; 							/// <summary>MonoBehaviour's Reference.</summary>
	protected Coroutine coroutine; 									/// <summary>Coroutine's Reference.</summary>

	/// <summary>Gets and Sets clock property.</summary>
    public Clock clock
    {
        get { return _clock; }
        protected set { _clock = value; }
    }

    /// <summary>Gets and Sets timeLimit property.</summary>
    public float timeLimit
    {
        get { return _timeLimit; }
        set
        {
        	_timeLimit = value;
        	if(clock != null) clock.ellapsedTime = timeLimit;
        }
    }
	
	/// <summary>Gets and Sets score property.</summary>
    public int score
    {
        get { return _score; }
        protected set { _score = value; }
    }

    /// <summary>Gets and Sets maxScore property.</summary>
    public int maxScore
    {
    	get { return _maxScore; }
    	protected set { _maxScore = value; }
    }

	/// <summary>Gets and Sets state property.</summary>
	public MiniGameState state
	{
		get { return _state; }
		protected set { _state = value; }
	}

	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		protected set { _monoBehaviour = value; }
	}

	/// <summary>Gets scorePercentage property.</summary>
	public float scorePercentage { get { return (float)score / (float)maxScore; } }

	/// <summary>Initializes Mini-Game.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour that will start the coroutine.</param>
	/// <param name="onMiniGameEvent">Optional callback invoked then the Mini-Game invokes Events.</param>
	public virtual void Initialize(MonoBehaviour _monoBehaviour, OnMiniGameEvent onMiniGameEvent = null)
	{
		if(_monoBehaviour == null) return;

		score = 0;
		monoBehaviour = _monoBehaviour;
		if(onMiniGameEvent != null) this.onMiniGameEvent = onMiniGameEvent; 
		monoBehaviour.StartCoroutine(MiniGameCoroutine(), ref coroutine);
		state = MiniGameState.Running;
	}

	/// <summary>Terminates Mini-Game.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour that will request the Coroutine's dispatchment.</param>
	public virtual void Terminate()
	{
		monoBehaviour.DispatchCoroutine(ref coroutine);
		onMiniGameEvent = null;
		state = MiniGameState.NotRunning;
		InvokeEvent(ID_EVENT_MINIGAME_ENDED);
	}

	/// <summary>Invokes Event.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected void InvokeEvent(int _ID)
	{
		if(onMiniGameEvent != null) onMiniGameEvent(this, _ID);
	}

	/// <summary>Mini-Game's Coroutine.</summary>
	protected virtual IEnumerator MiniGameCoroutine() { yield return null; }
}
}