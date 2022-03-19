using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAbilitiesUI : MonoBehaviour
{
    [Header("Gauge Image(1, 2, 3 순서대로)")]
    [SerializeField] private Sprite[] gaugeSprite = new Sprite[3];

    [Header("Gauge Setting")]
    [SerializeField] private Image hpGauge;
    [SerializeField] private Image damageGauge;
    [SerializeField] private Image rangeGauge;
    [SerializeField] private Image speedGauge;

    [Header("Skill Icon Setting")]
    [SerializeField] private Image skillIconQ;
    [SerializeField] private Image skillIconW;
    [SerializeField] private Image skillIconE;
    [SerializeField] private Image skillIconR;

    public void SetAbilitiesUI(HeroInfomation heroInfomation)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        hpGauge.sprite = gaugeSprite[heroInfomation.hp - 1];
        damageGauge.sprite = gaugeSprite[heroInfomation.damage - 1];
        rangeGauge.sprite = gaugeSprite[heroInfomation.range - 1];
        speedGauge.sprite = gaugeSprite[heroInfomation.speed - 1];

        skillIconQ.sprite = heroInfomation.skillIconQ;
        skillIconW.sprite = heroInfomation.skillIconW;
        skillIconE.sprite = heroInfomation.skillIconE;
        skillIconR.sprite = heroInfomation.skillIconR;    
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
