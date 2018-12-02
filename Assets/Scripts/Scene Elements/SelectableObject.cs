﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {

    [SerializeField]
    private Emotion emotionType;
    //Variabile per dirci se l'oggetto è stato scelto oppure no
    [SerializeField]
    private bool selected;

    private Vector3 startScale;
    private Vector3 centralPosition;

    public delegate void OnObjectSelected(GameObject objectSelected);
    public static event OnObjectSelected objectSelectedEvent;

    public void Start()
    {
        selected = false;
        startScale = transform.localScale;
    }

    public void SetEmotionType(Emotion emotion)
    {
        emotionType = emotion;
    }

    public void OnMouseEnter()
    {
        transform.localScale = transform.localScale * 1.1f;
    }

    private void OnMouseExit()
    {
        transform.localScale = startScale;
    }

    //Metodo che viene chiamato nel momento in cui si seleziona un oggetto
    private void OnMouseDown()
    {
        selected = true;
        objectSelectedEvent(this.gameObject);
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

    //public static void DisableColliders()
    //{
    //    SelectableObject[] selectableObjects = FindObjectsOfType<SelectableObject>();
    //    foreach (SelectableObject s in selectableObjects)
    //        s.GetComponent<Collider2D>().enabled = false;
    //}

    //private static void EnableColliders()
    //{
    //    SelectableObject[] selectableObjects = FindObjectsOfType<SelectableObject>();
    //    foreach (SelectableObject s in selectableObjects)
    //        s.GetComponent<Collider2D>().enabled = true;
    //}
}