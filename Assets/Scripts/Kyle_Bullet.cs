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
        Basic,  //��Ÿ
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
                _bullet_time = 0.75f;  //ü���ð� 0.75��
                ps_tile =  Instantiate(projectile_skill, transform);
                ps_tile.transform.LookAt(look_dir);
                ps_tile.Play();
                Invoke("Destroy", _bullet_time);
                StartCoroutine(DT());
                break;

            case "W":
            case "E":
                _bullet = _Bullet.WE;
                _bullet_time = 0.44f;  //ü���ð� 0.44��
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
    //Q�� Ÿ������ �ƴ��ݾ�.
    //enemy�� ����� �� �ʿ䰡 ����.
    //�⺻ ���ݿ����� ����Ϸ���? 
    //�������� �б���ѳ��´�?
    //�⺻ ���ݰ� ��ų ����Ʈ�� �ٸ� -> �Ű������� �޾Ƽ� � ��ƼŬ�� ���� �����Ѵ�.
    //Q�� WE ����Ʈ�� ������ ü���ð�? �� �ٸ�. 
    //�� ���� �Ű������� �޾ƿ´�. 

    //�׷��� ��Ÿ ���ݰ��� ���� ������ ��󿡰� �����Ѵ�.
    //��ų�� �ƴѵ�?
    //�̰͵� �б�? ����?
}
