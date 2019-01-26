using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCreationManager : MonoBehaviour
{
	//il numero del pannello attuale a cui siamo arrivati
	private int currentPanelIndex = 0;
	private List<GameObject> panels;
	[SerializeField]
	private GameObject skinColorPanel, hairPanel, eyesColorPanel;
	//componenti dell'avatar
	[SerializeField]
	private GameObject avatarSkinColor, avatarEyesColor, avatarHair, avatarEyebrow;

	private Dictionary<string, Color32> skinColorDictionary, eyesColorDictionary, hairColorDictionary;

	#region methods

	void Start ()
	{
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

		skinColorDictionary.Add ("PaleWhite", new Color32 ());
		skinColorDictionary.Add ("White", new Color32 ());
		skinColorDictionary.Add ("SouthWhite", new Color32 (250, 191, 133, 255));
		skinColorDictionary.Add ("Olive", new Color32 ());
		skinColorDictionary.Add ("Brown", new Color32 ());
		skinColorDictionary.Add ("Dark", new Color32 ());

		eyesColorDictionary.Add ("LightBlue", new Color32 ());
		eyesColorDictionary.Add ("Blue", new Color32 ());
		eyesColorDictionary.Add ("Green", new Color32 ());
		eyesColorDictionary.Add ("Brown", new Color32 ());
		eyesColorDictionary.Add ("Black", new Color32 ());

		hairColorDictionary.Add ("DarkBrown", new Color32 (104, 82, 61, 255));
		hairColorDictionary.Add ("LightBrown", new Color32 (152, 108, 68, 255));
		hairColorDictionary.Add ("Black", new Color32 (61, 52, 48, 255));
		hairColorDictionary.Add ("Blonde", new Color32 (255, 217, 81, 255));
		hairColorDictionary.Add ("Red", new Color32 (255, 131, 66, 255));
	}

	//assegna all'avatar dei valori di default
	void AssignDefaultAvatarFace ()
	{
		AssignSkinColor ("SouthWhite");
		AssignEyesColor ("Brown");
		AssignHairStyle ("Ciuffo");
		AssignHairColor ("DarkBrown");
	}

	void InitialzePanels ()
	{
		panels = new List<GameObject> ();
		//i pannelli devono rimanere in quest'ordine se si vuole manetenere l'ordine attuale
		panels.Add (skinColorPanel);
		panels.Add (hairPanel);
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
	public void AssignSkinColor (string skinColorName)
	{
		avatarSkinColor.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
	}

	public void AssignHairStyle (string hairName)
	{
		avatarHair.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Hair/" + hairName);
	}

	public void AssignHairColor (string hairColorName)
	{
		avatarHair.GetComponent<SpriteRenderer> ().color = hairColorDictionary[hairColorName];
		avatarEyebrow.GetComponent<SpriteRenderer> ().color = hairColorDictionary[hairColorName];
	}

	public void AssignEyesColor (string eyesColorName)
	{
		avatarEyesColor.GetComponent<SpriteRenderer> ().color = eyesColorDictionary[eyesColorName];
	}

	#endregion
}
