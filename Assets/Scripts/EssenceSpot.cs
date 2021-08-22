using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceSpot : MonoBehaviour
{
    public int RemainEssence;

    [SerializeField] private int GetEssencePerOnce = 50;
    
    private Material navy, blue, sky_blue;
    private Material material;


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
    }


    public int GetRemainEssence()
    {
        return RemainEssence;
    }


    public Material GetNowMaterial()
    {
        return gameObject.GetComponent<Renderer>().material;
    }


    public void SetRemainEssence(int variance)
    {
        RemainEssence -= variance;
    }


    public void Digging()
    {
        RemainEssence -= GetEssencePerOnce;
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