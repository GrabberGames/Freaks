using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeModel
{
    float _nowTime = 0f;
    float _totalTime = 0f;
    public float nowTime
    {
        get => _nowTime;
        set => _nowTime = value;
    }
    public float totalTime
    {
        get => _totalTime;
        set => _totalTime = value;
    }
}
