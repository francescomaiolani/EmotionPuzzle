using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Games { GuessExpression, HowDoYouFeel, WhichPersonIs, Composition, PhotographicEmotion }

public class Diagram : MonoBehaviour
{

    public InputField username;
    public GameObject type;
    public GameObject canvas;
    public float maxScale;

    private List<GameObject> rects;
    private string targetName;
    private string targetType;
    private float errorPercentage;

    [SerializeField]
    private Color[] colori = new Color[3];

    private void Start()
    {
        rects = new List<GameObject>();
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
        if (rects.Count != 0)
            foreach (GameObject r in rects)
                Destroy(r);

        SetType();

        if (CheckUsername())
        {

            string[] targets;
            if (targetType == "Emozione")
                targets = System.Enum.GetNames(typeof(Emotion));
            else
                targets = System.Enum.GetNames(typeof(Games));

            int i;
            float offset;
            for (i = 0, offset = 0; i < targets.Length; i++, offset += 2.5f)
            {
                int totalRounds, totalErrors;
                targets[i] = ConvertInDatabaseFormat(targets[i]);

                if (targetType == "Emozione")
                {
                    totalRounds = DatabaseManager.GetTotalRoundsByEmotion(targetName, targets[i]);
                    totalErrors = DatabaseManager.GetTotalErrorsByEmotion(targetName, targets[i]);
                }
                else
                {
                    totalRounds = DatabaseManager.GetTotalRoundsByGame(targetName, targets[i]);
                    totalErrors = DatabaseManager.GetTotalErrorsByGame(targetName, targets[i]);
                }

                GameObject rect = Instantiate(Resources.Load<GameObject>("Prefab/DataVisualization/DiagramRect"), new Vector3(-4.5f + offset, -3.2f, 0), Quaternion.identity, canvas.transform);
                rect.GetComponentInChildren<Image>().color = colori[2];
                rects.Add(rect);

                Debug.Log("Errori " + targets[i] + ":" + totalErrors + " Round " + targets[i] + ": " + totalRounds);

                if (totalRounds == 0)
                {
                    errorPercentage = 0;
                }
                else
                {
                    errorPercentage = ((float) totalErrors / totalRounds) * 100;
                }
                Vector3 newScale = new Vector3(rect.transform.localScale.x, (errorPercentage * maxScale) / 100, rect.transform.localScale.z);
                rect.transform.Find("RectImage").transform.localScale = newScale;
                targets[i] = ConvertInDiagramFormat(targets[i]);

                rect.transform.Find("DataText").GetComponent<Text>().text = targets[i] + "\n" + errorPercentage + "%";
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

    private string ConvertInDatabaseFormat(string s)
    {
        if (s == "Felicità")
            s = "Felicita";
        if (s == "1. Indovina l'emozione")
            s = "GuessExpression";
        if (s == "2. Come ti senti?")
            s = "HowDoYouFeel";
        if (s == "3. Quale persona è...")
            s = "WhichPersonIs";
        if (s == "4. Componi la faccia")
            s = "Composition";
        if (s == "5. Emozione Fotografica")
            s = "PhotographicEmotion";
        return s;
    }

    private string ConvertInDiagramFormat(string s)
    {
        if (s == "Felicita")
            s = "Felicità";
        if (s == "GuessExpression")
            s = "1. Indovina l'emozione";
        if (s == "HowDoYouFeel")
            s = "2. Come ti senti?";
        if (s == "WhichPersonIs")
            s = "3. Quale persona è...";
        if (s == "Composition")
            s = "4. Componi la faccia";
        if (s == "PhotographicEmotion")
            s = "5. Emozione Fotografica";
        return s;
    }
}
