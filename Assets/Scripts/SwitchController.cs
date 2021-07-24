using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private bool isTimerON = false;
    [SerializeField] private bool isSwitchON = false;
    [SerializeField] private int timer = 90;

    public Material[] switchMats;
    private Renderer targetRenderer;


    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = gameObject.GetComponent<MeshRenderer>();
    }


    IEnumerator SwitchTimer()
    {
        while(true)
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
        //viewPreview(); BuildingController.cs



        if (!isTimerON)
        {
            StartCoroutine("SwitchTimer");
            isTimerON = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        SwitchControll();
    }
}
