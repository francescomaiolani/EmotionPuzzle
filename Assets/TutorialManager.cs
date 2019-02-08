using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{

	private string[] emotions = new string[] { "Felicità", "Tristezza", "Disgusto", "Rabbia", "Paura" };
	private GameObject facesPanel;
	public GameSessionSettings gameSessionSettings;

	// Use this for initialization
	void Start()
	{
		SelectableObject.objectSelectedEvent += ShowTutorialEmotion;
		if (GameObject.FindObjectOfType<GameSessionSettings>() == null)
			gameSessionSettings = Instantiate(new GameObject(), transform.position, Quaternion.identity).AddComponent<GameSessionSettings>();
		else
			gameSessionSettings = GameObject.FindObjectOfType<GameSessionSettings>();

		//istanzia la mano
		Instantiate(Resources.Load<GameObject>("Prefab/HandSelection"), Vector2.zero, Quaternion.identity);
		//istanzia le 5 facce
		int i = 0;
		foreach (string e in emotions)
		{
			Vector3 position = new Vector3(-6 + (i * 3), 0, 0);
			CreateFace(e, position, 1);
			i++;
		}

	}

	private void CreateFace(string emotionString, Vector3 position, float scale)
	{
		AvatarSettings avatarSettings = gameSessionSettings.avatarSettings;
		GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFace"), position, Quaternion.identity, facesPanel.transform) as GameObject;
		face.GetComponent<Avatar>().CreateCompleteFace(MinigameManager.ConvertTextInEmotion(emotionString), avatarSettings.gender,
			avatarSettings.skinColor, avatarSettings.hairStyle, avatarSettings.hairColor, avatarSettings.eyesColor);
		face.GetComponent<Avatar>().MakeAvatarSelectableObject();
		face.transform.localScale = new Vector3(scale, scale, 0);
	}

	private void CreateEmotionString(string emotionString, Vector3 position)
	{
		GameObject obj = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/RiquadriEmozione/RiquadroEmozione"), Camera.main.WorldToScreenPoint(position), Quaternion.identity, GameObject.Find("Canvas").transform);
		obj.GetComponent<TextMeshProUGUI>().text = MinigameManager.ConvertInCorrectText(emotionString);
		obj.transform.localScale = new Vector3(1, 1, 1);
	}

	private void ShowTutorialEmotion(GameObject objectSelected)
	{
		//disattiva il pannello
		facesPanel.SetActive(false);
		SelectableObject sel = objectSelected.GetComponent<SelectableObject>();
		Emotion emo = sel.GetEmotionType();

		//crea la faccia principale
		CreateFace(emo.ToString(), Vector3.zero, 1.5f);
		CreateEmotionString(emo.ToString(),new Vector3(0,-4,0));

	}
}
