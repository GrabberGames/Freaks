using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class AlterController : MonoBehaviour, DamageService, HealthService, InterfaceRange
{
    public GameObject whiteFreaksPref;
    public TextMeshProUGUI wFreaksCount;
    public TextMeshProUGUI mineCount;
    public GameObject VFXAlterDestroy;
    public AudioSource SFXAlterDestroy;

    public float healthPoint = 2000.0f;
    public int essence = 1000;




    private int busyWhiteF = 0;

    private GameObject BuildRange;

    private void Start() 
    {
        BuildRange = transform.GetChild(2).gameObject;
        BuildRange.SetActive(false); 
        transform.GetChild(4).gameObject.SetActive(false);
    }
    
    public void DamageTaken(float damageTaken)
    {
        healthPoint -= damageTaken;
    }

    public float GetCurrentHP()
    {
        return healthPoint;
    }

    public void returnedBusyFreeks()
    {
        busyWhiteF--;
    }

    IEnumerator AlterDestroy()
    {
        SFXAlterDestroy.Play();
        Instantiate(VFXAlterDestroy);
        yield return YieldInstructionCache.WaitForSeconds(2.5f);
        Destroy(transform.GetChild(0).gameObject); //사라지게할것인가
    }

    public Vector3 getAlterPosition()
    {
        return transform.position;
    }

    public float getAlterRange()
    {
        return GetComponent<SphereCollider>().radius;
    }

    public void BuildingRangeON(bool check)
    {
        if (check)
            BuildRange.SetActive(true);
        else
            BuildRange.SetActive(false);
    }

}