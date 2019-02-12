using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Emotion { Felicità, Tristezza, Disgusto, Rabbia, Paura }

public abstract class MinigameManager : MonoBehaviour
{

    [Header("true se siamo in magicRoom")]
    public bool inMagicRoom;
    [Header("Punti dove saranno spawnati gli oggeti de spostare")]
    public Transform[] spawnPointPositions;
    [Header("Inserire il pannello di fine round")]
    public GameObject endRoundPanel;
    [Header("Numero di round totali")]
    public int totalRounds;

    //Variabile statica che tiene conto della scelta del percoso o del minigioco singolo
    public static bool pathEnabled = false;

    [SerializeField]
    protected Emotion mainEmotion;
    protected Emotion previousChosenEmotion;
    //Risposta scelta dall'utente
    protected Emotion emotionAnswer;
    protected int currentRound = 0;
    //risultato dell'ultimo round effettuato, inizializzato a true perche' parte da subito
    protected bool roundResult = true;
    //public GameObject[] spawnableObjectsPrefab;
    //Lista che tiene i game object degli oggetti risposta creati nell'endGame e che vengono cancellate all'inizio di un nuovo round
    [SerializeField]
    public List<GameObject> endRoundObjectSpawned;

    public GameSessionSettings gameSessionSettings;

    // verosimilmente in ogni minigioco degli oggetti andranno spawnati 
    //ossia tutti quei pezzi tra cui scegliere ecc..
    protected abstract void SpawnSceneObjects();
    protected abstract void DestroySceneObjects();
    public abstract void StartNewRound();
    protected abstract void RepeatRound();
    protected abstract void EndRound();
    //protected abstract void CheckIfMinigameCompleted();

    protected virtual void Start()
    {
        gameObject.AddComponent<AudioSource>();

        if (GameObject.FindObjectOfType<GameSessionSettings>() == null)
        {
            gameSessionSettings = Instantiate(new GameObject(), transform.position, Quaternion.identity).AddComponent<GameSessionSettings>();
        }
        else
            gameSessionSettings = GameObject.FindObjectOfType<GameSessionSettings>();
    }
    //Metodo per scegliere un'emozione tra quelle disponibili
    protected void PickNewEmotion()
    {
        //seleziona un'emozione a caso
        int randomEmotion = UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        mainEmotion = (Emotion) randomEmotion;
    }

    //metodo che ritorna un'emozione che non e' quella corretta in modo da assegnare un'emozione random agli altri pezzi sbagliati
    protected Emotion PickNotMainEmotion(Emotion main)
    {
        int randomEmotion = UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
        Emotion chosenEmotion = (Emotion) randomEmotion;

        while (chosenEmotion == mainEmotion)
        {
            randomEmotion = UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(Emotion)).Length);
            chosenEmotion = (Emotion) randomEmotion;
        }

        return chosenEmotion;
    }

    //Metodo che aumenta il contatore dei round
    protected void UpdateRound()
    {
        currentRound += 1;
    }

    //Metodo che si occupa di controllare l'esito della selezione di una risposta
    public virtual bool CheckAnswer()
    {
        if (emotionAnswer == mainEmotion)
        {
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Trumpet");
            GetComponent<AudioSource>().Play();
            return true;
        }
        else
        {
            GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Fail");
            GetComponent<AudioSource>().Play();
            return false;
        }
    }

    // metodo che distrugge tutte le istanze delle risposte create. Da invocare ad ogni start round
    protected void DestroyAnswerObjectSpawned()
    {
        if (endRoundObjectSpawned.Count != 0)
        {
            for (int i = 0; i < endRoundObjectSpawned.Count; i++)
            {
                Destroy(endRoundObjectSpawned[i]);
            }
            endRoundObjectSpawned.Clear();
        }
    }

    //Metodo che inserisce il risultato di un round nel database
    protected void UpdateResultDB()
    {
        string game = GetGameMode();
        string emotion = mainEmotion.ToString();
        if (emotion == "Felicità")
            emotion = "Felicita";
        int result;
        if (roundResult)
            result = 1;
        else
            result = 0;
        DatabaseManager.InsertResult(PlayerPrefs.GetString("PlayerName"), emotion, game, result);
    }


    private string GetGameMode()
    {
        string s = "";
        if (SceneManager.GetActiveScene().name == "1_GuessExpression")
            s = "GuessExpression";
        else if (SceneManager.GetActiveScene().name == "2_HowDoYouFeel")
            s = "HowDoYouFeel";
        else if (SceneManager.GetActiveScene().name == "3_WhichPersonIsGame")
            s = "WhichPersonIs";
        else if (SceneManager.GetActiveScene().name == "4_CompositionGameSelectable")
            s = "Composition";
        else if (SceneManager.GetActiveScene().name == "5_PhotographicEmotion")
            s = "PhotographicEmotion";
        return s;
    }

    //Metodo che salva la risposta data dall'utente
    public void SetAnswer(Emotion e)
    {
        emotionAnswer = e;
    }

    public string GetEmotionString()
    {
        return mainEmotion.ToString();
    }

    public Emotion GetMainEmotion()
    {
        return mainEmotion;
    }

    public string GetEmotionAnswerString()
    {
        return emotionAnswer.ToString();
    }

    public Emotion GetEmotionAnswer()
    {
        return emotionAnswer;
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }

    public int GetTotalRounds()
    {
        return totalRounds;
    }

    public bool IsPathEnabled()
    {
        return pathEnabled;
    }

    //il font non ha la a con l'accento quindi converto con l'apostrofo
    public static string ConvertInCorrectText(string emotion)
    {
        if (emotion == "Felicità")
            return "Felicita'";
        else
            return emotion;
    }

    public static Emotion ConvertTextInEmotion(string emotionText)
    {
        if (emotionText == "Felicità")
            return Emotion.Felicità;
        else if (emotionText == "Tristezza")
            return Emotion.Tristezza;
        else if (emotionText == "Rabbia")
            return Emotion.Rabbia;
        else if (emotionText == "Disgusto")
            return Emotion.Disgusto;
        else if (emotionText == "Paura")
            return Emotion.Paura;
        else
            return Emotion.Felicità;
    }
}
