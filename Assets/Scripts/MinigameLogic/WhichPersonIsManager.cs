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

    //array che viene riempito man mano che si creano le facce delle varie persone, Serve a sapere quali posizioni sono state occupate
    bool[] facesCreated;
    //numero di facce corrette create
    int numberOfCorrectFaces;

    private void Start()
    {
        SelectableObject.objectSelectedEvent += CheckIfGameCompleted;      
        SpawnSceneObjects();
    }

    void CheckIfGameCompleted() {
        //qui andra' fatto il check sul fatto che il gioco e' stato completato
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

        facesCreated = new bool[4];
        CreateFaceOfCorrectPeople();
        CreateFaceOfIncorrectPeople();
    }

    void CreateFaceOfCorrectPeople()
    {
        numberOfCorrectFaces = Random.Range(0, 3);
        for (int i = 0; i < numberOfCorrectFaces; i++) {
            InstantiateFace(mainEmotion);
        }

    }

  

    void CreateFaceOfIncorrectPeople() {
        int numberOfIncorrectFaces = 4 - numberOfCorrectFaces;
        for (int i = 0; i < numberOfIncorrectFaces; i++)
        {
            InstantiateFace(PickNotMainEmotion(mainEmotion));
        }

    }

    //metodo che istanzia una faccia delle 4 di persone random
    void InstantiateFace(Emotion emotion)
    {
        int randomPosition = Random.Range(0, facesCreated.Length);

        //finche' quella posizione e' gia' occupata ceerca altri posti
        while (facesCreated[randomPosition] == true)
            randomPosition = Random.Range(0, facesCreated.Length);

        facesCreated[randomPosition] = true;
        GameObject Face = Instantiate(Resources.Load<GameObject>("Prefab/ImagePrefab/FacePrefab"), spawnPointPositions[randomPosition].transform.position, Quaternion.identity);
        Face.GetComponent<SelectableObject>().SetEmotionType(emotion);
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
        //scglie l'espressione della faccia principale
        PickNewEmotion();
        //aggiorna la UI
        FindObjectOfType<UIWhichPersonIsManager>().UpdateUI(this);
         
        //crea le faccia principale, quella dell'emozione corretta
        CreateMainFace();
        //crea e 4 facce di altre persone
        CreateFacesOfDifferentPeople();
    }
}
