using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessExpressionManager : SelectionGameManager
{

    [Header ("Riquadro dell'interfaccia dove viene salvata l'emotion principale")]
    public Text emotionType;

    protected override void SetupCentralEmotion ()
    {

        emotionType.text = ConvertInCorrectText (GetEmotionString ());
    }
    protected override GameObject InstantiateEmotionElement (string emotionString, Vector3 position)
    {
        GameObject obj = Instantiate (Resources.Load<GameObject> ("Prefab/SelectableObject/Faces/face" + emotionString), position, Quaternion.identity);
        return obj;
    }
}
