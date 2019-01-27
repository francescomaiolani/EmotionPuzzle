using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessExpressionManager : SelectionGameManager
{

    [Header ("Riquadro dell'interfaccia dove viene salvata l'emotion principale")]
    public Text emotionType;
    private AvatarSettings avatarSettings;

    private void Awake ()
    {
        if (GameObject.FindObjectOfType<GameSessionSettings> () != null)
            avatarSettings = GameObject.FindObjectOfType<GameSessionSettings> ().GetComponent<GameSessionSettings> ().avatarSettings;
    }

    protected override void SetupCentralEmotion ()
    {
        emotionType.text = ConvertInCorrectText (GetEmotionString ());
    }

    protected override GameObject InstantiateEmotionElement (string emotionString, Vector3 position)
    {
        if (GameObject.FindObjectOfType<GameSessionSettings> () != null)
            avatarSettings = GameObject.FindObjectOfType<GameSessionSettings> ().GetComponent<GameSessionSettings> ().avatarSettings;

        GameObject face = Instantiate (Resources.Load<GameObject> ("Prefab/AvatarFace"), position, Quaternion.identity);
        //face.GetComponent<Avatar> ().CreateCompleteFace (MinigameManager.ConvertTextInEmotion (emotionString), avatarSettings.gender, avatarSettings.skinColor, avatarSettings.hairStyle, avatarSettings.hairColor, avatarSettings.eyesColor);
        face.GetComponent<Avatar> ().CreateCompleteFace (MinigameManager.ConvertTextInEmotion (emotionString), avatarSettings.gender,
            avatarSettings.skinColor, avatarSettings.hairStyle, avatarSettings.hairColor, avatarSettings.eyesColor);
        face.transform.localScale = new Vector3 (1, 1, 1);
        face.AddComponent<CircleCollider2D> ().radius = 0.5f;
        face.GetComponent<CircleCollider2D> ().isTrigger = true;
        face.AddComponent<SelectableObject> ().SetEmotionType (MinigameManager.ConvertTextInEmotion (emotionString));
        return face;

    }
}
