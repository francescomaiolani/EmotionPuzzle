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
	}

	private void Start ()
	{
		DontDestroyOnLoad (gameObject);
		AssignDefaultAvatarSettings ();
	}

	void AssignDefaultAvatarSettings ()
	{
		avatarSettings.gender = Gender.Male;
		avatarSettings.hairStyle = "Ciuffo";
		avatarSettings.eyesColor = "Black";
		avatarSettings.hairColor = "DarkBrown";
		avatarSettings.skinColor = "White";
	}

}
