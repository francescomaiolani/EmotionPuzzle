using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichPersonIsManager : MinigameManager
{

    //array che viene riempito man mano che si creano le facce delle varie persone, Serve a sapere quali posizioni sono state occupate
    Emotion?[] facesCreated;
    //numero di facce corrette create
    int numberOfCorrectFaces;
    public Transform facePosition;
    //risposte date 
    List<SelectableObject> facesSelected;
    GameObject centralFace;

    private SelectableObject[] selectableObjects;

    UIWhichPersonIsManager UIManager;

    protected override void Start()
    {
        base.Start();
        //istanzia la mano per la selezione
        GameObject selectionHand = Resources.Load<GameObject>("Prefab/HandSelection");
        Instantiate(selectionHand, Vector2.zero, Quaternion.identity);

        UIManager = FindObjectOfType<UIWhichPersonIsManager>();
        facesSelected = new List<SelectableObject>();
        SelectableObject.objectSelectedEvent += CheckIfGameCompleted;
        StartNewRound();
    }

    public override void StartNewRound()
    {
        DestroyAnswerObjectSpawned();
        //Disabilita schermata di fine round
        endRoundPanel.SetActive(false);
        //aggiorna la UI
        FindObjectOfType<UIWhichPersonIsManager>().UpdateUI(this);
        if (roundResult)
        {
            DestroySceneObjects();
            //scglie l'espressione della faccia principale
            PickNewEmotion();
            SpawnSceneObjects();
        }
        else
            RepeatRound();
    }

    protected override void RepeatRound()
    {
        foreach (SelectableObject s in facesSelected)
        {
            if (s.GetEmotionType() != mainEmotion)
                s.DeactivateSelectableObject();
        }
        //togli le risposte precedentemente date
        facesSelected.Clear();
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
        AvatarSettings a = gameSessionSettings.avatarSettings;
        centralFace = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFace"), facePosition.position, Quaternion.identity);
        centralFace.GetComponent<Avatar>().CreateCompleteFace(GetMainEmotion(), a.gender, a.skinColor, a.hairStyle, a.hairColor, a.eyesColor);
    }

    void CreateFacesOfDifferentPeople()
    {
        selectableObjects = new SelectableObject[4];
        facesCreated = new Emotion?[4] { null, null, null, null };
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
            SelectableObject face = InstantiateFace(facesCreated[i], i);
            selectableObjects[i] = face;
            i++;
        }
    }

    //controllo sul fatto che hai selezionato tutte le facce corrette o hai sbagliato
    void CheckIfGameCompleted(GameObject objectSelected)
    {
        SelectableObject selectableObject = objectSelected.GetComponent<SelectableObject>();
        facesSelected.Add(selectableObject);
        SetAnswer(selectableObject.GetEmotionType());

        //se la risposta e' cannata ripeti il round senza altre storie
        if (selectableObject.GetEmotionType() != mainEmotion)
        {
            roundResult = false;
            UpdateResultDB();
            Invoke("EndRound", 0.1f);
        }
        //altrimenti controlla le precedenti risposte date
        else
        {
            int correct = 0;
            //per ogni risposta data
            foreach (SelectableObject s in facesSelected)
            {
                //se l'emozione e' giusta allora segnalo
                if (s.GetEmotionType() == mainEmotion)
                    correct++;

                //se ho tante emozioni giuste quante quelle date
                if (correct == numberOfCorrectFaces)
                {
                    roundResult = true;
                    UpdateResultDB();
                    Invoke("EndRound", 0.1f);
                }
            }
        }
    }

    void AssignFaceOfCorrectPeople()
    {
        numberOfCorrectFaces = Random.Range(1, 3);
        for (int i = 0; i < numberOfCorrectFaces; i++)
        {
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
    void AssignFacePosition(Emotion emotion)
    {
        int randomPosition = Random.Range(0, facesCreated.Length);

        //finche' quella posizione e' gia' occupata cerca altri posti
        while (facesCreated[randomPosition] != null)
            randomPosition = Random.Range(0, facesCreated.Length);

        facesCreated[randomPosition] = emotion;
    }

    //metodo che istanzia una faccia delle 4 di persone random
    SelectableObject InstantiateFace(Emotion? emotion, int position)
    {
        //GameObject face = Instantiate (Resources.Load<GameObject> ("Prefab/ImagePrefab/FacePrefab"), spawnPointPositions[position].transform.position, Quaternion.identity);
        //AssignFaceSprite (face.GetComponent<SpriteRenderer> (), face.GetComponent<SelectableObject> ().GetEmotionType ());

        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFace"), spawnPointPositions[position].transform.position, Quaternion.identity);
        face.transform.localScale = new Vector3(1, 1, 1);
        face.GetComponent<Avatar>().CreateRandomFace();
        face.GetComponent<Avatar>().AssignEmotion((Emotion) emotion);

        //aggiungi i component necessari
        face.AddComponent<SelectableObject>();
        face.AddComponent<CircleCollider2D>().radius = 0.5f;
        face.GetComponent<SelectableObject>().SetEmotionType((Emotion) emotion);
        return face.GetComponent<SelectableObject>();
    }

    protected override void DestroySceneObjects()
    {
        if (centralFace != null)
            Destroy(centralFace.gameObject);
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
        if (roundResult)
            UpdateRound();
    }

    private void OnDestroy()
    {
        SelectableObject.objectSelectedEvent -= CheckIfGameCompleted;
    }

}
