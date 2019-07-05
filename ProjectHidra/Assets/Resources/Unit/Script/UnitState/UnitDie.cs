using UnityEngine;
using UnityEditor;


public class UnitDie : UnitStateMachine
{
    public UnitDie(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.DIE);
    }
    public void Update(Unit unit)
    {
        if (unit.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f &&
            !unit.Animator.IsInTransition(0))
        {
            Object.Destroy(unit.gameObject);
            unit.ChangeStateMachine(null);
        }
    }

    public void SendMessage(GameObject obj, string message)
    {

    }
}