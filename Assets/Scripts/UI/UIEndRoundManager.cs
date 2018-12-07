using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UIEndRoundManager : MonoBehaviour {

    public Image endRoundPanel;
    public Image[] imagesQA;
    public Text[] sentencesQA;
    public TextMeshProUGUI resultSentence;
    public Text resultSentence;
    public MinigameManager gameManager;

    protected abstract void SetQA(bool roundResult);

    public void EndRoundUI(bool roundResult)
    {
        if (roundResult)
        {
            resultSentence.text = "<color=#FFC132>H</color><color=#FF9138>a</color>i<color=#F16B68> v</color>" +
                "<color=#FFC132>i</color>nt<color=#FF9138>o</color>!<color=#F16B68>!</color>";
            endRoundPanel.color = new Color32(200,246,88,255);
        }
        else {
            resultSentence.text = "<color=#FFC132>H</color><color=#FF9138>a</color>i<color=#F16B68> p</color>" +
               "<color=#FFC132>e</color>rs<color=#FF9138>o</color>!<color=#F16B68>!</color>";
            endRoundPanel.color = new Color32(241, 107, 104, 255);
        }
           
        SetQA(manager, roundResult);
    }

}
