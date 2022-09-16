using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextSet _textSet;
    [SerializeField] private int idx;

    private string myText = "";
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetText();
        
        if(_textSet.idx == 0)
            _textSet.SetSkillExplain(myText);
        
        else if (_textSet.idx == 1)
            _textSet.SetSkillExplain(myText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _textSet.SetText(_textSet.idx);
    }
    

    public void SetText()
    {
        if (_textSet.idx == 0)
        {
            switch (idx)
            {
                case 0 :
                    myText = "와론이 땅에서 바위를 들어 올려\n 적을 향해 직선으로 던집니다.";
                    break;
                case 1 :
                    myText = "와론이 전방으로 높이 뛰어오르며\n 범위 내의 적 다수를 내려찍어 대미지를 입힙니다.";
                    break;
                case 2 :
                    myText = "와론이 잠시 몸을 낮춰 자세를 취하고\n 전방을 향해 순식간에 돌진합니다.";
                    break;
                case 3 :
                    myText = "와론이 뜨거운 대지의 정수를 흡수하여\n 자신의 주변에 불타오르는 오라를 두릅니다.";
                    break;
            }
        } 
        else if (_textSet.idx == 1)
        {
            switch (idx)
            {
                case 0 :
                    myText = "카일이 자세를 잡고 전방을 향해\n 자신의 의지가 담긴 강력한 탄환을 하나 발사합니다.";
                    break;
                case 1 :
                    myText = "카일이 본인 주변에 쌍권총을 한 바퀴 돌며 난사합니다.";
                    break;
                case 2 :
                    myText = "카일이 전방을 향해 다섯 발의 탄환을 난사하며\n 순식간에 몸을 뒤로 던집니다.";
                    break;
                case 3 :
                    myText = "카일이 맵의 특정 위치를 지정하고 하늘을 향해\n 자신의 정수를 담은 특수 탄환을 하나 발사합니다.";
                    break;
            }
        }
    }
}
