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
    public override void DeadSignal()
    {
        if (HP <= 0)
        {    
           Disappear();


            GetComponent<WorkshopController>().GetConnectingFreaks().gameObject.SetActive(true);
            GetComponent<WorkshopController>().GetConnectingFreaks().SetDestination(GameManager.Instance.Alter, false);


            GetComponent<WorkshopController>().GetConnectEssence().GetComponent<EssenceSpot>()
                .SetRemainEssence(GetComponent<WorkshopController>().GetRemainEssence());

            GetComponent<WorkshopController>().GetConnectEssence().SetActive(true);



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
        // GetComponentInParent<Building>().ReturnBuildingPool();
    }


}