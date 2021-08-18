using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletpre;
    private float AttackPerSeconds = 4f;
    public GameObject player;

    private Vector3 bulletSpawnPosition;
    public ParticleSystem fx_blackTower;

    void Start()
    {
        FindHero();
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 18.98f, transform.position.z - 0.29f);
        print(bulletSpawnPosition);
    }
    // Update is called once per frame
    void Update()
    {

    }
    void FindHero()
    {
        player = GameObject.Find("Waron");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Waron")
        {
            StartCoroutine(FindInAttackRange());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Waron")
        {
            StopCoroutine(FindInAttackRange());
        }
    }
    IEnumerator FindInAttackRange()
    {
        bullet = Instantiate(bulletpre, bulletSpawnPosition, Quaternion.Euler(bulletSpawnPosition - player.transform.position));
        bullet.GetComponent<TowerBullet>().InitSetting(player);
        fx_blackTower.Play(true);
        yield return new WaitForSeconds(fx_blackTower.main.startDelayMultiplier);
        fx_blackTower.Play(false);
        yield return new WaitForSeconds(AttackPerSeconds - fx_blackTower.main.startDelayMultiplier);
        StartCoroutine(FindInAttackRange());
    }

}
