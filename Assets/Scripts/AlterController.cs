using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class AlterController :  MonoBehaviour,DamageService, HealthService, InterfaceRange
{
    public GameObject whiteFreaksPref;
    public TextMeshProUGUI wFreaksCount;
    public TextMeshProUGUI mineCount;
    public GameObject VFXAlterDestroy;
    public AudioSource SFXAlterDestroy;

    public float healthPoint = 2000.0f;
    public int essence = 1000;

    [SerializeField] private int whiteFreaks = 5;
    [SerializeField] private bool isAlterClicked = false;

    private List<GameObject> miningFreaks = new List<GameObject>();

    private int busyWhiteF = 0;

    private GameObject BuildRange;

    private void Start() //오브젝트 풀링에서 알터를 설정해주는 함수입니다.
    {
        if (ObjectPooling.instance.Alter == null)
            ObjectPooling.instance.Alter_Setting(this.gameObject);


        BuildRange = this.gameObject.transform.GetChild(2).gameObject;
        BuildRange.SetActive(false); //건설가능범위 비활성화

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
        yield return new WaitForSeconds(2.5f);
        Destroy(transform.GetChild(0).gameObject);
    }

    public Vector3 getAlterPosition()
    {
        return this.gameObject.transform.position;
    }

    public float getAlterRange()
    {
        return this.gameObject.GetComponent<SphereCollider>().radius;
    }

    public void AlterRangeON()
    {
        BuildRange.SetActive(true);
    }

    public void AlterRangeOFF()
    {
        BuildRange.SetActive(false);
    }

    public void BuildingRangeON(bool check)
    {
        if (check)
            BuildRange.SetActive(true);
        else
            BuildRange.SetActive(false);
    }

}