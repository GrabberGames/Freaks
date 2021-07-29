using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    //[SerializeField] private bool isTimerON = false;
    //[SerializeField] private bool isSwitchON = false;
    
    [SerializeField] private GameObject switchClone;
    public GameObject switchPre;
    private SwitchTimer SwitchTimer;

    public bool isSwitchBtnActivate = false;
    public ParticleSystem[] fx_Switch;   // INDEX => ON: 0 || OFF: 1
    //private Renderer targetRenderer;
    private Vector3 switchCloneStartPos;
    private bool isTimerON;

    // Start is called before the first frame update
    void Start()
    {
        switchCloneStartPos = new Vector3(343, 8, -100);
        //targetRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    /*
    IEnumerator SwitchTimer()
    {
        while (true)
        {
            if (timer > 0)
            {
                timer--;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                timer = 90;
                targetRenderer.material = switchMats[1];
                isSwitchON = true;
                break;
            }
        }
    }

    private void SwitchControll()
    {
        if (!isTimerON)
        {
            //StartCoroutine("SwitchTimer");
            isTimerON = true;
        }
    }

    */




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
            Debug.Log(hit.point);
            Vector3 mousePos = hit.point;
            switchClone.transform.position = mousePos;
        }
    }


    void SwitchControl()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))  // Switch Creating
        {
            Vector3 pos = hit.point; pos.y = -0.5f;
            Instantiate(switchPre, pos, Quaternion.Euler(180f, 0, 0));

            // switch ON FX
            SwitchFX(pos, "ON");

            switchClone.transform.position = switchCloneStartPos;   // clone position reset

            // switch Timer ON
            SwitchTimer = switchPre.GetComponent<SwitchTimer>();
            SwitchTimer.isTimerON = true;
            isSwitchBtnActivate = false;
        }
    }


    void SwitchFX(Vector3 pos, string onOff)
    {
        Debug.Log("POS: " + pos);
        // switch ON FX
        pos.y = 0.5f; pos.x += 0.8f; // FX Pos. calibration

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
