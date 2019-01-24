using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header ("Vicinanza della mano con il mouse")]
    public float lerpingFactor;
    [Header ("Reference al pezzo della faccia preso")]
    public GameObject pieceTaken;
    [SerializeField]
    public List<DroppableArea> droppableArea;
    public SpriteRenderer spriteRenderer;
    public Sprite[] hands;
    private KinectBodySkeleton currentSkeleton;
    protected bool dragging;
    //[Header("mettere true se siamo in magicRoom")]
    private bool inMagicRoom;

    public delegate void OnAdviceGiven (string advice);
    public static event OnAdviceGiven adviceGiven;
    public delegate void OnPiecePositioned ();
    public static event OnPiecePositioned piecePositioned;

    protected virtual void Start ()
    {
        droppableArea = new List<DroppableArea> ();
        if (MagicRoomKinectV2Manager.instance != null)
            inMagicRoom = true;
        else
            inMagicRoom = false;
    }

    // Nella routine di ogni mano devono essere eseguiti gli step di :
    //segui mouse o mano reale
    void Update ()
    {
        FollowMouseOrKinectHand ();
        CheckInputs ();
        //DetectCollision ();
    }
    //la mano segue il mouse 
    void FollowMouseOrKinectHand ()
    {
        Vector2 mousePositionInWorldCoordinates;
        if (inMagicRoom)
        {
            if (MagicRoomKinectV2Manager.instance.MagicRoomKinectV2Manager_active)
                mousePositionInWorldCoordinates = MagicRoomKinectV2Manager.instance.GetCloserSkeleton ().HandRight * 11;
            else
                mousePositionInWorldCoordinates = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
        }
        else
            mousePositionInWorldCoordinates = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
        Vector2 newPosition;
        newPosition = new Vector2 (Mathf.Lerp (transform.position.x, mousePositionInWorldCoordinates.x, lerpingFactor), Mathf.Lerp (transform.position.y, mousePositionInWorldCoordinates.y, lerpingFactor));
        transform.position = newPosition;
    }
    //cose da fare quando la mano e' chiusa o quando il mouse e' cliccato
    void Drag ()
    {
        //MagicRoomLightManager.instance.sendColour(Color.blue);
        ChangeHandSprite ("closed");
        //annulla el precedenti reference alle droppable area che avevi toccato
        droppableArea.Clear ();
        //se ho un oggetto con cui ho colliso
        if (pieceTaken != null)
        {
            //inizia a draggare l'oggetto
            pieceTaken.GetComponent<DraggableObject> ().StartDragging (this.gameObject);
            dragging = true;
        }
    }
    //cose da fare quando il mouse viene rilasciato o quando la mano di apre
    void Drop ()
    {
        ChangeHandSprite ("open");
        // se sto effettivamente trascinando qualcosa
        if (pieceTaken != null)
        {
            DraggableObject draggableComponent = pieceTaken.GetComponent<DraggableObject> ();
            if (draggableComponent.GetDroppableArea () != null)
            {
                draggableComponent.GetDroppableArea ().SetOccupied (false);
            }
            //se sei sopra una droppable area 
            if (droppableArea.Count > 0)
            {
                //ho trovato qualcosa?
                bool found = false;
                foreach (DroppableArea d in droppableArea)
                {
                    //se e' giusta la droppable area
                    if (draggableComponent.CheckIfCorrectDropArea (d.GetMainType (), d.GetSubType ()) && !d.GetOccupied ())
                    {
                        DropItem (d, draggableComponent);
                        piecePositioned ();
                        found = true;
                    }
                }
                //se non ho trovato nessuna droppable area compatibile
                if (!found)
                    draggableComponent.StopDragging (false, null);
            }
            else
                //altrimenti semplicemente sei fuori da una droppable area 
                draggableComponent.StopDragging (false, null);
            //alla fine comunque vada non hai piu' un pezzo da draggare e non stai draggando
            pieceTaken = null;
            dragging = false;
        }
    }
    //checka ogni frame se sto premendo qualcosa
    protected virtual void CheckInputs ()
    {
        //non bella soluzione ma serve a noi per differenziare se siamo nella magic room o no
        if (inMagicRoom)
        {
            //se sto cliccando >>> Inizia il drag
            if (currentSkeleton.isRightHandClosed (0.1f))
                Drag ();
            //Quando sollevo il mouse >>> inizio drop
            else if (!currentSkeleton.isRightHandClosed (0.1f))
                Drop ();
        }
        //se non sono nella magic room e quindi il controllo deve essere effettuato col mouse e basta
        else
        {
            if (Input.GetMouseButtonDown (0))
                Drag ();
            else if (Input.GetMouseButtonUp (0))
                Drop ();
        }
    }

    //lancia un circle raycast per vedere se ho colliso con qualche item. Non so se va bene implementarlo
    //protected virtual void DetectCollision () { }

    //disambigua la scelta tra bocca e occhi e da' un consiglio giusto
    protected virtual void ChooseProperAdvice (string type) { }

    //posso chiamare un evento solo da questa classe quindi aggiro chiamando un metodo che chiama l'evento
    protected void GiveAdvice (string advice)
    {
        adviceGiven (advice);
    }

    //Cambia l'immagine della mano da aperta a ciusa e viceversa
    void ChangeHandSprite (string state)
    {
        if (state == "closed")
            spriteRenderer.sprite = hands[1];
        else if (state == "open")
            spriteRenderer.sprite = hands[0];
    }
    //droppa nella droppable area il droppable object trascinato
    void DropItem (DroppableArea d, DraggableObject draggableComponent)
    {
        draggableComponent.StopDragging (draggableComponent.CheckIfCorrectDropArea (d.GetMainType (), d.GetSubType ()), d.gameObject);
        draggableComponent.SetDropAreaDestination (d.transform.position);
        draggableComponent.SetDroppableArea (d.gameObject);
        d.SetContainedPiece (draggableComponent.gameObject);
        d.SetOccupied (true);
    }

    //aggiunge una droppable area quando passi sopra ad una. Non piu' utile
    protected void AddDroppableArea (DroppableArea area)
    {
        if (droppableArea.Contains (area))
            return;

        else
            droppableArea.Add (area);
    }

     //Check delle collisioni va fatto nelle classi derivate perche' ogni minigioco ha bisogno di input diversi
     protected virtual void OnTriggerEnter2D (Collider2D collision) { }
     protected virtual void OnTriggerExit2D (Collider2D collision) { }
}
