using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessExpressionManager : MinigameManager {

    [Header("Inserisci UIText dell'emozione")]
    public Text emotionType;

    //Lista che viene utilizzare per tenere traccia delle emozioni già scelte per i SelectableObject
    private List<Emotion> emotionUsed;
    private UIGuessExpressionManager UIManager;
    private SelectableObject[] selectableObjects;
    //Array che tiene conto di quali spawn points sono già stati occupati
    private bool[] occupiedPosition;

    void Start()
    {
        emotionUsed = new List<Emotion>();
        occupiedPosition = new bool[spawnPointPositions.Length];
        UIManager = FindObjectOfType<UIGuessExpressionManager>();
        //Sottoscriversi all'evento che chiamerà CheckIfMinigameCompleted;
        StartNewRound();
    }

    //Metodo che gestisce la logica del singolo round
    private void StartNewRound()
    {
        //TODO: resettare occupiedPosition list
        //Resettiamo la lista delle emozioni usate
        emotionUsed.Clear();
        //Facciamo visualizzare la main emotion al centro
        ChooseMainEmotion();
        //Spawn dei SelectableObjects in basso
        SpawnSceneObjects();
    }

    //Metodo che sceglie l'emozione principale del round
    private void ChooseMainEmotion()
    {
        DestroySceneObjects();
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
                face.GetComponent<SelectableObject>().SetEmotionType(e);
                occupiedPosition[i] = true;
            }
        }

    }

    //Metodo che si occupa di spawnare la faccia corrispondente all'emozione del round corrente
    private void SpawnCorrectFace()
    {
        int positionIndex = UnityEngine.Random.Range(0, spawnPointPositions.Length);
        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/Faces/face" + GetEmotionString()), spawnPointPositions[positionIndex].position, Quaternion.identity);
        face.GetComponent<SelectableObject>().SetEmotionType(mainEmotion);
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
        SelectableObject[] draggableObjects = FindObjectsOfType<SelectableObject>();
        foreach (SelectableObject s in draggableObjects)
            Destroy(s.gameObject);
    }

    protected override void CheckIfMinigameCompleted()
    {
        throw new System.NotImplementedException();
    }


    public override string GetEmotionString()
    {
        return mainEmotion.ToString();
    }

}
