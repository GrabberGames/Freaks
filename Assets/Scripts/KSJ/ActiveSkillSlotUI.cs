using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveSkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image coolTimeCover;

    [SerializeField] private TextMeshProUGUI coolTimeText;

    private Color cooldownSkillIconColor;


    public void SetSkillIcon(Sprite _skillIcon)
    {
        skillIcon.sprite = _skillIcon;
    }

    public void SetCooldownSkillIconColor(Color _color)
    {
        cooldownSkillIconColor = _color;
    }

    public void UseSkill()
    {
        skillIcon.color = cooldownSkillIconColor;
    }
       
    public void Visualization(float _remainingTime, float _fill)
    {
        if(_fill < 0.0f || _fill > 1.0f)
        {
            Debug.Log("input fill amount over value");
            return;
        }

        coolTimeCover.fillAmount = _fill;

        if(_remainingTime.Equals(0.0f))
        {
            skillIcon.color = Color.white;
            coolTimeText.text = "";
        }
        else
        {
            coolTimeText.text = string.Format("{0:N1}", _remainingTime);
        }
    }    
}
