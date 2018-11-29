using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicRoomTextToSpeachManagerOffline : MonoBehaviour {

    string receivigCodeExpression = "speachtotextOffline";

    public static MagicRoomTextToSpeachManagerOffline instance;
    /// <summary>
    /// True if the smart applianced middleware is open, false otherwise
    /// </summary>
    public bool MagicRoomSpeachToText_active;

    /// <summary>
    /// list of the smart apliances found by the middleware
    /// </summary>
    public VoicesOffline[] listofAssociatedNames;

    /// <summary>
    /// http address of the middleware
    /// </summary>
    private string address;
    /// <summary>
    /// the command to be sent to the middleware
    /// </summary>
    private SpeachToTextOfflineCommand command;

    public bool isPlaying = false;
    
    // Use this for initialization
    void Awake()
    {
        if (instance != null)
        {
            GameObject.DestroyImmediate(this);
        }
        else
        {
            instance = this;
            MagicRoomSpeachToText_active = true;
            address = "http://localhost:7073";
        }
    }

    public void isCompleted() {
        Logger.addToLogNewLine("ServerTTSO", "endplay");
        isPlaying = false;
    }

    void Start()
    {
        getConfiguration();
    }
    

    public void getConfiguration()
    {
        if (!MagicRoomSpeachToText_active)
        {
            return;
        }
        StartCoroutine(getConfigurationFromServer());
    }
    IEnumerator getConfigurationFromServer()
    {

        command = new SpeachToTextOfflineCommand();
        command.action = "getVoicesList";
        string json = JsonUtility.ToJson(command);
        print(json);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        //UnityWebRequest www = UnityWebRequest.Post(address, json);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomSpeachToText_active = false;
            }
        }
        else
        {
            MagicRoomSpeachToText_active = true;
            Debug.Log(www.downloadHandler.text);
            SpeachToTextOfflineConfiguration conf = new SpeachToTextOfflineConfiguration();
            conf = JsonUtility.FromJson<SpeachToTextOfflineConfiguration>(www.downloadHandler.text);
            listofAssociatedNames = conf.voices;
            string log = "";
            foreach (VoicesOffline s in listofAssociatedNames)
            {
                log += "Available " + s.name + " as a voice, ";
            }
            Logger.addToLogNewLine("ServerTTSO", log);
        }

    }

    public void generateAudioFromText(string text, VoicesOffline voice)
    {
        if (!MagicRoomSpeachToText_active)
        {
            return;
        }
        command = new SpeachToTextOfflineCommand();
        command.action = "speechSynthesis";
        command.activityAddress = HttpListenerForMagiKRoom.instance.address + ":" + HttpListenerForMagiKRoom.instance.port + "/" + receivigCodeExpression;
        command.text = text;
        command.voice = voice.name;
        isPlaying = true;
        Logger.addToLogNewLine("ServerTTSO", text + "," + voice.name + " started");
        StartCoroutine(sendCommand());
    }

    /// <summary>
    /// send the http crequest to the smart appliance
    /// </summary>
    /// <returns></returns>
    IEnumerator sendCommand()
    {
        string json = JsonUtility.ToJson(command);
        print(json);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);

        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomSpeachToText_active = false;
            }
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}

[Serializable]
public class SpeachToTextOfflineCommand
{
    public string action;
    public string activityAddress;
    public string text;
    public string voice;
}

[Serializable]
public class VoicesOffline
{
    public string name;
    public string gender;
    public string language;
}

[Serializable]
public class SpeachToTextOfflineConfiguration
{
    public VoicesOffline[] voices;
}
