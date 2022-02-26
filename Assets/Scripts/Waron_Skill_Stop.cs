using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waron_Skill_Stop : MonoBehaviour
{
    public WarriorAnims.HeroMovement hero;

    public void Waron_Q_Rock_Spawn()
    {
        if(hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
            print(hero);
        }
        hero.ThrowingRockSpawn();
    }
    public void Waron_Q_Rock_Throw()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
            print(hero);
        }
        hero.ThrowingRockThrow();
    }
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
    public void Warin_E_Particle()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
        }
        hero.BoldRushParticleStart();
    }
    public void Waron_E_Stop()
    {
        if (hero == null)
        {
            hero = GetComponentInParent<WarriorAnims.HeroMovement>();
        }
        hero.BoldRush_Stop();
        hero.BoldRushParticleEnd();
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
