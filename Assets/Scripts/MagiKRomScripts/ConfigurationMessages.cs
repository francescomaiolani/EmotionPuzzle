using System;

[Serializable]
public class SmartToyConfiguration {
    public string name;
    public string[] rfid_config;
    public string[] rfid_reader_name_config;
    public string[] touch_config;
    public string[] button_config;
    public string[] soundemitter_config;
    public string[] videoemitter_config;
    public string[] lightemitter_config;
    public string[] motorcontroller_config;
    public string[] effects;
    public string[] acceleormeter_reader_name_config;
    public string[] gyroscope_reader_name_config;
}

[Serializable]
public class SmartToyUpdateStatus
{
    public string responseType;
    public ButtonSerializedState Buttonstate;
    public LightControllerSerializedState LightControllerstate;
    public DeviceStatusSerializedState Devicestatusstate;
    public RFIDSerializedState RFIDstate;
    public PositionSerializedState Positionstate;
    public TouchSerializedState Touchstate;
    public EffectSerializedState Effectstate;
    public SoundEmitterSerializedState SoundEmitterstate;
    public MotorControllerSerializedState Motorcontrollerstate;
    public VideoEmitterSerializedState VideoEmitterstate;
}

[Serializable]
public class VideoEmitterSerializedState
{
    public bool isEnabled;

    public string videoName;

    public int volume;

    public string state;

    public bool repeat;
}

[Serializable]
public class MotorControllerSerializedState
{
    public bool isEnabled;

    public string[] code;

    public bool[] isMoving;

    public string[] position;
}

[Serializable]
public class SoundEmitterSerializedState
{
    public bool isEnabled;

    public string trackName;

    public int volume;

    public string state;

    public bool repeat;
}

[Serializable]
public class LightControllerSerializedState
{
    public bool isEnabled;

    public LightState[] lights;
}

[Serializable]
public class LightState
{
    public string code;
    public string color;
    public int brightness;
}

[Serializable]
public class EffectSerializedState
{
    public bool isEnabled;

    public bool isPlaying;

    public string effectName;
}

[Serializable]
public class TouchSerializedState
{
    public bool isEnabled;

    public TouchEvent[] touches;
}

[Serializable]
public class ButtonSerializedState
{
    public bool isEnabled;

    public ButtonEvent[] buttons;
}

[Serializable]
public class ButtonEvent {
    public string nam;
    public bool val;
    public int dur;
}
[Serializable]
public class TouchEvent
{
    public string nam;
    public int val;
    public int dur;
}

[Serializable]
public class PositionSerializedState
{
    public bool isEnabled;

    public float[] accelerometer;

    public float[] gyroscope;

    public float[] position;
    public string namsesensorGyroscope;
    public string namsesensorAccelerometer;
}

[Serializable]
public class RFIDSerializedState
{
    public bool isEnabled;
    public string code;
    public int duration;
}

[Serializable]
public class DeviceStatusSerializedState {
    public int battery;

    public string id;

    public int streamFreq;
}

[Serializable]
public class SmartToyExecuteStatus {
    public bool? rfid;

    public bool? touch;

    public bool? button;

    public bool? gyroscope;

    public bool? accelerometer;

    public bool? position;

    public soundEmitterSet soundEmitterCommand;

    public videoEmitterSet videoEmitterCommand;

    public lightControllerSetter[] lightsControllerCommand;

    public string effectnametoActivate;

    public MotorControllerSetter[] motorControlCommands;

    public SmartToyExecuteStatus()
    {
        rfid = null;

        touch = null;

        button = null;

        gyroscope = null;

        accelerometer = null;

        position = null;

        soundEmitterCommand = null;

        videoEmitterCommand = null;

        lightsControllerCommand = null;

        effectnametoActivate = null;

        motorControlCommands = null;
}
    public string ToJson() {
        string json = "{";
        if (rfid != null) {
            json += "\"rfid\" : " + rfid.ToString().ToLower() + ", ";
        }
        if (button != null)
        {
            json += "\"button\" : " + button.ToString().ToLower() + ", ";
        }
        if (touch != null)
        {
            json += "\"touch\" : " + touch.ToString().ToLower() + ", ";
        }
        if (gyroscope != null) {
            json += " \"gyroscope\" : " + gyroscope.ToString().ToLower() + ", ";
        }
        if (accelerometer != null)
        {
            json += " \"accelerometer\" : " + accelerometer.ToString().ToLower() + ", ";
        }
        if (position != null)
        {
            json += " \"position\" : " + position.ToString().ToLower() + ", ";
        }
        if (soundEmitterCommand != null)
        {
            json += " \"soundEmitterCommand\" : " + UnityEngine.JsonUtility.ToJson(soundEmitterCommand) + ", ";
        }
        if (videoEmitterCommand != null)
        {
            json += " \"videoEmitterCommand\" : " + UnityEngine.JsonUtility.ToJson(videoEmitterCommand) + ", ";
        }
        if (lightsControllerCommand != null)
        {
            json += " \"lightsControllerCommand\" : [";
            for (int i = 0; i < lightsControllerCommand.Length; i++)
            {
                json += UnityEngine.JsonUtility.ToJson(lightsControllerCommand[i]);
                if (i < lightsControllerCommand.Length - 2) {
                    json += ", ";
                }
            }
            json += "], ";
        }
        if (effectnametoActivate != null)
        {
            json += "\"effectnametoActivate\" : " + effectnametoActivate + ", ";
        }
        if (motorControlCommands != null)
        {
            json += " \"motorControlCommands\" : " + UnityEngine.JsonUtility.ToJson(motorControlCommands) + ", ";
        }
        json = json.Substring(0, json.Length - 2);
        json += "}";
        return json;
    }
}

[Serializable]
public class soundEmitterSet {
    public string trackname;

    public int volume;

    public bool repeat;

    public string state;
}
[Serializable]
public class videoEmitterSet
{
    public string videoname;

    public int volume;

    public bool repeat;

    public string state;
}

[Serializable]
public class lightControllerSetter
{
    public string code;

    public string color;

    public int brightness;
}

[Serializable]
public class MotorControllerSetter
{
    public string code;

    public float destination;
}

[Serializable]
public class SmartToyGetStatus
{
    public string requestType;

    public string[] components;
}