using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour {

    public void ChangeToMinigame1() {
        SceneManager.LoadSceneAsync("Minigame1");
    }

}
