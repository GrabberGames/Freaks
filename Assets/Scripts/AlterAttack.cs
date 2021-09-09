using System.Collections;
using UnityEngine;

public class AlterAttack : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletpre;
    public GameObject enemy;
    public ParticleSystem FX_Alter_Emit;
    public ParticleSystem FX_Alter_Smoke;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;


    void Start()
    {
        FindEnemy();
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
    }


    void FindEnemy()
    {
        enemy = GameObject.FindGameObjectWithTag("BlackFreaks");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks"))
        {
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
        bullet = Instantiate(bulletpre, bulletSpawnPosition, Quaternion.Euler(bulletSpawnPosition - enemy.transform.position));
        bullet.GetComponent<AlterBullet>().InitSetting(enemy);
        yield return new WaitForSeconds(FX_Alter_Smoke.main.startDelayMultiplier);
        FX_Alter_Smoke.Play(false);
        yield return new WaitForSeconds(AttackPerSeconds - FX_Alter_Emit.main.startDelayMultiplier);

        StartCoroutine(FindInAttackRange());
    }
}
