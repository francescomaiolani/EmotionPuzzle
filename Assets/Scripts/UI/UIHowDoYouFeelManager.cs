using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHowDoYouFeelManager : UIEndRoundManager
{
    protected override void SetQA(bool roundResult)
    {
        SpawnFace(new Vector3(0, 0, 0), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), true, 1.2f);
        if (roundResult)
        {
            sentencesQA[0].text = gameManager.ConvertInCorrectText(gameManager.GetEmotionString());
        }
        else
        {
            sentencesQA[1].text = gameManager.ConvertInCorrectText(gameManager.GetEmotionString());
            sentencesQA[2].text = gameManager.ConvertInCorrectText(gameManager.GetEmotionAnswerString());
        }
    }
}
