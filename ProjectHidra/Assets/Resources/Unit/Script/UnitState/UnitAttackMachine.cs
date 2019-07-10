using UnityEngine;
using UnityEditor;

public class UnitAttackMachine : UnitStateMachine
{
    private GameObject target = null;
    Unit myUnit = null;
    float time = 0;

    public UnitAttackMachine(Unit unit, GameObject _target)
    {
        if (!_target)
        {
            unit.ChangeStateMachine(new UnitIdle());
            return;
        }
        target = _target;
        myUnit = unit;
        if (unit.WeaponAnimator)
            unit.WeaponAnimator.SetBool("IsAttack", true);
    }

    public void Update(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.ATTACK);
        unit.Animator.SetFloat("AttackSpeed", unit.ShootSpeed);
        unit.WeaponAnimator.SetFloat("AttackSpeed", unit.ShootSpeed);

        if (!target)
        {
            unit.ChangeStateMachine(new UnitIdle());
            return;
        }

        Vector2 diff = target.transform.position - unit.transform.position;
        diff.Normalize();

        if (unit.Weapon != null)
        {
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            // 위를 가르킬 때 변경
            SpriteRenderer weaponSprite = unit.WeaponSprite;
            if (rot_z >= 0)
                weaponSprite.sortingLayerName = "BackGun";
            else
                weaponSprite.sortingLayerName = "FrontGun";

            // 방향 바꾸기
            if (rot_z >= 90 || rot_z <= -90)
            {
                rot_z = rot_z - 180;
                unit.ChangeFlip(new Vector2(-1, 1));
            }
            else
                unit.ChangeFlip(new Vector2(1, 1));

            unit.Weapon.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }

        time += Time.deltaTime;
        if (unit.UnitAttack != null && time >= (0.2f / unit.ShootSpeed))
        {
            myUnit.UnitAttack.Attack(target, myUnit.Status.Attack);
            time = 0;
        }
    }

    public void SendMessage(GameObject obj, string message)
    {
        if (message == "OutOfRange")
        {
            myUnit.ChangeStateMachine(new UnitIdle());
        }
    }
}