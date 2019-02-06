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
    List<GameObject> facesSelected;
    GameObject centralFace;

    UIWhichPersonIsManager UIManager;

    protected override void Start ()
    {
        base.Start ();
        //istanzia la mano per la selezione
        GameObject selectionHand = Resources.Load<GameObject> ("Prefab/HandSelection");
        Instantiate (selectionHand, Vector2.zero, Quaternion.identity);

        UIManager = FindObjectOfType<UIWhichPersonIsManager> ();
        facesSelected = new List<GameObject> ();
        SelectableObject.objectSelectedEvent += CheckIfGameCompleted;
        StartNewRound ();
    }

    public override void StartNewRound ()
    {
        DestroyAnswerObjectSpawned ();
        DestroySceneObjects ();
        //Disabilita schermata di fine round
        endRoundPanel.SetActive (false);
        //Aumentiamo il contatore dei round
        UpdateRound ();
        //scglie l'espressione della faccia principale
        PickNewEmotion ();
        //aggiorna la UI
        FindObjectOfType<UIWhichPersonIsManager> ().UpdateUI (this);

        SpawnSceneObjects ();
    }

    protected override void SpawnSceneObjects ()
    {
        //crea le faccia principale, quella dell'emozione corretta
        CreateMainFace ();
        //crea e 4 facce di altre persone
        CreateFacesOfDifferentPeople ();
    }

    void CreateMainFace ()
    {
        AvatarSettings a = gameSessionSettings.avatarSettings;
        centralFace = Instantiate (Resources.Load<GameObject> ("Prefab/AvatarFace"), facePosition.position, Quaternion.identity);
        centralFace.GetComponent<Avatar> ().CreateCompleteFace (GetMainEmotion (), a.gender, a.skinColor, a.hairStyle, a.hairColor, a.eyesColor);
    }

    void CreateFacesOfDifferentPeople ()
    {

        facesCreated = new Emotion?[4] { null, null, null, null };
        AssignFaceOfCorrectPeople ();
        AssignFaceOfIncorrectPeople ();
        StartCoroutine (CreateFaces ());
    }

    //coroutine che crea le 4 facce una ogni 0.2f secondi
    private IEnumerator CreateFaces ()
    {
        int i = 0;

        while (i < 4)
        {
            yield return new WaitForSeconds (0.25f);
            InstantiateFace (facesCreated[i], i);
            i++;
        }
    }

    //controllo sul fatto che hai selezionato tutte le facce corrette o hai sbagliato
    void CheckIfGameCompleted (GameObject objectSelected)
    {
        SelectableObject selectebleObject = objectSelected.GetComponent<SelectableObject> ();

        if (selectebleObject.GetEmotionType () == mainEmotion)
        {
            SetAnswer (selectebleObject.GetEmotionType ());
            facesSelected.Add (objectSelected);
        }
        else
        {
            SetAnswer (selectebleObject.GetEmotionType ());
            roundResult = false;
            UpdateResultDB();
            Invoke ("EndRound", 0.1f);
            
        }
        if (facesSelected.Count == numberOfCorrectFaces)
        {
            roundResult = true;
            UpdateResultDB();
            Invoke("EndRound", 0.1f);
        }
    }

    void AssignFaceOfCorrectPeople ()
    {
        numberOfCorrectFaces = Random.Range (1, 3);
        for (int i = 0; i < numberOfCorrectFaces; i++)
        {
            AssignFacePosition (mainEmotion);
        }

    }

    void AssignFaceOfIncorrectPeople ()
    {
        int numberOfIncorrectFaces = 4 - numberOfCorrectFaces;
        for (int i = 0; i < numberOfIncorrectFaces; i++)
        {
            AssignFacePosition (PickNotMainEmotion (mainEmotion));
        }

    }

    //assegna un'emozione a una delle 4 posizioni disponibili
    void AssignFacePosition (Emotion emotion)
    {
        int randomPosition = Random.Range (0, facesCreated.Length);

        //finche' quella posizione e' gia' occupata cerca altri posti
        while (facesCreated[randomPosition] != null)
            randomPosition = Random.Range (0, facesCreated.Length);

        facesCreated[randomPosition] = emotion;
    }

    //metodo che istanzia una faccia delle 4 di persone random
    void InstantiateFace (Emotion? emotion, int position)
    {
        //GameObject face = Instantiate (Resources.Load<GameObject> ("Prefab/ImagePrefab/FacePrefab"), spawnPointPositions[position].transform.position, Quaternion.identity);
        //AssignFaceSprite (face.GetComponent<SpriteRenderer> (), face.GetComponent<SelectableObject> ().GetEmotionType ());

        GameObject face = Instantiate (Resources.Load<GameObject> ("Prefab/AvatarFace"), spawnPointPositions[position].transform.position, Quaternion.identity);
        face.transform.localScale = new Vector3 (1, 1, 1);
        face.GetComponent<Avatar> ().CreateRandomFace ();
        face.GetComponent<Avatar> ().AssignEmotion ((Emotion) emotion);

        //aggiungi i component necessari
        face.AddComponent<SelectableObject> ();
        face.AddComponent<CircleCollider2D> ().radius = 0.5f;
        face.GetComponent<SelectableObject> ().SetEmotionType ((Emotion) emotion);
    }

    protected override void DestroySceneObjects ()
    {
        if (centralFace != null)
            Destroy (centralFace.gameObject);
        facesCreated = new Emotion?[] { null, null, null, null };
        facesSelected.Clear ();

        SelectableObject[] selectableObjects = GameObject.FindObjectsOfType<SelectableObject> ();
        for (int i = 0; i < selectableObjects.Length; i++)
            Destroy (selectableObjects[i].gameObject);
    }

    protected override void EndRound ()
    {
        endRoundPanel.SetActive (true);
        UIManager.EndRoundUI (roundResult);
    }

    private void OnDestroy ()
    {
        SelectableObject.objectSelectedEvent -= CheckIfGameCompleted;
    }

}
