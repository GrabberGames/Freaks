using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTowerAttack : Stat
{

    public ParticleSystem fx_whiteTower;

    Stat _stat;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;
    private GameObject NoBuildRange ;

    bool isAttack = false;
    Vector3 getPos;

    public AudioSource SFXWhiteTowerDestroy;
    public AudioSource SFXWhiteTowerAttack;

    public void SetPosition(Vector3 pos) //��ǥ �޾ƿ��� �Լ�
    {

        getPos = pos;
        //this.gameObject.SetActive(true);
    }


    protected override void Init()
    {
        base.Init();

        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 18.98f, transform.position.z - 0.29f);
        NoBuildRange = transform.GetChild(2).gameObject;
        NoBuildRange.SetActive(false);

    }
    public override void DeadSignal()
    {
        if (HP <= 0)
        {
            StartCoroutine("FadeOut"); //fadeout�Ҷ� �� �ڵ�
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        Init();

    }

    //�Ҹ� Ȯ���ϱ����� update��
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine("FadeOut");

        }
    }

    private void OnTriggerEnter(Collider other) //��Ÿ����� ���� ��freaks�� ���� ���
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

    private void OnTriggerStay(Collider other) //��Ÿ����� ���� ��freaks�� �������� ���
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


    public void WhiteTowerRangeON()
        {
        NoBuildRange.SetActive(true);
        }

    public void WhiteTowerRangeOFF()
    {
        NoBuildRange.SetActive(false);
    }


}
