using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CompositionManager : MinigameManager {

    public DroppableArea[] droppableArea;

    private UICompositionManager UIManager;
    private bool[] mouthPositioned = new bool[2];
    private bool[] eyesPositioned = new bool[2];
    readonly int maxPiecesNumber = 4;

    void Start() {
        UIManager = FindObjectOfType<UICompositionManager>();
        Hand.piecePositioned += CheckIfMinigameCompleted;
        StartNewGame();
    }

    public void StartNewGame() {
        //distruggi i vecchi pezzi 
        DestroySceneObjects();
        PickNewEmotion();
        SpawnSceneObjects();
        UIManager.UpdateUI(this);
        UIManager.StartGame();
    }

    protected override void DestroySceneObjects()
    {
        DraggableObject[] draggableObjects = FindObjectsOfType<DraggableObject>();
        foreach (DraggableObject d in draggableObjects)
            Destroy(d.gameObject);
    }

    protected override void CheckIfMinigameCompleted()
    {
        foreach (DroppableArea d in droppableArea) {
            if (!d.GetOccupied())
                return;
        }
        ShowEndingScreen();           
    }


    void ShowEndingScreen() {
        UIManager.EndGame();
        Invoke("StartNewGame", 3f);
    }

    protected override void SpawnSceneObjects()
    {
        SpawnCorrectPieces();
        FillOtherPieces();
    }

    void SpawnCorrectPieces() {
        //  SPAWN DELLA BOCCA
        int indexOfMouth = Random.Range(0, 2);
        GameObject mouth = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Mouth"), spawnPointPositions[indexOfMouth].localPosition, Quaternion.identity, GameObject.Find("BocchePanel").transform);
        //questo posto e' stato occupato
        mouthPositioned[indexOfMouth] = true;
        //assegna l'emozione scelta alla faccia
        mouth.GetComponent<DraggableFacePart>().SetFacePartEmotion(mainEmotion);

        //SPAWN DEGLI OCCHI
        int indexOfEyes = Random.Range(0, 2);
        GameObject eyes = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Eyes"), spawnPointPositions[indexOfEyes + 2].localPosition, Quaternion.identity, GameObject.Find("OcchiPanel").transform);
        //questo posto e' stato occupato
        eyesPositioned[indexOfEyes] = true;
        eyes.GetComponent<DraggableFacePart>().SetFacePartEmotion(mainEmotion);
    }

    //crea le altre bocche e occhi in modo che siano sbagliati
    void FillOtherPieces() {
        for (int i = 0; i < mouthPositioned.Length; i++)
        {
            if (!mouthPositioned[i]) {
                GameObject mouth = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Mouth"), spawnPointPositions[i].localPosition, Quaternion.identity, GameObject.Find("BocchePanel").transform);
                mouthPositioned[i] = true;
                mouth.GetComponent<DraggableFacePart>().SetFacePartEmotion(PickNotMainEmotion(mainEmotion));
            }
        }
        for (int i = 0; i < eyesPositioned.Length; i++)
        {
            if (!eyesPositioned[i])
            {
                GameObject eyes = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Eyes"), spawnPointPositions[i + 2].localPosition, Quaternion.identity, GameObject.Find("OcchiPanel").transform);
                eyesPositioned[i] = true;
                eyes.GetComponent<DraggableFacePart>().SetFacePartEmotion(PickNotMainEmotion(mainEmotion));
            }
        }
    }

    //metodo che ritorna un'emozione che non e' quella corretta in modo da assegnare un'emozione random agli altri pezzi sbagliati
    Emotion PickNotMainEmotion(Emotion main) {

        int randomEmotion = Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        Emotion chosenEmotion = (Emotion)randomEmotion;

        while (chosenEmotion == mainEmotion) {
            randomEmotion = Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
            chosenEmotion = (Emotion)randomEmotion;
        }

        return chosenEmotion;
    }


    public override string GetEmotionString() {
        return mainEmotion.ToString();
    }

    
}
