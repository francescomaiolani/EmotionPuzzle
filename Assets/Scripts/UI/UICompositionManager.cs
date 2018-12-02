﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICompositionManager : MonoBehaviour {

    public Image advicePanel;
    public Text adviceText;

    public Text emotionText;

    Dictionary<Emotion, string> fumettoPhrase = new Dictionary<Emotion, string>();

    public Animator endGameAnimator;

    private void Start()
    {
        Hand.adviceGiven += GiveAdvice;
        PopulateDictionary();
    }

    public void EndGame() {
        endGameAnimator.SetTrigger("EndGame");
    }

    public void StartGame() {
        endGameAnimator.SetTrigger("StartGame");
    }

    void PopulateDictionary() {
        fumettoPhrase.Add(Emotion.Felicità, "Quando sono felice mangio un bel gelato");
        fumettoPhrase.Add(Emotion.Tristezza, "Sono triste quando qualcuno si fa male");
        fumettoPhrase.Add(Emotion.Rabbia, "Sono arrabbiato quando qualcuno non vuole giocare con me");
    }

    void GiveAdvice(string advice) {
        adviceText.text = advice;
        advicePanel.gameObject.SetActive(true);
        advicePanel.GetComponent<Animator>().Play("Advice");
    }

    void DisableAdvicePanel() {
        advicePanel.gameObject.SetActive(false);
    }

    public void UpdateUI(MinigameManager manager) {
        emotionText.text = manager.GetEmotionString().ToUpper();
        //fumettoText.text = fumettoPhrase.Values
    }

}
