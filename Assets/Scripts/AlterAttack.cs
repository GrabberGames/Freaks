using System.Collections;
using UnityEngine;

public class AlterAttack : Stat
{
    //public GameObject bullet;
    //public GameObject bulletpre;
    public GameObject enemy;
    public ParticleSystem FX_Alter_Emit;
    public ParticleSystem FX_Alter_Smoke;
    public AudioSource SFXAttackStart;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;


    void Start()
    {
        Init();
    }


    protected override void Init()
    {
        base.Init();
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z );

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks"))
        {
            enemy = other.gameObject;/*
            if (!SFXAttackStart.isPlaying)
            {
                SFXAttackStart.Play();
            }*/
            StartCoroutine(FindInAttackRange());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks"))
        {
            StopAllCoroutines();
        }
    }


    IEnumerator FindInAttackRange()
    {
        FX_Alter_Emit.Play(true);
        yield return new WaitForSeconds(1.45f);
        FX_Alter_Emit.Play(false);
        FX_Alter_Smoke.Play(true);
        FX_Alter_Smoke.transform.position = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        FX_Alter_Smoke.transform.rotation = Quaternion.Euler(FX_Alter_Smoke.transform.position - enemy.transform.position);
        /* 원래 총알 발사하던 코드
        bullet = Instantiate(bulletpre, bulletSpawnPosition, Quaternion.Euler(bulletSpawnPosition - enemy.transform.position));
        bullet.GetComponent<AlterBullet>().InitSetting(enemy);
        */
        GameObject bullet = BulletPooling.GetObject("AlterBullet");
        bullet.GetComponent<AlterBullet>().InitSetting(PD, enemy, bulletSpawnPosition);
        bullet.SetActive(true);

        yield return new WaitForSeconds(FX_Alter_Smoke.main.startDelayMultiplier);
        FX_Alter_Smoke.Play(false);
        yield return new WaitForSeconds(AttackPerSeconds - FX_Alter_Emit.main.startDelayMultiplier);

        StartCoroutine(FindInAttackRange());
    }
}
