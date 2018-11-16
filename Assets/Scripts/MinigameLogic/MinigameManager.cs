using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Emotion { Felicità, Tristezza, Disgusto, Rabbia, Neutra, Paura, Sorpresa}
public enum Emotion { Felicità, Tristezza, Rabbia}

public abstract class MinigameManager : MonoBehaviour {

    [Header("Punti dove saranno spawnati gli oggeti de spostare")]
    public Transform[] spawnPointPositions;

    protected Emotion mainEmotion;
    protected Emotion previousChosenEmotion;
    //public GameObject[] spawnableObjectsPrefab;

    // verosimilmente in ogni minigioco degli oggetti andranno spawnati 
    //ossia tutti quei pezzi tra cui scegliere ecc..
    protected abstract void SpawnSceneObjects();
    protected abstract void DestroySceneObjects();
    protected abstract void CheckIfMinigameCompleted();
    public abstract string GetEmotionString();

    //Metodo per scegliere un'emozione tra quelle disponibili
    protected void PickNewEmotion()
    {
        //seleziona un'emozione a caso
        int randomEmotion = Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        mainEmotion = (Emotion)randomEmotion;
    }

}
