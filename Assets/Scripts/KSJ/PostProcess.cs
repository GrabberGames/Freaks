using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    //[SerializeField] private ColorGrading tqt;
    [SerializeField] private PostProcessVolume activeVolume;

    private ColorGrading colorGrading;

    [Header ("Color Grading - Color Filter")]
    [SerializeField] private Color normalModeColor;
    [SerializeField] private Color rageModeColor;

    [Range(0.0f, 10.0f)]
    [SerializeField] private float fadeInTime;
    [SerializeField] private AnimationCurve fadeCurve;

    private Color temp;

    // Start is called before the first frame update
    private void Awake()
    {
        activeVolume.profile.TryGetSettings(out colorGrading);
    }
    
    public void ActiveRageMode(bool value)
    {
        if(value)
        {
            colorGrading.colorFilter.overrideState = true;
            colorGrading.colorFilter.Override(normalModeColor);
            StartCoroutine(ModeChange(normalModeColor, rageModeColor));
        }
        else
        {
            colorGrading.colorFilter.overrideState = true;
            colorGrading.colorFilter.Override(rageModeColor);
            StartCoroutine(ModeChange(rageModeColor, normalModeColor));
        }
    }

    IEnumerator ModeChange(Color before, Color after)
    {
        temp = after - before;

        float tempTime = 0.0f;
        float tempfade = 1 / fadeInTime;

        while(true)
        {
            yield return null;
            tempTime += Time.deltaTime;


            if(tempTime >= fadeInTime)
            {
                colorGrading.colorFilter.Override(after);
                break;
            }

            colorGrading.colorFilter.Override(before + (temp * fadeCurve.Evaluate(tempTime * tempfade)));
        }
    }




}
