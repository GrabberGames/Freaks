using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject enemy;
    private Vector3 enemyPos;
    private ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void InitSetting(GameObject enemy)
    {
        this.enemy = enemy;
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play(true);
        StartCoroutine(Delete());
    }
    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
            transform.position -= (transform.position - enemyPos);
            transform.rotation = Quaternion.LookRotation(enemyPos - transform.position);
        }
    }
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(particleSystem.main.startLifetimeMultiplier);
        Destroy(gameObject);
    }
}