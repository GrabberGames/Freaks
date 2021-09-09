using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterBullet : MonoBehaviour
{
    public ParticleSystem FX_Alter_Projectile;
    public ParticleSystem FX_Alter_Projectile_Pre;
    public ParticleSystem FX_Alter_Hit;
    public ParticleSystem FX_Alter_Hit_Pre;

    [HideInInspector]
    public GameObject enemy;

    private Vector3 enemyPos;
    private float BulletSpeed = 8000f;
    
    private int State = 0;
    // 1 = 투사체 날라가는 중
    // 2 = 블랙프릭스랑 충돌
    public void InitSetting(GameObject enemy)
    {
        this.enemy = enemy;
        StartProjectile();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BlackFreaks"))
        {
            FX_Alter_Projectile.Play(false);
            FX_Alter_Hit = Instantiate(FX_Alter_Hit_Pre);
            FX_Alter_Hit.transform.SetParent(this.gameObject.transform);
            FX_Alter_Hit.transform.position = gameObject.transform.position;
            FX_Alter_Hit.transform.rotation = Quaternion.LookRotation(enemyPos - FX_Alter_Hit.transform.position);
            FX_Alter_Hit.Play(true);
            State = 2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1.5f, enemy.transform.position.z);
        switch(State)
        {
            case 1: //투사체 날라가는 중
                FX_Alter_Projectile.transform.position = transform.position;
                FX_Alter_Projectile.transform.rotation = Quaternion.LookRotation(enemyPos - FX_Alter_Projectile.transform.position);
                transform.position -= (transform.position - enemyPos) * BulletSpeed * Time.deltaTime;
                transform.rotation = Quaternion.identity;
                break;
            case 2: // 투사체 피격
                FX_Alter_Hit.transform.position = transform.position;
                break;
        }
    }
    void StartProjectile()
    {
        FX_Alter_Projectile = Instantiate(FX_Alter_Projectile_Pre);
        FX_Alter_Projectile.transform.SetParent(this.gameObject.transform);
        FX_Alter_Projectile.transform.position = transform.position;
        FX_Alter_Projectile.transform.rotation = Quaternion.LookRotation(enemyPos - FX_Alter_Projectile.transform.position);
        FX_Alter_Projectile.Play(true);
        State = 1;
    }
    IEnumerator DeleteThis()
    {

        yield return new WaitForSeconds(FX_Alter_Hit.main.startLifetimeMultiplier);
        State = 3;
        Destroy(FX_Alter_Hit);
        Destroy(FX_Alter_Projectile);
        Destroy(this.gameObject);
    }
}
