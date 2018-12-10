using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotographicEmotionManager : SelectionGameManager
{
    [Header("Inserisci la posizione dove viene inserita la foto da indovinare")]
    public SpriteRenderer photoSprite;

    protected override GameObject InstantiateEmotionElement(string emotionString, Vector3 position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/Faces/face" + emotionString), position, Quaternion.identity);
        return obj;
    }

    protected override void SetupCentralEmotion()
    {
        photoSprite.sprite = Resources.Load<Sprite>("Sprite/Photos/" + mainEmotion + "/" + Random.Range(1,5) );
        photoSprite.size = new Vector2(5f, 5f);
    }
}
