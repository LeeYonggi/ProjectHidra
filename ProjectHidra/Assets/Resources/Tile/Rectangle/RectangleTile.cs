using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleTile : MonoBehaviour
{
    bool isWall = false;

    public bool IsWall { get => isWall; set => isWall = value; }

    // Start is called before the first frame update
    void Start()
    {
        isWall = IsTileWall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    bool IsTileWall()
    {
        Ray2D ray2D = new Ray2D(transform.position, Vector2.zero);
        RaycastHit2D[] hit = Physics2D.RaycastAll(ray2D.origin, ray2D.direction);

        int length = hit.Length;
        for (int i = 0; i < length; i++)
        {
            if (hit[i].collider.tag == "HexagonTile")
            {
                return false;
            }
        }
        return true;
    }
}
