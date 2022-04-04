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
            StartCoroutine("FadeOut"); //fadeout할때 쓸 코드
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

    IEnumerator FadeOut()
    {
        Debug.Log("2초후에 fadeout ");
        yield return new WaitForSeconds(2f);
        Debug.Log(" fadeout !! ");

        MeshRenderer mr_c1 = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        mr_c1.material.shader = Shader.Find("UI/Unlit/Transparent");

        MeshRenderer mr_c2 = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        mr_c2.material.shader = Shader.Find("UI/Unlit/Transparent");

        for (int i = 25; i >= 0; i--)
            {
         

            float f = i /25.0f ;
        
                float c1_r = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.r;
                float c1_g = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.g;
                float c1_b = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.b;
                float c1_a = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.a;
            Color c1 = new Color(c1_r , c1_g , c1_b );
            c1.a = f;
     

                float c2_r = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color.r;
                float c2_g = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color.g;
                float c2_b = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color.b;
                float c2_a = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color.a;
            Color c2 = new Color(c2_r , c2_g , c2_b );
            c2.a = f ;
           

            
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = c1;
            transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = c2;
            
            yield return new WaitForSeconds(0.03f);
            }
        Destroy(transform.GetChild(0).gameObject.transform.GetChild(0).gameObject); //파티클 시스템 제거
        Destroy(transform.GetChild(2).gameObject);
    }
    

}
