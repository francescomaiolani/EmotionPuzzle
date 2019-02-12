using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayOrDataUIManager : MonoBehaviour
{
	public Text nomePlayer;

	public void GoToData()
	{
		SceneManager.LoadSceneAsync("DataVisualization");
	}
	public void GoToLogin()
	{
		SceneManager.LoadSceneAsync("Login");
	}
	private void GoToGame()
	{
		SceneManager.LoadSceneAsync("AvatarCreation");
	}

	public void SavePlayerName()
	{
		if (nomePlayer.text != "")
		{
			PlayerPrefs.SetString("PlayerName", nomePlayer.text);
			GoToGame();
		}
	}
}
