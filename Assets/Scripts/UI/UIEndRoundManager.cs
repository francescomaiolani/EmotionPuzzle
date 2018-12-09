﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UIEndRoundManager : MonoBehaviour {

    public Image endRoundPanel;
    public Image[] imagesQA;
    public Text[] sentencesQA;
    public TextMeshProUGUI resultSentence;
    public MinigameManager gameManager;
    public GameObject cannons;

    protected abstract void SetQA(bool roundResult);

    public void EndRoundUI(bool roundResult)
    {
        ActivateCannon(false);

        if (roundResult)
        {
            resultSentence.text = "<color=#FFC132>H</color><color=#FF9138>a</color>i<color=#F16B68> v</color>" +
                "<color=#FFC132>i</color>nt<color=#FF9138>o</color>!<color=#F16B68>!</color>";
            endRoundPanel.color = new Color32(200,246,88,255);
            ActivateCannon(roundResult);
        }
        else {
            resultSentence.text = "<color=#FFC132>H</color><color=#FF9138>a</color>i<color=#F16B68> p</color>" +
               "<color=#FFC132>e</color>rs<color=#FF9138>o</color>!<color=#F16B68>!</color>";
            endRoundPanel.color = new Color32(241, 107, 104, 255);
        }
           
        SetQA(roundResult);
    }


    void ActivateCannon(bool active) {
        cannons.SetActive(active);
    }

    //metodo che spawna una faccia nel gioco sopra la UI, con un certo scaling e emozioni per occhi e bocca potenzialmente diverse
    protected void SpawnFace(Vector3 position, Emotion eyesEmotion, Emotion mouthEmotion, bool correct, float scaling)
    {
        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/ImagePrefab/FaceOverUI"), position, Quaternion.identity);
        face.transform.localScale = new Vector3(scaling, scaling, 0);
        face.transform.Find("Eyes").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/FacePieces/Eyes/Eyes" +  eyesEmotion.ToString() );
        face.transform.Find("Mouth").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/FacePieces/Mouth/Mouth" + mouthEmotion.ToString());
        if (correct)
            face.transform.Find("Correct").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/UI/OkIcon");

        else
        {
            face.transform.Find("Correct").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/UI/NotOkIcon");
            ChangeOpacity(face);
        }

        gameManager.answerObjectSpawned.Add(face);
    }

    //metodo per diminuire l'opacita' di una risposta sbagliata
    void ChangeOpacity(GameObject face)
    {
        face.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
        face.transform.Find("Eyes").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
        face.transform.Find("Mouth").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
    }

    //metodo che converte una stringa in multicolore
    public string ChangeTextToRandomColors(string text)
    {
        int currentColor;
        int previousColor = 0;
        string coloredString = "";
        string currentColorCode = "";

        for (int i = 0; i < text.Length; i++) {
            currentColor = UnityEngine.Random.Range(0,4);
            while (currentColor == previousColor)
                currentColor = UnityEngine.Random.Range(0, 4);

            switch (currentColor) {
                case 0:
                    currentColorCode = "<color=#FFC132>";
                    break;
                case 1:
                    currentColorCode = "<color=#F16B68>";
                    break;
                case 2:
                    currentColorCode = "<color=#FF9138>";
                    break;
                case 3:
                    currentColorCode = "<color=#50C1C7>";
                    break;
            }
            coloredString += currentColorCode + text[i] + "</color>";
        }

        return coloredString;
    
    }

}
