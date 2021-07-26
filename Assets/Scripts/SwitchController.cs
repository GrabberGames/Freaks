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
    public Material[] switchMats;
    private Renderer targetRenderer;
    private Vector3 switchCloneStartPos;
    private bool isTimerON;

    // Start is called before the first frame update
    void Start()
    {
        switchCloneStartPos = new Vector3(343, 8, -100);
        targetRenderer = gameObject.GetComponent<MeshRenderer>();
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
            Vector3 mousePos = hit.point;
            mousePos.y = 10;
            switchClone.transform.position = mousePos;
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
                Debug.Log("!!");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))  // Switch Creating
                {
                    Vector3 pos = hit.point;
                    Debug.Log(pos);
                    pos.y = 10;
                    Instantiate(switchPre, pos, Quaternion.identity);
                    switchClone.transform.position = switchCloneStartPos;
                    SwitchTimer = switchPre.GetComponent<SwitchTimer>();
                    SwitchTimer.isTimerON = true;
                    isSwitchBtnActivate = false;
                }
                
            }
        }

    }
}
