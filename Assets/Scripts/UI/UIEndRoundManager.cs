using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIEndRoundManager : MonoBehaviour {

    public Image[] imagesQA;
    public Text[] sentencesQA;
    public Text resultSentence;
    public MinigameManager gameManager;

    protected abstract void SetQA(bool roundResult);

    public void EndRoundUI(bool roundResult)
    {
        if (roundResult)
            resultSentence.text = "COMPLIMENTI!\nRISPOSTA ESATTA!";
        else
            resultSentence.text = "NOOO!\nRISPOSTA ERRATA!";
        SetQA(roundResult);
    }

}
