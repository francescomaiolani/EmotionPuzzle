using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGuessExpressionManager : UIEndRoundManager
{

    protected override void SetQA(bool roundResult)
    {
        sentencesQA[0].text = MinigameManager.ConvertInCorrectText(gameManager.GetEmotionString());
        if (roundResult)
        {
            SpawnFace(new Vector3(0, -2, 0), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), true, 1.4f, true);
        }
        else
        {
            SpawnFace(new Vector3(0f, -2, 0), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), gameManager.GetComponent<MinigameManager>().GetEmotionAnswer(), false, 1.4f, true);
        }

    }

}
