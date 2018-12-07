using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIEndRoundManager : MonoBehaviour {

    public Image[] imagesQA;
    public Text[] sentencesQA;
    public Text resultSentence;

    protected abstract void SetQA(MinigameManager manager, bool roundResult);

    public void EndRoundUI(MinigameManager manager, bool roundResult)
    {
        if (roundResult)
            resultSentence.text = "COMPLIMENTI!\nRISPOSTA ESATTA!";
        else
            resultSentence.text = "NOOO!\nRISPOSTA ERRATA!";
        SetQA(manager, roundResult);
    }

}
