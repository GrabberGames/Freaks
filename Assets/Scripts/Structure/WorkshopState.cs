﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopState : Stat
{

    public AudioSource SFXworkshopDestroy;
    public AudioSource SFXworkshopComplete;

    GameObject go;

    protected override void Init()
    {
        base.Init();
        go = this.gameObject;

    }
    void Start()
    {
        Init();
    }

    bool isFirst = true;
    // 죽는거

    public override void DeadSignal()
    {
        if (isFirst == true)
        {
            if (HP <= 0)
            {
                Disappear();


                GetComponent<WorkshopController>().GetConnectingFreaks().gameObject.SetActive(true);
                GetComponent<WorkshopController>().GetConnectingFreaks().SetDestination(BuildingManager.Instance.Alter, false);


                GetComponent<WorkshopController>().GetConnectEssence().GetComponent<EssenceSpot>()
                    .SetRemainEssence(GetComponent<WorkshopController>().GetRemainEssence());

                GetComponent<WorkshopController>().GetConnectEssence().SetActive(true);
                isFirst = false;


            }
        }
    }
    public void Disappear()
    {
        StartCoroutine(FadeOut()); 
    }


    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            HP = 0;
            DeadSignal();
        }
    }
    */
    Renderer renderer;
    MaterialPropertyBlock propertyBlock;
    IEnumerator FadeOut()
    {
        renderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        // SFXworkshopDestroy.Play();

        float f;
        Color c = GetComponent<WorkshopController>().GetColor();
        for (int i = 50; i >= 0; i--)
        {
            f = i / 50.0f;


            c.a = f;
            propertyBlock.SetColor("_Color", c);
            renderer.SetPropertyBlock(propertyBlock);

            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }
        BuildingPooling.instance.ReturnObject(this.gameObject);
        this.gameObject.SetActive(false);
        // GetComponentInParent<Building>().ReturnBuildingPool();
    }


}