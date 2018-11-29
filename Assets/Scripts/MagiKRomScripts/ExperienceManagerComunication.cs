using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExperienceManagerComunication : MonoBehaviour {
    public static ExperienceManagerComunication instance;
	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            GameObject.DestroyImmediate(this);
        }
	}

    public void SendReadyCommand() {
        StartCoroutine(sendCommand("Ready"));
    }

    public void SendConcludedCommand()
    {
        StartCoroutine(sendCommand("Concluded"));
    }

    public void SendStartedCommand()
    {
        StartCoroutine(sendCommand("Started"));
    }

    public void SendWinCommand()
    {
        StartCoroutine(sendCommand("Win"));
    }

    IEnumerator sendCommand(string command)
    {
        string json = "{ \"state\": \"" + command + "\"}";
        print(json);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put("http://localhost:7100", myData);
        yield return www.Send();
        if (www.isNetworkError)
        {

        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    public void SendNextPlayerRequest()
    {
        StartCoroutine(sendCommand("Turn"));
    }
}
