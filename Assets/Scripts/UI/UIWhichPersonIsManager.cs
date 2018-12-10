using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWhichPersonIsManager : UIEndRoundManager {

    public TextMeshProUGUI emotionText;

    public void UpdateUI(MinigameManager manager)
    {
        emotionText.text = ChangeTextToRandomColors("Quali persone sono " + CovertEmotionToAdjective(manager.GetEmotionString()) + "?"); 
        
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
        sentencesQA[0].text = gameManager.ConvertInCorrectText(gameManager.GetEmotionString());
        if (roundResult)
        {
            SpawnFace(new Vector3(0, -2, 0), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), true, 1.4f, true);
        }
        else
        {
            SpawnFace(new Vector3(-3f, -2, 0), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), true, 1.4f, true);
            SpawnFace(new Vector3(3f, -2, 0), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), false, 1.4f, true);
        }
    }
}
