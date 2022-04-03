using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTowerAttack : Stat
{

    public ParticleSystem fx_whiteTower;

    Stat _stat;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;
    

    bool isAttack = false;
    Vector3 getPos;

    public void SetPosition(Vector3 pos) //좌표 받아오는 함수
    {

        getPos = pos;
        //this.gameObject.SetActive(true);
    }


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

   

    private void OnTriggerEnter(Collider other) //사거리범위 내에 블랙freaks가 들어올 경우
    {
        if (other.gameObject.CompareTag ("BlackFreaks"))
        {
            if (isAttack)
                return;
            else
            {
                StartCoroutine(FindInAttackRange(other.gameObject));
       
                isAttack = true;
            }        
        }
    }
    
    private void OnTriggerStay(Collider other) //사거리범위 내에 블랙freaks가 들어와있을 경우
    {
        if (other.gameObject.CompareTag("BlackFreaks"))
        {
            if (isAttack)
                return;
            else
            {
                StartCoroutine(FindInAttackRange(other.gameObject));               
                isAttack = true;
            }
        }
    }

    
    IEnumerator FindInAttackRange(GameObject blackFreaks)
    {

        GameObject bullet = BulletPooling.GetObject("WhiteTowerBullet");
        bullet.GetComponent<WhiteTowerBullet>().InitSetting(PD, blackFreaks, bulletSpawnPosition);
     
        fx_whiteTower.Play(true);

        yield return new WaitForSeconds(fx_whiteTower.main.startDelayMultiplier);

        fx_whiteTower.Play(false);

        yield return new WaitForSeconds(AttackPerSeconds - fx_whiteTower.main.startDelayMultiplier);
        isAttack = false;
    }
    
    IEnumerator FadeOut()
    {
        Debug.Log("2초후에 fadeout ");
        yield return new WaitForSeconds(2f);
        Debug.Log(" fadeout !! ");

        MeshRenderer mr = transform.gameObject.GetComponent<MeshRenderer>();
        mr.material.shader = Shader.Find("UI/Unlit/Transparent");

    
        for (int i = 25; i >= 0; i--)
        {


            float f = i / 25.0f;

            float c_r = transform.gameObject.GetComponent<MeshRenderer>().material.color.r;
            float c_g = transform.gameObject.GetComponent<MeshRenderer>().material.color.g;
            float c_b = transform.gameObject.GetComponent<MeshRenderer>().material.color.b;
            float c_a = transform.gameObject.GetComponent<MeshRenderer>().material.color.a;
            Color c = new Color(c_r, c_g, c_b);
            c.a = f;


            transform.gameObject.GetComponent<MeshRenderer>().material.color = c;
          

            yield return new WaitForSeconds(0.03f);
        }
        Destroy(this.gameObject);

       
    }

}
