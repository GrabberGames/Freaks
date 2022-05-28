using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteFreaksInfoUI : MonoBehaviour
{
    [SerializeField] private Text freaksInfoText;
    public void Visualization(int idleFreaksCount, int maxFreaksCount)
    {
        freaksInfoText.text = idleFreaksCount.ToString("#00") + " / " + maxFreaksCount.ToString("#00");
    }
}
