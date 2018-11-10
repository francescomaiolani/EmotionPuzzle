using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Minigame1Manager : MinigameManager {

    //ogni match avra' un'emozione da comporre, per evitare di essere ripetitivi si tiene traccia dell'ultima fatta
    protected Emotion mainEmotion;
    private Emotion previousChosenEmotion;

    Minigame1UIManager UIManager;

    void Start()
    {
        PickNewEmotion();
        SpawnDraggableObject();
        UIManager = FindObjectOfType<Minigame1UIManager>();
        UIManager.UpdateUI(this);
    }

    protected void PickNewEmotion() {
        //seleziona un'emozione a caso
        int randomEmotion = Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        mainEmotion = (Emotion)randomEmotion;

    }
    protected override void CheckIfMinigameCompleted()
    {
        
    }

    protected override void SpawnDraggableObject()
    {
        
    }

    public override string GetEmotionString() {
        return mainEmotion.ToString();
    }

    
}
