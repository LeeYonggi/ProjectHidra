using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Status = new ObjectStatus(ObjectStatus.TEAM_KIND.TEAM_BLUE,
            100, 5, 20);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > 5)
        {
            CreateUnit(unitPrefab);
            time = 0;
        }
    }

    public void AddUnitPrefab(string unitName)
    {
        unitPrefab = Resources.Load("Unit/Prefab/" + unitName) as GameObject;
        Debug.Log(unitPrefab);
    }

    public void CreateUnit(GameObject _unitPrefab)
    {
        GameObject obj = Instantiate(_unitPrefab);
        Unit unit = obj.GetComponent<Unit>();

        Vector2 objPosition = transform.position;
        obj.transform.position = objPosition + new Vector2(0, -0.3f);

        Vector2 moveVector = transform.position;
        moveVector += new Vector2(0, -1);

        unit.ChangeStateMachine(new UnitMove(moveVector));
        unit.Status.ChangeTeam(status.teamKind);
    }
}
