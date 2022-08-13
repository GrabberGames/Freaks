using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConstructionPreviewController : MonoBehaviour
{
    [SerializeField] Canvas hudCanvas;
    [SerializeField] Canvas previewCanvas;

    private GraphicRaycaster grHUD;
    private GraphicRaycaster grPreview;
    private PointerEventData ped;

    List<RaycastResult> resultsHUD = new List<RaycastResult>();
    List<RaycastResult> resultsPreview = new List<RaycastResult>();

    private void Start()
    {
        grHUD = hudCanvas.GetComponent<GraphicRaycaster>();
        grPreview = previewCanvas.GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ConstructionPreviewManager.Instance.OnAlterConstructionPreview();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ConstructionPreviewManager.Instance.OnWhiteTowerConstructionPreview();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ConstructionPreviewManager.Instance.OnWorkshopConstructionPreview();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ConstructionPreviewManager.Instance.ConstructionPreview(false);
        }


        if (ConstructionPreviewManager.Instance.isPreviewMode)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ped = new PointerEventData(null);
                ped.position = Input.mousePosition;
                grHUD.Raycast(ped, resultsHUD);
                grPreview.Raycast(ped, resultsPreview);

                if (!resultsHUD.Count.Equals(0))
                {
                    Debug.Log("UIÅ¬¸¯");
                    ConstructionPreviewManager.Instance.ConstructionPreview(false);
                }
                else if(resultsPreview.Count.Equals(0))
                {
                    if (ConstructionPreviewManager.Instance.ChkConstructionArea())
                    {
                        ConstructionPreviewManager.Instance.ConstructionPreview(false);

                        switch(ConstructionPreviewManager.Instance.nowPreviewBuilding)
                        {
                            case eBuilding.Alter:
                                if (StageManager.Instance.UseEssence(600))
                                {
                                    BuildingManager.Instance.SetBuildPosition(ConstructionPreviewManager.Instance.nowPreviewBuilding, ConstructionPreviewManager.Instance.PreviewPosition());
                                }
                                else
                                {
                                    SystemMassage.Instance.PrintSystemMassage("error : Impossible resource situation(10001)");
                                }
                                break;

                            case eBuilding.WhiteTower:
                                if (StageManager.Instance.UseEssence(500))
                                {
                                    BuildingManager.Instance.SetBuildPosition(ConstructionPreviewManager.Instance.nowPreviewBuilding, ConstructionPreviewManager.Instance.PreviewPosition());
                                }
                                else
                                {
                                    SystemMassage.Instance.PrintSystemMassage("error : Impossible resource situation(10001)");
                                }
                                break;

                            case eBuilding.Workshop:
                                BuildingManager.Instance.SetBuildPosition(ConstructionPreviewManager.Instance.nowPreviewBuilding, ConstructionPreviewManager.Instance.PreviewPosition()
                                    ,Physics.OverlapSphere(ConstructionPreviewManager.Instance.PreviewPosition(), 1.0f,(int)eLayerMask.Essence)[0].gameObject);
                                break;

                            default:
                                break;
                        }                        
                    }
                    else
                        ConstructionPreviewManager.Instance.PrintMessage();
                }

                resultsHUD.Clear();
                resultsPreview.Clear();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ConstructionPreviewManager.Instance.ConstructionPreview(false);
        }
    }
}
