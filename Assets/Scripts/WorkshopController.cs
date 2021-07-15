using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopController : MonoBehaviour
{
    public int remainEssense = 1000;
    private GameObject worker = null;
    private Renderer roofRenderer;

    [SerializeField]
    private Material[] roof;

    private float workerActiveDelay = 2.0f;

    private bool isWorkerEnter = false;
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
        currentRoofNum = getRoofNum();
        roofRenderer = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
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
        if (!isRoofChange()) return;

        currentRoofNum = getRoofNum();
        roofRenderer.material = roof[currentRoofNum];
    }

    private bool isRoofChange()
    {
        return currentRoofNum != getRoofNum();
    }

    private int getRoofNum()
    {
        int roofNum = remainEssense / 500;
        if (roofNum > 2) roofNum = 2;
        return roofNum;
    }
}
