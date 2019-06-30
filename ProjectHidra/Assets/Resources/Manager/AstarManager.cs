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
    private List<TileNode> openTiles = new List<TileNode>();
    private List<TileNode> closeTiles = new List<TileNode>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        field.PlaceTiles();
        tileList = field.TileList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// A스타 리스트 초기화
    /// </summary>
    /// <param name="nowObject">현재 오브젝트</param>
    /// <param name="targetObject">타겟 오브젝트</param>
    /// <param name="nowIndex">현재 오브젝트 인덱스</param>
    /// <param name="targetIndex">타겟 오브젝트 인덱스</param>
    void AstarInit(GameObject nowObject, GameObject targetObject, ref Vector2Int nowIndex, ref Vector2Int targetIndex)
    {
        astarTile.Clear();
        for (int i = 0; i < tileList.Count; i++)
        {
            astarTile.Add(new List<TileNode>());
            for (int j = 0; j < tileList[i].Count; j++)
            {
                if (tileList[i][j] == null || tileList[i][j].GetComponent<RectangleTile>().IsWall == true)
                {
                    TileNode tileNode = new TileNode();
                    tileNode.tilePos = new Vector2Int(i, j);
                    tileNode.isWall = true;
                    astarTile[i].Add(tileNode);
                }
                else
                    astarTile[i].Add(null);

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
        closeTiles.Clear();
        openTiles.Clear();
    }

    public List<Vector2> AstarPathFinder(Vector2 nowPosition, Vector2 targetPosition)
    {
        List<Vector2> pathResult = new List<Vector2>();

        GameObject nowObject = GetTargetToTile(nowPosition);
        GameObject targetObject = GetTargetToTile(targetPosition);
        
        Vector2Int nowIndex = Vector2Int.zero;
        Vector2Int targetIndex = Vector2Int.zero;

        if (nowObject == null || targetObject == null)
            return null;
        
        AstarInit(nowObject, targetObject, ref nowIndex, ref targetIndex);

        // 시작 노드 생성
        TileNode firstNode = TileNodeInit(nowIndex, nowIndex, targetIndex, null);
        astarTile[nowIndex.x][nowIndex.y] = firstNode;
        closeTiles.Add(firstNode);
        firstNode.isOpen = false;

        TileNode result;
        // 나머지 6개의 인접 노드 생성
        if (CreateAdjacentTile(firstNode.tilePos, targetIndex))
        {
            result = TileNodeInit(targetIndex, targetIndex, targetIndex, firstNode);
        }
        else
        {
            result = ResursiveAstar(null, targetIndex);
        }
        //재귀함수 호출

        if (result == null)
            return pathResult;
        while(true)
        {
            pathResult.Add(tileList[result.tilePos.x][result.tilePos.y].transform.position);
            result = result.parent;

            if (result == null)
                break;
        }

        pathResult.RemoveAt(pathResult.Count - 1);
        return pathResult;
    }

    // 그 위치의 타일을 반환
    GameObject GetTargetToTile(Vector2 nowPosition)
    {
        Ray2D ray = new Ray2D(nowPosition, Vector2.zero);
        RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction);

        GameObject nowObject = null;

        for (int i = 0; i < hit.Length; i++)
        {
            var hitobj = hit[i].collider.gameObject;
            if (hitobj.CompareTag("RectangleTile"))
            {
                nowObject = hitobj;
            }
        }
        return nowObject;
    }
    // 타일 노드 생성
    TileNode TileNodeInit(Vector2Int pivotPos, Vector2Int tilePos, Vector2Int targetPos, TileNode parent)
    {
        TileNode node = new TileNode();

        node.tilePos = tilePos;
        
        node.h = GetDistanceToNode(tilePos, targetPos);
        node.g = GetDistanceToNode(pivotPos, tilePos);
        if (parent != null)
            node.g += parent.g;
        node.parent = parent;

        return node;
    }

    // 노드간의 거리를 구하고, 맨하탄 방법으로 거리측정
    int GetDistanceToNode(Vector2Int tilePos, Vector2Int targetPos)
    {
        int h = 0;
        int x = targetPos.x - tilePos.x;
        int y = targetPos.y - tilePos.y;
        
        if (x != 0)
        {
            h += Mathf.Abs(x);
        }
        if (y != 0)
        {
            h += Mathf.Abs(y);
        }

        return h;
    }

    // 인접 타일 생성, 인접 배열에 목적지가 있는 경우 True를 반환
    bool CreateAdjacentTile(Vector2Int pivot, Vector2Int target)
    {
        for(int i = pivot.x - 1; i <= pivot.x + 1; i++)
        {
            if (i < 0 || i >= astarTile.Count)
                break;
            for (int j = pivot.y - 1; j <= pivot.y + 1; j++)
            {
                if (j < 0 || j >= astarTile[i].Count)
                    break;
                // 인접한 배열에 목적지가 있을경우
                if (new Vector2Int(i, j) == target)
                    return true;
                if (astarTile[i][j] == null)
                {
                    TileNode tile = TileNodeInit(pivot, new Vector2Int(i, j), target, astarTile[pivot.x][pivot.y]);
                    astarTile[i][j] = tile;
                    openTiles.Add(tile);
                }
            }
        }
        return false;
    }

    int TileCompare(TileNode tile1, TileNode tile2)
    {
        int a = tile1.h + tile1.g;
        int b = tile2.h + tile2.g;
        if (a == b)
            return tile1.h.CompareTo(tile2.h);
        return a.CompareTo(b);
    }

    // a스타 재귀함수
    TileNode ResursiveAstar(TileNode nowNode, Vector2Int target)
    {
        TileNode tile = nowNode;
        if (openTiles.Count == 0)
            return null;
        if (tile == null)
        {
            openTiles.Sort(TileCompare);
            tile = openTiles[0];
        }
        if (tile.h == 0)
            return tile;
        openTiles.Remove(tile);
        closeTiles.Add(tile);
        tile.isOpen = false;

        TileNode nextTile = FindAdjacentTile(tile, target);
        return ResursiveAstar(nextTile, target);
    }

    // 인접 타일 생성 & 탐색
    TileNode FindAdjacentTile(TileNode pivot, Vector2Int target)
    {
        TileNode nextNode = null;
        for (int i = pivot.tilePos.x - 1; i <= pivot.tilePos.x + 1; i++)
        {
            if (i < 0 || i >= astarTile.Count)
                break;
            for (int j = pivot.tilePos.y - 1; j <= pivot.tilePos.y + 1; j++)
            {
                if (j < 0 || j >= astarTile[i].Count)
                    break;
                if (astarTile[i][j] == null)
                {
                    TileNode tile = TileNodeInit(pivot.tilePos, new Vector2Int(i, j), target, pivot);
                    astarTile[i][j] = tile;
                    openTiles.Add(tile);
                    if (nextNode != null && tile.h < nextNode.h)
                        nextNode = tile;
                    else if (tile.h < pivot.h)
                        nextNode = tile;
                }
                else if (astarTile[i][j].isWall == true || astarTile[i][j].isOpen == false)
                    continue;
                else if(pivot.parent.g > astarTile[i][j].g)
                {
                    // g 비용이 더 작으면 부모 변경
                    pivot.parent = astarTile[i][j];
                    // g 비용 변경
                    pivot.g = GetDistanceToNode(pivot.tilePos, astarTile[i][j].tilePos);
                    pivot.g += pivot.parent.g;
                }
            }
        }
        return nextNode;
    }
    
} 


public class TileNode
{
    public Vector2Int tilePos;
    public TileNode parent = null;
    public bool isWall = false;
    public bool isOpen = true;
    public int h = 0;
    public int g = 0;
}