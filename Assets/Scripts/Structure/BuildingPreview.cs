using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public enum BuildingNum
    {
        Alter,
        Tower,
        Workshop,
        Switch
    }

    [SerializeField] private int me;

    private bool areyouSure = false;
    private int distance = 0;

    private GameObject belowObject;
    private Building parentController;
    private Transform parent;

    private List<Collider> colliders = new List<Collider>();

    private void Update()
    {
        if (me == (int) BuildingNum.Alter)
        {
            SetMaterial(colliders.Count == 0);
        }

        if(me == (int) BuildingNum.Workshop)
        {
            // 건설 가능 지역 머티리얼 표현
            SetMaterial(areyouSure);
        }
    }


    /// <summary>
    /// 겹쳐 있는 오브젝트 리턴
    /// </summary>
    /// <returns></returns>
    public GameObject GetBelowObject()
    {
        return belowObject;
    }

    // 각 건물의 건설 조건에 따른 건설 가능/불가 설정
    #region OnTriggerEnter/Exit
    private void OnTriggerEnter(Collider other)
    {
        parent = other.transform.parent;
        switch (me)
        {
            case (int)BuildingNum.Alter:
                colliders.Add(other);
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (parent != null && (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large" || parent.name == "SwitchController"))
                {
                    belowObject = other.gameObject;
                }
                if (parent != null && (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large" || parent.name == "SwitchController"))
                {
                    areyouSure = true;
                }
                break;

            case (int)BuildingNum.Switch:
                if(parent != null && (parent.name == "Bridge_Top" || parent.name == "Bridge_Bottom" || parent.name == "Bridge_Right"))
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
                colliders.Remove(other);
                break;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (parent != null && (parent.name == "Small" || parent.name == "Medium" || parent.name == "Large" || parent.name == "Switch_N" || parent.name == "Switch_S" || parent.name == "Swtich_E"))
                {
                    areyouSure = false;
                }
                break;
            case (int)BuildingNum.Switch:
                if (parent != null && (parent.name == "Bridge_Top" || parent.name == "Bridge_Bottom" || parent.name == "Bridge_Right"))
                {
                    areyouSure = false;
                }
                break;
        }
    }
    #endregion

    /// <summary>
    /// 건설 가능 여부 리턴
    /// </summary>
    /// <returns></returns>
    public bool IsBuildable()
    {
        switch (me)
        {
            case (int)BuildingNum.Alter:
                return colliders.Count == 0;
            case (int)BuildingNum.Tower:
            case (int)BuildingNum.Workshop:
                if (belowObject != null)
                {
                    return areyouSure && (belowObject.transform.parent.name == "SwitchController" || belowObject.transform.parent.transform.parent.name == "Arrow");
                }
                else
                {
                    return false;
                }
            case (int)BuildingNum.Switch:
                return areyouSure;
        }
        return false;
    }


    /// <summary>
    /// 머티리얼 초기화
    /// </summary>
    /// <returns></returns>
    public void Init(int n)
    {
        me = n;
        switch (me)
        {
            case (int)BuildingNum.Alter:
                parentController = GetComponentInParent<AlterPreview>();
                break;
            case (int)BuildingNum.Tower:
                break;
            case (int)BuildingNum.Workshop:
                parentController = GetComponentInParent<WorkshopController>();
                break;
            case (int)BuildingNum.Switch:
                return;
        }
    }


    private void SetMaterial(bool isRed)
    {
        parentController.SetMaterial(isRed);
    }
}