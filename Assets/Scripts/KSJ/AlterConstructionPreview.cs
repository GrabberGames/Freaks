using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterConstructionPreview : BasePreviewObject
{
    protected override bool ChkConstructionArea()
    {
        if (Physics.OverlapSphere(transform.position, constructionAreaSize * 0.5f, (int)eLayerMask.Building).Length.Equals(0))
        {
            _conditionConstructionPreview = eConditionConstructionPreview.Buildable;
            return true;
        }
        else
        {
            _conditionConstructionPreview = eConditionConstructionPreview.NearbyBuilding;
            return false;
        }
    }

    protected override bool ChkConstructionArea(Vector3 position)
    {
        if (Physics.OverlapSphere(position, constructionAreaSize * 0.5f, (int)eLayerMask.Building).Length.Equals(0))
        {
            _conditionConstructionPreview = eConditionConstructionPreview.Buildable;
            return true;
        }
        else
        {
            _conditionConstructionPreview = eConditionConstructionPreview.NearbyBuilding;
            return false;
        }
    }

}
