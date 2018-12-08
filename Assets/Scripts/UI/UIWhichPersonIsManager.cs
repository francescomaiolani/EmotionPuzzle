using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWhichPersonIsManager : UIEndRoundManager {

    public TextMeshProUGUI emotionText;

    public void UpdateUI(MinigameManager manager)
    {
        emotionText.text = "<color=#FFC132>Q</color><color=#FF9138>u</color>a<color=#F16B68>l</color><color=#FFC132>i " +
            "</color>p<color=#F16B68>e</color><color=#FF9138>rs</color>o<color=#FFC132>n</color><color=#F16B68>e </color>" +
            "<color=#FF9138>s</color>o<color=#FFC132>n</color>o " + CovertEmotionToAdjective(manager.GetEmotionString()) + "?";
        
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
    }
}
