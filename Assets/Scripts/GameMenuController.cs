using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct HeroInfomation
{
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

public enum eHeroType { Waron, Kyle }

public class GameMenuController : MonoBehaviour
{
    public enum eCanvasList { Main, Play, HeroSelect }

    [Header("UI Canvas List")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject heroSelect;
    //[SerializeField] private GameObject heroSelect;

    [Header("Hero Info")]
    [SerializeField] private HeroInfomation waronInformation;
    [SerializeField] private HeroInfomation kyleInformation;

    [Header("Hero Select UI Setting")]
    [SerializeField] private HeroAbilitiesUI heroAbilitiesUI;
    [SerializeField] private HeroSelectUI heroSelectUI;

    GameObject[] uiCanvasArray = new GameObject[3];

    eHeroType nowSelectHero;

    public void Start()
    {
        uiCanvasArray[(int)eCanvasList.Main] = main;
        uiCanvasArray[(int)eCanvasList.Play] = play;
        uiCanvasArray[(int)eCanvasList.HeroSelect] = heroSelect;

        OnEnableMainMenu();        
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
        ActiveCanvas(eCanvasList.HeroSelect);
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
        heroAbilitiesUI.SetAbilitiesUI(waronInformation);
        heroSelectUI.SelectHero(eHeroType.Waron);
        nowSelectHero = eHeroType.Waron;
    }
    public void SelectKyle()
    {
        heroAbilitiesUI.SetAbilitiesUI(kyleInformation);
        heroSelectUI.SelectHero(eHeroType.Kyle);
        nowSelectHero = eHeroType.Kyle;
    }
    #endregion


    public void LoadScene(int indx) {
        SceneManager.LoadScene(indx);
    }

    
    public void GameQuit() {
        Application.Quit();
    }
}
