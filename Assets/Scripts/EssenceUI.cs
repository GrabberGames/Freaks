using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EssenceUI : MonoBehaviour
{
    [SerializeField] private Text essenceText;

    public void Visualization(int value)
    {
        essenceText.text = value.ToString();
    }

}
