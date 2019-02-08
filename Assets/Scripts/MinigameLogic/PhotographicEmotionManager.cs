using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotographicEmotionManager : SelectionGameManager
{
    [Header("Inserisci la posizione dove viene inserita la foto da indovinare")]
    public SpriteRenderer photoSprite;

    protected override GameObject InstantiateEmotionElement(string emotionString, Vector3 position)
    {
        AvatarSettings ava = gameSessionSettings.avatarSettings;
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFace"), position, Quaternion.identity);
        obj.GetComponent<Avatar>().CreateCompleteFace(ConvertTextInEmotion(emotionString), ava.gender, ava.skinColor, ava.hairStyle, ava.hairColor, ava.eyesColor);
        obj.GetComponent<Avatar>().MakeAvatarSelectableObject();
        return obj;
    }

    protected override void SetupCentralEmotion()
    {
        photoSprite.sprite = Resources.Load<Sprite>("Sprite/Photos/" + mainEmotion + "/" + Random.Range(1, 5));
        photoSprite.size = new Vector2(5f, 5f);
    }
}
