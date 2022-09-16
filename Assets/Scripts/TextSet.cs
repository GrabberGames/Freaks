using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSet : MonoBehaviour
{
    public int idx;
    [SerializeField] private Text _text;
    
    public void SetText(int index)
    {
        idx = index;
        if (index == 0)
        {
            _text.text =
                "불멸의 기사단장인 와론은 적을 두려워하지 않고 치열한 전투를 즐기는 강력한 무투파입니다.\n신속하게 적에게 돌격하여 강력한 근접 스킬로 적을 압도하는 것이 와론의 전투 스타일입니다.";
        }
        else if (index == 1)
        {
            _text.text =
                "망각의 재앙의 유일한 생존자인 카일은 권총을 사용하여 날렵하게 다수의 적을 섬멸합니다.\n카일은 멀리서 적의 공격으로부터 벗어나 원거리에서 일방적인 전투를 하는데 능숙합니다.";
        }
    }

    public void SetSkillExplain(string text)
    {
        _text.text = text;
    }
}
