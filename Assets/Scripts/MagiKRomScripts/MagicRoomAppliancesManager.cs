using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicRoomAppliancesManager : MonoBehaviour
{

    /// <summary>
    /// Singleton of the Script
    /// </summary>
    public static MagicRoomAppliancesManager instance;
    /// <summary>
    /// True if the smart applianced middleware is open, false otherwise
    /// </summary>
    public bool MagicRoomAppliancesManager_active;

    /// <summary>
    /// list of the smart apliances found by the middleware
    /// </summary>
    public string[] listofAssociatedNames;

    /// <summary>
    /// http address of the middleware
    /// </summary>
    private string address;
    /// <summary>
    /// the command to be sent to the middleware
    /// </summary>    
    private SmartPlugCommand command;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        address = "http://localhost:7071";
        command = new SmartPlugCommand();
        command.type = "SmartPlugCommand";
        MagicRoomAppliancesManager_active = true;
    }
    void Start()
    {
        Logger.addToLogNewLine("ServerSP", "Searched Magic Room Appliances");
        StartCoroutine(sendConfigurationRequest());
    }

    /// <summary>
    /// Encodes the request to import the list of associated names
    /// </summary>
    /// <returns></returns>
    IEnumerator sendConfigurationRequest()
    {
        SmartPlugCommand cmd = new SmartPlugCommand();
        cmd.type = "SmartPlugDiscovery";
        string json = JsonUtility.ToJson(cmd);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomAppliancesManager_active = false;
            }
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ServerSmartPlugConfiguration conf = new ServerSmartPlugConfiguration();
            conf = JsonUtility.FromJson<ServerSmartPlugConfiguration>(www.downloadHandler.text);
            listofAssociatedNames = conf.configuration;
            string log = "";
            foreach (string s in listofAssociatedNames)
            {
                log += "Found " + s + " on the network, ";
            }
            Logger.addToLogNewLine("ServerSP", log);
        }
    }

    /// <summary>
    /// Send the command to change the state of a smart appliance
    /// </summary>
    /// <param name="appliance"> the name of the appliance you want to switch from the list of associated names</param>
    /// <param name="cmd">"ON" to switch on the applaice, "OFF" otherwise</param>
    public void sendChangeCommand(string appliance, string cmd)
    {
        if (!MagicRoomAppliancesManager_active)
        {
            return;
        }
        cmd = cmd.ToUpper();
        if (!checkCommand(cmd))
        {
            return;
        }

        command.command = cmd;
        command.id = appliance;
        Logger.addToLogNewLine(appliance, cmd.ToUpper());
        StartCoroutine(sendCommand());
    }

    /// <summary>
    /// check that the command is valid
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private bool checkCommand(string c)
    {
        if (c != "ON" && c != "OFF")
        {
            return false;
        }
        return true;
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
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomAppliancesManager_active = false;
            }
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}

[Serializable]
public class SmartPlugCommand
{
    public string type;
    public string command;
    public string id;
}

[Serializable]
public class ServerSmartPlugConfiguration
{
    public string[] configuration;
}
