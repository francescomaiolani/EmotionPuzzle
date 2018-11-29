using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterController : MonoBehaviour {

    /// <summary>
    /// is the audio player active?
    /// </summary>
    public bool sensorEnabled;
    /// <summary>
    /// which audio tracks can be played?
    /// </summary>
    public string[] tracks;
    /// <summary>
    /// which track is playing
    /// </summary>
    public string playingTrack;
    /// <summary>
    /// the state of the playing track
    /// </summary>
    public SoundAndVideoState state;
    /// <summary>
    /// the volume reguator
    /// </summary>
    public int volume;
    /// <summary>
    /// the audio is in repeat mode?
    /// </summary>
    public bool repeat;
    /// <summary>
    /// configure the audio player
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names)
    {
        tracks = names;
    }

    internal void updateState(SoundEmitterSerializedState soundEmitterstate)
    {
        sensorEnabled = soundEmitterstate.isEnabled;
        playingTrack = soundEmitterstate.trackName;
        if (soundEmitterstate.state == "Play") {
            state = SoundAndVideoState.Play;
        }
        if (soundEmitterstate.state == "Stop")
        {
            state = SoundAndVideoState.Stop;
        }
        if (soundEmitterstate.state == "Pause")
        {
            state = SoundAndVideoState.Pause;
        }
        volume = soundEmitterstate.volume;
        repeat = soundEmitterstate.repeat;
    }
}
public enum SoundAndVideoState {
    Play, Stop, Pause
}

