using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{


    Material switchMaterial;
    public AudioSource SFXswitchOn;

    float c_r, c_g, c_b, c_a, f;
    Color c;

    public IEnumerator SwitchPurify()
    {
      

        switchMaterial = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material;

        SFXswitchOn.Play();
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 30; i >= 0; i--)
        {


            f = i / 30.0f;

            c_r = switchMaterial.color.r;
            c_g = switchMaterial.color.g;
            c_b = switchMaterial.color.b;
            c_a = switchMaterial.color.a;
            c = new Color(c_r, c_g, c_b);
            c.a = f;

            switchMaterial.color = c;

            yield return YieldInstructionCache.WaitForSeconds(0.03f);

        }

        yield return null;

    }




}
