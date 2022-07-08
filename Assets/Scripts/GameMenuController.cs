using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct HeroInfomation
{
    public string heroName;
    public Sprite heroStandImage;
    [Range(1, 3)]
    public int hp;
    [Range(1, 3)]
    public int damage;
    [Range(1, 3)]
    public int range;
    [Range(1, 3)]
    public int speed;
    public Sprite skillIconQ;
    public Sprite skillIconW;
    public Sprite skillIconE;
    public Sprite skillIconR;
}

public enum eHeroType { None = 0 , Waron= 1, Kyle = 2 }

public class GameMenuController : MonoBehaviour
{
    public enum eCanvasList { Main, Play, HeroSelect, Creadit, Setting, MaxCount }

    [Header("UI Canvas List")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject heroSelect;
    [SerializeField] private GameObject creadit;
    [SerializeField] private GameObject setting;
    //[SerializeField] private GameObject heroSelect;

    [Header("Hero Info")]
    [SerializeField] private HeroInfomation warronInformation;
    [SerializeField] private HeroInfomation kyleInformation;

    [Header("Hero Select UI Setting")]
    [SerializeField] private HeroAbilitiesUI heroAbilitiesUI;
    [SerializeField] private HeroSelectUI heroSelectUI;
    [SerializeField] private CreditUI creditUI;


    [Header("Scene Loader")]
    [SerializeField] private GameObject sceneLoaderUI;
    

    GameObject[] uiCanvasArray = new GameObject[(int)eCanvasList.MaxCount];

    eHeroType nowSelectHero;

    public void Start()
    {
        uiCanvasArray[(int)eCanvasList.Main] = main;
        uiCanvasArray[(int)eCanvasList.Play] = play;
        uiCanvasArray[(int)eCanvasList.HeroSelect] = heroSelect;
        uiCanvasArray[(int)eCanvasList.Creadit] = creadit;
        uiCanvasArray[(int)eCanvasList.Setting] = setting;

        OnEnableMainMenu();

        sceneLoaderUI.SetActive(false);
    }

    #region UI Canvas Controll

    public void OnEnablePlayMenu()
    {
        ActiveCanvas(eCanvasList.Play);
    }
    public void OnEnableMainMenu()
    {
        ActiveCanvas(eCanvasList.Main);
    }
    public void OnEnableHeroSelectMenu()
    {
        GameManager.Instance.SetSelectHero(eHeroType.Waron);
        ActiveCanvas(eCanvasList.HeroSelect);
        SelectWaron();
    }

    public void OnEnableCreditMenu()
    {
        uiCanvasArray[(int)eCanvasList.Creadit].SetActive(true);
    }

    public void OnEnableSettingMenu()
    {
        uiCanvasArray[(int)eCanvasList.Setting].SetActive(true);
    }

    public void ActiveCanvas(eCanvasList canvas)
    {
        for (int i = 0; i < uiCanvasArray.Length; i++)
        {
            uiCanvasArray[i].SetActive(i.Equals((int)canvas));
        }
    }

    #endregion

    #region Hero Select Controll
    public void SelectWaron()
    {
        heroAbilitiesUI.SetAbilitiesUI(warronInformation);
        heroSelectUI.SelectHero(eHeroType.Waron);
        GameManager.Instance.SetSelectHero(eHeroType.Waron);
    }
    public void SelectKyle()
    {
        heroAbilitiesUI.SetAbilitiesUI(kyleInformation);
        heroSelectUI.SelectHero(eHeroType.Kyle);
        GameManager.Instance.SetSelectHero(eHeroType.Kyle);
    }
    #endregion

    public void DisableCreditMenu()
    {
        creditUI.Disable();
    }


    public void LoadPlayScene()
    {
        sceneLoaderUI.SetActive(true);
        SceneLoader.Instance.LoadScene("Map");
        /*
        if (!GameManager.Instance.selectHero.Equals(eHeroType.None))
        {
            SceneManager.LoadScene("Map");
        }
        else
        {
            Debug.Log("error : Hero selection is required.");
        }*/
    }

    
    public void GameQuit() 
    {
        Application.Quit();
    }
}
