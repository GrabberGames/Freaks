using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceSpot : MonoBehaviour
{
    [SerializeField] private int RemainEssence;
    Renderer renderer;
    MaterialPropertyBlock propertyBlock ;

    void Start()
    {
        renderer = this.gameObject.GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        SetColor();
    }


    public int GetRemainEssence()
    {
        return RemainEssence;
    }

    public void SetRemainEssence(int essence)
    {
        this.RemainEssence = essence;
        SetColor();
    }

    void SetColor()
    {
        if (RemainEssence >= 501)
        {
            propertyBlock.SetColor("_Color0", GetComponentInParent<EssenceManager>().colors[2, 0]);
            propertyBlock.SetColor("_Color1", GetComponentInParent<EssenceManager>().colors[2, 1]);
            renderer.SetPropertyBlock(propertyBlock);
        }
        else if (RemainEssence >= 201)
        {
            propertyBlock.SetColor("_Color0", GetComponentInParent<EssenceManager>().colors[1, 0]);
            propertyBlock.SetColor("_Color1", GetComponentInParent<EssenceManager>().colors[1, 1]);
            renderer.SetPropertyBlock(propertyBlock);
        }
        else if (RemainEssence >= 1)
        {
            propertyBlock.SetColor("_Color0", GetComponentInParent<EssenceManager>().colors[0, 0]);
            propertyBlock.SetColor("_Color1", GetComponentInParent<EssenceManager>().colors[0, 1]);
            renderer.SetPropertyBlock(propertyBlock);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    


}