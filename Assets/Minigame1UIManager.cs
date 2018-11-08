using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame1UIManager : MonoBehaviour {

    public Image advicePanel;
    public Text adviceText;

    private void Start()
    {
        Hand.adviceGiven += GiveAdvice;
    }

    void GiveAdvice(string advice) {
        adviceText.text = advice;
        advicePanel.gameObject.SetActive(true);

        Invoke("DisableAdvicePanel", 2f);
    }

    void DisableAdvicePanel() {
        advicePanel.gameObject.SetActive(false);
    }
}

