using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopStat : Stat
{
    

    protected override void Init()
    {
        base.Init();
        Debug.Log("workshop HP = " + HP);
    
       
    }
    void Start()
    {
        Init();
    }
    public override void DeadSignal()
    {
        if (HP <= 0)
        { 
            // StartCoroutine("FadeOut"); //fadeoutÇÒ¶§ ¾µ ÄÚµå
            Destroy(this.gameObject);
        }
    }
  




    IEnumerator FadeOut()
    {
        Debug.Log("2ÃÊÈÄ¿¡ fadeout ");
        yield return new WaitForSeconds(2f);
        Debug.Log(" fadeout !! ");

        Material[] materials = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;


        MeshRenderer mr = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        mr.materials[0].shader = Shader.Find("UI/Unlit/Transparent");
        mr.materials[1].shader = Shader.Find("UI/Unlit/Transparent");
        mr.materials[2].shader = Shader.Find("UI/Unlit/Transparent");

        // materials[0] : ¿öÅ©¼¥ ÁöºØ , materials[1],[2] : ¿öÅ©¼¥ ±âµÕ
        for (int i = 30; i >= 0; i--)
        {


            float f = i / 30.0f;

            for (int j = 0; j < 3; j++)
            {
                float c_r = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.r;
                float c_g = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.g;
                float c_b = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.b;
                float c_a = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.a;
                Color c = new Color(c_r, c_g, c_b);
                c.a = f;

                transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color = c;
            }

            yield return new WaitForSeconds(0.03f);
        }

    }


}
