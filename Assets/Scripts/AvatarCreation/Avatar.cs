using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
	[SerializeField]
	private GameObject face, eyes, hair, eyebrow, earInsideLeft, earInsideRight, nose, earRing, lashes, mouth;
	private Emotion emotion;

	//METODO PER CREARE UNA FACCIA COMPLETA QUALUNQUE MA CON VALORI ASSEGNATI DALL'ESTERNO
	public void CreateCompleteFace (Emotion emotion, Gender gender, string skinColor, string hairStyle, string hairColor, string eyesColor)
	{
		this.emotion = emotion;
		AssignEmotion (emotion);
		AssignGender (gender.ToString ());
		AssignSkinColor (skinColor);
		AssignHairStyle (gender, hairStyle);
		AssignHairColor (hairColor);
		AssignEyesColor (eyesColor);
	}

	//METODO CHE CREA UNA FACCIA CASUALE MA SENZA ASSEGNARE UN'EMOZIONE
	public void CreateRandomFace ()
	{
		Gender[] genders = new Gender[] { Gender.Male, Gender.Female };
		Gender randomGender = genders[Random.Range (0, 2)];

		AssignGender (randomGender.ToString ());
		string skinColor = AvatarData.skinColorNames[Random.Range (0, AvatarData.skinColorNames.Length)];
		AssignSkinColor (skinColor);
		if (randomGender == Gender.Male)
			AssignHairStyle (randomGender, AvatarData.maleHairNames[Random.Range (0, AvatarData.maleHairNames.Length)]);
		else if (randomGender == Gender.Female)
			AssignHairStyle (randomGender, AvatarData.femaleHairNames[Random.Range (0, AvatarData.femaleHairNames.Length)]);

		AssignHairColor (AvatarData.hairColorNames[Random.Range (0, AvatarData.hairColorNames.Length)]);
		AssignEyesColor (AvatarData.eyesColorNames[Random.Range (0, AvatarData.eyesColorNames.Length)]);
		CheckFaceCoherence (skinColor);
	}

	//METODO PER CONTROLLARE CHE LA FACCIA SIA COERENTE UN MINIMO
	void CheckFaceCoherence (string skinColor)
	{
		if (skinColor == "Dark")
		{
			AssignEyesColor ("Black");
			AssignHairColor ("Black");
		}
		else if (skinColor == "Brown")
		{
			AssignHairColor ("DarkBrown");
			AssignEyesColor ("Black");
		}
	}

	//METODO CHE ASSEGNA UN'EMOZIONE ALLA FACCIA CAMBIANDO I COMPONENTI DA CAMBIARE: OCCHI, SOPRACCIGLIA E BOCCA
	public void AssignEmotion (Emotion emotion)
	{
		this.emotion = emotion;
		mouth.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Mouth/" + emotion.ToString ());
		/* eyebrow.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyebrow/" + emotion.ToString ());
		if (emotion == Emotion.Disgusto)
			eyes.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyes/" + emotion.ToString ()); */

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
		face.GetComponent<SpriteRenderer> ().color = AvatarData.skinColorDictionary[skinColorName];
		earInsideLeft.GetComponent<SpriteRenderer> ().color = AvatarData.skinColorDictionary[skinColorName];
		earInsideRight.GetComponent<SpriteRenderer> ().color = AvatarData.skinColorDictionary[skinColorName];
		nose.GetComponent<SpriteRenderer> ().color = AvatarData.skinColorDictionary[skinColorName];
	}

	public void AssignHairStyle (Gender gender, string hairName)
	{
		hair.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Hair/" + gender.ToString () + "/" + hairName);
	}

	public void AssignHairColor (string hairColorName)
	{
		hair.GetComponent<SpriteRenderer> ().color = AvatarData.hairColorDictionary[hairColorName];
		eyebrow.GetComponent<SpriteRenderer> ().color = AvatarData.hairColorDictionary[hairColorName];
	}

	public void AssignEyesColor (string eyesColorName)
	{
		eyes.GetComponent<SpriteRenderer> ().color = AvatarData.eyesColorDictionary[eyesColorName];
	}
}
