using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using UnityEngine.AddressableAssets;

namespace Flamingo
{
public class MoskarBossAIController : CharacterAIController<MoskarBoss>
{
	[Space(5f)]
	[SerializeField] private AssetReference _moskarReference; 	/// <summary>Moskar's Reference.</summary>
	private HashSet<MoskarBoss> _reproductions; 				/// <summary>Moskar's Reproductions.</summary>

	/// <summary>Gets moskarReference property.</summary>
	public AssetReference moskarReference { get { return _moskarReference; } }

	/// <summary>Gets and Sets reproductions property.</summary>
	public HashSet<MoskarBoss> reproductions
	{
		get { return _reproductions; }
		set { _reproductions = value; }
	}

	/// <summary>MoskarBossAIController's instance initialization.</summary>
	protected override void Awake()
	{
		base.Awake();
	}

	/// <summary>MoskarBossAIController's starting actions before 1st Update frame.</summary>
	protected override void Start ()
	{
		base.Start();
		if(character != null) SubscribeToMoskarEvents(character);
	}
	
	/// <summary>MoskarBossAIController's tick at each frame.</summary>
	protected override void Update ()
	{
		base.Update();
	}

	/// <summary>Callback invoked when the character invokes an event.</summary>
	/// <param name="_ID">Event's ID.</param>
	protected override void OnCharacterIDEvent(int _ID) { /*...*/ }

	private void SubscribeToMoskarEvents(MoskarBoss _moskar, bool _subscribe = true)
	{

	}
}
}