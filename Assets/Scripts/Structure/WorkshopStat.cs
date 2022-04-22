using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopStat : Stat
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
            StartCoroutine("FadeOut"); //fadeout할때 쓸 코드
            Destroy(this.gameObject);
        }
    }


    //소리 확인하기위한 update문
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine("FadeOut");

        }
    }


    IEnumerator FadeOut()
    {
        SFXworkshopDestroy.Play();

        Material[] materials = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;


        MeshRenderer mr = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        mr.materials[0].shader = Shader.Find("UI/Unlit/Transparent");
        mr.materials[1].shader = Shader.Find("UI/Unlit/Transparent");
        mr.materials[2].shader = Shader.Find("UI/Unlit/Transparent");

        float c_r, c_g, c_b, c_a,f;
        Color c;
        // materials[0] : 워크샵 지붕 , materials[1],[2] : 워크샵 기둥
        for (int i = 30; i >= 0; i--)
        {


             f = i / 30.0f;

            for (int j = 0; j < 3; j++)
            {
                 c_r = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.r;
                 c_g = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.g;
                 c_b = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.b;
                 c_a = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.a;
                c = new Color(c_r, c_g, c_b);
                c.a = f;

                transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color = c;
            }

            yield return new WaitForSeconds(0.03f);
        }

    }


}
