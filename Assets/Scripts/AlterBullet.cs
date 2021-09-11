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
    private float BulletSpeed = 1f;
    
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
            State = 2;
            FX_Alter_Projectile_Pre.Play(false);
            FX_Alter_Hit_Pre.Play(true);
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
        yield return new WaitForSeconds(0.05f);
        State = 3;
        Destroy(FX_Alter_Hit);
        Destroy(FX_Alter_Projectile);
        Destroy(this.gameObject);
    }
}
