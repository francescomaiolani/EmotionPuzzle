using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HowDoYouFeelManager : SelectionGameManager
{
    [Header ("Inserisci la posizione dove viene inserita la faccia da indovinare")]
    public Transform faceCentralPosition;

    protected override GameObject InstantiateEmotionElement (string emotionString, Vector3 position)
    {
        GameObject obj = Instantiate (Resources.Load<GameObject> ("Prefab/SelectableObject/RiquadriEmozione/RiquadroEmozione"), position, Quaternion.identity) as GameObject;
        obj.GetComponent<TextMeshProUGUI> ().text = ConvertInCorrectText (emotionString);
        obj.transform.parent = FindObjectOfType<Canvas> ().transform;
        obj.transform.localScale = new Vector3 (1, 1, 1);
        return obj;
    }

    //Metodo che si occupa di far spawnare la faccia da indovinare al centro della scena
    protected override void SetupCentralEmotion ()
    {
        GameObject face = Instantiate (Resources.Load<GameObject> ("Prefab/SelectableObject/Faces/face" + GetEmotionString ()), faceCentralPosition.position, Quaternion.identity);
        //Disabilitiamo il collider perchè non vogliamo che sia cliccabile
        face.GetComponent<Collider2D> ().enabled = false;
    }
}
