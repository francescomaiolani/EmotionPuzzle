using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPhotographicEmotionManager : UIEndRoundManager {

    public Image photoImage;

    protected override void SetQA(bool roundResult)
    {
        //photoImage.sprite = Resources.Load<Sprite>("Sprite/FotoEmozioni/blablablabla" + gameManager.emotionAnswer); Questa riga setta la foto nella schermata di fine round
        if (roundResult)
        {
            imagesQA[0].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + gameManager.GetEmotionAnswerString());
            imagesQA[0].gameObject.SetActive(true);
        }
        else
        {
            imagesQA[1].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + gameManager.GetEmotionAnswerString());
            imagesQA[1].gameObject.SetActive(true);
            imagesQA[2].sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/face" + gameManager.GetEmotionString());
            imagesQA[2].gameObject.SetActive(true);
        }
    }

}
