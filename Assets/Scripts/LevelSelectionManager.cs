using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{

    public void StartGuessExpression()
    {
        MinigameManager.pathEnabled = false;
        SceneManager.LoadSceneAsync("1_GuessExpression");
    }

    public void StartPath()
    {
        MinigameManager.pathEnabled = true;
        SceneManager.LoadSceneAsync("1_GuessExpression");
    }

    public void LoadTutorialMode()
    {
        SceneManager.LoadScene("TutorialSelection");
    }

    public static void GoToModeSelection()
    {
        SceneManager.LoadScene("ModeSelection");
    }

    public static void GoToMinigameSelection()
    {
        SceneManager.LoadScene("MinigameSelection");
    }

    public void GoToMinigameSelectionFromMenu()
    {
        SceneManager.LoadScene("MinigameSelection");
    }

    public static void NextGame(Scene currentScene)
    {
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

}
