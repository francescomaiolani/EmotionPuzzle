using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWhichPersonIsManager : UIEndRoundManager {

    public Text emotionText;

    // Use this for initialization
    void Start () {
		
	}

    public void UpdateUI(MinigameManager manager)
    {
        emotionText.text = manager.GetEmotionString().ToUpper();
    }

    //Metodo utilizzato per settare la schermata di fine round, VEDI UIEndRoundManager
    protected override void SetQA(SelectionGameManager manager, bool roundResult)
    {
        throw new System.NotImplementedException(); //Commenta sta linea se ti da fastidio
    }
}
