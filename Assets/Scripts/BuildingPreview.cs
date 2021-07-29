using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    private enum BuildingNum
    {
        Alter,
        Tower,
        Workshop
    }

    private List<Collider> colliders = new List<Collider>();
    private GameObject belowObject;
    private int me;
    private bool areyouSure = false;
    private bool cantbuild;
    private int distance = 0;

    private Building parentController;
    private WorkshopController workshopController;
    Transform parent;


    private void Awake()
    {
        workshopController = GetComponentInParent<WorkshopController>();
    }


    void Update()
    {
        if (me == 0)    //알터의 경우
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 Pos = new Vector3(hit.point.x, 0, hit.point.z);
                distance = (int)(Vector3.Distance(GameObject.Find("Alter").transform.position, hit.point));
            }
        }
        if(me == 2)     //워크샵의 경우
        {
            SetMaterial(areyouSure);
        }
    }


    public GameObject GetBelowObject()
    {
        return belowObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        parent = other.transform.parent;

        switch (me)
        {
            case (int)BuildingNum.Alter:
                if (parent !=  null && parent.name == "Road")
                    belowObject = parent.gameObject;
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (parent != null && (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large"))
                    belowObject = other.gameObject;
                if (parent != null && (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large"))
                {
                    areyouSure = true;
                }
                break;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        parent = other.transform.parent;
        switch (me)
        {
            case (int)BuildingNum.Alter:
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (parent != null && (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large"))
                {
                    areyouSure = false;
                }
                break;
        }
    }


    public bool IsBuildable()
    {
        switch (me)
        {
            case (int)BuildingNum.Alter:
                if (belowObject != null) { 
                    return distance <= 69 && belowObject.transform.name == "Road";
                }       
                else
                {
                    return false;
                }
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (belowObject != null)
                {
                    return areyouSure && belowObject.transform.parent.transform.parent.name == "Arrow";
                }
                else
                    return false;

        }
        return false;
    }


    public void Init(int n)
    {
        me = n;
        switch (me)
        {
            case (int)BuildingNum.Alter:
                parentController = GetComponentInParent<AlterController>();
                break;

            case (int)BuildingNum.Tower:
                break;

            case (int)BuildingNum.Workshop:
                parentController = GetComponentInParent<WorkshopController>();
                break;
        }

        parentController.SetOpacity(true);
    }


    private void SetMaterial(bool isRed)
    {
        parentController.SetMaterial(isRed);
    }


    public void Destroy()
    {
        parentController.SetOpacity(false);
    }
}
