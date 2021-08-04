using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;




public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private RaycastHit hit;
    private string hitColliderName;

    int min = 0;
    int sec = 0;

    // FX
    [SerializeField] private ParticleSystem fx_Move;

    private void Start()
    {
        StartCoroutine(PlayTimer());
    }

    private void Update()
    {
        FXmovePlayer();
    }


    IEnumerator PlayTimer()
    {
        while(true)
        {
            timerText.text = string.Format("{0:D1}:{1:D2}", min, sec);
            
            sec++;
            
            if (sec > 59)
            {
                sec = 0;
                min++;
            }
            
            yield return new WaitForSeconds(1);
        } 
    }

    // FX Play on Mouse Click pos.
    private void FXmovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Vector3 mPos;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                mPos = hit.point; mPos.y = 0.3f;
                fx_Move.transform.position = mPos;
                fx_Move.Play(true);
            }
        }

        if (fx_Move.isStopped)
        {
            fx_Move.Stop();
        }
    }

    // 삭제 예정
    //public void ObjectMove(NavMeshAgent agent)
    //{
    //    if (Input.GetMouseButtonDown(0))    // Get Hero's name
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        {
    //            Debug.Log(hit.transform.gameObject.name);
    //            hitColliderName = hit.transform.gameObject.name;
    //        }
    //    }

    //    if (Input.GetMouseButtonDown(1))    // Right Mouse Click && Hero Clicked
    //    {
    //        Debug.Log(hitColliderName);
    //        agent = GameObject.Find(hitColliderName).GetComponent<NavMeshAgent>();
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Ray Set; Mouse Pointer Position
    //        Debug.Log(ray);
    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        {
    //            agent.SetDestination(hit.point); // Hero Move
    //        }
    //    }
    //}
}
