using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopConstructionPreview : BasePreviewObject
{
    Vector3 essencePosition;

    protected override void MovePreviewObject(Vector3 position)
    {
        if (!_canBuild.Equals(ChkConstructionArea(position)))
        {
            _canBuild = !canBuild;
            ChangePriviewObjectColor(canBuild);
        }

        if(_canBuild)
            transform.position = essencePosition;
        else
            transform.position = position;
    }

    protected override bool ChkConstructionArea()
    {
        if (Vector3.Distance(BuildingManager.Instance.GetAlterPosition(), tr.position) < BuildingManager.Instance.GetAlterBuildRadius() - (constructionAreaSize * 0.5f) &&
           Vector3.Distance(BuildingManager.Instance.GetAlterPosition(), tr.position) > BuildingManager.Instance.GetAlterNoBuildRadius() + (constructionAreaSize * 0.5f))
        {
            var essenceObj = Physics.OverlapSphere(transform.position, constructionAreaSize * 0.5f, (int)eLayerMask.Essence);

            if (!essenceObj.Length.Equals(0))
            {
                essencePosition = essenceObj[0].GetComponent<Transform>().position;
                _conditionConstructionPreview = eConditionConstructionPreview.Buildable;
                return true;
            }
            else
            {
                _conditionConstructionPreview = eConditionConstructionPreview.NearbyNoEssence;
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
            var essenceObj = Physics.OverlapSphere(position, constructionAreaSize * 0.5f, (int)eLayerMask.Essence);

            if (!essenceObj.Length.Equals(0))
            {
                essencePosition = essenceObj[0].GetComponent<Transform>().position;
                _conditionConstructionPreview = eConditionConstructionPreview.Buildable;
                return true;
            }
            else
            {
                _conditionConstructionPreview = eConditionConstructionPreview.NearbyNoEssence;
            }
        }
        else
        {
            _conditionConstructionPreview = eConditionConstructionPreview.NonBuildable;
        }
        return false;
    }


}
