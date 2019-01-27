using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diagram : MonoBehaviour {

    //Inserisci quanti errori sono stati fatti per emozione
    public int[] errorValues;

    public GameObject[] rects;

    public float maxSize;

    private int totalErrors;

    void Start()
    {
        totalErrors = GetTotalErrors();
        SetRectValues();
        DatabaseManager.GetTotalErrorsByEmotion("Felicita");
    }

    private void SetRectValues()
    {
        float perc;
        float value;

        if (errorValues.Length != rects.Length)
            Debug.LogError("Numero differente tra rettangoli e valori");
        for (int i = 0; i < errorValues.Length; i++)
        {
            value = (errorValues[i] * maxSize) / totalErrors;
            rects[i].transform.localScale = new Vector3(rects[i].transform.localScale.x, value, rects[i].transform.localScale.z);
        }
    }

    private int GetTotalErrors()
    {
        int sum = 0;
        foreach(int v in errorValues)
        {
            sum += v;
        }
        return sum;
    }
}
