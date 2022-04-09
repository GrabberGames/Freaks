using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopConstructionPreview : BasePreviewObject
{
    protected override bool ChkConstructionArea()
    {
        if (Vector3.Distance(GameManager.getAlterPosition(), tr.position) < GameManager.getAlterBuildRadius() - (constructionAreaSize * 0.5f) &&
            Vector3.Distance(GameManager.getAlterPosition(), tr.position) > GameManager.getAlterNoBuildRadius() + (constructionAreaSize * 0.5f))
        {
            if (!Physics.OverlapSphere(transform.position, constructionAreaSize * 0.5f, (int)eLayerMask.Essence).Length.Equals(0))
            {
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
        if (Vector3.Distance(GameManager.getAlterPosition(), position) < GameManager.getAlterBuildRadius() - (constructionAreaSize * 0.5f) &&
            Vector3.Distance(GameManager.getAlterPosition(), position) > GameManager.getAlterNoBuildRadius() + (constructionAreaSize * 0.5f))
        {
            if (!Physics.OverlapSphere(transform.position, constructionAreaSize * 0.5f, (int)eLayerMask.Essence).Length.Equals(0))
            {
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
