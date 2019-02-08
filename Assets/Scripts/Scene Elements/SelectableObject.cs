using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectableObject : MonoBehaviour
{
    [SerializeField]
    private Emotion emotionType;
    //Variabile per dirci se l'oggetto è stato scelto oppure no
    [SerializeField]
    private bool selected;

    private Vector3 startScale;
    private Vector3 centralPosition;

    private Animator animator;

    //serve solo per i pezzi della faccia
    public FaceParts facePartType;

    public Vector3 initialPosition;

    public delegate void OnObjectSelected(GameObject objectSelected);
    public static event OnObjectSelected objectSelectedEvent;

    public void Start()
    {
        initialPosition = transform.position;
        selected = false;
        startScale = transform.localScale;
        animator = gameObject.AddComponent<Animator>();
        HandSelectionGame.SelectableObjectClicked += ObjectSelected;
        HandSelectionGame.SelectableObjectEnter += OnHandEnter;
        HandSelectionGame.SelectableObjectExit += OnHandExit;

        GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/Pop");
    }

    public void SetEmotionType(Emotion emotion)
    {
        emotionType = emotion;
    }

    public void OnHandEnter(SelectableObject obj)
    {
        if (obj == this && !selected)
            animator.SetTrigger("PopUp");
    }

    private void OnHandExit(SelectableObject obj)
    {
        if (obj == this && !selected)
            animator.SetTrigger("PopDown");
    }

    //Metodo che viene chiamato nel momento in cui si seleziona un oggetto
    private void ObjectSelected(SelectableObject obj)
    {
        if (obj == this && !selected)
        {
            selected = true;
            animator.enabled = false;
            objectSelectedEvent(this.gameObject);
        }
    }

    //disattiva un selectable object in tutti i suoi aspetti e lo rende meno opaco
    public void DeactivateSelectableObject()
    {
        //azzera tutto e annulla la possibilita' di selezioanre di nuovo
        transform.localScale = new Vector3(1, 1, 1);
        animator.enabled = false;
        selected = false;
        GetComponent<Collider2D>().enabled = false;
        //se e' una faccia metti l'opacita' piu' bassa
        if (GetComponent<Avatar>() != null)
            GetComponent<Avatar>().ChangeFaceOpacity(100);

        //se e' il tessto del minigame 2 mettilo meno opaco
        else if (GetComponent<TextMeshProUGUI>() != null)
            GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 100);

    }
    //disattiva un selectable object in tutti i suoi aspetti e lo rende meno opaco
    public void ReactivateSelectableObject()
    {
        //azzera tutto e annulla la possibilita' di selezioanre di nuovo
        transform.localScale = new Vector3(1, 1, 1);
        animator.enabled = true;
        selected = false;
        GetComponent<Collider2D>().enabled = true;
        //se e' una faccia metti l'opacita' piu' alto
        if (GetComponent<Avatar>() != null)
            GetComponent<Avatar>().ChangeFaceOpacity(255);

        //se e' il tessto del minigame 2 mettilo piu' opaco
        else if (GetComponent<TextMeshProUGUI>() != null)
            GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);

    }

    public bool isSelected()
    {
        if (selected)
            return true;
        else
            return false;
    }
    public Emotion GetEmotionType()
    {
        return this.emotionType;
    }

    private void OnDisable()
    {
        HandSelectionGame.SelectableObjectClicked -= ObjectSelected;
        HandSelectionGame.SelectableObjectEnter -= OnHandEnter;
        HandSelectionGame.SelectableObjectExit -= OnHandExit;

    }

    //setta i parametri di emozione e tipo di pezzo della faccia
    public void SetFacePartEmotion(Emotion emo, AvatarSettings avatar)
    {
        emotionType = emo;
        AssignFacePartSprite(avatar);
    }

    void AssignFacePartSprite(AvatarSettings avatar)
    {
        if (facePartType == FaceParts.Mouth)
        {
            transform.Find("Mouth").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/" + facePartType.ToString() + "/" + emotionType.ToString());
        }

        else if (facePartType == FaceParts.Eyes)
        {
            transform.Find("Eyes").GetComponent<SpriteRenderer>().color = AvatarData.eyesColorDictionary[avatar.eyesColor];
            transform.Find("Eyes").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/" + facePartType.ToString() + "/Default");

            if (emotionType.ToString() == "Disgusto")
            {
                transform.Find("Eyes").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/" + facePartType.ToString() + "/Disgusto");
                transform.Find("EyesLight").gameObject.SetActive(false);
            }

            transform.Find("Eyebrow").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Avatar/Eyebrow/" + avatar.gender.ToString() + emotionType.ToString());
            transform.Find("Eyebrow").GetComponent<SpriteRenderer>().color = AvatarData.hairColorDictionary[avatar.hairColor];

        }
    }
}
