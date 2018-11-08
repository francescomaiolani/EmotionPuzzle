using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableArea : MonoBehaviour {


    public DraggableObjectType mainType;

    public FaceParts subType;

    [SerializeField]
    private GameObject pieceContained;

    private bool occupied;

    public void SetContainedPiece(GameObject piece) {
        pieceContained = piece;
    }

    public DraggableObjectType GetMainType() {
        return mainType;
    }

    public FaceParts GetSubType() {
        return subType;
    }

    public void SetOccupied(bool state)
    {
        occupied = state;
    }

    public bool GetOccupied() {
        return occupied;
    }

}
