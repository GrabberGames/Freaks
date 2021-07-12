using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopController : MonoBehaviour
{
    public int remiainEssense = 1000;
    private GameObject worker = null;

    [SerializeField]
    private float workerActiveDelay = 1.0f;

    private bool isWorkerEnter = false;

    public void SetMiningWorker(GameObject worker)
    {
        this.worker = worker;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isWorkerEnter && collision.transform.gameObject == worker)
        {
            isWorkerEnter = true;
            remiainEssense -= 10;
            worker.SetActive(false);
            StartCoroutine(SetActive());
            StartCoroutine(WorkerEnter());
        }
    }

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
}
