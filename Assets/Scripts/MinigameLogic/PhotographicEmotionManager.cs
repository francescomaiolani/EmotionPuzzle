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
        //throw new System.NotImplementedException();
    }
}
