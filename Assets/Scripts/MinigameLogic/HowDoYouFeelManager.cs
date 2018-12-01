using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowDoYouFeelManager : SelectionGameManager
{
    [Header("Inserisci la posizione dove viene inserita la faccia da indovinare")]
    public Transform faceCentralPosition;

    protected override GameObject InstantiateEmotionElement(string emotionString, Vector3 position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/RiquadriEmozione/riquadro" + emotionString), position, Quaternion.identity) as GameObject;
        return obj;
    }

    //Metodo che si occupa di far spawnare la faccia da indovinare al centro della scena
    protected override void SetupCentralEmotion()
    {
        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/Faces/face" + GetEmotionString()), faceCentralPosition.position, Quaternion.identity);
        //Disabilitiamo il collider perchè non vogliamo che sia cliccabile
        face.GetComponent<Collider2D>().enabled = false;
    }
}
