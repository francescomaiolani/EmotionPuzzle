using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessExpressionManager : MinigameManager {

    [Header("Inserisci UIText dell'emozione")]
    public Text emotionType;
    [Header("Posizione nella quale si posizione la facce dopo essere stata scelta")]
    public Transform centralPosition;

    //Lista che viene utilizzare per tenere traccia delle emozioni già scelte per i SelectableObject
    private List<Emotion> emotionUsed;
    private UIGuessExpressionManager UIManager;
    private SelectableObject[] selectableObjects;
    //Array che tiene conto di quali spawn points sono già stati occupati
    private bool[] occupiedPosition;
    //Risposta scelta dall'utente
    private Emotion emotionAnswer;

    void Start()
    {
        emotionUsed = new List<Emotion>();
        occupiedPosition = new bool[spawnPointPositions.Length];
        UIManager = FindObjectOfType<UIGuessExpressionManager>();
        SelectableObject.objectSelectedEvent += HandleSelection;
        StartNewRound();
    }

    //Metodo che gestisce la logica del singolo round
    private void StartNewRound()
    {
        //Resettiamo la lista delle emozioni usate
        emotionUsed.Clear();
        //Distruggiamo tutti gli oggetti utilizzati nel round precedente
        if (selectableObjects != null)
            DestroySceneObjects();
        //Facciamo visualizzare la main emotion al centro
        ChooseMainEmotion();
        //Spawn dei SelectableObjects in basso
        SpawnSceneObjects();
        //Ci salviamo tutti i selectable objects spawnati
        selectableObjects = FindObjectsOfType<SelectableObject>();
    }

    //Metodo che sceglie l'emozione principale del round
    private void ChooseMainEmotion()
    {
        PickNewEmotion();
        emotionUsed.Add(mainEmotion);
        emotionType.text = GetEmotionString().ToUpper();
    }

    //Metodo che si occupa della logica di posizionamento dei principali elementi di scena
    protected override void SpawnSceneObjects()
    {
        SpawnCorrectFace();
        SpawnOtherFaces();
    }

    //Metodo che si occupa di spawnare le facce sbagliate
    private void SpawnOtherFaces()
    {
        for(int i = 0; i < spawnPointPositions.Length; i++)
        {
            if (occupiedPosition[i] == false)
            {
                Emotion e = PickNextEmotion();
                GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/Faces/face" + e.ToString()), spawnPointPositions[i].position, Quaternion.identity);
                SelectableObject so = face.GetComponent<SelectableObject>();
                so.SetEmotionType(e);
                occupiedPosition[i] = true;
            }
        }
        //Resettiamo subito dopo le posizioni a false in modo tale da trovarcele pronto per il round successivo
        for (int i = 0; i < spawnPointPositions.Length; i++)
            occupiedPosition[i] = false;

    }

    //Metodo che si occupa di spawnare la faccia corrispondente all'emozione del round corrente
    private void SpawnCorrectFace()
    {
        int positionIndex = UnityEngine.Random.Range(0, spawnPointPositions.Length);
        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/Faces/face" + GetEmotionString()), spawnPointPositions[positionIndex].position, Quaternion.identity);
        SelectableObject so = face.GetComponent<SelectableObject>();
        so.SetEmotionType(mainEmotion);
        occupiedPosition[positionIndex] = true;
    }

    //Metodo che si occupa di scegliere una nuova emozione differente da quelle già scelte
    private Emotion PickNextEmotion()
    {
        int randomEmotion = UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        Emotion chosenEmotion = (Emotion) randomEmotion;

        while (emotionUsed.Contains(chosenEmotion))
        {
            randomEmotion = UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
            chosenEmotion = (Emotion) randomEmotion;
        }

        emotionUsed.Add(chosenEmotion);

        return chosenEmotion;
    }

    protected override void DestroySceneObjects()
    {
        foreach (SelectableObject s in selectableObjects)
        {
            if (s != null)
                Destroy(s.gameObject);
        }
    }

    private void HandleSelection(GameObject objectSelected)
    {

        foreach (SelectableObject s in selectableObjects)
        {
            if (s != null)
            {
                //Disabilitiamo i collider in modo tale da non triggerare più OnMouseOver ecc
                s.GetComponent<Collider2D>().enabled = false;
                //Se troviamo l'oggetto selezionato allora andiamo a settare la risposta al gioco
                if (s.name == objectSelected.name)
                {
                    Debug.Log("E' stata selezionata " + s.GetEmotionType());
                    //Spostiamo la faccia al centro
                    s.transform.position = Vector3.Lerp(transform.position, centralPosition.position, 1.0f);
                    SetAnswer(s.GetEmotionType());
                }
            }
        }
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if (emotionAnswer == mainEmotion)
            Debug.Log("Hai vinto");
        else
            Debug.Log("Hai perso");
        StartNewRound();
    }

    public void SetAnswer(Emotion e)
    {
        emotionAnswer = e;
    }

    public override string GetEmotionString()
    {
        return mainEmotion.ToString();
    }

    private void OnDestroy()
    {
        SelectableObject.objectSelectedEvent -= HandleSelection;
    }

}
