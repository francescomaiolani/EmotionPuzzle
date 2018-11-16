using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessExpressionManager : MinigameManager {

    UIGuessExpressionManager UIManager;

    void Start()
    {
        UIManager = FindObjectOfType<UIGuessExpressionManager>();
        //Sottoscriversi all'evento che chiamerà CheckIfMinigameCompleted;
        StartNewRound();
    }

    private void StartNewRound()
    {

    }

    public override string GetEmotionString()
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckIfMinigameCompleted()
    {
        throw new System.NotImplementedException();
    }

    protected override void SpawnSceneObjects()
    {
        throw new System.NotImplementedException();
    }

    protected override void DestroySceneObjects()
    {
        throw new NotImplementedException();
    }
}
