using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KailParent : MonoBehaviour
{
    public KailAni kailAni;
    public void SetStartPos()
    {
        kailAni.SetStartPosition();
    }
    public void SetEndPos()
    {
        kailAni.SetEndPosition();
    }
    public void SetPos()
    {
        transform.position += kailAni.returnMove();
    }
}
