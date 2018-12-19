using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicRoomLightManager : MonoBehaviour {
 /// <summary>
    /// Singleton of the Script
    /// </summary>
    public static MagicRoomLightManager instance;
/// <summary>
    /// True if the smart light middleware is open, false otherwise
    /// </summary>    
public bool MagicRoomLightManager_active; 

/// <summary>
    /// http address of the middleware
    /// </summary>    
private string address;
 /// <summary>
    /// Object to comunicate with the iddleware
    /// </summary>    
private static LightCommand command;

/// <summary>
    /// list of the smart light identified by the middleware
    /// </summary>    
public string[] listofAssociatedNames;

    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else {
            GameObject.DestroyImmediate(this);
        }
        address = "http://localhost:7070";
        
        MagicRoomLightManager_active = true;
    }

    void Start()
    {
        Logger.addToLogNewLine("ServerHue", "Searching Light Server");
        StartCoroutine(sendConfigurationRequest());
    }

 /// <summary>
    /// Encodes the request to import the lst of associated names
    /// </summary>
    /// <returns></returns>    
IEnumerator sendConfigurationRequest()
    {
        LightCommand cmd = new LightCommand();
        cmd.Action = "getConfiguration";
        string json = JsonUtility.ToJson(cmd);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host")
            {
                MagicRoomLightManager_active = false;
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
    /// Send the command to the iddleware to change the room color.
    /// All the smart lights will take the color of your chose.
    /// </summary>
    /// <param name="color">the hexadecimal value of the colour preceded by #</param>
    /// <param name="brighness">the intensity of the light from 0 (switch off) to 255 (maxium brightness)</param>    
public void sendColour(string color, int brighness) {
        command = new LightCommand();
        command.Action = "LightCommand";
        if (!MagicRoomLightManager_active) {
            return;
        }
        color = color.ToLower();
        if (!(checkStringColour(color) && checkBrightness(brighness))) {
            return;
        }

        command.Color = color;
        command.Brightness = brighness.ToString();
        Logger.addToLogNewLine("Hue_allRoom", command.Color + "," + command.Brightness);
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// Send the command to the iddleware to change the room color.
    /// All the smart lights will take the color of your chose. brightness will be set to 100/255
    /// </summary>
    /// <param name="color">the hexadecimal value of the colour preceded by #</param>
    public void sendColour(string color)
    {
        command = new LightCommand();
        command.Action = "LightCommand";
        if (!MagicRoomLightManager_active)
        {
            return;
        }
        color = color.ToLower();
        if (!(checkStringColour(color)))
        {
            return;
        }

        command.Color = color;
        command.Brightness = "100";
        Logger.addToLogNewLine("Hue_allRoom", command.Color + "," + command.Brightness);
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// Send the command to the iddleware to change the room color.
    /// All the smart lights will take the color of your chose.
    /// </summary>
    /// <param name="colour">The color you want the lights to change alpha channel will be used to determine the brightness</param>
    public void sendColour(Color c)
    {
        command = new LightCommand();
        command.Action = "LightCommand";
        if (!MagicRoomLightManager_active)
        {
            return;
        }
        string color = ConvertColor(c);
        int brightness = ConvertBrightness(c);
        if (!(checkStringColour(color) && checkBrightness(brightness)))
        {
            return;
        }

        command.Color = color;
        command.Brightness = brightness.ToString();
        Logger.addToLogNewLine("Hue_allRoom", command.Color + "," + command.Brightness);
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// Send the command to the iddleware to change the room color.
    /// All the smart lights will take the color of your chose.
    /// </summary>
    /// <param name="colour">The color you want the lights to change alpha channel will be used to determine the brightness</param>
    /// <param name="brighness">the intensity of the light from 0 (switch off) to 255 (maxium brightness)</param>
    public void sendColour(Color c, int brightness)
    {
        command = new LightCommand();
        command.Action = "LightCommand";
        if (!MagicRoomLightManager_active)
        {
            return;
        }
        string color = ConvertColor(c);
        if (!(checkStringColour(color) && checkBrightness(brightness)))
        {
            return;
        }

        command.Color = color;
        command.Brightness = brightness.ToString();
        Logger.addToLogNewLine("Hue_allRoom", command.Color + "," + command.Brightness);
        StartCoroutine(sendCommand());
    }

    /// <summary>
    /// Convert the brightness form a color tyipe to int.
    /// </summary>
    /// <param name="c">the colur</param>
    /// <returns> the integer value of the brightness between 0 and 255</returns>    
    private int ConvertBrightness(Color c)
    {
        return (int)(c.a * 255);
    }

/// <summary>
    /// Convert the color into the hexadecimal format
    /// </summary>
    /// <param name="c">color to convert</param>
    /// <returns> the hexadecimal format of the color preceeded by #</returns>    
private string ConvertColor(Color c)
    {
        string col = "#";
        col += ((int)(c.r * 255)).ToString("X2");
        col += ((int)(c.g * 255)).ToString("X2");
        col += ((int)(c.b * 255)).ToString("X2");
        return col;
    }

/// <summary>
    /// Send the command to the iddleware to change the room color.
    /// </summary>
    /// <param name="color">the hexadecimal value of the colour preceded by #</param>
    /// <param name="brighness">the intensity of the light from 0 (switch off) to 255 (maxium brightness)</param>
    /// <param name="name">Name of the lght you want to change from the list of associated names</param>    
public void sendColour(string color, int brighness, string name)
    {
        command = new LightCommand();
        command.Action = "LightCommand";
        if (!MagicRoomLightManager_active)
        {
            return;
        }
        color = color.ToLower();
        if (!(checkStringColour(color) && checkBrightness(brighness)))
        {
            return;
        }

        command.Color = color;
        command.Brightness = brighness.ToString();
        command.id = name;
        Logger.addToLogNewLine(command.id, command.Color + "," + command.Brightness);
        StartCoroutine(sendCommand());
    }

/// <summary>
    /// Send the command to the iddleware to change the room color.
    /// </summary>
    /// <param name="colour">The color you want the lights to change alpha channel will be used to determine the brightness</param>
    /// <param name="name">Name of the lght you want to change from the list of associated names</param>
    public void sendColour(Color colour, string name)
    {
        command = new LightCommand();
        command.Action = "LightCommand";
        if (!MagicRoomLightManager_active)
        {
            return;
        }
        string color = ConvertColor(colour);
        int brightness = ConvertBrightness(colour);
        if (!(checkStringColour(color) && checkBrightness(brightness)))
        {
            return;
        }

        command.Color = color;
        command.Brightness = brightness.ToString();
        command.id = name;
        Logger.addToLogNewLine("Hue_allRoom", command.Color + "," + command.Brightness);
        StartCoroutine(sendCommand());
    }

    /// <summary>
    /// Send the command to the iddleware to change the room color.
    /// </summary>
    /// <param name="colour">The color you want the lights to change alpha channel will be used to determine the brightness</param>
    /// <param name="name">Name of the lght you want to change from the list of associated names</param>
    /// <param name="brighness">the intensity of the light from 0 (switch off) to 255 (maxium brightness)</param>
    public void sendColour(Color colour, int brightness, string name)
    {
        command = new LightCommand();
        command.Action = "LightCommand";
        if (!MagicRoomLightManager_active)
        {
            return;
        }
        string color = ConvertColor(colour);
        if (!(checkStringColour(color) && checkBrightness(brightness)))
        {
            return;
        }

        command.Color = color;
        command.Brightness = brightness.ToString();
        command.id = name;
        Logger.addToLogNewLine("Hue_allRoom", command.Color + "," + command.Brightness);
        StartCoroutine(sendCommand());
    }
    /// <summary>
    /// verify that the brighness passed is a valid value between 0 and 255
    /// </summary>
    /// <param name="b">the value of brightness</param>
    /// <returns></returns>    
    private bool checkBrightness(int b)
    {
        if (b < 0 || b > 255) {
            return false;
        }
        return true;
    }

/// <summary>
    /// verify that the color string passed is a valid hexadeciaml colour
    /// </summary>
    /// <param name="c">the string to check</param>
    /// <returns></returns>    
private bool checkStringColour(string c) {
        if ((c.Length != 7 || !c.StartsWith("#"))) {
            return false;
        }
        c = c.Substring(1, c.Length-1);
        c = c.ToLower();
        foreach (char ch in c) {
            if (!((ch >= 'a' && ch <= 'f') || (ch >= '0' && ch <= '9'))) {
                return false;
            }
        }
        return true;
    }

/// <summary>
    /// send the command to the middleware
    /// </summary>
    /// <returns></returns>    
IEnumerator sendCommand() {
        string json = JsonUtility.ToJson(command);
        if (command.id == null || command.id == "") {
            json = json.Substring(0, json.Length - 9) + "}";
        }
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put(address, myData);
        yield return www.Send();
        if (www.isNetworkError)
        {
            if (www.error == "Cannot connect to destination host") {
                MagicRoomLightManager_active = false;
            }
            
        }
    }
}

[Serializable]
public class LightCommand {
    public string Action;
    public string Color;
    public string Brightness;
    public string id = null;
}
