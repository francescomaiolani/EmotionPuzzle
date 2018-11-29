using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MagicRoomTextToSpeechManagerOnline : MonoBehaviour {

    string receivigCodeExpression = "speachtotext";

    public static MagicRoomTextToSpeechManagerOnline instance;
    /// <summary>
    /// True if the smart applianced middleware is open, false otherwise
    /// </summary>
    public bool MagicRoomSpeachToText_active;

    /// <summary>
    /// list of the smart apliances found by the middleware
    /// </summary>
    public Voices[] listofAssociatedNames;

    /// <summary>
    /// http address of the middleware
    /// </summary>
    private string address;
    /// <summary>
    /// the command to be sent to the middleware
    /// </summary>
    private SpeachToTextCommand command;


    public List<string> listOfCompletedFiles = new List<string>();
    // Use this for initialization
    void Awake () {
        if (instance != null)
        {
            GameObject.DestroyImmediate(this);
        }
        else {
            instance = this;
            MagicRoomSpeachToText_active = true;
            address = "http://192.168.0.111:7074";
        }
	}

    void Start()
    {
        listOfCompletedFiles = new List<string>();
        getConfiguration();
    }

    void Update()
    {
    }

    public void ReadyFile(string filename) {

        fileSysntesysstatus f = JsonUtility.FromJson<fileSysntesysstatus>(filename);
        if(f.status == "OK")
        listOfCompletedFiles.Add(f.filename);
    }

    public void getConfiguration() {
        if (!MagicRoomSpeachToText_active)
        {
            return;
        }
        StartCoroutine(getConfigurationFromServer());
    }
    IEnumerator getConfigurationFromServer()
    {
        
        command = new SpeachToTextCommand();
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
            SpeachToTextCommandConfiguration conf = new SpeachToTextCommandConfiguration();
            conf = JsonUtility.FromJson<SpeachToTextCommandConfiguration>(www.downloadHandler.text);
            listofAssociatedNames = conf.voices;
            string log = "";
            foreach (Voices s in listofAssociatedNames)
            {
                log += "Available " + s.voiceName + " as a voice, ";
            }
            Logger.addToLogNewLine("ServerTTS", log);
        }

    }

    public void generateAudioFromText(string text, Voices voice, string filename)
    {
        if (!MagicRoomSpeachToText_active)
        {
            return;
        }
        if (File.Exists(Application.streamingAssetsPath + "/audio/" + filename)) {
            return;
        }
        command = new SpeachToTextCommand();
        command.action = "speechSynthesis";
        command.activityAddress = HttpListenerForMagiKRoom.instance.address + ":" + HttpListenerForMagiKRoom.instance.port + "/" + receivigCodeExpression;
        command.filename = filename + ".mp3";
        command.lang = voice.voiceLang;
        command.path = Application.streamingAssetsPath + "/audio/";
        command.text = text;
        command.voice = voice.voiceName;

        StartCoroutine(sendCommand());
        Logger.addToLogNewLine("ServerTTS", text + "," + voice.voiceName);
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
public class SpeachToTextCommand
{
    public string action;
    public string activityAddress;
    public string text;
    public string path;
    public string filename;
    public string lang;
    public string voice;
}
[Serializable]
public class SpeachToTextCommandConfiguration
{
    public Voices[] voices;
}

[Serializable]
public class Voices {
    public string voiceName;
    public string voiceGender;
    public string voiceLang;
}

[Serializable]
public class fileSysntesysstatus
{
    public string filename;
    public string status;
}