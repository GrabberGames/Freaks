using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{   
    public struct ActiveSkillSlot
    {
        private ActiveSkillSlotUI _activeSkillSlotUI;
        public ActiveSkillSlotUI activeSkillSlotUI { get => _activeSkillSlotUI; }
        public bool canUse;

        public void SetActiveSkillSlotUI(ActiveSkillSlotUI activeSkillSlotUI)
        {
            _activeSkillSlotUI = activeSkillSlotUI;
        }
    }



    public enum eSkillSlotKey { Q, W, E, R };

    [Header("HP Bar")]
    [SerializeField] private HpUI hpUI;

    [Header("Skill Slot")]
    [SerializeField] private ActiveSkillSlotUI activeSkillSlotQ;
    [SerializeField] private ActiveSkillSlotUI activeSkillSlotW;
    [SerializeField] private ActiveSkillSlotUI activeSkillSlotE;
    [SerializeField] private ActiveSkillSlotUI activeSkillSlotR;

    [Header("Cooldown Skill Icon Color")]
    [SerializeField] private Color coolDownSkillColor;


    private ActiveSkillSlot[] activeSkillSlotArray = new ActiveSkillSlot[4];


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
        activeSkillSlotArray[0].SetActiveSkillSlotUI(activeSkillSlotQ);
        activeSkillSlotArray[1].SetActiveSkillSlotUI(activeSkillSlotW);
        activeSkillSlotArray[2].SetActiveSkillSlotUI(activeSkillSlotE);
        activeSkillSlotArray[3].SetActiveSkillSlotUI(activeSkillSlotR);

        for (int i = 0; i < activeSkillSlotArray.Length; i++)
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
