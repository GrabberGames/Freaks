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

    private void Start() //������Ʈ Ǯ������ ���͸� �������ִ� �Լ��Դϴ�.
    {
        //if (ObjectPooling.instance.Alter == null)
         //   ObjectPooling.instance.Alter_Setting(this.gameObject);


        BuildRange = this.gameObject.transform.GetChild(2).gameObject;
        BuildRange.SetActive(false); //�Ǽ����ɹ��� ��Ȱ��ȭ

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