using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackRader : MonoBehaviour
{
    [SerializeField]
    private Unit unit = null;
    [SerializeField]
    private float radius;

    private GameObject targetObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AttackRader();
    }

    void AttackRader()
    {
        if (unit.UnitStateMachine.ToString() == "UnitAttackMachine") return;

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        float distance = radius;
        GameObject target = null;

        for (int i = 0; i < buildings.Length; i++)
        {
            if (unit.IsAttackPossible(buildings[i]) == false)
                continue;

            float targetDistance = Vector2.Distance(transform.position, buildings[i].transform.position);

            if (distance <= targetDistance)
                continue;

            distance = targetDistance;
            target = buildings[i];
        }

        for (int i = 0; i < units.Length; i++)
        {
            if (unit.IsAttackPossible(units[i]) == false)
                continue;

            float targetDistance = Vector2.Distance(transform.position, units[i].transform.position);

            if (distance <= targetDistance)
                continue;

            distance = targetDistance;
            target = units[i];
        }

        if (target != null)
        {
            unit.ChangeStateMachine(new UnitAttackMachine(unit, target));
        }
    }
}
