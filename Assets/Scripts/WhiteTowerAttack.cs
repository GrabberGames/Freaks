using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTowerAttack : Stat, InterfaceRange
{

    public ParticleSystem fx_whiteTower;

    // Stat _stat;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;
    private GameObject NoBuildRange;

    private GameController gameController;
    private List<FreaksController> blackFreaks = new List<FreaksController>();
    bool isAttack = false;


    public AudioSource SFXWhiteTowerDestroy;
    public AudioSource SFXWhiteTowerAttack;


    protected override void Init()
    {
        base.Init();
        blackFreaks = gameController.GetAliveBlackFreaksList();
        // Debug.Log("base.HP : " + base.HP);
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 18.98f, transform.position.z - 0.29f);
        NoBuildRange = transform.GetChild(2).gameObject;

        if (ConstructionPreviewManager.Instance.isPreviewMode)
            NoBuildRange.SetActive(true);
        else
            NoBuildRange.SetActive(false);


    }
    public override void DeadSignal()
    {
        if (HP <= 0)
            StartCoroutine(Dissolve());

    }

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        Init();

    }

    //소리 확인하기위한 update문

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Dissolve());
            
        }*/



        for (int i = 0; i < blackFreaks.Count; i++)
        {


            if ((blackFreaks[i].gameObject.transform.position - transform.position).magnitude < 45f)
            {
                if (isAttack)
                    return;
                else
                {
                    StartCoroutine(FindInAttackRange(blackFreaks[i].gameObject));
                    isAttack = true;
                }
            }

        }



    }

    GameObject bullet;
    IEnumerator FindInAttackRange(GameObject blackFreaks)
    {

        bullet = BulletPooling.instance.GetObject("WhiteTowerBullet");
        bullet.GetComponent<WhiteTowerBullet>().InitSetting(PD, blackFreaks, bulletSpawnPosition);
        bullet.SetActive(true);
        fx_whiteTower.Play(true);

        yield return new WaitForSeconds(fx_whiteTower.main.startDelayMultiplier);

        fx_whiteTower.Play(false);

        yield return new WaitForSeconds(AttackPerSeconds - fx_whiteTower.main.startDelayMultiplier);
        isAttack = false;
    }
    


    IEnumerator Dissolve()
    {


        MeshRenderer Sr1 = transform.gameObject.GetComponent<MeshRenderer>();
        MeshRenderer Sr2 = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        float threshold1;
        float threshold2;

        threshold1 = Sr1.material.GetFloat("_Dissolve");
        threshold2 = Sr2.material.GetFloat("_Dissolve");

        SFXWhiteTowerDestroy.Play();
        for (int i = 60; i <= 100; i++)
        {
            threshold1 = i / 100f;
            Sr1.material.SetFloat("_Dissolve", threshold1);
            Sr2.material.SetFloat("_Dissolve", threshold1);



            yield return YieldInstructionCache.WaitForSeconds(0.03f);
        }


        GetComponentInParent<Building>().ReturnBuildingPool();

    }




    public void BuildingRangeON(bool check)
    {
        if (NoBuildRange != null)
        {
            if (check)
                NoBuildRange.SetActive(true);
            else
                NoBuildRange.SetActive(false);
        }
    }
}
