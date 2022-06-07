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

    private WhiteFreaksController ConnectingFreaks;


    public int GetRemainEssence()
    {
        return RemainEssence;
    }

    public GameObject GetConnectEssence()
    {
        return connectEssence;
    }
    public void StartDigging()
    {
        StartCoroutine(Diggle());
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
            GetComponent<WorkshopState>().Disappear();

            ConnectingFreaks.gameObject.SetActive(true);
            ConnectingFreaks.SetDestination(GameManager.Instance.Alter, false);

            connectEssence.gameObject.SetActive(false);
        }
    }

    public void SetConnectEssence(GameObject go)
    {
        renderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        connectEssence = go;
        RemainEssence = go.GetComponent<EssenceSpot>().GetRemainEssence();
        go.SetActive(false);
        SetColor();

       
    }

    IEnumerator Diggle()
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


    public void SetConnetingFreaks(WhiteFreaksController whiteFreaksController)
    {
        ConnectingFreaks = whiteFreaksController;
    }
}
