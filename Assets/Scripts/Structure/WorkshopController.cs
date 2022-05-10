using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopController : MonoBehaviour
{

    public GameObject essenceSpot;
    private int RemainEssence;
    // Start is called before the first frame update
    void Start()
    {
        if (essenceSpot == null)
            return;

    }

    void Update()
    {
        if (essenceSpot == null)
            return;
        else
        SetColor();
    }

    void SetColor()
    {
        RemainEssence = essenceSpot.GetComponent<EssenceSpot>().RemainEssence;
        if (RemainEssence >= 501)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1/255f,3/255f,31/255f,255/255f);
        }
        else if (RemainEssence >= 201)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0/ 255f, 255 / 255f, 232 / 255f, 255 / 255f);
        }
        else if (RemainEssence >= 1)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(255/ 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }
        else
        {
            this.gameObject.GetComponent<WorkshopStat>().Disappear();
        }
    }

    public void SetEssenceSpot(GameObject go) //자원지 오브젝트 받아옴
    {
        essenceSpot = go;
    }

}
