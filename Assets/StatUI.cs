using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    [SerializeField]
    Text PDText;
    [SerializeField]
    Text EDText;
    [SerializeField]
    Text ARMORText;
    [SerializeField]
    Text MOVESPEEDText;
    [SerializeField]
    Text ATTACKSPEEDText;
    // Start is called before the first frame update
    void Start()
    {
        PDText.text = GameManager.Instance.models.playerModel.PlayerPD.ToString();
        EDText.text = GameManager.Instance.models.playerModel.PlayerED.ToString();
        ARMORText.text = GameManager.Instance.models.playerModel.PlayerArmor.ToString();
        MOVESPEEDText.text = GameManager.Instance.models.playerModel.PlayerMoveSpeed.ToString();
        ATTACKSPEEDText.text = GameManager.Instance.models.playerModel.PlayerAttackSpeed.ToString();

        GameManager.Instance.models.playerModel.StatChanged -= StatChanged;
        GameManager.Instance.models.playerModel.StatChanged += StatChanged;
    }
    private void StatChanged(StatusType type)
    {
        switch(type)
        {
            case StatusType.PD:
                PDText.text = GameManager.Instance.models.playerModel.PlayerPD.ToString();
                PDText.color = new Color32(255, 246, 154,255);
                    break;
            case StatusType.ED:
                EDText.text = GameManager.Instance.models.playerModel.PlayerED.ToString();
                EDText.color = new Color32(255, 246, 154, 255);
                break;
            case StatusType.ARMOR:
                ARMORText.text = GameManager.Instance.models.playerModel.PlayerArmor.ToString();
                ARMORText.color = new Color32(255, 246, 154, 255);
                break;
            case StatusType.MOVESPEED:
                MOVESPEEDText.text = GameManager.Instance.models.playerModel.PlayerMoveSpeed.ToString();
                MOVESPEEDText.color = new Color32(255, 246, 154, 255);
                break;
            case StatusType.ATTACKSPEED:
                ATTACKSPEEDText.text = GameManager.Instance.models.playerModel.PlayerAttackSpeed.ToString();
                ATTACKSPEEDText.color = new Color32(255, 246, 154, 255);
                break;
        }
    }
}
