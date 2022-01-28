using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    static AudioManager a_instance = null;
    public static AudioManager a_Instance
    {
        get
        {
            Init();
            return a_instance;
        }
    }

    [SerializeField] AudioSource sfxPlayer;

    Dictionary<string, AudioClip> data = new Dictionary<string, AudioClip>();

    bool SoundIsEnd = false;
    static void Init()
    {
        if (a_instance == null)
        {
            GameObject ob_go = GameObject.Find("@AudioManager");

            if (ob_go == null)
            {
                 ob_go = new GameObject { name = "@AudioManager" };
                 ob_go.AddComponent<ObjectPooling>();
             }
             DontDestroyOnLoad(ob_go);
             a_instance = ob_go.GetComponent<AudioManager>();
        }
    }
    public void Stop()
    {
        sfxPlayer.Pause();
    }
    public void Play() 
    {
        sfxPlayer.Play();
    }
    public void Load()
    {
        object[] tmp = Resources.LoadAll("Audios");

        for(int i = 0; i < tmp.Length; i++)
        {
            AudioClip audioClip = tmp[i] as AudioClip;
            string audioName = tmp[i].ToString();
            audioName = audioName.Replace(" (UnityEngine.AudioClip)", "");
            data.Add(audioName, audioClip);
        }
    }
    public bool Check()
    {
        if (sfxPlayer.isPlaying)
            SoundIsEnd = false;
        else
            SoundIsEnd = true;
        return SoundIsEnd;
    }
    public void Read(string text)
    {
        if (data.Count == 0)
            Load();


        var enumData = data.GetEnumerator();

        while (enumData.MoveNext())
        {
            if (enumData.Current.Key == text)
            {
                sfxPlayer.clip = enumData.Current.Value;
                sfxPlayer.Play();
                return;
            }
        }
        return;
    }
    private void Start()
    {
        a_instance = this;
    }
}