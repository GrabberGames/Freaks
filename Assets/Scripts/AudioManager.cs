using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource[] sfxPlayer;

    AudioSource audioSource;

    public void Read(string text)
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if(text.IndexOf("!") != -1 || text.IndexOf("?") != -1)
        {
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[!?]", "");
        }
        for(int i = 0; i < sfxPlayer.Length; i++)
        {
            if(!sfxPlayer[i].isPlaying)
            {
                audioSource.clip = Resources.Load<AudioClip>("Audios/" + text);
                audioSource.Play();
                return;
            }
        }
    }
    private void Start()
    {
        instance = this;
    }
}