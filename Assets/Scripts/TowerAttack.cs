﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerAttack : Stat
{


    public GameObject player;
    public ParticleSystem fx_blackTower;
    public ParticleSystem fx_hit;

    public AudioSource SFXBlackTowerDestroy;
    public AudioSource SFXBlackTowerAttack;

    Stat _stat;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;
    private List<WhiteFreaksController> whiteFreaks = new List<WhiteFreaksController>();

    bool isAttack = false;
    protected override void Init()
    {
        base.Init();
        sqrATTACK_RANGE = ATTACK_RANGE * ATTACK_RANGE;
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 18.98f, transform.position.z - 0.29f);

        whiteFreaks = WhiteFreaksManager.Instance.GetWhiteFreaksList();

    }
    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            SFXBlackTowerDestroy.Play();
            StartCoroutine(Dissolve()); //�ǹ��ر�
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            BarPooling.instance.ReturnObject(hpBar);
        }
    }

    void Start()
    {
        Init();
        SetHpBar();

    }
    float sqrATTACK_RANGE ;
    


    GameObject bullet;
    IEnumerator FindInAttackRange()
    {
        bullet = BulletPooling.instance.GetObject("BlackTowerBullet");
        bullet.GetComponent<TowerBullet>().InitSetting(PD, bulletSpawnPosition);
        fx_blackTower.Play(true);
        SFXBlackTowerAttack.Play();

        yield return YieldInstructionCache.WaitForSeconds(fx_blackTower.main.startDelayMultiplier);
        fx_blackTower.Play(false);

        yield return YieldInstructionCache.WaitForSeconds(AttackPerSeconds - fx_blackTower.main.startDelayMultiplier);
        isAttack = false;
    }


    IEnumerator FindInAttackRange(GameObject enemy)
    {
        //Debug.Log("화이트프릭스한테 발사!");
        bullet = BulletPooling.instance.GetObject("BlackTowerBullet");
        bullet.GetComponent<TowerBullet>().InitSetting(PD, enemy, bulletSpawnPosition);
        fx_blackTower.Play(true);
        SFXBlackTowerAttack.Play();

        yield return YieldInstructionCache.WaitForSeconds(fx_blackTower.main.startDelayMultiplier);
        fx_blackTower.Play(false);

        yield return YieldInstructionCache.WaitForSeconds(AttackPerSeconds - fx_blackTower.main.startDelayMultiplier);
        isAttack = false;


    }

    IEnumerator Dissolve()
    {

        MeshRenderer Sr1 = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        MeshRenderer Sr2 = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        float threshold1;
        float threshold2;

        threshold1 = Sr1.material.GetFloat("_Dissolve");
        threshold2 = Sr2.material.GetFloat("_Dissolve");

        SFXBlackTowerDestroy.Play();
        for (int i = 60; i <= 100; i++)
        {
            threshold1 = i / 100f;
            Sr1.material.SetFloat("_Dissolve", threshold1);
            Sr2.material.SetFloat("_Dissolve", threshold1);



            yield return YieldInstructionCache.WaitForSeconds(0.03f);
        }
        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        // Destroy(transform.GetChild(0).gameObject.transform.GetChild(0).gameObject); //��ƼŬ �ý��� ����
        // Destroy(transform.GetChild(2).gameObject);

    }


    public GameObject hpBar;
    public Vector3 hpBarOffset = new Vector3(0, 10f, 0);
    private RectTransform rect;
    private Image hpBarImage;
    private Text hpBarText;
    void SetHpBar()
    {
        hpBar = BarPooling.instance.GetObject(BarPooling.bar_name.enemy_bar);
        rect = (RectTransform)hpBar.transform;
        rect.sizeDelta = new Vector2(100, 21);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarImage.rectTransform.sizeDelta = rect.sizeDelta;
        hpBarText = hpBar.GetComponentsInChildren<Text>()[0];
        hpBarText.text = HP.ToString();
        var _hpbar = hpBar.GetComponent<HpBar>();
        _hpbar.target = this.gameObject;
        _hpbar.offset = hpBarOffset;
        _hpbar.what = HpBar.targets.balckTower;
    }
    public override void OnAttackSignal()
    {
        hpBarImage.fillAmount = HP / MAX_HP;
        if (HP >= 0)
            hpBarText.text = HP.ToString();
    }



}