using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UISelectionManager : MonoBehaviour {

    public Image[] imagesQA;
    public Text[] sentecesQA;
    public Text resultSentence;

    protected abstract void SetQA(SelectionGameManager manager, bool roundResult);

    public void EndRoundUI(SelectionGameManager manager, bool roundResult)
    {
        if (roundResult)
            resultSentence.text = "COMPLIMENTI!\nRISPOSTA ESATTA!";
        else
            resultSentence.text = "NOOO!\nRISPOSTA ERRATA!";
        SetQA(manager, roundResult);
        
    }

}
