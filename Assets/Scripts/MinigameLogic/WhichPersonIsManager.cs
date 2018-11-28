using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichPersonIsManager : MinigameManager
{

    [Header("posizioni della bocca principale e deglio occhi principali")]
    public Transform mainMouthPosition;
    public Transform mainEyesPosition;

    //scale della faccia principale perche' i pezzi spawnati sono piu' piccoli
    public Vector3 mainFaceScale;

    private void Start()
    {
        //scglie l'espressione della faccia principale
        PickNewEmotion();
        CreateMainFace();
        CreateFacesOfDifferentPeople();

    }



    void CreateMainFace() {

        //crea 2 game Object con solo un'immagine attaccata
        GameObject mouth = Instantiate(Resources.Load<GameObject>("Prefab/ImagePrefab/ImagePrefab"), mainMouthPosition.position, Quaternion.identity);
        AssignFacePartSprite(mouth.GetComponent<SpriteRenderer>(), FaceParts.Mouth, mainEmotion);
        GameObject eyes = Instantiate(Resources.Load<GameObject>("Prefab/ImagePrefab/ImagePrefab"), mainEyesPosition.position, Quaternion.identity);
        AssignFacePartSprite(eyes.GetComponent<SpriteRenderer>(), FaceParts.Eyes, mainEmotion);

        //cambia lo scale per adattarlo allo scale della faccia
        mouth.transform.localScale = mainFaceScale;
        eyes.transform.localScale = mainFaceScale;

    }

    void CreateFacesOfDifferentPeople() {
        CreateFaceOfCorrectPeople();
        CreateFaceOfIncorrectPeople();
    }

    void CreateFaceOfCorrectPeople()
    {


    }

    void CreateFaceOfIncorrectPeople() {

    }

    void AssignFacePartSprite(SpriteRenderer spr, FaceParts facePartType, Emotion emotion)
    {
        spr.sprite = Resources.Load<Sprite>("Sprite/FacePieces/" + facePartType.ToString() + "/" + facePartType.ToString() + emotion.ToString());
    }

    public override string GetEmotionString()
    {
        return mainEmotion.ToString();
    }

    protected override void DestroySceneObjects()
    {
    }

    protected override void SpawnSceneObjects()
    {
    }
}
