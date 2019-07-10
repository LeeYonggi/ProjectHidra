using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UnitMove : UnitStateMachine
{
    private List<Vector2> path = new List<Vector2>();
    private int nowPath = 0;
    private Unit myUnit = null;
    private GameObject targetObject = null;

    public UnitMove(Vector2 _target, Unit unit)
    {
        UnitAstarMove(_target, unit);
        myUnit = unit;
    }
    
    public void UnitAstarMove(Vector2 _target, Unit unit)
    {
        path = AstarManager.Instance.AstarPathFinder(unit.transform.position, _target);

        nowPath = path.Count - 1;
    }

    public void Update(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.MOVE);
        unit.Animator.SetFloat("MoveSpeed", unit.Speed * 2.0f);
        unit.WeaponAnimator.SetBool("IsAttack", false);

        //unit.transform.position = Vector2.MoveTowards(unit.transform.position, , unit.Speed * Time.deltaTime);

        bool isEndMove = PlayerMoveTowards(unit, path[nowPath], 0.1f, unit.Speed * Time.deltaTime);

        if (isEndMove)
        {
            if (nowPath != 0)
                nowPath -= 1;
            else
            {
                unit.ChangeStateMachine(new UnitIdle());
            }
        }
    }

    public bool PlayerMoveTowards(Unit unit, Vector2 target, float distance, float speed)
    {
        if (Vector2.Distance(unit.transform.position, target) > distance)
        {
            Vector2 positionVec2 = new Vector2(unit.transform.position.x, unit.transform.position.y);

            unit.MoveVector = target - positionVec2;
            unit.MoveVector = unit.MoveVector.normalized;

            unit.transform.position = Vector2.MoveTowards(unit.transform.position, target, speed);

            return false;
        }
        return true;
    }

    public void SendMessage(GameObject obj, string message)
    {
        if (message == "AddTarget" && targetObject != obj)
        {
            UnitAstarMove(obj.transform.position, myUnit);
            targetObject = obj;
        }
    }
}