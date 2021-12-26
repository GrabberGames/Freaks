using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class CharacterAttack : MonoBehaviour
{
    Stat white_stat = ObjectPooling.instance.Get_Stat("whitefreaks");
    Stat black_stat = ObjectPooling.instance.Get_Stat("blackfreaks");
    Stat waron_stat = ObjectPooling.instance.Get_Stat("waron");
    Stat kail_stat = ObjectPooling.instance.Get_Stat("kail");
    Stat alter_stat = ObjectPooling.instance.Get_Stat("alter");

    private GameObject alter = GameObject.Find("Alter");
    private GameObject white_freaks = GameObject.Find("White Freaks");
    private GameObject black_freaks = GameObject.Find("Black Freaks");
    private GameObject kail = GameObject.Find("Kail");
    private GameObject waron = GameObject.Find("Waron");
    void Attack(GameObject near, float amount)
    {
        if (near == kail)
        {
            kail_stat.hp -= amount;
        }
        else if (near == waron)
        {
            waron_stat.hp -= amount;
        }
        else if (near == alter)
        {
            alter_stat.hp -= amount;
        }
        else if (near == black_freaks)
        {
            black_stat.hp -= amount;
        }
        else
        {
            white_stat.hp -= amount;
        }
    }
}