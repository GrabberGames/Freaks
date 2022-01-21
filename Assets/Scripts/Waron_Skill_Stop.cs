using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waron_Skill_Stop : MonoBehaviour
{
    public WarriorAnims.HeroMovement hero;

    public void WaronBasicAttackStop()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
            print(hero);
        }
        hero.Normal_Attack_Fun();
    }
    public void Waron_Q_Stop()
    {
        if(hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
            print(hero);
        }
        hero.ThrowingRock_Stop();
    }
    public void Waron_W_Stop_1()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
        }
        hero.LeafOfPride_Stop_1();
    }
    public void Waron_W_Stop_2()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
        }
        hero.LeafOfPride_Stop_2();
    }
    public void Waron_E_Stop()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
        }
        hero.BoldRush_Stop();
    }
    public void Waron_R_Stop()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
        }
        hero.ShockOfLand_Stop();
    }
}
