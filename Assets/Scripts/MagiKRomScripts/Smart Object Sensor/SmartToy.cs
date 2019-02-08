using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SmartToy : MonoBehaviour
{
    /// <summary>
    /// the name of the smart toy
    /// </summary>
    public string smartToyName;
    /// <summary>
    /// the touch sensor of the device, null if not preesent
    /// </summary>
    public TouchSensor touchsensor;
    /// <summary>
    /// the buttons of the device
    /// </summary>
    public ButtonSensor button;
    /// <summary>
    /// the RFID reader of the device
    /// </summary>
    public RFIDReader rfidsensor;
    /// <summary>
    /// the position handler of the device
    /// </summary>
    public PositionReader objectposition;
    /// <summary>
    /// the light controller of the device
    /// </summary>
    public LightController lightcontroller;
    /// <summary>
    /// the sound controller of the device
    /// </summary>
    public SoundEmitterController soundemitter;
    /// <summary>
    /// the motor controller of the device
    /// </summary>
    public MotorController motorcontroller;
    /// <summary>
    /// the video emitter of the device
    /// </summary>
    public VideoEmitterController videoemitter;
    /// <summary>
    /// the effect controller of the device
    /// </summary>
    public EffectController effectcontroller;
    /// <summary>
    /// the frequency of the stream on UDP channel
    /// </summary>
    public int stream_frequency;
    /// <summary>
    /// the battery level of the deivce
    /// </summary>
    public int battery;

    private void Awake()
    {
        touchsensor = gameObject.GetComponent<TouchSensor>();
        button = gameObject.GetComponent<ButtonSensor>();
        rfidsensor = gameObject.GetComponent<RFIDReader>();
        objectposition = gameObject.GetComponent<PositionReader>();
        lightcontroller = gameObject.GetComponent<LightController>();
        soundemitter = gameObject.GetComponent<SoundEmitterController>();
        motorcontroller = gameObject.GetComponent<MotorController>();
        videoemitter = gameObject.GetComponent<VideoEmitterController>();
        effectcontroller = gameObject.GetComponent<EffectController>();
    }

    private void Start()
    { }

    /// <summary>
    /// ecode the cofiguation for each objects
    /// </summary>
    /// <param name="conf"></param>
    public void decodeConfiguration(SmartToyConfiguration conf)
    {
        gameObject.name = conf.name;
        smartToyName = conf.name;
        if (conf.effects.Length > 0)
        {
            effectcontroller.setUpConfiguration(conf.effects);
        }
        else
        {
            effectcontroller.enabled = false;
        }
        if (conf.touch_config.Length > 0)
        {
            touchsensor.setUpConfiguration(conf.touch_config);
        }
        if (conf.button_config.Length > 0)
        {
            button.setUpConfiguration(conf.button_config);
        }
        else
        {
            touchsensor.enabled = false;
        }
        if (conf.button_config.Length > 0)
        {
            touchsensor.setUpConfiguration(conf.button_config);
        }
        else
        {
            button.enabled = false;
        }
        if (conf.rfid_config.Length > 0)
        {
            rfidsensor.setUpConfiguration(conf.rfid_config, conf.rfid_reader_name_config);
        }
        else
        {
            rfidsensor.enabled = false;
        }
        if (conf.lightemitter_config.Length > 0)
        {
            lightcontroller.setUpConfiguration(conf.lightemitter_config);
        }
        else
        {
            lightcontroller.enabled = false;
        }
        if (conf.soundemitter_config.Length > 0)
        {
            soundemitter.setUpConfiguration(conf.soundemitter_config);
        }
        else
        {
            soundemitter.enabled = false;
        }
        if (conf.motorcontroller_config.Length > 0)
        {
            motorcontroller.setUpConfiguration(conf.motorcontroller_config);
        }
        else
        {
            motorcontroller.enabled = false;
        }
        if (conf.videoemitter_config.Length > 0)
        {
            videoemitter.setUpConfiguration(conf.videoemitter_config);
        }
        else
        {
            videoemitter.enabled = false;
        }
        if (conf.acceleormeter_reader_name_config.Length > 0 || conf.gyroscope_reader_name_config.Length > 0)
        {
            objectposition.setAccelerometers(conf.acceleormeter_reader_name_config);
            objectposition.setGyroscopes(conf.gyroscope_reader_name_config);
        }
    }

    internal void updateState(SmartToyUpdateStatus conf)
    {
        battery = conf.Devicestatusstate.battery;
        stream_frequency = conf.Devicestatusstate.streamFreq;
        if (touchsensor != null && conf.Touchstate.isEnabled)
        {
            touchsensor.updateState(conf.Touchstate);
        }
        if (button != null && conf.Buttonstate.isEnabled)
        {
            button.updateState(conf.Buttonstate);
        }
        if (rfidsensor != null && conf.RFIDstate.isEnabled)
        {
            rfidsensor.updateState(conf.RFIDstate);
        }
        if (objectposition != null && conf.Positionstate.isEnabled)
        {
            objectposition.updateState(conf.Positionstate);
        }
        if (lightcontroller != null && conf.LightControllerstate.isEnabled)
        {
            lightcontroller.updateState(conf.LightControllerstate);
        }
        if (soundemitter != null && conf.SoundEmitterstate.isEnabled)
        {
            soundemitter.updateState(conf.SoundEmitterstate);
        }
        if (motorcontroller != null && conf.Motorcontrollerstate.isEnabled)
        {
            motorcontroller.updateState(conf.Motorcontrollerstate);
        }
        if (videoemitter != null && conf.VideoEmitterstate.isEnabled)
        {
            videoemitter.updateState(conf.VideoEmitterstate);
        }
        if (effectcontroller != null && conf.Effectstate.isEnabled)
        {
            effectcontroller.updateState(conf.Effectstate);
        }
    }

    /// <summary>
    /// Command are executed at the end of the frame, where all the requess have been alrady processed during the Update phase
    /// </summary>
    SmartToyExecuteStatus executecommand = null;
    SmartToyGetStatus getstatus = null;
    void LateUpdate()
    {
        if (MagicRoomSmartToyManager.instance.MagicRoomSmartToyManager_active)
        {
            //once I reach this the whole update frame has been computed
            if (executecommand != null)
            {
                //send the data
                string json = executecommand.ToJson(); //JsonUtility.ToJson(executecommand);
                MagicRoomSmartToyManager.instance.sendCommandExecuteSmartToy(smartToyName, json);
                //clean up executecommand
                executecommand = null;
            }
            if (getstatus != null)
            {
                //send the data
                string json = JsonUtility.ToJson(getstatus);
                MagicRoomSmartToyManager.instance.sendCommandGetStateSmartToy(smartToyName, json);
                //clean up executecommand
                getstatus = null;
            }
        }
    }
    /// <summary>
    /// turn on the touch sensor
    /// </summary>
    public void switchOnTouchSensor()
    {
        executeCommandTouchSensor(true);
    }
    /// <summary>
    /// turn off the touch sensor
    /// </summary>
    public void switchOffTouchSensor()
    {
        executeCommandTouchSensor(false);
    }

    private void executeCommandTouchSensor(bool turnonsensor)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        executecommand.touch = turnonsensor;
    }
    private void executeCommandButtonSensor(bool turnonsensor)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        executecommand.button = turnonsensor;
    }
    /// <summary>
    /// turn on the buttons
    /// </summary>
    public void switchOnButtonSensor()
    {
        executeCommandButtonSensor(true);
    }
    /// <summary>
    /// turn off the buttons
    /// </summary>
    public void switchOffButtonSensor()
    {
        executeCommandButtonSensor(false);
    }
    /// <summary>
    /// turn on the RFID reader
    /// </summary>
    public void switchOnRFIDSensor()
    {
        executeCommandRFIDSensor(true);
    }
    /// <summary>
    /// turn off the RFID reader
    /// </summary>
    public void switchOffRFIDSensor()
    {
        executeCommandRFIDSensor(false);
    }
    private void executeCommandRFIDSensor(bool turnonsensor)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        executecommand.rfid = turnonsensor;
    }
    /// <summary>
    /// turn on the position sensor
    /// </summary>
    public void switchOnPositionSensor()
    {
        executeCommandPositionSensor(true, true, true);
    }
    /// <summary>
    /// turn off the position sensor
    /// </summary>
    public void switchOffPositionSensor()
    {
        executeCommandPositionSensor(false, false, false);
    }
    private void executeCommandPositionSensor(bool turnonAccelerometer, bool turnonGyroscope, bool turnonPosition)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        executecommand.accelerometer = turnonAccelerometer;
        executecommand.gyroscope = turnonGyroscope;
        executecommand.position = turnonPosition;
    }
    /// <summary>
    /// Execute a command on the light controller
    /// </summary>
    /// <param name="color">color to be assigned to the led</param>
    /// <param name="brightness">brightness of the led</param>
    /// <param name="lightpartcode">the name of the liht part form the configuration of the light controller component</param>
    public void executeCommandLightController(Color color, int brightness, string lightpartcode = null)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        if (executecommand.lightsControllerCommand != null)
        {
            List<lightControllerSetter> temp = executecommand.lightsControllerCommand.ToList();
            lightControllerSetter t = new lightControllerSetter();
            t.code = lightpartcode;
            t.brightness = brightness;
            t.color = "#" + ColorUtility.ToHtmlStringRGB(color);
            temp.Add(t);
            executecommand.lightsControllerCommand = temp.ToArray();
        }
        else
        {
            lightControllerSetter t = new lightControllerSetter();
            t.code = lightpartcode;
            t.brightness = brightness;
            t.color = "#" + ColorUtility.ToHtmlStringRGB(color);
            executecommand.lightsControllerCommand = new lightControllerSetter[1];
            executecommand.lightsControllerCommand[0] = t;
        }
    }

    internal void updateStateFromUDP(udpPackage pack)
    {
        if (pack.accelerometer != null && pack.accelerometer.Length == 3)
        {
            objectposition.setAccelerometer(pack.accelerometer);
        }
        if (pack.gyroscope != null && pack.gyroscope.Length == 3)
        {
            objectposition.setGyroscope(pack.gyroscope);
        }
        if (pack.position != null && pack.position.Length == 3)
        {
            objectposition.setPosition(pack.position);
        }
        foreach (UDPEvent e in pack.state)
        {
            switch (e.typ)
            {
                case "touchsensor":
                    touchsensor.updateFromUDP(e.val, e.dur);
                    break;
                case "button":
                    button.updateFromUDP(e.id, e.val, e.dur);
                    break;
                case "rfidsensor":
                    rfidsensor.updateFromUDP(e.id, e.val, e.dur);
                    break;
                    //case "accelerometer": objectposition.updateAccFromUDP(e.id, e.val, e.dur); break;
                    //case "gyroscope": objectposition.updateGyrFromUDP(e.id, e.val, e.dur); break;
                case "position":
                    objectposition.updatePosFromUDP(e.id, e.val, e.dur);
                    break;
                default:
                    break;
            }
        }
    }

    internal void updateStateFromTCP(tcpPackage pack)
    {

        foreach (UDPEvent e in pack.events)
        {
            Logger.addToLogNewLine(smartToyName, " sensor " + e.typ + " " + e.id + " value " + e.val + " " + e.dur);
            switch (e.typ)
            {
                case "touchsensor":
                    touchsensor.updateFromUDP(e.val, e.dur);
                    break;
                case "button":
                    button.updateFromUDP(e.id, e.val, e.dur);
                    break;
                case "rfidsensor":
                    rfidsensor.updateFromUDP(e.id, e.val, e.dur);
                    break;
                    //case "accelerometer": objectposition.updateAccFromUDP(e.id, e.val, e.dur); break;
                    //case "gyroscope": objectposition.updateGyrFromUDP(e.id, e.val, e.dur); break;
                case "position":
                    objectposition.updatePosFromUDP(e.id, e.val, e.dur);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// execute a command for the video emitter
    /// </summary>
    /// <param name="videoname">the name of the video, form the tracks in video controller component</param>
    /// <param name="volume">the volume of the player</param>
    /// <param name="repeat">the vieo shoul e put in repeat mode</param>
    /// <param name="state">the state of the player</param>
    public void executeCommandVideoEmitter(string videoname, int volume, bool repeat, SoundAndVideoState state)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        executecommand.videoEmitterCommand = new videoEmitterSet();
        executecommand.videoEmitterCommand.repeat = repeat;
        executecommand.videoEmitterCommand.state = state.ToString().Split('.') [1];
        executecommand.videoEmitterCommand.videoname = videoname;

    }
    /// <summary>
    /// xecute a commadn on audio player
    /// </summary>
    /// <param name="trackname">the name of the track form the sound controller compoentn</param>
    /// <param name = "volume" > the volume of the player</param>
    /// <param name="repeat">the vieo shoul e put in repeat mode</param>
    /// <param name="state">the state of the player</param>
    public void executeCommandSoundEmitter(string trackname, int volume, bool repeat, SoundAndVideoState state)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        executecommand.soundEmitterCommand = new soundEmitterSet();
        executecommand.soundEmitterCommand.repeat = repeat;
        executecommand.soundEmitterCommand.state = state.ToString().Split('.') [1];
        executecommand.soundEmitterCommand.trackname = trackname;
    }
    /// <summary>
    /// start an effect
    /// </summary>
    /// <param name="effecttoactivate">name of the effect form the list of names of effects in the effect state compoentn</param>
    public void executeCommandEffectEmitter(string effecttoactivate)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        executecommand.effectnametoActivate = effecttoactivate;
    }
    /// <summary>
    /// execute a command on the motor controller
    /// </summary>
    /// <param name="motorcode">the motor on which perform</param>
    /// <param name="value">the value of the motor: for vibrational and contunous motor represent the duration of the motion, for step motor the number of steps for servo the angle to reach</param>
    public void executeCommandMotorController(string motorcode, float value)
    {
        if (executecommand == null)
        {
            executecommand = new SmartToyExecuteStatus();
        }
        if (executecommand.motorControlCommands != null)
        {
            List<MotorControllerSetter> temp = executecommand.motorControlCommands.ToList();
            MotorControllerSetter t = new MotorControllerSetter();
            t.code = motorcode;
            t.destination = value;
            temp.Add(t);
            executecommand.motorControlCommands = temp.ToArray();
        }
        else
        {
            MotorControllerSetter t = new MotorControllerSetter();
            t.code = motorcode;
            t.destination = value;
            executecommand.motorControlCommands = new MotorControllerSetter[1];
            executecommand.motorControlCommands[0] = t;
        }

    }
    /// <summary>
    /// ask to get the state of the touch sensor
    /// </summary>
    public void getstateofTouchsensor()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "touch";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("touch");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the RFID reader
    /// </summary>
    public void getstateofRfidsensor()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "rfid";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("rfid");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the Position
    /// </summary>
    public void getstateofPositionsensor()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "position";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("position");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the gyroscope
    /// </summary>
    public void getstateofGyroscopesensor()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "gyroscope";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("gyroscope");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the Accelrometer
    /// </summary>
    public void getstateofAccelerometersensor()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "accelerometer";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("accelerometer");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the effect
    /// </summary>
    public void getstateofEffect()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "effect";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("effect");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the sound emitter
    /// </summary>
    public void getstateofSoundEmitter()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "soundEmitter";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("soundEmitter");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the video emitter
    /// </summary>
    public void getstateofVideoEmitter()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "videoEmitter";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("videoEmitter");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the light controller
    /// </summary>
    public void getstateofLightController()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "light";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("light");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the motor controller
    /// </summary>
    public void getstateofMotorController()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "motor";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("motor");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the device state
    /// </summary>
    public void getstateofDeviceStatus()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "deviceStatus";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("deviceStatus");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of the buttons
    /// </summary>
    public void getstateofButtonSensor()
    {
        if (getstatus == null)
        {
            getstatus = new SmartToyGetStatus();
            getstatus.requestType = "get";
            getstatus.components = new string[1];
            getstatus.components[0] = "button";
        }
        else
        {
            List<string> s = getstatus.components.ToList<string>();
            s.Add("button");
            getstatus.components = s.ToArray();
        }

    }
    /// <summary>
    /// ask to get the state of all the sensors
    /// </summary>
    public void getstateofSensors()
    {
        getstateofAccelerometersensor();
        getstateofButtonSensor();
        getstateofGyroscopesensor();
        getstateofPositionsensor();
        getstateofRfidsensor();
        getstateofTouchsensor();

    }
    /// <summary>
    /// ask to get the state of all the actuators
    /// </summary>
    public void getstateofActuators()
    {
        getstateofEffect();
        getstateofLightController();
        getstateofMotorController();
        getstateofSoundEmitter();
        getstateofVideoEmitter();

    }
    /// <summary>
    /// ask to get the state of all the compoennts in the smart toys
    /// </summary>
    public void getstateofSmartToy()
    {
        getstateofSensors();
        getstateofActuators();
        getstateofDeviceStatus();
    }
}
