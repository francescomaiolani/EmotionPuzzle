using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuessExpressionManager : UISelectionManager {

    protected override void SetQA(SelectionGameManager manager, bool roundResult)
    {
        sentecesQA[0].text = manager.GetEmotionString().ToUpper();
        if (roundResult)
        {
            imagesQA[0].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + manager.emotionAnswer);
            imagesQA[0].gameObject.SetActive(true);
        } else
        {
            imagesQA[1].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + manager.emotionAnswer);
            imagesQA[1].gameObject.SetActive(true);
            imagesQA[2].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + manager.GetEmotionString());
            imagesQA[2].gameObject.SetActive(true);
        }
            
    }


}
