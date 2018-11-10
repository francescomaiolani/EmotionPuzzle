using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    [Header("Vicinanza della mano con il mouse")]
    public float lerpingFactor;
    [Header("Reference al pezzo della faccia preso")]
    public GameObject pieceTaken;
    
    [SerializeField]
    public List<DroppableArea> droppableArea;

    public SpriteRenderer spriteRenderer;
    public Sprite[] hands;

    protected bool dragging;

    public delegate void OnAdviceGiven(string advice);
    public static event OnAdviceGiven adviceGiven;

    private void Start()
    {
        droppableArea = new List<DroppableArea>();
    }

    // Update is called once per frame
    void Update () {
        FollowMouse();
        CheckInputs();    
	}

    //la mano segue il mouse 
    void FollowMouse()
    {
        Vector2 newPosition;
        Vector2 mousePositionInWorldCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        newPosition = new Vector2(Mathf.Lerp(transform.position.x, mousePositionInWorldCoordinates.x, lerpingFactor), Mathf.Lerp(transform.position.y, mousePositionInWorldCoordinates.y, lerpingFactor));
        transform.position = newPosition;
    }

    //checka ogni frame se sto premendo qualcosa
    protected virtual void CheckInputs() {

        //se sto cliccando >>> Inizia il drag
        if (Input.GetMouseButtonDown(0))
        {
            ChangeHandSprite("closed");
            //annulla el precedenti reference alle droppable area che avevi toccato
            droppableArea.Clear();
            //se ho un oggetto con cui ho colliso
            if (pieceTaken != null)
            {
                pieceTaken.GetComponent<DraggableObject>().StartDragging(this.gameObject);
                dragging = true;
            }
        }

        //Quando sollevo il mouse >>> inizio drop
        else if (Input.GetMouseButtonUp(0))
        {
            ChangeHandSprite("open");
            // se sto effettivamente trascinando qualcosa
            if (pieceTaken != null)
            {
                DraggableObject draggableComponent = pieceTaken.GetComponent<DraggableObject>();
                if (draggableComponent.GetDroppableArea() != null)
                    draggableComponent.GetDroppableArea().SetOccupied(false);

                //se sei sopra una droppable area 
                if (droppableArea.Count > 0)
                {
                    //ho trovato qualcosa?
                    bool found = false;
                    foreach (DroppableArea d in droppableArea)
                    {
                        //se e' giusta la droppable area
                        if (draggableComponent.CheckIfCorrectDropArea(d.GetMainType(), d.GetSubType()) && !d.GetOccupied())
                        {
                            DropItem(d, draggableComponent);
                            found = true;
                        }
                    }
                    //se non ho trovato nessuna droppable area compatibile
                    if (!found)
                    {
                        ChoseProperAdvice(draggableComponent.GetSubType());
                        draggableComponent.StopDragging(false, null);
                    }
                }
                else {
                    //altrimenti semplicemente sei fuori da una droppable area 
                    draggableComponent.StopDragging(false, null);
                    if (!draggableComponent.GetInCorrectPlace())
                        adviceGiven("Posiziona il pezzo sopra la faccia del ragazzo!");
                }


                pieceTaken = null;
                dragging = false;
            }
        }
    }

    //disambigua la scelta tra bocca e occhi e da' un consiglio giusto
    protected virtual void ChoseProperAdvice(string type) { }

    //posso chiamare un evento solo da questa classe quindi aggiro chiamando un metodo che chiama l'evento
    protected void GiveAdvice(string advice) {
        adviceGiven(advice);
    }

    //Cambia l'immagine della mano da aperta a ciusa e viceversa
    void ChangeHandSprite(string state)
    {
        if (state == "closed")
            spriteRenderer.sprite = hands[1];
        else if (state == "open")
            spriteRenderer.sprite = hands[0];
    }

    //droppa nella droppable area il droppable object trascinato
    void DropItem(DroppableArea d, DraggableObject draggableComponent)
    {
        draggableComponent.StopDragging(draggableComponent.CheckIfCorrectDropArea(d.GetMainType(), d.GetSubType()), d.gameObject);
        draggableComponent.SetDropAreaDestination(d.transform.position);
        draggableComponent.SetDroppableArea(d.gameObject);
        d.SetContainedPiece(draggableComponent.gameObject);
        d.SetOccupied(true);
    }

    //Check delle collisioni va fatto nelle classi derivate perche' ogni minigioco ha bisogno di input diversi
    protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    protected virtual void OnTriggerExit2D(Collider2D collision) { }
}
