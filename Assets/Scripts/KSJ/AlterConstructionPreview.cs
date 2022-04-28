using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterConstructionPreview : BasePreviewObject
{
    protected override void MovePreviewObject(Vector3 position)
    {
        if (!_canBuild.Equals(ChkConstructionArea(position)))
        {
            _canBuild = !canBuild;
            ChangePriviewObjectColor(canBuild);
        }

        transform.position = position;
    }

    protected override bool ChkConstructionArea()
    {
        if (Vector3.Distance(BuildingManager.Instance.GetAlterPosition(), tr.position) < BuildingManager.Instance.GetAlterBuildRadius() - (constructionAreaSize * 0.5f) &&
           Vector3.Distance(BuildingManager.Instance.GetAlterPosition(), tr.position) > BuildingManager.Instance.GetAlterNoBuildRadius() + (constructionAreaSize * 0.5f))
        {
            if (Physics.OverlapSphere(transform.position, constructionAreaSize * 0.5f, (int)eLayerMask.Building).Length.Equals(0))
            {
                _conditionConstructionPreview = eConditionConstructionPreview.Buildable;
                return true;
            }
            else
            {
                _conditionConstructionPreview = eConditionConstructionPreview.NearbyBuilding;
            }
        }
        else
        {
            _conditionConstructionPreview = eConditionConstructionPreview.NonBuildable;
        }
        return false;
    }

    protected override bool ChkConstructionArea(Vector3 position)
    {
        if (Vector3.Distance(BuildingManager.Instance.GetAlterPosition(), position) < BuildingManager.Instance.GetAlterBuildRadius() - (constructionAreaSize * 0.5f) &&
            Vector3.Distance(BuildingManager.Instance.GetAlterPosition(), position) > BuildingManager.Instance.GetAlterNoBuildRadius() + (constructionAreaSize * 0.5f))
        {
            if (Physics.OverlapSphere(position, constructionAreaSize * 0.5f, (int)eLayerMask.Building).Length.Equals(0))
            {
                _conditionConstructionPreview = eConditionConstructionPreview.Buildable;
                return true;
            }
            else
            {
                _conditionConstructionPreview = eConditionConstructionPreview.NearbyBuilding;
            }
        }
        else
        {
            _conditionConstructionPreview = eConditionConstructionPreview.NonBuildable;
        }
        return false;
    }
}
