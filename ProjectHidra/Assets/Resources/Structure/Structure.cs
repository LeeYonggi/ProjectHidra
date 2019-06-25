using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private GameObject unitPrefab;
    private float time = 0.0f;

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
            GameObject obj = Instantiate(unitPrefab);
            obj.transform.position = transform.position;
            Unit unit = obj.GetComponent<Unit>();
            unit.ChangeStateMachine(new UnitMove(new Vector2(0, -1), unit));
            time = 0;
        }
    }

    public void AddUnitPrefab(string unitName)
    {
        unitPrefab = Resources.Load("Unit/Prefab/" + unitName) as GameObject;
        Debug.Log(unitPrefab);
    }
}
