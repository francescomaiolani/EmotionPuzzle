using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicRoomSmartToyManager : MonoBehaviour {
    /// <summary>
    /// singleton of the script
    /// </summary>
    public static MagicRoomSmartToyManager instance;
    /// <summary>
    /// true if the smart toy middleware is open, false otherwise
    /// </summary>
    public bool MagicRoomSmartToyManager_active;
    /// <summary>
    /// receiving udp address for smart objectstream
    /// </summary>
    const string receivigUDPAddress = "http://localhost:7001";

    /// <summary>
    /// prefab to be replicated for each smart object found on the middleware
    /// </summary>
    public GameObject SmartToyPrefab;

    /// <summary>
    /// address of the middleware
    /// </summary>
    private string address;
    /// <summary>
    /// command to be sent ot the smart object
    /// </summary>
    private SmartToyCommand command;
    /// <summary>
    /// handler of the http request comeing from the middleware for event management
    /// </summary>
    private string receivigCodeExpression = "smarttoyevent";

    void Awake()
    {
        instance = this;
        address = "http://192.168.31.214:7081";
        MagicRoomSmartToyManager_active = true;
    }

    void Start()
    {
        Logger.addToLogNewLine("ServerToy", "Discoverying server smart toy");
        SendDiscoverySmartToyMessage();
        retrieveConfigurationSmartToy();
    }

    /// <summary>
    /// retrieve all the gameobject of the different smart toys
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getSmartToys() {
        List<GameObject> temp = new List<GameObject>();
        Transform t = transform.GetChild(1);
        for (int i = 0; i < t.childCount; i++)
        {
            temp.Add(t.GetChild(i).gameObject);
        }
        return temp;
    }

    /// <summary>
    /// send the message to discover if the middleware is actve
    /// </summary>
    public void SendDiscoverySmartToyMessage()
    {
        if (!MagicRoomSmartToyManager_active)
        {
            return;
        }
        command = new SmartToyCommand();
        command.type = "DiscoveryMessage";
        string listeningaddress = HttpListenerForMagiKRoom.instance.address + ":" + HttpListenerForMagiKRoom.instance.port + "/" + receivigCodeExpression;
        command.value = listeningaddress;
        StartCoroutine(sendCommand());
    }

    /// <summary>
    /// send the message to discover all the smart objects registered in the middleware
    /// </summary>
    public void retrieveConfigurationSmartToy()
    {
        if (!MagicRoomSmartToyManager_active)
        {
            return;
        }
        command = new SmartToyCommand();
        command.type = "getAllDevicesAndConfiguration";
        command.value = "";
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// command t open the streaming udp for the smart object
    /// </summary>
    /// <param name="deviceName">nameof the smart object who has to open the stream</param>
    /// <param name="freq">frequency of the udp stream</param>
    public void openStreamSmartToy(string deviceName, float freq)
    {
        if (!MagicRoomSmartToyManager_active)
        {
            return;
        }
        command = new SmartToyCommand();
        command.type = "SmartToyCommand";
        command.command = "OpenStreamRequest";
        command.namedevice = deviceName;
        command.value = UDPListenerForMagiKRoom.instance.address + ":" + UDPListenerForMagiKRoom.instance.port + "/" + receivigCodeExpression;
        command.frequency = freq;
        Logger.addToLogNewLine("ServerToy", "Open stream UDP for smart object " + deviceName + " with frequency " + freq);
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// open the event channel from the smart toy to the applicain to signal events
    /// </summary>
    /// <param name="deviceName">the name of the deivce who has to to open the stream</param>
    public void openEventChannelSmartToy(string deviceName)
    {
        if (!MagicRoomSmartToyManager_active)
        {
            return;
        }
        command = new SmartToyCommand();
        command.type = "SmartToyCommand";
        command.command = "OpenChannelEvent";
        command.namedevice = deviceName;
        command.value = HttpListenerForMagiKRoom.instance.address + ":" + HttpListenerForMagiKRoom.instance.port + "/" + receivigCodeExpression;
        Logger.addToLogNewLine("ServerToy", "Open event channel for smart bject " + deviceName);
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// close the stream for the smart object
    /// </summary>
    /// <param name="deviceName"></param>
    public void closeStreamSmartToy(string deviceName)
    {
        if (!MagicRoomSmartToyManager_active)
        {
            return;
        }
        command = new SmartToyCommand();
        command.type = "SmartToyCommand";
        command.command = "CloseStreamRequest";
        command.namedevice = deviceName;
        StartCoroutine(sendCommand());
        Logger.addToLogNewLine("ServerToy", "Close stream UDP for smart object " + deviceName);
        UDPListenerForMagiKRoom.instance.StopReceiver();
    }
    
    /// <summary>
    /// perform some execution command on a device
    /// </summary>
    /// <param name="deviceName">name of the device that has to operate</param>
    /// <param name="request">the command to excute</param>
    public void sendCommandExecuteSmartToy(string deviceName, string request)
    {
        if (!MagicRoomSmartToyManager_active)
        {
            return;
        }
        command = new SmartToyCommand();
        command.type = "SmartToyCommand";
        command.command = "ExecuteCommand";
        command.namedevice = deviceName;
        command.value = request;
        Logger.addToLogNewLine(deviceName, request);
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// ask the smart toy to provide its state
    /// </summary>
    /// <param name="deviceName">the name of the device to ask</param>
    /// <param name="request">list of components to ask</param>
    public void sendCommandGetStateSmartToy(string deviceName, string request)
    {
        if (!MagicRoomSmartToyManager_active)
        {
            return;
        }
        command = new SmartToyCommand();
        command.type = "SmartToyCommand";
        command.command = "GetState";
        command.namedevice = deviceName;
        command.value = request;
        Logger.addToLogNewLine(deviceName, "state");
        StartCoroutine(sendCommand());
    }

    /// <summary>
    /// update the smart object from an event form event stream
    /// </summary>
    /// <param name="content"></param>
    internal void updateFromTCPEvent(string content)
    {
        Debug.Log(content);
        tcpPackage package = JsonUtility.FromJson<tcpPackage>(content);
        updatefromTCPmessage(package);
        Logger.addToLogNewLine("ServerToy", "Got TCP event " + content);
    }
    /// <summary>
    /// updae the state of the object from the UDP channel stream
    /// </summary>
    /// <param name="content"></param>
    internal void updateFromUDPEvent(string content)
    {
        udpPackage package = JsonUtility.FromJson<udpPackage>(content);
        updatefromUDPmessage(package);
        Logger.addToLogNewLine("ServerToy", "Got UDP event " + content);
    }

    IEnumerator sendCommand()
    {
        string messagetype = command.type;
        string json = JsonUtility.ToJson(command);
        print(json);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        yield return www.Send();
        
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomSmartToyManager_active = false;
            }
        }
        else
        {
            manageResponse(www.downloadHandler.text, messagetype, command.command);
        }
    }
    /// <summary>
    /// deterine how the message should be handles from the middleware
    /// </summary>
    /// <param name="response"></param>
    /// <param name="messagetype"></param>
    /// <param name="command"></param>
    private void manageResponse(string response, string messagetype, string command)
    {
        Debug.Log(response);
        if (response == "Smart toy server active")
        {
            MagicRoomSmartToyManager_active = true;
        }
        else
        {
            if (response == "Smart Object not reachable on the network")
            {

            }
            else
            {
                if (messagetype == "getAllDevicesAndConfiguration")
                {
                    response = "{\"configurations\" : " + response + "}";
                    SmartToyConfigurationMessage conf = JsonUtility.FromJson<SmartToyConfigurationMessage>(response);
                    for (int i = 0; i < conf.configurations.Length; i++)
                    {
                        GameObject sm = GameObject.Instantiate(SmartToyPrefab);
                        sm.GetComponent<SmartToy>().decodeConfiguration(conf.configurations[i]);
                        sm.transform.SetParent(transform.GetChild(1));
                        Logger.addToLogNewLine("ServerToy", "Found configuration for smart object " + conf.configurations[i].name);
                    }
                }
                else
                {
                    if (command == "GetState")
                    {
                        SmartToyUpdateStatus conf = JsonUtility.FromJson<SmartToyUpdateStatus>(response);
                        for (int i = 0; i < transform.GetChild(1).childCount; i++)
                        {
                            if (transform.GetChild(1).GetChild(i).name == conf.Devicestatusstate.id) {
                                transform.GetChild(1).GetChild(i).GetComponent<SmartToy>().updateState(conf);
                            }
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// update the device from udp stream
    /// </summary>
    /// <param name="pack"></param>
    public void updatefromUDPmessage(udpPackage pack) {
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            if (transform.GetChild(1).GetChild(i).name == pack.id)
            {
                transform.GetChild(1).GetChild(i).GetComponent<SmartToy>().updateStateFromUDP(pack);
            }
        }
    }
    /// <summary>
    /// update the message form event messages
    /// </summary>
    /// <param name="pack"></param>
    public void updatefromTCPmessage(tcpPackage pack)
    {
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            if (transform.GetChild(1).GetChild(i).name == pack.id)
            {
                transform.GetChild(1).GetChild(i).GetComponent<SmartToy>().updateStateFromTCP(pack);
            }
        }
    }

}

[Serializable]
public class SmartToyCommand
{
    public string type;
    public string command;
    public string namedevice;
    public string value;
    public float frequency;
}

[Serializable]
public class SmartToyConfigurationMessage {
    public SmartToyConfiguration[] configurations;
}
