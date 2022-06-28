using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{   
    public struct ActiveSkillSlot
    {
        private SkillSlotUI _activeSkillSlotUI;
        public SkillSlotUI activeSkillSlotUI { get => _activeSkillSlotUI; }
        public bool canUse;

        public void SetActiveSkillSlotUI(SkillSlotUI activeSkillSlotUI)
        {
            _activeSkillSlotUI = activeSkillSlotUI;
        }
    }

    public enum eSkillSlotKey { Passive, Q, W, E, R, MaxCount };

    [Header("TimeInfo")]
    [SerializeField] private TimeInfoUI timeInfoUI;

    [Header("HP Bar")]
    [SerializeField] private HpUI hpUI;
    
    [Header("Essence")]
    [SerializeField] private EssenceUI essenceUI;

    [Header("WhiteFreaksInfo")]
    [SerializeField] private WhiteFreaksInfoUI WhiteFreaksInfoUI;

    [Header("Skill Slot")]
    [SerializeField] private SkillSlotUI passiveSkillSlot;
    [SerializeField] private SkillSlotUI activeSkillSlotQ;
    [SerializeField] private SkillSlotUI activeSkillSlotW;
    [SerializeField] private SkillSlotUI activeSkillSlotE;
    [SerializeField] private SkillSlotUI activeSkillSlotR;

    [Header("Hero Profile")]
    [SerializeField] private HeroProfileUI heroProfileUI;

    [Header("Cooldown Skill Icon Color")]
    [SerializeField] private Color coolDownSkillColor;

    [Header("Revive Timer")]
    [SerializeField] private Text reviveText;


    private ActiveSkillSlot[] activeSkillSlotArray = new ActiveSkillSlot[5];


    //singleton
    private static HUDManager mInstance;
    public static HUDManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<HUDManager>();
            }
            return mInstance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        activeSkillSlotArray[(int)eSkillSlotKey.Passive].SetActiveSkillSlotUI(passiveSkillSlot);
        activeSkillSlotArray[(int)eSkillSlotKey.Q].SetActiveSkillSlotUI(activeSkillSlotQ);
        activeSkillSlotArray[(int)eSkillSlotKey.W].SetActiveSkillSlotUI(activeSkillSlotW);
        activeSkillSlotArray[(int)eSkillSlotKey.E].SetActiveSkillSlotUI(activeSkillSlotE);
        activeSkillSlotArray[(int)eSkillSlotKey.R].SetActiveSkillSlotUI(activeSkillSlotR);

        for (int i = 1; i < activeSkillSlotArray.Length; i++)
        {
            activeSkillSlotArray[i].activeSkillSlotUI.SetCooldownSkillIconColor(coolDownSkillColor);
        }
    }

    public void SetHeroProfile(eHeroType hero)
    {
        heroProfileUI.SetProfile(hero);
    }

    public void SetSkillIcon(eSkillSlotKey key, Sprite skillIcon)
    {
        activeSkillSlotArray[(int)key].activeSkillSlotUI.SetSkillIcon(skillIcon);
    }

    public void CoolTimeVisualization(eSkillSlotKey key, float elapsedtime, float cooltime)
    {
        activeSkillSlotArray[(int)key].activeSkillSlotUI.Visualization(elapsedtime, elapsedtime / cooltime);
    }

    public void HPVisualization(float nowHP, float maxHP)
    {
        hpUI.Visualization(nowHP, nowHP / maxHP);
    }

    public void TimeInfoVisualization(int nowPlayTime, int beforeSpawnTime, int spawnIntervalTime)
    {
        timeInfoUI.WaveGaugeVisualization((float)beforeSpawnTime, (float)nowPlayTime, (float)spawnIntervalTime);
        timeInfoUI.PlayTimeTextVisualization(nowPlayTime);
    }

    public void EssenceVisualization(int value)
    {
        essenceUI.Visualization(value);
    }

    public void WhiteFreaksInfoVisualization(int idleCount, int maxCount )
    {
        WhiteFreaksInfoUI.Visualization(idleCount, maxCount);
    }
    public void SetReviveTimer(float time)
    {
        reviveText.text = time.ToString();
    }
    public void ActiveReviveTimer(bool b)
    {
        reviveText.gameObject.SetActive(b);
    }
    IEnumerator CoolTimeVisualization()
    {
        while(true)
        {
            yield return null;
            //플레이어에게 받아옴
        }        
    }


}
