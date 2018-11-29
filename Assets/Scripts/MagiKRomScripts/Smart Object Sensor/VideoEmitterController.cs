using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoEmitterController : MonoBehaviour {
    /// <summary>
    /// is the vidoeplayer actve?
    /// </summary>
    public bool sensorEnabled;
    /// <summary>
    /// which tracks can be played?
    /// </summary>
    public string[] tracks;
    /// <summary>
    /// the state of the current playing track
    /// </summary>
    public SoundAndVideoState state;
    /// <summary>
    /// the volume of the player
    /// </summary>
    public int volume;
    /// <summary>
    /// the track curreltly being played
    /// </summary>
    public string playingTrack;
    /// <summary>
    /// is the track in repeat mode?
    /// </summary>
    public bool repeat;

    /// <summary>
    /// set up the actuator
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names)
    {
        tracks = names;
    }

    internal void updateState(VideoEmitterSerializedState videoEmitterstate)
    {
        sensorEnabled = videoEmitterstate.isEnabled;
        playingTrack = videoEmitterstate.videoName;
        if (videoEmitterstate.state == "Play")
        {
            state = SoundAndVideoState.Play;
        }
        if (videoEmitterstate.state == "Stop")
        {
            state = SoundAndVideoState.Stop;
        }
        if (videoEmitterstate.state == "Pause")
        {
            state = SoundAndVideoState.Pause;
        }
        volume = videoEmitterstate.volume;
        repeat = videoEmitterstate.repeat;
    }
}
