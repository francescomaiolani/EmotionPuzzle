using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPListenerForMagiKRoom : MonoBehaviour
{

    public static UDPListenerForMagiKRoom instance;

    public string address;
    public int port;

    public bool _stop;

    // udpclient object
    static UdpClient client;


    // the data
    public string lastReceivedUDPPacket = "";
    private static bool messageReceived;

    void Awake()
    {
        instance = this;
        _stop = true;
    }
    IPEndPoint endpoint;
    UdpStates udpstate;
    public void StartReceiver(int freq)
    {
        _stop = false;
        client = new UdpClient(port);
        messageReceived = false;
        UdpStates udpstate = new UdpStates();
        udpstate.e = new IPEndPoint(IPAddress.Parse(address), port);
        udpstate.u = client;
        endpoint = new IPEndPoint(IPAddress.Parse(address), port);
        client.BeginReceive(new AsyncCallback(ReceiveCallback), udpstate);
        //StartCoroutine(listeningUDP(freq));
    }

    private void Update()
    {

        if (!_stop)
        {
            if (messageReceived)
            {
                string temp = lastReceivedUDPPacket;
                if (temp != null && temp != "")
                {
                    try
                    {
                        MagicRoomSmartToyManager.instance.updateFromUDPEvent(temp);
                        //lastReceivedUDPPacket = null;
                        messageReceived = false;
                    }
                    catch
                    {
                        print("message unreadable");
                    }
                    finally
                    {
                        /*UdpStates udpstate = new UdpStates();
                        udpstate.e = new IPEndPoint(IPAddress.Parse(address), port); ;
                        udpstate.u = client;
                        client.BeginReceive(new AsyncCallback(ReceiveCallback), udpstate);*/
                    }
                }
            }
        }
    }

    IEnumerator listeningUDP(int freq)
    {
        while (!_stop)
        {
            yield return new WaitForSeconds(1 / freq);
            byte[] data = client.Receive(ref endpoint);
            lastReceivedUDPPacket = Encoding.ASCII.GetString(data);
            messageReceived = true;
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {

        UdpClient u = client;
        IPEndPoint e = endpoint;
        Byte[] receiveBytes = u.EndReceive(ar, ref e);
        if (receiveBytes.Length > 0)
        {
            string receiveString = Encoding.ASCII.GetString(receiveBytes);

            Debug.Log("Received: " + receiveString);
            lastReceivedUDPPacket = receiveString;
            messageReceived = true;
        }
        UdpStates udpstate = new UdpStates();
        udpstate.e = endpoint;
        udpstate.u = client;
        client.BeginReceive(new AsyncCallback(ReceiveCallback), udpstate);
    }


    public void StopReceiver()
    {
        _stop = true;
        if (client != null)
        {
            client.Close();
        }
    }

    private void OnDestroy()
    {
        StopReceiver();
    }
}

[Serializable]
public class tcpPackage
{
    public string id;
    public UDPEvent[] events;
}

[Serializable]
public class udpPackage
{
    public string id;
    public triplet[] gyroscope;
    public triplet[] accelerometer;
    public float[] position;
    public UDPEvent[] state;
}
[Serializable]
public class UDPEvent
{
    public string typ;
    public string id;
    public string val;
    public int dur;
}

class UdpStates
{
    public IPEndPoint e;
    public UdpClient u;
}

[Serializable]
public class triplet
{
    public string sensorId;
    public float x;
    public float y;
    public float z;
}