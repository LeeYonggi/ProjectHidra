using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UnitAttackState
{
    void Attack(GameObject target, int damage);
}

public class NoneThrowRangeAttack : UnitAttackState
{
    public void Attack(GameObject target, int damage)
    {
        target.GetComponent<ObjectStatus>().Hp -= damage;
    }
}