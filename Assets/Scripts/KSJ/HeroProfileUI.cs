using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroProfileUI : MonoBehaviour
{
    [Header("Profile Image")]
    [SerializeField] GameObject waron;
    [SerializeField] GameObject kyle;

    [Header("Profile Name Text")]
    [SerializeField] Text nameText;

    public void SetProfile(eHeroType hero)
    {
        switch(hero)
        {
            case eHeroType.Kyle:
                kyle.SetActive(true);
                waron.SetActive(false);
                break;
            case eHeroType.Waron:
                waron.SetActive(true);
                kyle.SetActive(false);
                break;
            default:
                Debug.Log("error : Hero selection is required.");
                break;
        }

        nameText.text = hero.ToString().ToUpper();
    }

}
