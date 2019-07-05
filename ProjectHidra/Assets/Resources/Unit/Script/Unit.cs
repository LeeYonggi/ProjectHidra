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
    public UnitStateMachine UnitStateMachine { get => unitStateMachine; }
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
        if (unitStateMachine.ToString() == "UnitDie" || !weapon)
            return;
        unitStateMachine = machine;
    }

    // 유닛 죽음
    public bool IsUnitDie() => (status.Hp < 1);

    private void UnitDie()
    {
        ChangeStateMachine(new UnitDie(this));
        Destroy(weapon);
        weapon = null;
        WeaponAnimator = null;
    }

    public abstract bool IsAttackPossible(GameObject target);
}

