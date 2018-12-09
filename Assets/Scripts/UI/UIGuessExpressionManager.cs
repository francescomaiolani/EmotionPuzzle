using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuessExpressionManager : UIEndRoundManager {

    protected override void SetQA(bool roundResult)
    {
        sentencesQA[0].text = gameManager.ConvertInCorrectText(gameManager.GetEmotionString());
        if (roundResult)
        {
            SpawnFace(new Vector3(0, -2, 0), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), true, 1.4f);
        } else
        {
            SpawnFace(new Vector3(-3f, -2, 0), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), true, 1.4f);
            SpawnFace(new Vector3(3f, -2, 0), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), false, 1.4f);
        }

    }


}
