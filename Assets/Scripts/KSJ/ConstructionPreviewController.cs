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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ConstructionPreviewManager.Instance.OnAlterConstructionPreview();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ConstructionPreviewManager.Instance.OnWhiteTowerConstructionPreview();
        }

        if (Input.GetKeyDown(KeyCode.C))
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
                    Debug.Log("UI클릭");
                    ConstructionPreviewManager.Instance.ConstructionPreview(false);
                }
                else if(resultsPreview.Count.Equals(0))
                {
                    if (ConstructionPreviewManager.Instance.ChkConstructionArea())
                    {
                        ConstructionPreviewManager.Instance.ConstructionPreview(false);
                        if (StageManager.Instance.UseEssence(200))
                            BuildingManager.Instance.SetBuildPosition(ConstructionPreviewManager.Instance.nowPreviewBuilding, ConstructionPreviewManager.Instance.PreviewPosition());
                        else
                            SystemMassage.Instance.PrintSystemMassage("error : Impossible resource situation(1001)");

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
            Debug.Log("마우스 우클릭");
            ConstructionPreviewManager.Instance.ConstructionPreview(false);
        }
    }
}
