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

    private bool isCrushed = false;
    float _damage;

    public void InitSetting(float damage)
    {
        _damage = damage;
        if (player == null)
        {
            if (GameManager.Instance.Player == null)
                return;
            else
                player = GameManager.Instance.Player;
        }
        StartCoroutine(StartProjectile(0.5f));
    }


    private void Update()
    {
        if (player == null)
            return;
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 4f, player.transform.position.z);

        switch (State) 
        {
            case 1:
                fx_blackTower[0].transform.LookAt(playerPos);
                transform.position += (playerPos - transform.position).normalized * BulletSpeed * Time.deltaTime;
                transform.rotation = Quaternion.identity;
                break;
            case 2:
                transform.position += (playerPos-transform.position).normalized * BulletSpeed * Time.deltaTime;
                transform.rotation = Quaternion.identity;
                break;
            default:
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (isCrushed == true || other == null)
            {
                return;
            }
            GameManager.Damage.OnAttacked(_damage, other.GetComponent<Stat>());
            fx_blackTower[0].Pause();
            fx_blackTower[0].Play(false);
            fx_blackTowerPre[0].SetActive(false);
            Destroy(fx_blackTowerPre[0]);

            fx_blackTowerPre[1].SetActive(true);
            fx_blackTower[1] = fx_blackTowerPre[1].GetComponent<ParticleSystem>();
            fx_blackTower[1].Play(true);

            State = 2;
            StartCoroutine(DeleteThis());
            isCrushed = true;
        }
    }


    IEnumerator StartProjectile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        fx_blackTowerPre[0].SetActive(true);
        fx_blackTower[0] = fx_blackTowerPre[0].GetComponent<ParticleSystem>();
        fx_blackTower[0].Play(true);
        State = 1;
    }


    IEnumerator DeleteThis()
    {
        yield return new WaitForSeconds(fx_blackTower[1].main.startLifetimeMultiplier); 
        fx_blackTowerPre[1].SetActive(false);
        State = 3;
        Destroy(fx_blackTower[1]);
        Destroy(gameObject);
    }
}