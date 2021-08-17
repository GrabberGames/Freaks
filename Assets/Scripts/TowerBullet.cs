using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public float BulletSpeed = 2;
    Quaternion DirRot;
    GameObject player;
    public ParticleSystem[] fx_blackTower;
    private bool[] State;
    public void InitSetting(GameObject player)
    {
        this.player = player;
        Instantiate(fx_blackTower[0], transform.position, Quaternion.Euler(transform.position - player.transform.position));
        fx_blackTower[0].Play(true);
        StartCoroutine(effectManage(0, 0.1f, false));
    }
    private void Update()
    {
        fx_blackTower[0].transform.position = this.transform.position;
        fx_blackTower[1].transform.position = this.transform.position;
        fx_blackTower[2].transform.position = this.transform.position;
        transform.position -= (transform.position - player.transform.position).normalized * BulletSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.position - player.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Waron")
        {
            Instantiate(fx_blackTower[2], transform.position, Quaternion.Euler(transform.position - player.transform.position));
            fx_blackTower[2].Play(true);
            StartCoroutine(DeleteThis());
        }
    }
    IEnumerator effectManage(int number, float waitTime, bool onoff)
    {
        yield return new WaitForSeconds(waitTime);
        fx_blackTower[number].Play(onoff);
        Instantiate(fx_blackTower[1], transform.position, Quaternion.Euler(transform.position - player.transform.position));
        fx_blackTower[1].Play(true);
    }
    IEnumerator DeleteThis() {
        yield return new WaitForSeconds(fx_blackTower[2].main.startLifetimeMultiplier);
        Destroy(this.gameObject);
    }
}
