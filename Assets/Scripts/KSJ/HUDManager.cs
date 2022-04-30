using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("HP Bar")]
    [SerializeField] private HpUI hpUI;

    [Header("Skill Slot")]
    [SerializeField] private SkillSlotUI passiveSkillSlot;
    [SerializeField] private SkillSlotUI activeSkillSlotQ;
    [SerializeField] private SkillSlotUI activeSkillSlotW;
    [SerializeField] private SkillSlotUI activeSkillSlotE;
    [SerializeField] private SkillSlotUI activeSkillSlotR;

    [Header("Cooldown Skill Icon Color")]
    [SerializeField] private Color coolDownSkillColor;


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


    IEnumerator CoolTimeVisualization()
    {
        while(true)
        {
            yield return null;
            //플레이어에게 받아옴

        }        
    }


}
