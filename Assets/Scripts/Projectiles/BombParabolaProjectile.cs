using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum BombState
{
	WickOff,
	WickOn,
	Exploding
}

public class BombParabolaProjectile : Projectile, IFiniteStateMachine<BombState>
{
	[Space(5f)]
	[Header("Bomb's Attributes:")]
	[SerializeField] private GameObjectTag[] _flamableTags; 			/// <summary>Tags of GameObjects that are considered flamable.</summary>
	[SerializeField] private VAssetReference _explodableReference; 		/// <summary>Explodable's Reference.</summary>
	[Space(5f)]
	[Header("Fuse's Attributes:")]
	[SerializeField] private VAssetReference _fireEffectReference; 		/// <summary>Fire Particle-Effect's Reference.</summary>
	[SerializeField] private LineRenderer _fuse; 						/// <summary>Bomb's Fuse.</summary>
	[SerializeField] private float _fuseDuration; 						/// <summary>Fuse's Duration.</summary>
	[SerializeField] private float _fuseLength; 						/// <summary>Fuse's Length.</summary>
	[Space(5f)]
	[Header("Explosion Containment's Attributes:")]
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _containmentPhaseThreshold; 		/// <summary>Normalized value that determines when does the bomb enter a containment phase [relative to its fuse duration].</summary>
	[SerializeField] private float _containmentOscillationMagnitude; 	/// <summary>Containment Oscillation's Manitude.</summary>
	[SerializeField] private float _containmentOscillationCycles; 		/// <summary>Containmet Oscillation's Cycles.</summary>
	[SerializeField] private FloatRange _containmentOscillationSpeed; 	/// <summary>Containment Oscillation's Speed.</summary>
	private float _currentFuseLength; 									/// <summary>Current Fuse's Length.</summary>
	private BombState _state; 											/// <summary>Current Bomb's State.</summary>
	private BombState _previousState; 									/// <summary>Current Bomb's Previous State.</summary>
	private ParticleEffect _fuseFire; 									/// <summary>Fuse Fire's ParticleEffect.</summary>
	private Coroutine fuseRoutine; 										/// <summary>Fuse Coroutines' Reference.</summary>
	private Coroutine containmentPhase; 								/// <summary>Containment Phase Coroutine's Reference.</summary>
	private Explodable _explosion; 										/// <summary>Explosion's Reference.</summary>
	private Vector3 _originalScale; 									/// <summary>Original MeshContainer's Scale.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets flamableTags property.</summary>
	public GameObjectTag[] flamableTags
	{
		get { return _flamableTags; }
		set { _flamableTags = value; }
	}

	/// <summary>Gets and Sets explodableReference property.</summary>
	public VAssetReference explodableReference
	{
		get { return _explodableReference; }
		set { _explodableReference = value; }
	}

	/// <summary>Gets and Sets fireEffectReference property.</summary>
	public VAssetReference fireEffectReference
	{
		get { return _fireEffectReference; }
		set { _fireEffectReference = value; }
	}

	/// <summary>Gets and Sets fuse property.</summary>
	public LineRenderer fuse
	{
		get { return _fuse; }
		set { _fuse = value; }
	}

	/// <summary>Gets and Sets fuseDuration property.</summary>
	public float fuseDuration
	{
		get { return _fuseDuration; }
		set { _fuseDuration = value; }
	}

	/// <summary>Gets and Sets fuseLength property.</summary>
	public float fuseLength
	{
		get { return _fuseLength; }
		set { _fuseLength = value; }
	}

	/// <summary>Gets and Sets containmentPhaseThreshold property.</summary>
	public float containmentPhaseThreshold
	{
		get { return _containmentPhaseThreshold; }
		set { _containmentPhaseThreshold = value; }
	}

	/// <summary>Gets and Sets containmentOscillationMagnitude property.</summary>
	public float containmentOscillationMagnitude
	{
		get { return _containmentOscillationMagnitude; }
		set { _containmentOscillationMagnitude = value; }
	}

	/// <summary>Gets and Sets containmentOscillationCycles property.</summary>
	public float containmentOscillationCycles
	{
		get { return _containmentOscillationCycles; }
		set { _containmentOscillationCycles = value; }
	}

	/// <summary>Gets and Sets currentFuseLength property.</summary>
	public float currentFuseLength
	{
		get { return _currentFuseLength; }
		set { _currentFuseLength = value; }
	}

	/// <summary>Gets and Sets containmentOscillationSpeed property.</summary>
	public FloatRange containmentOscillationSpeed
	{
		get { return _containmentOscillationSpeed; }
		set { _containmentOscillationSpeed = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public BombState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public BombState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}

	/// <summary>Gets and Sets fuseFire property.</summary>
	public ParticleEffect fuseFire
	{
		get { return _fuseFire; }
		set { _fuseFire = value; }
	}

	/// <summary>Gets and Sets explosion property.</summary>
	public Explodable explosion
	{
		get { return _explosion; }
		set { _explosion = value; }
	}

	/// <summary>Gets and Sets originalScale property.</summary>
	public Vector3 originalScale
	{
		get { return _originalScale; }
		set { _originalScale = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		if(fuse == null) return;

		Gizmos.DrawRay(fuse.transform.position, (fuse.transform.up * fuseLength));
	}
#endif
	/// <summary>BombParabolaProjectile's instance initialization when loaded [Before scene loads].</summary>
	protected override void Awake()
	{
		base.Awake();
		currentFuseLength = fuseLength;
		originalScale = meshContainer.transform.localScale;
	}

	/// <summary>Updates BombParabolaProjectile's instance at each frame.</summary>
	protected override void Update()
	{
		base.Update();
		UpdateFuse();
	}

	/// <summary>Enters T State.</summary>
	/// <param name="_state">T State that will be entered.</param>
	public void OnEnterState(BombState _state)
	{
		switch(_state)
		{
			case BombState.WickOff:
			currentFuseLength = fuseLength;
			break;

			case BombState.WickOn:
				fuseFire = PoolManager.RequestParticleEffect(fireEffectReference, fuse.transform.position + (fuse.transform.up * fuseLength), fuse.transform.rotation);
				fuseFire.transform.parent = transform;
				this.StartCoroutine(FuseOnRoutine(), ref fuseRoutine);
			break;

			case BombState.Exploding:
				ActivateHitBoxes(false);
				explosion = PoolManager.RequestExplodable(explodableReference, transform.position, transform.rotation);
			break;
		}

		eventsHandler.InvokeContactWeaponIDEvent(IDs.EVENT_STATECHANGED);
	}

	/// <summary>Exits T State.</summary>
	/// <param name="_state">T State that will be left.</param>
	public void OnExitState(BombState _state)
	{
		switch(_state)
		{
			case BombState.WickOn:
				if(fuseFire != null)
				{
					if(gameObject.activeSelf) fuseFire.transform.parent = null;
					fuseFire.OnObjectDeactivation();
					fuseFire = null;
				}
				this.DispatchCoroutine(ref fuseRoutine);
			break;
		}
	}

	/// <summary>Callback invoked when the object is deactivated.</summary>
	public override void OnObjectDeactivation()
	{
		base.OnObjectDeactivation();
		rigidbody.Sleep();
		this.DispatchCoroutine(ref fuseRoutine);
		this.DispatchCoroutine(ref containmentPhase);

		if(fuseFire != null && gameObject.activeSelf)
		{
			fuseFire.transform.parent = null;
			fuseFire.OnObjectDeactivation();
			fuseFire = null;
		}
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		state = BombState.WickOff;
		this.DispatchCoroutine(ref fuseRoutine);
		this.DispatchCoroutine(ref containmentPhase);
		//meshContainer.transform.localScale = Vector3.one;
		meshContainer.transform.localScale = originalScale;
		if(fuseFire != null) fuseFire.gameObject.SetActive(false);
	}

	/// <summary>Event invoked when a Collision2D intersection is received.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_eventType">Type of the event.</param>
	/// <param name="_ID">Optional ID of the HitCollider2D.</param>
	public override void OnTriggerEvent(Trigger2DInformation _info, HitColliderEventTypes _eventType, int _ID = 0)
	{
		if(state == BombState.Exploding) return;

		base.OnTriggerEvent(_info, _eventType, _ID);

		Collider2D collider = _info.collider;
		GameObject obj = collider.gameObject;

		switch(state)
		{
			case BombState.WickOff:
				if(flamableTags != null) foreach(GameObjectTag tag in flamableTags)
				{
					if(obj.CompareTag(tag))
					{
						this.ChangeState(BombState.WickOn);
						break;
					}
				}
			break;

			case BombState.WickOn:
				/*if(repelTags != null) foreach(GameObjectTag tag in repelTags)
				{
					if(obj.CompareTag(tag))
					{
						InvokeDeactivationEvent(DeactivationCause.Destroyed, _info);
						this.ChangeState(BombState.Exploding);
						break;
					}
				}*/
			break;
		}
	}

	/// <summary>Callback internally called when there was an impact.</summary>
	/// <param name="_info">Trigger2D's Information.</param>
	/// <param name="_ID">ID of the HitCollider2D.</param>
	protected override void OnImpact(Trigger2DInformation _info, int _ID = 0)
	{
		if(state == BombState.WickOn) this.ChangeState(BombState.Exploding);
		base.OnImpact(_info, _ID);
	}

	/// <summary>Updates Fuse.</summary>
	private void UpdateFuse()
	{
		if(fuse == null || state == BombState.Exploding) return;

		Vector3 origin = fuse.transform.position;
		Vector3 fuseTip = origin + (fuse.transform.up * currentFuseLength);

		fuse.SetPosition(0, origin);
		fuse.SetPosition(1, fuseTip);

		if(state != BombState.WickOn || fuseFire == null) return;

		fuseFire.transform.position = fuseTip;
		fuseFire.transform.rotation = fuse.transform.rotation;
	}

	/// <summary>Fuse-On's Coroutine.</summary>
	private IEnumerator FuseOnRoutine()
	{
		float t = 0.0f;
		float inverseDuration = 1.0f / fuseDuration;

		while(t < 1.0f)
		{
			currentFuseLength = Mathf.Lerp(fuseLength, 0.0f, t);

			if(t >= containmentPhaseThreshold && containmentPhase == null)
			this.StartCoroutine(ContainmentPhase(), ref containmentPhase);

			t += (inverseDuration * Time.deltaTime);
			yield return null;
		}

		currentFuseLength = 0.0f;
		this.ChangeState(BombState.Exploding);
		InvokeDeactivationEvent(DeactivationCause.LifespanOver);
	}

	/// <summary>Containment's Phase.</summary>
	private IEnumerator ContainmentPhase()
	{
		if(meshContainer == null) yield break;

		float t = 0.0f;
		float x = 360.0f * Mathf.Deg2Rad * containmentOscillationCycles;
		float sin = 0.0f;
		float inverseDuration = 1.0f / (fuseDuration * (1.0f - containmentPhaseThreshold));
		float s = 0.0f;
		//Vector3 a = Vector3.one;
		Vector3 a = originalScale;
		Vector3 b = a * containmentOscillationMagnitude;

		while(t < 1.0f)
		{
			s = containmentOscillationSpeed.Lerp(t);
			sin = VMath.RemapValueToNormalizedRange(Mathf.Sin(t * x * s), -1.0f, 1.0f);
			meshContainer.transform.localScale = Vector3.Lerp(a, (b * sin), t);
			meshContainer.transform.localScale = (b * sin);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}
	}
}
}