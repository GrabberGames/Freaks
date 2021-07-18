using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public abstract void SetMaterial(bool isRed);
    public abstract void SetOpacity(bool isTransparent);
}