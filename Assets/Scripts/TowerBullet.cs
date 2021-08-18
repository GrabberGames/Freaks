using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public float BulletSpeed = 8000;
    Vector3 playerPos;
    GameObject player;
    public ParticleSystem[] fx_blackTower;
    public ParticleSystem[] fx_blackTowerPre;
    private int State = 0;
    public void InitSetting(GameObject player)
    {
        this.player = player;
        StartCoroutine(StartProjectile(0.5f));
    }

    private void Update()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 5f, player.transform.position.z);
        switch (State) {
            case 1:
                fx_blackTower[1].transform.position = this.transform.position;
                fx_blackTower[1].transform.rotation = Quaternion.LookRotation(playerPos - fx_blackTower[1].transform.position);
                transform.position -= (transform.position - playerPos) * BulletSpeed * Time.deltaTime;
                transform.rotation = Quaternion.identity;
                break;
            case 2:
                fx_blackTower[2].transform.position = this.transform.position;
                fx_blackTower[2].transform.rotation = Quaternion.LookRotation(playerPos - fx_blackTower[2].transform.position);
                transform.position -= (transform.position - playerPos) * BulletSpeed * Time.deltaTime;
                transform.rotation = Quaternion.identity;
                break;
            default:
                break;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Waron")
        {   
            fx_blackTower[1].Play(false);
            fx_blackTower[2] = Instantiate(fx_blackTowerPre[2]);
            fx_blackTower[2].transform.SetParent(this.gameObject.transform);
            fx_blackTower[2].transform.position = transform.position;
            fx_blackTower[2].transform.rotation = Quaternion.LookRotation(playerPos - fx_blackTower[2].transform.position);
            fx_blackTower[2].Play(true);
            State = 2;
            StartCoroutine(DeleteThis());
        }
    }
    IEnumerator StartProjectile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        fx_blackTower[1] = Instantiate(fx_blackTowerPre[1]);
        fx_blackTower[1].transform.SetParent(this.gameObject.transform);
        fx_blackTower[1].transform.position = transform.position;
        fx_blackTower[1].transform.rotation = Quaternion.LookRotation(playerPos - fx_blackTower[1].transform.position);
        fx_blackTower[1].Play(true);
        State = 1;
    }
    IEnumerator DeleteThis()
    {
        Destroy(fx_blackTower[1]);
        yield return new WaitForSeconds(fx_blackTower[2].main.startLifetimeMultiplier);
        State = 3;
        Destroy(fx_blackTower[2]);
        Destroy(this.gameObject);
    }
}
