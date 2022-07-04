using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIController : MonoBehaviour
{
    public GameObject inGameWin;
    public GameObject DcheckWin;
    public GameObject[] menuWins;

    [SerializeField] GameObject MainText;
    [SerializeField] GameObject ExitText;

    private bool isMain;
    private bool isExit;
    public bool isMenuON;

    private bool isFirst = true; //게임 최초시작

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
                    AudioManager.Instance.Stop();
                    Time.timeScale = 0; // Game Pause

                    isMenuON = true;
                    inGameWin.SetActive(true);   // Pause Window ON
                    menuWins[0].SetActive(true);
                }                
            }
            else
            {
                MenuWinOFF();
                AudioManager.Instance.Play();
            }
        }

        if(isFirst == false)
        canActiveMenu = !ConstructionPreviewManager.Instance.isPreviewMode;
        else
        {
            canActiveMenu = true;
        }
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
        MainText.SetActive(false);
        ExitText.SetActive(false);
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
        isMain = false;
        isExit = false;
        MainText.SetActive(false);
        ExitText.SetActive(false);
    }


    public void MainOn()
    {
        isMain = true;
        MainText.SetActive(true);
    }


    public void ExitOn()
    {
        isExit = true;
        ExitText.SetActive(true);
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
            GameToHome();
        }
        else if (isExit)
        {
            isExit = false;
            GameToExit();
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

    public void NotFirst()
    {
        isFirst = false;
    }

    public void SettingWindowON()
    {
        isMenuON = true;
        inGameWin.SetActive(true);   // Pause Window ON
        menuWins[0].SetActive(true);
    }
}
