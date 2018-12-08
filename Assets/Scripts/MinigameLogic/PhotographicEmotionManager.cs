using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotographicEmotionManager : SelectionGameManager
{
    [Header("Inserisci la posizione dove viene inserita la foto da indovinare")]
    public Transform photoPosition;

    protected override GameObject InstantiateEmotionElement(string emotionString, Vector3 position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/Faces/face" + emotionString), position, Quaternion.identity);
        return obj;
    }

    protected override void SetupCentralEmotion()
    {
        GameObject photo = Instantiate(Resources.Load<GameObject>("Prefab/ImagePrefab/ImagePrefab"), photoPosition.position, Quaternion.identity);
        photo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Photos/" + mainEmotion + "/" + Random.Range(1,5) );
        photo.transform.localScale = new Vector3(5f / photo.GetComponent<SpriteRenderer>().size.x, 5f / photo.GetComponent<SpriteRenderer>().size.y, 1);
    }
}
