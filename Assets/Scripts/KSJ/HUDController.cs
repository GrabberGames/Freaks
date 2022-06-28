using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header ("Waron Skill Icon(Passive/Q/W/E/R)")]
    [SerializeField] private Sprite[] waronSkillIcon = new Sprite[5];

    [Header("Kyle Skill Icon(Passive/Q/W/E/R)")]
    [SerializeField] private Sprite[] kyleSkillIcon = new Sprite[5];
    
    // Start is called before the first frame update
    void Start()
    {
        HUDManager.Instance.SetHeroProfile(GameManager.Instance.selectHero);
        SetActiveSkillSlot(GameManager.Instance.selectHero);        
    }

    // Update is called once per frame
    void Update()
    { 
        HUDManager.Instance.HPVisualization(GameManager.Instance.models.playerModel.PlayerNowHp, GameManager.Instance.models.playerModel.PlayerMaxHp);
        PostProcessManager.Instance.ActiveWarningMode(GameManager.Instance.models.playerModel.PlayerNowHp <= GameManager.Instance.models.playerModel.PlayerMaxHp * 0.25f);
        PostProcessManager.Instance.ActiveDeathMode(GameManager.Instance.models.playerModel.PlayerNowHp <= 0.0f);

        //¸ðµ¨ ÃÖ´ë ÄðÅ¸ÀÓµµ ³Ö¾îÁà¾ß ÇÔ 
        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.Q, GameManager.Instance.models.playerModel.QSkillCoolTime, GameManager.Instance.models.playerModel.QSkillMaxCoolTime);
        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.W, GameManager.Instance.models.playerModel.WSkillCoolTime, GameManager.Instance.models.playerModel.WSkillMaxCoolTime);
        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.E, GameManager.Instance.models.playerModel.ESkillCoolTime, GameManager.Instance.models.playerModel.ESkillMaxCoolTime);
        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.R, GameManager.Instance.models.playerModel.RSkillCoolTime, GameManager.Instance.models.playerModel.RSkillMaxCoolTime);

        HUDManager.Instance.TimeInfoVisualization(StageManager.Instance.nowPlayTime, StageManager.Instance.beforeSpawnTime, StageManager.Instance.spawnIntervalTime);

        HUDManager.Instance.WhiteFreaksInfoVisualization(WhiteFreaksManager.Instance.idleFreaksCount, WhiteFreaksManager.Instance.allFreaksCount);
    }

    public void SetActiveSkillSlot(eHeroType heroType)
    {
        switch(heroType)
        {
            case eHeroType.Waron:
                for (int i = 0; i < waronSkillIcon.Length; i++)
                {
                    HUDManager.Instance.SetSkillIcon((HUDManager.eSkillSlotKey)i, waronSkillIcon[i]);
                }
                break;

            case eHeroType.Kyle:
                for (int i = 0; i < waronSkillIcon.Length; i++)
                {
                    HUDManager.Instance.SetSkillIcon((HUDManager.eSkillSlotKey)i, kyleSkillIcon[i]);
                }
                break;

            default:
                Debug.Log("Herotype value error.");
                break;
        }
    }
}
