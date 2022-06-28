using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
{
    PD,
    ED,
    HP,
    ARMOR,
    MOVESPEED,
    ATTACKSPEED
}

public class PlayerModel 
{
    #region Region Variable
    private float _qSkillCoolTime= 0f;
    private float _wSkillCoolTime = 0f;
    private float _eSkillCoolTime = 0f;
    private float _rSkillCoolTime = 0f;
    private float _qSkillMaxCoolTime= 0f;
    private float _wSkillMaxCoolTime = 0f;
    private float _eSkillMaxCoolTime = 0f;
    private float _rSkillMaxCoolTime = 0f;
    private float _playerNowHp = 0f;
    private float _playerMaxHp = 0f;
    private float _playerPD = 0f;
    private float _playerED = 0f;
    private float _moveSpeed = 0f;
    private float _attackSpeed = 0f;
    private float _armor = 0f;
    #endregion
    public float QSkillCoolTime
    {
        get => _qSkillCoolTime;
        set => _qSkillCoolTime = value;
    }
    public float WSkillCoolTime
    {
        get => _wSkillCoolTime;
        set => _wSkillCoolTime = value;
    }
    public float ESkillCoolTime
    {
        get => _eSkillCoolTime;
        set => _eSkillCoolTime = value;
    }
    public float RSkillCoolTime
    {
        get => _rSkillCoolTime;
        set => _rSkillCoolTime = value;
    }    
    public float QSkillMaxCoolTime
    {
        get => _qSkillMaxCoolTime;
        set => _qSkillMaxCoolTime = value;
    }
    public float WSkillMaxCoolTime
    {
        get => _wSkillMaxCoolTime;
        set => _wSkillMaxCoolTime = value;
    }
    public float ESkillMaxCoolTime
    {
        get => _eSkillMaxCoolTime;
        set => _eSkillMaxCoolTime = value;
    }
    public float RSkillMaxCoolTime
    {
        get => _rSkillMaxCoolTime;
        set => _rSkillMaxCoolTime = value;
    }
    public float PlayerNowHp
    {
        get => _playerNowHp;
        set => _playerNowHp = value;
    }
    public float PlayerMaxHp
    {
        get => _playerMaxHp;
        set => _playerMaxHp = value;
    }
    public float PlayerED
    {
        get => _playerED;   
        set => _playerED = value;
    }
    public float PlayerPD
    {
        get => _playerPD;
        set => _playerPD = value;
    }
    public float PlayerMoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }
    public float PlayerAttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }
    public float PlayerArmor
    {
        get => _armor;
        set => _armor = value;
    }
    public void IncreaseStatus(StatusType type, float value)
    {
        switch (type)
        {
            case StatusType.PD:
                PlayerPD += value;
                break;
            case StatusType.ED:
                PlayerED += value;
                break;
            case StatusType.HP:
                PlayerNowHp += value;
                PlayerMaxHp += value;
                break;
            case StatusType.ARMOR:
                PlayerArmor += value;
                break;
            case StatusType.MOVESPEED:
                PlayerMoveSpeed += value;
                break;
            case StatusType.ATTACKSPEED:
                PlayerAttackSpeed += value;
                break;
        }
    }
}
