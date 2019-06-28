using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus
{
    public enum TEAM_KIND
    {
        TEAM_NONE,
        TEAM_RED,
        TEAM_BLUE
    }

    public TEAM_KIND teamKind;
    public int hp;
    public int defence;
    public int attack;

    public ObjectStatus(TEAM_KIND kind, int _hp, int _defence, int _attack)
    {
        teamKind = kind;
        hp = _hp;
        defence = _defence;
        attack = _attack;
    }

    public void ChangeTeam(TEAM_KIND team)
    {
        teamKind = team;
    }
    public void ChangeStatus(int _hp, int _defence, int _attack)
    {
        hp = _hp;
        defence = _defence;
        attack = _attack;
    }
}
