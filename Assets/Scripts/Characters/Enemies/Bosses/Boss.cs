using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class Boss : Enemy
{
	public const int STAGE_1 = 1; 																					/// <summary>Stage 1's ID.</summary>
	public const int STAGE_2 = 2; 																					/// <summary>Stage 2's ID.</summary>
	public const int STAGE_3 = 3; 																					/// <summary>Stage 3's ID.</summary>
	public const int STAGE_4 = 4; 																					/// <summary>Stage 4's ID.</summary>
	public const int STAGE_5 = 5; 																					/// <summary>Stage 5's ID.</summary>

	[Space(5f)]
	[Header("Boss' Attributes:")]
	[SerializeField] private float[] _healthDistribution; 															/// <summary>Health Distribution across the Stages.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Boss' Tests:")]
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] protected bool forceStageTesting; 	/// <summary>Force Stage Testing?.</summary>
	[TabGroup("Testing Group", "Testing (Editor-Mode Only)")][SerializeField] protected int testStage; 				/// <summary>Test's Stage.</summary>
#endif
	private int _stages; 																							/// <summary>Boss' Stages.</summary>
	private int _currentStage; 																						/// <summary>Current Boss' Stage.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets stages property.</summary>
	public int stages
	{
		get { return _stages; }
		set { _stages = value; }
	}

	/// <summary>Gets and Sets currentStage property.</summary>
	public int currentStage
	{
		get { return _currentStage; }
		set { _currentStage = value; }
	}

	/// <summary>Gets stageScale property.</summary>
	public float stageScale { get { return (float)(currentStage - 1.0f) / (float)(stages - 1.0f); } }

	/// <summary>Gets and Sets healthDistribution property.</summary>
	public float[] healthDistribution
	{
		get { return _healthDistribution; }
		set { _healthDistribution = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>Resets Boss's instance to its default values.</summary>
	public override void Reset()
	{
		base.Reset();
		currentStage = 0;
		AdvanceStage();
	}

	/// <summary>Callback internally called right after Awake.</summary>
	protected override void Awake()
	{
		base.Awake();
		if(healthDistribution != null) stages = healthDistribution.Length;
		else VDebug.Log(LogType.Error, "Health Distribution not setted!");
		currentStage = 0;

		ResourcesManager.onResourcesLoaded += OnResourcesLoaded;
	}

	/// <summary>Callback internally called right after Start.</summary>
	protected override void Start()
	{
		base.Start();

		if(animationEventInvoker != null) animationEventInvoker.AddIntActionListener(OnAnimationIntEvent);
	}
#endregion

	/// <summary>Advances Stage.</summary>
	protected void AdvanceStage()
	{
		if(healthDistribution == null || currentStage >= healthDistribution.Length) return;

		currentStage = Mathf.Min(currentStage, stages);
		health.SetMaxHP(healthDistribution[currentStage++], true);
		OnStageChanged();
	}

	/// <summary>Callback invoked when resources are loaded.</summary>
	protected virtual void OnResourcesLoaded()
	{
#if UNITY_EDITOR
		if(forceStageTesting) currentStage =  Mathf.Clamp(testStage - 1, -1, stages);
#endif
		
		AdvanceStage();
	}

	/// <summary>Callback internally called when the Boss advances stage.</summary>
	protected virtual void OnStageChanged()
	{
		eventsHandler.InvokeIDEvent(IDs.EVENT_STAGECHANGED);
	}

	/// <summary>Begins Death's Routine.</summary>
	protected virtual void BeginDeathRoutine()
	{
		this.RemoveStates(IDs.STATE_ALIVE);
		eventsHandler.InvokeIDEvent(IDs.EVENT_DEATHROUTINE_BEGINS);
		deadFXs.PlayScheduleRoutine(this, OnDeadFXsFinished);
		
		Debug.Log("[Boss] " + name + " BeginDeathRoutine();");
		this.StartCoroutine(DeathRoutine(OnDeathRoutineEnds));
	}

	/// <summary>Callback invoked after the Death's routine ends.</summary>
	protected virtual void OnDeathRoutineEnds()
	{
		//OnObjectDeactivation();
		eventsHandler.InvokeIDEvent(IDs.EVENT_DEATHROUTINE_ENDS);
		eventsHandler.InvokeCharacterDeactivationEvent(DeactivationCause.Destroyed);
	} 

	/// <summary>Callback invoked when the DeadFX's routine ends.</summary>
	protected virtual void OnDeadFXsFinished(){ /*...*/ }

	/// <summary>Callback invoked when a Health's event has occured.</summary>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		switch(_event)
		{
			case HealthEvent.FullyDepleted:
			if(currentStage < stages) AdvanceStage();
			else
			{
				state &= ~IDs.STATE_ALIVE;
				BeginDeathRoutine();
			}
			break;
		}
	}

	/// <summary>Callback invoked when an Animation Event is invoked.</summary>
	/// <param name="_ID">Int argument.</param>
	protected virtual void OnAnimationIntEvent(int _ID) { /*...*/ }

	/// <returns>String representing enemy's stats.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine(base.ToString());
		builder.Append("Current Stage: ");
		builder.Append(currentStage.ToString());

		return builder.ToString();
	}

	/// <summary>Death's Routine.</summary>
	/// <param name="onDeathRoutineEnds">Callback invoked when the routine ends.</param>
	protected virtual IEnumerator DeathRoutine(Action onDeathRoutineEnds)
	{
		yield return null;
		if(onDeathRoutineEnds != null) onDeathRoutineEnds();
	}
}
}