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
    public Material[] material_Switch;   // INDEX => ON: 0 || OFF: 1
    public bool isSwitchBtnActivate = false;

    private GameObject alter;
    private AlterController alterController;
    private BuildingPreview buildingPreview;
    private SpawnController spawnController;
    Vector3 pos;
    private bool isTimerON;
    private int isActivate = 3;


    // Start is called before the first frame update
    void Start()
    {
        switchObjects = new GameObject[] { transform.GetChild(0).gameObject, transform.GetChild(1).gameObject, transform.GetChild(2).gameObject};

        alter = GameObject.Find("Alter");
        alterController = alter.GetComponent<AlterController>();
        spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();
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


    public void SwitchFX(Vector3 workshopPos, string onOff)
    {
        GameObject parentObject;
        ParticleSystem onFx, offFx;
        // Vector3(-8.8653307,0,156.667099) 0 
        // Vector3(-28.9882011,0,-172.248169) 1 
        // Vector3(285.082642,0,-16.6710186) 2 
        if (workshopPos.x > 280f)
        {
            pos = switchObjects[2].transform.position;
            parentObject = switchObjects[2];
            onFx = parentObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
            offFx = parentObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        }
        else if (workshopPos.z > 0)
        {
            pos = switchObjects[0].transform.position;
            parentObject = switchObjects[0];
            onFx = parentObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
            offFx = parentObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        }
        else
        {
            pos = switchObjects[1].transform.position;
            parentObject = switchObjects[1];
            onFx = parentObject.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
            offFx = parentObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        }
        isActivate--;

        // switch ON FX
        pos.y = 0.5f; pos.x += 1.0f; // FX Pos. calibration

        if (onOff.Equals("ON"))
        {
            offFx.Pause();
            offFx.gameObject.SetActive(false);
            onFx.gameObject.SetActive(true);
            onFx.Play();

            parentObject.transform.GetChild(1).GetComponent<MeshRenderer>().material = material_Switch[1];
        }
        else if (onOff.Equals("OFF"))
        {
            onFx.Pause();
            onFx.gameObject.SetActive(false);
            offFx.gameObject.SetActive(true);
            offFx.Play();

            parentObject.transform.GetChild(1).GetComponent<MeshRenderer>().material = material_Switch[0];
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

        if(isActivate == 0)
        {
            spawnController.SetIsRageActivate(true);
        }
    }
}
