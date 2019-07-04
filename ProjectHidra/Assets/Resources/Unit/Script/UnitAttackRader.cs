using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackRader : MonoBehaviour
{
    [SerializeField]
    private Unit unit = null;

    private GameObject targetObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unit.IsAttackPossible(collision.gameObject))
        {
            unit.ChangeStateMachine(new UnitAttackMachine(unit, collision.gameObject));
            targetObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == targetObject)
        {
            unit.ChangeStateMachine(new UnitIdle());
        }
    }
}
