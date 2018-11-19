using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {

    [SerializeField]
    private Emotion emotionType;

    public void SetEmotionType(Emotion emotion)
    {
        emotionType = emotion;
    }
}
