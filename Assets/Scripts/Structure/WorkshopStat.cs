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
            Disappear();
        
        }
    }
    public void Disappear()
    {
        StartCoroutine("FadeOut"); //fadeout할때 쓸 코드
     
    }


    //소리 확인하기위한 update문
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Disappear();

        }
    }


    IEnumerator FadeOut()
    {
        SFXworkshopDestroy.Play();

     

        MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();


        float c_r, c_g, c_b, c_a,f;
        Color c;

        for (int i = 30; i >= 0; i--)
        {


             f = i / 30.0f;

                 c_r = this.gameObject.GetComponent<MeshRenderer>().material.color.r;
                 c_g = this.gameObject.GetComponent<MeshRenderer>().material.color.g;
                 c_b = this.gameObject.GetComponent<MeshRenderer>().material.color.b;
                 c_a = this.gameObject.GetComponent<MeshRenderer>().material.color.a;
                c = new Color(c_r, c_g, c_b);
                c.a = f;

                this.gameObject.GetComponent<MeshRenderer>().material.color = c;
   
            yield return new WaitForSeconds(0.03f);
        }
        Destroy(this.gameObject);
    }


}
