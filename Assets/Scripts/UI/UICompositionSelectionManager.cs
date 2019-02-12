using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UICompositionSelectionManager : UIEndRoundManager
{

	public Image advicePanel;
	public Text adviceText;

	public TextMeshProUGUI emotionText;

	protected override void Start()
	{
		base.Start();
		Hand.adviceGiven += GiveAdvice;
	}

	void GiveAdvice(string advice)
	{
		adviceText.text = advice;
		advicePanel.gameObject.SetActive(true);
		advicePanel.GetComponent<Animator>().Play("Advice");
	}

	void DisableAdvicePanel()
	{
		advicePanel.gameObject.SetActive(false);
	}

	public void UpdateUI(MinigameManager manager)
	{
		emotionText.text = MinigameManager.ConvertInCorrectText(manager.GetEmotionString());
		//fumettoText.text = fumettoPhrase.Values
	}

	protected override void SetQA(bool roundResult)
	{
		sentencesQA[0].text = MinigameManager.ConvertInCorrectText(gameManager.GetEmotionString());

		if (roundResult)	
			SpawnFace(new Vector3(0, -2, 0), gameManager.GetComponent<CompositionSelectionManager>().GetEyesEmotionChosen(), gameManager.GetComponent<CompositionSelectionManager>().GetMouthEmotionChosen(), true, 1.4f, true);
		
		else
			SpawnFace(new Vector3(0f, -2, 0), gameManager.GetComponent<CompositionSelectionManager>().GetEyesEmotionChosen(), gameManager.GetComponent<CompositionSelectionManager>().GetMouthEmotionChosen(), false, 1.4f, true);
	}

}
