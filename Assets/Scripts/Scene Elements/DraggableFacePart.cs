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
    public void SetFacePartEmotion(Emotion emo, AvatarSettings avatar)
    {
        emotion = emo;
        AssignFacePartSprite(avatar);
    }

    void AssignFacePartSprite(AvatarSettings avatar)
    {
        if (facePartType == FaceParts.Mouth)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/" + facePartType.ToString() + "/" + emotion.ToString());
        }

        else if (facePartType == FaceParts.Eyes)
        {
            transform.Find("Eyes").GetComponent<SpriteRenderer>().color = AvatarData.eyesColorDictionary[avatar.eyesColor];
            transform.Find("Eyes").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/" + facePartType.ToString() + "/Default");

            if (emotion.ToString() == "Disgusto")
            {
                transform.Find("Eyes").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/" + facePartType.ToString() + "/Disgusto");
                transform.Find("EyesLight").gameObject.SetActive(false);
            }

            transform.Find("Eyebrow").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/Eyebrow/" + avatar.gender.ToString() + emotion.ToString());
            transform.Find("Eyebrow").GetComponent<SpriteRenderer>().color = AvatarData.hairColorDictionary[avatar.hairColor];

        }
    }

    //metodo per checkare se hai posizionato il pezzo della faccia sulla giusta droppable area
    public override bool CheckIfCorrectDropArea(DraggableObjectType area, FaceParts subArea)
    {
        if (area == draggableObjectType && subArea == facePartType)
            return true;
        else
            return false;
    }

    //metodo che ritorna il sottotipo di un draggableObject (in questo caso il pezzo della faccia EYES o MOUTH )
    public override string GetSubType()
    {
        return facePartType.ToString();
    }

    public Emotion GetEmotion()
    {
        return emotion;
    }
}
