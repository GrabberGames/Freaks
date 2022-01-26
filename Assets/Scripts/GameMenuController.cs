using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public void LoadScene(int indx) {
        SceneManager.LoadScene(indx);
    }

    
    public void GameQuit() {
        Application.Quit();
    }
}
