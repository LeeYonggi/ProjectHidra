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
    private Animator weaponAnimator = null;
    private SpriteRenderer weaponSprite = null;

    // 상태머신
    private UnitStateMachine unitStateMachine = new UnitIdle();
    private UnitAttackState unitAttack = null;

    // 애니메이터
    private Animator animator = null;

    // 스프라이트
    protected SpriteRenderer spRenderer = null;

    // 이동관련
    [SerializeField]
    private float speed = 1.0f;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb2d = null;

    // 스텟 관련
    [SerializeField]
    private float shootSpeed = 1.0f;
    
    private ObjectStatus status;
    
    // 태어난 건물
    private Structure structure;

    #region Property
    public UnitAttackState UnitAttack { get => unitAttack; set => unitAttack = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public float Speed { get => speed; set => speed = value; }
    public Vector2 MoveVector { get => moveVector; set => moveVector = value; }
    public float ShootSpeed { get => shootSpeed; set => shootSpeed = value; }
    public GameObject Weapon { get => weapon; set => weapon = value; }
    public SpriteRenderer WeaponSprite { get => weaponSprite; set => weaponSprite = value; }
    public ObjectStatus Status { get => status; set => status = value; }
    public Rigidbody2D Rb2d { get => rb2d; set => rb2d = value; }
    public Structure Structure { get => structure; set => structure = value; }
    public Animator WeaponAnimator { get => weaponAnimator; set => weaponAnimator = value; }
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

        Rb2d = GetComponent<Rigidbody2D>();

        spRenderer = GetComponent<SpriteRenderer>();

        WeaponAnimator = weapon.GetComponent<Animator>();

        status = GetComponent<ObjectStatus>();

        if (structure)
            status.teamKind = structure.Status.teamKind;
    }
    
    // Update is called once per frame
    protected void UnitUpdate()
    {
        unitStateMachine.Update(this);

        ChangeFlip(moveVector);

        if(IsUnitDie())
            UnitDie();
    }

    // 좌우 변환
    public void ChangeFlip(Vector2 directionVector)
    {
        if (directionVector.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (directionVector.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // 상태머신 상태 변경
    public void ChangeStateMachine(UnitStateMachine machine)
    {
        unitStateMachine = machine;
    }

    // 유닛 죽음
    public bool IsUnitDie() => (status.Hp < 1);

    private void UnitDie()
    {
        ChangeStateMachine(new UnitDie(this));
        Destroy(weapon);
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
        unit.WeaponAnimator.SetBool("IsAttack", false);

        GameObject target = FindShortDistanceBuilding(unit);
        if (target)
        {
            UnitMove unitMove = new UnitMove();
            unitMove.UnitAstarMove(target.transform.position, unit);
            unit.ChangeStateMachine(unitMove);
        }
    }

    public GameObject FindShortDistanceBuilding(Unit unit)
    {
        GameObject[] building = GameObject.FindGameObjectsWithTag("Building");

        float shortDistance = -1;
        GameObject target = null;

        for(int i = 0; i < building.Length; i++)
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

    public void SendMessage(string message)
    {

    }
}

public class UnitMove : UnitStateMachine
{
    private List<Vector2> path = new List<Vector2>();
    private int nowPath = 0;

    public UnitMove(){  }

    public UnitMove(Vector2 _target)
    {
        path.Add(_target);
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
        if(Vector2.Distance(unit.transform.position, target) > distance)
        {
            Vector2 positionVec2 = new Vector2(unit.transform.position.x, unit.transform.position.y);

            unit.MoveVector = target - positionVec2;
            unit.MoveVector = unit.MoveVector.normalized;

            unit.transform.position = Vector2.MoveTowards(unit.transform.position, target, speed);

            return false;
        }
        return true;
    }

    public void SendMessage(string message)
    {

    }
}

public class UnitAttackMachine : UnitStateMachine
{
    private GameObject target;
    Unit myUnit = null;

    public UnitAttackMachine(Unit unit, GameObject _target)
    {
        target = _target;
        myUnit = unit;
        unit.WeaponAnimator.SetBool("IsAttack", true);

        if (unit.UnitAttack != null)
        {
            unit.StartCoroutine(AttackCoroutine());
        }
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

    }

    public IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.2f / myUnit.ShootSpeed);
        while (target)
        {
            myUnit.UnitAttack.Attack(target, myUnit.Status.Attack);
            yield return new WaitForSeconds(0.2f / myUnit.ShootSpeed);
        }
    }

    public void SendMessage(string message)
    {
        if(message == "OutOfRange")
        {

        }
    }
}

public class UnitDie : UnitStateMachine
{
    public UnitDie(Unit unit)
    {
        unit.Animator.SetInteger("AnimationState", (int)Unit.ANIMATION_STATE.DIE);
    }
    public void Update(Unit unit)
    {
        if(unit.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && 
            !unit.Animator.IsInTransition(0))
        {
            Object.Destroy(unit.gameObject);
            unit.ChangeStateMachine(null);
        }
    }

    public void SendMessage(string message)
    {

    }
}