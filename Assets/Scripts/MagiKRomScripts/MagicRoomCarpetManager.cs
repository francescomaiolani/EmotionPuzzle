using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicRoomCarpetManager : MonoBehaviour {

/// <summary>
    /// Singleton of the script
    /// </summary>
    public static MagicRoomCarpetManager instance;
    /// <summary>
    /// True if the smart carpet middleware is open, false otherwise
    /// </summary>
    public bool MagicRoomCarpetManager_active;

    /// <summary>
    /// list of the command available on the smart carpet
    /// </summary>
    public string[] listofAssociatedNames;

    /// <summary>
    /// addressof the middleware
    /// </summary>
    private string address;
    /// <summary>
    /// command to be sent at the middleware
    /// </summary>
    private CarpetCommand cmd;
    

    // Use this for initialization
    void Awake()
    {
        instance = this;
        address = "http://localhost:7072";
        cmd = new CarpetCommand();
        cmd.type = "SmartCarpetCommand";
        MagicRoomCarpetManager_active = true;
    }

/// <summary>
    /// send a command to be actuated on the carpet
    /// </summary>
    /// <param name="command">the command to be sent to the carpet form the list of assocated names</param>
    public void sendCommand(string command)
    {

        cmd.command = command;
        StartCoroutine(sendCommand());
    }
    void Start()
    {
        Logger.addToLogNewLine("ServerCarpet", "Searched Magic Room Carpet");
        StartCoroutine(sendConfigurationRequest());
    }

/// <summary>
    /// send the http request to obtain all the carpet command available
    /// </summary>
    /// <returns></returns>
    IEnumerator sendConfigurationRequest()
    {
        SmartPlugCommand cmd = new SmartPlugCommand();
        cmd.type = "SmartCarpetDiscovery";
        string json = JsonUtility.ToJson(cmd);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomCarpetManager_active = false;
            }
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ServerSmartPlugConfiguration conf = new ServerSmartPlugConfiguration();
            conf = JsonUtility.FromJson<ServerSmartPlugConfiguration>(www.downloadHandler.text);
            listofAssociatedNames = conf.configuration;
        }
    }

/// <summary>
    /// send the command to the smart carpet
    /// </summary>
    /// <param name="command">the command form the list of associated names</param>
    public void sendChangeCommand(string command)
    {
        if (!MagicRoomCarpetManager_active)
        {
            return;
        }


        cmd.type = "SmartCarpetCommand";
        cmd.command = command;
        Logger.addToLogNewLine("ServerCarpet", "Sent command: activate the effect on the carpet " + command);
        StartCoroutine(sendCommand());
    }

/// <summary>
    /// perform the http connaction to the middleware to send data
    /// </summary>
    /// <returns></returns>
    IEnumerator sendCommand()
    {
        string json = JsonUtility.ToJson(cmd);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomCarpetManager_active = false;
            }

        }
    }
}

[Serializable]
public class CarpetCommand
{
    public string type;
    public string command;
}