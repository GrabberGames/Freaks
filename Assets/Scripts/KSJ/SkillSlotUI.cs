using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private bool isPassive;

    [SerializeField] private GameObject canUseEffect;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image coolTimeCover;    

    [SerializeField] private Text coolTimeText;

    private bool _skillReady;

    private Color cooldownSkillIconColor;


    public void SetSkillIcon(Sprite skillIcon)
    {
        this.skillIcon.sprite = skillIcon;
        if (isPassive)
            this.skillIcon.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void SetCooldownSkillIconColor(Color color)
    {
        cooldownSkillIconColor = color;
    }



    public void UseSkill()
    {
        skillIcon.color = cooldownSkillIconColor;
    }
       
    public void Visualization(float remainingTime, float fill)
    {
        if(fill < 0.0f || fill > 1.0f)
        {
            Debug.Log("input fill amount over value");
            return;
        }

        coolTimeCover.fillAmount = fill;

        if(remainingTime.Equals(0.0f))
        {
            skillIcon.color = Color.white;
            coolTimeText.text = "";
            ChangeSkillFrame(true);
        }
        else
        {
            skillIcon.color = cooldownSkillIconColor;
            coolTimeText.text = string.Format("{0:N1}", remainingTime);
            ChangeSkillFrame(false);
        }
    }
    
    private void ChangeSkillFrame(bool value)
    {
        if (!_skillReady.Equals(value))
        {
            canUseEffect.SetActive(value);
            _skillReady = value;
        }
    }
}
