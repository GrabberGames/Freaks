using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterBullet : MonoBehaviour
{
    public ParticleSystem FX_Alter_Projectile_Pre;
    public GameObject FX_Alter_Hit;
    public GameObject FX_Alter_Hit_Pre;
    public AudioSource SFXAlterBulletEx;

    [HideInInspector]
    public GameObject enemy;

    private Vector3 enemyPos;
    private float BulletSpeed = 8f;
    
    private int State = 0;
    private bool One = false;
    // 1 = 투사체 날라가는 중
    // 2 = 블랙프릭스랑 충돌
    public void InitSetting(GameObject enemy)
    {
        this.enemy = enemy;
        StartProjectile();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks")  && !One)
        {
            State = 2;
            FX_Alter_Projectile_Pre.Play(false);
            FX_Alter_Hit = Instantiate(FX_Alter_Hit_Pre, transform);
            FX_Alter_Hit.AddComponent<Follow>().InitSetting(enemy);
            One = !One;
            StartCoroutine(DeleteThis());
        }
    }
    // Update is called once per frame
    void Update()
    {
        enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y , enemy.transform.position.z);
        transform.position -= (transform.position - enemyPos) * BulletSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(enemyPos - transform.position);
    }
    void StartProjectile()
    {
        State = 1;
        FX_Alter_Projectile_Pre.Play(true);
    }
    IEnumerator DeleteThis()
    {
        //print(FX_Alter_Hit_Pre.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        yield return new WaitForSeconds(FX_Alter_Hit_Pre.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        State = 3;
        SFXAlterBulletEx.Play();
        Destroy(this.gameObject);
    }
}
