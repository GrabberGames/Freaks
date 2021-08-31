using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KailAni : MonoBehaviour
{
    Vector3 StartPosition;
    Vector3 EndPosition;

    public void SetStartPosition()
    {
        StartPosition = transform.position;
    }
    public void SetEndPosition()
    {
        EndPosition = transform.position;
    }
    public Vector3 returnMove()
    {
        return EndPosition - StartPosition;
    }
}
