using System.Collections;
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
    WorkshopController wc;
    public override void DeadSignal()
    {
        if (isFirst == true)
        {
            wc = GetComponent<WorkshopController>();
            if (HP <= 0)
            {
                Disappear();
                if (wc.isPurify)
                {
                    wc.StopAllCoroutines();
                    BarPooling.instance.ReturnObject(wc.purifyBar);
                }
                  

                   

                if (wc.GetConnectingFreaks().HP <= 0) //(화이트프릭스가 거의 죽을때& 건물 딱 도착했을때의 오차를 위한 오류수정
                    wc.GetConnectingFreaks().HP = 10;
                wc.GetConnectingFreaks().gameObject.SetActive(true);
                wc.GetConnectingFreaks().hpBar.SetActive(true);
                wc.GetConnectingFreaks().SetDestination(BuildingManager.Instance.Alter, false);

                if (!wc.isPurify)
                    wc.GetConnectEssence().GetComponent<EssenceSpot>()
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

         SFXworkshopDestroy.Play();

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

        if (wc.isPurify)
            wc.connectEssence.gameObject.SetActive(true);

        BuildingPooling.instance.ReturnObject(this.gameObject);
        this.gameObject.SetActive(false);
       
        // GetComponentInParent<Building>().ReturnBuildingPool();
    }


}