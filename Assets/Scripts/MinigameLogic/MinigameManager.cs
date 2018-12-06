using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion { Felicità, Tristezza, Disgusto, Rabbia, Paura}
//public enum Emotion { Felicità, Tristezza, Rabbia}

public abstract class MinigameManager : MonoBehaviour {

    [Header("Punti dove saranno spawnati gli oggeti de spostare")]
    public Transform[] spawnPointPositions;
    [Header("Inserire il pannello di fine round")]
    public GameObject endRoundPanel;
    [Header("Numero di round totali")]
    public int roundsAmount;

    //Variabile statica che tiene conto della scelta del percoso o del minigioco singolo
    public static bool pathEnabled = false;

    protected Emotion mainEmotion;
    protected Emotion previousChosenEmotion;
    protected int currentRound = 0;
    protected bool roundResult;
    //public GameObject[] spawnableObjectsPrefab;

    // verosimilmente in ogni minigioco degli oggetti andranno spawnati 
    //ossia tutti quei pezzi tra cui scegliere ecc..
    protected abstract void SpawnSceneObjects();
    protected abstract void DestroySceneObjects();
    protected abstract void EndRound();
    //protected abstract void CheckIfMinigameCompleted();

    //Metodo per scegliere un'emozione tra quelle disponibili
    protected void PickNewEmotion()
    {
        //seleziona un'emozione a caso
        int randomEmotion = Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        mainEmotion = (Emotion)randomEmotion;
    }

    //metodo che ritorna un'emozione che non e' quella corretta in modo da assegnare un'emozione random agli altri pezzi sbagliati
    protected Emotion PickNotMainEmotion(Emotion main)
    {

        int randomEmotion = Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        Emotion chosenEmotion = (Emotion)randomEmotion;

        while (chosenEmotion == mainEmotion)
        {
            randomEmotion = Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
            chosenEmotion = (Emotion)randomEmotion;
        }

        return chosenEmotion;
    }

    //Metodo che aumenta il contatore dei round
    protected void UpdateRound()
    {
        currentRound += 1;
    }

    public string GetEmotionString()
    {
        return mainEmotion.ToString();
    }


}
