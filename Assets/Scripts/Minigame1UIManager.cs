using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame1UIManager : MonoBehaviour {

    public Image advicePanel;
    public Text adviceText;

    public Text emotionText;

    private void Start()
    {
        Hand.adviceGiven += GiveAdvice;
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
        emotionText.text = manager.GetEmotionString();
    }

}

