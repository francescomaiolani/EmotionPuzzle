﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion { Felicità, Tristezza, Disgusto, Rabbia, Paura }

public abstract class MinigameManager : MonoBehaviour
{

    [Header ("true se siamo in magicRoom")]
    public bool inMagicRoom;
    [Header ("Punti dove saranno spawnati gli oggeti de spostare")]
    public Transform[] spawnPointPositions;
    [Header ("Inserire il pannello di fine round")]
    public GameObject endRoundPanel;
    [Header ("Numero di round totali")]
    public int totalRounds;

    //Variabile statica che tiene conto della scelta del percoso o del minigioco singolo
    public static bool pathEnabled = false;

    [SerializeField]
    protected Emotion mainEmotion;
    protected Emotion previousChosenEmotion;
    //Risposta scelta dall'utente
    protected Emotion emotionAnswer;
    protected int currentRound = 0;
    protected bool roundResult;
    //public GameObject[] spawnableObjectsPrefab;
    //Lista che tiene i game object degli oggetti risposta creati nell'endGame e che vengono cancellate all'inizio di un nuovo round
    public List<GameObject> answerObjectSpawned;

    // verosimilmente in ogni minigioco degli oggetti andranno spawnati 
    //ossia tutti quei pezzi tra cui scegliere ecc..
    protected abstract void SpawnSceneObjects ();
    protected abstract void DestroySceneObjects ();
    public abstract void StartNewRound ();
    protected abstract void EndRound ();
    //protected abstract void CheckIfMinigameCompleted();

    //Metodo per scegliere un'emozione tra quelle disponibili
    protected void PickNewEmotion ()
    {
        //seleziona un'emozione a caso
        int randomEmotion = Random.Range (0, System.Enum.GetNames (typeof (Emotion)).Length);
        mainEmotion = (Emotion) randomEmotion;
    }

    //metodo che ritorna un'emozione che non e' quella corretta in modo da assegnare un'emozione random agli altri pezzi sbagliati
    protected Emotion PickNotMainEmotion (Emotion main)
    {
        int randomEmotion = Random.Range (0, System.Enum.GetNames (typeof (Emotion)).Length);
        Emotion chosenEmotion = (Emotion) randomEmotion;

        while (chosenEmotion == mainEmotion)
        {
            randomEmotion = Random.Range (0, System.Enum.GetNames (typeof (Emotion)).Length);
            chosenEmotion = (Emotion) randomEmotion;
        }

        return chosenEmotion;
    }

    //Metodo che aumenta il contatore dei round
    protected void UpdateRound ()
    {
        currentRound += 1;
    }

    //Metodo che si occupa di controllare l'esito della selezione di una risposta
    public virtual bool CheckAnswer ()
    {
        if (emotionAnswer == mainEmotion)
            return true;
        else
            return false;
    }

    // metodo che distrugge tutte le istanze delle risposte create. Da invocare ad ogni start round
    protected void DestroyAnswerObjectSpawned ()
    {
        if (answerObjectSpawned.Count != 0)
        {
            for (int i = 0; i < answerObjectSpawned.Count; i++)
            {
                Destroy (answerObjectSpawned[i]);
            }
            answerObjectSpawned.Clear ();
        }
    }

    //Metodo che salva la risposta data dall'utente
    public void SetAnswer (Emotion e)
    {
        emotionAnswer = e;
    }

    public string GetEmotionString ()
    {
        return mainEmotion.ToString ();
    }

    public Emotion GetMainEmotion ()
    {
        return mainEmotion;
    }

    public string GetEmotionAnswerString ()
    {
        return emotionAnswer.ToString ();
    }

    public Emotion GetEmotionAnswer ()
    {
        return emotionAnswer;
    }

    public int GetCurrentRound ()
    {
        return currentRound;
    }

    public int GetTotalRounds ()
    {
        return totalRounds;
    }

    public bool IsPathEnabled ()
    {
        return pathEnabled;
    }

    //il font non ha la a con l'accento quindi converto con l'apostrofo
    public string ConvertInCorrectText (string emotion)
    {
        if (emotion == "Felicità")
            return "Felicita'";
        else
            return emotion;
    }

    public static Emotion ConvertTextInEmotion (string emotionText)
    {
        if (emotionText == "Felicità")
            return Emotion.Felicità;
        else if (emotionText == "Tristezza")
            return Emotion.Tristezza;
        else if (emotionText == "Rabbia")
            return Emotion.Rabbia;
        else if (emotionText == "Disgusto")
            return Emotion.Disgusto;
        else if (emotionText == "Paura")
            return Emotion.Paura;
        else
            return Emotion.Felicità;
    }
}
