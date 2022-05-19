using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopState : Stat
{

    public AudioSource SFXworkshopDestroy;

    protected override void Init()
    {
        base.Init();

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
            this.gameObject.GetComponent<WorkshopController>().GetConnectEssence().GetComponent<EssenceSpot>()
                .SetRemainEssence(this.gameObject.GetComponent<WorkshopController>().GetRemainEssence());

            this.gameObject.GetComponent<WorkshopController>().GetConnectEssence().SetActive(true);

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
        renderer = this.gameObject.GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

       // SFXworkshopDestroy.Play();

        MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();
        mr.material.shader = Shader.Find("UI/Unlit/Transparent");

        float  f;
        Color c;

        for (int i = 50; i >= 0; i--)
        {
            f = i / 50.0f;


            c = this.gameObject.GetComponent<WorkshopController>().GetColor();
            c.a = f;
            propertyBlock.SetColor("_Color", c);
            renderer.SetPropertyBlock(propertyBlock);


            yield return new WaitForSeconds(0.05f);
        }

        this.gameObject.GetComponentInParent<Building>().ReturnBuildingPool();
    }


}
