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
        if (RemainEssence != essenceSpot.GetComponent<EssenceSpot>().RemainEssence)
        {

            RemainEssence = essenceSpot.GetComponent<EssenceSpot>().RemainEssence;
            if (RemainEssence >= 501)
            {
                this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0.01f, 0.12f, 1f);
            }
            else if (RemainEssence >= 201)
            {
                this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 1f, 0.9f, 1f);
            }
            else if (RemainEssence >= 1)
            {
                this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                this.gameObject.GetComponent<WorkshopStat>().Disappear();
            }
        }
    }

    public void SetEssenceSpot(GameObject go) //자원지 오브젝트 받아옴
    {
        essenceSpot = go;
    }

}
