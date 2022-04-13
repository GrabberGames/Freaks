using System.Collections;
using UnityEngine;

public class AlterAttack : Stat
{
    //public GameObject bullet;
    //public GameObject bulletpre;
   // public GameObject enemy;
    public ParticleSystem FX_Alter_Emit;
    public ParticleSystem FX_Alter_Smoke;
    public AudioSource SFXAttackStart;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;
    bool isAttack = false;

    void Start()
    {
        Init();
    }


    protected override void Init()
    {
        base.Init();
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);

    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.transform.CompareTag("BlackFreaks"))
        {
            if (isAttack)
                return;

            else
            {
             /*
            if (!SFXAttackStart.isPlaying)
            {
                SFXAttackStart.Play();
            }*/
                StartCoroutine(FindInAttackRange(other.gameObject));
                isAttack = true;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks"))
        {

            if (isAttack)
                return;

            else
            {
          /*
            if (!SFXAttackStart.isPlaying)
            {
                SFXAttackStart.Play();
            }*/
                StartCoroutine(FindInAttackRange(other.gameObject));
                isAttack = true;
            }
        }
    }


    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks"))
        {
            StopAllCoroutines();
        }
    }
    */

    IEnumerator FindInAttackRange(GameObject enemy)
    {

        FX_Alter_Emit.Play(true);
        yield return new WaitForSeconds(1f); //¿ø·¡ 1.45f
        FX_Alter_Emit.Play(false);
        FX_Alter_Smoke.Play(true);
        FX_Alter_Smoke.transform.position = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        FX_Alter_Smoke.transform.rotation = Quaternion.Euler(FX_Alter_Smoke.transform.position - enemy.transform.position);
   

        GameObject bullet = BulletPooling.GetObject("AlterBullet");
        bullet.GetComponent<AlterBullet>().InitSetting(PD, enemy, bulletSpawnPosition);
        bullet.SetActive(true);

        yield return new WaitForSeconds(FX_Alter_Smoke.main.startDelayMultiplier);
        FX_Alter_Smoke.Play(false);
        yield return new WaitForSeconds(AttackPerSeconds - FX_Alter_Emit.main.startDelayMultiplier);

        isAttack = false;
      
    }
}
