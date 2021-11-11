using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyle_Q : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed = 10f;
    GameObject Bullet_Obj;
    public ParticleSystem Bullet;
    GameObject Hit_Obj;
    public ParticleSystem Hit;
    Vector3 _dirVector;

    private bool One = false;
    //스킬 시전시 FX_KailQ_Emit : 총구에서 이펙트
    //Kail Script에서 현재 위치와 바라보고 있는 방향벡터를 얻어온다. - 1
    //바라보고 있는 방향을 어떻게 얻어올 것인가? - 2
    //얻어온 정보를 바탕으로 Paticle의 방향과 위치를 설정해준다. - 3
    public void GetVectorInfo(Vector3 _nowPosition, Vector3 _dirVector) // -1 & 2
    {
        transform.position = _nowPosition; // -3
        this._dirVector = _dirVector;            // -3
        transform.LookAt(_dirVector);         // -3
    }
    //====================================//
    //스킬 시전시 FX_KailQ_Bullet : 총알 이펙트
    //스킬 시전 시 Bullet 이동
    private void Update()
    {
        transform.position += _dirVector.normalized * _bulletSpeed * Time.deltaTime;
    }
    private void Particle_Bullet()
    {
        Bullet.Play(true); 
    }
    //====================================//
    //적 피격 시 FX_Kail_Hit : 적 위치에 이펙트
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("BlackFreaks") && !One)
        {
            _bulletSpeed = 0f;
            Bullet.Play(false);
            Hit.Play(true);
            One = !One;
            StartCoroutine(DeleteThis());
        }
    }
    IEnumerator DeleteThis()
    {
        yield return new WaitForSeconds(Hit.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        Destroy(this.gameObject);
    }
}
