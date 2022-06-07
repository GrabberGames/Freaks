using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopState : Stat
{

    public AudioSource SFXworkshopDestroy;


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
            WhiteFreaksManager.Instance.ReturnWhiteFreaks(go);
            Disappear();
            GetComponent<WorkshopController>().GetConnectEssence().GetComponent<EssenceSpot>()
                .SetRemainEssence(GetComponent<WorkshopController>().GetRemainEssence());

            GetComponent<WorkshopController>().GetConnectEssence().SetActive(true);
       

        }
    }
    public void Disappear()
    {
        StartCoroutine(FadeOut()); //fadeout할때 쓸 코드
    }


    //소리 확인하기위한 update문
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Disappear();
        }
    }

    Renderer renderer;
    MaterialPropertyBlock propertyBlock;
    IEnumerator FadeOut()
    {
        renderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

       // SFXworkshopDestroy.Play();

        float  f;
        Color c = GetComponent<WorkshopController>().GetColor();
        for (int i = 50; i >= 0; i--)
        {
            f = i / 50.0f;


            c.a = f;
            propertyBlock.SetColor("_Color", c);
            renderer.SetPropertyBlock(propertyBlock);

            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }

       GetComponentInParent<Building>().ReturnBuildingPool();
    }


}
