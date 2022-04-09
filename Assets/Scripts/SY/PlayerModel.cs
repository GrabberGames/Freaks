using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel 
{
    #region Region Variable
    private float _qSkillCoolTime = 0f;
    private float _wSkillCoolTime = 0f;
    private float _eSkillCoolTime = 0f;
    private float _rSkillCoolTime = 0f;
    private float _playerNowHp = 0f;
    private float _playerMaxHp = 0f;
    private float _playerPD = 0f;
    private float _playerED = 0f;
    #endregion
    public float qSkillCoolTime
    {
        get => _qSkillCoolTime;
        set => _qSkillCoolTime = value;
    }
    public float wSkillCoolTime
    {
        get => _wSkillCoolTime;
        set => _wSkillCoolTime = value;
    }
    public float eSkillCoolTime
    {
        get => _eSkillCoolTime;
        set => _eSkillCoolTime = value;
    }
    public float rSkillCoolTime
    {
        get => _rSkillCoolTime;
        set => _rSkillCoolTime = value;
    }
    public float playerNowHp
    {
        get => _playerNowHp;
        set => _playerNowHp = value;
    }
    public float playerMaxHp
    {
        get => _playerMaxHp;
        set => _playerMaxHp = value;
    }
    public float playerED
    {
        get => _playerED;   
        set => _playerED = value;
    }
    public float playerPD
    {
        get => _playerPD;
        set => _playerPD = value;
    }
}
