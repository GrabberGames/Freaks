using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceSpot : MonoBehaviour
{
    public int RemainEssence;
    [SerializeField]
    private int GetEssencePerOnce = 50;
    private Material navy, blue, sky_blue;
    private Material material;
    private bool isDig = false;
    void Start()
    {
        material = this.gameObject.GetComponent<Renderer>().material;
        navy = GetComponentInParent<EssenceManager>().materials[0];
        blue = GetComponentInParent<EssenceManager>().materials[1];
        sky_blue = GetComponentInParent<EssenceManager>().materials[2];
    }
    // Update is called once per frame
    void Update()
    {
        SetColor();
        Digging();
    }
    void Digging()
    {
        if (isDig)
        {
            RemainEssence -= GetEssencePerOnce;
            isDig = false;
        }
    }
    void SetColor()
    {
        if(RemainEssence >= 801)
        {
            gameObject.GetComponent<Renderer>().material = navy;
        }
        else if(RemainEssence >= 401)
        {
            gameObject.GetComponent<Renderer>().material = blue;
        }
        else if(RemainEssence >= 1)
        {
            gameObject.GetComponent<Renderer>().material = sky_blue;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
