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
	[SerializeField] private Health _health; 																		/// <summary>Health Component's Reference.</summary>
	[Space(5f)]
	[Header("Flash's Attributes:")]
	[TabGroup("Main", "Flash")][SerializeField] private bool _flash; 												/// <summary>Flash when damage is received?.</summary>
	[TabGroup("Main", "Flash")][SerializeField] private Renderer[] _renderers; 										/// <summary>Renderer's to flash.</summary>
	[TabGroup("Main", "Flash")][SerializeField] private Color _flashColor; 											/// <summary>Flash's Color.</summary>
	[TabGroup("Main", "Flash")][SerializeField] private string _blendTag; 											/// <summary>Flash Blend's Property Tag.</summary>
	[TabGroup("Main", "Flash")][SerializeField] private string _baseColorTag; 										/// <summary>Base Color's Property Tag.</summary>
	[TabGroup("Main", "Flash")][SerializeField] private string _flashColorTag; 										/// <summary>Flash Color's Property Tag.</summary>
	[TabGroup("Main", "Flash")][SerializeField] private float _duration; 											/// <summary>Flash's Duration.</summary>
	[TabGroup("Main", "Flash")][SerializeField] private float _cycles; 												/// <summary>Flash's Cycles.</summary>
	[Space(5f)]
	[Header("Shake' Attributes:")]
	[TabGroup("Main", "Shaking")][SerializeField] private bool _shake; 												/// <summary>Shake when damage is received?.</summary>
	[TabGroup("Main", "Shaking")][SerializeField] private Transform[] _shakeTransforms; 							/// <summary>Transforms to shake.</summary>
	[TabGroup("Main", "Shaking")][SerializeField] private float _shakeDuration; 									/// <summary>Shake's Duration.</summary>
	[TabGroup("Main", "Shaking")][SerializeField] private float _shakeSpeed; 										/// <summary>Shake's Speed.</summary>
	[TabGroup("Main", "Shaking")][SerializeField] private float _shakeMagnitude; 									/// <summary>Shake's Magnitude.</summary>
	[Space(5)]
	[Header("Particle Effects:")]
	[TabGroup("FXs", "Particle Effects")][SerializeField] private ParticleEffectEmissionData _hurtParticleEffect; 	/// <summary>Hurt Particle Effect's Emission Data.</summary>
	[Space(5f)]
	[Header("Sound Effects:")]
	[TabGroup("FXs", "Sound Effects")][SerializeField] private SoundEffectEmissionData _hurtSoundEffect; 			/// <summary>Hurt's Sound-Effect's Emission Data.</summary>
	private Material[][] _materials; 																				/// <summary>Materials contained on all Renderers.</summary>
	private Color[][] _colors; 																						/// <summary>Colors of all materials.</summary>
	private Coroutine flashRoutine; 																				/// <summary>Flash Coroutine's Reference.</summary>
	private Coroutine shakeRoutine; 																				/// <summary>Shake Coroutine's Reference.</summary>

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

	/// <summary>Gets and Sets shakeTransforms property.</summary>
	public Transform[] shakeTransforms
	{
		get { return _shakeTransforms; }
		set { _shakeTransforms = value; }
	}

	/// <summary>Gets and Sets flash property.</summary>
	public bool flash
	{
		get { return _flash; }
		set { _flash = value; }
	}

	/// <summary>Gets and Sets shake property.</summary>
	public bool shake
	{
		get { return _shake; }
		set { _shake = value; }
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

	/// <summary>Gets and Sets blendTag property.</summary>
	public string blendTag
	{
		get { return _blendTag; }
		set { _blendTag = value; }
	}

	/// <summary>Gets and Sets baseColorTag property.</summary>
	public string baseColorTag
	{
		get { return _baseColorTag; }
		set { _baseColorTag = value; }
	}

	/// <summary>Gets and Sets flashColorTag property.</summary>
	public string flashColorTag
	{
		get { return _flashColorTag; }
		set { _flashColorTag = value; }
	}

	/// <summary>Gets duration property.</summary>
	public float duration { get { return _duration; } }

	/// <summary>Gets cycles property.</summary>
	public float cycles { get { return _cycles; } }

	/// <summary>Gets shakeDuration property.</summary>
	public float shakeDuration { get { return _shakeDuration; } }

	/// <summary>Gets shakeSpeed property.</summary>
	public float shakeSpeed { get { return _shakeSpeed; } }

	/// <summary>Gets shakeMagnitude property.</summary>
	public float shakeMagnitude { get { return _shakeMagnitude; } }

	/// <summary>Gets hurtParticleEffect property.</summary>
	public ParticleEffectEmissionData hurtParticleEffect { get { return _hurtParticleEffect; } }

	/// <summary>Gets hurtSoundEffect property.</summary>
	public SoundEffectEmissionData hurtSoundEffect { get { return _hurtSoundEffect; } }

	/// <summary>Gets and Sets materials property.</summary>
	public Material[][] materials
	{
		get { return _materials; }
		protected set { _materials = value; }
	}

	/// <summary>Gets and Sets colors property.</summary>
	public Color[][] colors
	{
		get { return _colors; }
		protected set { _colors = value; }
	}
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
		
		if(renderers != null) foreach(Renderer renderer in renderers)
		{
			foreach(Material material in renderer.materials)
			{
				material.EnableKeyword("_EMISSION");
				if(material.HasProperty(flashColorTag)) material.SetColor(flashColorTag, flashColor);
			}
		}

		UpdateMaterialsColors();
	}

	/// <summary>Callback invoked when HealthEventReceiver's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		if(health != null) health.onHealthEvent -= OnHealthEvent;
	}

	/// <summary>Updates Materials' colors.</summary>
	public void UpdateMaterialsColors()
	{
		int length = renderers.Length;

		materials = new Material[length][];
		colors = new Color[length][];

		for(int i = 0; i < length; i++)
		{
			materials[i] = renderers[i].materials;
			colors[i] = new Color[materials[i].Length];

			/// Unnecessary...
			/*for(int j = 0; j < colors[i].Length; j++)
			{
				Material material = materials[i][j];

			}*/
		}
	}

	/// <summary>Returns Materials' colors to their default color.</summary>
	private void ReturnToDefaultMaterialsColors()
	{
		int length = renderers.Length;

		for(int i = 0; i < length; i++)
		{
			for(int j = 0; j < colors[i].Length; j++)
			{
				Material material = materials[i][j];
				
				if(material.HasProperty(blendTag)) material.SetFloat(blendTag, 0.0f);
			}
		}
	}

	/// <summary>Event invoked when a Health's event has occured.</summary>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
	/// <param name="_object">GameObject that caused the event, null be default.</param>
	private void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null)
	{
		//Debug.Log("[HealthEventReceiver] DUDE: " + _event.ToString());
		if(_event == HealthEvent.Depleted)
		{
			if(flash) this.StartCoroutine(FlashRoutine(), ref flashRoutine);
			if(shake) this.StartCoroutine(ShakeRoutine(), ref shakeRoutine);
			hurtParticleEffect.EmitParticleEffects();
			AudioController.PlayOneShot(SourceType.SFX, hurtSoundEffect.soundIndex, ResourcesManager.GetAudioClip(hurtSoundEffect.soundReference));
		}
	}

	/// <summary>Cancels Routine.</summary>
	public void CancelRoutine()
	{
		this.DispatchCoroutine(ref flashRoutine);
		this.DispatchCoroutine(ref shakeRoutine);
	}

	[Button("Get Renderers")]
	/// <summary>Gets Renderers inside GameObject.</summary>
	public void GetRenderers()
	{
		renderers = GetComponentsInChildren<Renderer>();
	}

	/// <summary>Flash's Routine.</summary>
	public IEnumerator FlashRoutine()
	{
		/// Jagged Arrays: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/jagged-arrays
		FloatRange sinRange = new FloatRange(-1.0f, 1.0f);
		int length = renderers.Length;
		float inverseDuration = 1.0f / duration;
		float t = 0.0f;
		float x = 360.0f * cycles * Mathf.Deg2Rad;
		float s = 0.0f;	

		ReturnToDefaultMaterialsColors();

		while(t < 1.0f)
		{
			s = VMath.RemapValueToNormalizedRange(Mathf.Sin(t * x), sinRange);

			for(int i = 0; i < length; i++)
			{
				for(int j = 0; j < colors[i].Length; j++)
				{
					Material material = materials[i][j];

					if(material.HasProperty(blendTag)) material.SetFloat(blendTag, s);
				}
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		ReturnToDefaultMaterialsColors();
	}

	/// <summary>Shake's Routine.</summary>
	public IEnumerator ShakeRoutine()
	{
		if(shakeTransforms == null) yield break;

		IEnumerator[] shakeRoutines = new IEnumerator[shakeTransforms.Length];
		int i = 0;

		foreach(Transform shakeTransform in shakeTransforms)
		{
			shakeRoutines[i] = shakeTransform.ShakePosition(shakeDuration, shakeSpeed, shakeMagnitude);
			i++;
		}

		i = 0;

		while(i < shakeTransforms.Length)
		{
			foreach(IEnumerator shakeRoutine in shakeRoutines)
			{
				if(!shakeRoutine.MoveNext()) i++;
			}

			yield return null;
		}
	}
}
}