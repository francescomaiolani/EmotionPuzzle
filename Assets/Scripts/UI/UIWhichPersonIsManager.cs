using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWhichPersonIsManager : MonoBehaviour {

    public TextMeshProUGUI emotionText;

    // Use this for initialization
    void Start () {
		
	}

    public void UpdateUI(MinigameManager manager)
    {
        emotionText.text = "Quali persone sono " + CovertEmotionToAdjective(manager.GetEmotionString()) + "?";
        
    }

    string CovertEmotionToAdjective(string emotion) {
        switch (emotion)
        {
            case "Felicità":
                return "felici";
            case "Tristezza":
                return "tristi";
            case "Disgusto":
                return "disgutate";
            case "Rabbia":
                return "arrabbiate";
            case "Paura":
                return "impaurite";
            default:
                return null;
        }
    }
}
