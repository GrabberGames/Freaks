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
        SetActiveSkillSlot(GameManager.Instance.selectHero);
    }

    // Update is called once per frame
    void Update()
    {
        HUDManager.Instance.HPVisualization(GameManager.Instance.models.playerModel.playerNowHp, GameManager.Instance.models.playerModel.playerMaxHp);

        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.Q, GameManager.Instance.models.playerModel.qSkillCoolTime, 11.0f);
        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.W, GameManager.Instance.models.playerModel.wSkillCoolTime, 15.0f);
        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.E, GameManager.Instance.models.playerModel.eSkillCoolTime, 16.0f);
        HUDManager.Instance.CoolTimeVisualization(HUDManager.eSkillSlotKey.R, GameManager.Instance.models.playerModel.rSkillCoolTime, 120.0f);

        HUDManager.Instance.TimeInfoVisualization(StageManager.Instance.nowPlayTime, StageManager.Instance.beforeSpawnTime, StageManager.Instance.spawnIntervalTime);
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
