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

        SFXworkshopDestroy.Play();



        MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();


        float c_r, c_g, c_b, c_a, f;
        Color c;

        for (int i = 50; i >= 0; i--)
        {


            f = i / 50.0f;

            c_r = this.gameObject.GetComponent<MeshRenderer>().material.color.r;
            c_g = this.gameObject.GetComponent<MeshRenderer>().material.color.g;
            c_b = this.gameObject.GetComponent<MeshRenderer>().material.color.b;
            c_a = this.gameObject.GetComponent<MeshRenderer>().material.color.a;
            c = new Color(c_r, c_g, c_b);
            c.a = f;
            propertyBlock.SetColor("_Color", c);
            //this.gameObject.GetComponent<MeshRenderer>().material.color = c;

            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }


}
