using UnityEngine;
using UnityEditor;


public class UnitIdle : UnitStateMachine
{
    public void Update(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.IDLE);
        unit.WeaponAnimator.SetBool("IsAttack", false);

        GameObject target = FindShortDistanceBuilding(unit);
        if (target)
        {
            UnitMove unitMove = new UnitMove(target.transform.position, unit);
            unit.ChangeStateMachine(unitMove);
        }
    }

    public GameObject FindShortDistanceBuilding(Unit unit)
    {
        GameObject[] building = GameObject.FindGameObjectsWithTag("Building");

        float shortDistance = -1;
        GameObject target = null;

        for (int i = 0; i < building.Length; i++)
        {
            if (building[i].GetComponent<ObjectStatus>().teamKind == unit.Status.teamKind)
                continue;
            float distance = Vector2.Distance(building[i].transform.position, unit.transform.position);

            if (distance < shortDistance || shortDistance == -1)
            {
                shortDistance = distance;
                target = building[i];
            }
        }
        return target;
    }

    public void SendMessage(GameObject obj, string message)
    {

    }
}