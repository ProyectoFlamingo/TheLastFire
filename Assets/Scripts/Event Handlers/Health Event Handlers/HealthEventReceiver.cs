using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Flamingo
{
public class HealthEventReceiver : MonoBehaviour
{
	[SerializeField] private Health _health; 					/// <summary>Health Component's Reference.</summary>
	[Space(5f)]
	[Header("Flash's Attributes:")]
	[SerializeField] private Renderer[] _renderers; 			/// <summary>Renderer's to flash.</summary>
	[SerializeField] private Color _flashColor; 				/// <summary>Flash's Color.</summary>
	[SerializeField] private MaterialTag _selfIlluminationTag; 	/// <summary>Self-Illumination's Property Tag.</summary>
	[SerializeField] private MaterialTag _amountTag; 			/// <summary>Flash Amount's Property Tag.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _maxSelfIllumination; 	/// <summary>Maximum's Self-Illumination.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _maxFlashAmount; 			/// <summary>Maximum's Flash Amount.</summary>
	[SerializeField] private float _duration; 					/// <summary>Flash's Duration.</summary>
	[SerializeField] private float _cycles; 					/// <summary>Flash's Cycles.</summary>
	private Coroutine routine; 									/// <summary>Coroutine's Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets health Component.</summary>
	public Health health
	{ 
		get
		{
			if(_health == null) _health = GetComponent<Health>();
			return _health;
		}
		set { _health = value; }
	}

	/// <summary>Gets and Sets renderers property.</summary>
	public Renderer[] renderers
	{
		get { return _renderers; }
		set { _renderers = value; }
	}

	/// <summary>Gets and Sets flashColor property.</summary>
	public Color flashColor
	{
		get { return _flashColor; }
		set { _flashColor = value; }
	}

	/// <summary>Gets and Sets selfIlluminationTag property.</summary>
	public MaterialTag selfIlluminationTag
	{
		get { return _selfIlluminationTag; }
		set { _selfIlluminationTag = value; }
	}

	/// <summary>Gets and Sets amountTag property.</summary>
	public MaterialTag amountTag
	{
		get { return _amountTag; }
		set { _amountTag = value; }
	}

	/// <summary>Gets maxSelfIllumination property.</summary>
	public float maxSelfIllumination { get { return _maxSelfIllumination; } }

	/// <summary>Gets maxFlashAmount property.</summary>
	public float maxFlashAmount { get { return _maxFlashAmount; } }

	/// <summary>Gets duration property.</summary>
	public float duration { get { return _duration; } }

	/// <summary>Gets cycles property.</summary>
	public float cycles { get { return _cycles; } }
#endregion

	/// <summary>Resets FlashWhenReceivingDamage's instance to its default values.</summary>
	private void Reset()
	{
		flashColor = Color.white;
		GetRenderers();
	}

	/// <summary>HealthEventReceiver's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		if(health != null) health.onHealthEvent += OnHealthEvent;
	}

	/// <summary>Callback invoked when HealthEventReceiver's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		if(health != null) health.onHealthEvent -= OnHealthEvent;
	}

	/// <summary>Event invoked when a Health's event has occured.</summary>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	private void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		if(_event == HealthEvent.Depleted) this.StartCoroutine(Routine(), ref routine);
	}

	/// <summary>Cancels Routine.</summary>
	public void CancelRoutine()
	{
		this.DispatchCoroutine(ref routine);
	}

	[Button("Get Renderers")]
	/// <summary>Gets Renderers inside GameObject.</summary>
	public void GetRenderers()
	{
		renderers = GetComponentsInChildren<Renderer>();
	}

	/// <summary>Routine.</summary>
	public IEnumerator Routine()
	{
		/// Jagged Arrays: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/jagged-arrays
		FloatRange sinRange = new FloatRange(-1.0f, 1.0f);
		int length = renderers.Length;
		Material[][] materials = new Material[length][];
		Color[][] colors = new Color[length][];
		float inverseDuration = 1.0f / duration;
		float t = 0.0f;
		float x = 360.0f * cycles * Mathf.Deg2Rad;
		float s = 0.0f;

		for(int i = 0; i < length; i++)
		{
			materials[i] = renderers[i].materials;
			colors[i] = new Color[materials[i].Length];

			for(int j = 0; j < colors[i].Length; j++)
			{
				colors[i][j] = materials[i][j].GetColor(selfIlluminationTag);
			}
		}

		while(t < 1.0f)
		{
			s = VMath.RemapValueToNormalizedRange(Mathf.Sin(t * x), sinRange);

			for(int i = 0; i < length; i++)
			{
				for(int j = 0; j < colors[i].Length; j++)
				{
					materials[i][j].SetColor(selfIlluminationTag, Color.Lerp(colors[i][j], flashColor, s));
				}
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		for(int i = 0; i < length; i++)
		{
			for(int j = 0; j < colors[i].Length; j++)
			{
				materials[i][j].SetColor(selfIlluminationTag, colors[i][j]);
			}
		}
	}
}
}