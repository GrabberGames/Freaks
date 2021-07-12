using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterController : MonoBehaviour
{
    public int essence = 1000;
    [SerializeField]
    private int workerFreeks = 5;
    private int busyWorkerF = 0;

    private List<GameObject> constructingBuilding = new List<GameObject>();
    private List<GameObject> collectedWorkshop= new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanBuild()
    {
        return workerFreeks - busyWorkerF > 0;
    }

    public void GoBuild(GameObject building)
    {
        busyWorkerF++;
    }

}
