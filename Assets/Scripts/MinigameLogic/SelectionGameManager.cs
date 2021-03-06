using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectionGameManager : MinigameManager
{

    //Lista che viene utilizzare per tenere traccia delle emozioni già scelte per i SelectableObject
    private List<Emotion> emotionUsed;
    [SerializeField]
    private SelectableObject[] selectableObjects;
    private UIEndRoundManager UIManager;
    //Array che tiene conto di quali spawn points sono già stati occupati
    private bool[] occupiedPosition;

    private SelectableObject answerGiven;
    //Metodo utilizato per instanziare gli elementi di scena in base al minigame che si sta giocando
    protected abstract GameObject InstantiateEmotionElement(string emotionString, Vector3 position);
    //Metodo che prepara l'emozione centrale (da indovinare) in base al minigame che si sta giocando
    protected abstract void SetupCentralEmotion();

    protected override void Start()
    {
        base.Start();
        //istanzia la mano per la selezione
        GameObject selectionHand = Resources.Load<GameObject>("Prefab/HandSelection");
        Instantiate(selectionHand, Vector2.zero, Quaternion.identity);

        Debug.Log("Percorso attivo: " + pathEnabled);
        emotionUsed = new List<Emotion>();
        occupiedPosition = new bool[spawnPointPositions.Length];
        UIManager = FindObjectOfType<UIEndRoundManager>();
        SelectableObject.objectSelectedEvent += HandleSelection;
        StartNewRound();
    }

    //Metodo che gestisce la logica del singolo round
    public override void StartNewRound()
    {
        //Disabilita schermata di fine round
        endRoundPanel.SetActive(false);
        Debug.Log("Siamo al round #" + currentRound);
        //in ogni caso distruggi le risposte create nell'end round panel
        DestroyAnswerObjectSpawned();

        if (roundResult)
        {
            //Resettiamo la lista delle emozioni usate
            emotionUsed.Clear();
            //Facciamo visualizzare la main emotion al centro
            ChooseMainEmotion();
            //Spawn dei SelectableObjects in basso
            SpawnSceneObjects();
            //Ci salviamo tutti i selectable objects spawnati
            selectableObjects = FindObjectsOfType<SelectableObject>();
        }
        else
            RepeatRound();
    }

    //Metodo che sceglie l'emozione principale del round
    private void ChooseMainEmotion()
    {
        PickNewEmotion();
        emotionUsed.Add(mainEmotion);
        SetupCentralEmotion();
    }

    //Metodo che si occupa della logica di posizionamento dei principali elementi di scena
    protected override void SpawnSceneObjects()
    {
        SpawnCorrectElement();
        SpawnOtherElements();
    }

    //Metodo che si occupa di spawnare le facce sbagliate
    private void SpawnOtherElements()
    {
        for (int i = 0; i < spawnPointPositions.Length; i++)
        {
            if (occupiedPosition[i] == false)
            {
                Emotion e = PickNextEmotion();
                GameObject face = InstantiateEmotionElement(e.ToString(), spawnPointPositions[i].position);
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
    private void SpawnCorrectElement()
    {
        int positionIndex = UnityEngine.Random.Range(0, spawnPointPositions.Length);
        GameObject face = InstantiateEmotionElement(GetEmotionString(), spawnPointPositions[positionIndex].position);
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

    //Metodo che si occupa di distruggere tutti gli oggetti di scena prima del round successivo
    protected override void DestroySceneObjects()
    {
        foreach (SelectableObject s in selectableObjects)
        {
            if (s != null)
                Destroy(s.gameObject);
        }
    }

    //Metodo che viene chiamato nel momento in cui un oggetto viene selezionato
    protected virtual void HandleSelection(GameObject objectSelected)
    {
        answerGiven = objectSelected.GetComponent<SelectableObject>();
        foreach (SelectableObject s in selectableObjects)
        {
            if (s != null)
            {
                //Disabilitiamo i collider in modo tale da non triggerare più OnMouseOver ecc
                s.GetComponent<Collider2D>().enabled = false;
                //Se troviamo l'oggetto selezionato allora andiamo a settare la risposta al gioco
                if (s.GetEmotionType() == objectSelected.GetComponent<SelectableObject>().GetEmotionType())
                {
                    Debug.Log("E' stata selezionata " + s.GetEmotionType());
                    SetAnswer(s.GetEmotionType());
                }
            }
        }
        roundResult = CheckAnswer();
        UpdateResultDB();
        EndRound();
    }

    protected override void RepeatRound()
    {
        foreach (SelectableObject s in selectableObjects)
        {
            //Riabilitiamo i collider 
            s.GetComponent<Collider2D>().enabled = true;
            //disabilita la possibilita' di selezionare la risposta sbagliata
            answerGiven.DeactivateSelectableObject(100);
        }
    }

    //Metodo che gestisce la schermata di fine round
    protected override void EndRound()
    {
        //se non devo ripetere il round allora posso azzerare tutto e ricominciare
        if (roundResult)
        {
            DestroySceneObjects();
            UpdateRound();
        }
        endRoundPanel.SetActive(true);
        UIManager.EndRoundUI(roundResult);
    }

    private void OnDestroy()
    {
        SelectableObject.objectSelectedEvent -= HandleSelection;
    }

}
