using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHowDoYouFeelManager : UIEndRoundManager
{
    protected override void SetQA(bool roundResult)
    {
        imagesQA[0].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + gameManager.GetEmotionString());
        if (roundResult)
        {
            sentencesQA[0].text = gameManager.GetEmotionAnswerString();
            sentencesQA[0].gameObject.SetActive(true);
        }
        else
        {
            sentencesQA[1].text = gameManager.GetEmotionAnswerString();
            sentencesQA[1].gameObject.SetActive(true);
            sentencesQA[2].text = gameManager.GetEmotionString();
            sentencesQA[2].gameObject.SetActive(true);
        }
    }
}
