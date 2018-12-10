using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichPersonIsManager : MinigameManager
{

    [Header("gameObject della bocca principale e deglio occhi principali")]
    public GameObject mainMouth;
    public GameObject mainEyes;

    //array che viene riempito man mano che si creano le facce delle varie persone, Serve a sapere quali posizioni sono state occupate
    Emotion?[] facesCreated;
    //numero di facce corrette create
    int numberOfCorrectFaces;

    List<GameObject> facesSelected;

    UIWhichPersonIsManager UIManager;

    private void Start()
    {
        UIManager = FindObjectOfType<UIWhichPersonIsManager>();
        facesSelected = new List<GameObject>();
        SelectableObject.objectSelectedEvent += CheckIfGameCompleted;
        StartNewRound();
    }

    public override void StartNewRound()
    {
        DestroyAnswerObjectSpawned();
        DestroySceneObjects();
        //Disabilita schermata di fine round
        endRoundPanel.SetActive(false);
        //Aumentiamo il contatore dei round
        UpdateRound();
        //scglie l'espressione della faccia principale
        PickNewEmotion();
        //aggiorna la UI
        FindObjectOfType<UIWhichPersonIsManager>().UpdateUI(this);

        SpawnSceneObjects();
    }

    protected override void SpawnSceneObjects()
    {
        //crea le faccia principale, quella dell'emozione corretta
        CreateMainFace();
        //crea e 4 facce di altre persone
        CreateFacesOfDifferentPeople();
    }

    void CreateMainFace()
    {
        //crea 2 game Object con solo un'immagine attaccata
        AssignFacePartSprite(mainMouth.GetComponent<SpriteRenderer>(), FaceParts.Mouth, mainEmotion);
        AssignFacePartSprite(mainEyes.GetComponent<SpriteRenderer>(), FaceParts.Eyes, mainEmotion);
    }

    void CreateFacesOfDifferentPeople()
    {

        facesCreated = new Emotion?[4] {null,null,null,null };
        AssignFaceOfCorrectPeople();
        AssignFaceOfIncorrectPeople();
        StartCoroutine(CreateFaces());
    }

    //coroutine che crea le 4 facce una ogni 0.2f secondi
    private IEnumerator CreateFaces()
    {
        int i = 0;

        while (i < 4)
        {
            yield return new WaitForSeconds(0.25f);
            InstantiateFace(facesCreated[i], i);
            i++;
        }    
    }

    //metodo che assegna agli occhi o alla bocca dalla main face l'immagine dell'emozione giusta 
    void AssignFacePartSprite(SpriteRenderer spr, FaceParts facePartType, Emotion emotion)
    {
        spr.sprite = Resources.Load<Sprite>("Sprite/FacePieces/" + facePartType.ToString() + "/" + facePartType.ToString() + emotion.ToString());
    }

    // da cambiare nel momento in cui avremo le facce di tutte le persone
    void AssignFaceSprite(SpriteRenderer spr, Emotion emotion)
    {
        spr.sprite = Resources.Load<Sprite>("Sprite/CompleteFaces/" + "face" + emotion.ToString());
    }


    //controllo sul fatto che hai selezionato tutte le facce corrette o hai sbagliato
    void CheckIfGameCompleted(GameObject objectSelected)
    {
        SelectableObject selectebleObject = objectSelected.GetComponent<SelectableObject>();

        if (selectebleObject.GetEmotionType() == mainEmotion) {
            SetAnswer(selectebleObject.GetEmotionType());
            facesSelected.Add(objectSelected);
        }
        else
        {
            SetAnswer(selectebleObject.GetEmotionType());
            roundResult = false;
            Invoke("EndRound", 0.5f);
        }
        if (facesSelected.Count == numberOfCorrectFaces)
        {
            roundResult = true;
            Invoke("EndRound", 0.5f);
        }           
    }

    void AssignFaceOfCorrectPeople()
    {
        numberOfCorrectFaces = Random.Range(1, 3);
        for (int i = 0; i < numberOfCorrectFaces; i++) {
           AssignFacePosition(mainEmotion);
        }

    }

    void AssignFaceOfIncorrectPeople()
    {
        int numberOfIncorrectFaces = 4 - numberOfCorrectFaces;
        for (int i = 0; i < numberOfIncorrectFaces; i++)
        {
            AssignFacePosition(PickNotMainEmotion(mainEmotion));
        }

    }

    //assegna un'emozione a una delle 4 posizioni disponibili
    void AssignFacePosition(Emotion emotion) {
        int randomPosition = Random.Range(0, facesCreated.Length);

        //finche' quella posizione e' gia' occupata cerca altri posti
        while (facesCreated[randomPosition] != null)
            randomPosition = Random.Range(0, facesCreated.Length);

        facesCreated[randomPosition] = emotion;
    }

    //metodo che istanzia una faccia delle 4 di persone random
    void InstantiateFace(Emotion? emotion, int position)
    {
       
        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/ImagePrefab/FacePrefab"), spawnPointPositions[position].transform.position, Quaternion.identity);
        
        face.GetComponent<SelectableObject>().SetEmotionType((Emotion)emotion);

        AssignFaceSprite(face.GetComponent<SpriteRenderer>(), face.GetComponent<SelectableObject>().GetEmotionType());
    }

    protected override void DestroySceneObjects()
    {
        facesCreated = new Emotion?[] { null, null, null, null };
        facesSelected.Clear();

        SelectableObject[] selectableObjects = GameObject.FindObjectsOfType<SelectableObject>();
        for (int i = 0; i < selectableObjects.Length; i++)
            Destroy(selectableObjects[i].gameObject);
    }

   

    protected override void EndRound()
    {
        endRoundPanel.SetActive(true);
        UIManager.EndRoundUI(roundResult);
    }

}
