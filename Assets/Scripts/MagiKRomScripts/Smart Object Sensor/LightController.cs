using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    /// <summary>
    /// is the light controller active?
    /// </summary>
    public bool sensorEnabled;
    /// <summary>
    /// list of the state of the controllable ights
    /// </summary>
    public ControllableLight[] lights;

    /// <summary>
    /// setup the cofiguration
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names)
    {
        lights = new ControllableLight[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            lights[i] = new ControllableLight();
            lights[i].name = names[i];
            lights[i].color = Color.black;
            lights[i].brightness = 0;
        }
    }
    /// <summary>
    /// update the state of the actators
    /// </summary>
    /// <param name="lightControllerstate"></param>
    internal void updateState(LightControllerSerializedState lightControllerstate)
    {
        sensorEnabled = lightControllerstate.isEnabled;
        foreach (ControllableLight t in lights)
        {
            for (int i = 0; i < lightControllerstate.lights.Length; i++)
            {
                if (t.name == lightControllerstate.lights[i].code)
                {
                    t.color = ColorParse(lightControllerstate.lights[i].color);
                    t.brightness = lightControllerstate.lights[i].brightness;
                }
            }
        }
    }

    /// <summary>
    /// convert the color from hex to Color
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private Color ColorParse(string v)
    {
        if (!v.StartsWith("#"))
        {
            v = "#" + v;
        }
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(v, out myColor);
        return myColor;
    }

    /// <summary>
    /// return the list of lights available from the object
    /// </summary>
    /// <returns></returns>
    public List<string> getConfigurationList()
    {
        List<string> conf = new List<string>();
        foreach (ControllableLight l in lights)
        {
            conf.Add(l.name);
        }
        return (conf);
    }
}

[Serializable]
public class ControllableLight
{
    public string name;
    public Color color;
    public int brightness;
}
