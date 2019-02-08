using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSensor : MonoBehaviour
{
    /// <summary>
    /// is the sensor active?
    /// </summary>
    public bool sensorEnabled;
    /// <summary>
    /// The different states for each buttons
    /// </summary>
    public ButtonPoint[] buttonpoints;

    /// <summary>
    /// configure the sensor
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names)
    {
        buttonpoints = new ButtonPoint[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            buttonpoints[i] = new ButtonPoint();
            buttonpoints[i].name = names[i];
            buttonpoints[i].duration = 0;
            buttonpoints[i].pressed = false;
        }
    }
    /// <summary>
    /// update the state of the sensor
    /// </summary>
    /// <param name="buttonstate"></param>
    internal void updateState(ButtonSerializedState buttonstate)
    {
        sensorEnabled = buttonstate.isEnabled;
        foreach (ButtonPoint t in buttonpoints)
        {
            for (int i = 0; i < buttonstate.buttons.Length; i++)
            {
                if (t.name == buttonstate.buttons[i].nam)
                {
                    t.pressed = buttonstate.buttons[i].val;
                    t.duration = buttonstate.buttons[i].dur;
                }
            }
        }
    }

    /// <summary>
    /// update form the UDP stream
    /// </summary>
    /// <param name="value"></param>
    /// <param name="active"></param>
    /// <param name="duration"></param>
    internal void updateFromUDP(string value, string active, int duration)
    {
        foreach (ButtonPoint p in buttonpoints)
        {
            if (p.name == value)
            {
                p.pressed = (active == "1") ? true : false;
                p.duration = duration;
            }
        }
    }
}

[Serializable]
public class ButtonPoint
{
    public string name;
    public bool pressed;
    public int duration;
}
