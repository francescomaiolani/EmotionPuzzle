using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public abstract class UIEndRoundManager : MonoBehaviour
{
    public Image endRoundPanel;
    public Image[] imagesQA;
    public Text[] sentencesQA;
    public TextMeshProUGUI resultSentence;
    public MinigameManager gameManager;
    public GameObject cannons;

    private bool inMagicRoom;

    protected virtual void Start()
    {
        if (MagicRoomKinectV2Manager.instance != null)
            inMagicRoom = true;
        else
            inMagicRoom = false;
    }

    protected abstract void SetQA(bool roundResult);

    public void EndRoundUI(bool roundResult)
    {
        ActivateCannon(false);

        Color c;
        if (roundResult)
        {
            c = Color.green;
            c.a = 100 / 255f;
            if (inMagicRoom)
                MagicRoomLightManager.instance.sendColour(c);

            resultSentence.text = ChangeTextToRandomColors("Corretto!!");
            endRoundPanel.color = new Color32(200, 246, 88, 255);
            ActivateCannon(roundResult);
        }
        else
        {
            c = Color.red;
            c.a = 100 / 255f;
            if (inMagicRoom)
                MagicRoomLightManager.instance.sendColour(c);

            resultSentence.text = ChangeTextToRandomColors("Riprova");
            endRoundPanel.color = new Color32(241, 107, 104, 255);
        }

        SetQA(roundResult);
    }

    void ActivateCannon(bool active)
    {
        cannons.SetActive(active);
    }

    protected void SpawnFace(Vector3 position, Emotion eyesEmotion, Emotion mouthEmotion, bool correct, float scaling, bool iconNeeded)
    {
        GameSessionSettings gameSessionSettings = gameManager.gameSessionSettings;
        GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFaceUI"), position, Quaternion.identity);
        face.transform.localScale = new Vector3(scaling, scaling);
        face.GetComponent<Avatar>().CreateCompleteFace(mouthEmotion, eyesEmotion, gameSessionSettings.avatarSettings.gender,
            gameSessionSettings.avatarSettings.skinColor, gameSessionSettings.avatarSettings.hairStyle, gameSessionSettings.avatarSettings.hairColor,
            gameSessionSettings.avatarSettings.eyesColor);
        if (iconNeeded)
        {
            if (correct)
                face.transform.Find("Correct").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/UI/OkIcon");
            else
            {
                face.transform.Find("Correct").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/UI/NotOkIcon");
                face.GetComponent<Avatar>().ChangeFaceOpacity(150);
            }
        }
        else
            face.transform.Find("Correct").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        gameManager.endRoundObjectSpawned.Add(face);
    }
    //metodo che spawna una faccia nel gioco sopra la UI, con un certo scaling e emozioni per occhi e bocca potenzialmente diverse
    protected void SpawnTextUI(Vector2 position, Emotion emotion, bool correct)
    {
        GameObject textUI = Instantiate(Resources.Load<GameObject>("Prefab/ImagePrefab/RiquadroEmozioneUI"), position, Quaternion.identity);
        textUI.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        textUI.GetComponent<TextMeshProUGUI>().text = MinigameManager.ConvertInCorrectText(emotion.ToString());
        Image imageComponent = textUI.transform.Find("Correct").GetComponent<Image>();
        if (correct)
        {
            imageComponent.sprite = Resources.Load<Sprite>("Sprite/UI/OkIcon");
            imageComponent.SetNativeSize();
        }
        else
        {
            imageComponent.sprite = Resources.Load<Sprite>("Sprite/UI/NotOkIcon");
            imageComponent.SetNativeSize();
            //ChangeOpacity(textUI);
        }

        gameManager.endRoundObjectSpawned.Add(textUI);
    }

    //metodo che converte una stringa in multicolore
    public static string ChangeTextToRandomColors(string text)
    {
        int currentColor;
        int previousColor = 0;
        string coloredString = "";
        string currentColorCode = "";

        for (int i = 0; i < text.Length; i++)
        {
            currentColor = UnityEngine.Random.Range(0, 4);
            while (currentColor == previousColor)
                currentColor = UnityEngine.Random.Range(0, 4);

            previousColor = currentColor;

            switch (currentColor)
            {
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

    public void NextRound()
    {
        if (gameManager.GetCurrentRound() < gameManager.GetTotalRounds())
            gameManager.StartNewRound();
        else
        {
            if (gameManager.IsPathEnabled() && SceneManager.GetActiveScene().name != "5_PhotographicEmotion")
                LevelSelectionManager.NextGame(SceneManager.GetActiveScene());
            else if (gameManager.IsPathEnabled())
                LevelSelectionManager.GoToModeSelection();
            else
                LevelSelectionManager.GoToMinigameSelection();
        }

    }
}
