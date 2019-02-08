using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameSelectionManager : MonoBehaviour
{

    public void StartGuessExpression()
    {
        MinigameManager.pathEnabled = false;
        SceneManager.LoadSceneAsync("1_GuessExpression");
    }

    public void StartHowDoYouFeel()
    {
        MinigameManager.pathEnabled = false;
        SceneManager.LoadSceneAsync("2_HowDoYouFeel");
    }
    public void StartWhichPersonIsGame()
    {
        MinigameManager.pathEnabled = false;
        SceneManager.LoadSceneAsync("3_WhichPersonIsGame");
    }
    public void StartCompositionGame()
    {
        MinigameManager.pathEnabled = false;
        SceneManager.LoadSceneAsync("4_CompositionGameSelectable");
    }
    public void StartPhotographicEmotion()
    {
        MinigameManager.pathEnabled = false;
        SceneManager.LoadSceneAsync("5_PhotographicEmotion");
    }
}
