using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : Stat
{
    public GameObject bullet;
    public GameObject bulletpre;
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
        bullet = Instantiate(bulletpre, bulletSpawnPosition, Quaternion.Euler(bulletSpawnPosition - player.transform.position));
        bullet.GetComponent<TowerBullet>().InitSetting(PD);
        fx_blackTower.Play(true);

        yield return new WaitForSeconds(fx_blackTower.main.startDelayMultiplier);

        fx_blackTower.Play(false);

        yield return new WaitForSeconds(AttackPerSeconds - fx_blackTower.main.startDelayMultiplier);
        isAttack = false;
    }
}
