using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hexagonTilePrefab;
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private Vector2 tileSize;
    Vector2 tileSpriteSize = new Vector2(0, 0);
    

    public List<List<GameObject>> TileList { get; set; } = new List<List<GameObject>>();
    public List<List<GameObject>> HexagonTileList { get; set; } = new List<List<GameObject>>();


    // Start is called before the first frame update
    void Awake()
    {
        tileSpriteSize = hexagonTilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        tileSpriteSize.x *= 0.7f;

        PlaceHexagonTiles(transform.position, tileSize, tileSpriteSize);

        Destroy(HexagonTileList[1][3]);
        Destroy(HexagonTileList[3][2]);
    }

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 헥사곤 타일을 배치합니다.
    /// </summary>
    /// <param name="pivot">기준점</param>
    /// <param name="size">크기</param>
    /// <param name="spriteSize">스프라이트 크기</param>
    void PlaceHexagonTiles(Vector3 pivot, Vector2 size, Vector2 spriteSize)
    {
        for (int x = 0; x < size.x; x++)
        {
            int sizeY = (int)(size.y) - Mathf.Abs((int)(size.y * 0.5f) - x);
            HexagonTileList.Add(new List<GameObject>());

            int emptyNum = (int)size.y - sizeY;
            int empty = emptyNum;
            for (empty = emptyNum; empty > emptyNum * 0.5f; empty--)
            {
                HexagonTileList[x].Add(null);
            }
            for (int y = 0; y < sizeY; y++)
            {
                GameObject obj = Instantiate(hexagonTilePrefab, transform);
                float firstPosition = 0 - (size.x * spriteSize.x * 0.4f);

                float width  = spriteSize.x * x;
                float height = spriteSize.y * y;
                
                height = height - spriteSize.y * sizeY * 0.5f;

                obj.transform.position = new Vector3(firstPosition + width,
                    spriteSize.y * 0.5f + height, transform.position.z);

                HexagonTileList[x].Add(obj);
            }
            for (empty = emptyNum; empty > emptyNum * 0.5f; empty--)
            {
                HexagonTileList[x].Add(null);
            }
        }
    }

    /// <summary>
    /// 일반 타일을 배치합니다.
    /// </summary>
    /// <param name="pivot">기준점</param>
    /// <param name="size">헥사곤 크기</param>
    /// <param name="hexagonSpriteSize">헥사곤 스프라이트 크기</param>
    void PlaceTiles(Vector3 pivot, Vector2 size, Vector2 hexagonSpriteSize)
    {
        Vector2 firstPosition = Vector2.zero - (size * hexagonSpriteSize * 0.5f);
        Vector2 lastPosition = Vector2.zero + (size * hexagonSpriteSize * 0.5f);
        Vector2 spriteSize = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;

        int indexX = 0;
        for (float x = firstPosition.x; x <= lastPosition.x; x += spriteSize.x, indexX += 1)
        {
            TileList.Add(new List<GameObject>());

            int indexY = 0;

            for(float y = firstPosition.y; y <= lastPosition.y; y += spriteSize.y, indexY += 1)
            {
                GameObject tile = Instantiate(tilePrefab, transform);

                Vector3 tilePosition = new Vector3(x, y);

                tile.transform.position = tilePosition;

                TileList[indexX].Add(tile);
            }
        }
    }
    public void PlaceTiles()
    {
        PlaceTiles(transform.position, tileSize, tileSpriteSize);
    }
}
