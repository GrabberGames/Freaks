using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectUI : MonoBehaviour
{
    [Header("Waron")]
    [SerializeField] private Image waronButtonFrame;

    [Header("Kyle")]
    [SerializeField] private Image kyleButtonFrame;

    [Header("Frame Sprite")]
    [SerializeField] private Sprite normalFrame;
    [SerializeField] private Sprite selectFrame;

    public void SelectHero(eHeroType hero)
    {
        switch(hero)
        {
            case eHeroType.WARON:
                waronButtonFrame.sprite = selectFrame;
                kyleButtonFrame.sprite = normalFrame;
                break;

            case eHeroType.KYLE:
                kyleButtonFrame.sprite = selectFrame;
                waronButtonFrame.sprite = normalFrame;
                break;

            default:
                kyleButtonFrame.sprite = normalFrame;
                waronButtonFrame.sprite = normalFrame;
                break;
        }
    }

    private void OnDisable()
    {
        kyleButtonFrame.sprite = normalFrame;
        waronButtonFrame.sprite = normalFrame;
    }
}
