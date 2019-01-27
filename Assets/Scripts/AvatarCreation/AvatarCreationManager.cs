using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public struct AvatarSettings
{
	public Gender gender;
	public string skinColor, hairColor, hairStyle, eyesColor;
}

public enum Gender { Male, Female }

public class AvatarCreationManager : MonoBehaviour
{
	//il numero del pannello attuale a cui siamo arrivati
	private int currentPanelIndex = 0;
	[SerializeField]
	private GameObject nextPanelButton;
	private List<GameObject> panels;
	//pannelli che conterranno la UI per selezionare il colore della pelle ecc;
	[SerializeField]
	private GameObject genderSelectionPanel, skinColorPanel, hairStylePanel, hairColorPanel, eyesColorPanel, hairStyleMale, hairStyleFemale;

	//le avatar settings assegnate da salvare con l'utente
	public static AvatarSettings avatarSettings;
	public Avatar avatarFace;

	//messaggio in alto per indicare all'utente cosa fare
	[SerializeField]
	private TextMeshProUGUI message;
	private Dictionary<GameObject, string> panelMessage;

	void Start ()
	{
		//istanziazione della mano
		Instantiate (Resources.Load<GameObject> ("Prefab/HandSelection"), Vector2.zero, Quaternion.identity);
		//inizializzo i pannelli che serviranno nella lista
		InitialzePanels ();
		//inizializza i messaggi da dire quando cambi pannello
		InitializeDictionary ();
		//inizializza la faccia avatar a uno stato di default
		CreateAvatarFaceAndDefaultIt ();

	}

	//METODO CHE METTE TUTTI I PANNELLI IN UNA LISTA IN ORDINE DI APPARIZIONE
	//CAMBIARE ORDINE DI INSERIMENTO SE SI VUOLE CAMBIARE ORDINE DEI PANNELLI
	void InitialzePanels ()
	{
		panels = new List<GameObject> ();
		//i pannelli devono rimanere in quest'ordine se si vuole manetenere l'ordine attuale
		panels.Add (genderSelectionPanel);
		panels.Add (skinColorPanel);
		panels.Add (hairStylePanel);
		panels.Add (hairColorPanel);
		panels.Add (eyesColorPanel);
	}

	//assegna all'avatar dei valori di default
	void CreateAvatarFaceAndDefaultIt ()
	{
		//settaggio avatar da salvare successivamente
		avatarSettings = new AvatarSettings ();
		GameObject facePrefab = Instantiate (Resources.Load<GameObject> ("Prefab/AvatarFace"), new Vector3 (-4, -1, 0), Quaternion.identity);
		avatarFace = facePrefab.GetComponent<Avatar> ();
		AssignDefaultFaceValues ();
	}

	public void AssignDefaultFaceValues ()
	{
		AssignAvatarEmotion (Emotion.Felicità);
		AssignAvatarGender ("Male");
		AssignAvatarSkinColor ("White");
		AssignAvatarEyesColor ("Brown");
		AssignAvatarHairStyle ("Ciuffo");
		AssignAvatarHairColor ("DarkBrown");
	}

	//inizializza i messaggi da dire quando cambi pannello
	void InitializeDictionary ()
	{
		panelMessage = new Dictionary<GameObject, string> ();
		panelMessage.Add (genderSelectionPanel, "Sei un maschio o una femmina?");
		panelMessage.Add (skinColorPanel, "Di che colore e' la tua pelle?");
		panelMessage.Add (hairStylePanel, "Come sono i tuoi capelli?");
		panelMessage.Add (hairColorPanel, "Di che colore hai i capelli?");
		panelMessage.Add (eyesColorPanel, "Di che colore hai gli occhi?");
	}

	public void ShowNextPanel ()
	{
		//prima disattivo tutti i pannelli attivi nel caso
		DeactivateAllPanels ();
		//se l'indice del pannello e' in uno della lista 
		if (currentPanelIndex < panels.Count)
		{
			panels[currentPanelIndex].SetActive (true);
			if (panels[currentPanelIndex] == hairStylePanel)
			{
				//controllo se sei un maschio o una femmina per lo stile di capelli giusto
				if (avatarSettings.gender == Gender.Male)
				{
					hairStyleMale.SetActive (true);
					hairStyleFemale.SetActive (false);
				}
				else if (avatarSettings.gender == Gender.Female)
				{
					hairStyleMale.SetActive (false);
					hairStyleFemale.SetActive (true);
				}
			}
			//assegna il testo a seconda del pannello in cui siamo e lo colora random
			message.text = UIEndRoundManager.ChangeTextToRandomColors (panelMessage[panels[currentPanelIndex]]);
			//incrementa l'indice del pannello attuale
			currentPanelIndex++;
		}
		else
		{
			SceneManager.LoadSceneAsync ("MinigameSelection");
		}
	}

	void DeactivateAllPanels ()
	{
		foreach (GameObject g in panels)
			g.SetActive (false);
	}

	#region METODI PER ASSEGNARE TUTTE LE CARATTERISTICHE DELLA FACCIA

	//METODI CHE ASSEGNANO I COLORI GIUSTI ALLE COMPONENTI DELLA AVATAR FACE E SALVANO IN STRUCT I VALORI
	public void AssignAvatarGender (string gender)
	{
		avatarFace.AssignGender (gender);

		if (gender == "Male")
		{
			avatarSettings.gender = Gender.Male;
			avatarFace.AssignHairStyle (Gender.Male, "Ciuffo");
			avatarSettings.hairStyle = "Ciuffo";
		}
		else
		{
			avatarSettings.gender = Gender.Female;
			avatarFace.AssignHairStyle (Gender.Female, "RicciCorti");
			avatarSettings.hairStyle = "RicciCorti";
		}
		// siccome il genere e' la prima cosa che si setta appena ho finito vai al panel successivo
		ShowNextPanel ();
	}

	public void AssignAvatarEmotion (Emotion emotion)
	{
		avatarFace.AssignEmotion (emotion);
	}

	public void AssignAvatarSkinColor (string skinColorName)
	{
		avatarFace.AssignSkinColor (skinColorName);
		avatarSettings.skinColor = skinColorName;
	}

	public void AssignAvatarHairStyle (string hairName)
	{
		avatarFace.AssignHairStyle (avatarSettings.gender, hairName);
		avatarSettings.hairStyle = hairName;
	}

	public void AssignAvatarHairColor (string hairColorName)
	{
		avatarFace.AssignHairColor (hairColorName);
		avatarSettings.hairColor = hairColorName;
	}

	public void AssignAvatarEyesColor (string eyesColorName)
	{
		avatarFace.AssignEyesColor (eyesColorName);
		avatarSettings.eyesColor = eyesColorName;
	}

	void ActivateNextPanelButton ()
	{
		nextPanelButton.SetActive (true);
	}
	#endregion

}
