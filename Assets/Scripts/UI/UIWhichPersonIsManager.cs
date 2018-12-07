using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWhichPersonIsManager : UIEndRoundManager {

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

    //Metodo utilizzato per settare la schermata di fine round, VEDI UIEndRoundManager
    protected override void SetQA(bool roundResult)
    {
        throw new System.NotImplementedException(); //Commenta sta linea se ti da fastidio
    }
}
