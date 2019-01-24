using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCompositionGame : Hand
{

    [SerializeField]
    private List<DroppableArea> droppableArea;
    [Header ("Reference al pezzo della faccia preso")]
    private GameObject pieceTaken;

    public delegate void OnPiecePositioned ();
    public static event OnPiecePositioned piecePositioned;


    protected override void Start ()
    {
        base.Start ();
        droppableArea = new List<DroppableArea> ();
    }
    //checka ogni frame se sto premendo qualcosa
    protected override void CheckInputs ()
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

    protected override void OnTriggerEnter2D (Collider2D collision)
    {
        if (!dragging)
        {
            if (collision.gameObject.tag == "FacePiece")
                pieceTaken = collision.gameObject;
        }

        else
        {
            //se sono entrato in una droppable area
            if (collision.gameObject.tag == "DroppableArea")
            {
                droppableArea.Add (collision.gameObject.GetComponent<DroppableArea> ());
            }
        }
    }

    protected override void OnTriggerExit2D (Collider2D collision)
    {
        //se non sto draggando nulla mi interessa sapere con che pezzo collido
        if (!dragging)
        {
            if (pieceTaken == collision.gameObject)
                pieceTaken = null;
        }

        //se invece sto draggando devo sapere se sono uscito da una droppable area
        else
        {
            if (collision.gameObject.tag == "DroppableArea")
                //rimuovi il primo elemento della lista per forza
                droppableArea.Remove (collision.GetComponent<DroppableArea> ());
        }
    }

    //disambigua la scelta tra bocca e occhi e da' un consiglio giusto
    /* protected override void ChooseProperAdvice (string type)
    {
        if (type == "EYES")
            GiveAdvice ("Prova posizionando gli occhi sopra al naso");
        else if (type == "MOUTH")
            GiveAdvice ("Prova posizionando la bocca sotto al naso");
    }
 */
    //DETECT COLLISION DA NON USARE ANCORA
    /* protected override void DetectCollision () 
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll (transform.position, 0.5f, Vector2.up, 0, layerToDetectCollision);

        foreach (RaycastHit2D hitShape in hit)
        {
            if (!dragging)
            {
                if (hitShape.collider.gameObject.tag == "FacePiece")
                    pieceTaken = hitShape.collider.gameObject;
            }
            else
            {
                //se sono entrato in una droppable area
                if (hitShape.collider.gameObject.tag == "DroppableArea")
                {
                    AddDroppableArea (hitShape.collider.gameObject.GetComponent<DroppableArea> ());
                }
            }

            Debug.Log (hitShape.collider.gameObject.name);

        }
    }*/

}
