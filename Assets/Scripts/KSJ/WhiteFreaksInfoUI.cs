using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteFreaksInfoUI : MonoBehaviour
{
    [SerializeField] private Text freaksInfoText;

    private bool emptyIdleFreaks;

    public void Visualization(int idleFreaksCount, int maxFreaksCount)
    {
        freaksInfoText.text = idleFreaksCount.ToString("#00") + " / " + maxFreaksCount.ToString("#00");

        if (!idleFreaksCount.Equals(0).Equals(emptyIdleFreaks))
        {
            emptyIdleFreaks = !emptyIdleFreaks;

            if(emptyIdleFreaks)
                freaksInfoText.color = Color.red;
            else
                freaksInfoText.color = Color.white;
        }
    }
}
