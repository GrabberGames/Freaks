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


    private GameController gameController;

    Vector3 pos;
    private bool isTimerON;
    private int isActivate = 3;


    // Start is called before the first frame update
    void Start()
    {
        switchObjects = new GameObject[] { transform.GetChild(0).gameObject, transform.GetChild(1).gameObject, transform.GetChild(2).gameObject};

        alter = GameObject.Find("alter");
        alterController = alter.GetComponent<AlterController>();

        gameController = GameObject.FindObjectOfType<GameController>();
        //targetRenderer = gameObject.GetComponent<MeshRenderer>();
    }


    
    public Vector3 Getpos()
    {
        pos.y = 0f;
        return pos;
    }





}
