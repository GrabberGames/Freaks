using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterBullet : MonoBehaviour
{
    public ParticleSystem[] fx_Alter;
    public GameObject[] fx_AlterPre;
    /*
    [HideInInspector]
    public GameObject enemy;
    */
    private GameObject enemy;

    private Vector3 enemyPos;
    private float BulletSpeed = 35f;

    private int State = 0;
    private bool isCrushed = false;
    float _damage;

    public void InitSetting(float damage , GameObject enemy, Vector3 bulletSpawnPosition)
    {
        _damage = damage;

        this.enemy = enemy;
        StartCoroutine(StartProjectile(0.4f));
        this.transform.position = bulletSpawnPosition;
       
    }
  
    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            BulletPooling.ReturnObject(this.gameObject);
            return;
        }
        enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y , enemy.transform.position.z);


        switch (State)
        {
            case 1:
                fx_Alter[0].transform.LookAt(enemyPos);
                transform.position += (enemyPos - transform.position).normalized * BulletSpeed * Time.deltaTime;
                transform.rotation = Quaternion.identity;
                break;
            case 2:
                transform.position += (enemyPos - transform.position).normalized * BulletSpeed * Time.deltaTime;
                transform.rotation = Quaternion.identity;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == enemy)
        {

            if (isCrushed == true || other == null)
            {
                return;
            }
            GameManager.Damage.OnAttacked(_damage, other.GetComponent<Stat>());
            fx_Alter[0].Pause();
            fx_Alter[0].Play(false);
            fx_AlterPre[0].SetActive(false);


            fx_AlterPre[1].SetActive(true);
            fx_Alter[1] = fx_AlterPre[1].GetComponent<ParticleSystem>();
            fx_Alter[1].Play(true);

            State = 2;
            StartCoroutine(DeleteThis());
            isCrushed = true;
        }

    }


    IEnumerator StartProjectile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        this.gameObject.SetActive(true);      
        fx_AlterPre[0].SetActive(true);
        fx_Alter[0] = fx_AlterPre[0].GetComponent<ParticleSystem>();
        fx_Alter[0].Play(true);
        State = 1;
    }
    IEnumerator DeleteThis()
    {
        yield return new WaitForSeconds(fx_Alter[1].main.startLifetimeMultiplier);
        fx_AlterPre[1].SetActive(false);
        State = 3;

        // SFXAlterBulletEx.Play();
        BulletPooling.ReturnObject(this.gameObject);
    }
}
