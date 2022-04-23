using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIController : MonoBehaviour
{
    public GameObject inGameWin;
    public GameObject DcheckWin;
    public GameObject[] menuWins;

    private bool isMain;
    private bool isHome;
    public bool isMenuON;

    private bool canActiveMenu;


    private void Update()
    {               
        // IF Menu Window is OFF && IF Player KeyDown the Escape; Menu Window ON
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isMenuON)
            {
                if(canActiveMenu)
                {
                    AudioManager.a_Instance.Stop();
                    Time.timeScale = 0; // Game Pause

                    isMenuON = true;
                    inGameWin.SetActive(true);   // Pause Window ON
                    menuWins[0].SetActive(true);
                }                
            }
            else
            {
                MenuWinOFF();
                AudioManager.a_Instance.Play();
            }
        }

        canActiveMenu = !ConstructionPreviewManager.Instance.isPreviewMode;
    }


    private void MenuWinAct(int target)
    {
        for (int i = 0; i < menuWins.Length; i++)
        {
            if (menuWins[i].activeSelf)
            {
                menuWins[i].SetActive(false);
            }
        }
        menuWins[target].SetActive(true);
    }


    public void MenuWinOFF()
    {
        Time.timeScale = 1;

        isMenuON = false;
        inGameWin.SetActive(false);

        for (int i = 0; i < menuWins.Length; i++) {
            menuWins[i].SetActive(false);
        }
    }


    public void DisplayWin()
    {
        MenuWinAct(0);
    }


    public void SoundWin()
    {
        MenuWinAct(1);
    }


    public void ExitWin()
    {
        MenuWinAct(2);
    }


    public void DchkWin()
    {
        DcheckWin.SetActive(true);
    }


    public void DchkWinOff()
    {
        DcheckWin.SetActive(false);
    }


    public void MainOn()
    {
        isMain = true;
    }


    public void HomeOn()
    {
        isHome = true;
    }

    
    public void ModeWindow()
    {
        Screen.SetResolution(Screen.height, Screen.width, false);
    }


    public void ModeFull()
    {
        Screen.SetResolution(Screen.height, Screen.width, true);
    }


    public void DchkClick()
    {
        if (isMain)
        {
            isMain = false;
            GameToExit();
        }
        else if (isHome)
        {
            isHome = false;
            GameToHome();
        }
    }


    private void GameToHome()
    {
        SceneManager.LoadScene("Intro");
    }


    private void GameToExit()
    {
        Application.Quit();
    }
}
