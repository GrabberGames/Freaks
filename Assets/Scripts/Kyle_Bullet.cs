using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Bullet
{
    Basic,  //평타
    Q,        //Q
    E      //E
}
public class Kyle_Bullet : MonoBehaviour
{
    private float _bullet_speed = 60;
    private float _bullet_time = 0f;

    public GameObject enemy;
    public GameObject projectile_basic;
    public GameObject projectile_skill;
    public GameObject hit_basic;
    public GameObject hit_skill;

    private GameObject ps_tile;  // <- use particle
    private GameObject ps_hit; // <- use hit particle
    private Vector3 dir;

    private float _damage;

    private bool _isAttack = false;

    Bullet _bullet = Bullet.Basic;
    Kali _kail;
    public void InitSetting(GameObject enemy, Bullet _state, Quaternion p_ldir, Vector3 p_hdir, float damage, Kali kail)
    {
        this.enemy = enemy;
        transform.LookAt(p_hdir);
        dir = new Vector3(p_hdir.x - transform.position.x, 0, p_hdir.z - transform.position.z).normalized;
        _damage = damage;
        StartProjectile(_state);
        _kail = kail;
    }

    void StartProjectile(Bullet _state)
    {
        switch (_state)
        {
            case Bullet.Basic:
                _bullet = Bullet.Basic;
                ps_tile = ObjectPooling.Instance.GetObject("KyleNormalBulletEffect");
                ps_tile.transform.position = transform.position;
                //ps_tile.transform.localEulerAngles = Vector3.zero;

                ps_tile.transform.SetParent(gameObject.transform);
                ps_tile.transform.rotation = Quaternion.identity;
                ps_tile.transform.localEulerAngles = Vector3.zero;

                var particleSystem = ps_tile.GetComponent<ParticleSystem>();

                particleSystem.Play();
                break;

            case Bullet.Q:
                _bullet = Bullet.Q; 
                _bullet_time = 0.75f;  //체공시간 0.75초
                ps_tile = ObjectPooling.Instance.GetObject("KyleSkillEffect");
                ps_tile.transform.position = transform.position;
                ps_tile.transform.SetParent(transform);
                ps_tile.transform.localEulerAngles = Vector3.zero;

                var particle = ps_tile.GetComponent<ParticleSystem>();
                particle.Play();

                Invoke("Destroy", _bullet_time);
                break;

            case Bullet.E:
                _bullet = Bullet.E;
                _bullet_time = 0.44f;  //체공시간 0.44초
                ps_tile = ObjectPooling.Instance.GetObject("KyleSkillEffect");
                ps_tile.transform.position = transform.position;
                ps_tile.transform.SetParent(transform);
                ps_tile.transform.localEulerAngles = Vector3.zero;

                var particleS = ps_tile.GetComponent<ParticleSystem>();
                particleS.Play();

                Invoke("Destroy", _bullet_time);
                break;

            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("BlackFreaks") || other.transform.CompareTag("BlackTower"))
        {
            if (_isAttack)
                return;

            if (other == null)
                return;

            GameManager.Damage.OnAttacked(_damage, other.GetComponent<Stat>());
            
            switch (_bullet)
            {
                case Bullet.Basic:
                    ps_hit = ObjectPooling.Instance.GetObject("KyleHitEffect");
                    ps_hit.transform.position = transform.position;
                    var particle = ps_hit.GetComponent<ParticleSystem>();
                    particle.Play();
                    StartCoroutine(Destroy_Hit());
                    _kail.PlaySFX(0);
                    break;
                case Bullet.Q:
                    _kail.PlaySFX(1);
                    break;
                case Bullet.E:
                    ps_hit = ObjectPooling.Instance.GetObject("KyleHitEffect");
                    ps_hit.transform.position = transform.position;


                    var particleSystem = ps_hit.GetComponent<ParticleSystem>();
                    particleSystem.Play();

                    StartCoroutine(Destroy_Hit());
                    _kail.PlaySFX(2);
                    break;
                default:
                    break;
            }
            _isAttack = true;
        }
    }
    void Destroy()
    {
        ObjectPooling.Instance.ReturnObject(gameObject);
    }
    IEnumerator Destroy_Hit()
    {
        yield return new WaitForSeconds(0.5f);
        ObjectPooling.Instance.ReturnObject(ps_hit);
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
            case Bullet.Basic:
                if (enemy == null)
                    return;
                Vector3 enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
                transform.position += (enemyPos - transform.position).normalized * _bullet_speed * Time.deltaTime;
                //transform.LookAt(enemyPos);
                break;
            case Bullet.Q:
                transform.position += dir * _bullet_speed * Time.deltaTime;
                break;
            case Bullet.E:
                transform.position += dir * _bullet_speed * Time.deltaTime;
                break;
            default:
                break;
        }
    }
}
