using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CompositionManager : MinigameManager {

    public DroppableArea[] droppableArea;
    public DraggableObject[] draggableObjects;

    private UICompositionManager UIManager;
    private bool[] mouthPositioned = new bool[2];
    private bool[] eyesPositioned = new bool[2];
    readonly int maxPiecesNumber = 4;

    //prende le emozioni scelte dall'utente e le immagazzina qui
    [SerializeField]
    protected Emotion eyesEmotionChosen;
    [SerializeField]
    protected Emotion mouthEmotionChosen;
   

    void Start() {
        UIManager = FindObjectOfType<UICompositionManager>();
        Hand.piecePositioned += CheckIfMinigameCompleted;
        answerObjectSpawned = new List<GameObject>();
        StartNewRound();
    }

    public override void StartNewRound() {      
        endRoundPanel.SetActive(false);
        DestroySceneObjects();
        PickNewEmotion();
        UIManager.UpdateUI(this);
        SpawnSceneObjects();
    }

    protected override void SpawnSceneObjects()
    {
        SpawnCorrectElement();
        SpawnOtherElements();
    }

    protected override void DestroySceneObjects()
    {
        DestroyAnswerObjectSpawned();

        //resetto gli array degli icchi e bocca
        mouthPositioned = new bool[]{ false, false };
        eyesPositioned = new bool[] { false, false };

        //distruggi tutti i pezzi della faccia
        DraggableObject[] draggableObjects = FindObjectsOfType<DraggableObject>();
        foreach (DraggableObject d in draggableObjects)
            Destroy(d.gameObject);

        //resetta le droppable area
        foreach (DroppableArea d in droppableArea) {
            d.SetOccupied(false);
            d.SetContainedPiece(null);
        }
    }

    protected void CheckIfMinigameCompleted()
    {
        for (int i = 0; i < droppableArea.Length; i++)
        {
            if (!droppableArea[i].GetOccupied())
                return;

            else
            {
                if (i == 0)
                    eyesEmotionChosen = droppableArea[i].GetContainedPiece().GetComponent<DraggableFacePart>().GetEmotion();
                else if (i == 1)
                    mouthEmotionChosen = droppableArea[i].GetContainedPiece().GetComponent<DraggableFacePart>().GetEmotion();
            }
        }

        roundResult = CheckAnswer();
        Invoke("DestroySceneObjects", 1f);
        Invoke("EndRound", 1f);
    }

    public override bool CheckAnswer()
    {
        if (eyesEmotionChosen != mainEmotion || mouthEmotionChosen != mainEmotion)
            return false;

        return true;
    }


    //Metodo che gestisce la schermata di fine round
    protected override void EndRound()
    {
        endRoundPanel.SetActive(true);
        UIManager.EndRoundUI(roundResult);
    }

    void SpawnCorrectElement() {
        //  SPAWN DELLA BOCCA
        int indexOfMouth = Random.Range(0, 2);
        GameObject mouth = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Mouth"), spawnPointPositions[indexOfMouth].localPosition, Quaternion.identity);
        //questo posto e' stato occupato
        mouthPositioned[indexOfMouth] = true;
        //assegna l'emozione scelta alla faccia
        mouth.GetComponent<DraggableFacePart>().SetFacePartEmotion(mainEmotion);

        //SPAWN DEGLI OCCHI
        int indexOfEyes = Random.Range(0, 2);
        GameObject eyes = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Eyes"), spawnPointPositions[indexOfEyes + 2].localPosition, Quaternion.identity);
        //questo posto e' stato occupato
        eyesPositioned[indexOfEyes] = true;
        eyes.GetComponent<DraggableFacePart>().SetFacePartEmotion(mainEmotion);
    }

    //crea le altre bocche e occhi in modo che siano sbagliati
    void SpawnOtherElements() {
        for (int i = 0; i < mouthPositioned.Length; i++)
        {
            if (!mouthPositioned[i]) {
                GameObject mouth = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Mouth"), spawnPointPositions[i].localPosition, Quaternion.identity);
                mouthPositioned[i] = true;
                mouth.GetComponent<DraggableFacePart>().SetFacePartEmotion(PickNotMainEmotion(mainEmotion));
            }
        }
        for (int i = 0; i < eyesPositioned.Length; i++)
        {
            if (!eyesPositioned[i])
            {
                GameObject eyes = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/Eyes"), spawnPointPositions[i + 2].localPosition, Quaternion.identity);
                eyesPositioned[i] = true;
                eyes.GetComponent<DraggableFacePart>().SetFacePartEmotion(PickNotMainEmotion(mainEmotion));
            }
        }
    }

    //ritorna i due pezzi di faccia che sono stati scelti
    public Emotion GetEyesEmotionChosen() {
        return eyesEmotionChosen;
    }

    //ritorna i due pezzi di faccia che sono stati scelti
    public Emotion GetMouthEmotionChosen()
    {
        return mouthEmotionChosen;
    }
}
