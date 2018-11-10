using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Emotion { Felicità, Tristezza, Disgusto, Rabbia, Neutra, Paura, Sorpresa}
public enum Emotion { Felicità, Tristezza, Rabbia}

public abstract class MinigameManager : MonoBehaviour {

    [Header("Punti dove saranno spawnati gli oggeti de spostare")]
    public Transform[] spawnPointPositions;

    //public GameObject[] spawnableObjectsPrefab;

    // verosimilmente in ogni minigioco degli oggetti andranno spawnati 
    //ossia tutti quei pezzi tra cui scegliere ecc..
    protected abstract void SpawnDraggableObject();
    protected abstract void CheckIfMinigameCompleted();
    public abstract string GetEmotionString();
}
