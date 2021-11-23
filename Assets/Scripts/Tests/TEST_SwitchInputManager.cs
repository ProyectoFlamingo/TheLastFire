using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
#if UNITY_SWITCH  || NN_PLUGIN_ENABLE	
using nn.hid;
#endif	
namespace Flamingo
{
/*
Debugs on Test 0:

- Handheld Mode:

	ID 0
	NpadID No1
	Style None

	ID 1
	NpadID Handheld
	Style Handheld

- JoyCon Left Off:

	ID 0
	NpadID No1
	Style JoyDual

	ID 1
	NpadID Handheld
	Style None

- JoyCon Right Off:

	ID 0
	NpadID No1
	Style JoyDual

	ID 1
	NpadID Handheld
	Style None

- JoyCon Left & Right Off:

	ID 0
	NpadID No1
	Style JoyDual

	ID 1
	NpadID Handheld
	Style None
*/

public class TEST_SwitchInputManager : MonoBehaviour
{
	public NintendoSwitchButton TESTSwitchButton;
	public Rect rect;
	private Vector2 leftAxes;
	private Vector2 rightAxes;
	private string ERROR_MESSAGE;
	private StringBuilder additionalBuilder;

	private void OnGUI()
	{
		GUI.Label(rect, ToString());
	}

#if UNITY_SWITCH  || NN_PLUGIN_ENABLE
	[SerializeField] private Mateo mateo; 										/// <summary>Mateo's Reference.</summary>
	[Space(5f)]
	[SerializeField] private NpadId[] supportedIDTypes; 						/// <summary>Supported ID Types.</summary>
	[SerializeField] private NpadStyle supportedNpadStyles; 					/// <summary>Supported Controller's Styles.</summary>
	[SerializeField] private NpadJoyHoldType supportedHoldTypes; 				/// <summary>Supported NpadJoy's Types.</summary>
	[Space(5f)]
	[Header("Input's Settings:")]
	[SerializeField] private NintendoSwitchButton attackInput; 					/// <summary>Attack's Input.</summary>
	[SerializeField] private NintendoSwitchButton jumpInput; 					/// <summary>Jump's Input.</summary>
	[SerializeField] private NintendoSwitchButton fireConjuringFrontalInput0; 	/// <summary>Fire Conjuring Frontal Input #0.</summary>
	[SerializeField] private NintendoSwitchButton fireConjuringFrontalInput1; 	/// <summary>Fire Conjuring Frontal Input #1.</summary>
	[SerializeField] private NintendoSwitchButton toggleSupportedTypeInput; 	/// <summary>Toggle Supported Hold Type Input #1.</summary>
	private NpadState[] NpadStates;

	/// <summary>TEST_SwitchInputManager's instance initialization.</summary>
	private void Awake()
	{
		CompareEnums();
		additionalBuilder = new StringBuilder();

		try
		{
			NpadStates = new NpadState[1];
			NpadStates[0] = new NpadState();
			Npad.Initialize();
			/*Npad.SetSupportedStyleSet(supportedNpadStyles);
			NpadJoy.SetHoldType(supportedHoldTypes);
			Npad.SetSupportedIdType(supportedIDTypes);*/
			Npad.SetSupportedIdType(supportedIDTypes);
			NpadJoy.SetHoldType(supportedHoldTypes);
		}
		catch(Exception e)
		{
			ERROR_MESSAGE = e.Message;

			/*supportedNpadStyles = NpadStyle.Handheld;
			supportedIDTypes = new NpadId[1];

			Npad.SetSupportedStyleSet(supportedNpadStyles);
			NpadJoy.SetHoldType(supportedHoldTypes);
			Npad.SetSupportedIdType(supportedIDTypes);*/
		}
		finally
		{
			BindIDs(true);
		}
	}

	/// <summary>Binds set of supported IDs.</summary>
	/// <param name="_bind">Bind them? True by default.</param>
	private void BindIDs(bool _bind = true)
	{
		foreach(NpadId ID in supportedIDTypes)
		{
			NpadJoy.SetAssignmentModeSingle(ID);
			switch(_bind)
			{
				case true:
				Npad.BindStyleSetUpdateEvent(ID);
				break;

				case false:
				Npad.DestroyStyleSetUpdateEvent(ID);
				break;
			}	
		}
	}
	
	/// <summary>TEST_SwitchInputManager's tick at each frame.</summary>
	private void Update ()
	{
		if(mateo == null) return;

		NpadState state = NpadStates[0];

		foreach(NpadId ID in supportedIDTypes)
		{
			NpadStyle style = Npad.GetStyleSet(ID);

			if(style == NpadStyle.None)
			{
				//Npad.DestroyStyleSetUpdateEvent(ID);
				continue;
			}
			/*else
			{
				Npad.BindStyleSetUpdateEvent(ID);
			}*/

			Npad.GetState(ref state, ID, style);
			AnalogStickState rightAnalogStick = state.analogStickR;
			AnalogStickState leftAnalogStick = state.analogStickL;
			rightAxes = ID.IDStyleToLeftAxis(new Vector2(rightAnalogStick.fx, rightAnalogStick.fy));
			leftAxes = ID.IDStyleToLeftAxis(new Vector2(leftAnalogStick.fx, leftAnalogStick.fy));

			if(state.GetButton(ID.IDStyleToNpadButton(attackInput)))
			{
				mateo.SwordAttack(leftAxes);
			}
			if(state.GetButton(ID.IDStyleToNpadButton(jumpInput)))
			{
				mateo.Jump(leftAxes);
			}
			if(state.GetButton(ID.IDStyleToNpadButton(toggleSupportedTypeInput)))
			{
				ToggleSupportedType();
			}

			if(leftAxes.x != 0.0f) mateo.Move(leftAxes.WithY(0.0f));

			mateo.OnLeftAxesChange(leftAxes);
			mateo.OnRightAxesChange(rightAxes);
		}


		HandleIDsStates();
		//BindIDs(true);
	}

	private void HandleIDsStates()
	{
		int i = 0;
		additionalBuilder.Clear();
		additionalBuilder.AppendLine("IDs' Handling:");
		additionalBuilder.AppendLine();

		foreach(NpadId ID in supportedIDTypes)
		{
			bool updated = Npad.IsStyleSetUpdated(ID);
			NpadStyle style = Npad.GetStyleSet(ID);

			additionalBuilder.Append("Iteration #");
			additionalBuilder.Append(i.ToString());
			additionalBuilder.AppendLine(": ");
			additionalBuilder.Append("Current Npad ID: ");
			additionalBuilder.AppendLine(ID.ToString());
			additionalBuilder.Append("Is Style Set Updated? ");
			additionalBuilder.AppendLine(updated.ToString());
			additionalBuilder.Append("Current ID's Style Set: ");
			additionalBuilder.AppendLine(style.ToString());
			additionalBuilder.Append("Style's Set [Npad.GetStyleSet(ID)]: ");
			additionalBuilder.AppendLine(Npad.GetStyleSet(ID).ToString());
			additionalBuilder.AppendLine("\n");
		
			i++;
		}

		additionalBuilder.AppendLine();
	}

	private void ToggleSupportedType()
	{
		supportedHoldTypes = supportedHoldTypes == NpadJoyHoldType.Horizontal ? NpadJoyHoldType.Vertical : NpadJoyHoldType.Horizontal;
		NpadJoy.SetHoldType(supportedHoldTypes);
	}

	private void CompareEnums()
	{
#if UNITY_SWITCH
		StringBuilder b = new StringBuilder();
		b.AppendLine();

		for(int i = 0; i < 28; i++)
		{
			NintendoSwitchButton nsB = (NintendoSwitchButton)(0x1 << i);
			NpadButton nB = (NpadButton)(nsB);
			bool equal = ((int)(nsB) == (int)(nB));

			b.Append("Is ");
			b.Append(nsB.ToString());
			b.Append(" equal to ");
			b.Append(nB.ToString());
			b.Append("? ");
			b.AppendLine(equal.ToString());
		}

		Debug.Log("[TEST_SwitchInputManager] Evaluating 2 different Enums: " + b.ToString());
#endif
	}

	public override string ToString()
	{
		NpadState state = NpadStates[0];
		StringBuilder builder = new StringBuilder();
		int  i = 0;

		builder.Append("Supported Style's Set: ");
		builder.AppendLine(Npad.GetSupportedStyleSet().ToString());
		builder.Append("Supported Hold Types: ");
		builder.AppendLine(supportedHoldTypes.ToString());
		builder.AppendLine("\n");

		foreach(NpadId ID in supportedIDTypes)
		{
			NpadStyle style = Npad.GetStyleSet(ID);
			Npad.GetState(ref state, ID, style);
			AnalogStickState rightAnalogStick = state.analogStickR;
			AnalogStickState leftAnalogStick = state.analogStickL;
			rightAxes = new Vector2(rightAnalogStick.fx, rightAnalogStick.fy);
			leftAxes = new Vector2(leftAnalogStick.fx, leftAnalogStick.fy);

			if(!string.IsNullOrEmpty(ERROR_MESSAGE))
			{
				builder.Append("ERROR MESSAGE: ");
				builder.AppendLine(ERROR_MESSAGE);
			}
			builder.Append("ID with index ");
			builder.AppendLine(i.ToString());
			builder.Append("Npad ID: ");
			builder.AppendLine(ID.ToString());
			builder.Append("Style: ");
			builder.AppendLine(style.ToString());
			builder.Append("Left-Axes: " );
			builder.AppendLine(leftAxes.ToString());
			builder.Append("Right-Axes: " );
			builder.AppendLine(rightAxes.ToString());
			builder.Append("N-Pad State: ");
			builder.AppendLine(state.ToString());
			builder.AppendLine();

			i++;
		}

		builder.Append(additionalBuilder.ToString());

		return builder.ToString();
	}
#endif
}
}