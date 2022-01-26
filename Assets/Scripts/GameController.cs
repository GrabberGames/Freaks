using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;



public class GameController : MonoBehaviour
{
    public static string hero;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI[] SkillCoolTimers;
    public Image[] SkillCoolMasks;
    public Image mask;
    public int wave_min = 0;
    public int wave_sec = 0;

    // FX
    [SerializeField] private ParticleSystem fx_Move;

    // wShot the ray to the pos. of Mouse Pointer Clicked.
    private SpawnController spawnController;
    private Kali kyle;
    private WarriorAnims.HeroMovement waron; 
    private RaycastHit hit;

    private string hitColliderName;
    private int min = 0;
    private int sec = 0;
    private float originalSize;
    private float value = 0;
    private float[] skillSizes = new float[4];

    /*
    private void Awake() {
        if(GameController == null) {
            GameController = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    */


    public void Init() {
        switch (GameController.hero)
        {
            case "kyle" :
                kyle = GameObject.Find("kyle").GetComponent<Kali>();
                for (int i = 0; i < 4; i++) {
                    skillSizes[i] = SkillCoolMasks[i].rectTransform.rect.height;
                } 
                break;
            
            case "waron" :
                waron = GameObject.Find("warron").GetComponent<WarriorAnims.HeroMovement>();
                for (int i = 4; i < 8; i++) {
                    skillSizes[i] = SkillCoolMasks[i].rectTransform.rect.height;
                } 
                break;

            default:
                break;

        }
    }


    private void Start()
    {
        originalSize = mask.rectTransform.rect.width;

        StartCoroutine(PlayTimer());
        StartCoroutine(WaveTimer());
    }


    private void Update()
    {
        FXmovePlayer();
        if (GameController.hero == "kyle") {
            kailSCool();
            kailSCoolBtn();
        }
        else if (GameController.hero == "waron") {
            waronSCool();
            waronSCoolBtn();
        }
    }


    IEnumerator PlayTimer()
    {
        while(true)
        {
            timerText.text = string.Format("{0:D1}:{1:D2}", min, sec);
            
            sec++;
            
            if (sec > 59)
            {
                sec = 0;
                min++;
            }
            
            yield return new WaitForSeconds(1);
        } 
    }


    IEnumerator WaveTimer()
    {
        // 2mins Timer
        while(wave_min < 2)
        {
            wave_sec++;
            value += 0.825f;

            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize - value);

            if (wave_sec > 59)
            {
                wave_sec = 0;
                wave_min++;
            }
            yield return new WaitForSeconds(1);
        }

        if (wave_min >= 2)
        {
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize);
            GameManager.Spawn.FreaksSpawn();
            wave_min = 0;
            wave_sec = 0;
            StartCoroutine(WaveTimer());
            value = 0;
        }
    }


    // FX Play on Mouse Click pos.
    private void FXmovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Vector3 mPos;

            int layerMask = (1 << LayerMask.NameToLayer("Building")) + (1 << LayerMask.NameToLayer("Walkable"));

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity,layerMask))
            {
                mPos = hit.point; mPos.y = 0.3f;
                fx_Move.transform.position = mPos;
                fx_Move.Play(true);
            }
        }

        if (fx_Move.isStopped)
        {
            fx_Move.Stop();
        }
    }


    // Skill Cool Timer UI Controll; Kail
    public void kailSCool(){
        // CoolTime Text print
        SkillCoolTimers[0].text = string.Format("{0:N1}", kyle.getTimer("Q"));
        SkillCoolTimers[1].text = string.Format("{0:N1}", kyle.getTimer("W"));
        SkillCoolTimers[2].text = string.Format("{0:N1}", kyle.getTimer("E"));
        SkillCoolTimers[3].text = string.Format("{0:N1}", kyle.getTimer("R"));
    }

        // Skill Cool Timer UI Controll; waron
    public void waronSCool(){
        // CoolTime Text print
        SkillCoolTimers[4].text = string.Format("{0:N1}", waron.getTimer("Q"));
        SkillCoolTimers[5].text = string.Format("{0:N1}", waron.getTimer("W"));
        SkillCoolTimers[6].text = string.Format("{0:N1}", waron.getTimer("E"));
        SkillCoolTimers[7].text = string.Format("{0:N1}", waron.getTimer("R"));
    }
    

    public void kailSCoolBtn() {
        // CoolTime Gauge(mask) handler
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(ScoolMask(0.367f, "Q", 0, "kyle"));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(ScoolMask(0.267f, "W", 1, "kyle"));

        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ScoolMask(0.250f, "E", 2, "kyle"));

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ScoolMask(0.045f, "R", 3, "kyle"));
        }
    }

    public void waronSCoolBtn() {
        // CoolTime Gauge(mask) handler
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(ScoolMask(0.367f, "Q", 0, "waron"));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(ScoolMask(0.267f, "W", 1, "waron"));

        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ScoolMask(0.250f, "E", 2, "waron"));

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ScoolMask(0.045f, "R", 3, "waron"));
        }
    }


    IEnumerator ScoolMask(float val, string skill, int indx, string hero)
    {
        float _val = val;

        if (hero == "kyle") {
            while (kyle.getTimer(skill) > 0.1f)
            {
                SkillCoolMasks[indx].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, skillSizes[indx] - val);
                val += _val;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (hero == "waron") {
            while (waron.getTimer(skill) > 0.1f)
            {
                SkillCoolMasks[indx].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, skillSizes[indx] - val);
                val += _val;
                yield return new WaitForSeconds(0.1f);
            }
        }

        
    }




}
