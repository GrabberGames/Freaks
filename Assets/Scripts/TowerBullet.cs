using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public ParticleSystem[] fx_blackTower;
    public GameObject[] fx_blackTowerPre;
    private float BulletSpeed = 30f;

    private Vector3 playerPos;
    private GameObject player;

    private int State = 0;

    public AudioSource SFXBlackTowerBullet;
    private GameObject whitefreaks;
    private Vector3 whitefreaksPos;

    private bool TargetPlayer;

    private bool isCrushed = false;
    float _damage;

    public void InitSetting(float damage, GameObject enemy, Vector3 bulletSpawnPosition)
    {
        _damage = damage;
        TargetPlayer = false;

        this.whitefreaks = enemy;
        StartCoroutine(StartProjectile(0.5f));
        this.transform.position = bulletSpawnPosition;
    }

    public void InitSetting(float damage, Vector3 bulletSpawnPosition)
    {
        _damage = damage;
        TargetPlayer = true;
        if (player == null)
        {
            if (GameManager.Instance.Player == null)
                return;
            else
                player = GameManager.Instance.Player;
        }
   
        this.transform.position = bulletSpawnPosition;
        StartCoroutine(StartProjectile(0.5f));

    }

         
    IEnumerator Shoot()
    {
        while(true)
        {
            if (TargetPlayer)
            {
                if (player == null)
                {
                    BulletPooling.ReturnObject(this.gameObject);
                    break;
                }
                playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
                if ((playerPos - transform.position).sqrMagnitude < 25f)
                {
                    if (isCrushed == true || player == null)
                    {
                        break;
                    }
                    SFXBlackTowerBullet.Play();

                   GameManager.Damage.OnAttacked(_damage, player.GetComponent<Stat>());
                    fx_blackTower[0].Pause();
                    fx_blackTower[0].Play(false);
                    fx_blackTowerPre[0].SetActive(false);
                    //Destroy(fx_blackTowerPre[0]);



                    fx_blackTowerPre[1].SetActive(true);
                    fx_blackTower[1] = fx_blackTowerPre[1].GetComponent<ParticleSystem>();
                    fx_blackTower[1].Play(true);

                    State = 2;
                    StartCoroutine(DeleteThis());
                    isCrushed = true;
                }

                switch (State)
                {

                    case 1:
                        fx_blackTower[0].transform.LookAt(playerPos);
                        transform.position += (playerPos - transform.position).normalized * BulletSpeed * Time.deltaTime;
                        transform.rotation = Quaternion.identity;
                        break;
                    case 2:
                        transform.position += (playerPos - transform.position).normalized * BulletSpeed * Time.deltaTime;
                        transform.rotation = Quaternion.identity;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (whitefreaks == null)
                {
                    BulletPooling.ReturnObject(this.gameObject);
                    break;
                }
                whitefreaksPos = new Vector3(whitefreaks.transform.position.x, whitefreaks.transform.position.y + 1f, whitefreaks.transform.position.z);

                if ((whitefreaksPos - transform.position).sqrMagnitude < 25f)
                {
                    if (isCrushed == true || whitefreaks == null)
                    {
                        break;
                    }
                    SFXBlackTowerBullet.Play();

                    GameManager.Damage.OnAttacked(_damage, whitefreaks.GetComponent<Stat>());
                    fx_blackTower[0].Pause();
                    fx_blackTower[0].Play(false);
                    fx_blackTowerPre[0].SetActive(false);
                    //Destroy(fx_blackTowerPre[0]);



                    fx_blackTowerPre[1].SetActive(true);
                    fx_blackTower[1] = fx_blackTowerPre[1].GetComponent<ParticleSystem>();
                    fx_blackTower[1].Play(true);

                    State = 2;
                    StartCoroutine(DeleteThis());                   
                    isCrushed = true;

                }

                switch (State)
                {
                    case 1:
                        fx_blackTower[0].transform.LookAt(whitefreaksPos);
                        transform.position += (whitefreaksPos - transform.position).normalized * BulletSpeed * Time.deltaTime;
                        transform.rotation = Quaternion.identity;
                        break;
                    case 2:
                        transform.position += (whitefreaksPos - transform.position).normalized * BulletSpeed * Time.deltaTime;
                        transform.rotation = Quaternion.identity;
                        break;
                    default:
                        break;
                }

            }
            yield return null;
        }
    }

    IEnumerator StartProjectile(float waitTime)
    {
        yield return YieldInstructionCache.WaitForSeconds(waitTime);
        fx_blackTowerPre[0].SetActive(true);
        fx_blackTower[0] = fx_blackTowerPre[0].GetComponent<ParticleSystem>();
        fx_blackTower[0].Play(true);
        State = 1;

        StartCoroutine(Shoot());
    }


    IEnumerator DeleteThis()
    {
        yield return YieldInstructionCache.WaitForSeconds(fx_blackTower[1].main.startLifetimeMultiplier);
        fx_blackTowerPre[1].SetActive(false);
        State = 3;
        /*
        Destroy(fx_blackTower[1]);
        Destroy(gameObject);*/
        gameObject.SetActive(false);
        BulletPooling.ReturnObject(this.gameObject);
    }
}