using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    static AudioManager instance = null;
    public static AudioManager Instance
    {
        get
        {
            Init();
            return instance;
        }
    }

    [SerializeField] AudioSource sfxPlayer;

    Dictionary<string, AudioClip> data = new Dictionary<string, AudioClip>();

    bool SoundIsEnd = false;
    static void Init()
    {
        if (instance == null)
        {
            GameObject ob_go = GameObject.Find("@AudioManager");

            if (ob_go == null)
            {
                 ob_go = new GameObject { name = "@AudioManager" };
                 ob_go.AddComponent<ObjectPooling>();
             }
             DontDestroyOnLoad(ob_go);
             instance = ob_go.GetComponent<AudioManager>();
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
    public void Load(string hero)
    {
        object[] tmp = Resources.LoadAll($"Audios/{hero}");

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
    public void Read(string hero, string text)
    {
        if (data.Count == 0)
            Load(hero);

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
    private void Awake()
    {
        instance = this;
        //Load();
    }
}