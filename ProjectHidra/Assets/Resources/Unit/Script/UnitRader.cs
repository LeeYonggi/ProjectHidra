using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRader : MonoBehaviour
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(unit.IsAttackPossible(collision.gameObject))
            unit.UnitStateMachine.SendMessage(collision.gameObject, "AddTarget");
    }
}

