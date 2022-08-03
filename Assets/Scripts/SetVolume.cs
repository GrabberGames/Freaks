using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public Text text;

    public AudioMixer mixer;
    

    public void SetLevel(float sliderVal)
    {
        mixer.SetFloat("masterVolume", Mathf.Log10(sliderVal)*20);
        text.text = ((int)(sliderVal * 100)).ToString();
    }
}
