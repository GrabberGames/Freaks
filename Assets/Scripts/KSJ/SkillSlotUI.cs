using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image coolTimeCover;

    [SerializeField] private TextMeshProUGUI coolTimeText;

    private Color cooldownSkillIconColor;


    public void SetSkillIcon(Sprite skillIcon)
    {
        this.skillIcon.sprite = skillIcon;
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
        }
        else
        {
            skillIcon.color = cooldownSkillIconColor;
            coolTimeText.text = string.Format("{0:N1}", remainingTime);
        }
    }    
}
