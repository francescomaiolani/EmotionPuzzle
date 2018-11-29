using System;
using UnityEngine;

public class TouchSensor:MonoBehaviour{
    /// <summary>
    /// is the sensor active?
    /// </summary>
    public bool sensorEnabled;
    /// <summary>
    /// state of the different touchpoint
    /// </summary>
    public TouchPoint[] touchpoints;

    /// <summary>
    /// configure the sensor
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names) {
        touchpoints = new TouchPoint[names.Length];
        for (int i = 0; i < names.Length; i ++) {
            touchpoints[i] = new TouchPoint();
            touchpoints[i].name = names[i];
            touchpoints[i].duration = 0;
            touchpoints[i].touched = false;
        }
    }

    internal void updateState(TouchSerializedState touchstate)
    {
        sensorEnabled = touchstate.isEnabled;
        foreach (TouchPoint t in touchpoints) {
            for (int i = 0; i < touchstate.touches.Length; i++) {
                if (t.name == touchstate.touches[i].nam) {
                    t.touched = (touchstate.touches[i].val == 1);
                    t.duration = touchstate.touches[i].dur;
                }
            }
        }
    }

    internal void updateFromUDP(string value, int duration)
    {
        foreach (TouchPoint p in touchpoints) {
            if (p.name == value) {
                p.touched = (duration == 0)? true : false;
                p.duration = duration;
            }
        }
    }
}
[Serializable]
public class TouchPoint {
    public string name;
    public bool touched;
    public int duration;
}




