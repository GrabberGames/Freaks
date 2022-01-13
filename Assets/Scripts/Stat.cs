using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    public float PD { get; set; }
    [SerializeField]
    public float ED { get; set; }
    [SerializeField]
    public float HP { get; set; }
    [SerializeField]
    public float ATTACK_SPEED { get; set; }
    [SerializeField]
    public float MOVE_SPEED { get; set; }
    [SerializeField]
    public float ATTACK_RANGE { get; set; }
    [SerializeField]
    public float ARMOR { get; set; }
    public Dictionary<string, Stat> Json { get; set; }

    private void Start()
    {
        PD = ObjectPooling.instance.Get_Stat(gameObject.transform.name).PD;
        ED = ObjectPooling.instance.Get_Stat(gameObject.transform.name).ED;
        HP = ObjectPooling.instance.Get_Stat(gameObject.transform.name).HP;
        ATTACK_SPEED = ObjectPooling.instance.Get_Stat(gameObject.transform.name).ATTACK_SPEED;
        MOVE_SPEED = ObjectPooling.instance.Get_Stat(gameObject.transform.name).MOVE_SPEED;
        ATTACK_RANGE = ObjectPooling.instance.Get_Stat(gameObject.transform.name).ATTACK_RANGE;
        ARMOR = ObjectPooling.instance.Get_Stat(gameObject.transform.name).ARMOR;
    }
}