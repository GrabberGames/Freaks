using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    //[SerializeField] private bool isTimerON = false;
    //[SerializeField] private bool isSwitchON = false;
    //private Renderer targetRenderer;

    
    [SerializeField] private GameObject switchClone;

    public GameObject switchPre;
    public ParticleSystem[] fx_Switch;   // INDEX => ON: 0 || OFF: 1
    public bool isSwitchBtnActivate = false;

    private GameObject alter;
    private AlterController alterController;
    private SwitchTimer SwitchTimer;
    private Vector3 switchCloneStartPos;
    private bool isTimerON;


    // Start is called before the first frame update
    void Start()
    {
        alter = GameObject.Find("Alter");
        alterController = alter.GetComponent<AlterController>();
        switchCloneStartPos = new Vector3(343, 8, -100);
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
            //Debug.Log(hit.point);
            Vector3 mousePos = hit.point;
            switchClone.transform.position = mousePos;
        }
    }


    void SwitchControl()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))  
        {
            // Switch Creating position Set.
            Vector3 pos = hit.point; pos.y = -0.5f;
                       
            // switch prefab Spawn
            Instantiate(switchPre, pos, Quaternion.Euler(180f, 0, 0));

            // switch ON FX
            SwitchFX(pos, "ON");

            // clone position reset
            switchClone.transform.position = switchCloneStartPos;  

            // switch Timer ON
            SwitchTimer = switchPre.GetComponent<SwitchTimer>();
            SwitchTimer.isTimerON = true;
            isSwitchBtnActivate = false;


            // switchFreaks Spawn 
            if (alterController.CanBuild())
            {
                GameObject switchFreaks = Instantiate(alterController.whiteFreaksPref, alter.transform.position, alter.transform.rotation);
                WhiteFreaksController switchFreaksController = switchFreaks.GetComponent<WhiteFreaksController>();

                pos.y = 0f;
                switchFreaksController.SetSwitch(pos);
            }






        }
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

            if (Input.GetMouseButtonDown(0))
            {
                SwitchControl();
            }
        }




    }
}
