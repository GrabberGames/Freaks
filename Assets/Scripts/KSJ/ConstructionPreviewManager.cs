using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eLayerMask
{
    Building = 1 << 9,
    Ground = 1 << 13,
    Essence = 1 << 14
}

public enum eConditionConstructionPreview
{
    Buildable, NearbyBuilding, NonBuildable, NearbyNoEssence
}

public enum eBuilding
{
    Alter, WhiteTower, Workshop, MaxCount
}

public class ConstructionPreviewManager : MonoBehaviour
{
    [Header ("Preview Object Setting")]
    [SerializeField] private BasePreviewObject _alter;
    [SerializeField] private BasePreviewObject _whiteTower;
    [SerializeField] private BasePreviewObject _workshop;

    [Header("Mouse Snap Setting")]
    [SerializeField] private float snapDistanceUnit;

    [Header("PreviewObject Color Setting")]
    [SerializeField] private Color _buildableColor;
    [SerializeField] private Color _nonBuildableColor;

    public BasePreviewObject alter { get => _alter; }
    public BasePreviewObject whiteTower { get => _whiteTower; }
    public BasePreviewObject workshop { get => _workshop; }




    private BasePreviewObject[] previewObjectArray = new BasePreviewObject[(int)eBuilding.MaxCount];
    private eBuilding _nowPreviewBuilding;

    private bool _isPreviewMode;


    public Color buildableColor { get => _buildableColor; }
    public Color nonBuildableColor { get => _nonBuildableColor; }
    public bool isPreviewMode { get => _isPreviewMode; }
    public eBuilding nowPreviewBuilding { get => _nowPreviewBuilding; }

    private Vector3 snapPosition;

    RaycastHit hit;
    Ray ray;

    IEnumerator previewPositioning;

    //singleton
    private static ConstructionPreviewManager mInstance;
    public static ConstructionPreviewManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<ConstructionPreviewManager>();
            }
            return mInstance;
        }
    }

    private void Awake()
    {
        previewObjectArray[(int)eBuilding.Alter] = alter;
        previewObjectArray[(int)eBuilding.WhiteTower] = whiteTower;
        previewObjectArray[(int)eBuilding.Workshop] = workshop;

        _isPreviewMode = false;
    }


    #region ��ư ���ۿ� �Լ� ����
    public void OnAlterConstructionPreview()
    {
        OnConstructionPreview(eBuilding.Alter);
    }

    public void OnWhiteTowerConstructionPreview()
    {
        OnConstructionPreview(eBuilding.WhiteTower);
    }

    public void OnWorkshopConstructionPreview()
    {
        OnConstructionPreview(eBuilding.Workshop);
    }

    public void OnConstructionPreview(eBuilding buildingType)
    {
        if (_nowPreviewBuilding.Equals(buildingType))
        {
            if (!isPreviewMode)
            {
                ConstructionPreview(true);
            }
        }
        else
        {
            if (isPreviewMode)
            {
                ConstructionPreview(false);
            }

            _nowPreviewBuilding = buildingType;
            ConstructionPreview(true);
        }
    }
    #endregion


    public void ConstructionPreview(bool value)
    {
        switch(value)
        {
            case true:
                if(!isPreviewMode)
                {
                    GameManager.Instance.Alter.GetComponent<AlterController>().AlterRangeON();

                    _isPreviewMode = true;
                    previewObjectArray[(int)_nowPreviewBuilding].SetActive(true);
                }                
                break;

            case false:
                if (isPreviewMode)
                {
                    GameManager.Instance.Alter.GetComponent<AlterController>().AlterRangeOFF();

                    _isPreviewMode = false;
                    previewObjectArray[(int)_nowPreviewBuilding].SetActive(false);
                }
                break;
        }
    }

    public Vector3 PreviewPosition()
    {
        return previewObjectArray[(int)_nowPreviewBuilding].PreviewPosition();
    }

    public bool ChkConstructionArea()
    {
        return previewObjectArray[(int)_nowPreviewBuilding].canBuild;
    }

    public void PrintMessage()
    {
        PrintMessage(previewObjectArray[(int)_nowPreviewBuilding].conditionConstructionPreview);
    }

    private void PrintMessage(eConditionConstructionPreview condition)
    {

        //���� �˸��޼��� �Է¿���
        switch (condition)
        {
            case eConditionConstructionPreview.Buildable:
                Debug.Log("�Ǽ� �����ѵ� �� ����� ����?");
                break;

            case eConditionConstructionPreview.NearbyBuilding:
                Debug.Log("�ٸ� �ǹ� �αٿ� �Ǽ��� �� �����ϴ�.");
                break;

            case eConditionConstructionPreview.NonBuildable:
                Debug.Log("�Ǽ� ������ ��ġ�� �ƴմϴ�.");
                break;

            case eConditionConstructionPreview.NearbyNoEssence:
                Debug.Log("�αٿ� �ڿ��� �����ϴ�.");
                break;

            default:
                break;
        }
    }

    #region ���콺 ��ġ ���� ����
    public Vector3 SnapPosition(Vector3 buildingPosition, Vector3 position)
    {
        if (buildingPosition.x > position.x)
        {
            if(position.x < 0.0f)
                snapPosition.x = ((int)((position.x + (snapDistanceUnit * 0.5f)) / snapDistanceUnit) - 0.5f) * snapDistanceUnit;
            else
                snapPosition.x = ((int)((position.x + (snapDistanceUnit * 0.5f)) / snapDistanceUnit) + 0.5f) * snapDistanceUnit;
        }
        else if (buildingPosition.x < position.x)
        {
            if (position.x < 0.0f)
                snapPosition.x = ((int)((position.x - (snapDistanceUnit * 0.5f)) / snapDistanceUnit) - 0.5f) * snapDistanceUnit;
            else
                snapPosition.x = ((int)((position.x - (snapDistanceUnit * 0.5f)) / snapDistanceUnit) + 0.5f) * snapDistanceUnit;
        }
        else
            snapPosition.x = buildingPosition.x;

        if (buildingPosition.z > position.z)
        {
            if (position.z < 0.0f)
                snapPosition.z = ((int)((position.z + (snapDistanceUnit * 0.5f)) / snapDistanceUnit) - 0.5f) * snapDistanceUnit;
            else
                snapPosition.z = ((int)((position.z + (snapDistanceUnit * 0.5f)) / snapDistanceUnit) + 0.5f) * snapDistanceUnit;
        }
        else if (buildingPosition.z < position.z)
        {
            if (position.z < 0.0f)
                snapPosition.z = ((int)((position.z - (snapDistanceUnit * 0.5f)) / snapDistanceUnit) - 0.5f) * snapDistanceUnit;
            else
                snapPosition.z = ((int)((position.z - (snapDistanceUnit * 0.5f)) / snapDistanceUnit) + 0.5f) * snapDistanceUnit;
        }
        else
            snapPosition.z = buildingPosition.z;

        snapPosition.y = position.y;

        return snapPosition;
    }
    #endregion
}
