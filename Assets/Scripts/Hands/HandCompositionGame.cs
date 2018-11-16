using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCompositionGame : Hand {


    //disambigua la scelta tra bocca e occhi e da' un consiglio giusto
    protected override void ChooseProperAdvice(string type)
    {
        if (type == "EYES")
            GiveAdvice("Prova posizionando gli occhi sopra al naso");
        else if (type == "MOUTH")
            GiveAdvice("Prova posizionando la bocca sotto al naso");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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
                droppableArea.Add(collision.gameObject.GetComponent<DroppableArea>());
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
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
                droppableArea.Remove(collision.GetComponent<DroppableArea>());
        }
    }
}
