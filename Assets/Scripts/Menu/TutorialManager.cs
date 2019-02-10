using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

	private string[] emotions = new string[] { "Felicità", "Tristezza", "Disgusto", "Rabbia", "Paura" };
	[SerializeField]
	private GameObject facesPanel;
	public TextMeshProUGUI topText;
	public GameSessionSettings gameSessionSettings;
	public SpriteRenderer photoSprite;
	public GameObject cornicePhoto;

	// Use this for initialization
	void Start()
	{
		topText.text = UIEndRoundManager.ChangeTextToRandomColors("Scopri le emozioni");
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
			CreateFace("main", e, position, 1, facesPanel.transform, true);
			i++;
		}

	}

	private GameObject CreateFace(string type, string emotionString, Vector3 position, float scale, Transform parent, bool selectable)
	{
		AvatarSettings avatarSettings = gameSessionSettings.avatarSettings;
		GameObject face = Instantiate(Resources.Load<GameObject>("Prefab/AvatarFace"), position, Quaternion.identity, parent) as GameObject;
		if (type == "main")
			face.GetComponent<Avatar>().CreateCompleteFace(MinigameManager.ConvertTextInEmotion(emotionString), avatarSettings.gender,
				avatarSettings.skinColor, avatarSettings.hairStyle, avatarSettings.hairColor, avatarSettings.eyesColor);
		else
			face.GetComponent<Avatar>().CreateRandomFace();
		if (selectable)
			face.GetComponent<Avatar>().MakeAvatarSelectableObject();
		face.transform.localScale = new Vector3(scale, scale, 0);
		return face;
	}

	private void CreateEmotionString(string emotionString, Vector3 position)
	{
		GameObject obj = Instantiate(Resources.Load<GameObject>("Prefab/SelectableObject/RiquadriEmozione/RiquadroEmozione"), position, Quaternion.identity, GameObject.Find("Canvas").transform);
		obj.GetComponent<RectTransform>().anchoredPosition = position;
		obj.GetComponent<TextMeshProUGUI>().text = MinigameManager.ConvertInCorrectText(emotionString);
		obj.transform.localScale = new Vector3(1.2f, 1.2f, 1);
		obj.GetComponent<SelectableObject>().enabled = false;
	}

	private void CreatePhoto(string emotionString)
	{
		cornicePhoto.SetActive(true);
		photoSprite.gameObject.SetActive(true);
		photoSprite.sprite = Resources.Load<Sprite>("Sprite/Photos/" + emotionString + "/" + Random.Range(1, 5));
		Vector2 dimension = new Vector2(5f / photoSprite.size.x, 5f / photoSprite.size.y);
		photoSprite.transform.localScale = dimension;
	}

	private void ShowTutorialEmotion(GameObject objectSelected)
	{
		//disattiva il pannello
		facesPanel.SetActive(false);
		SelectableObject sel = objectSelected.GetComponent<SelectableObject>();
		Emotion emo = sel.GetEmotionType();
		if (emo.ToString() != "Disgusto")
			topText.text = UIEndRoundManager.ChangeTextToRandomColors("Questa e' la " + MinigameManager.ConvertInCorrectText(emo.ToString()));
		else
			topText.text = UIEndRoundManager.ChangeTextToRandomColors("Questo e' il " + MinigameManager.ConvertInCorrectText(emo.ToString()));

		//crea tutti gli elementi
		CreateFace("main", emo.ToString(), Vector3.zero, 1.5f, null, false);
		CreateEmotionString(emo.ToString(), new Vector3(0, -160, 0));
		GameObject face1 = CreateFace("random", emo.ToString(), new Vector3(5.5f, 2, 0), 1.2f, null, false);
		face1.GetComponent<Avatar>().AssignEmotion(emo);
		GameObject face2 = CreateFace("random", emo.ToString(), new Vector3(5.5f, -2, 0), 1.2f, null, false);
		face2.GetComponent<Avatar>().AssignEmotion(emo);
		CreatePhoto(emo.ToString());
	}

	public void GoToModeSelection()
	{
		SceneManager.LoadSceneAsync("ModeSelection");
	}
	private void OnDisable()
	{
		SelectableObject.objectSelectedEvent -= ShowTutorialEmotion;

	}
}
