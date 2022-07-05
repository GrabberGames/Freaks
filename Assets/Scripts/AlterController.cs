using System.Collections;
using UnityEngine;


public class AlterController : Stat, InterfaceRange
{

    [SerializeField] private GameObject VFXAlterDestroy;
    [SerializeField] private AudioSource SFXAlterDestroy;
    [SerializeField] private AudioSource SFXAlterComplete;

 

    private GameObject BuildRange;

    GameObject go;

    protected override void Init()
    {
        base.Init();
        go = this.gameObject;

    }

    private void Start()
    {
        Init();
        BuildRange = transform.GetChild(2).gameObject;
        BuildRange.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
    }
    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            StartCoroutine(AlterDestroy());

        }
    }

    IEnumerator AlterDestroy()
    {
        SFXAlterDestroy.Play();
        VFXAlterDestroy.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
        go.transform.GetChild(0).gameObject.SetActive(false);
        yield return YieldInstructionCache.WaitForSeconds(1.8f);
        go.SetActive(false);
        GameManager.Instance.DefeatShow();
 
    }
  
    public void BuildingRangeON(bool check)
    {
        if (check)
            BuildRange.SetActive(true);
        else
            BuildRange.SetActive(false);
    }

}