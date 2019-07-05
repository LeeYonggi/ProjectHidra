using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleUnit : Unit
{
    public RifleUnit() : base(new NoneThrowRangeAttack())
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Init();

    }

    // Update is called once per frame
    void Update()
    {
        base.UnitUpdate();
    }

    public bool IsRifleAttack(GameObject target)
    {
        if(target.tag == "Unit" && target.GetComponent<Unit>().Status.teamKind != Status.teamKind)
        {
            return true;
        }
        else if(target.tag == "Building" && target.GetComponent<Structure>().Status.teamKind != Status.teamKind)
        {
            return true;
        }
        return false;
    }

    public override bool IsAttackPossible(GameObject target)
    {
        if (IsRifleAttack(target))
        {
            return true;
        }
        return false;
    }
}
