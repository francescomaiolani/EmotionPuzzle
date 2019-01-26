using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private List<GameObject> panels;
	//pannelli che conterranno la UI per selezionare il colore della pelle ecc;
	[SerializeField]
	private GameObject genderSelectionPanel, skinColorPanel, hairStylePanel, hairColorPanel, eyesColorPanel;
	//componenti dell'avatar
	[SerializeField]
	private GameObject avatarFace, avatarEyesColor, avatarHair, avatarEyebrow, avatarEarInsideLeft, avatarEarInsideRight, avatarNose;
	//dictionary con tutti i colori disponibili da customizzare, catalogati per parte del corpo
	private Dictionary<string, Color32> skinColorDictionary, eyesColorDictionary, hairColorDictionary;
	//le avatar settings assegnate da salvare con l'utente
	AvatarSettings avatarSettings;


	#region methods

	void Start ()
	{
		//settaggio avatar da salvare successivamente
		avatarSettings = new AvatarSettings ();

		InitializeDictionaries ();
		//inizializza la faccia avatar a uno stato di default
		AssignDefaultAvatarFace ();
		//inizializzo i pannelli che serviranno nella lista
		InitialzePanels ();
		//mostra il primo pannello nella lista
		ShowNextPanel ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			ShowNextPanel ();
	}

	//inizializza i valori dei dictionaries dei colori 
	void InitializeDictionaries ()
	{
		skinColorDictionary = new Dictionary<string, Color32> ();
		eyesColorDictionary = new Dictionary<string, Color32> ();
		hairColorDictionary = new Dictionary<string, Color32> ();

		skinColorDictionary.Add ("PaleWhite", new Color32 (255, 208, 160, 255));
		skinColorDictionary.Add ("White", new Color32 (250, 191, 133, 255));
		skinColorDictionary.Add ("Olive", new Color32 (239, 192, 107, 255));
		skinColorDictionary.Add ("Brown", new Color32 (176, 139, 77, 255));
		skinColorDictionary.Add ("Dark", new Color32 (133, 105, 56, 255));

		hairColorDictionary.Add ("DarkBrown", new Color32 (104, 82, 61, 255));
		hairColorDictionary.Add ("LightBrown", new Color32 (152, 108, 68, 255));
		hairColorDictionary.Add ("Black", new Color32 (61, 52, 48, 255));
		hairColorDictionary.Add ("Blonde", new Color32 (255, 217, 81, 255));
		hairColorDictionary.Add ("Red", new Color32 (255, 131, 66, 255));

		eyesColorDictionary.Add ("LightBlue", new Color32 ());
		eyesColorDictionary.Add ("Blue", new Color32 ());
		eyesColorDictionary.Add ("Green", new Color32 ());
		eyesColorDictionary.Add ("Brown", new Color32 ());
		eyesColorDictionary.Add ("Black", new Color32 ());
	}

	//assegna all'avatar dei valori di default
	void AssignDefaultAvatarFace ()
	{
		AssignGender ("Male");
		AssignSkinColor ("White");
		AssignEyesColor ("Brown");
		AssignHairStyle ("Ciuffo");
		AssignHairColor ("DarkBrown");
	}

	void InitialzePanels ()
	{
		panels = new List<GameObject> ();
		//i pannelli devono rimanere in quest'ordine se si vuole manetenere l'ordine attuale
		panels.Add (genderSelectionPanel);
		panels.Add (skinColorPanel);
		panels.Add (hairColorPanel);
		panels.Add (hairStylePanel);
		panels.Add (eyesColorPanel);
	}

	private void ShowNextPanel ()
	{
		//prima disattivo tutti i pannelli attivi nel caso
		DeactivateAllPanels ();
		//se l'indice del pannello e' in uno della lista 
		if (currentPanelIndex < panels.Count)
		{
			panels[currentPanelIndex].SetActive (true);
			//incrementa l'indice del pannello attuale
			currentPanelIndex++;
		}
		else
		{
			//vai alla schermata successiva
		}
	}

	void DeactivateAllPanels ()
	{
		foreach (GameObject g in panels)
			g.SetActive (false);
	}

	//METODI CHE ASSEGNANO I COLORI GIUSTI ALLE COMPONENTI
	public void AssignGender (string gender)
	{
		avatarFace.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Face/" + gender);
		if (gender == "Male")
			avatarSettings.gender = Gender.Male;
		else
			avatarSettings.gender = Gender.Female;
	}

	public void AssignSkinColor (string skinColorName)
	{
		avatarFace.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
		avatarEarInsideLeft.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
		avatarEarInsideRight.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
		avatarNose.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
		avatarSettings.skinColor = skinColorName;
	}

	public void AssignHairStyle (string hairName)
	{
		avatarHair.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Hair/" + hairName);
		avatarSettings.hairStyle = hairName;
	}

	public void AssignHairColor (string hairColorName)
	{
		avatarHair.GetComponent<SpriteRenderer> ().color = hairColorDictionary[hairColorName];
		avatarEyebrow.GetComponent<SpriteRenderer> ().color = hairColorDictionary[hairColorName];
		avatarSettings.hairColor = hairColorName;
	}

	public void AssignEyesColor (string eyesColorName)
	{
		avatarEyesColor.GetComponent<SpriteRenderer> ().color = eyesColorDictionary[eyesColorName];
		avatarSettings.eyesColor = eyesColorName;
	}

	#endregion
}
