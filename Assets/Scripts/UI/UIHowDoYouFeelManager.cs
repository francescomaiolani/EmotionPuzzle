using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHowDoYouFeelManager : UIEndRoundManager
{
    protected override void SetQA(bool roundResult)
    {
        SpawnFace(new Vector3(0, -0.5f, 0), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), gameManager.GetComponent<MinigameManager>().GetMainEmotion(), true, 1.4f, false);
        if (roundResult)
        {
            SpawnTextUI(new Vector2(0, -180), gameManager.GetEmotionAnswer(), true);
        }
        else
        {
            SpawnTextUI(new Vector2(-150, -180), gameManager.GetMainEmotion(), true);
            SpawnTextUI(new Vector2(300, -180), gameManager.GetEmotionAnswer(), false);
        }
    }
}
