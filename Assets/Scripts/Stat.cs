using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat 
{
    public float PD { get; set; }
    public float ED { get; set; }
    public float HP { get; set; }
    public float ATTACK_SPEED { get; set; }
    public float MOVE_SPEED { get; set; }
    public float ATTACK_RANGE { get; set; }
    public float ARMOR { get; set; }
    public Dictionary<string, Stat> Json { get; set; }
}