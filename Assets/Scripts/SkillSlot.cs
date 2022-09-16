using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TextSet _textSet;
    [SerializeField] private string _Warrontext;
    [SerializeField] private string _Kyletext;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_textSet.idx == 0)
            _textSet.SetSkillExplain(_Warrontext);
        
        else if (_textSet.idx == 1)
            _textSet.SetSkillExplain(_Kyletext);
    }
}
