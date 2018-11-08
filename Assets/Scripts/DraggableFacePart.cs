using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceParts { EYES, MOUTH };

public class DraggableFacePart : DraggableObject {

    public FaceParts facePartType;

    public override bool CheckIfCorrectDropArea(DraggableObjectType area, FaceParts subArea)
    {
        if (area == draggableObjectType && subArea == facePartType)
            return true;
        else
            return false;
    }


    public override string GetSubType()
    {
        return facePartType.ToString();
    }
}
