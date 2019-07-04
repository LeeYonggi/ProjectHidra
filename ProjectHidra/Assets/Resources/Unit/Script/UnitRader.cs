﻿using System.Collections;
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

    // 힙으로 저장해야함
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unit.IsAttackPossible(collision.gameObject) && targetObject == null)
        {
            unit.ChangeStateMachine(new UnitMove(collision.transform.position));
            targetObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targetObject == collision.gameObject)
        {
            unit.ChangeStateMachine(new UnitIdle());
            targetObject = null;
        }
    }
}

