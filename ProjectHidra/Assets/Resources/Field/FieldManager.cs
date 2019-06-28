using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private Vector2 tileSize;

    // 섹스
    private Vector2 tileSpriteSize = new Vector2(0, 0);

    public List<List<GameObject>> TileList { get; set; } = new List<List<GameObject>>();
    public List<List<GameObject>> HexagonTileList { get; set; } = new List<List<GameObject>>();


    // Start is called before the first frame update
    void Awake()
    {
        tileSpriteSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        tileSpriteSize.x *= 0.7f;
        PlaceTiles(transform.position, tileSize, tileSpriteSize);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 타일을 배치합니다.
    /// </summary>
    /// <param name="pivot">기준점</param>
    /// <param name="size">크기</param>
    /// <param name="spriteSize">스프라이트 크기</param>
    void PlaceTiles(Vector3 pivot, Vector2 size, Vector2 spriteSize)
    {
        for (int x = 0; x < size.x; x++)
        {
            int sizeY = (int)(size.y) - Mathf.Abs((int)(size.y * 0.5f) - x);
            TileList.Add(new List<GameObject>());

            int emptyNum = (int)size.y - sizeY;
            int empty = emptyNum;
            for (empty = emptyNum; empty > emptyNum * 0.5f; empty--)
            {
                TileList[x].Add(null);
            }
            for (int y = 0; y < sizeY; y++)
            {
                GameObject obj = Instantiate(tilePrefab, transform);
                float firstPosition = 0 - (size.x * spriteSize.x * 0.4f);

                float width  = spriteSize.x * x;
                float height = spriteSize.y * y;
                
                height = height - spriteSize.y * sizeY * 0.5f;

                obj.transform.position = new Vector3(firstPosition + width,
                    spriteSize.y * 0.5f + height, transform.position.z);

                TileList[x].Add(obj);
            }
            for (empty = emptyNum; empty > emptyNum * 0.5f; empty--)
            {
                TileList[x].Add(null);
            }
        }
    }
}
