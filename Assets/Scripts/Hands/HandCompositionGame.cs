using UnityEngine;

public class HandCompositionGame : Hand
{

    LayerMask layerToDetectCollision;

    protected override void Start ()
    {
        base.Start ();
        layerToDetectCollision = LayerMask.GetMask ("Default");

    }
    //disambigua la scelta tra bocca e occhi e da' un consiglio giusto
    protected override void ChooseProperAdvice (string type)
    {
        if (type == "EYES")
            GiveAdvice ("Prova posizionando gli occhi sopra al naso");
        else if (type == "MOUTH")
            GiveAdvice ("Prova posizionando la bocca sotto al naso");
    }

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
}
