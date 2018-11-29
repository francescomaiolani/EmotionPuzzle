using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicRoomKinectV2Manager : MonoBehaviour {

/// <summary>
    /// Singleton of the script
    /// </summary>
    public static MagicRoomKinectV2Manager instance;
    /// <summary>
    /// true if the kinect V2 middleware is open, false otherwise
    /// </summary>
    public bool MagicRoomKinectV2Manager_active;
    /// <summary>
    /// true if the Kinect is sampling the data, false otherwive
    /// </summary>
    public bool MagicRoomKinectV2Manager_sampling;
    /// <summary>
    /// handler of http events comeing to the system from the kinect middleware when position is detected
    /// </summary>
    const string receivigCodeExpression = "kinectposition";
    /// <summary>
    /// handler of http events comeing to the system from the kinect middleware when audio is detected
    /// </summary>
    const string receivigCodeExpressionAudio = "kinectaudio";

    /// <summary>
    /// address of the middleware
    /// </summary>
    private string address;
    /// <summary>
    /// command to be sent to the middleware
    /// </summary>
    private KinectCommand command;
    
    /// <summary>
    /// which mode set to read from the kinect
    /// </summary>
    private KinectReadMode _kinectreadmode;

    /// <summary>
    /// the skeletons read form the kinect
    /// </summary>
    public KinectBodySkeleton[] skeletons;

    /// <summary>
    /// lastcommand that has been detected by the inect audio record
    /// </summary>
    private string _recognizedCommand;

    /// <summary>
    /// threshold to recognize gestures
    /// </summary>
    float threshold = 0.07f;
    /// <summary>
    /// which mode set to read from the kinect
    /// </summary>    
public KinectReadMode Kinectreadmode
    {
        get
        {
            return _kinectreadmode;
        }

        set
        {
            _kinectreadmode = value;
        }
    }
    /// <summary>
    /// get the last recognized command from the Kinect Sensor. 
    /// It is a token , once you read it is cleared
    /// </summary>
    public string RecognizedCommand
    {
        get
        {
            if (_recognizedCommand != "")
            {
                string temp = _recognizedCommand;
                _recognizedCommand = "";
                return temp;
            }
            else {
                return "";
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        instance = this;
        address = "http://localhost:7080";
        MagicRoomKinectV2Manager_active = true;
        MagicRoomKinectV2Manager_sampling = false;
        skeletons = new KinectBodySkeleton[6];
    }

    void Start()
    {
        Logger.addToLogNewLine("ServerKinect", "Searched Magic Room Kinect Server");
    }

/// <summary>
    /// last audio command recognized by the Kinet middleware for logging purpose only
    /// </summary>
    public string _lastreadedcommand;
    void Update()
    {
        if (MagicRoomKinectV2Manager_active && MagicRoomKinectV2Manager_sampling)
        {
            readLastSamplingKinect(_kinectreadmode);
        }

        if (_lastreadedcommand != "") {
            Logger.addToLogNewLine("ServerKinect", "identified the world " + _lastreadedcommand + " from the Kinect.");
            _lastreadedcommand = "";
        }
    }

 /// <summary>
    /// Define the kinect sampling capability
    /// </summary>
    /// <param name="frequency">number of sampling that have to be taken by the kinect (in streming mode)</param>
    /// <param name="window">the number of elements saved into the window</param>    
public void setUpKinect(int frequency, int window)
    {
        string listeningaddress = HttpListenerForMagiKRoom.instance.address + ":" + HttpListenerForMagiKRoom.instance.port + "/" + receivigCodeExpression;
        if (!MagicRoomKinectV2Manager_active)
        {
            return;
        }
        command = new KinectCommand();
        command.type = "KinectCommand";
        command.command = "SetUpKinect";
        command.frequency = frequency;
        command.window = window;
        command.listeningaddress = listeningaddress;
        Logger.addToLogNewLine("ServerKinect", "SetUp Kinekt V2 sampling frequency " + frequency + " and window " + window);
        StartCoroutine(sendCommand());
    }
/// <summary>
    /// Send the command to the middleware to start sampling
    /// </summary>
    /// <param name="samplingmode"></param>    
public void startSamplingKinect(KinectSamplingMode samplingmode)
    {
        if (!MagicRoomKinectV2Manager_active)
        {
            return;
        }
        command = new KinectCommand();
        command.type = "KinectCommand";
        command.command = "StartKinect";
        if (samplingmode == KinectSamplingMode.Polling)
        {
            command.option = "Polling";
            MagicRoomKinectV2Manager_sampling = true;
        }
        else {
            command.option = "Streaming";
        }
        Logger.addToLogNewLine("ServerKinect", "Started Kinect sampling " + samplingmode.ToString());
        StartCoroutine(sendCommand());
        
    }
/// <summary>
    /// stop the sampling for the Kinect
    /// </summary>    
public void stopSamplingKinect()
    {
        if (!MagicRoomKinectV2Manager_active)
        {
            return;
        }
        command = new KinectCommand();
        command.type = "KinectCommand";
        command.command = "StopKinect";
        Logger.addToLogNewLine("ServerKinect", "Stopped Kinect sampling");
        StartCoroutine(sendCommand());
        MagicRoomKinectV2Manager_sampling = false;
    }
 /// <summary>
    /// read the last sampling of the body parts obtined by the server
    /// </summary>
    /// <param name="readmode"></param>    
public void readLastSamplingKinect(KinectReadMode readmode)
    {
        if (!MagicRoomKinectV2Manager_active)
        {
            return;
        }
        command = new KinectCommand();
        command.type = "KinectCommand";
        command.command = "ReadKinect";
        if (readmode == KinectReadMode.Single)
        {
            command.option = "Single";
        }
        else
        {
            command.option = "Window";
        }
        StartCoroutine(sendCommand());
    }

/// <summary>
    /// define the gestures that are to be recognized by the kinect, if present in the database
    /// </summary>
    /// <param name="activegesture">key is the gesture name, value is the percentage of confidence</param>    
public void setGestureRecognitionKinect(Dictionary<string, int> activegesture)
    {
        if (!MagicRoomKinectV2Manager_active)
        {
            return;
        }
        command = new KinectCommand();
        command.type = "KinectCommand";
        command.command = "SetGesture";
        string formattedstring = "";
        foreach (string key in activegesture.Keys) {
            formattedstring += key + ":" + activegesture[key] + ",";
        }
        formattedstring = formattedstring.Substring(0, formattedstring.Length - 1);
        command.option = formattedstring;
        StartCoroutine(sendCommand());
    }
/// <summary>
    /// set up the audio command to be recognized
    /// </summary>
    /// <param name="worldtorecognize">list of terms to be recognized by the kinect</param>    
public void setVoiceRecognitionKinect(List<string> worldtorecognize)
    {
        if (!MagicRoomKinectV2Manager_active)
        {
            return;
        }
        command = new KinectCommand();
        command.type = "KinectCommand";
        command.command = "SetAudioRecognition";
        command.listeningaddress = HttpListenerForMagiKRoom.instance.address + ":" + HttpListenerForMagiKRoom.instance.port + "/" + receivigCodeExpressionAudio;
        string formattedstring = "";
        foreach (string key in worldtorecognize)
        {
            formattedstring += key + ",";
        }
        formattedstring = formattedstring.Substring(0, formattedstring.Length - 1);
        command.option = formattedstring;
        StartCoroutine(sendCommand());
    }

/// <summary>
    /// send the http command to the middleware
    /// </summary>
    /// <returns></returns>    
IEnumerator sendCommand()
    {
        string json = JsonUtility.ToJson(command);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        yield return www.Send();
        if (www.isNetworkError)
        {
            Debug.Log("Kinectmanager " + www.isNetworkError + " " +  www.error);
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomKinectV2Manager_active = false;
            }
        }
    }

/// <summary>
    /// transform the message from the middlewar into the data about the skeletons 
    /// </summary>
    /// <param name="json"></param>    
public void setSkeletons(string json) {
        json = "{\"skeletons\" : " + json + "}";
        json = json.Replace("X", "x");
        json = json.Replace("Y", "y");
        json = json.Replace("Z", "z");
        FrameFromKinectServer frame = JsonUtility.FromJson<FrameFromKinectServer>(json);
        skeletons = frame.skeletons;
        //Logger.addToLogNewLine("identified Skeletons from Kinect Server " + json);
    }

/// <summary>
    /// trasform the message from the middleware nto the data for audio recognition
    /// </summary>
    /// <param name="json"></param>    
public void detectedAudio(string json)
    {
        AudioEventFromKinectServer frame = JsonUtility.FromJson<AudioEventFromKinectServer>(json);
        _recognizedCommand = frame.world;
        _lastreadedcommand = frame.world;
    }

}

public enum KinectSamplingMode {
    Polling, Streaming
}
public enum KinectReadMode
{
    Single, Window
}

[Serializable]
public class KinectCommand
{
    public string type;
    public string command;
    public string option;
    public string listeningaddress;
    public int frequency;
    public int window;
}