using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Games { GuessExpression, HowDoYouFeel, WhichPersonIs, Composition, PhotographicEmotion }

public class Diagram : MonoBehaviour {

    public InputField username;
    public GameObject type;

    private string targetName;
    private string targetType;
    private int totalErrors;
    private int targetErrors;

    private void Start()
    {
        
    }

    private void SetType()
    {
        if (type.transform.Find("Label").GetComponent<Text>().text == "Emozione")
            targetType = "Emozione";
        if (type.transform.Find("Label").GetComponent<Text>().text == "Gioco")
            targetType = "Gioco";
    }

    //METODO CHE VIENE CHIAMATO QUANDO VIENE PREMUTO IL TASTO MOSTRA RISULTATI
    public void SetDiagram()
    {
        SetType();
        if (CheckUsername())
        {
            totalErrors = DatabaseManager.GetTotalErrors(targetName);
            Debug.Log("Gli errori totali del soggetto sono: " + totalErrors);
            if (targetType == "Emozione")
            {
                string[] emotions = System.Enum.GetNames(typeof(Emotion));
                for (int i = 0; i < emotions.Length; i++)
                {
                    if (emotions[i] == "Felicità")
                        emotions[i] = "Felicita";
                    Debug.Log("Gli errori per " + emotions[i] + " sono:" + DatabaseManager.GetTotalErrorsByEmotion(targetName, emotions[i]));
                }
            }
            
            if (targetType == "Gioco")
            {
                string[] games = System.Enum.GetNames(typeof(Games));
                for (int i = 0; i < games.Length; i++)
                {
                    if (games[i] == "1. Indovina l'emozione")
                        games[i] = "GuessExpression";
                    if (games[i] == "2. Come ti senti?")
                        games[i] = "HowDoYouFeel";
                    if (games[i] == "3. Quale persona è...")
                        games[i] = "WhichPersonIs";
                    if (games[i] == "4. Componi la faccia")
                        games[i] = "Composition";
                    if (games[i] == "5. Emozione Fotografica")
                        games[i] = "PhotographicEmotion";
                    Debug.Log("Gli errori per " + games[i] + " sono:" + DatabaseManager.GetTotalErrorsByGame(targetName, games[i]));
                }
            }
        }

        
    }

    private bool CheckUsername()
    {
        targetName = username.transform.Find("Text").GetComponent<Text>().text;
        if (DatabaseManager.CheckIfAlreadyExistInResult(targetName))
        {
            return true;
        }
        else
        {
            return false;
            //Errore
        }
    }

    //    //Inserisci quanti errori sono stati fatti per emozione
    //    public int[] errorValues;

    //    public GameObject[] rects;

    //    public float maxSize;

    //    private int totalErrors;

    //    void Start()
    //    {
    //        totalErrors = GetTotalErrors();
    //        SetRectValues();
    //        DatabaseManager.GetTotalErrorsByEmotion("Felicita");
    //    }

    //    private void SetRectValues()
    //    {
    //        float perc;
    //        float value;

    //        if (errorValues.Length != rects.Length)
    //            Debug.LogError("Numero differente tra rettangoli e valori");
    //        for (int i = 0; i < errorValues.Length; i++)
    //        {
    //            value = (errorValues[i] * maxSize) / totalErrors;
    //            rects[i].transform.localScale = new Vector3(rects[i].transform.localScale.x, value, rects[i].transform.localScale.z);
    //        }
    //    }

    //    private int GetTotalErrors()
    //    {
    //        int sum = 0;
    //        foreach(int v in errorValues)
    //        {
    //            sum += v;
    //        }
    //        return sum;
    //    }
}
