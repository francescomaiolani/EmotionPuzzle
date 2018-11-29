using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWhichPersonIsManager : MonoBehaviour {

    public Text emotionText;

    // Use this for initialization
    void Start () {
		
	}

    public void UpdateUI(MinigameManager manager)
    {
        emotionText.text = manager.GetEmotionString().ToUpper();
    }
}
