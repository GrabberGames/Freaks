using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    //[SerializeField] private ColorGrading tqt;
    [SerializeField] private PostProcessVolume activeVolume;

    private ColorGrading colorGrading;
    private Vignette vignetting;

    [Header ("Rage - Color Filter")]
    [SerializeField] private Color normalModeColor;
    [SerializeField] private Color rageModeColor;


    [Range(0.0f, 10.0f)]
    [SerializeField] private float fadeInTime;
    [SerializeField] private AnimationCurve fadeCurve;


    [Header("Warning - Intensity")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float warningOff;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float warningOn;
    [SerializeField] private float warningFadeTime;

    private Color temp;

    private bool _isRage;
    private bool _isDeath;
    private bool _isWarning;

    private IEnumerator cRageMode;
    private IEnumerator cWarningMode;

    private static PostProcessManager mInstance;
    public static PostProcessManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<PostProcessManager>();
            }
            return mInstance;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        activeVolume.profile.TryGetSettings(out colorGrading);
        activeVolume.profile.TryGetSettings(out vignetting);
    }
    
    public void ActiveRageMode(bool value)
    {
        if(!value.Equals(_isRage))
        {
            if (value)
            {
                colorGrading.colorFilter.overrideState = true;
                colorGrading.colorFilter.Override(normalModeColor);
                StartCoroutine(RageModeChange(rageModeColor));
            }
            else
            {
                colorGrading.colorFilter.overrideState = true;
                colorGrading.colorFilter.Override(rageModeColor);
                StartCoroutine(RageModeChange(normalModeColor));
            }
            _isRage = value;
        }
    }

    public void ActiveDeathMode(bool value)
    {
        if(!value.Equals(_isDeath))
        {
            if (value)
            {
                colorGrading.saturation.value = -100.0f;
                colorGrading.contrast.value = 50.0f;
            }
            else
            {
                colorGrading.saturation.value = 0.0f;
                colorGrading.contrast.value = 0.0f;
            }

            colorGrading.saturation.overrideState = value; 
            colorGrading.contrast.overrideState = value;
            _isDeath = value;
        }
    }

    public void ActiveWarningMode(bool value)
    {
        if (!value.Equals(_isWarning))
        {
            if (!vignetting.active)
                vignetting.active = true;

            if(cWarningMode != null)
            {
                StopCoroutine(cWarningMode);
                cWarningMode = null;
            }       

            if (value)
                cWarningMode = WarningModeChange(warningOn);               
            else
                cWarningMode = WarningModeChange(warningOff);

            StartCoroutine(cWarningMode);

            _isWarning = value;
        }
    }

    IEnumerator RageModeChange(Color after)
    {
        Color before = colorGrading.colorFilter.value;

        temp = after - before;

        float startFadeTime = Time.time;
        float tempTime = 0.0f;
        float tempfade = 1 / fadeInTime;

        while(true)
        {
            yield return null;
            tempTime = Time.time - startFadeTime;

            if(tempTime >= fadeInTime)
            {
                colorGrading.colorFilter.Override(after);
                break;
            }

            colorGrading.colorFilter.Override(before + (temp * fadeCurve.Evaluate(tempTime * tempfade)));
        }
    }

    IEnumerator WarningModeChange(float value)
    {
        float before = vignetting.intensity.value;

        float temp = value - before;

        float startFadeTime = Time.time;
        float tempTime = 0.0f;
        float tempfade = temp * (1 / warningFadeTime);

        while (true)
        {
            yield return null;
            tempTime = Time.time - startFadeTime;

            if (tempTime >= fadeInTime)
            {
                vignetting.intensity.value = value;
                break;
            }

            vignetting.intensity.value = before + (tempTime * tempfade);
        }
    }


}
