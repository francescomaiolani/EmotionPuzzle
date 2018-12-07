using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuessExpressionManager : UIEndRoundManager {

    protected override void SetQA(MinigameManager manager, bool roundResult)
    {
        sentencesQA[0].text = manager.GetEmotionString().ToUpper();
        if (roundResult)
        {
            imagesQA[0].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + manager.GetEmotionAnswerString());
            imagesQA[0].SetNativeSize();
            imagesQA[0].gameObject.SetActive(true);
        } else
        {
            imagesQA[1].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + manager.GetEmotionAnswerString());
            imagesQA[1].gameObject.SetActive(true);
            imagesQA[2].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + manager.GetEmotionString());
            imagesQA[2].gameObject.SetActive(true);
        }
            
    }


}
