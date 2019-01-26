using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceParts { Eyes, Mouth }

public class DraggableFacePart : DraggableObject
{
    public FaceParts facePartType;
    [SerializeField]
    Emotion emotion;

    //setta i parametri di emozione e tipo di pezzo della faccia
    public void SetFacePartEmotion (Emotion emo)
    {
        emotion = emo;
        AssignFacePartSprite ();
    }

    void AssignFacePartSprite ()
    {
        GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprite/FacePieces/" + facePartType.ToString () + "/" + facePartType.ToString () + emotion.ToString ());
    }

    //metodo per checkare se hai posizionato il pezzo della faccia sulla giusta droppable area
    public override bool CheckIfCorrectDropArea (DraggableObjectType area, FaceParts subArea)
    {
        if (area == draggableObjectType && subArea == facePartType)
            return true;
        else
            return false;
    }

    //metodo che ritorna il sottotipo di un draggableObject (in questo caso il pezzo della faccia EYES o MOUTH )
    public override string GetSubType ()
    {
        return facePartType.ToString ();
    }

    public Emotion GetEmotion ()
    {
        return emotion;
    }
}
