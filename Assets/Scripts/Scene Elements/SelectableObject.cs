using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {

    [SerializeField]
    private Emotion emotionType;

    private Vector3 startScale;
    private Vector3 centralPosition;

    private GuessExpressionManager gameManager;

    public delegate void OnObjectSelected();
    public static event OnObjectSelected objectSelectedEvent;

    public void Start()
    {
        startScale = transform.localScale;
        gameManager = FindObjectOfType<GuessExpressionManager>();
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
        transform.position = Vector3.Lerp(transform.position, centralPosition, 1.0f);
        DisableColliders();
        gameManager.SetAnswer(emotionType);
        objectSelectedEvent();
    }

    public void SetCentralPosition(Transform centralPosition)
    {
        this.centralPosition = centralPosition.position;
    }

    private void DisableColliders()
    {
        SelectableObject[] draggableObjects = FindObjectsOfType<SelectableObject>();
        foreach (SelectableObject s in draggableObjects)
            s.GetComponent<Collider2D>().enabled = false;
    }

    private void EnableColliders()
    {
        SelectableObject[] draggableObjects = FindObjectsOfType<SelectableObject>();
        foreach (SelectableObject s in draggableObjects)
            s.GetComponent<Collider2D>().enabled = true;
    }
}
