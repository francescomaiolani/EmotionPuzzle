using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {
    /// <summary>
    /// is the effect manager acive?
    /// </summary>
    public bool isEffectActive;
    /// <summary>
    /// list of all effects that are avalaable
    /// </summary>
    public string[] effects;
    /// <summary>
    /// which effect is active
    /// </summary>
    public string activeEffect;
    /// <summary>
    /// is the effect playing
    /// </summary>
    public bool isEffectPlaying;
    /// <summary>
    /// setup the configuration
    /// </summary>
    /// <param name="names"></param>
    public void setUpConfiguration(string[] names)
    {
        effects = names;
    }

    internal void updateState(EffectSerializedState effectstate)
    {
        isEffectActive = effectstate.isEnabled;
        activeEffect = effectstate.effectName;
        isEffectPlaying = effectstate.isPlaying;
    }
}
