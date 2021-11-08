using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat 
{
    public float attack { get; set; }
    public float hp { get; set; }
    public float ats { get; set; }
    public float ms { get; set; }
    public float rg { get; set; }
    public float am { get; set; }
    public Dictionary<string, Stat> Json { get; set; }
}