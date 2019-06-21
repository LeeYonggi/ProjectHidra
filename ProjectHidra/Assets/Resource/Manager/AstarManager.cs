using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarManager : MonoBehaviour
{
    private static AstarManager instance = null;

    public static AstarManager Instance { get => instance; set => instance = value; }

    public FieldManager field = null;

    private List<List<GameObject>> tileList = null;
    private List<List<TileNode>> astarTile = new List<List<TileNode>>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        tileList = field.TileList;
        for (int i = 0; i < tileList.Count; i++)
        {
            astarTile.Add(new List<TileNode>());
            for (int j = 0; j < tileList[i].Count; j++)
                astarTile[i].Add(new TileNode());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 리스트 초기화
    void AstarInit()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
                astarTile[i][j] = null;
        }
    }

    List<Vector2> AstarPathFinder(Vector2 nowPosition, Vector2 targetPosition)
    {
        List<Vector2> pathResult = new List<Vector2>();

        GameObject nowObject = GetTargetToTile(nowPosition);
        GameObject targetObject = GetTargetToTile(targetPosition);
        
        Vector2Int nowIndex = Vector2Int.zero;
        Vector2Int targetIndex = Vector2Int.zero;

        if (nowObject == null || targetObject == null)
            return null;

        for(int i = 0; i < tileList.Count; i++)
        {
            for(int j = 0; j < tileList.Count; j++)
            {
                if (tileList[i][j] == nowObject)
                {
                    nowIndex = new Vector2Int(i, j);
                }
                if (tileList[i][j] == targetObject)
                {
                    targetIndex = new Vector2Int(i, j);
                }
            }
        }
        

        return pathResult;
    }

    // 그 위치의 타일을 반환
    GameObject GetTargetToTile(Vector2 nowPosition)
    {
        Ray2D ray = MouseManager.Instance.GetMouseRay2D(nowPosition);
        RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction);

        GameObject nowObject = null;

        for (int i = 0; i < hit.Length; i++)
        {
            var hitobj = hit[i].collider.gameObject;
            if (hitobj.CompareTag("Tile"))
            {
                nowObject = hitobj;
            }
        }
        return nowObject;
    }
    // 타일 노드 생성
    TileNode TileNodeInit(Vector2Int pivotPos, Vector2Int tilePos, Vector2Int targetPos)
    {
        TileNode node = new TileNode();

        node.tilePos = tilePos;

        Vector2Int virtualPos = tilePos;
        
        node.h = GetDistanceToNode(tilePos, targetPos);
        node.g = GetDistanceToNode(pivotPos, tilePos);

        return node;
    }

    // 노드간의 거리를 구하고, 맨하탄 방법으로 거리측정
    int GetDistanceToNode(Vector2Int tilePos, Vector2Int targetPos)
    {
        int h = 0;
        while (tilePos != targetPos)
        {
            int x = targetPos.x - tilePos.x;
            int y = targetPos.y - tilePos.y;

            if (x > 0) x = 1;
            if (x < 0) x = -1;
            if (y > 0) y = 1;
            if (y < 0) y = -1;

            if (x != 0)
            {
                tilePos.x += x;
                h += 1;
            }
            if (y != 0)
            {
                tilePos.y += y;
                h += 1;
            }
        }
        return h;
    }

    // 인접 타일 생성
    void CreateAdjacentTile(Vector2Int pivot, Vector2Int target)
    {
        for(int i = pivot.x - 1; i < pivot.x + 2; i++)
        {
            for (int j = pivot.y - 1; j < pivot.y + 2; j++)
            {
                if(astarTile[i][j] != null)
                    astarTile[i][j] = TileNodeInit(pivot, new Vector2Int(i, j), target);
            }
        }
    }

    // a스타 재귀함수
    void ResursiveAstar(Vector2Int pivot, Vector2Int target)
    {
        CreateAdjacentTile(pivot, target);
    }
} 


public class TileNode
{
    public Vector2Int tilePos;
    public int h;
    public int g;
}