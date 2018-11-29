using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class HttpListenerForMagiKRoom : MonoBehaviour
{
    /// <summary>
    /// Singleton of the system
    /// </summary>
    public static HttpListenerForMagiKRoom instance;
    /// <summary>
    /// address of the listener
    /// </summary>
    public string address;
    /// <summary>
    /// port of the listener
    /// </summary>
    public int port;

    public delegate void RequestHandler(Match match, HttpListenerResponse response, string content);

    private Dictionary<Regex, RequestHandler> _requestHandlers = new Dictionary<Regex, RequestHandler>();

    HttpListener _listener;

    void Awake()
    {
        instance = this;
        _requestHandlers[new Regex(@"^/kinectposition$")] = HandleKinectPosition;
        _requestHandlers[new Regex(@"^/smarttoyevent$")] = HandleSmartToyEventPosition;
        _requestHandlers[new Regex(@"^/kinectaudio$")] = HandleKinectAudio;
        _requestHandlers[new Regex(@"^/speachtotext$")] = HandleSpeachToText;
        _requestHandlers[new Regex(@"^/ExperienceManager$")] = HandleExperienceManager;
        _requestHandlers[new Regex(@"^/speachtotextOffline$")] = HandleSpeachToTextOffline;
        _requestHandlers[new Regex(@"^.*$")] = HandleDefault;

        
    }

    void Start()
    {

        _listener = new HttpListener();
        _listener.Prefixes.Add(address + ":" + port.ToString() + "/");

        _listener.Start();

        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    void Destroy()
    {
        if (_listener != null)
        {
            _listener.Close();
        }
    }
    /// <summary>
    /// function called whenever thelistener get something
    /// </summary>
    /// <param name="result"></param>
    private void ListenerCallback(IAsyncResult result)
    {
        HttpListener listener = (HttpListener)result.AsyncState;
        // Call EndGetContext to complete the asynchronous operation.
        HttpListenerContext context = listener.EndGetContext(result);
        HttpListenerRequest request = context.Request;
        string contentread = new StreamReader(request.InputStream).ReadToEnd();
        // Obtain a response object.
        HttpListenerResponse response = context.Response;

        foreach (Regex r in _requestHandlers.Keys)
        {
            Match m = r.Match(request.Url.AbsolutePath);
            if (m.Success)
            {
                (_requestHandlers[r])(m, response, contentread);
                _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
                return;
            }
        }

        response.StatusCode = 404;
        response.Close();
    }

    void Update()
    {
        if (httpcontent != null && httpcontent != "")
        {
            MagicRoomSmartToyManager.instance.updateFromTCPEvent(httpcontent);
            httpcontent = "";
        }
        if (p != "")
        {
            string text = poolofsentencies[UnityEngine.Random.Range(0, poolofsentencies.Length - 1)];
            text = text.Replace("#playername", p);
            if (MagicRoomTextToSpeachManagerOffline.instance.MagicRoomSpeachToText_active)
            {
                MagicRoomTextToSpeachManagerOffline.instance.generateAudioFromText(text, MagicRoomTextToSpeachManagerOffline.instance.listofAssociatedNames[0]);
            }
            p = "";
        }
    }
    /// <summary>
    /// handler of the events when the message is directed to the kinect position 
    /// </summary>
    /// <param name="match"></param>
    /// <param name="response"></param>
    /// <param name="content"></param>
    private static void HandleKinectPosition(Match match, HttpListenerResponse response, string content)
    {
        MagicRoomKinectV2Manager.instance.setSkeletons(content);

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Success");
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
    }
    private static  string httpcontent;
    /// <summary>
    /// handler of the events when the message is directed to the smart toy event  
    /// </summary>
    /// <param name="match"></param>
    /// <param name="response"></param>
    /// <param name="content"></param>
    private static void HandleSmartToyEventPosition(Match match, HttpListenerResponse response, string content)
    {
        Debug.Log(content);
        httpcontent = content;
        //MagicRoomSmartToyManager.instance.updateFromEvent(content);

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Success");
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
    }
    /// <summary>
    /// handler of the events when the message is directed to the kinect audiocommand 
    /// </summary>
    /// <param name="match"></param>
    /// <param name="response"></param>
    /// <param name="content"></param>
    private static void HandleKinectAudio(Match match, HttpListenerResponse response, string content)
    {
        //MagicRoomKinectV2Manager.instance.setSkeletons(content);

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Success");
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
    }

    /// <summary>
    /// handler of the events when the message is directed to the audiospeacker 
    /// </summary>
    /// <param name="match"></param>
    /// <param name="response"></param>
    /// <param name="content"></param>
    private static void HandleSpeachToText(Match match, HttpListenerResponse response, string content)
    {
        Debug.Log(content);
        MagicRoomTextToSpeechManagerOnline.instance.ReadyFile(content);
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Success");
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
    }

    /// <summary>
    /// handler of the events when the message is directed to the audiospeacker 
    /// </summary>
    /// <param name="match"></param>
    /// <param name="response"></param>
    /// <param name="content"></param>
    private static void HandleSpeachToTextOffline(Match match, HttpListenerResponse response, string content)
    {
        Debug.Log(content);
        MagicRoomTextToSpeachManagerOffline.instance.isCompleted();
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Success");
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
    }

    public static bool receivedconfig = false;
    public static bool receivedforcedCommand = false;
    static string[] poolofsentencies = new string[]{
        "#playername è il tuo turno!",
        "#playername adesso tocca a te!",
        "Forza #playername, giochiamo!",
        "#playername facci vedere quanto sei bravo!",
        "Vediamo, adesso gioca #playername"
    };
    public static bool pause = false;
    public static bool skip = false;
    public static bool next = false;
    public static bool back = false;
    public static bool repeat = false;
    public static bool close = false;
    static string p = "";
    private static void HandleExperienceManager(Match match, HttpListenerResponse response, string content)
    {
        string messageback = "";
        Debug.Log(content);
        try
        {
            if (content.Contains("type"))
            {
                MessageFromExpManager m = JsonUtility.FromJson<MessageFromExpManager>(content);
                
                if (m.action == "commands")
                {
                    commandmessages msg = (commandmessages)Enum.Parse(typeof(commandmessages), m.payload);
                    Debug.Log(msg);
                    //force command
                    switch (msg)
                    {
                        case commandmessages.pause:
                            pause = true;
                            break;
                        case commandmessages.play:
                            pause = false;
                            break;
                        case commandmessages.next:
                            next = true;
                            break;
                        case commandmessages.back:
                            back = true;
                            break;
                        case commandmessages.skip:
                            skip = true;
                            break;
                        case commandmessages.repeat:
                            repeat = true;
                            break;
                        case commandmessages.close:
                            close = true;
                            break;
                    }
                    
                }

                if (m.action == "turn")
                {
                    PlayerCommand gg = JsonUtility.FromJson<PlayerCommand>(content);
                    p = gg.phonema;
                }
                messageback = "Success";
            }
            else
            {



               //DECODE THE GAME CONFIGURATION HERE



                receivedconfig = true;
            }
        }
        catch (Exception e)
        {
            messageback = e.Message;
        }

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(messageback);
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
    }

    /// <summary>
    /// handler of the events when the message is directed to unknown addresses
    /// </summary>
    /// <param name="match"></param>
    /// <param name="response"></param>
    /// <param name="content"></param>
    private static void HandleDefault(Match match, HttpListenerResponse response, string content)
    {
        response.StatusCode = 404;
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Not accetabe response");
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        System.IO.Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
    }

    public void signalReadyToExperienceManager()
    {
        ExperienceManagerComunication.instance.SendReadyCommand();
    }

    public void signalConclusionToExperienceManager()
    {
        ExperienceManagerComunication.instance.SendConcludedCommand();
    }
    
    public void signalStartToExperienceManager() {
        ExperienceManagerComunication.instance.SendStartedCommand();
    }
}

[Serializable]
public class MessageFromExpManager
{
    public string type;
    public string action;
    public string payload;
}

[Serializable]
public class PlayerCommand
{
    public string writtenname;
    public string phonema;
}

public enum commandmessages
{
    pause, play, next, back, skip, repeat, close
}