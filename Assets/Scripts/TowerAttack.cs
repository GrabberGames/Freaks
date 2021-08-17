using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletpre;
    public float AttackPerSeconds = 1f;
    public GameObject player;

    private Vector3 bulletSpawnPosition;

    //WarriorAnims.HeroMovement heroMovement;
    // Start is called before the first frame update
    void Start()
    {
        FindHero();
        //heroMovement = player.GetComponent<WarriorAnims.HeroMovement>();
        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 38.98f, transform.position.z - 0.29f);
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
        yield return new WaitForSeconds(100f);
        StartCoroutine(FindInAttackRange());
    }
}
