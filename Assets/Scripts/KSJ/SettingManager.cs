using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    int screenWidth;
    int screenHeigth;
    bool isFullScreen;

    private static SettingManager mInstance;
    public static SettingManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<SettingManager>();
            }
            return mInstance;
        }
    }

    public void Start()
    {
        SetFullScreen();
    }

    public void SetFullScreen()
    {
        SetResolution(1920, 1080, true);
    }

    public void SetWindowScreen()
    {
        SetResolution(1280, 720, false);
    }


    public void SetResolution(int width, int heigth, bool fullscreen)
    {
        if(!screenWidth.Equals(width) || screenHeigth.Equals(heigth) || isFullScreen.Equals(fullscreen))
        {
            screenWidth = width;
            screenHeigth = heigth;
            isFullScreen = fullscreen;
            Screen.SetResolution(screenWidth, screenHeigth, isFullScreen);
        }
    }
    
    public void QuitApplication()
    {
        Application.Quit();
    }

}
