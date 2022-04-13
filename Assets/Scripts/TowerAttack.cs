using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : Stat
{

    public GameObject player;
    public ParticleSystem fx_blackTower;
    public ParticleSystem fx_hit;

  
    Stat _stat;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;


    bool isAttack = false;
    protected override void Init()
    {
        base.Init();

        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 18.98f, transform.position.z - 0.29f);
        
    }
    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            StartCoroutine("Dissolve"); //�ǹ��ر�
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        Init();
      

    }

    private void Update()
    {
        if(player == null)
        {
            if (GameManager.Instance.Player == null)
                return;
            else
                player = GameManager.Instance.Player;
        }
        if((player.transform.position - transform.position).magnitude <=  ATTACK_RANGE)
        {
            
            if (isAttack)
                return;
            else
            {
                StartCoroutine(FindInAttackRange());
                isAttack = true;
            }
        }
    }

    IEnumerator FindInAttackRange()
    {
        GameObject bullet = BulletPooling.GetObject("BlackTowerBullet");
        bullet.GetComponent<TowerBullet>().InitSetting(PD, bulletSpawnPosition);
        fx_blackTower.Play(true);

        yield return new WaitForSeconds(fx_blackTower.main.startDelayMultiplier);

        fx_blackTower.Play(false);

        yield return new WaitForSeconds(AttackPerSeconds - fx_blackTower.main.startDelayMultiplier);
        isAttack = false;
    }

    IEnumerator Dissolve()
    {

        MeshRenderer Sr1 = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        MeshRenderer Sr2 = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        float threshold1;
        float threshold2;

        threshold1 = Sr1.material.GetFloat("_Dissolve");
        threshold2 = Sr2.material.GetFloat("_Dissolve");
        
        for(int i=1;i<=100;i++)
        {
            threshold1 = i/ 100f;
            Sr1.material.SetFloat("_Dissolve", threshold1);
            Sr2.material.SetFloat("_Dissolve", threshold1);

            yield return new WaitForSeconds(0.01f);
        }
        Destroy(transform.GetChild(0).gameObject.transform.GetChild(0).gameObject); //��ƼŬ �ý��� ����
        Destroy(transform.GetChild(2).gameObject);
      
    }




    }
