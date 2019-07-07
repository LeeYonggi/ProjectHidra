using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectStatus : MonoBehaviour
{
    public enum TEAM_KIND
    {
        TEAM_NONE,
        TEAM_RED,
        TEAM_BLUE
    }

    public TEAM_KIND teamKind;

    public int basicHp;
    public int basicDefence;
    public int basicAttack;
    public int basicCost;
    
    private int hp;
    private int defence;
    private int attack;

    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp < 0)
                hp = 0;
        }
    }

    public int Defence { get => defence; set => defence = value; }
    public int Attack { get => attack; set => attack = value; }

    public ObjectStatus()
    {
    }

    public void ChangeStatus(int _hp, int _defence, int _attack)
    {
        Hp = _hp;
        Defence = _defence;
        Attack = _attack;
    }

    private void Start()
    {
        Hp = basicHp;
        Defence = basicDefence;
        Attack = basicAttack;
    }
}
