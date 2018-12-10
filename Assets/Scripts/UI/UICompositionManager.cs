using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICompositionManager : UIEndRoundManager {

    public Image advicePanel;
    public Text adviceText;

    public TextMeshProUGUI emotionText;

    private void Start()
    {
        Hand.adviceGiven += GiveAdvice;
    }

    void GiveAdvice(string advice) {
        adviceText.text = advice;
        advicePanel.gameObject.SetActive(true);
        advicePanel.GetComponent<Animator>().Play("Advice");
    }

    void DisableAdvicePanel() {
        advicePanel.gameObject.SetActive(false);
    }

    public void UpdateUI(MinigameManager manager) {
        emotionText.text = manager.ConvertInCorrectText(manager.GetEmotionString());
        //fumettoText.text = fumettoPhrase.Values
    }

    protected override void SetQA(bool roundResult)
    {
        sentencesQA[0].text = gameManager.ConvertInCorrectText(gameManager.GetEmotionString());

        if (roundResult)
        {
            SpawnFace(new Vector3(0, -2, 0), gameManager.GetComponent<CompositionManager>().GetEyesEmotionChosen(), gameManager.GetComponent<CompositionManager>().GetMouthEmotionChosen(),true, 1.4f, true) ;
        }
        else
        {
            SpawnFace(new Vector3(-3f, -2,0), gameManager.GetMainEmotion(), gameManager.GetMainEmotion(),true,  1.4f, true);
            SpawnFace(new Vector3(3f, -2, 0), gameManager.GetComponent<CompositionManager>().GetEyesEmotionChosen(), gameManager.GetComponent<CompositionManager>().GetMouthEmotionChosen(),false, 1.4f, true);
        }

    }

   
}

