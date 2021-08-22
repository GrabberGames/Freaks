using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    //[SerializeField] private bool isTimerON = false;
    //[SerializeField] private bool isSwitchON = false;
    //private Renderer targetRenderer;

    
    private GameObject[] switchObjects;

    public GameObject switchObject;
    public ParticleSystem[] fx_Switch;   // INDEX => ON: 0 || OFF: 1
    public bool isSwitchBtnActivate = false;

    private GameObject alter;
    private AlterController alterController;
    private BuildingPreview buildingPreview;
    Vector3 pos;
    private bool isTimerON;



    // Start is called before the first frame update
    void Start()
    {
        switchObjects = new GameObject[] { transform.GetChild(0).gameObject, transform.GetChild(1).gameObject, transform.GetChild(2).gameObject};

        alter = GameObject.Find("Alter");
        alterController = alter.GetComponent<AlterController>();
        //targetRenderer = gameObject.GetComponent<MeshRenderer>();
    }

   
    // If button Clicked
    public void CreateNewSwitch()
    {
        isSwitchBtnActivate = true;
    }


    public void viewPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePos = hit.point;
            mousePos.y = -0.5f;
            switchObject.transform.position = mousePos;
        }
    }


    void SwitchControl()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))  
        {
            // BuildingPreview delete
            Destroy(switchObject.GetComponent<BuildingPreview>());

            // SwitchTimer enable
            switchObject.GetComponent<SwitchTimer>().enabled = true;

            // Switch Creating position Set.
            pos = hit.point; pos.y = -0.5f;

            // switch ON FX
            SwitchFX(pos, "ON");

            // switch Timer ON
            isSwitchBtnActivate = false;

            // switchFreaks Spawn 
            if (alterController.CanBuild())
            {
                alterController.GoBuild(switchObject);
            }
        }
    }

    public Vector3 Getpos()
    {
        pos.y = 0f;
        return pos;
    }


    void SwitchFX(Vector3 pos, string onOff)
    {
        Debug.Log("POS: " + pos);
        // switch ON FX
        pos.y = 0.5f; pos.x += 1.0f; // FX Pos. calibration

        if (onOff.Equals("ON"))
        {
            Instantiate(fx_Switch[0], pos, Quaternion.Euler(-90f, 0, 0));
            fx_Switch[0].Play(true);
        }
        else if (onOff.Equals("OFF"))
        {
            Instantiate(fx_Switch[1], pos, Quaternion.Euler(-90f, 0, 0));
            fx_Switch[1].Play(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isSwitchBtnActivate)
        {
            viewPreview();

            if (Input.GetMouseButtonDown(0) && buildingPreview.IsBuildable())
            {
                SwitchControl();
            }
        }
    }
}
