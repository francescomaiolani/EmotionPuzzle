using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPhotographicEmotionManager : UIEndRoundManager {

    protected override void SetQA(bool roundResult)
    {
        //photoImage.sprite = Resources.Load<Sprite>("Sprite/FotoEmozioni/blablablabla" + gameManager.emotionAnswer); Questa riga setta la foto nella schermata di fine round
        sentencesQA[0].text = gameManager.ConvertInCorrectText(gameManager.GetEmotionString());

        if (roundResult)
        {
            SpawnFace(new Vector3(0, -2, 0), gameManager.GetComponent<PhotographicEmotionManager>().GetEmotionAnswer(), gameManager.GetComponent<PhotographicEmotionManager>().GetEmotionAnswer(), true, 1.4f, true);
        }
        else
        {
            SpawnFace(new Vector3(-3f, -2, 0), gameManager.GetMainEmotion(), gameManager.GetMainEmotion(), true, 1.4f, true);
            SpawnFace(new Vector3(3f, -2, 0), gameManager.GetComponent<PhotographicEmotionManager>().GetEmotionAnswer(), gameManager.GetComponent<PhotographicEmotionManager>().GetEmotionAnswer(), false, 1.4f, true);
        }

    }



}
