using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionSettings : MonoBehaviour
{

	public AvatarSettings avatarSettings;

	//SINGLETON
	private static GameSessionSettings instance;

	private void Awake ()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (this.gameObject);

		if (avatarSettings.skinColor==null)
			AssignDefaultAvatarSettings ();

	}

	private void Start ()
	{
		DontDestroyOnLoad (gameObject);
	}

	void AssignDefaultAvatarSettings ()
	{
		avatarSettings.gender = Gender.Male;
		avatarSettings.skinColor = "White";
		avatarSettings.hairStyle = "Ciuffo";
		avatarSettings.eyesColor = "Black";
		avatarSettings.hairColor = "DarkBrown";
	}

}
