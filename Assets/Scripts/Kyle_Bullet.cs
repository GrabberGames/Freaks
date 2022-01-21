using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyle_Bullet : MonoBehaviour
{
    private float _bullet_speed = 40;
    private float _bullet_time = 0f;

    public GameObject enemy;
    public ParticleSystem projectile_basic;
    public ParticleSystem projectile_skill;
    public ParticleSystem hit_basic;
    public ParticleSystem hit_skill;

    private ParticleSystem ps_tile;  // <- use particle
    private ParticleSystem ps_hit; // <- use hit particle
    private Vector3 dir;
    private Vector3 look_dir;

    private float _damage;

    enum _Bullet
    {
        Basic,  //평타
        Q,        //Q
        WE      //WE
    }
    _Bullet _bullet = _Bullet.Basic;
    public void InitSetting(GameObject enemy, string _state, Vector3 dir, float damage)
    {
        this.enemy = enemy;
        look_dir = dir;
        this.dir = (transform.position - dir).normalized;
        _damage = damage;
        StartProjectile(enemy, _state);
    }

    void StartProjectile(GameObject enemy, string _state)
    {
        switch(_state)
        {
            case "Basic":
                _bullet = _Bullet.Basic;
                ps_tile = Instantiate(projectile_basic, transform);
                ps_tile.transform.SetParent(this.gameObject.transform);
                ps_tile.Play();
                StartCoroutine(DT());
                break;

            case "Q":
                _bullet = _Bullet.Q; 
                _bullet_time = 0.75f;  //체공시간 0.75초
                ps_tile =  Instantiate(projectile_skill, transform);
                ps_tile.transform.LookAt(look_dir);
                ps_tile.Play();
                Invoke("Destroy", _bullet_time);
                StartCoroutine(DT());
                break;

            case "W":
            case "E":
                _bullet = _Bullet.WE;
                _bullet_time = 0.44f;  //체공시간 0.44초
                ps_tile = Instantiate(projectile_skill);
                ps_tile.transform.SetParent(this.gameObject.transform);
                ps_tile.transform.LookAt(look_dir);
                ps_tile.Play();
                Invoke("Destroy", _bullet_time);
                StartCoroutine(DT());
                break;

            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("BlackFreaks") || other.transform.CompareTag("BlackTower"))
        {
            if (other == null)
                return;
            GameManager.Damage.OnAttacked(_damage, other.GetComponent<Stat>());
            
            switch (_bullet)
            {
                case _Bullet.Basic:
                    ps_hit = Instantiate(hit_basic);
                    ps_hit.transform.position = transform.position;
                    //ps_hit.transform.rotation = transform.rotation;
                    ps_hit.Play();
                    StartCoroutine(DT_Hit());
                    break;
                case _Bullet.Q :
                case _Bullet.WE:
                    ps_hit = Instantiate(hit_skill);
                    ps_hit.transform.position = transform.position;
                    ps_hit.Play();
                    StartCoroutine(DT_Hit());
                    break;
                default:
                    break;
            }
        }
    }
    void Destroy()
    {
        Destroy(this.gameObject);
    }
    IEnumerator DT()
    {
        yield return new WaitForSeconds(ps_tile.main.startLifetimeMultiplier); 
        Destroy(ps_tile);
        Destroy();
    }
    IEnumerator DT_Hit()
    {
        yield return new WaitForSeconds(ps_hit.main.startLifetimeMultiplier);
        Destroy(ps_hit);
        Destroy();
    }
    void Update()
    {
        if (ps_tile != null)
        {
            ps_tile.transform.position = transform.position;
        }
        switch (_bullet)
        {
            case _Bullet.Basic:
                if (enemy == null)
                    return;
                Vector3 enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
                transform.position += (enemyPos - transform.position).normalized * _bullet_speed * Time.deltaTime;
                transform.LookAt(enemyPos);
                break;
            case _Bullet.Q:
            case _Bullet.WE:
                transform.position -= dir * _bullet_speed * Time.deltaTime;
                break;
            default:
                break;
        }
    }
    //Q는 타겟팅이 아니잖아.
    //enemy를 만들어 둘 필요가 없다.
    //기본 공격에서도 사용하려면? 
    //여러개로 분기시켜놓는다?
    //기본 공격과 스킬 이펙트가 다름 -> 매개변수로 받아서 어떤 파티클을 쓸지 선택한다.
    //Q와 WE 이펙트는 같지만 체공시간? 이 다름. 
    //이 역시 매개변수로 받아온다. 

    //그런데 평타 공격같은 경우는 정해진 대상에게 공격한다.
    //스킬은 아닌데?
    //이것도 분기? 가능?
}
