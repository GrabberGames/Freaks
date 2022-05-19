using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopController : MonoBehaviour
{
   
   
     Color large = new Color(0, 0.01f, 0.12f, 1f);
     Color medium = new Color(0, 1f, 0.9f, 1f);
     Color small = new Color(1f, 1f, 1f, 1f);
   
    [SerializeField] private int RemainEssence;
    private int GetEssencePerOnce = 10;
    public GameObject connectEssence;

    Renderer renderer;
    MaterialPropertyBlock propertyBlock;
 



    public int GetRemainEssence()
    {
        return RemainEssence;
    }

    public GameObject GetConnectEssence()
    {
        return this.connectEssence;
    }
    public void StartDigging()
    {
        StartCoroutine(DiggleTest());
    }


    public void Digging()
    {
        RemainEssence -= GetEssencePerOnce;
        StageManager.Instance.AddEssence(GetEssencePerOnce);
        SetColor();
    }

    public Color GetColor()
    {

        if (RemainEssence >= 501)
        {
            return large;
        }
        else if (RemainEssence >= 201)
        {
            return medium;
        }
        else if (RemainEssence >= 1)
        {
            return small;
        }
        else
        {
            return small;
        }
    }

   
    public void SetColor()
    {
        if (RemainEssence >= 501)
        {
         
     
            propertyBlock.SetColor("_Color", large);
            renderer.SetPropertyBlock(propertyBlock);
        }
        else if (RemainEssence >= 201)
        {
            propertyBlock.SetColor("_Color", medium);
            renderer.SetPropertyBlock(propertyBlock);
        }
        else if (RemainEssence >= 1)
        {
            propertyBlock.SetColor("_Color", small);
            renderer.SetPropertyBlock(propertyBlock);
        }
        else
        {
            this.GetComponent<WorkshopState>().Disappear();
            connectEssence.gameObject.SetActive(false);
        }
    }

    public void SetConnectEssence(GameObject go)
    {
        renderer = this.gameObject.GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        connectEssence = go;
        this.RemainEssence = go.GetComponent<EssenceSpot>().GetRemainEssence();
        go.SetActive(false);
        SetColor();

       
    }

    IEnumerator DiggleTest()
    {
        float startTime = Time.time;

        while (RemainEssence >= 0)
        {
            if (RemainEssence == 0)
            {
                this.GetComponent<WorkshopState>().Disappear();
                connectEssence.gameObject.SetActive(false);
                break;
            }

            if (Time.time - startTime >= 5f)
            {
                Digging();
                startTime = Time.time;
            }
            yield return null;
        }
    }
}
