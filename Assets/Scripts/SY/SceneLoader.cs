using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    protected static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                var go = FindObjectOfType<SceneLoader>();
                if (go != null)
                {
                    instance = go;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    WaitForSeconds _oneS = new WaitForSeconds(1f);
    IEnumerator loadCo;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [Header("Hero Portrait")]
    [SerializeField]
    private Sprite[] images;

    [SerializeField]
    private Image image;

    [Header("Load ETC")]
    [SerializeField]
    private Text etcText;
    [SerializeField]
    string[] etcTextList;

    [Header("Load Text")]
    [SerializeField]
    private Text loadText;


    private string loadSceneName;
    static SceneLoader SceneLoaderPrefab;

    public static SceneLoader Create()
    {
        SceneLoaderPrefab = Resources.Load<SceneLoader>("Load");
        return Instantiate(SceneLoaderPrefab);
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
    private IEnumerator ChangeLoadText()
    {
        int cnt = 1;
        while(true)
        {
            loadText.text = "Loading";
            cnt++;
            cnt %= 4;

            for(int i = 0; i < cnt; i++)
            {
                loadText.text += ".";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void LoadScene(string sceneName)
    {
        etcText.text = etcTextList[Random.Range(0, etcTextList.Length)].Replace("|", "\n");
        image.sprite = images[(int)GameManager.Instance.selectHero - 1];
        loadCo = ChangeLoadText();
        StartCoroutine(loadCo);

        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProgress(sceneName));
    }
    private IEnumerator LoadSceneProgress(string sceneName)
    {
        AudioManager.Instance.Load(GameManager.Instance.selectHero.ToString());

        yield return StartCoroutine(Fade(true));
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        oper.allowSceneActivation = false;

        float amount = 0.0f;

        while (true)
        {
            yield return null;

            if (oper.progress < 0.9f)
            {
                amount = oper.progress;
            }
            else
            {
                amount += Time.unscaledDeltaTime;
                if (amount >= 1f)
                {
                    StopCoroutine(loadCo);
                    oper.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            StopCoroutine(loadCo);
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
