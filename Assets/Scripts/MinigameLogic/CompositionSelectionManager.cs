using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositionSelectionManager : MinigameManager
{

	public Transform eyesPosition;
	public Transform mouthPosition;

	private UICompositionSelectionManager UIManager;
	private bool[] mouthPositioned = new bool[2];
	private bool[] eyesPositioned = new bool[2];
	readonly int maxPiecesNumber = 4;

	private SelectableObject mainMouth = null;
	private SelectableObject mainEyes = null;

	[SerializeField]
	private Avatar centralFace;

	//prende le emozioni scelte dall'utente e le immagazzina qui
	[SerializeField]
	protected Emotion eyesEmotionChosen;
	[SerializeField]
	protected Emotion mouthEmotionChosen;

	protected override void Start()
	{
		base.Start();
		UIManager = FindObjectOfType<UICompositionSelectionManager>();
		SelectableObject.objectSelectedEvent += CheckIfMinigameCompleted;
		endRoundObjectSpawned = new List<GameObject>();
		StartNewRound();
	}

	public override void StartNewRound()
	{
		if (inMagicRoom)
			ResetLights();
		UpdateRound();
		endRoundPanel.SetActive(false);
		DestroySceneObjects();
		PickNewEmotion();
		UIManager.UpdateUI(this);
		SpawnSceneObjects();
		AssignCentralFace();
	}

	private void ResetLights()
	{
		Color c = Color.yellow;
		c.a = 100 / 255f;
		MagicRoomLightManager.instance.sendColour(c);
	}

	protected override void SpawnSceneObjects()
	{
		SpawnCorrectElement();
		SpawnOtherElements();
	}

	protected override void DestroySceneObjects()
	{
		DestroyAnswerObjectSpawned();

		//resetto gli array degli icchi e bocca
		mouthPositioned = new bool[] { false, false };
		eyesPositioned = new bool[] { false, false };

		//distruggi tutti i pezzi della faccia
		SelectableObject[] selectableObjects = FindObjectsOfType<SelectableObject>();
		foreach (SelectableObject s in selectableObjects)
			Destroy(s.gameObject);
	}

	protected void CheckIfMinigameCompleted(GameObject selObject)
	{
		SelectableObject selectableObject = selObject.GetComponent<SelectableObject>();
		if (mainEyes == null && selectableObject.facePartType == FaceParts.Eyes)
		{
			mainEyes = selectableObject;
			selectableObject.transform.position = eyesPosition.position;
			eyesEmotionChosen = mainEyes.GetEmotionType();
			mainEyes.transform.localScale = new Vector3(1, 1, 1);
		}
		else if (mainEyes != null && selectableObject.facePartType == FaceParts.Eyes)
		{
			mainEyes.transform.position = mainEyes.initialPosition;
			mainEyes.ReactivateSelectableObject();
			mainEyes.GetComponent<Animator>().SetTrigger("PopDown");
			selectableObject.transform.position = eyesPosition.position;
			mainEyes = selectableObject;
			eyesEmotionChosen = mainEyes.GetEmotionType();
			mainEyes.transform.localScale = new Vector3(1, 1, 1);
		}

		if (mainMouth == null && selectableObject.facePartType == FaceParts.Mouth)
		{
			mainMouth = selectableObject;
			selectableObject.transform.position = mouthPosition.position;
			mouthEmotionChosen = mainMouth.GetEmotionType();
			mainMouth.transform.localScale = new Vector3(1, 1, 1);
		}
		else if (mainMouth != null && selectableObject.facePartType == FaceParts.Mouth)
		{
			mainMouth.transform.position = mainMouth.initialPosition;
			mainMouth.ReactivateSelectableObject();
			mainEyes.GetComponent<Animator>().SetTrigger("PopDown");
			selectableObject.transform.position = mouthPosition.position;
			mainMouth = selectableObject;
			mouthEmotionChosen = mainMouth.GetEmotionType();
			mainMouth.transform.localScale = new Vector3(1, 1, 1);
		}

		if (mainMouth != null && mainEyes != null)
		{
			roundResult = CheckAnswer();
			UpdateResultDB();
			Invoke("DestroySceneObjects", 1f);
			Invoke("EndRound", 1f);
		}

	}

	public override bool CheckAnswer()
	{
		if (eyesEmotionChosen != mainEmotion || mouthEmotionChosen != mainEmotion)
		{
			GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Fail");
			GetComponent<AudioSource>().Play();
			return false;
		}

		GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Trumpet");
		GetComponent<AudioSource>().Play();
		return true;

	}

	protected override void RepeatRound()
	{

	}

	//Metodo che gestisce la schermata di fine round
	protected override void EndRound()
	{
		endRoundPanel.SetActive(true);
		UIManager.EndRoundUI(roundResult);
	}

	//ASSEGNA AL VOLTO IN MEZZO LE FATTEZZE DEL TUO AVATAR E DISATTIVO GLI ELEMENTI DELLA FACCIA CHE NON DEVONO ESSERE VISIBILI
	void AssignCentralFace()
	{
		AvatarSettings referenceAvatar = gameSessionSettings.avatarSettings;
		centralFace.CreateCompleteFace(GetMainEmotion(), referenceAvatar.gender, referenceAvatar.skinColor, referenceAvatar.hairStyle, referenceAvatar.hairColor, referenceAvatar.eyesColor);
		centralFace.DeactivateFaceElements();
	}

	void SpawnCorrectElement()
	{
		//  SPAWN DELLA BOCCA
		int indexOfMouth = Random.Range(0, 2);
		GameObject mouth = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/MouthSelect"), spawnPointPositions[indexOfMouth].localPosition, Quaternion.identity);
		//questo posto e' stato occupato
		mouthPositioned[indexOfMouth] = true;
		//assegna l'emozione scelta alla faccia
		mouth.GetComponent<SelectableObject>().SetFacePartEmotion(mainEmotion, gameSessionSettings.avatarSettings);

		//SPAWN DEGLI OCCHI
		int indexOfEyes = Random.Range(0, 2);
		GameObject eyes = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/EyesSelect"), spawnPointPositions[indexOfEyes + 2].localPosition, Quaternion.identity);
		//questo posto e' stato occupato
		eyesPositioned[indexOfEyes] = true;
		eyes.GetComponent<SelectableObject>().SetFacePartEmotion(mainEmotion, gameSessionSettings.avatarSettings);
	}

	//crea le altre bocche e occhi in modo che siano sbagliati
	void SpawnOtherElements()
	{
		for (int i = 0; i < mouthPositioned.Length; i++)
		{
			if (!mouthPositioned[i])
			{
				GameObject mouth = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/MouthSelect"), spawnPointPositions[i].localPosition, Quaternion.identity);
				mouthPositioned[i] = true;
				mouth.GetComponent<SelectableObject>().SetFacePartEmotion(PickNotMainEmotion(mainEmotion), gameSessionSettings.avatarSettings);
			}
		}
		for (int i = 0; i < eyesPositioned.Length; i++)
		{
			if (!eyesPositioned[i])
			{
				GameObject eyes = Instantiate(Resources.Load<GameObject>("Prefab/DraggableObject/FacePieces/EyesSelect"), spawnPointPositions[i + 2].localPosition, Quaternion.identity);
				eyesPositioned[i] = true;
				eyes.GetComponent<SelectableObject>().SetFacePartEmotion(PickNotMainEmotion(mainEmotion), gameSessionSettings.avatarSettings);
			}
		}
	}

	//ritorna i due pezzi di faccia che sono stati scelti
	public Emotion GetEyesEmotionChosen()
	{
		return eyesEmotionChosen;
	}

	//ritorna i due pezzi di faccia che sono stati scelti
	public Emotion GetMouthEmotionChosen()
	{
		return mouthEmotionChosen;
	}

	private void OnDisable()
	{
		SelectableObject.objectSelectedEvent -= CheckIfMinigameCompleted;

	}

}
