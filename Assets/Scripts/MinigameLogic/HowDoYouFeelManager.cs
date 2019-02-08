using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HowDoYouFeelManager : SelectionGameManager
{
    [Header("Inserisci la posizione dove viene inserita la faccia da indovinare")]
    public Transform faceCentralPosition;
    GameObject centralFace;

    protected override GameObject InstantiateEmotionElement(string emotionString, Vector3 position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/RiquadriEmozione/RiquadroEmozione"), position, Quaternion.identity, GameObject.Find("TextContainer").transform) as GameObject;
        obj.GetComponent<TextMeshProUGUI>().text = ConvertInCorrectText(emotionString);
        obj.transform.localScale = new Vector3(1, 1, 1);
        return obj;
    }

    //Metodo che si occupa di far spawnare la faccia da indovinare al centro della scena
    protected override void SetupCentralEmotion()
    {
        centralFace = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFace"), faceCentralPosition.position, Quaternion.identity);
        centralFace.GetComponent<Avatar>().CreateCompleteFace(GetMainEmotion(), gameSessionSettings.avatarSettings.gender,
            gameSessionSettings.avatarSettings.skinColor, gameSessionSettings.avatarSettings.hairStyle, gameSessionSettings.avatarSettings.hairColor,
            gameSessionSettings.avatarSettings.eyesColor);
        centralFace.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

    }

    protected override void DestroySceneObjects()
    {
        base.DestroySceneObjects();
        Destroy(centralFace.gameObject);

    }
}
