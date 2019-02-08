using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessExpressionManager : SelectionGameManager
{

    [Header("Riquadro dell'interfaccia dove viene salvata l'emotion principale")]
    public Text emotionType;

    protected override void SetupCentralEmotion()
    {
        emotionType.text = ConvertInCorrectText(GetEmotionString());
    }

    protected override GameObject InstantiateEmotionElement(string emotionString, Vector3 position)
    {
        AvatarSettings avatarSettings = gameSessionSettings.avatarSettings;
        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFace"), position, Quaternion.identity);
        //face.GetComponent<Avatar> ().CreateCompleteFace (MinigameManager.ConvertTextInEmotion (emotionString), avatarSettings.gender, avatarSettings.skinColor, avatarSettings.hairStyle, avatarSettings.hairColor, avatarSettings.eyesColor);
        face.GetComponent<Avatar>().CreateCompleteFace(MinigameManager.ConvertTextInEmotion(emotionString), avatarSettings.gender,
            avatarSettings.skinColor, avatarSettings.hairStyle, avatarSettings.hairColor, avatarSettings.eyesColor);
        face.GetComponent<Avatar>().MakeAvatarSelectableObject();
        return face;

    }

}
