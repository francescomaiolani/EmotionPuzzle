using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public delegate void OnObjectSelected (GameObject objectSelected);
    public static event OnObjectSelected objectSelectedEvent;

    public void Start ()
    {
        selected = false;
        startScale = transform.localScale;
        animator = gameObject.AddComponent<Animator> ();
        HandSelectionGame.SelectableObjectClicked += ObjectSelected;
        HandSelectionGame.SelectableObjectEnter += OnHandEnter;
        HandSelectionGame.SelectableObjectExit += OnHandExit;

        GetComponent<Animator> ().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animator/Pop");
    }

    public void SetEmotionType (Emotion emotion)
    {
        emotionType = emotion;
    }

    public void OnHandEnter (SelectableObject obj)
    {
        if (obj == this && !selected)
            animator.SetTrigger ("PopUp");
    }

    private void OnHandExit (SelectableObject obj)
    {
        if (obj == this && !selected)
            animator.SetTrigger ("PopDown");
    }

    //Metodo che viene chiamato nel momento in cui si seleziona un oggetto
    private void ObjectSelected (SelectableObject obj)
    {
        Debug.Log ("Event received");
        if (obj == this && !selected)
        {
            selected = true;
            animator.enabled = false;
            objectSelectedEvent (this.gameObject);
        }
    }

    public bool isSelected ()
    {
        if (selected)
            return true;
        else
            return false;
    }
    public Emotion GetEmotionType ()
    {
        return this.emotionType;
    }

    private void OnDisable ()
    {
        HandSelectionGame.SelectableObjectClicked -= ObjectSelected;
        HandSelectionGame.SelectableObjectEnter -= OnHandEnter;
        HandSelectionGame.SelectableObjectExit -= OnHandExit;

    }
}
