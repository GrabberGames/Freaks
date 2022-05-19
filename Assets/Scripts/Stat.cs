using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public Stat(float pd=0, float ed=0, float hp=0, float mhp=0, float ats=0, float ms=0, float atr=0, float ar=0)
    {
        PD = pd;
        ED = ed;
        HP = hp;
        MAX_HP = mhp;
        ATTACK_SPEED = ats;
        MOVE_SPEED = ms;
        ATTACK_RANGE = atr;
        ARMOR = ar;
        DECREASE_DAMAGE = 0;
    }
    [SerializeField]
    public float PD { get; set; }
    [SerializeField]
    public float ED { get; set; }
    [SerializeField]
    public float HP { get; set; }
    [SerializeField]
    public float MAX_HP { get; set; }
    [SerializeField]
    public float ATTACK_SPEED { get; set; }
    [SerializeField]
    public float MOVE_SPEED { get; set; }
    [SerializeField]
    public float ATTACK_RANGE { get; set; }
    [SerializeField]
    public float DECREASE_DAMAGE { get; set; }

    [SerializeField]
    public float ARMOR { get; set; }
    public Dictionary<string, Stat> Json { get; set; }

    protected virtual void Init() 
    {
        gameObject.transform.name = gameObject.transform.name.Replace("(Clone)", "");
        PD = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).PD;
        ED = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).ED;
        HP = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).HP;
        MAX_HP = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).MAX_HP;
        ATTACK_SPEED = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).ATTACK_SPEED;
        MOVE_SPEED = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).MOVE_SPEED;
        ATTACK_RANGE = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).ATTACK_RANGE;
        ARMOR = ObjectPooling.Instance.Get_Stat(gameObject.transform.name).ARMOR;
    }
    public virtual void DeadSignal() { }
    public virtual void SetModel() { }
}