using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DraggableObjectType { FACE, FACE_PIECE, WORD, SCENE }

public abstract class DraggableObject : MonoBehaviour {

    //offset dalla mano
    private Vector3 offsetFromHandWhenDragging;

    private Vector3 startingPosition;

    [SerializeField]
    private GameObject droppableArea;

    //se il pezzo si sta muovendo o e' fermo
    private bool move;

    [SerializeField]
    //se il pezzo e' stato droppato in un'area giusta
    bool inCorrectPlace;

    //lerping del movimento nella destinazione assegnata
    private float lerpingFactor = 0.4f;

    // destinazione scelta
    Vector3 dropAreaDestination;

    public DraggableObjectType draggableObjectType;

    private void Start()
    {
        //offset del game object rispetto alla mano per renderlo distinguibile
        offsetFromHandWhenDragging = new Vector3(0, 0, 0);

        //salva la posizione iniziale nel caso in cui droppi in una zona in cui non puoi droppare
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (move)
            MovePieceToPosition(dropAreaDestination);
    }

    public void StartDragging(GameObject hand) {


        //disabilitare il composite collider che si crea parentando il game object come figlio della mano
        //il delay e' necessario altrimenti il collider fa scattare il trigget exit e annulla la reference all'oggetto draggato
        Invoke("DisableCollider", 0.01f);

        //non e' piu' dropped
        move = false;

        // roota alla mano
        transform.parent = hand.transform;

        //posiziona un po' staccato in modo da mantenerlo visibile
        transform.localPosition = offsetFromHandWhenDragging;

        if (inCorrectPlace) {
            droppableArea.GetComponent<DroppableArea>().SetOccupied(false);
            inCorrectPlace = false;
        }

        droppableArea = null;

    }

    //stop dragging ti dice gia' se la posizione e' droppable o no e l'eventuale destinazione
    public virtual void StopDragging(bool correct,  GameObject destination) {

        GetComponent<CapsuleCollider2D>().enabled = true;

        //stacca dalla mano
        transform.parent = null;

        if (correct)  {
            move = true;
            inCorrectPlace = true;
            dropAreaDestination = destination.transform.position;
        }
        else {
            inCorrectPlace = false;
            dropAreaDestination = startingPosition;
            move = true;                
        }
    }

    void DisableCollider() {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public virtual bool CheckIfCorrectDropArea(DraggableObjectType area, FaceParts subArea) {
        return true;
    }

    public void MovePieceToPosition(Vector3 destination) {
        Vector3 newPosition = new Vector2(Mathf.Lerp(transform.position.x, destination.x, lerpingFactor), Mathf.Lerp(transform.position.y, destination.y, lerpingFactor));
        transform.position = newPosition;
    }

    public virtual string GetSubType() {
        return  null;
    }

    public bool GetInCorrectPlace() {
        return inCorrectPlace;
    }

    public void SetDroppableArea(GameObject area)
    {
        droppableArea = area;
    }

    public DroppableArea GetDroppableArea() {
        if (droppableArea != null)
            return droppableArea.GetComponent<DroppableArea>();
        else return null;
    }

    public void SetDropAreaDestination(Vector3 area) {
        dropAreaDestination = area;
    }
}
