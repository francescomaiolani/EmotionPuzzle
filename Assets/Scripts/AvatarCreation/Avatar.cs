using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
	[SerializeField]
	private GameObject face, eyes, hair, eyebrow, earInsideLeft, earInsideRight, nose, earRing, lashes, mouth, eyesLight;
	private Emotion emotion;
	private Gender gender;

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

	//METODO PER CREARE UNA FACCIA COMPLETA QUALUNQUE, NEL CASO IN CUI TU ABBIA 2 EMOZIONI DIVERSE PER OCCHI E BOCCA
	public void CreateCompleteFace (Emotion mouthEmotion, Emotion eyesEmotion, Gender gender, string skinColor, string hairStyle, string hairColor, string eyesColor)
	{
		AssignGender (gender.ToString ());
		AssignEmotion (mouthEmotion, eyesEmotion);
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
		gender = randomGender;

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
		eyebrow.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyebrow/" + gender.ToString () + emotion.ToString ());
		if (emotion == Emotion.Disgusto)
		{
			eyes.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyes/" + emotion.ToString ());
			eyesLight.SetActive (false);
			lashes.SetActive (false);
		}
		else
		{
			eyes.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyes/Default");
			eyesLight.SetActive (true);
		}
	}

	//METODO CHE ASSEGNA UN'EMOZIONE ALLA FACCIA CAMBIANDO I COMPONENTI DA CAMBIARE: OCCHI, SOPRACCIGLIA E BOCCA
	//USARE QUESTO IN CASO IN CUI SI ABBIA UN'EMOZIONE DIVERSA PER GLI OCCHI O LA BOCCA
	public void AssignEmotion (Emotion mouthEmotion, Emotion eyesEmotion)
	{
		mouth.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Mouth/" + mouthEmotion.ToString ());
		eyebrow.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyebrow/" + gender.ToString () + eyesEmotion.ToString ());
		if (eyesEmotion == Emotion.Disgusto)
		{
			eyes.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyes/" + eyesEmotion.ToString ());
			eyesLight.SetActive (false);
			lashes.SetActive (false);
		}
		else
		{
			eyes.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyes/Default");
			eyesLight.SetActive (true);
		}

	}
	//METODI CHE ASSEGNANO I VALORI GIUSTI ALLE COMPONENTI
	public void AssignGender (string gender)
	{
		if (gender == "Male")
			this.gender = Gender.Male;
		else
			this.gender = Gender.Female;

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
		eyebrow.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Avatar/Eyebrow/" + gender + emotion.ToString ());
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

	//metodo che cambia l'opacita' di tutta la faccia per quando hai sbagliato a indovinare
	public void ChangeFaceOpacity (float value)
	{
		GameObject[] componenti = new GameObject[] { face, eyes, hair, eyebrow, earInsideLeft, earInsideRight, nose, earRing, lashes, mouth, eyesLight };
		foreach (GameObject comp in componenti)
		{
			SpriteRenderer spr = comp.GetComponent<SpriteRenderer> ();
			spr.color = new Color (spr.color.r, spr.color.g, spr.color.b, value / 255);
		}
	}

	//nel composition Game serve disattivare alcune parti della faccia
	public void DeactivateFaceElements ()
	{
		eyes.SetActive (false);
		eyebrow.SetActive (false);
		eyesLight.SetActive (false);
		lashes.SetActive (false);
		mouth.SetActive (false);
	}

	//RENDE UN AVATAR UN SELECTABLE OBJECT AGGIUNGENDO UN  CIRCLE COLLIDER E UN SELECTABLEOBJECT COMPONENT
	public void MakeAvatarSelectableObject ()
	{
		transform.localScale = new Vector3 (1, 1, 1);
		this.gameObject.AddComponent<CircleCollider2D> ().radius = 0.5f;
		GetComponent<CircleCollider2D> ().isTrigger = true;
		gameObject.AddComponent<SelectableObject> ().SetEmotionType (emotion);
	}
}
