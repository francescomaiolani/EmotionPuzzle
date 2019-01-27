using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{

	private string[] maleHairNames, femaleHairNames, skinColorNames, eyesColorNames, hairColorNames;
	public delegate void OnFaceReady ();
	public event OnFaceReady FaceReady;
	[SerializeField]
	private GameObject face, eyes, hair, eyebrow, earInsideLeft, earInsideRight, nose, earRing, lashes;
	private Emotion emotion;
	//dictionary con tutti i colori disponibili da customizzare, catalogati per parte del corpo
	private Dictionary<string, Color32> skinColorDictionary, eyesColorDictionary, hairColorDictionary;

	void Start ()
	{
		InitializeDictionariesAndArrays ();
		FaceReady ();
	}

	//inizializza i valori dei dictionaries dei colori 
	void InitializeDictionariesAndArrays ()
	{
		maleHairNames = new string[] { "Corti", "Medi", "Ricci", "Pelato", "Ciuffo" };
		femaleHairNames = new string[] { "Corti", "Medi", "CortiRicci", "Ricci", "Lunghi" };
		skinColorNames = new string[] { "PaleWhite", "White", "Olive", "Brown", "Dark", };
		hairColorNames = new string[] { "DarkBrown", "LightBrown", "Black", "Blonde", "Red", };
		eyesColorNames = new string[] { "LightBlue", "Green", "Brown", "Black" };

		skinColorDictionary = new Dictionary<string, Color32> ();
		eyesColorDictionary = new Dictionary<string, Color32> ();
		hairColorDictionary = new Dictionary<string, Color32> ();


		skinColorDictionary.Add (skinColorNames[0], new Color32 (255, 208, 160, 255));
		skinColorDictionary.Add (skinColorNames[1], new Color32 (250, 191, 133, 255));
		skinColorDictionary.Add (skinColorNames[2], new Color32 (239, 192, 107, 255));
		skinColorDictionary.Add (skinColorNames[3], new Color32 (176, 139, 77, 255));
		skinColorDictionary.Add (skinColorNames[4], new Color32 (118, 100, 67, 255));

		hairColorDictionary.Add (hairColorNames[0], new Color32 (104, 82, 61, 255));
		hairColorDictionary.Add (hairColorNames[1], new Color32 (152, 108, 68, 255));
		hairColorDictionary.Add (hairColorNames[2], new Color32 (61, 52, 48, 255));
		hairColorDictionary.Add (hairColorNames[3], new Color32 (255, 217, 81, 255));
		hairColorDictionary.Add (hairColorNames[4], new Color32 (255, 131, 66, 255));

		eyesColorDictionary.Add (eyesColorNames[0], new Color32 (115, 183, 167, 255));
		eyesColorDictionary.Add (eyesColorNames[1], new Color32 (136, 183, 65, 255));
		eyesColorDictionary.Add (eyesColorNames[2], new Color32 (94, 69, 45, 255));
		eyesColorDictionary.Add (eyesColorNames[3], new Color32 (60, 60, 60, 255));
	}

	//METODO PER CREARE UNA FACCIA COMPLETA QUALUNQUE MA CON VALORI ASSEGNATI DALL'ESTERNO
	public void CreateCompleteFace (Gender gender, string skinColor, string hairStyle, string hairColor, string eyesColor)
	{
		AssignGender (gender.ToString ());
		AssignSkinColor (skinColor);
		AssignHairStyle (gender, hairStyle);
		AssignHairColor (hairColor);
		AssignEyesColor (eyesColor);
	}

	public void CreateRandomFace ()
	{
		Gender[] genders = new Gender[] { Gender.Male, Gender.Female };
		Gender randomGender = genders[Random.Range (0, 2)];

		AssignGender (randomGender.ToString ());
		AssignSkinColor (skinColorNames[Random.Range (0, skinColorNames.Length)]);
		if (randomGender == Gender.Male)
			AssignHairStyle (randomGender, maleHairNames[Random.Range (0, maleHairNames.Length)]);
		else if (randomGender == Gender.Female)
			AssignHairStyle (randomGender, femaleHairNames[Random.Range (0, femaleHairNames.Length)]);

		AssignHairColor (hairColorNames[Random.Range (0, hairColorNames.Length)]);
		AssignEyesColor (eyesColorNames[Random.Range (0, eyesColorNames.Length)]);

	}

	//METODI CHE ASSEGNANO I VALORI GIUSTI ALLE COMPONENTI
	public void AssignGender (string gender)
	{
		face.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Face/" + gender);
		if (gender == "Male")
		{
			earRing.SetActive (false);
			lashes.SetActive (false);
		}
		else
		{
			earRing.SetActive (true);
			lashes.SetActive (true);
		}
		eyebrow.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyebrow/" + gender);
	}

	public void AssignSkinColor (string skinColorName)
	{
		face.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
		earInsideLeft.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
		earInsideRight.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
		nose.GetComponent<SpriteRenderer> ().color = skinColorDictionary[skinColorName];
	}

	public void AssignHairStyle (Gender gender, string hairName)
	{
		hair.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Hair/" + gender.ToString () + "/" + hairName);
	}

	public void AssignHairColor (string hairColorName)
	{
		hair.GetComponent<SpriteRenderer> ().color = hairColorDictionary[hairColorName];
		eyebrow.GetComponent<SpriteRenderer> ().color = hairColorDictionary[hairColorName];
	}

	public void AssignEyesColor (string eyesColorName)
	{
		eyes.GetComponent<SpriteRenderer> ().color = eyesColorDictionary[eyesColorName];
	}
}
