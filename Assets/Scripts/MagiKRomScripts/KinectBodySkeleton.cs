using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

[Serializable]
public class FrameFromKinectServer {
    public KinectBodySkeleton[] skeletons;
}

[Serializable]
public class AudioEventFromKinectServer
{
    public string type;
    public string world;
}

public class EventFromKinectServer
{
    public string detectedWorld;
}

[Serializable]
    public class KinectBodySkeleton
    {
        public Vector3 SpineBase;
        public Vector3 SpineMid;
        public Vector3 Neck;
        public Vector3 Head;
        public Vector3 ShoulderLeft;
        public Vector3 ElbowLeft;
        public Vector3 WristLeft;
        public Vector3 HandLeft;
        public Vector3 ShoulderRight;
        public Vector3 ElbowRight;
        public Vector3 WristRight;
        public Vector3 HandRight;
        public Vector3 HipLeft;
        public Vector3 KneeLeft;
        public Vector3 AnkleLeft;
        public Vector3 FootLeft;
        public Vector3 HipRight;
        public Vector3 KneeRight;
        public Vector3 AnkleRight;
        public Vector3 FootRight;
        public Vector3 SpineShoulder;
        public Vector3 HandTipLeft;
        public Vector3 ThumbLeft;
        public Vector3 HandTipRight;
        public Vector3 ThumbRight;
        
        public string GestureMask;


    override public string ToString() {
        return "SpineBase" + SpineBase.ToString() + "SpineMid" + SpineMid.ToString() + "Neck" + Neck.ToString() + "Head" + Head.ToString() + "ShoulderLeft" + ShoulderLeft.ToString() +
        "ElbowLeft" + ElbowLeft.ToString() + "WristLeft" + WristLeft.ToString() + "HandLeft" + HandLeft.ToString() + "ShoulderRight" + ShoulderRight.ToString() + "ElbowRight" + ElbowRight.ToString() +
        "WristRight" + WristRight.ToString() + "HandRight" + HandRight.ToString() + "HipLeft" + HipLeft.ToString() +  "HipRight" + HipRight.ToString() + "KneeLeft" + KneeLeft.ToString() + "KneeRight" +
        KneeRight.ToString() + "AnkleLeft" + AnkleLeft.ToString() + "AnkleRight" + AnkleRight.ToString() +  "FootLeft" + FootLeft.ToString() + "FootRight" + FootRight.ToString() + 
        "SpineShoulder" + SpineShoulder.ToString() + "HandTipLeft" + HandTipLeft.ToString() + "HandTipRight" + HandTipRight.ToString() + "ThumbLeft" + ThumbLeft.ToString() + 
        "ThumbRight" + ThumbRight.ToString();
    }

    public Vector3 getBodyPosition() {
        return SpineBase;
    }
    public bool isRightHandClosed(float threshold)
    {
        Debug.Log(Vector3.Distance(HandRight, HandTipRight));
        return Vector3.Distance(HandRight, HandTipRight) <= threshold;
    }
    public bool isLeftHandClosed(float threshold)
    {
        return Vector3.Distance(HandLeft, HandTipLeft) <= threshold;
    }
}
