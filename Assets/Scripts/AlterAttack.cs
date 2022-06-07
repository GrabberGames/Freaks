using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlterAttack : Stat
{

    public ParticleSystem FX_Alter_Emit;
    public ParticleSystem FX_Alter_Smoke;

    public AudioSource SFXAlterAttack;
    //
    private GameController gameController;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;
    bool isAttack = false;


    private List<FreaksController> blackFreaks = new List<FreaksController>();


    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        Init();
    }


    protected override void Init()
    {
        base.Init();
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        blackFreaks = gameController.GetAliveBlackFreaksList();
    }




    void Update()
    {


        for (int i = 0; i < blackFreaks.Count; i++)
        {


            if ((blackFreaks[i].gameObject.transform.position - transform.position).magnitude < 30f)
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
    IEnumerator FindInAttackRange(GameObject enemy)
    {

        FX_Alter_Emit.Play(true);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        FX_Alter_Emit.Play(false);
        FX_Alter_Smoke.Play(true);
        FX_Alter_Smoke.transform.position = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        FX_Alter_Smoke.transform.rotation = Quaternion.Euler(FX_Alter_Smoke.transform.position - enemy.transform.position);


        SFXAlterAttack.Play();

        bullet= BulletPooling.instance.GetObject("AlterBullet");
        bullet.GetComponent<AlterBullet>().InitSetting(PD, enemy, bulletSpawnPosition);
        bullet.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(FX_Alter_Smoke.main.startDelayMultiplier);
        FX_Alter_Smoke.Play(false);
        yield return YieldInstructionCache.WaitForSeconds(AttackPerSeconds - FX_Alter_Emit.main.startDelayMultiplier);


        isAttack = false;

    }

    public void bulletSpawnNewSetting()
    {
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);

    }



}
