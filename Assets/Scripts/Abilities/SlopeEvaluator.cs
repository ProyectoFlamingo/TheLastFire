using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(SensorSystem2D))]
[RequireComponent(typeof(OrientationNormalAdjuster))]
public class SlopeEvaluator : MonoBehaviour
{
	[SerializeField] private float _angleLimit; 			/// <summary>Angle's Limit.</summary>
	[SerializeField] private int _groundSensorID; 			/// <summary>Ground's Sensor ID.</summary>
	private float _dotLimit; 								/// <summary>Dot Product's Limit.</summary>
	private Vector3 _right; 								/// <summary>Right's orientation vector [not necessarily the same as Transform.right].</summary>
	private SensorSystem2D _sensorSystem; 					/// <summary>SensorSystem2D's Component.</summary>
	private OrientationNormalAdjuster _normalAdjuster; 		/// <summary>OrientationNormalAdjuster's Component.</summary>
	private bool _onSlope; 									/// <summary>Is the GameObject on a Slope?.</summary>

	/// <summary>Gets and Sets angleLimit property.</summary>
	public float angleLimit
	{
		get { return _angleLimit; }
		set
		{
			_angleLimit = value;
			_dotLimit = VMath.AngleToDotProduct(value);
		}
	}

	/// <summary>Gets and Sets dotLimit property.</summary>
	public float dotLimit
	{
		get { return _dotLimit; }
		set { _dotLimit = value; }
	}

	/// <summary>Gets and Sets groundSensorID property.</summary>
	public int groundSensorID
	{
		get { return _groundSensorID; }
		set { _groundSensorID = value; }
	}

	/// <summary>Gets and Sets right property.</summary>
	public Vector3 right
	{
		get { return _right; }
		set { _right = value; }
	}

	/// <summary>Gets sensorSystem Component.</summary>
	public SensorSystem2D sensorSystem
	{ 
		get
		{
			if(_sensorSystem == null) _sensorSystem = GetComponent<SensorSystem2D>();
			return _sensorSystem;
		}
	}

	/// <summary>Gets normalAdjuster Component.</summary>
	public OrientationNormalAdjuster normalAdjuster
	{ 
		get
		{
			if(_normalAdjuster == null) _normalAdjuster = GetComponent<OrientationNormalAdjuster>();
			return _normalAdjuster;
		}
	}

	/// <summary>Gets and Sets onSlope property.</summary>
	public bool onSlope
	{
		get { return _onSlope; }
		private set { _onSlope = value; }
	}

	/// <summary>SlopeEvaluator's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		dotLimit = _dotLimit = VMath.AngleToDotProduct(angleLimit);
		onSlope = false;
	}
	
	/// <summary>SlopeEvaluator's tick at each frame.</summary>
	private void LateUpdate ()
	{
		RaycastHit2D hit = default(RaycastHit2D);
		Vector3 up = Vector3.zero;

		sensorSystem.GetSubsystemDetection(groundSensorID, out hit);

		up = hit.normal.ToVector3();

		float dot = Vector3.Dot(up, Vector3.up);

		normalAdjuster.up = hit.HasInfo() && dot >= dotLimit ? up : Vector3.up;
		onSlope = dot != 1.0f;
	}

	/// <summary>Calculates a proper orientation given a movement axis.</summary>
	/// <param name="_movementAxis">Movement's Axis.</param>
	/// <returns>Proper Orientation given a movement axis.</returns>
	public Vector2 GetOrientation(Vector2 _movementAxis)
	{
		bool right = _movementAxis.x > 0.0f;
		normalAdjuster.forward = right ? Vector3.forward : Vector3.back;

		return normalAdjuster.right;
	}

	/// <returns>String representing this SlopeEvaluator Component.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("SlopeEvaluator: \n{");
		builder.Append("\tOn Slope = ");
		builder.AppendLine(onSlope.ToString());

		if(onSlope)
		{
			builder.Append("\tSlope Angle = ");
			builder.Append(Vector3.Angle(normalAdjuster.up, Vector3.up));
			builder.AppendLine(" (Degrees)");
		}
		
		builder.Append("}");

		return builder.ToString();
	}
}
}