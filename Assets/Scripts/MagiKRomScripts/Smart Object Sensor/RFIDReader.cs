using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFIDReader : MonoBehaviour {

    /// <summary>
    /// is the sensor active?
    /// </summary>
    public bool sensorEnabled;
    /// <summary>
    /// the list of cards that can been read
    /// </summary>
    public RFIDcard[] cardReader;
    /// <summary>
    /// the list of the names assocated to the different readers placed on the object
    /// </summary>
    public string[] readerNames;

    public string lastread;

    /// <summary>
    /// configure the sensor
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names, string[] sensornames)
    {
        cardReader = new RFIDcard[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            cardReader[i] = new RFIDcard();
            cardReader[i].cardname = names[i];
            cardReader[i].duration = 0;
            cardReader[i].read = false;
        }
        readerNames = sensornames;
    }

    internal void updateState(RFIDSerializedState rFIDstate)
    {
        lastread = rFIDstate.code;
        sensorEnabled = rFIDstate.isEnabled;
        foreach (RFIDcard t in cardReader)
        {
            t.read = false;
            if (t.cardname == rFIDstate.code)
            {
                t.read = true;
                t.duration = rFIDstate.duration;
            }
        }
    }

    internal void updateFromUDP(string id, string val, int duration)
    {
        lastread = val;
        foreach (RFIDcard p in cardReader)
        {
            if (p.cardname == val)
            {
                Debug.Log("turn true");
                p.readername = id;
                p.read = true;
                p.duration = duration;
            }
            else {
                p.read = false;
                Debug.Log("turn false");
            }
        }
    }
}
[Serializable]
public class RFIDcard
{
    public string readername;
    public string cardname;
    public bool read;
    public int duration;
}
