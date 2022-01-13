using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletpre;
    public GameObject player;
    public ParticleSystem fx_blackTower;
    public ParticleSystem fx_hit;
    Stat _stat;

    private float AttackPerSeconds = 4f;

    private Vector3 bulletSpawnPosition;


    bool isAttack = false;

    void Start()
    {
        _stat = GetComponent<Stat>();

        bulletSpawnPosition = new Vector3(transform.position.x, transform.position.y + 18.98f, transform.position.z - 0.29f);
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
        if((player.transform.position - transform.position).magnitude <=  _stat.ATTACK_RANGE)
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
        bullet = Instantiate(bulletpre, bulletSpawnPosition, Quaternion.Euler(bulletSpawnPosition - player.transform.position));
        bullet.GetComponent<TowerBullet>().InitSetting(_stat.PD);
        fx_blackTower.Play(true);

        yield return new WaitForSeconds(fx_blackTower.main.startDelayMultiplier);

        fx_blackTower.Play(false);

        yield return new WaitForSeconds(AttackPerSeconds - fx_blackTower.main.startDelayMultiplier);
        isAttack = false;
    }
}
