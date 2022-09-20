using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnBuilding :  Stat
{
    GameObject go;
    private WhiteFreaksController ConnectingFreaks;
    private Building building;
    public int what; //0:alter, 1:whitetower
    void Start()
    {
        base.Init();
        SetHpBar();
        go = this.gameObject;
    }
    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            go.SetActive(false);
            hpBar.SetActive(false);
            ConnectingFreaks.gameObject.SetActive(true);
            ConnectingFreaks.hpBar.SetActive(true);
            ConnectingFreaks.SetDestination(BuildingManager.Instance.Alter, false);

            building.StopBuilding();
            BuildingPooling.instance.ReturnObject(go);
            BarPooling.instance.ReturnObject(hpBar);
        }
    }

    public void SetConnetingFreaks(WhiteFreaksController whiteFreaksController)
    {
        ConnectingFreaks = whiteFreaksController;
    }
    public void SetBuilding(Building building)
    {
        this.building = building;
    }


    public GameObject hpBar;
    public Vector3 hpBarOffset = new Vector3(0, 5, 0);
    private RectTransform rect;
    private Image hpBarImage;
    private Text hpBarText;
    void SetHpBar()
    {
        hpBar = BarPooling.instance.GetObject(BarPooling.bar_name.ally_bar);
        rect = (RectTransform)hpBar.transform;
        rect.sizeDelta = new Vector2(89, 21);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarImage.rectTransform.sizeDelta = rect.sizeDelta;
        hpBarText = hpBar.GetComponentsInChildren<Text>()[0];
        hpBarText.text = HP.ToString();
        var _hpbar = hpBar.GetComponent<HpBar>();
        _hpbar.target = this.gameObject;
        _hpbar.offset = hpBarOffset;
        if(what == 0)
            _hpbar.what = HpBar.targets.Alter;
        else
            _hpbar.what = HpBar.targets.WhiteTower;
    }

    public override void OnAttackSignal()
    {
        hpBarImage.fillAmount = HP / MAX_HP;
        if (HP >= 0)
            hpBarText.text = HP.ToString();
    }
}
