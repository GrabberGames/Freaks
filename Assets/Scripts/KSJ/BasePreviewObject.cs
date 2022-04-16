using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BasePreviewObject : MonoBehaviour
{
    protected Transform tr;
    protected GameObject go;

    protected List<Renderer> previewObject = new List<Renderer>();
    protected bool _canBuild;

    protected eConditionConstructionPreview _conditionConstructionPreview;

    public bool canBuild { get => _canBuild; }
    public bool activeSelf { get => gameObject.activeSelf; }
    public eConditionConstructionPreview conditionConstructionPreview { get => _conditionConstructionPreview; }


    [SerializeField] protected float constructionAreaSize;
    [SerializeField] protected Transform constructionAreaUI;

    IEnumerator updatePreviewObject;
    protected bool isPreviewMode;

    RaycastHit hit;
    Ray ray;

    protected void Awake()
    {
        tr = GetComponent<Transform>();
        go = gameObject;


        var tempRenderer = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < tempRenderer.Length; i++)
        {
            previewObject.Add(tempRenderer[i]);
        }

        constructionAreaUI.localScale = new Vector3(constructionAreaSize, constructionAreaSize, 1.0f);

        go.SetActive(false);
    }

    protected void Start()
    {

    }

    private IEnumerator UpdatePreviewObject()
    {
        while (true)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 5000.0f, (int)eLayerMask.Ground))
            {
                MovePreviewObject(ConstructionPreviewManager.Instance.SnapPosition(tr.position, hit.point));
            }

            if (!_canBuild.Equals(ChkConstructionArea()))
            {
                _canBuild = !canBuild;
                ChangePriviewObjectColor(canBuild);
            }

            yield return null;
        }
    }


    public void MovePreviewObject(Vector3 position)
    {
        if (!_canBuild.Equals(ChkConstructionArea(position)))
        {
            _canBuild = !canBuild;
            ChangePriviewObjectColor(canBuild);
        }

        transform.position = position;
    }


    public void SetActive(bool value)
    {
        go.SetActive(value);

        if (value)
        {
            _canBuild = ChkConstructionArea();
            ChangePriviewObjectColor(canBuild);
        }

        if(value)
        {
            updatePreviewObject = UpdatePreviewObject();
            StartCoroutine(updatePreviewObject);
        }
        else
        {
            StopCoroutine(updatePreviewObject);
            updatePreviewObject = null;

            _conditionConstructionPreview = eConditionConstructionPreview.NonBuildable;
        }

    }

    protected void ChangePriviewObjectColor(bool value)
    {
        if (value)
        {
            for (int i = 0; i < previewObject.Count; i++)
            {
                previewObject[i].material.color = ConstructionPreviewManager.Instance.buildableColor;
            }
        }
        else
        {
            for (int i = 0; i < previewObject.Count; i++)
            {
                previewObject[i].material.color = ConstructionPreviewManager.Instance.nonBuildableColor;
            }
        }
    }
    
    abstract protected bool ChkConstructionArea();
    abstract protected bool ChkConstructionArea(Vector3 position);

}
