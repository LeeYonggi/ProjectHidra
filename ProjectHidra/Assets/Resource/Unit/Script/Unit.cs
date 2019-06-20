using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Unit : MonoBehaviour
{
    // 유닛 애니메이션
    public enum ANIMATION_STATE
    {
        IDLE,
        MOVE,
        ATTACK,
        DIE
    }
    // 유닛 종류
    public enum UNIT_KIND
    {
        FLY_UNIT,
        GROUND_UNIT
    }

    // 레이더
    [SerializeField]
    protected UnitRader unitRader = null;

    // 무기
    [SerializeField]
    private GameObject weapon = null;
    private SpriteRenderer weaponSprite = null;

    // 상태머신
    private UnitStateMachine unitStateMachine = new UnitIdle();
    private UnitAttackState unitAttack = null;

    // 애니메이터
    private Animator animator = null;

    // 이동관련
    [SerializeField]
    private float speed = 1.0f;
    private Vector2 moveVector = Vector2.zero;

    // 스텟 관련
    [SerializeField]
    private float shootSpeed = 1.0f;
    
    #region Property
    public UnitAttackState UnitAttack { get => unitAttack; set => unitAttack = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public float Speed { get => speed; set => speed = value; }
    public Vector2 MoveVector { get => moveVector; set => moveVector = value; }
    public float ShootSpeed { get => shootSpeed; set => shootSpeed = value; }
    public GameObject Weapon { get => weapon; set => weapon = value; }
    public SpriteRenderer WeaponSprite { get => weaponSprite; set => weaponSprite = value; }
    #endregion

    public Unit(UnitAttackState state)
    {
        UnitAttack = state;
    }
    // Start is called before the first frame update
    protected void Init()
    {
        Animator = GetComponent<Animator>();
        weaponSprite = weapon.GetComponent<SpriteRenderer>();
    }
    
    // Update is called once per frame
    protected void UnitUpdate()
    {
        unitStateMachine.Update(this);

        ChangeFlip(moveVector);
    }

    public void ChangeFlip(Vector2 directionVector)
    {
        if (directionVector.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (directionVector.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void ChangeStateMachine(UnitStateMachine machine)
    {
        unitStateMachine = machine;
    }

    public abstract void AttackStart(GameObject target);
}

public interface UnitStateMachine
{
    void Update(Unit unit);
    void SendMessage(string message);
}

public class UnitIdle : UnitStateMachine
{
    public void Update(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.IDLE);

        GameObject target = FindShortDistanceBuilding(unit);
        if(target)
            unit.ChangeStateMachine(new UnitMove(target));
    }

    public GameObject FindShortDistanceBuilding(Unit unit)
    {
        GameObject[] building = GameObject.FindGameObjectsWithTag("Building");

        float shortDistance = -1;
        GameObject target = null;

        for(int i = 0; i < building.Length; i++)
        {
            float distance = Vector2.Distance(building[i].transform.position, unit.transform.position);

            if (distance < shortDistance || shortDistance == -1)
            {
                shortDistance = distance;
                target = building[i];
            }
        }
        return target;
    }

    public void SendMessage(string message)
    {

    }
}

public class UnitMove : UnitStateMachine
{
    private GameObject target = null;

    public UnitMove(GameObject _target)
    {
        target = _target;
    }

    public void Update(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.MOVE);
        unit.Animator.SetFloat("MoveSpeed", unit.Speed);
        unit.transform.position = Vector2.MoveTowards(unit.transform.position, target.transform.position, unit.Speed * Time.deltaTime);
    }

    public void SendMessage(string message)
    {

    }
}

public class UnitAttackMachine : UnitStateMachine
{
    private GameObject target;
    Unit myUnit = null;
    public UnitAttackMachine(GameObject _target)
    {
        target = _target;
    }

    public void Update(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.ATTACK);
        unit.Animator.SetFloat("AttackSpeed", unit.ShootSpeed);

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
        if (unit.UnitAttack != null)
            unit.UnitAttack.Attack(target);

        myUnit = unit;
    }

    public void SendMessage(string message)
    {
        if(message == "OutOfRange")
        {

        }
    }
}