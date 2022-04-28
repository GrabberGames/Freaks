using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTowerAttack : Stat, InterfaceRange
{

    public ParticleSystem fx_whiteTower;

    // Stat _stat;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;
    private GameObject NoBuildRange;

    bool isAttack = false;


    public AudioSource SFXWhiteTowerDestroy;
    public AudioSource SFXWhiteTowerAttack;



    protected override void Init()
    {
        base.Init();
        // Debug.Log("base.HP : " + base.HP);
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 18.98f, transform.position.z - 0.29f);
        NoBuildRange = transform.GetChild(2).gameObject;
        NoBuildRange.SetActive(false);

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

    //소리 확인하기위한 update문
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine("FadeOut");

        }
    }
    */
    private void OnTriggerEnter(Collider other) //사거리범위 내에 블랙freaks가 들어올 경우
    {
        if (other.gameObject.CompareTag("BlackFreaks"))
        {
            if (isAttack)
                return;
            else
            {
                SFXWhiteTowerAttack.Play();
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
                SFXWhiteTowerAttack.Play();
                StartCoroutine(FindInAttackRange(other.gameObject));
                isAttack = true;
            }
        }
    }


    IEnumerator FindInAttackRange(GameObject blackFreaks)
    {

        GameObject bullet = BulletPooling.GetObject("WhiteTowerBullet");
        bullet.GetComponent<WhiteTowerBullet>().InitSetting(PD, blackFreaks, bulletSpawnPosition);
        bullet.SetActive(true);
        fx_whiteTower.Play(true);

        yield return new WaitForSeconds(fx_whiteTower.main.startDelayMultiplier);

        fx_whiteTower.Play(false);

        yield return new WaitForSeconds(AttackPerSeconds - fx_whiteTower.main.startDelayMultiplier);
        isAttack = false;
    }

    IEnumerator FadeOut()
    {

        SFXWhiteTowerDestroy.Play();


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

    /*
    public void WhiteTowerRangeON()
        {
        NoBuildRange.SetActive(true);
        }

    public void WhiteTowerRangeOFF()
    {
        NoBuildRange.SetActive(false);
    }
    */
    public void BuildingRangeON(bool check)
    {
        if (NoBuildRange != null)
        {
            if (check)
                NoBuildRange.SetActive(true);
            else
                NoBuildRange.SetActive(false);
        }
    }
}
