using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHowDoYouFeelManager : UIEndRoundManager
{
    protected override void SetQA(SelectionGameManager manager, bool roundResult)
    {
        imagesQA[0].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + manager.GetEmotionString());
        if (roundResult)
        {
            sentencesQA[0].text = manager.emotionAnswer.ToString();
            sentencesQA[0].gameObject.SetActive(true);
        }
        else
        {
            sentencesQA[1].text = manager.emotionAnswer.ToString();
            sentencesQA[1].gameObject.SetActive(true);
            sentencesQA[2].text = manager.GetEmotionString();
            sentencesQA[2].gameObject.SetActive(true);
        }
    }
}
