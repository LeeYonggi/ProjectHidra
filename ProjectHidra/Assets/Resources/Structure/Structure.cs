using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Structure : MonoBehaviour
{
    private GameObject unitPrefab;
    private float time = 0.0f;
    private ObjectStatus status;

    #region Property
    public ObjectStatus Status { get => status; set => status = value; }
    #endregion

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<ObjectStatus>();
        GameManager.Instance.turnStartEvent.AddListener(CreateUnit);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddUnitPrefab(string unitName)
    {
        unitPrefab = Resources.Load("Unit/Prefab/" + unitName) as GameObject;
        Debug.Log(unitPrefab);
    }

    private void CreateUnit(GameObject _unitPrefab)
    {
        GameObject obj = Instantiate(_unitPrefab);
        Unit unit = obj.GetComponent<Unit>();

        Vector2 objPosition = transform.position;
        obj.transform.position = objPosition + new Vector2(0, -0.3f);

        Vector2 moveVector = transform.position;
        moveVector += new Vector2(0, -1);

        unit.ChangeStateMachine(new UnitMove(moveVector, unit));
        unit.Structure = this;
    }


    public void CreateUnit()
    {
        ObjectStatus status = unitPrefab.GetComponent<ObjectStatus>();

        if (GameManager.Instance.ResourceStatus.Mineral >= status.basicCost)
        {
            CreateUnit(unitPrefab);
            GameManager.Instance.ResourceStatus.Mineral -= status.basicCost;
        }
    }

}
