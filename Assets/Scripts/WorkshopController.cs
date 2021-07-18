using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopController : Building
{
    private enum MaterialNum
    {
        SmallRoof,
        MeduimRoof,
        LargeRoof,
        Leg,
        Green,
        Red
    }


    public int remainEssense = 1000;
    private GameObject worker = null;
    private Renderer roofRenderer;

    [SerializeField] 
    private Material[] materials;

    private float workerActiveDelay = 2.0f;

    private bool isWorkerEnter = false;
    private bool beConstructed = true;
    private int currentRoofNum;

    private void Update()
    {
        ChangeRoof();
    }

    public void SetMiningWorker(GameObject worker)
    {
        this.worker = worker;
    }

    private void Start()
    {
        currentRoofNum = GetRoofNum();
        roofRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isWorkerEnter && collision.transform.gameObject == worker)
        {
            isWorkerEnter = true;
            remainEssense -= 10;
            worker.SetActive(false);
            StartCoroutine(SetActive());
            StartCoroutine(WorkerEnter());

            if (beConstructed) ConstructBuilding();
        }
    }

    #region MiningCoroutine
    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(workerActiveDelay);

        worker.SetActive(true);
    }

    IEnumerator WorkerEnter()
    {
        yield return new WaitForSeconds(workerActiveDelay + 0.5f);
        isWorkerEnter = false;
    }
    #endregion


    public void ChangeRoof()
    {
        if (!IsRoofChange()) return;

        currentRoofNum = GetRoofNum();
        roofRenderer.material = materials[currentRoofNum];
    }

    private bool IsRoofChange()
    {
        return currentRoofNum != GetRoofNum();
    }

    private int GetRoofNum()
    {
        int roofNum = remainEssense / 500;
        if (roofNum > 2) roofNum = 2;
        return roofNum;
    }

    public override void SetOpacity(bool isTransparent)
    {
    }


    public override void SetMaterial(bool canBuild)
    {
        if (canBuild)
        {
            Material[] mats = { materials[(int)MaterialNum.Green], materials[(int)MaterialNum.Green], materials[(int)MaterialNum.Green] };

            roofRenderer.materials = mats;
        }
        else
        {
            Material[] mats = { materials[(int)MaterialNum.Red], materials[(int)MaterialNum.Red], materials[(int)MaterialNum.Red] };

            roofRenderer.materials = mats;
        }
    }

    private void ConstructBuilding()
    {
        Material[] mats = { materials[(int)MaterialNum.LargeRoof], materials[(int)MaterialNum.Leg], materials[(int)MaterialNum.Leg] };

        roofRenderer.materials = mats;
        beConstructed = false;
    }

}
