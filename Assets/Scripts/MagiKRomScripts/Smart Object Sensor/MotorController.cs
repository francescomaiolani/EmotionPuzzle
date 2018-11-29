using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour {
    /// <summary>
    /// is the motor active?
    /// </summary>
    public bool sensorEnabled;
    /// <summary>
    /// lsit of the motor state
    /// </summary>
    public ControllableMotor[] motors;

    /// <summary>
    /// configure the object
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names)
    {
        motors = new ControllableMotor[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            motors[i] = new ControllableMotor();
            motors[i].name = names[i];
            motors[i].isMoving = false;
            motors[i].position = "";
        }
    }
    /// <summary>
    /// update the state
    /// </summary>
    /// <param name="motorcontrollerstate"></param>
    internal void updateState(MotorControllerSerializedState motorcontrollerstate)
    {
        sensorEnabled = motorcontrollerstate.isEnabled;
        foreach (ControllableMotor t in motors)
        {
            for (int i = 0; i < motorcontrollerstate.code.Length; i++)
            {
                if (t.name == motorcontrollerstate.code[i])
                {
                    t.isMoving = motorcontrollerstate.isMoving[i];
                    t.position = motorcontrollerstate.position[i];
                }
            }
        }
    }
}
[Serializable]
public class ControllableMotor
{
    public string name;
    public bool isMoving;
    public string position;
}
