using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(DashAbility))]
[RequireComponent(typeof(JumpAbility))]
public class RinocircusBoss : Boss
{
	[Space(5f)]
	[Header("Prop's Attrbiues:")]
	[SerializeField] private Transform _propsContainer; 							/// <summary>Props' Container.</summary>
	[SerializeField] private Transform _ballTransform; 								/// <summary>Ball's Transform.</summary>
	[SerializeField] private Transform _tricycleTransform; 							/// <summary>Tricycle's Container.</summary>
	[SerializeField] private VAssetReference _propSpawningParticleEffectReference; 	/// <summary>Particle-Effet reference for the spawning of props.</summary>
	[SerializeField] private int _feetAnchorContainerIndex; 						/// <summary>Index for the feet on the VirtualAnchorContainer [where the ball is gonna spawn].</summary>
	[SerializeField] private float _propSpawningInterpolationDuration; 				/// <summary>Prop Spawning's Interpolation Duration.</summary>
	private DashAbility _dashAbility; 												/// <summary>DashAbility's Component.</summary>
	private JumpAbility _jumpAbility; 												/// <summary>JumpAbility's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets propsContainer property.</summary>
	public Transform propsContainer { get { return _propsContainer; } }

	/// <summary>Gets ballTransform property.</summary>
	public Transform ballTransform { get { return _ballTransform; } }

	/// <summary>Gets tricycleTransform property.</summary>
	public Transform tricycleTransform { get { return _tricycleTransform; } }

	/// <summary>Gets propSpawningParticleEffectReference property.</summary>
	public VAssetReference propSpawningParticleEffectReference { get { return _propSpawningParticleEffectReference; } }

	/// <summary>Gets feetAnchorContainerIndex property.</summary>
	public int feetAnchorContainerIndex { get { return _feetAnchorContainerIndex; } }

	/// <summary>Gets propSpawningInterpolationDuration property.</summary>
	public float propSpawningInterpolationDuration { get { return _propSpawningInterpolationDuration; } }

	/// <summary>Gets dashAbility Component.</summary>
	public DashAbility dashAbility
	{ 
		get
		{
			if(_dashAbility == null) _dashAbility = GetComponent<DashAbility>();
			return _dashAbility;
		}
	}

	/// <summary>Gets jumpAbility Component.</summary>
	public JumpAbility jumpAbility
	{ 
		get
		{
			if(_jumpAbility == null) _jumpAbility = GetComponent<JumpAbility>();
			return _jumpAbility;
		}
	}
#endregion

	/// <summary>Callback invoked when this is instanciated.</summary>
	protected override void Awake()
	{
		base.Awake();
		ballTransform.gameObject.SetActive(false);
		tricycleTransform.gameObject.SetActive(false);
	}

	/// <summary>Spawns Ball.</summary>
	/// <param name="onSpawnEnds">Optional callback invoked when the prop spawn ends.</param>
	/// <returns>True if prop could be spawned.</returns>
	public bool SpawnBall(Action onSpawnEnds = null)
	{
		if(!IsGrounded()) return false;
		
		this.StartCoroutine(SpawnOnPropRoutine(ballTransform, onSpawnEnds), ref behaviorCoroutine);
		return true;
	}

	/// <summary>Spawns Tricycle.</summary>
	/// <param name="onSpawnEnds">Optional callback invoked when the prop spawn ends.</param>
	/// <returns>True if prop could be spawned.</returns>
	public bool SpawnTricycle(Action onSpawnEnds = null)
	{
		if(!IsGrounded()) return false;
		
		this.StartCoroutine(SpawnOnPropRoutine(tricycleTransform, onSpawnEnds), ref behaviorCoroutine);
		return true;
	}

	/// <summary>Dashes.</summary>
	public void Dash()
	{
		dashAbility.Dash();
	}

	/// <returns>True if it is grounded.</returns>
	private bool IsGrounded()
	{
		return jumpAbility.gravityApplier.grounded;
	}

	/// <summary>Ball Spawning Routine.</summary>
	private IEnumerator SpawnOnPropRoutine(Transform _prop, Action onSpawnEnds = null)
	{
		RaycastHit2D groundInfo = jumpAbility.gravityApplier.GetGroundInfo();
		Vector3 feetAnchorPosition = anchorContainer.GetAnchoredPosition(feetAnchorContainerIndex);
		Vector3 propPosition = _prop.position;
		Vector3 floorPoint = groundInfo.point;
		Vector3 spawnPosition = Vector3.zero;
		Vector3 a = transform.position;
		Vector3 b = Vector3.zero;
		Vector3 scaleA = Vector3.zero;
		Vector3 scaleB = Vector3.one;
		Renderer[] renderers = _prop.GetComponentsInChildren<Renderer>();
		Bounds bounds = VBounds.GetBoundsToFitSet(renderers);
		float halfHeight = bounds.extents.y; // bounds.size.y * 0.5f
		float offsetY = propPosition.y - bounds.center.y;
		float t = 0.0f;
		float iD = 1.0f / propSpawningInterpolationDuration;

		_prop.gameObject.SetActive(true);
		_prop.parent = null;
		spawnPosition = floorPoint;
		spawnPosition.y += (halfHeight + offsetY);
		b = spawnPosition;
		b.y += (halfHeight - offsetY);

		while(t < 1.0f)
		{
			transform.position = Vector3.Lerp(a, b, t);
			_prop.transform.position = Vector3.Lerp(feetAnchorPosition, spawnPosition, t);
			_prop.transform.localScale = Vector3.Lerp(scaleA, scaleB, t);
			t += (Time.deltaTime * iD);
			yield return null;
		}

		transform.position = b;
		_prop.transform.position = spawnPosition;
		_prop.transform.localScale = scaleB;
		transform.parent = _prop;

		if(onSpawnEnds != null) onSpawnEnds();
	}
}
}