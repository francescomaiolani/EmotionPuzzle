using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Logger : MonoBehaviour {
    static string filepath;
    static List<string> log = new List<string>();
    static public string SessionID = "SA_PELPX4";
    static int autoincrement = 0;
    public static Logger instance;
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.DestroyImmediate(this);
        }
        openlog();
    }

    public static void openlog()
    {
        if (log.Count == 0) {
            //empty log
            autoincrement = 0;
        }
        else
        {
            //send log to db
            string js = "[";
            foreach (string s in log) {
                js += s + ",";
            }
            js = js.Substring(0, js.Length - 1);
            js += "]";

            WWWForm form = new WWWForm();
            form.AddField("request", "save_actData");
            form.AddField("email", "email");
            form.AddField("token", "token");
            form.AddField("sessionId", SessionID);
            form.AddField("data", js);

            UnityWebRequest www = UnityWebRequest.Post("http://ludomi.i3lab.me/api/", form);
            www.Send();
            //clean
            log = new List<string>();
            autoincrement = 0;
        }
        filepath = Application.persistentDataPath + "/" + Application.productName + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".log";
    }

    public static void addToLogNewLine(string source, string payload)
    {
        string s = "{ \"source\" : \"" + source + "\", \"payload\": \"" + payload + "\", \"creation\":\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\", \"autoincrement\": " + autoincrement + ", \"ref\" : -1}";
        File.AppendAllText(filepath, s + Environment.NewLine);
        log.Add(s);
        autoincrement++;
    }
    bool allowquittng = false;
    void OnApplicationQuit()
    {
        addToLogNewLine("Activity", "Application ending after " + Time.time + " seconds");
        StartCoroutine(sendCommand());
        if (!allowquittng)
        {
            Application.CancelQuit();
        }
    }

    IEnumerator sendCommand()
    {
        string js = "[";
        foreach (string s in log)
        {
            js += s + ",";
        }
        js = js.Substring(0, js.Length - 1);
        js += "]";

        WWWForm form = new WWWForm();
        form.AddField("request", "save_actData");
        form.AddField("email", "email");
        form.AddField("token", "token");
        form.AddField("sessionId", SessionID);
        form.AddField("data", js);

        UnityWebRequest www = UnityWebRequest.Post("http://ludomi.i3lab.me/api/", form);
        yield return www.Send();
        if (www.isNetworkError)
        {

        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            allowquittng = true;
            Application.Quit();
        }
    }

	public void sceneChange(int index) {
        StartCoroutine(sendCommand (index));
    }
	IEnumerator sendCommand(int index) {
        string json = "{\"currentSceneIndex\": " + index + "}";
        Debug.Log(json);
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
}