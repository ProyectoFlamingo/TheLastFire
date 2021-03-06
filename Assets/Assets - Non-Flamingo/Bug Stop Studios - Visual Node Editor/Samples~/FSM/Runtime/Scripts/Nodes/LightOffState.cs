using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualGraphRuntime;

[NodeName(_name: "Light Off")]
[CustomNodeStyle("LightOffStyle")]
public class LightOffState : BaseState
{
	float currentDelay = 0.0f;
	Light light;

	public override void OnEnter()
	{
		base.OnEnter();

		GameObject light_go = null;
		fsm.GetPropertyValue<GameObject>("Light", ref light_go);
		if (light_go != null)
		{
			light = light_go.GetComponent<Light>();
			if (light != null)
			{
				light.enabled = false;
			}
		}
		fsm.GetPropertyValue<float>("Delay", ref currentDelay);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		currentDelay -= Time.deltaTime;
		if (currentDelay <= 0.0f)
		{
			fsm.GoToState(FSMPort.State.On);
		}
	}
}
