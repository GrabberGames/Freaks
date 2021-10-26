using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusClass : MonoBehaviour
{
    protected class Stat
    {
        public string tag;
        public float attack;
        public float health;
        public float attackspeed;
        public float movespeed;
        public float range;
        public float price;
    }
    protected virtual void Init() { }
}
