using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceSpot : MonoBehaviour
{
    public int RemainEssence;
    private GameObject connectWorkshop;
    [SerializeField] private int GetEssencePerOnce = 50;

    private Material navy, white, sky_blue;
    private Material material;


    void Start()
    {
        material = this.gameObject.GetComponent<Renderer>().material;
        navy = GetComponentInParent<EssenceManager>().materials[0];
        sky_blue = GetComponentInParent<EssenceManager>().materials[1];
        white = GetComponentInParent<EssenceManager>().materials[2];
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
        SetColor();
    }


    public void Digging()
    {
        RemainEssence -= GetEssencePerOnce;
    }


    void SetColor()
    {
        if (RemainEssence >= 501)
        {
            gameObject.GetComponent<Renderer>().material = navy;
            connectWorkshop.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0.01f, 0.12f, 1f);
        }
        else if (RemainEssence >= 201)
        {
            gameObject.GetComponent<Renderer>().material = sky_blue;
            connectWorkshop.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 1f, 0.9f, 1f);
        }
        else if (RemainEssence >= 1)
        {
            gameObject.GetComponent<Renderer>().material = white;
            connectWorkshop.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            gameObject.SetActive(false);
            connectWorkshop.gameObject.GetComponent<WorkshopStat>().Disappear();
        }
    }

   public  void SetConnectWorkshop(GameObject go)
    {
        connectWorkshop = go;
    }
}